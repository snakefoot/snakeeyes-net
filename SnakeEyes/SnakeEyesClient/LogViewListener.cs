using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SnakeEyesTray
{
    internal class LogViewListener : TraceListener
    {
        public delegate void WriteTraceEvent(string source, TraceEventType eventType, int eventId, string message);

        WriteTraceEvent _messageWriter;

        public LogViewListener(WriteTraceEvent messageWriter)
            :base(typeof(LogViewListener).Name)
        {
            _messageWriter = messageWriter;
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            _messageWriter(source, eventType, id, data != null ? data.ToString() : "");
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            _messageWriter(source, eventType, id, "");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            _messageWriter(source, eventType, id, "");
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
            _messageWriter(source, eventType, id, value);
        }

        public override void Write(string message)
        {
            _messageWriter("", TraceEventType.Information, 0, message);
        }

        public override void WriteLine(string message)
        {
            _messageWriter("", TraceEventType.Information, 0, message);
        }
    }
}
