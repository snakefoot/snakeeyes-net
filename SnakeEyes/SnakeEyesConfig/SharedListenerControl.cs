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
    public partial class SharedListenerControl : UserControl
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

        public SharedListenerControl()
        {
            InitializeComponent();
        }

        public SharedListenerControl(XmlDocument document, string name) : this()
        {
            Document = document;
            Id = name;

            if (Document.SelectSingleNode("/configuration") == null)
            {
                IsExample = true;
                XPath = String.Format("/examples/listeners/{0}/configuration", Id);
            }
            else
            {
                IsExample = false;
                XPath = String.Format("/configuration/system.diagnostics/sharedListeners/add[@name='{0}']", Id);
            }
        }

        protected string GetValue(string name)
        {
            return Node.Attributes[name]?.Value;
        }

        protected void SetValue(string key, string value)
        {
            if (Node.Attributes[key] == null)
            {
                XmlAttribute k = Document.CreateAttribute(key);
                k.Value = value;
                Node.Attributes.Append(k);
            }
            else
            {
                Node.Attributes[key].Value = value;
            }
        }

        protected void UpdateDataFromTextBox(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string keyName = textBox.Name.Substring(7, 1).ToLower() + textBox.Name.Substring(8);
            SetValue(keyName, textBox.Text);
        }

        protected void UpdateDataFromCheckBox(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string keyName = checkBox.Name.Substring(8, 1).ToLower() + checkBox.Name.Substring(8);
            string value = checkBox.Checked ? "true" : "";
            SetValue(keyName, value);
        }

        protected void UpdateFilterData(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string value = comboBox.SelectedItem.ToString();

            XmlNode node = Node.SelectSingleNode("filter");

            if (String.IsNullOrWhiteSpace(value))
            {
                if(node != null)
                {
                    Node.RemoveChild(node);
                }
            }
            else
            {
                XmlElement element = null;
                if (node == null)
                {
                    element = Document.CreateElement("filter");
                    Node.AppendChild(element);
                }
                else
                {
                    element = (XmlElement)node;
                }

                element.SetAttribute("type", "SnakeEyes.DelayStateFilter, DelayStateFilter");
                element.SetAttribute("initializeData", value);
            }
        }
    }
}
