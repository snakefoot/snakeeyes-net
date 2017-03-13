using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace SnakeEyesClient
{
    public partial class ViewConfigForm : Form
    {
        ConfigManager _configManager;

        public ViewConfigForm()
        {
            InitializeComponent();
        }

        private void ViewConfigForm_Load(object sender, EventArgs e)
        {
            Configuration exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _configManager = new ConfigManager(exeConfiguration);

            foreach (ConfigItem listener in _configManager.ListenerList)
                _listenerList.Items.Add(listener.Name).SubItems.Add(listener.Type);

            foreach (ConfigItem probe in _configManager.ProbeList)
                _probesList.Items.Add(probe.Name).SubItems.Add(probe.Type);
        }
    }
}
