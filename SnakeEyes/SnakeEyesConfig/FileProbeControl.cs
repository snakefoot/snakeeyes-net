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
    public partial class FileProbeControl : KeyValueControl
    {
        public FileProbeControl(XmlDocument document, string id) : base(document, id)
        {
            InitializeComponent();

            comboBoxEventType.Items.Clear();
            comboBoxEventType.Items.Add("");
            foreach (var i in Enum.GetValues(typeof(TraceEventType)))
            {
                comboBoxEventType.Items.Add(i.ToString());
            }

            textBoxFileName.Text = GetValue("CategoryName");
            textBoxMaxFileSize.Text = GetValue("CounterName");
            textBoxMaxFileAge.Text = GetValue("InstanceName");
            textBoxDefaultFileSize.Text = GetValue("ServiceName");
            textBoxDefaultFileAge.Text = GetValue("MaxValue");
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
