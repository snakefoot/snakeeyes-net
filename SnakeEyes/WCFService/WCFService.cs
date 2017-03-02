using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace SnakeEyes
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WCFService : TraceListener, IProbeMonitor, IProbeMonitorService, IProbeStatusService, IFileHostService
    {
        TraceSource _traceSource = null;
        ServiceHost _crossDomainserviceHost = null;
        ServiceHost _clientServiceHost = null;

        ProbeCollection _probes = new ProbeCollection();
        Dictionary<string, ProbeHistory> _probeStates = new Dictionary<string, ProbeHistory>();

        public new void Dispose()
        {
            base.Dispose();
            if (_crossDomainserviceHost != null && _crossDomainserviceHost is IDisposable)
            {
                ((IDisposable)_crossDomainserviceHost).Dispose();
                _crossDomainserviceHost = null;
            }
            if (_clientServiceHost != null && _clientServiceHost is IDisposable)
            {
                ((IDisposable)_clientServiceHost).Dispose();
                _clientServiceHost = null;
            }
        }

        public TraceSource StartMonitor(string configName)
        {
            _traceSource = new TraceSource(configName);

            //_crossDomainserviceHost = new ServiceHost(typeof(CrossDomainService));
            //_crossDomainserviceHost.Faulted += new EventHandler(host_Faulted);
            //_crossDomainserviceHost.Description.Behaviors.Add(new WCFErrorServiceBehavior(_traceSource));
            //_crossDomainserviceHost.Open();

            //_clientServiceHost = new ServiceHost(this);
            //_clientServiceHost.Faulted += new EventHandler(host_Faulted);
            //_clientServiceHost.Description.Behaviors.Add(new WCFErrorServiceBehavior(_traceSource));
            //_clientServiceHost.Open();

            return _traceSource;
        }

        void host_Faulted(object sender, EventArgs e)
        {
            if (_traceSource != null)
                _traceSource.TraceEvent(TraceEventType.Critical, 0, "ClientService ServiceHost Faulted");
            else
                Trace.WriteLine("ClientService ServiceHost Faulted");
        }

        public void RegisterProbe(TraceSource probe)
        {
            probe.Listeners.Add(this);

            ProbeInfo probeInfo = new ProbeInfo();
            probeInfo.Name = probe.Name;
            if (_probes.Probes == null)
                _probes.Probes = new List<ProbeInfo>();
            _probes.Probes.Add(probeInfo);

            ProbeHistory probeHistory = new ProbeHistory();
            probeHistory.Name = probe.Name;
            probeHistory.History = new List<ProbeState>();
            _probeStates.Add(probe.Name, probeHistory);
        }

        public Stream CurrentStatus()
        {
            // Open "Status.html" and let javascript query for nodes and history (GWT ?)

            // Implement Search and Replace framework for filling into html-template
            MemoryStream stream = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(stream);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteRaw(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">");
            {
                xmlWriter.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
                {
                    xmlWriter.WriteStartElement("head");
                    xmlWriter.WriteElementString("title", "SnakeEyes Status");
                    xmlWriter.WriteStartElement("link");
                    xmlWriter.WriteAttributeString("rel", "stylesheet");
                    xmlWriter.WriteAttributeString("href", "../files/status.css");
                    xmlWriter.WriteAttributeString("type", "text/css");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                {
                    xmlWriter.WriteStartElement("body");
                    foreach (ProbeInfo probe in _probes.Probes)
                    {
                        xmlWriter.WriteStartElement("div");
                        xmlWriter.WriteAttributeString("class", "probeInfo");
                        xmlWriter.WriteAttributeString("id", probe.Name);

                        {
                            xmlWriter.WriteStartElement("div");
                            xmlWriter.WriteAttributeString("class", "probeName");
                            xmlWriter.WriteValue(probe.Name);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteStartElement("div");
                            xmlWriter.WriteAttributeString("class", "probeLastStatus");
                            if (probe.Status != null)
                                xmlWriter.WriteValue(probe.Status);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteStartElement("div");
                            xmlWriter.WriteAttributeString("class", "probeLastEventId");
                            xmlWriter.WriteValue(probe.EventId);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteStartElement("div");
                            xmlWriter.WriteAttributeString("class", "probeLastMessage");
                            if (probe.Message != null)
                                xmlWriter.WriteValue(probe.Message);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteStartElement("div");
                            xmlWriter.WriteAttributeString("class", "probeLastUpdate");
                            if (probe.Timestamp != null)
                                xmlWriter.WriteValue(probe.Timestamp);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteStartElement("div");
                            xmlWriter.WriteAttributeString("class", "probeLastValue");
                            if (probe.Value != null)
                                xmlWriter.WriteValue(probe.Value);
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("div");
                        xmlWriter.WriteAttributeString("class", "probeHistory");
                        xmlWriter.WriteAttributeString("id", probe.Name);
                        {
                            ProbeHistory probeHistory = GetProbeHistory(probe.Name);
                            if (probeHistory != null)
                            {
                                foreach (ProbeState probeEvent in probeHistory.History)
                                {
                                    xmlWriter.WriteStartElement("div");
                                    xmlWriter.WriteAttributeString("class", "probeState");
                                    xmlWriter.WriteAttributeString("id", probe.Timestamp);

                                    {
                                        xmlWriter.WriteStartElement("div");
                                        xmlWriter.WriteAttributeString("class", "probeStatus");
                                        if (probeEvent.Status != null)
                                            xmlWriter.WriteValue(probeEvent.Status);
                                        xmlWriter.WriteEndElement();

                                        xmlWriter.WriteStartElement("div");
                                        xmlWriter.WriteAttributeString("class", "probeEventId");
                                        xmlWriter.WriteValue(probeEvent.EventId);
                                        xmlWriter.WriteEndElement();

                                        xmlWriter.WriteStartElement("div");
                                        xmlWriter.WriteAttributeString("class", "probeMessage");
                                        if (probeEvent.Message != null)
                                            xmlWriter.WriteValue(probeEvent.Message);
                                        xmlWriter.WriteEndElement();

                                        xmlWriter.WriteStartElement("div");
                                        xmlWriter.WriteAttributeString("class", "probeTimestamp");
                                        if (probeEvent.Timestamp != null)
                                            xmlWriter.WriteValue(probeEvent.Timestamp);
                                        xmlWriter.WriteEndElement();

                                        xmlWriter.WriteStartElement("div");
                                        xmlWriter.WriteAttributeString("class", "probeValue");
                                        if (probeEvent.Value != null)
                                            xmlWriter.WriteValue(probeEvent.Value);
                                        xmlWriter.WriteEndElement();
                                    }

                                    xmlWriter.WriteEndElement();
                                }
                            }
                        }
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();

            stream.Position = 0;

            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return stream;
        }

        public Stream Files(string filename)
        {
            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            filepath = System.IO.Path.GetDirectoryName(filepath);
            filepath = Path.Combine(filepath, "wwwroot");
            filepath = Path.Combine(filepath, filename);    // Note protect against evil paths like ".."

            Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //Set the correct context type for the file requested.
            if (WebOperationContext.Current != null)
            {
                int extIndex = filename.LastIndexOf(".");
                string extension = filename.Substring(extIndex, filename.Length - extIndex);
                switch (extension)
                {
                    case ".html":
                    case ".htm":
                        WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                        break;
                    case ".css":
                        WebOperationContext.Current.OutgoingResponse.ContentType = "text/css";
                        break;
                    case "*.js":
                        WebOperationContext.Current.OutgoingResponse.ContentType = "text/javascript";
                        break;
                    case ".xap":
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/x-silverlight-2-b2";
                        break;
                    default:
                        throw (new ApplicationException("File type not supported"));
                }
            }

            return stream;
        }

        public ProbeCollection GetProbeCollection()
        {
            return _probes;
        }

        public ProbeHistory GetProbeHistory(string name)
        {
            ProbeHistory probeHistory;
            if (_probeStates.TryGetValue(name, out probeHistory))
                return probeHistory;
            else
                return null;
        }

        void AddProbeState(ProbeState probeState, ref ProbeHistory probeHistory)
        {
            probeHistory.History.Insert(0, probeState);
            probeHistory.History.RemoveAll(delegate(ProbeState state) { return DateTime.Now.Subtract(DateTime.Parse(state.Timestamp)) > TimeSpan.FromDays(1); });
            string name = probeHistory.Name;
            ProbeInfo probeInfo = _probes.Probes.Find(delegate(ProbeInfo probe) { return probe.Name == name; });
            if (probeInfo != null)
                probeInfo.UpdateState(probeState);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            ProbeHistory probeHistory;
            if (_probeStates.TryGetValue(source, out probeHistory))
            {
                ProbeState probeState = new ProbeState();
                probeState.EventId = id;
                if (data != null)
                    probeState.Message = data.ToString();
                probeState.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                probeState.Status = eventType.ToString();
                AddProbeState(probeState, ref probeHistory);
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            ProbeHistory probeHistory;
            if (_probeStates.TryGetValue(source, out probeHistory))
            {
                ProbeState probeState = new ProbeState();
                probeState.EventId = id;
                if (data != null && data.Length > 0)
                    probeState.Message = data[0].ToString();
                probeState.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                probeState.Status = eventType.ToString();
                AddProbeState(probeState, ref probeHistory);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            ProbeHistory probeHistory;
            if (_probeStates.TryGetValue(source, out probeHistory))
            {
                ProbeState probeState = new ProbeState();
                probeState.EventId = id;
                probeState.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                probeState.Status = eventType.ToString();
                AddProbeState(probeState, ref probeHistory);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            ProbeHistory probeHistory;
            if (_probeStates.TryGetValue(source, out probeHistory))
            {
                string value = "";
                try
                {
                    using (System.IO.StringReader reader = new System.IO.StringReader(message))
                    {
                        System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(reader);
                        System.Xml.XPath.XPathNavigator xmlNav = xmlDoc.CreateNavigator();
                        if (eventType != TraceEventType.Critical)
                            value = xmlNav.SelectSingleNode("//Value").InnerXml;
                    }
                }
                catch (Exception)
                {
                }

                string eventMessage = message;
                try
                {
                    using (System.IO.StringReader reader = new System.IO.StringReader(message))
                    {
                        System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(reader);
                        System.Xml.XPath.XPathNavigator xmlNav = xmlDoc.CreateNavigator();
                        eventMessage = xmlNav.SelectSingleNode("//Message").InnerXml;
                    }
                }
                catch (Exception)
                {
                }

                ProbeState probeState = new ProbeState();
                probeState.EventId = id;
                probeState.Message = eventMessage;
                probeState.Value = value;
                probeState.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                probeState.Status = eventType.ToString();
                AddProbeState(probeState, ref probeHistory);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            ProbeHistory probeHistory;
            if (_probeStates.TryGetValue(source, out probeHistory))
            {
                ProbeState probeState = new ProbeState();
                probeState.EventId = id;
                if (format != null && args != null)
                    probeState.Message = String.Format(format, args);
                else
                    probeState.Message = format;
                probeState.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                probeState.Status = eventType.ToString();
                AddProbeState(probeState, ref probeHistory);
            }
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
            ProbeHistory probeHistory;
            if (!_probeStates.TryGetValue("(Console)", out probeHistory))
            {
                ProbeInfo probeInfo = new ProbeInfo();
                probeInfo.Name = "(Console)";
                if (_probes.Probes == null)
                    _probes.Probes = new List<ProbeInfo>();
                _probes.Probes.Add(probeInfo);

                probeHistory = new ProbeHistory();
                probeHistory.Name = "(Console)";
                probeHistory.History = new List<ProbeState>();
                _probeStates.Add(probeHistory.Name, probeHistory);
            }

            ProbeState probeState = new ProbeState();
            probeState.Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            probeState.Message = message;
            AddProbeState(probeState, ref probeHistory);
        }
    }
}
