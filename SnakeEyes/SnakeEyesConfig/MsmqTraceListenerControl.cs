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

namespace SnakeEyesConfig
{
    public partial class MsmqTraceListenerControl : SharedListenerControl
    {
        public MsmqTraceListenerControl(XmlDocument document, string id) : base(document, id)
        {
            InitializeComponent();

            textBoxQueueName.Text = GetValue("queueName");
            textBoxQueueLabel.Text = GetValue("queueLabel");
            checkBoxCreateQueue.Checked = String.IsNullOrWhiteSpace(GetValue("createQueue"));
            textBoxFormatLabel.Text = GetValue("formatLabel");
            textBoxFormatBody.Text = GetValue("formatBody");
        }

        new protected void UpdateDataFromTextBox(object sender, EventArgs e)
        {
            base.UpdateDataFromTextBox(sender, e);
        }

        new protected void UpdateDataFromCheckBox(object sender, EventArgs e)
        {
            base.UpdateDataFromCheckBox(sender, e);
        }

        new protected void UpdateFilterData(object sender, EventArgs e)
        {
            base.UpdateFilterData(sender, e);
        }
    }
}
