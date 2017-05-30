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
    public partial class EmailTraceListenerControl : SharedListenerControl
    {
        public EmailTraceListenerControl(XmlDocument document, string id) : base(document, id)
        {
            InitializeComponent();

            textBoxFromAddress.Text = GetValue("fromAddress");
            textBoxToAddress.Text = GetValue("toAddress");
            textBoxFormatSubject.Text = GetValue("formatSubject");
            textBoxFormatBody.Text = GetValue("formatBody");
            textBoxHost.Text = GetValue("host");
            textBoxPort.Text = GetValue("port");
            textBoxUsername.Text = GetValue("username");
            textBoxPassword.Text = GetValue("password");
            textBoxSsl.Text = GetValue("ssl");
            textBoxFormatBundleSource.Text = GetValue("formatBundleSource");
        }

        new protected void UpdateDataFromTextBox(object sender, EventArgs e)
        {
            base.UpdateDataFromTextBox(sender, e);
        }

        new protected void UpdateFilterData(object sender, EventArgs e)
        {
            base.UpdateFilterData(sender, e);
        }
    }
}
