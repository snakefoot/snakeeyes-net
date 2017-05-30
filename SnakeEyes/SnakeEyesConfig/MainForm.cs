using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SnakeEyesConfig
{
    public partial class MainForm : Form
    {
        public XmlDocument XmlData { get; private set; }
        public Configuration Config;
        private string Template;
        public string FileName;

        public MainForm()
        {
            InitializeComponent();
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Config = ConfigurationManager.OpenExeConfiguration(assemblyPath);
            FileName = null;

            string[] tree = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnakeEyesConfig.template.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                Template = reader.ReadToEnd();
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Template);

            XmlData = xml;
            RefreshListBoxes();
        }

        // ==================================================================
        // Toolbar
        // ==================================================================
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(FileName))
            {
                DialogResult result = saveFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    FileName = saveFileDialog1.FileName;
                }
                else
                {
                    return;
                }
            }

            SaveToFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileName = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            SaveToFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;
                if (!FileName.ToLower().EndsWith(".config")) return;

                LoadData();
            }
        }

        // ==================================================================
        // Loading / saving data
        // ==================================================================
        private void SaveToFile()
        {
            SortDocument();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.Encoding = new UTF8Encoding(false); // The false means, do not emit the BOM.

            using (XmlWriter writer = XmlWriter.Create(FileName, settings))
            {
                XmlData.WriteTo(writer);
            }
        }

        public void LoadData()
        {
            string xml_text = File.ReadAllText(FileName, Encoding.GetEncoding(1252));

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xml_text);

            XmlData = xml;

            RefreshListBoxes();
            RefreshListenersProbesPanel();
        }

        internal void RefreshListBoxes()
        {
            listBoxProbes.Items.Clear();
            listBoxFilters.Items.Clear();
            listBoxListeners.Items.Clear();

            foreach (XmlNode a in XmlData.SelectNodes("/configuration/configSections/section"))
            {
                string name = a.Attributes["name"].Value;

                if (name.EndsWith("Probe"))
                {
                    listBoxProbes.Items.Add(name);
                }
                else if (name.EndsWith("Filter"))
                {
                    listBoxFilters.Items.Add(name);
                }
            }

            foreach (XmlNode a in XmlData.SelectNodes("/configuration/system.diagnostics/sharedListeners/add"))
            {
                string name = a.Attributes["name"].Value;
                listBoxListeners.Items.Add(name);
            }
        }

        // ==================================================================
        // Clicking lists
        // ==================================================================
        private void listBoxProbes_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshProbePanel();
        }

        private void listBoxListeners_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListenerPanel();
        }

        private void listBoxFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshFilterPanel();
        }

        // ==================================================================
        // Clicking tabs
        // ==================================================================
        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPageFilters)
            {
                RefreshFilterPanel();
            }
            else if (tabControl1.SelectedTab == tabPageProbes)
            {
                RefreshProbePanel();
            }
            else if (tabControl1.SelectedTab == tabPageListeners)
            {
                RefreshListenerPanel();
            }
            else if (tabControl1.SelectedTab == tabPageListenersProbes)
            {
                RefreshListenersProbesPanel();
            }
        }

        // ==================================================================
        // Refreshing panels
        // ==================================================================
        public void RefreshProbePanel()
        {
            if (listBoxProbes.SelectedItem == null) return;

            string name = listBoxProbes.SelectedItem.ToString();

            Control panel = LoadProbeControl(XmlData, name);
            panel.Dock = DockStyle.Fill;

            splitContainerProbes.Panel2.Controls.Clear();
            splitContainerProbes.Panel2.Controls.Add(panel);
        }

        public void RefreshListenerPanel()
        {
            if (listBoxListeners.SelectedItem == null) return;

            string name = listBoxListeners.SelectedItem.ToString();

            XmlNode node = XmlData.SelectSingleNode(String.Format("/configuration/system.diagnostics/sharedListeners/add[@name='{0}']", name));
            string type = node.Attributes["type"]?.Value;

            Control panel = LoadListenerControl(XmlData, name, type);
            panel.Dock = DockStyle.Fill;

            splitContainerListeners.Panel2.Controls.Clear();
            splitContainerListeners.Panel2.Controls.Add(panel);
        }

        public void RefreshFilterPanel()
        {
            if (listBoxFilters.SelectedItem == null) return;

            string name = listBoxFilters.SelectedItem.ToString();

            Control panel = LoadFilterControl(XmlData, name);
            panel.Dock = DockStyle.Fill;

            splitContainerFilters.Panel2.Controls.Clear();
            splitContainerFilters.Panel2.Controls.Add(panel);
        }

        public void RefreshListenersProbesPanel()
        {
            dataGridViewListenersProbes.Rows.Clear();
            dataGridViewListenersProbes.Columns.Clear();

            foreach (string listener in listBoxListeners.Items)
            {
                dataGridViewListenersProbes.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    HeaderText = listener,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    FalseValue = false,
                    TrueValue = true
                });
            }

            // In case we do have no listeners
            if (dataGridViewListenersProbes.Columns.Count == 0) return;

            foreach (string probe in listBoxProbes.Items)
            {
                int i = dataGridViewListenersProbes.Rows.Add();
                dataGridViewListenersProbes.Rows[i].HeaderCell.Value = probe;
                int col = 0;
                foreach (string listener in listBoxListeners.Items)
                {
                    XmlNode node = XmlData.SelectSingleNode(String.Format("/configuration/system.diagnostics/sources/source[@name='{0}']/listeners/add[@name='{1}']", probe, listener));
                    dataGridViewListenersProbes.Rows[i].Cells[col++].Value = node != null;
                }
            }
        }

        // ==================================================================
        // Loading correct control
        // ==================================================================
        internal Control LoadProbeControl(XmlDocument XmlData, string name)
        {
            if (name.EndsWith("FileProbe"))
            {
                return new FileProbeControl(XmlData, name);
            }
            else if (name.EndsWith("PerfMonProbe"))
            {
                return new PerfMonProbeControl(XmlData, name);
            }
            else if (name.EndsWith("PingProbe"))
            {
                return new PingProbeControl(XmlData, name);
            }
            else if (name.EndsWith("PowerShellProbe"))
            {
                return new PowerShellProbeControl(XmlData, name);
            }
            else
            {
                return new UnknownKeyValueControl(XmlData, name);
            }
        }

        internal Control LoadListenerControl(XmlDocument XmlData, string name, string type)
        {
            if (type.EndsWith("EmailTraceListener"))
            {
                return new EmailTraceListenerControl(XmlData, name);
            }
            if (type.EndsWith("MsmqTraceListener"))
            {
                return new MsmqTraceListenerControl(XmlData, name);
            }
            else
            {
                return new UnknownSharedListenerControl(XmlData, name);
            }
        }

        internal Control LoadFilterControl(XmlDocument XmlData, string name)
        {
            return new DelayStateFilterControl(XmlData, name);
        }

        // ==================================================================
        // Adding/Removing Probes
        // ==================================================================
        public void AddProbe(XmlNode originalNode)
        {
            XmlNode node = XmlData.ImportNode(originalNode, true);
            string name = node.Name;

            // Create entry in sections
            XmlElement sectionNode = XmlData.CreateElement("section");
            sectionNode.SetAttribute("name", name);
            sectionNode.SetAttribute("type", "System.Configuration.NameValueSectionHandler");
            XmlData.SelectSingleNode("/configuration/configSections").AppendChild(sectionNode);

            // Add the config (the original node)
            XmlData.SelectSingleNode("/configuration").AppendChild(node);

            XmlElement sourceNode = XmlData.CreateElement("source");
            sourceNode.SetAttribute("name", name);
            sourceNode.SetAttribute("switchValue", "Information");
            XmlElement listeners = XmlData.CreateElement("listeners");
            listeners.AppendChild(XmlData.CreateElement("clear"));
            sourceNode.AppendChild(listeners);
            XmlData.SelectSingleNode("/configuration/system.diagnostics/sources").AppendChild(sourceNode);
        }

        public void RemoveProbe(string name)
        {
            // Remove section
            XmlNode sectionNode = XmlData.SelectSingleNode(String.Format("/configuration/configSections/section[@name='{0}']", name));
            sectionNode.ParentNode.RemoveChild(sectionNode);

            // Remove config section
            XmlNode configNode = XmlData.SelectSingleNode(String.Format("/configuration/{0}", name));
            configNode.ParentNode.RemoveChild(configNode);

            // Remove from sources
            foreach (XmlNode node in XmlData.SelectNodes(String.Format("/configuration/system.diagnostics/sources/source[@name='{0}']", name)))
            {
                node.ParentNode.RemoveChild(node);
            }

            listBoxProbes.ClearSelected();
            listBoxProbes.Items.Remove(name);
            splitContainerProbes.Panel2.Controls.Clear();
        }

        // ==================================================================
        // Adding/Removing Filters
        // ==================================================================
        public void AddFilter(XmlNode originalNode)
        {
            XmlNode node = XmlData.ImportNode(originalNode, true);
            string name = node.Name;

            // Create entry in sections
            XmlElement sectionNode = XmlData.CreateElement("section");
            sectionNode.SetAttribute("name", name);
            sectionNode.SetAttribute("type", "System.Configuration.NameValueSectionHandler");
            XmlData.SelectSingleNode("/configuration/configSections").AppendChild(sectionNode);

            // Add the config (the original node)
            XmlData.SelectSingleNode("/configuration").AppendChild(node);
        }

        public void RemoveFilter(string name)
        {
            // Remove section
            XmlNode sectionNode = XmlData.SelectSingleNode(String.Format("/configuration/configSections/section[@name='{0}']", name));
            sectionNode.ParentNode.RemoveChild(sectionNode);

            // Remove config section
            XmlNode configNode = XmlData.SelectSingleNode(String.Format("/configuration/{0}", name));
            configNode.ParentNode.RemoveChild(configNode);

            // Remove from listeners
            foreach (XmlNode node in XmlData.SelectNodes(String.Format("/configuration/system.diagnostics/sharedListeners/add/filter[@initializeData='{0}']", name)))
            {
                node.ParentNode.RemoveChild(node);
            }

            listBoxFilters.ClearSelected();
            listBoxFilters.Items.Remove(name);
            splitContainerFilters.Panel2.Controls.Clear();
        }

        // ==================================================================
        // Adding/Removing Listeners
        // ==================================================================
        public void AddListener(XmlNode originalNode)
        {
            XmlNode node = XmlData.ImportNode(originalNode, true);

            // Add the config (the original node)
            XmlData.SelectSingleNode("/configuration/system.diagnostics/sharedListeners").AppendChild(node);
        }

        public void RemoveListener(string name)
        {
            // Remove section
            XmlNode sectionNode = XmlData.SelectSingleNode(String.Format("/configuration/system.diagnostics/sharedListeners/add[@name='{0}']", name));
            sectionNode.ParentNode.RemoveChild(sectionNode);

            // Remove from listeners
            foreach (XmlNode node in XmlData.SelectNodes(String.Format("/configuration/system.diagnostics/sources/source/listeners/add[@name='{0}']", name)))
            {
                node.ParentNode.RemoveChild(node);
            }

            listBoxListeners.ClearSelected();
            listBoxListeners.Items.Remove(name);
            splitContainerListeners.Panel2.Controls.Clear();
        }

        // ==================================================================
        // Sorting the lists
        // ==================================================================
        private void buttonUpProbe_Click(object sender, EventArgs e)
        {
            SortListBox(listBoxProbes, true);
        }

        private void buttonDownProbe_Click(object sender, EventArgs e)
        {
            SortListBox(listBoxProbes, false);
        }

        private void buttonUpFilter_Click(object sender, EventArgs e)
        {
            SortListBox(listBoxFilters, true);
        }

        private void buttonDownFilter_Click(object sender, EventArgs e)
        {
            SortListBox(listBoxFilters, false);
        }

        private void buttonUpListener_Click(object sender, EventArgs e)
        {
            SortListBox(listBoxListeners, true);
        }

        private void buttonDownListener_Click(object sender, EventArgs e)
        {
            SortListBox(listBoxListeners, false);
        }

        private void SortListBox(ListBox listBox, bool up)
        {
            // Prevent 
            if (listBox.SelectedItem == null) return;
            if (up && listBox.SelectedIndex == 0) return;
            if (!up && listBox.SelectedIndex == listBox.Items.Count - 1) return;

            int position = listBox.SelectedIndex;
            int newPosition = up ? position - 1 : position + 1;
            object selected = listBox.Items[position];

            listBox.Items.RemoveAt(position);
            listBox.Items.Insert(newPosition, selected);

            listBox.SelectedIndex = newPosition;
        }

        // ==================================================================
        // Sort the document
        // ==================================================================
        private void SortDocument()
        {
            // We will always sort this way:
            //  configSections (Filters, Probes), Filters, Probes, diagnostics (sources, Listeners), rest (a-z)
            List<string> globalSortList = new List<string> { "appSettings", "configSections" };
            foreach (string item in listBoxFilters.Items)
            {
                globalSortList.Add(item);
            }
            foreach (string item in listBoxProbes.Items)
            {
                globalSortList.Add(item);
            }
            globalSortList.Add("system.diagnostics");
            globalSortList.Reverse();

            List<string> listenerList = new List<string>();
            foreach (string item in listBoxListeners.Items)
            {
                listenerList.Add(item);
            }
            listenerList.Reverse();

            // Sort root node
            XmlNode configurationNode = XmlData.SelectSingleNode("/configuration");
            foreach (string name in globalSortList)
            {
                XmlNode node = configurationNode.SelectSingleNode(name);
                if (node == null) continue;
                configurationNode.RemoveChild(node);
                configurationNode.PrependChild(node);
            }

            // Sort configSectionNode
            XmlNode configSectionNode = XmlData.SelectSingleNode("/configuration/configSections");
            foreach (string name in globalSortList)
            {
                XmlNode node = configSectionNode.SelectSingleNode(String.Format("section[@name='{0}']", name));
                if (node == null) continue;
                configSectionNode.RemoveChild(node);
                configSectionNode.PrependChild(node);
            }

            // Sort system.diagnostics/sources
            XmlNode sourcesNode = XmlData.SelectSingleNode("/configuration/system.diagnostics/sources");
            foreach (string name in globalSortList)
            {
                XmlNode node = sourcesNode.SelectSingleNode(String.Format("source[@name='{0}']", name));
                if (node == null) continue;
                sourcesNode.RemoveChild(node);
                sourcesNode.PrependChild(node);

                foreach(string listener in listenerList)
                {
                    XmlNode listenerNode = node.SelectSingleNode(String.Format("add[@name='{0}']", name));
                    if (listenerNode == null) continue;
                    node.RemoveChild(listenerNode);
                    node.PrependChild(listenerNode);
                }
                XmlNode clearNode = node.SelectSingleNode("clear");
                if (clearNode == null) continue;
                node.RemoveChild(clearNode);
                node.PrependChild(clearNode);
            }

            XmlNode sharedListenersNode = XmlData.SelectSingleNode("/configuration/system.diagnostics/sharedListeners");
            foreach (string name in listenerList)
            {
                XmlNode node = sharedListenersNode.SelectSingleNode(String.Format("add[@name='{0}']", name));
                if (node == null) continue;
                sharedListenersNode.RemoveChild(node);
                sharedListenersNode.PrependChild(node);
            }
        }

        // ==================================================================
        // Misc
        // ==================================================================
        private void MainForm_Resize(object sender, EventArgs e)
        {
            tabControl1.Height = Height - 66;

            TabPage shownTabPage = tabControl1.SelectedTab;
            splitContainerFilters.Height = shownTabPage.Height - 23;
            splitContainerListeners.Height = shownTabPage.Height - 23;
            splitContainerProbes.Height = shownTabPage.Height - 23;
            dataGridViewListenersProbes.Height = shownTabPage.Height - 23;

            int panelHeight = splitContainerFilters.Panel1.Height;

            listBoxListeners.Height = panelHeight - 33;
            listBoxProbes.Height = panelHeight - 33;
            listBoxFilters.Height = panelHeight - 33;
        }

        private void buttonDeleteProbe_Click(object sender, EventArgs e)
        {
            if (listBoxProbes.SelectedItem == null) return;
            string name = listBoxProbes.SelectedItem.ToString();
            RemoveProbe(name);
        }

        private void buttonDeleteListener_Click(object sender, EventArgs e)
        {
            if (listBoxListeners.SelectedItem == null) return;
            string name = listBoxListeners.SelectedItem.ToString();
            RemoveListener(name);
        }

        private void buttonDeleteFilter_Click(object sender, EventArgs e)
        {
            if (listBoxFilters.SelectedItem == null) return;
            string name = listBoxFilters.SelectedItem.ToString();
            RemoveFilter(name);
        }

        private void buttonAddProbe_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm(this, AddForm.AddType.Probe);
            addForm.ShowDialog();
        }

        private void buttonAddListener_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm(this, AddForm.AddType.Listener);
            addForm.ShowDialog();
        }

        private void buttonAddFilter_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm(this, AddForm.AddType.Filter);
            addForm.ShowDialog();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = null;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Template);

            XmlData = xml;
            RefreshListBoxes();
        }

        private void dataGridViewListenersProbes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            DataGridViewRow row = dataGridViewListenersProbes.Rows[e.RowIndex];
            DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)row.Cells[e.ColumnIndex];

            string probe = listBoxProbes.Items[e.RowIndex].ToString();
            string listener = listBoxListeners.Items[e.ColumnIndex].ToString();

            // INFO: at this point the checkbox still has its old value!
            bool active = !(bool)cell.Value;

            XmlNode node = XmlData.SelectSingleNode(String.Format("/configuration/system.diagnostics/sources/source[@name='{0}']/listeners/add[@name='{1}']", probe, listener));

            if (active && node == null)
            {
                XmlElement addNode = XmlData.CreateElement("add");
                addNode.SetAttribute("name", listener);
                XmlNode listeners = XmlData.SelectSingleNode(String.Format("/configuration/system.diagnostics/sources/source[@name='{0}']/listeners", probe));
                listeners.AppendChild(addNode);
            }
            else if (!active && node != null)
            {
                node.ParentNode.RemoveChild(node);
            }
        }
    }
}
