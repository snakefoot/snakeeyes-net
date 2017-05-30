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
    public partial class UnknownSharedListenerControl : SharedListenerControl
    {
        public UnknownSharedListenerControl(XmlDocument document, string id) : base(document, id)
        {
            InitializeComponent();

            textBoxCode.Text = Node.OuterXml;
        }
    }
}
