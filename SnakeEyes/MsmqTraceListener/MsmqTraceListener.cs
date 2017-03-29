using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SnakeEyes
{
    public class MsmqTraceListener : TraceListener
    {
        public string MsmqQueueName { get { return Attributes["queueName"]; } }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            SendMessage(message);
        }

        public override void Write(string message)
        {
            SendMessage(message);
        }

        public override void WriteLine(string message)
        {
            SendMessage(message);
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "queueName" };
        }

        void SendMessage(string trace)
        {
            //From Windows Service, use this code
            if (!MessageQueue.Exists(MsmqQueueName))
            {
                MessageQueue.Create(MsmqQueueName);
            }
            MessageQueue messageQueue = new MessageQueue(MsmqQueueName);
            messageQueue.Label = "SnakeEyesQueue";

            Message message = new Message()
            {
                Label = "SnakeMessage",
                Body = trace,
                Formatter = new ActiveXMessageFormatter()
            };
            messageQueue.Send(message);
        }
    }
}
