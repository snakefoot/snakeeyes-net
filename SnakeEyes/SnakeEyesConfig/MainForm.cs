using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SnakeEyes
{
    public partial class MainForm : Form
    {
        ProbeFactory _probeFactory;
        ConfigManager _configManager;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Iterate over all dlls found in current directory + Probes directory
            // - Check if they implement the known interfaces (IProbe / IProbeMonitor / TraceFilter / TraceListener)
            // - TraceListener + TraceFilter requires that the IProbeConfig tells what assembly-type it supports

            // First load all available trace functionality
            // Second load all available GUI config functionality
            // Third when needing to configure a trace option, then check if GUI config is available
            // Fouth need to allow that one have edited the app.config directly
            _probeFactory = new ProbeFactory();

            Configuration exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _configManager = new ConfigManager(exeConfiguration);

            foreach (ConfigItem listener in _configManager.ListenerList)
                _listenerList.Items.Add(listener.Name).SubItems.Add(listener.Type);

            foreach (ConfigItem probe in _configManager.ProbeList)
                _probesList.Items.Add(probe.Name).SubItems.Add(probe.Type);
        }
    }
}
