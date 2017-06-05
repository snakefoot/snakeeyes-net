using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SnakeEyes
{
    [XmlRoot("TraceEvent")]
    public class PerfMonTraceEvent
    {
        public string Source { get; set; }
        public string MachineName { get; set; }
        public string Timestamp { get; set; }
        public int EventId { get; set; }
        public TraceEventType EventType { get; set; }
        public string Value { get; set; }
        public string CategoryName { get; set; }
        public string CounterName { get; set; }
        public string InstanceName { get; set; }
        public string MaxValue { get; set; }
        public string MinValue { get; set; }
        public string ServiceName { get; set; }
        public string Message { get; set; }
    };

    public class PerfMonProbe : IProbe
    {
        TraceSource _traceSource;
        PerformanceCounter _perfCounter;
        CounterSample? _prevSample;

        [ConfigurationProperty("ProbeFrequency", DefaultValue = 1)]
        [Description("Number of seconds between each probe check")]
        public TimeSpan ProbeFrequency { get; set; }
        [ConfigurationProperty("EventId")]
        [Description("Trace EventId when probe triggers")]
        public int EventId { get; set; }
        [ConfigurationProperty("EventType", DefaultValue = TraceEventType.Error)]
        [Description("Trace EventType when probe triggers")]
        public TraceEventType EventType { get; set; }

        [ConfigurationProperty("CategoryName")]
        [Description("Performance Counter Category Name")]
        public string CategoryName { get; set; }
        [ConfigurationProperty("CounterName")]
        [Description("Performance Counter Name")]
        public string CounterName { get; set; }
        [ConfigurationProperty("InstanceName")]
        [Description("Performance Counter InstanceName")]
        public string InstanceName { get; set; }
        [ConfigurationProperty("MaxValue", DefaultValue = 0.0)]
        [Description("Trigger probe when counter is above limit")]
        public float? MaxValue { get; set; }
        [ConfigurationProperty("MinValue", DefaultValue = 0.0)]
        [Description("Trigger probe when counter is below limit")]
        public float? MinValue { get; set; }
        [ConfigurationProperty("ServiceName")]
        [Description("Lookup InstanceName from Windows Service Name")]
        public string ServiceName { get; set; }
        [ConfigurationProperty("DefaultValue", DefaultValue = 0.0)]
        [Description("Default Counter Value when counter does not exists")]
        public float? DefaultValue { get; set; }

        public PerfMonProbe()
        {
            ProbeFrequency = TimeSpan.FromSeconds(5);
            EventType = TraceEventType.Warning;
        }

        public void Dispose()
        {
            if (_perfCounter != null)
            {
                _perfCounter.Dispose();
                _perfCounter = null;
            }
        }

        public TraceSource ConfigureProbe(string configName)
        {
            _traceSource = new TraceSource(configName);
            StringDictionary attributes = _traceSource.Attributes;  // Initialize TraceSource
            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(configName);
            if (config != null)
            {
                CategoryName = config["CategoryName"];
                CounterName = config["CounterName"];
                InstanceName = config["InstanceName"];
                ServiceName = config["ServiceName"];
                if (config["MaxValue"]!=null)
                    MaxValue = float.Parse(config["MaxValue"]);
                if (config["MinValue"]!=null)
                    MinValue = float.Parse(config["MinValue"]);
                if (config["DefaultValue"]!=null)
                    DefaultValue = float.Parse(config["DefaultValue"]);
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
            if (_perfCounter == null)
            {
                try
                {
                    if (!StartProbe(true))
                        return ProbeFrequency;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);
                    TraceEvent(TraceEventType.Critical, float.NaN, CategoryName + ": " + ex.Message);
                    return ProbeFrequency;
                }
            }

            try
            {
                float value = 0;

                CounterSample curr = _perfCounter.NextSample();
                switch (_perfCounter.CounterType)
                {
                    case PerformanceCounterType.NumberOfItemsHEX32:
                    case PerformanceCounterType.NumberOfItemsHEX64:
                    case PerformanceCounterType.NumberOfItems32:
                    case PerformanceCounterType.NumberOfItems64:
                        {
                            value = CounterSample.Calculate(curr);
                        } break;
                    default:
                        {
                            value = CounterSample.Calculate(_prevSample.Value, curr);
                        } break;
                }

                _prevSample = curr;

                TraceEvent(TraceEventType.Information, value, "Ok");
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

        void TraceEvent(TraceEventType eventType, float value, string message)
        {
            if (eventType == TraceEventType.Information)
            {
                if (MaxValue.HasValue && value > MaxValue)
                {
                    message = "Value is above maximum threshold";
                    eventType = EventType;
                }
                else
                if (MinValue.HasValue && value < MinValue)
                {
                    message = "Value is below minimum threshold";
                    eventType = EventType;
                }
            }

            PerfMonTraceEvent traceEvent = new PerfMonTraceEvent();
            traceEvent.Source = _traceSource.Name;
            traceEvent.MachineName = Environment.MachineName;
            traceEvent.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            traceEvent.CategoryName = CategoryName;
            traceEvent.CounterName = CounterName;
            traceEvent.InstanceName = InstanceName;
            traceEvent.Value = value.ToString("F");
            traceEvent.MaxValue = MaxValue.HasValue ? MaxValue.ToString() : null;
            traceEvent.MinValue = MinValue.HasValue ? MinValue.ToString() : null;
            traceEvent.ServiceName = ServiceName;
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
                XmlSerializer serializer = new XmlSerializer(typeof(PerfMonTraceEvent));
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
                if (_perfCounter != null)
                {
                    _perfCounter.Dispose();
                    _perfCounter = null;
                }

                if (!String.IsNullOrEmpty(InstanceName))
                {
                    if (!PerformanceCounterCategory.Exists(CategoryName))
                    {
                        PerformanceCounter.CloseSharedResources();
                        TraceEvent(TraceEventType.Critical, float.NaN, "Category Name " + CategoryName + " is not available.");
                        return false;
                    }

                    PerformanceCounterCategory cat = new PerformanceCounterCategory(CategoryName);
                    string[] instances = cat.GetInstanceNames();
                    foreach (string instance in instances)
                    {
                        if (instance.IndexOf(InstanceName, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            if (_perfCounter == null || _perfCounter.InstanceName.Equals(InstanceName, StringComparison.CurrentCultureIgnoreCase))
                                _perfCounter = new PerformanceCounter(CategoryName, CounterName, instance, true);
                        }
                    }
                    if (_perfCounter == null)
                    {
                        // Check if it is okay that instance cannot be found
                        if (DefaultValue.HasValue && writeEvent)
                        {
                            TraceEvent(TraceEventType.Information, DefaultValue.Value, "Instance does not exist");
                            return false;
                        }
                        _perfCounter = new PerformanceCounter(CategoryName, CounterName, InstanceName, true);
                    }
                }
                else
                if (!String.IsNullOrEmpty(ServiceName))
                {
                    // Lookup the process instance name using the ServiceName
                    uint processId = GetProcessIDByServiceName(ServiceName);
                    if (processId == 0)
                    {
                        if (DefaultValue.HasValue && writeEvent)
                        {
                            TraceEvent(TraceEventType.Information, DefaultValue.Value, "Service '" + ServiceName + "' is not running");
                            return false;
                        }
                        else
                        {
                            throw new InvalidOperationException("Service '" + ServiceName + "' is not running");
                        }
                    }

                    string instanceName = GetProcessInstanceName((long)processId);
                    if (instanceName == null)
                        throw new InvalidOperationException("Service instance '" + ServiceName + "' cannot be found");
                    _perfCounter = new PerformanceCounter(CategoryName, CounterName, instanceName, true);
                }
                else
                    _perfCounter = new PerformanceCounter(CategoryName, CounterName, true);

                _prevSample = _perfCounter.NextSample();
                return true;
            }
            catch (Exception)
            {
                if (_perfCounter != null)
                {
                    _perfCounter.Dispose();
                    _perfCounter = null;
                }
                PerformanceCounter.CloseSharedResources();
                throw;
            }
        }

        private static uint GetProcessIDByServiceName(string serviceName)
        {
            uint processId = 0;
            string qry = "SELECT PROCESSID FROM WIN32_SERVICE WHERE NAME = '" + serviceName + "'";
            System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(qry);
            foreach (System.Management.ManagementObject mngntObj in searcher.Get())
            {
                processId = (uint)mngntObj["PROCESSID"];
                if (processId > 0)
                    return processId;
            }
            return processId;
        }

        private static string GetProcessInstanceName(long pid)
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");

            string[] instances = cat.GetInstanceNames();
            foreach (string instance in instances)
            {
                using (PerformanceCounter cnt = new PerformanceCounter("Process", "ID Process", instance, true))
                {
                    long val = cnt.RawValue;
                    if (val == pid)
                    {
                        return instance;
                    }
                }
            }
            return null;
        }
    }
}
