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
    public partial class KeyValueControl : UserControl
    {
        protected string Id;
        protected XmlDocument Document;
        protected string XPath;
        protected bool IsExample;

        protected XmlNode Node
        {
            get
            {
                return Document.SelectSingleNode(XPath);
            }
        }

        public KeyValueControl()
        {
            InitializeComponent();
        }

        public KeyValueControl(XmlDocument document, string id) : this()
        {
            Document = document;
            Id = id;

            if (Document.SelectSingleNode("/configuration") == null)
            {
                IsExample = true;
                XPath = String.Format("/examples/*/{0}/configuration", Id);
            }
            else
            {
                IsExample = false;
                XPath = "/configuration/" + Id;
            }
        }

        protected string GetValue(string name)
        {
            XmlNode node = Node.SelectSingleNode(String.Format("add[@key='{0}']", name));
            if (node == null)
            {
                return null;
            }

            return node.Attributes["value"].Value;
        }

        protected void SetValue(string key, string value)
        {
            XmlNode node = Node.SelectSingleNode(String.Format("add[@key='{0}']", key));
            if (node == null && !String.IsNullOrEmpty(value))
            {
                XmlElement add = Document.CreateElement("add");
                add.SetAttribute("key", key);
                add.SetAttribute("value", value);

                Node.AppendChild(add);
            }
            else if (String.IsNullOrEmpty(value))
            {
                Node.RemoveChild(node);
            }
            else
            {
                node.Attributes["value"].Value = value;
            }
        }

        protected void UpdateDataFromTextBox(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string keyName = textBox.Name.Substring(7);
            SetValue(keyName, textBox.Text);
        }

        protected void UpdateDataFromComboBox(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string keyName = comboBox.Name.Substring(8);
            SetValue(keyName, comboBox.SelectedItem.ToString());
        }
    }
}
