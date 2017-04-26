using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;
using System.Text.RegularExpressions;
using System.Xml;

namespace SnakeEyes
{
    public class MsmqTraceListener : TraceListener
    {
        public string MsmqQueueName { get { return Attributes["queueName"]; } }

        public string MsmqQueueLabel { get { return Attributes["queueLabel"]; } }

        public string MessageFormatLabel { get { return Attributes["formatLabel"]; } }

        public string MessageFormatBody { get { return Attributes["formatBody"]; } }

        protected MessageQueue MsmqQueue
        {
            get
            {
                return _msmqQueue ?? (_msmqQueue = CreateMessageQueue());
            }
        }
        private MessageQueue _msmqQueue;
        private readonly IMessageFormatter _msmqMessageFormatter = new ActiveXMessageFormatter();

        protected virtual void SendMessage(string messageBody, string messageLabel)
        {
            try
            {
                MessageQueue msmqQueue = MsmqQueue;
                if (msmqQueue != null)
                {
                    Message msmqMessage = CreateMessage(messageBody, messageLabel);
                    if (msmqMessage != null)
                    {
                        msmqQueue.Send(msmqMessage, MessageQueueTransactionType.Single);
                    }
                }
            }
            catch (Exception ex)
            {
                _msmqQueue = null;
                System.Diagnostics.Trace.TraceError(Name + " " + ex.Message);
                System.Diagnostics.Trace.WriteLine(Name + " failed to send message");
                System.Diagnostics.Trace.WriteLine(Name + " " + messageBody);
                System.Diagnostics.Trace.WriteLine(Name + " " + ex.ToString());
            }
        }

        protected virtual MessageQueue CreateMessageQueue()
        {
            //From Windows Service, use this code
            if (!MessageQueue.Exists(MsmqQueueName))
            {
                MessageQueue.Create(MsmqQueueName);
            }

            MessageQueue messageQueue = new MessageQueue(MsmqQueueName);
            messageQueue.Label = MsmqQueueLabel;
            return messageQueue;
        }

        protected virtual Message CreateMessage(string messageBody, string messageLabel)
        {
            Message msmqMessage = new Message()
            {
                Body = messageBody ?? MessageFormatBody,
                BodyType = (int)System.Runtime.InteropServices.VarEnum.VT_BSTR,
                Formatter = _msmqMessageFormatter,
                Recoverable = true,
                UseDeadLetterQueue = false,
                UseJournalQueue = false,
            };

            if (String.IsNullOrEmpty(messageLabel))
                msmqMessage.Label = MessageFormatLabel;
            else
                msmqMessage.Label = messageLabel;
            return msmqMessage;
        }

        KeyValuePair<string, string> BuildMsmqMessage(string source, TraceEventType eventType, int id, string message)
        {
            string subject = "";
            if (String.IsNullOrEmpty(MessageFormatLabel))
            {
                subject += eventType.ToString();
                subject += " - ";
                subject += source;
                subject += " - ";
                subject += id.ToString();
            }
            else
            {
                subject = BuildFormatString(MessageFormatLabel, message);
            }

            string body = message;
            if (!String.IsNullOrEmpty(MessageFormatBody))
            {
                body = BuildFormatString(MessageFormatBody, message);
            }

            return new KeyValuePair<string, string>(subject, body);
        }

        readonly Regex _regexFormatString = new System.Text.RegularExpressions.Regex("{{(.*?)}}", RegexOptions.Compiled);
        string BuildFormatString(string formatString, string message)
        {
            string result = formatString;

            XmlDocument xmlDoc = null;

            foreach (Match match in _regexFormatString.Matches(formatString))
            {
                try
                {
                    if (xmlDoc == null)
                    {
                        xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(message);
                    }
                    string xpath = match.Groups[1].Value;
                    XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                    result = result.Replace(match.Value, xmlNode.InnerText);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(Name + " " + ex.Message);
                    System.Diagnostics.Trace.WriteLine(Name + " failed to replace token " + match.Value);
                    System.Diagnostics.Trace.WriteLine(Name + " " + message);
                    System.Diagnostics.Trace.WriteLine(Name + " " + ex.ToString());
                }
            }

            return result;
        }

        public override void Close()
        {
            MessageQueue msmqQueue = _msmqQueue;
            if (msmqQueue != null)
            {
                _msmqQueue = null;
                msmqQueue.Dispose();
            }
            base.Close();
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            KeyValuePair<string, string> msmqMsg = BuildMsmqMessage(source, eventType, id, message);
            SendMessage(msmqMsg.Key, msmqMsg.Value);
        }

        public override void Write(string message)
        {
            SendMessage(message, null);
        }

        public override void WriteLine(string message)
        {
            SendMessage(message, null);
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "queueName", "queueLabel", "formatLabel", "formatBody" };
        }
    }
}
