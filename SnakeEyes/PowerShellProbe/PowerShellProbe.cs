using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SnakeEyes
{
    [XmlRoot("TraceEvent")]
    public class PowerShellTraceEvent
    {
        public string Source { get; set; }
        public string MachineName { get; set; }
        public string Timestamp { get; set; }
        public int EventId { get; set; }
        public TraceEventType EventType { get; set; }
        public string Value { get; set; }
        public string MaxValue { get; set; }
        public string MinValue { get; set; }
        public string ScriptFile { get; set; }
        public string ScriptParameters { get; set; }
        public string Message { get; set; }
    };

    public class PowerShellProbe : IProbe
    {
        TraceSource _traceSource;
        Runspace _runspace;
        Command _runcmd;

        [ConfigurationProperty("ProbeFrequency", DefaultValue = 1)]
        [Description("Number of seconds between each probe check")]
        public TimeSpan ProbeFrequency { get; set; }
        [ConfigurationProperty("EventId")]
        [Description("Trace EventId when probe triggers")]
        public int EventId { get; set; }
        [ConfigurationProperty("EventType", DefaultValue = TraceEventType.Error)]
        [Description("Trace EventType when probe triggers")]
        public TraceEventType EventType { get; set; }

        [ConfigurationProperty("ScriptFile")]
        [Description("File path to Powershell script")]
        public string ScriptFile { get; set; }
        [ConfigurationProperty("ScriptParameters")]
        [Description("Parameters to the Powershell script")]
        public string ScriptParameters { get; set; }
        [ConfigurationProperty("MaxValue")]
        [Description("Trigger probe when Powershell script output is larger than value")]
        public float? MaxValue { get; set; }
        [Description("Trigger probe when Powershell script output is less than value")]
        public float? MinValue { get; set; }

        public PowerShellProbe()
        {
            ProbeFrequency = TimeSpan.FromSeconds(5);
        }

        public void Dispose()
        {
            if (_runspace != null)
            {
                _runspace.Dispose();
                _runspace = null;
            }
        }

        public TraceSource ConfigureProbe(string configName)
        {
            _traceSource = new TraceSource(configName);
            StringDictionary attributes = _traceSource.Attributes;  // Initialize TraceSource
            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(configName);
            if (config != null)
            {
                if (config["MaxValue"] != null)
                    MaxValue = float.Parse(config["MaxValue"]);
                if (config["MinValue"] != null)
                    MinValue = float.Parse(config["MinValue"]);
                if (config["ProbeFrequency"] != null)
                    ProbeFrequency = TimeSpan.FromSeconds(Int32.Parse(config["ProbeFrequency"]));
                if (config["EventId"] != null)
                    EventId = Int32.Parse(config["EventId"]);
                if (config["EventType"] != null)
                    EventType = (TraceEventType)Enum.Parse(typeof(TraceEventType), config["EventType"]);
                ScriptFile = config["ScriptFile"];
                ScriptParameters = config["ScriptParameters"];
            }
            StartProbe(false);
            return _traceSource;
        }

        static string[] SplitArguments(String argumentString)
        {
            StringBuilder translatedArguments = new StringBuilder(argumentString).Replace("\\\"", "\r");
            bool InsideQuote = false;
            for (int i = 0; i < translatedArguments.Length; i++)
            {
                if (translatedArguments[i] == '"')
                {
                    InsideQuote = !InsideQuote;
                }
                if (translatedArguments[i] == ' ' && !InsideQuote)
                {
                    translatedArguments[i] = '\n';
                }
            }
            string[] toReturn = translatedArguments.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < toReturn.Length; i++)
                toReturn[i] = toReturn[i].Replace("\r", "\"");
            return toReturn;
        }

        static string RemoveMatchingQuotes(string stringToTrim)
        {
            int firstQuoteIndex = stringToTrim.IndexOf('"');
            int lastQuoteIndex = stringToTrim.LastIndexOf('"');
            while (firstQuoteIndex != lastQuoteIndex)
            {
                stringToTrim = stringToTrim.Remove(firstQuoteIndex, 1);
                stringToTrim = stringToTrim.Remove(lastQuoteIndex - 1, 1); //-1 because we've shifted the indicies left by one
                firstQuoteIndex = stringToTrim.IndexOf('"');
                lastQuoteIndex = stringToTrim.LastIndexOf('"');
            }
            return stringToTrim;
        }

        public TimeSpan ExecuteProbe()
        {
            if (_runspace==null && !StartProbe(true))
                return ProbeFrequency;

            try
            {
                StringBuilder stringResult = new StringBuilder();
                using (Pipeline pipeline = _runspace.CreatePipeline())
                {
                    pipeline.Commands.Add(_runcmd);

                    Collection<PSObject> result = pipeline.Invoke();
                    foreach (PSObject psobj in result)
                        stringResult.AppendLine(psobj.ToString());
                }
                TraceEvent(TraceEventType.Information, stringResult.ToString().Trim(), "Ok");
            }
            catch (Exception ex)
            {
                StartProbe(true);
                TraceEvent(TraceEventType.Critical, "", ex.Message);
            }
            return ProbeFrequency;
        }

        bool StartProbe(bool writeEvent)
        {
            try
            {
                if (_runspace != null)
                {
                    _runspace.Dispose();
                    _runspace = null;
                }
                _runspace = RunspaceFactory.CreateRunspace();
                _runspace.Open();

                _runcmd = new Command(ScriptFile);
                if (ScriptParameters != null)
                {
                    string[] parameters = SplitArguments(ScriptParameters);
                    foreach (string param in parameters)
                    {
                        int splitParam = param.IndexOf('=');
                        if (splitParam != -1)
                        {
                            string name = param.Substring(0, splitParam);
                            string value = param.Substring(splitParam + 1);
                            value = RemoveMatchingQuotes(value);
                            _runcmd.Parameters.Add(name, value);
                        }
                        else
                            _runcmd.Parameters.Add(param);
                    }
                }
            }
            catch (Exception ex)
            {
                if (writeEvent)
                    TraceEvent(TraceEventType.Critical, "", ex.Message);
                if (_runspace != null)
                {
                    _runspace.Dispose();
                    _runspace = null;
                }
                return false;
            }
            return true;
        }

        void TraceEvent(TraceEventType eventType, string value, string message)
        {
            if (eventType == TraceEventType.Information)
            {
                if (MaxValue.HasValue && float.Parse(value) > MaxValue)
                {
                    message = "Value is above maximum threshold";
                    eventType = EventType;
                }
                else
                if (MinValue.HasValue && float.Parse(value) < MinValue)
                {
                    message = "Value is below minimum threshold";
                    eventType = EventType;
                }
            }

            PowerShellTraceEvent traceEvent = new PowerShellTraceEvent();
            traceEvent.Source = _traceSource.Name;
            traceEvent.MachineName = Environment.MachineName;
            traceEvent.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            traceEvent.ScriptFile = ScriptFile;
            traceEvent.ScriptParameters = ScriptParameters;
            traceEvent.Value = value;
            traceEvent.MaxValue = MaxValue.HasValue ? MaxValue.ToString() : null;
            traceEvent.MinValue = MinValue.HasValue ? MinValue.ToString() : null;
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
                XmlSerializer serializer = new XmlSerializer(typeof(PowerShellTraceEvent));
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
    }
}
