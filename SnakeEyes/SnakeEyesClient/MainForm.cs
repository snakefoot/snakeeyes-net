using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnakeEyes;

namespace SnakeEyesClient
{
    public partial class MainForm : Form
    {
        delegate void UniversalVoidDelegate();
        ProbeList _probes = new ProbeList();
        ProbeEventList _probeEvents = new ProbeEventList();

        public MainForm()
        {
            InitializeComponent();

            _probeEvents.ProbeEventMaxCount = 4;
            _probeEvents.GetProbeNames().CollectionChanged += ProbeNames_CollectionChanged;
        }

        void ProbeNames_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _probeEventList.Invoke(new UniversalVoidDelegate(() =>
            {
                _probeNameList.BeginUpdate();
                _probeNameList.Items.Clear();
                foreach (string probeName in _probeEvents.GetProbeNames())
                {
                    _probeNameList.Items.Add(probeName);
                }
                _probeNameList.EndUpdate();
                _probeNameList.Invalidate();
            }));
        }

        private void WriteTraceEvent(string source, TraceEventType eventType, int eventId, string message)
        {
            if (string.IsNullOrEmpty(source))
                source = "(event)";
            DateTime time = DateTime.Now;
            ListViewItem item = new ListViewItem();
            item.Text = message;
            item.SubItems.Add(time.ToString("g"));
            item.SubItems.Add(source);
            item.SubItems.Add(eventType.ToString());
            item.SubItems.Add(eventId.ToString());
            _probeEvents.AddEvent(source, time, eventType, message, eventId);
            _probeEventList.Invoke(new UniversalVoidDelegate(() => _probeEventList.Items.Add(item)));
            if (eventType < TraceEventType.Information)
                Invoke(new UniversalVoidDelegate(() =>
                    {
                        if (_trayIcon.Visible && _trayIcon.Icon != Properties.Resources.redEye)
                        { 
                            _trayIcon.Icon = Properties.Resources.redEye;
                            _trayIcon.ShowBalloonTip(3, source, string.Format("{0} - {1}", source, message), ToolTipIcon.Info);
                        }
                    }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var logView = new LogViewListener(WriteTraceEvent);
            Trace.Listeners.Add(logView);
            Trace.AutoFlush = true;

            Configuration exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSection diagnosticsSection = exeConfiguration.GetSection("system.diagnostics");

            ConfigurationElementCollection tracesources = diagnosticsSection.ElementInformation.Properties["sources"].Value as ConfigurationElementCollection;

            List<KeyValuePair<string, IProbeMonitor>> probeMonitors = new List<KeyValuePair<string, IProbeMonitor>>();
            foreach (TraceListener traceListener in Trace.Listeners)
            {
                IProbeMonitor probeMonitor = traceListener as IProbeMonitor;
                if (probeMonitor != null)
                {
                    Trace.WriteLine("Configures ProbeMonitor: " + traceListener.Name);
                    probeMonitors.Add(new KeyValuePair<string, IProbeMonitor>(traceListener.Name, probeMonitor));
                }
            }

            var builder = new Autofac.Builder.ContainerBuilder();
            builder.RegisterCollection<IProbe>().As<IEnumerable<IProbe>>();
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var dynamicProbes = ProbeTypeLoader.LoadList(typeof(IProbe), path);
            dynamicProbes.AddRange(ProbeTypeLoader.LoadList(typeof(IProbe), System.IO.Path.Combine(path, "Probes")));
            foreach (var probeType in dynamicProbes)
            {
                builder.Register(probeType).As<IProbe>().ExternallyOwned().FactoryScoped().Named(probeType.FullName).MemberOf<IEnumerable<IProbe>>();
                //builder.Register<PerfMonProbe>().As<IProbe>().ExternallyOwned().FactoryScoped().Named(typeof(PerfMonProbe).FullName).MemberOf<IEnumerable<IProbe>>();
                //builder.Register<EventLogProbe>().As<IProbe>().ExternallyOwned().FactoryScoped().Named(typeof(EventLogProbe).FullName).MemberOf<IEnumerable<IProbe>>();
                //builder.Register<PingProbe>().As<IProbe>().ExternallyOwned().FactoryScoped().Named(typeof(PingProbe).FullName).MemberOf<IEnumerable<IProbe>>();
            }

            var container = builder.Build();
            Microsoft.Practices.ServiceLocation.IServiceLocator locator = new AutofacServiceLocator(container);

            foreach (ConfigurationElement tracesource in tracesources)
            {
                string name = tracesource.ElementInformation.Properties["name"].Value.ToString();
                Trace.WriteLine("Configures TraceSource: " + name);

                if (name.IndexOf('.') != -1)
                {
                    string logicalName = name.Substring(0, name.IndexOf('.'));
                    string typeName = name.Substring(name.IndexOf('.') + 1);

                    IProbe probe = locator.GetInstance<IProbe>("SnakeEyes." + typeName);
                    if (probe == null)
                    {
                        Trace.WriteLine("Unknown Probe Type: " + typeName);
                        continue;
                    }
                    TraceSource probeSource = probe.ConfigureProbe(name);
                    probeSource.Listeners.Add(logView);

                    foreach (KeyValuePair<string, IProbeMonitor> probeMonitor in probeMonitors)
                        probeMonitor.Value.RegisterProbe(probeSource);

                    _probes.AddProbe(DateTime.MinValue, probe);
                }
            }

            if (_probes.Count == 0)
            {
                Trace.WriteLine("SnakeEyes have no probes to poll");
                return;
            }

            Trace.WriteLine("SnakeEyes starts monitoring...");

            foreach (KeyValuePair<string, IProbeMonitor> probeMonitor in probeMonitors)
                probeMonitor.Value.StartMonitor(probeMonitor.Key);

            _probeTimer.Enabled = true;
        }

        private void _probeTimer_Tick(object sender, EventArgs e)
        {
            while (_probes.GetNextPollTime() <= DateTime.UtcNow)
            {
                IProbe probe = _probes.GetNextPollProbe();
                try
                {
                    TimeSpan nextPollTime = probe.ExecuteProbe();
                    _probes.PollExecuted(nextPollTime, probe);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Probe Failure: " + ex.Message);
                    if (ex.InnerException != null)
                        System.Diagnostics.Trace.TraceError("Probe Failure: " + ex.InnerException.Message);
                    System.Diagnostics.Trace.WriteLine("Probe Failure: " + ex.StackTrace);

                    _probes.PollExecuted(TimeSpan.FromSeconds(5), probe);
                }
            }
        }

        private void _probeNameList_DoubleClick(object sender, EventArgs e)
        {
            ViewConfigForm viewConfig = new ViewConfigForm();
            viewConfig.ShowDialog(this);
        }

        void SingleProbeEvents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _singleProbeEventList.Invoke(new UniversalVoidDelegate(() =>
            {
                _singleProbeEventList.BeginUpdate();
                _singleProbeEventList.Items.Clear();
                var probeEvents = (ObservableCollection<ProbeEvent>)_singleProbeEventList.Tag;
                foreach (var probeEvent in probeEvents)
                {
                    DateTime time = DateTime.Now;
                    ListViewItem item = new ListViewItem();
                    item.Text = probeEvent.Message;
                    item.SubItems.Add(probeEvent.Timestamp.ToString("g"));
                    item.SubItems.Add(probeEvent.EventType.ToString());
                    item.SubItems.Add(probeEvent.EventId.ToString());
                    _singleProbeEventList.Items.Add(item);
                }
                _singleProbeEventList.EndUpdate();
                _singleProbeEventList.Invalidate();
            }));
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Visible = false;
                _trayIcon.Visible = true;
                _trayIcon.Icon = Properties.Resources.greenEye;
                _trayIcon.ShowBalloonTip(3, "Minimized to tray", "Double click the system tray icon to restore window", ToolTipIcon.Info);
            }
        }

        private void _trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            _trayIcon.Visible = false;
        }

        private void _probeNameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_probeNameList.SelectedItems.Count > 0)
            {
                if (_singleProbeEventList.Tag != null)
                    ((ObservableCollection<ProbeEvent>)_singleProbeEventList.Tag).CollectionChanged -= SingleProbeEvents_CollectionChanged;
                var probeEvents = _probeEvents.GetEvents(_probeNameList.SelectedItems[0].Text);
                _singleProbeEventList.Tag = probeEvents;
                probeEvents.CollectionChanged += SingleProbeEvents_CollectionChanged;
                SingleProbeEvents_CollectionChanged(probeEvents, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
