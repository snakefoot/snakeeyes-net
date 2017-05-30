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
    public partial class DelayStateFilterControl : SnakeEyesConfig.KeyValueControl
    {
        public DelayStateFilterControl(XmlDocument document, string id) : base(document, id)
        {
            InitializeComponent();
            textBoxNextTriggerTime.Text = GetValue("NextTriggerTime");
            textBoxDelayTriggerTime.Text = GetValue("DelayTriggerTime");
        }

        new private void UpdateDataFromTextBox(object sender, EventArgs e)
        {
            base.UpdateDataFromTextBox(sender, e);
        }
    }
}
