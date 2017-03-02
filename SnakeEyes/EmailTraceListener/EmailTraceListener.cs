using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Xml;

namespace SnakeEyes
{
    public class EmailTraceListener : TraceListener
    {
        public string EmailHost { get { return Attributes["host"]; } }
        public string EmailPort { get { return Attributes["port"]; } }
        public string EmailUsername { get { return Attributes["username"]; } }
        public string EmailPassword { get { return Attributes["password"]; } }
        public string EmailToAddress { get { return Attributes["toAddress"]; } }
        public string EmailFromAddress { get { return Attributes["fromAddress"]; } }
        public string EmailFormatSubject { get { return Attributes["formatSubject"]; } }
        public string EmailFormatBody { get { return Attributes["formatBody"]; } }
        public string EmailSsl { get { return Attributes["ssl"]; } }
        public string EmailFormatBundleSource { get { return Attributes["formatBundleSource"]; } }
        public string EmailThrottleSeconds { get { return Attributes["throttleSeconds"]; } }

        class BundledMessage
        {
            public BundledMessage(string source, KeyValuePair<string,string> emailMsg)
            {
                Source = source;
                Subject = emailMsg.Key;
                Body = emailMsg.Value;
            }

            public string Source { get; private set; }
            public string Subject { get; private set; }
            public string Body { get; private set; }
        };

        Dictionary<string, DateTime> _bundledSources = new Dictionary<string, DateTime>();
        List<BundledMessage> _bundledMsgs = new List<BundledMessage>();
        volatile SmtpClient _smtpClient = null;

        string BuildFormatString(string formatString, string message)
        {
            string result = formatString;

            XmlDocument xmlDoc = null;

            Regex regex = new System.Text.RegularExpressions.Regex("{{.*?}}");
            foreach (Match match in regex.Matches(formatString))
            {
                try
                {
                    if (xmlDoc == null)
                    {
                        xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(message);
                    }
                    string xpath = match.Value.Substring(2, match.Value.Length - 4);
                    XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                    result = result.Replace(match.Value, xmlNode.InnerText);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(Name + " " + ex.Message);
                    System.Diagnostics.Trace.WriteLine(Name + " failed to replace token " + match.Value);
                    System.Diagnostics.Trace.WriteLine(Name + " " + message);
                    System.Diagnostics.Trace.WriteLine(Name + " " + ex.StackTrace);
                }
            }

            return result;
        }

        KeyValuePair<string, string> BuildEmailMessage(string source, TraceEventType eventType, int id, string message)
        {
            string subject = "";
            if (String.IsNullOrEmpty(EmailFormatSubject))
            {
                subject += eventType.ToString();
                subject += " - ";
                subject += source;
                subject += " - ";
                subject += id.ToString();
            }
            else
            {
                subject = BuildFormatString(EmailFormatSubject, message);
            }

            string body = message;
            if (!String.IsNullOrEmpty(EmailFormatBody))
            {
                body = BuildFormatString(EmailFormatBody, message);
            }

            return new KeyValuePair<string, string>(subject, body);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter!=null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            // If multiple messages comes from the source quickly, then the SMTP server might throttle us
            //  - Bundle messages from the same source, and send them all in a single operation
            bool waitingForReply = false;
            string bundleSource = null;
            if (!String.IsNullOrEmpty(EmailFormatBundleSource))
                bundleSource = BuildFormatString(EmailFormatBundleSource, message);

            lock (_bundledSources)
            {
                waitingForReply = _smtpClient != null;
                if (!String.IsNullOrEmpty(bundleSource))
                {
                    if (waitingForReply)
                    {
                        DateTime bundleStatus;
                        if (_bundledSources.TryGetValue(bundleSource, out bundleStatus))
                        {
                            if (DateTime.UtcNow.Subtract(bundleStatus).TotalSeconds < 60)
                            {
                                // Bundle the message and return quickly
                                _bundledSources[bundleSource] = DateTime.UtcNow;
                                _bundledMsgs.Add(new BundledMessage(bundleSource, BuildEmailMessage(source, eventType, id, message)));
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (_bundledSources.ContainsKey(bundleSource))
                            _bundledSources[bundleSource] = DateTime.UtcNow;
                        else
                            _bundledSources.Add(bundleSource, DateTime.UtcNow);
                    }
                }
            }

            if (waitingForReply)
            {
                System.Diagnostics.Trace.WriteLine(Name + " waiting for previous smtp request to finish...");
                while (true)
                {
                    lock (_bundledSources)
                    {
                        if (_bundledMsgs.Count == 0 && _smtpClient == null)
                        {
                            if (!String.IsNullOrEmpty(bundleSource))
                            {
                                if (_bundledSources.ContainsKey(bundleSource))
                                    _bundledSources[bundleSource] = DateTime.UtcNow;
                                else
                                    _bundledSources.Add(bundleSource, DateTime.UtcNow);
                            }
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(10);
                }
            }

            KeyValuePair<string, string> emailMsg = BuildEmailMessage(source, eventType, id, message);
            SendTraceEmail(emailMsg.Value, emailMsg.Key);
        }

        public override void Write(string message)
        {
            SendTraceEmail(message, null);
        }

        public override void WriteLine(string message)
        {
            SendTraceEmail(message, null);
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "fromAddress", "toAddress", "formatSubject", "formatBody", "host", "port", "username", "password", "ssl", "formatBundleSource" };
        }

        void SendTraceEmail(string message, string subject)
        {
            lock (_bundledSources)
            {
                using (MailMessage msg = new MailMessage())
                {
                    msg.From = new MailAddress(EmailFromAddress);
                    foreach (string s in EmailToAddress.Split(";".ToCharArray()))
                    {
                        msg.To.Add(s);
                    }
                    if (String.IsNullOrEmpty(subject))
                        msg.Subject = EmailFormatSubject;
                    else
                        msg.Subject = subject;

                    msg.Body = message;

                    _smtpClient = null;
                    if (String.IsNullOrEmpty(EmailHost))
                    {
                        _smtpClient = new SmtpClient();    // Use <system.net><mailSettings>
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(EmailPort))
                            _smtpClient = new SmtpClient(EmailHost);
                        else
                            _smtpClient = new SmtpClient(EmailHost, Int32.Parse(EmailPort));

                        if (String.IsNullOrEmpty(EmailUsername) && String.IsNullOrEmpty(EmailPassword))
                            _smtpClient.UseDefaultCredentials = true;
                        else
                            _smtpClient.Credentials = new System.Net.NetworkCredential(EmailUsername, EmailPassword);
                    }
                    if (!String.IsNullOrEmpty(EmailSsl))
                        _smtpClient.EnableSsl = true;  // Only .NET 4.0 and newer support EnableSsl in <system.net><mailSettings>

                    _smtpClient.SendCompleted += new SendCompletedEventHandler(smtp_SendCompleted);
                    _smtpClient.SendAsync(msg, msg);
                }
            }
        }

        void smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                System.Diagnostics.Trace.TraceError(Name + " previous smtp request was cancelled");
            }
            if (e.Error != null)
            {
                System.Diagnostics.Trace.TraceError(Name + " " + e.Error.Message);
                System.Diagnostics.Trace.WriteLine(Name + " previous smtp request failed");
                System.Diagnostics.Trace.WriteLine(Name + " " + e.Error.StackTrace);
            }

            if (!String.IsNullOrEmpty(EmailThrottleSeconds))
            {
                int throttleTimeSec = Int32.Parse(EmailThrottleSeconds);
                System.Threading.Thread.Sleep(throttleTimeSec * 1000);
            }

            lock (_bundledSources)
            {
                _smtpClient = null;

                if (_bundledMsgs.Count > 5)
                {
                    // (And x other events)
                    StringBuilder bodyBuilder = new StringBuilder();
                    foreach (BundledMessage bundleMsg in _bundledMsgs)
                    {
                        if (bodyBuilder.Length > 0)
                        {
                            bodyBuilder.AppendLine();
                            bodyBuilder.AppendLine();
                        }
                        bodyBuilder.AppendLine("Subject: " + bundleMsg.Subject);
                        bodyBuilder.AppendLine("---------------------------------------------------------");
                        bodyBuilder.AppendLine(bundleMsg.Body);
                        _bundledSources[bundleMsg.Source] = DateTime.UtcNow;
                    }
                    string subject = _bundledMsgs[0].Subject + String.Format(" (And {0} other events)", _bundledMsgs.Count-1);
                    _bundledMsgs.Clear();
                    SendTraceEmail(bodyBuilder.ToString(), subject);
                }
                else
                if (_bundledMsgs.Count != 0)
                {
                    // Just send the next event without bundling
                    foreach (BundledMessage bundleMsg in _bundledMsgs)
                        _bundledSources[bundleMsg.Source] = DateTime.UtcNow;
                    BundledMessage emailMsg = _bundledMsgs[0];
                    _bundledMsgs.RemoveAt(0);
                    SendTraceEmail(emailMsg.Body, emailMsg.Subject);
                }
            }
        }
    }
}
