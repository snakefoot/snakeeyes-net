using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SnakeEyes
{
    [XmlRoot("TraceEvent")]
    public class FileTraceEvent
    {
        public string Source { get; set; }
        public string MachineName { get; set; }
        public string Timestamp { get; set; }
        public int EventId { get; set; }
        public TraceEventType EventType { get; set; }
        public string FileName { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public string Message { get; set; }
    };


    public class FileProbe : IProbe
    {
        TraceSource _traceSource;

        public string FileName { get; set; }
        public TimeSpan ProbeFrequency { get; set; }
        public long MaxSize { get; set; }
        public int EventId { get; set; }
        public TraceEventType EventType { get; set; }

        public FileProbe()
        {
            ProbeFrequency = TimeSpan.FromSeconds(5);
            EventType = TraceEventType.Warning;
        }

        public void Dispose()
        {
            // NOOP
        }

        public TraceSource ConfigureProbe(string configName)
        {
            _traceSource = new TraceSource(configName);
            StringDictionary attributes = _traceSource.Attributes;  // Initialize TraceSource
            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(configName);
            if (config != null)
            {
                FileName = config["FileName"];
                if (config["MaxSize"] != null)
                    MaxSize = long.Parse(config["MaxSize"]);
                if (config["ProbeFrequency"] != null)
                    ProbeFrequency = TimeSpan.FromSeconds(Int32.Parse(config["ProbeFrequency"]));
                if (config["EventId"] != null)
                    EventId = Int32.Parse(config["EventId"]);
                if (config["EventType"] != null)
                    EventType = (TraceEventType)Enum.Parse(typeof(TraceEventType), config["EventType"]);
            }
            try
            {
                StartProbe(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);
            }
            return _traceSource;
        }

        public TimeSpan ExecuteProbe()
        {
            try
            {
                long size = File.Exists(FileName) ? new FileInfo(FileName).Length : 0;
                TraceEvent(TraceEventType.Information, size, "Ok");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);
                TraceEvent(TraceEventType.Critical, 0, ex.Message); // Report initial exception
            }
            return ProbeFrequency;
        }

        void TraceEvent(TraceEventType eventType, long value, string message)
        {
            if (eventType == TraceEventType.Information)
            {
                if (value > MaxSize)
                {
                    message = "Value is above maximum threshold";
                    eventType = EventType;
                }
            }

            FileTraceEvent traceEvent = new FileTraceEvent();
            traceEvent.Source = _traceSource.Name;
            traceEvent.MachineName = Environment.MachineName;
            traceEvent.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            traceEvent.Value = value;
            traceEvent.MaxValue = MaxSize;
            traceEvent.EventId = EventId;
            traceEvent.EventType = eventType;
            traceEvent.Message = message;

            string formatMessage = "";

            using (StringWriter writer = new StringWriter())
            using (XmlTextWriter xmlwriter = new XmlTextWriter(writer))
            {
                xmlwriter.Formatting = Formatting.Indented;
                XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
                xmlnsEmpty.Add("", "");
                XmlSerializer serializer = new XmlSerializer(typeof(FileTraceEvent));
                serializer.Serialize(xmlwriter, traceEvent, xmlnsEmpty);
                formatMessage = writer.ToString();
            }

            try
            {
                _traceSource.TraceEvent(eventType, EventId, formatMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(_traceSource.Name + " " + ex.Message);
                if (ex.InnerException != null)
                    System.Diagnostics.Trace.TraceError(_traceSource.Name + " " + ex.InnerException.Message);
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " failed to trace event. Check TraceListeners:");
                foreach (TraceListener listener in _traceSource.Listeners)
                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " has listener: " + listener.Name + " (" + listener.ToString() + ")");
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.StackTrace);
            }
        }

        bool StartProbe(bool writeEvent)
        {
            // NOOP
            return true;
        }
    }
}

