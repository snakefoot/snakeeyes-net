using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SnakeEyesConfig
{
    public partial class AddForm : Form
    {
        private AddType Type;
        private XmlDocument Document;
        private MainForm Main;
        private string BasePath;

        public AddForm(MainForm configuration, AddType type)
        {
            InitializeComponent();
            Type = type;
            Main = configuration;

            // Read examples from file
            string examplesPath = "examples.xml";
            KeyValueConfigurationElement examplesPathKey = configuration.Config.AppSettings.Settings["ExamplesPath"];
            if (examplesPathKey != null && !string.IsNullOrWhiteSpace(examplesPathKey.Value))
            {
                examplesPath = examplesPathKey.Value;
            }

            if (!File.Exists(examplesPath))
            {
                MessageBox.Show("Unable to load examples file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.BeginInvoke(new MethodInvoker(this.Close));
            }

            string xml_text = File.ReadAllText(examplesPath);

            try
            {
                Document = new XmlDocument();
                Document.LoadXml(xml_text);

                if (Type == AddType.Probe)
                {
                    Text = "Add Probe";
                    BasePath = "/examples/probes";
                }
                else if (Type == AddType.Listener)
                {
                    Text = "Add Listener";
                    BasePath = "/examples/listeners";
                }
                else
                {
                    Text = "Add Filter";
                    BasePath = "/examples/filters";
                }

                foreach (XmlNode node in Document.SelectNodes(String.Format("{0}/*", BasePath)))
                {
                    listBoxChoice.Items.Add(node.Name);
                }

                if (listBoxChoice.Items.Count == 0)
                {
                    this.BeginInvoke(new MethodInvoker(this.Close));
                }
            }
            catch
            {
                MessageBox.Show("Unable to load examples from file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.BeginInvoke(new MethodInvoker(this.Close));
            }
        }

        public enum AddType { Probe, Filter, Listener }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string originalName = listBoxChoice.SelectedItem.ToString();
            string newName = textBoxName.Text;
            XmlNode originalNode = Document.SelectSingleNode(String.Format("{0}/{1}/configuration", BasePath, originalName));

            XmlNode node;
            if (Type == AddType.Listener)
            {
                node = Document.CreateElement("add");
                node.InnerXml = originalNode.InnerXml;
                foreach (XmlAttribute attribute in originalNode.Attributes)
                {
                    ((XmlElement)node).SetAttribute(attribute.Name, attribute.Value);
                }
                ((XmlElement)node).SetAttribute("name", newName);
            }
            else
            {
                node = Document.CreateElement(newName);
                node.InnerXml = originalNode.InnerXml;
            }

            if (Type == AddType.Probe)
            {
                Main.AddProbe(node);
            }
            else if (Type == AddType.Listener)
            {
                Main.AddListener(node);
            }
            else
            {
                Main.AddFilter(node);
            }
            Main.RefreshListBoxes();
        }

        private void listBoxChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxChoice.SelectedItem == null) return;

            string name = listBoxChoice.SelectedItem.ToString();
            labelName.Text = name;
            labelDescription.Text = Document.SelectSingleNode(String.Format("{0}/{1}/description", BasePath, name)).InnerText;
            textBoxName.Text = name;

            Control panel = null;
            if (Type == AddType.Probe)
            {
                panel = Main.LoadProbeControl(Document, name);
            }
            else if (Type == AddType.Listener)
            {
                XmlNode node = Document.SelectSingleNode(String.Format("{0}/{1}/configuration", BasePath, name));
                string type = node.Attributes["type"]?.Value;
                panel = Main.LoadProbeControl(Document, name);
            }
            else
            {
                panel = Main.LoadFilterControl(Document, name);
            }

            panel.Dock = DockStyle.Fill;

            splitContainer3.Panel1.Controls.Clear();
            splitContainer3.Panel1.Controls.Add(panel);

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (listBoxChoice.SelectedItem == null || Type == AddType.Listener) return;

            string item = listBoxChoice.SelectedItem.ToString();
            string type = item.Substring(item.LastIndexOf('.'));
            if (!textBoxName.Text.EndsWith(type))
            {
                textBoxName.Text += type;
            }
        }
    }
}
