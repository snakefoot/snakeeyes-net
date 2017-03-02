using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace SnakeEyes
{
    public class DelayStateFilter : TraceFilter
    {
        public TimeSpan NextTriggerTime { get; set; }
        public TimeSpan DelayTriggerTime { get; set; }

        string _configSection;
        Dictionary<string, TraceEventType> _lastEventType = new Dictionary<string, TraceEventType>();
        Dictionary<string, DateTime> _lastTrigger = new Dictionary<string, DateTime>();
        Dictionary<string, TimeSpan> _currentDelay = new Dictionary<string, TimeSpan>();

        public DelayStateFilter(string configSection)
        {
            _configSection = configSection;

            DelayTriggerTime = TimeSpan.FromSeconds(0);
            NextTriggerTime = TimeSpan.FromSeconds(0);

            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(configSection);
            if (config != null)
            {
                if (config["NextTriggerTime"] != null)
                    NextTriggerTime = TimeSpan.FromSeconds(Int32.Parse(config["NextTriggerTime"]));
                if (config["DelayTriggerTime"] != null)
                    DelayTriggerTime = TimeSpan.FromSeconds(Int32.Parse(config["DelayTriggerTime"]));
            }
        }

        public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
        {
            if (!_lastEventType.ContainsKey(source))
            {
                _lastEventType.Add(source, TraceEventType.Information);
                _lastTrigger.Add(source, DateTime.MinValue);
                _currentDelay.Add(source, TimeSpan.FromSeconds(0));
            }

            if (_lastEventType[source] != eventType)
            {
                if (DelayTriggerTime.TotalSeconds > 0)
                {
                    if (_lastEventType[source] != TraceEventType.Critical && eventType != TraceEventType.Critical)
                    {
                        if (eventType != TraceEventType.Information)
                        {
                            // Remember when we last had a bad state
                            //  - We only come here when have been in good state for a "long" time
                            _lastEventType[source] = eventType;
                            _lastTrigger[source] = DateTime.UtcNow;
                            _currentDelay[source] = DelayTriggerTime;
                        }
                        else
                        {
                            // We stay in bad state until enough time has passed
                            if (DateTime.UtcNow.Subtract(_lastTrigger[source]) < DelayTriggerTime)
                                return false;

                            // Clear bad state
                            _lastTrigger[source] = DateTime.UtcNow;
                            _lastEventType[source] = eventType;

                            // Delay have been changed if we have entered bad-state
                            if (NextTriggerTime.TotalSeconds > 0)
                            {
                                if (_currentDelay[source] >= NextTriggerTime)
                                {
                                    System.Diagnostics.Trace.WriteLine(_configSection + ": " + source + " triggered " + eventType.ToString() + " event.");
                                    _currentDelay[source] = TimeSpan.FromSeconds(0);
                                    return true;    // We have to clear the bad event
                                }
                            }
                            else
                            {
                                if (_currentDelay[source].TotalSeconds <= 0)
                                {
                                    System.Diagnostics.Trace.WriteLine(_configSection + ": " + source + " triggered " + eventType.ToString() + " event.");
                                    return true;    // We have to clear the bad event
                                }
                            }
                        }
                        return false;
                    }
                }
                _currentDelay[source] = NextTriggerTime;
                _lastTrigger[source] = DateTime.UtcNow;
                _lastEventType[source] = eventType;
                System.Diagnostics.Trace.WriteLine(_configSection + ": " + source + " triggered " + eventType.ToString() + " event.");
                return true;
            }

            if (eventType == TraceEventType.Information)
                return false;

            if (DelayTriggerTime.TotalSeconds > 0)
            {
                // We should only trigger again when state changes back to good
                if (NextTriggerTime.TotalSeconds <= 0 && _currentDelay[source].TotalSeconds <= 0)
                {
                    if (eventType != TraceEventType.Information)
                        _lastTrigger[source] = DateTime.UtcNow;    // Update last bad state
                    return false;
                }
            }

            if (DateTime.UtcNow.Subtract(_lastTrigger[source]) < _currentDelay[source])
                return false;

            if (NextTriggerTime.TotalSeconds > 0)
            {
                // Setup when to allow the next trigger
                _currentDelay[source] = _currentDelay[source].Add(_currentDelay[source]);
                if (_currentDelay[source] < NextTriggerTime)
                    _currentDelay[source] = NextTriggerTime;
            }
            else
                _currentDelay[source] = TimeSpan.FromSeconds(0);    // Trigger whenever state changes
            _lastTrigger[source] = DateTime.UtcNow;
            _lastEventType[source] = eventType;
            System.Diagnostics.Trace.WriteLine(_configSection + ": " + source + " triggered " + eventType.ToString() + " event.");
            return true;
        }
    }
}
