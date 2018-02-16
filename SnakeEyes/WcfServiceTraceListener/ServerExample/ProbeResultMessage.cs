using System;
using System.Diagnostics;

namespace Monitoring.Models
{
    public class ProbeResultMessage
    {
        public string MachineName { get; set; }
        public string Name { get; set; }
        public TraceEventType EventType { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public double? Value { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; }
    }
}
