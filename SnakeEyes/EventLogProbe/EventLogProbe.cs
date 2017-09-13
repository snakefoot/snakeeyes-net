using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SnakeEyes
{
    [XmlRoot("TraceEvent")]
    public class EventLogTraceEvent
    {
        public string Source { get; set; }
        public string MachineName { get; set; }
        public string Timestamp { get; set; }
        public int EventId { get; set; }
        public TraceEventType EventType { get; set; }
        public string EventLogSource { get; set; }
        public EventLogEntryType EventLogType { get; set; }
        public int EventLogId { get; set; }
        public string Message { get; set; }
    };

    public class EventLogProbe : IProbe
    {
        TraceSource _traceSource;
        EventLog _eventLog;
        int _eventLogIndex = -1;

        Dictionary<string, List<EventLogTraceEvent>> _eventFilter = new Dictionary<string, List<EventLogTraceEvent>>();

        [ConfigurationProperty("ProbeFrequency", DefaultValue = 1)]
        [Description("Number of seconds between each probe check")]
        public TimeSpan ProbeFrequency { get; set; }

        [ConfigurationProperty("EventLogName", IsRequired = true)]
        public string EventLogName { get; set; }
        [ConfigurationProperty("Filter0")]
        [Description("Filter format [EventLogSource,EventLogLevel,EventLogId]=[TraceLevel,TraceEventId]")]
        public string Filter0 { get; set; }

        public EventLogProbe()
        {
            ProbeFrequency = TimeSpan.FromSeconds(5);
        }

        public void Dispose()
        {
            if (_eventLog != null)
            {
                _eventLog.Dispose();
                _eventLog = null;
            }
        }

        public TraceSource ConfigureProbe(string configName)
        {
            _traceSource = new TraceSource(configName);
            StringDictionary attributes = _traceSource.Attributes;  // Initialize TraceSource
            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(configName);
            if (config != null)
            {
                EventLogName = config["EventLogName"];
                if (config["ProbeFrequency"] != null)
                    ProbeFrequency = TimeSpan.FromSeconds(Int32.Parse(config["ProbeFrequency"]));

                foreach (string eventFilter in config.AllKeys)
                {
                    if (!eventFilter.StartsWith("Filter"))
                        continue;

                    int filterIndex;
                    if (!int.TryParse(eventFilter.Substring(6), out filterIndex) || eventFilter != ("Filter" + filterIndex.ToString()))
                        continue;

                    string[] filterItems = config[eventFilter].Split('=').Select(f => f.Trim()).Where(f => !string.IsNullOrEmpty(f)).ToArray();

                    int eventId = -1;
                    TraceEventType eventType = 0;
                    if (filterItems.Length > 1)
                    {
                        foreach (string filterItem in filterItems[1].Split(','))
                        {
                            string traceResultItem = filterItem.Trim();
                            if (String.IsNullOrEmpty(traceResultItem))
                                continue;

                            try
                            {
                                eventId = Int32.Parse(traceResultItem);
                            }
                            catch (Exception exId)
                            {
                                try
                                {
                                    eventType = (TraceEventType)Enum.Parse(typeof(TraceEventType), traceResultItem);
                                }
                                catch (Exception exType)
                                {
                                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " failed to parse " + eventFilter + "=" + config[eventFilter]);
                                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " - " + exId.Message);
                                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " - " + exType.Message);
                                    break;
                                }
                            }
                        }
                    }

                    string eventLogSource = null;
                    int eventLogId = -1;
                    EventLogEntryType eventLogType = 0;
                    if (filterItems.Length >= 1)
                    {
                        foreach (string filterItem in filterItems[0].Split(','))
                        {
                            string eventFilterItem = filterItem.Trim();
                            if (eventLogSource == null)
                            {
                                eventLogSource = eventFilterItem;
                                continue;
                            }

                            if (String.IsNullOrEmpty(eventFilterItem))
                                continue;

                            try
                            {
                                eventLogId = Int32.Parse(eventFilterItem);
                            }
                            catch (Exception exId)
                            {
                                try
                                {
                                    eventLogType = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), eventFilterItem);
                                }
                                catch (Exception exType)
                                {
                                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " failed to parse " + eventFilter + "=" + config[eventFilter]);
                                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " - " + exId.Message);
                                    System.Diagnostics.Trace.WriteLine(_traceSource.Name + " - " + exType.Message);
                                    eventLogSource = null;
                                    break;
                                }
                            }
                        }
                    }

                    if (eventLogSource == null)
                        continue;

                    if (!_eventFilter.ContainsKey(eventLogSource))
                        _eventFilter.Add(eventLogSource, new List<EventLogTraceEvent>());
                    EventLogTraceEvent eventLogFilter = new EventLogTraceEvent();
                    eventLogFilter.EventId = eventId;
                    eventLogFilter.EventType = eventType;
                    eventLogFilter.EventLogSource = eventLogSource;
                    eventLogFilter.EventLogType = eventLogType;
                    eventLogFilter.EventLogId = eventLogId;
                    _eventFilter[eventLogSource].Add(eventLogFilter);
                }
            }
            StartProbe(false);
            return _traceSource;
        }

        bool StartProbe(bool writeEvent)
        {
            try
            {
                if (_eventLog != null)
                {
                    _eventLog.Dispose();
                    _eventLog = null;
                }

                _eventLog = new EventLog(EventLogName);
                _eventLog.EndInit();
                if (_eventLogIndex == -1)
                {
                    int count = _eventLog.Entries.Count;
                    if (count > 0)
                        _eventLogIndex = _eventLog.Entries[count - 1].Index;
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);

                if (writeEvent)
                    TraceCriticalEvent(ex.Message);
                if (_eventLog != null)
                {
                    _eventLog.Dispose();
                    _eventLog = null;
                }
                return false;
            }
        }

        public TimeSpan ExecuteProbe()
        {
            if (_eventLog == null && !StartProbe(true))
                return ProbeFrequency;

            try
            {
                // Collect all relevant TraceEvents that arrived since last check
                List<EventLogTraceEvent> traceEvents = new List<EventLogTraceEvent>();

                int lastEntryIndex = _eventLogIndex;
                int prevEntryIndex = -1;
                int count = _eventLog.Entries.Count;
                for (int i = count - 1; i >= 0; --i)
                {
                    EventLogEntry entry = null;

                    try
                    {
                        entry = _eventLog.Entries[i];
                        if (prevEntryIndex != -1)
                        {
                            // Sanity check to ensure the EventLog have not been pruned by Windows
                            if (_eventLog.Entries[i + 1].Index != prevEntryIndex)
                            {
                                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " Retries EventLog Check in 1 second.");
                                return TimeSpan.FromSeconds(1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Possible that the EventLog have been pruned by Windows
                        System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);
                        if (StartProbe(true))
                        {
                            System.Diagnostics.Trace.WriteLine(_traceSource.Name + " Retries EventLog Check in 1 second.");
                            return TimeSpan.FromSeconds(1);
                        }
                        throw;  // Rethrow original exception
                    }

                    if (entry.Index == _eventLogIndex)
                        break;

                    if (lastEntryIndex == _eventLogIndex)
                        lastEntryIndex = entry.Index;

                    int eventId = (int)(entry.InstanceId & 0x3fff);
                    List<EventLogTraceEvent> eventFilters = null;
                    if (_eventFilter.TryGetValue(entry.Source, out eventFilters))
                    {
                        foreach (EventLogTraceEvent eventFilter in eventFilters)
                        {
                            if (eventFilter.EventLogId != -1 && eventFilter.EventLogId != eventId)
                                continue;

                            if (eventFilter.EventLogType != 0 && eventFilter.EventLogType != entry.EntryType)
                                continue;

                            traceEvents.Add( CreateEventLogEntry(entry, eventFilter.EventType, eventFilter.EventId) );
                            break;
                        }
                    }

                    prevEntryIndex = entry.Index;
                }

                // Publish any relevant EventLog entries, in reported order
                for(int i = traceEvents.Count - 1; i >= 0; --i)
                    TraceEventLogEvent(traceEvents[i]);

                _eventLogIndex = lastEntryIndex;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);

                TraceCriticalEvent(ex.Message);

                StartProbe(true);
            }
            return ProbeFrequency;
        }

        EventLogTraceEvent CreateEventLogEntry(EventLogEntry entry, TraceEventType eventType, int eventId)
        {
            EventLogTraceEvent traceEvent = new EventLogTraceEvent();
            traceEvent.Source = _traceSource.Name;
            traceEvent.Timestamp = entry.TimeWritten.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            traceEvent.MachineName = Environment.MachineName;
            traceEvent.EventId = eventId;
            traceEvent.EventType = eventType;
            traceEvent.EventLogSource = entry.Source;
            traceEvent.EventLogId = (int)(entry.InstanceId & 0x3fff);
            traceEvent.EventLogType = entry.EntryType;

            if (traceEvent.EventId==-1)
                traceEvent.EventId = traceEvent.EventLogId;

            if (traceEvent.EventType == 0)
            {
                traceEvent.EventType = TraceEventType.Information;
                switch (entry.EntryType)
                {
                    case EventLogEntryType.Error: traceEvent.EventType = TraceEventType.Error; break;
                    case EventLogEntryType.Warning: traceEvent.EventType = TraceEventType.Warning; break;
                }
            }
            if (System.Environment.OSVersion.Version.Major >= 6)
            {
                if (entry.Message.IndexOf(" Event ID '" + entry.InstanceId.ToString() + "' ") == -1)
                {
                    traceEvent.Message = entry.Message;
                }
                else
                {
                    traceEvent.Message = GetEventLogItemMessage(_eventLog.MachineName, _eventLog.LogDisplayName, (uint)(int)entry.Index);
                }
            }
            else
            {
                traceEvent.Message = entry.Message;
            }

            return traceEvent;
        }

        void TraceCriticalEvent(string message)
        {
            EventLogTraceEvent traceEvent = new EventLogTraceEvent();
            traceEvent.Message = message;
            traceEvent.Source = _traceSource.Name;
            traceEvent.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            traceEvent.MachineName = Environment.MachineName;
            traceEvent.EventId = -1;
            traceEvent.EventType = TraceEventType.Critical;
            if (_eventLog != null)
                traceEvent.EventLogSource = _eventLog.Log;
            traceEvent.EventLogId = -1;
            traceEvent.EventLogType = EventLogEntryType.Error;
            TraceEventLogEvent(traceEvent);
        }

        void TraceEventLogEvent(EventLogTraceEvent traceEvent)
        {
            string formatMessage = "";

            using (StringWriter writer = new StringWriter())
            using (XmlTextWriter xmlwriter = new XmlTextWriter(writer))
            {
                xmlwriter.Formatting = Formatting.Indented;
                XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
                xmlnsEmpty.Add("", "");
                XmlSerializer serializer = new XmlSerializer(typeof(EventLogTraceEvent));
                serializer.Serialize(xmlwriter, traceEvent, xmlnsEmpty);
                formatMessage = writer.ToString();
            }

            TraceEvent(traceEvent.EventType, traceEvent.EventId, formatMessage);
        }

        void TraceEvent(TraceEventType eventType, int eventId, string message)
        {
            try
            {
                _traceSource.TraceEvent(eventType, eventId, message);
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

        private static string GetStandardPath(string machinename)
        {
            return String.Concat(
                        @"\\",
                        machinename,
                        @"\root\CIMV2"
            );
        }

        private static string GetEventLogItemMessage(string machinename, string logname, uint thisIndex)
        {
            ManagementScope messageScope = new ManagementScope(
                        GetStandardPath(machinename)
            );

            messageScope.Connect();

            StringBuilder query = new StringBuilder();
            query.Append("select Message, InsertionStrings from Win32_NTLogEvent where LogFile ='");
            query.Append(logname.Replace("'", "''"));
            query.Append("' AND RecordNumber='");
            query.Append(thisIndex);
            query.Append("'");

            System.Management.ObjectQuery objectQuery = new System.Management.ObjectQuery(
                query.ToString()
            );

            EnumerationOptions objectQueryOptions = new EnumerationOptions();
            objectQueryOptions.Rewindable = false;

            using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(messageScope, objectQuery, objectQueryOptions))
            {
                // Execute the query
                using (ManagementObjectCollection collection = objectSearcher.Get())
                {
                    // Execute the query
                    using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = collection.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            string message = (string)enumerator.Current["Message"];
                            string[] insertionStrings = (string[])enumerator.Current["InsertionStrings"];

                            if (message == null)
                            {
                                if (insertionStrings.Length > 0)
                                {
                                    StringBuilder sb = new StringBuilder();

                                    for (int i = 0; i < insertionStrings.Length; i++)
                                    {
                                        sb.Append(insertionStrings[i]);
                                        sb.Append(" ");
                                    }

                                    return sb.ToString();
                                }
                            }
                            else
                            {
                                return message;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
