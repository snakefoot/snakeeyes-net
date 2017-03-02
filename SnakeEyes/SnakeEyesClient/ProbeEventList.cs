using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SnakeEyesClient
{
    public class ProbeEvent
    {
        public DateTime Timestamp { get; set; }
        public TraceEventType EventType { get; set; }
        public string Message { get; set; }
        public int EventId { get; set; }
    }

    class ProbeEventList
    {
        Dictionary<string, ObservableCollection<ProbeEvent>> _probeEvents = new Dictionary<string, ObservableCollection<ProbeEvent>>();
        ObservableCollection<string> _probeNames = new ObservableCollection<string>();

        public int ProbeEventMaxCount { get; set; }

        public ObservableCollection<ProbeEvent> GetEvents(string probeName)
        {
            ObservableCollection<ProbeEvent> probeEvents;
            if (_probeEvents.TryGetValue(probeName, out probeEvents))
                return probeEvents;
            else
                return new ObservableCollection<ProbeEvent>();
        }

        public ObservableCollection<string> GetProbeNames()
        {
            return _probeNames;
        }

        public void AddEvent(string probeName, DateTime timestamp, TraceEventType eventType, string message, int eventId)
        {
            ObservableCollection<ProbeEvent> probeEvents;
            if (!_probeEvents.TryGetValue(probeName, out probeEvents))
            {
                probeEvents = new ObservableCollection<ProbeEvent>();
                _probeEvents[probeName] = probeEvents;
                _probeNames.Add(probeName);
            }
            ProbeEvent lastEvent = null;
            while (probeEvents.Count >= ProbeEventMaxCount)
            {
                if (lastEvent == null)
                    lastEvent = probeEvents[0];
                probeEvents.RemoveAt(0);
            }
            if (lastEvent == null)
                lastEvent = new ProbeEvent();
            lastEvent.Timestamp = timestamp;
            lastEvent.EventType = eventType;
            lastEvent.Message = message;
            lastEvent.EventId = eventId;
            probeEvents.Add(lastEvent);
        }
    }
}
