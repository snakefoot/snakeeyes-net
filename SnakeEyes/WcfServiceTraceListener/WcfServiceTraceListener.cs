using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using WcfServiceTraceListener.MonitoringService;

namespace SnakeEyes
{
    public class WcfServiceTraceListener : TraceListener
    {
        protected virtual void SendMessage(string message)
        {
            try
            {
                MonitoringServiceClient client = new MonitoringServiceClient();

                XmlDocument xmlDoc = null;
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(message);
                XmlNode n = xmlDoc.SelectSingleNode("/TraceEvent");
                Enum.TryParse<TraceEventType>(n.SelectSingleNode("EventType").InnerText, out TraceEventType type);

                ProbeResultMessage p = new ProbeResultMessage
                {
                    MachineName = n.SelectSingleNode("MachineName").InnerText,
                    Name = n.SelectSingleNode("Source").InnerText,
                    Timestamp = DateTime.Parse(n.SelectSingleNode("Timestamp").InnerText),
                    EventType = type,
                    Message = n.SelectSingleNode("Message").InnerText
                };

                if (n.SelectSingleNode("Value") != null && n.SelectSingleNode("Value").InnerText.Trim() != "")
                {
                    p.Value = Double.Parse(n.SelectSingleNode("Value").InnerText);
                }
                if (n.SelectSingleNode("MaxValue") != null && n.SelectSingleNode("MaxValue").InnerText.Trim() != "")
                {
                    p.MaxValue = Double.Parse(n.SelectSingleNode("MaxValue").InnerText);
                }
                if (n.SelectSingleNode("MinValue") != null && n.SelectSingleNode("MinValue").InnerText.Trim() != "")
                {
                    p.MinValue = Double.Parse(n.SelectSingleNode("MinValue").InnerText);
                }

                client.AddProbeResult(p);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(Name + " " + ex.Message);
                System.Diagnostics.Trace.WriteLine(Name + " failed to send message");
                System.Diagnostics.Trace.WriteLine(Name + " " + message);
                System.Diagnostics.Trace.WriteLine(Name + " " + ex.ToString());
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            SendMessage(message);
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] {};
        }
    }
}
