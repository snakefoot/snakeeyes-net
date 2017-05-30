using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace SnakeEyesConfig
{
    public partial class PingProbeControl : SnakeEyesConfig.KeyValueControl
    {
        public PingProbeControl(XmlDocument document, string id) : base(document, id)
        {
            InitializeComponent();

            comboBoxEventType.Items.Clear();
            comboBoxEventType.Items.Add("");
            foreach (var i in Enum.GetValues(typeof(TraceEventType)))
            {
                comboBoxEventType.Items.Add(i.ToString());
            }

            textBoxHostName.Text = GetValue("HostName");
            textBoxIpAddress.Text = GetValue("IpAddress");
            textBoxTimeoutMs.Text = GetValue("TimeoutMs");
            textBoxTTL.Text = GetValue("TTL");
            textBoxDontFragment.Text = GetValue("DontFragment");
            textBoxBuffersize.Text = GetValue("BufferSize");
            textBoxSampleCount.Text = GetValue("SampleCount");
            textBoxMaxValue.Text = GetValue("MaxValue");
            textBoxProbeFrequency.Text = GetValue("ProbeFrequency");
            textBoxEventId.Text = GetValue("EventId");
            comboBoxEventType.SelectedItem = GetValue("EventType") ?? "";
        }

        new protected void UpdateDataFromTextBox(object sender, EventArgs e)
        {
            base.UpdateDataFromTextBox(sender, e);
        }

        new protected void UpdateDataFromComboBox(object sender, EventArgs e)
        {
            base.UpdateDataFromComboBox(sender, e);
        }
    }
}
