using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SnakeEyes
{
    [XmlRoot("TraceEvent")]
    public class PingTraceEvent
    {
        public string Source { get; set; }
        public string MachineName { get; set; }
        public string Timestamp { get; set; }
        public int EventId { get; set; }
        public TraceEventType EventType { get; set; }
        public string Value { get; set; }
        public string HostName { get; set; }
        public string IpAddress { get; set; }
        public int TimeoutMs { get; set; }
        public int TTL { get; set; }
        public bool DontFragment { get; set; }
        public int BufferSize { get; set; }
        public string MaxValue { get; set; }
        public string Message { get; set; }
    };

    public class PingProbe : IProbe
    {
        TraceSource _traceSource;
        Ping _ping;
        PingOptions _pingOptions;
        byte[] _buffer;

        public string HostName { get; set; }
        public string IpAddress { get; set; }
        public TimeSpan Timeout { get; set; }
        public int TTL { get; set; }
        public bool DontFragment { get; set; }
        public int BufferSize { get; set; }
        public int SampleCount { get; set; }
        public TimeSpan ProbeFrequency { get; set; }
        public float? MaxValue { get; set; }
        public int EventId { get; set; }
        public TraceEventType EventType { get; set; }

        public PingProbe()
        {
            ProbeFrequency = TimeSpan.FromSeconds(5);
            EventType = TraceEventType.Warning;
        }

        public void Dispose()
        {
            if (_ping != null)
            {
                _ping.Dispose();
                _ping = null;
            }
        }

        public TraceSource ConfigureProbe(string configName)
        {
            _traceSource = new TraceSource(configName);
            StringDictionary attributes = _traceSource.Attributes;  // Initialize TraceSource
            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(configName);
            if (config != null)
            {
                HostName = config["HostName"];
                IpAddress = config["IpAddress"];
                if (config["TimeoutMs"] != null)
                    Timeout = TimeSpan.FromMilliseconds(Int32.Parse(config["TimeoutMs"]));
                else
                    Timeout = TimeSpan.FromSeconds(5);
                if (config["TTL"] != null)
                    TTL = int.Parse(config["TTL"]);
                if (config["DontFragment"] != null)
                    DontFragment = bool.Parse(config["DontFragment"]);
                if (config["BufferSize"] != null)
                    BufferSize = int.Parse(config["BufferSize"]);
                if (config["SampleCount"] != null)
                    SampleCount = int.Parse(config["SampleCount"]);
                if (config["MaxValue"] != null)
                    MaxValue = float.Parse(config["MaxValue"]);
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
            if (_ping == null)
            {
                try
                {
                    if (!StartProbe(true))
                        return ProbeFrequency;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);
                    TraceEvent(TraceEventType.Critical, float.NaN, IpAddress + "(" + HostName + ") " + ": " + ex.Message);
                    return ProbeFrequency;
                }
            }

            try
            {
                float roundTripTime = 0;
                long sampleCount = Math.Max(SampleCount, 1);
                for (int i = 0; i < sampleCount; ++i)
                {
                    PingReply pingReply = PingPong();

                    if (pingReply.Status == IPStatus.Success)
                    {
                        if (roundTripTime == float.NaN)
                            roundTripTime = (float)Timeout.TotalMilliseconds;
                        roundTripTime += pingReply.RoundtripTime;
                    }
                    else
                    {
                        TraceEvent(TraceEventType.Warning, float.NaN, pingReply.Status.ToString());
                        if (roundTripTime == 0)
                            roundTripTime = float.NaN;
                        else
                            roundTripTime += (float)Timeout.TotalMilliseconds;
                    }
                }
                if (roundTripTime != float.NaN)
                    TraceEvent(TraceEventType.Information, roundTripTime / sampleCount, "Ok");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);

                try
                {
                    StartProbe(true);
                }
                catch (Exception startEx)
                {
                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + startEx.Message);
                    TraceEvent(TraceEventType.Critical, float.NaN, ex.Message); // Report initial exception
                }
            }
            return ProbeFrequency;
        }

        PingReply PingPong()
        {
            if (_buffer != null && _pingOptions != null)
                return _ping.Send(IpAddress ?? HostName, (int)Timeout.TotalMilliseconds, _buffer, _pingOptions);
            else
                return _ping.Send(IpAddress ?? HostName, (int)Timeout.TotalMilliseconds);
        }

        void TraceEvent(TraceEventType eventType, float value, string message)
        {
            if (eventType == TraceEventType.Information)
            {
                if (MaxValue.HasValue && value > MaxValue)
                {
                    message = "Value is above maximum threshold";
                    eventType = EventType;
                }
            }

            PingTraceEvent traceEvent = new PingTraceEvent();
            traceEvent.Source = _traceSource.Name;
            traceEvent.MachineName = Environment.MachineName;
            traceEvent.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            traceEvent.Value = value.ToString("F");
            traceEvent.MaxValue = MaxValue.HasValue ? MaxValue.ToString() : null;
            traceEvent.HostName = HostName;
            traceEvent.IpAddress = IpAddress;
            traceEvent.IpAddress = IpAddress;
            traceEvent.TimeoutMs = (int)Timeout.TotalMilliseconds;
            traceEvent.TTL = TTL;
            traceEvent.DontFragment = DontFragment;
            traceEvent.BufferSize = BufferSize;
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
                XmlSerializer serializer = new XmlSerializer(typeof(PingTraceEvent));
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
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.ToString());
            }
        }

        bool StartProbe(bool writeEvent)
        {
            try
            {
                if (_ping != null)
                {
                    _ping.Dispose();
                    _ping = null;
                }

                _ping = new System.Net.NetworkInformation.Ping();
                if (TTL > 0)
                    _pingOptions = new System.Net.NetworkInformation.PingOptions(TTL, DontFragment);
                else
                    _pingOptions = null;

                _buffer = Enumerable.Repeat((byte)0x20, BufferSize).ToArray();

                PingPong();
                return true;
            }
            catch (Exception ex)
            {
                if (writeEvent)
                    TraceEvent(TraceEventType.Critical, 0, ex.Message);
                if (_ping != null)
                {
                    _ping.Dispose();
                    _ping = null;
                }
                throw;
            }
        }
    }
}
