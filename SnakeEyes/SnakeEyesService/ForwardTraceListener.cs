using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SnakeEyes
{
    // Forwards all probe-traces to the global Trace-interface
    class ForwardTraceListener : TraceListener
    {
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            Trace.WriteLine(source + ": " + id.ToString() + " (" + eventType.ToString() + ")");
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            Trace.WriteLine(source + ": " + id.ToString() + " (" + eventType.ToString() + ")");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            Trace.WriteLine(source + ": " + id.ToString() + " (" + eventType.ToString() + ")");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            string value = message;
            try
            {
                using (System.IO.StringReader sr = new System.IO.StringReader(message))
                using (System.Xml.XmlReader xr = System.Xml.XmlReader.Create(sr))
                {
                    while (xr.Read())
                    {
                        if (eventType == TraceEventType.Critical)
                        {
                            if (xr.NodeType == System.Xml.XmlNodeType.Element && xr.Name == "Message")
                            {
                                value = xr.ReadString();
                                break;
                            }
                        }
                        else
                        {
                            if (xr.NodeType == System.Xml.XmlNodeType.Element && xr.Name == "Value")
                            {
                                value = xr.ReadString();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            Trace.WriteLine(source + ": " + value + " (" + eventType.ToString() + ")");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            Trace.WriteLine(source + ": " + id.ToString() + " (" + eventType.ToString() + ")");
        }

        public override void Write(string message)
        {
            Trace.Write(message);
        }

        public override void WriteLine(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
