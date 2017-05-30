namespace SnakeEyesConfig
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabPageFilters = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainerFilters = new System.Windows.Forms.SplitContainer();
            this.buttonDownFilter = new System.Windows.Forms.Button();
            this.buttonUpFilter = new System.Windows.Forms.Button();
            this.buttonDeleteFilter = new System.Windows.Forms.Button();
            this.buttonAddFilter = new System.Windows.Forms.Button();
            this.listBoxFilters = new System.Windows.Forms.ListBox();
            this.tabPageListeners = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainerListeners = new System.Windows.Forms.SplitContainer();
            this.buttonDownListener = new System.Windows.Forms.Button();
            this.buttonUpListener = new System.Windows.Forms.Button();
            this.buttonDeleteListener = new System.Windows.Forms.Button();
            this.buttonAddListener = new System.Windows.Forms.Button();
            this.listBoxListeners = new System.Windows.Forms.ListBox();
            this.tabPageProbes = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainerProbes = new System.Windows.Forms.SplitContainer();
            this.buttonDownProbe = new System.Windows.Forms.Button();
            this.buttonUpProbe = new System.Windows.Forms.Button();
            this.buttonDeleteProbe = new System.Windows.Forms.Button();
            this.buttonAddProbe = new System.Windows.Forms.Button();
            this.listBoxProbes = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageListenersProbes = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridViewListenersProbes = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.tabPageFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFilters)).BeginInit();
            this.splitContainerFilters.Panel1.SuspendLayout();
            this.splitContainerFilters.SuspendLayout();
            this.tabPageListeners.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerListeners)).BeginInit();
            this.splitContainerListeners.Panel1.SuspendLayout();
            this.splitContainerListeners.SuspendLayout();
            this.tabPageProbes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProbes)).BeginInit();
            this.splitContainerProbes.Panel1.SuspendLayout();
            this.splitContainerProbes.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageListenersProbes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewListenersProbes)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(885, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.loadToolStripMenuItem.Text = "Open";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "exe.config";
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "exe.config";
            // 
            // tabPageFilters
            // 
            this.tabPageFilters.Controls.Add(this.label2);
            this.tabPageFilters.Controls.Add(this.splitContainerFilters);
            this.tabPageFilters.Location = new System.Drawing.Point(4, 22);
            this.tabPageFilters.Name = "tabPageFilters";
            this.tabPageFilters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilters.Size = new System.Drawing.Size(877, 492);
            this.tabPageFilters.TabIndex = 2;
            this.tabPageFilters.Text = "Filters";
            this.tabPageFilters.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Haven\'t got a clue what filters are for.";
            // 
            // splitContainerFilters
            // 
            this.splitContainerFilters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainerFilters.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerFilters.Location = new System.Drawing.Point(3, 20);
            this.splitContainerFilters.Name = "splitContainerFilters";
            // 
            // splitContainerFilters.Panel1
            // 
            this.splitContainerFilters.Panel1.Controls.Add(this.buttonDownFilter);
            this.splitContainerFilters.Panel1.Controls.Add(this.buttonUpFilter);
            this.splitContainerFilters.Panel1.Controls.Add(this.buttonDeleteFilter);
            this.splitContainerFilters.Panel1.Controls.Add(this.buttonAddFilter);
            this.splitContainerFilters.Panel1.Controls.Add(this.listBoxFilters);
            this.splitContainerFilters.Size = new System.Drawing.Size(871, 469);
            this.splitContainerFilters.SplitterDistance = 290;
            this.splitContainerFilters.TabIndex = 0;
            // 
            // buttonDownFilter
            // 
            this.buttonDownFilter.Location = new System.Drawing.Point(217, 4);
            this.buttonDownFilter.Name = "buttonDownFilter";
            this.buttonDownFilter.Size = new System.Drawing.Size(68, 23);
            this.buttonDownFilter.TabIndex = 8;
            this.buttonDownFilter.Text = "Down";
            this.buttonDownFilter.UseVisualStyleBackColor = true;
            this.buttonDownFilter.Click += new System.EventHandler(this.buttonDownFilter_Click);
            // 
            // buttonUpFilter
            // 
            this.buttonUpFilter.Location = new System.Drawing.Point(146, 4);
            this.buttonUpFilter.Name = "buttonUpFilter";
            this.buttonUpFilter.Size = new System.Drawing.Size(68, 23);
            this.buttonUpFilter.TabIndex = 7;
            this.buttonUpFilter.Text = "Up";
            this.buttonUpFilter.UseVisualStyleBackColor = true;
            this.buttonUpFilter.Click += new System.EventHandler(this.buttonUpFilter_Click);
            // 
            // buttonDeleteFilter
            // 
            this.buttonDeleteFilter.Location = new System.Drawing.Point(75, 4);
            this.buttonDeleteFilter.Name = "buttonDeleteFilter";
            this.buttonDeleteFilter.Size = new System.Drawing.Size(68, 23);
            this.buttonDeleteFilter.TabIndex = 6;
            this.buttonDeleteFilter.Text = "Delete";
            this.buttonDeleteFilter.UseVisualStyleBackColor = true;
            this.buttonDeleteFilter.Click += new System.EventHandler(this.buttonDeleteFilter_Click);
            // 
            // buttonAddFilter
            // 
            this.buttonAddFilter.Location = new System.Drawing.Point(4, 4);
            this.buttonAddFilter.Name = "buttonAddFilter";
            this.buttonAddFilter.Size = new System.Drawing.Size(68, 23);
            this.buttonAddFilter.TabIndex = 5;
            this.buttonAddFilter.Text = "Add";
            this.buttonAddFilter.UseVisualStyleBackColor = true;
            this.buttonAddFilter.Click += new System.EventHandler(this.buttonAddFilter_Click);
            // 
            // listBoxFilters
            // 
            this.listBoxFilters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxFilters.FormattingEnabled = true;
            this.listBoxFilters.IntegralHeight = false;
            this.listBoxFilters.Location = new System.Drawing.Point(0, 33);
            this.listBoxFilters.Name = "listBoxFilters";
            this.listBoxFilters.Size = new System.Drawing.Size(290, 436);
            this.listBoxFilters.TabIndex = 0;
            this.listBoxFilters.SelectedIndexChanged += new System.EventHandler(this.listBoxFilters_SelectedIndexChanged);
            // 
            // tabPageListeners
            // 
            this.tabPageListeners.Controls.Add(this.label3);
            this.tabPageListeners.Controls.Add(this.splitContainerListeners);
            this.tabPageListeners.Location = new System.Drawing.Point(4, 22);
            this.tabPageListeners.Name = "tabPageListeners";
            this.tabPageListeners.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageListeners.Size = new System.Drawing.Size(877, 492);
            this.tabPageListeners.TabIndex = 1;
            this.tabPageListeners.Text = "Listeners";
            this.tabPageListeners.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(307, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "The listeners specify where the results of the probes are sent to.";
            // 
            // splitContainerListeners
            // 
            this.splitContainerListeners.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainerListeners.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerListeners.Location = new System.Drawing.Point(3, 20);
            this.splitContainerListeners.Name = "splitContainerListeners";
            // 
            // splitContainerListeners.Panel1
            // 
            this.splitContainerListeners.Panel1.Controls.Add(this.buttonDownListener);
            this.splitContainerListeners.Panel1.Controls.Add(this.buttonUpListener);
            this.splitContainerListeners.Panel1.Controls.Add(this.buttonDeleteListener);
            this.splitContainerListeners.Panel1.Controls.Add(this.buttonAddListener);
            this.splitContainerListeners.Panel1.Controls.Add(this.listBoxListeners);
            this.splitContainerListeners.Size = new System.Drawing.Size(871, 469);
            this.splitContainerListeners.SplitterDistance = 290;
            this.splitContainerListeners.TabIndex = 0;
            // 
            // buttonDownListener
            // 
            this.buttonDownListener.Location = new System.Drawing.Point(217, 4);
            this.buttonDownListener.Name = "buttonDownListener";
            this.buttonDownListener.Size = new System.Drawing.Size(68, 23);
            this.buttonDownListener.TabIndex = 8;
            this.buttonDownListener.Text = "Down";
            this.buttonDownListener.UseVisualStyleBackColor = true;
            this.buttonDownListener.Click += new System.EventHandler(this.buttonDownListener_Click);
            // 
            // buttonUpListener
            // 
            this.buttonUpListener.Location = new System.Drawing.Point(146, 4);
            this.buttonUpListener.Name = "buttonUpListener";
            this.buttonUpListener.Size = new System.Drawing.Size(68, 23);
            this.buttonUpListener.TabIndex = 7;
            this.buttonUpListener.Text = "Up";
            this.buttonUpListener.UseVisualStyleBackColor = true;
            this.buttonUpListener.Click += new System.EventHandler(this.buttonUpListener_Click);
            // 
            // buttonDeleteListener
            // 
            this.buttonDeleteListener.Location = new System.Drawing.Point(75, 4);
            this.buttonDeleteListener.Name = "buttonDeleteListener";
            this.buttonDeleteListener.Size = new System.Drawing.Size(68, 23);
            this.buttonDeleteListener.TabIndex = 6;
            this.buttonDeleteListener.Text = "Delete";
            this.buttonDeleteListener.UseVisualStyleBackColor = true;
            this.buttonDeleteListener.Click += new System.EventHandler(this.buttonDeleteListener_Click);
            // 
            // buttonAddListener
            // 
            this.buttonAddListener.Location = new System.Drawing.Point(4, 4);
            this.buttonAddListener.Name = "buttonAddListener";
            this.buttonAddListener.Size = new System.Drawing.Size(68, 23);
            this.buttonAddListener.TabIndex = 5;
            this.buttonAddListener.Text = "Add";
            this.buttonAddListener.UseVisualStyleBackColor = true;
            this.buttonAddListener.Click += new System.EventHandler(this.buttonAddListener_Click);
            // 
            // listBoxListeners
            // 
            this.listBoxListeners.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxListeners.FormattingEnabled = true;
            this.listBoxListeners.IntegralHeight = false;
            this.listBoxListeners.Location = new System.Drawing.Point(0, 33);
            this.listBoxListeners.Name = "listBoxListeners";
            this.listBoxListeners.Size = new System.Drawing.Size(290, 436);
            this.listBoxListeners.TabIndex = 1;
            this.listBoxListeners.SelectedIndexChanged += new System.EventHandler(this.listBoxListeners_SelectedIndexChanged);
            // 
            // tabPageProbes
            // 
            this.tabPageProbes.Controls.Add(this.label1);
            this.tabPageProbes.Controls.Add(this.splitContainerProbes);
            this.tabPageProbes.Location = new System.Drawing.Point(4, 22);
            this.tabPageProbes.Name = "tabPageProbes";
            this.tabPageProbes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProbes.Size = new System.Drawing.Size(877, 492);
            this.tabPageProbes.TabIndex = 0;
            this.tabPageProbes.Text = "Probes";
            this.tabPageProbes.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Probes are the tests you want to run.";
            // 
            // splitContainerProbes
            // 
            this.splitContainerProbes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainerProbes.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerProbes.IsSplitterFixed = true;
            this.splitContainerProbes.Location = new System.Drawing.Point(3, 20);
            this.splitContainerProbes.Name = "splitContainerProbes";
            // 
            // splitContainerProbes.Panel1
            // 
            this.splitContainerProbes.Panel1.Controls.Add(this.buttonDownProbe);
            this.splitContainerProbes.Panel1.Controls.Add(this.buttonUpProbe);
            this.splitContainerProbes.Panel1.Controls.Add(this.buttonDeleteProbe);
            this.splitContainerProbes.Panel1.Controls.Add(this.buttonAddProbe);
            this.splitContainerProbes.Panel1.Controls.Add(this.listBoxProbes);
            this.splitContainerProbes.Size = new System.Drawing.Size(871, 469);
            this.splitContainerProbes.SplitterDistance = 290;
            this.splitContainerProbes.TabIndex = 0;
            // 
            // buttonDownProbe
            // 
            this.buttonDownProbe.Location = new System.Drawing.Point(217, 4);
            this.buttonDownProbe.Name = "buttonDownProbe";
            this.buttonDownProbe.Size = new System.Drawing.Size(68, 23);
            this.buttonDownProbe.TabIndex = 4;
            this.buttonDownProbe.Text = "Down";
            this.buttonDownProbe.UseVisualStyleBackColor = true;
            this.buttonDownProbe.Click += new System.EventHandler(this.buttonDownProbe_Click);
            // 
            // buttonUpProbe
            // 
            this.buttonUpProbe.Location = new System.Drawing.Point(146, 4);
            this.buttonUpProbe.Name = "buttonUpProbe";
            this.buttonUpProbe.Size = new System.Drawing.Size(68, 23);
            this.buttonUpProbe.TabIndex = 3;
            this.buttonUpProbe.Text = "Up";
            this.buttonUpProbe.UseVisualStyleBackColor = true;
            this.buttonUpProbe.Click += new System.EventHandler(this.buttonUpProbe_Click);
            // 
            // buttonDeleteProbe
            // 
            this.buttonDeleteProbe.Location = new System.Drawing.Point(75, 4);
            this.buttonDeleteProbe.Name = "buttonDeleteProbe";
            this.buttonDeleteProbe.Size = new System.Drawing.Size(68, 23);
            this.buttonDeleteProbe.TabIndex = 2;
            this.buttonDeleteProbe.Text = "Delete";
            this.buttonDeleteProbe.UseVisualStyleBackColor = true;
            this.buttonDeleteProbe.Click += new System.EventHandler(this.buttonDeleteProbe_Click);
            // 
            // buttonAddProbe
            // 
            this.buttonAddProbe.Location = new System.Drawing.Point(4, 4);
            this.buttonAddProbe.Name = "buttonAddProbe";
            this.buttonAddProbe.Size = new System.Drawing.Size(68, 23);
            this.buttonAddProbe.TabIndex = 1;
            this.buttonAddProbe.Text = "Add";
            this.buttonAddProbe.UseVisualStyleBackColor = true;
            this.buttonAddProbe.Click += new System.EventHandler(this.buttonAddProbe_Click);
            // 
            // listBoxProbes
            // 
            this.listBoxProbes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxProbes.FormattingEnabled = true;
            this.listBoxProbes.IntegralHeight = false;
            this.listBoxProbes.Location = new System.Drawing.Point(0, 33);
            this.listBoxProbes.Name = "listBoxProbes";
            this.listBoxProbes.Size = new System.Drawing.Size(290, 436);
            this.listBoxProbes.TabIndex = 0;
            this.listBoxProbes.SelectedIndexChanged += new System.EventHandler(this.listBoxProbes_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageProbes);
            this.tabControl1.Controls.Add(this.tabPageFilters);
            this.tabControl1.Controls.Add(this.tabPageListeners);
            this.tabControl1.Controls.Add(this.tabPageListenersProbes);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(885, 518);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_TabIndexChanged);
            // 
            // tabPageListenersProbes
            // 
            this.tabPageListenersProbes.Controls.Add(this.dataGridViewListenersProbes);
            this.tabPageListenersProbes.Controls.Add(this.label4);
            this.tabPageListenersProbes.Location = new System.Drawing.Point(4, 22);
            this.tabPageListenersProbes.Name = "tabPageListenersProbes";
            this.tabPageListenersProbes.Size = new System.Drawing.Size(877, 492);
            this.tabPageListenersProbes.TabIndex = 3;
            this.tabPageListenersProbes.Text = "Probe<->Listener";
            this.tabPageListenersProbes.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(322, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Here you can specify which probe should be sent to which listener.";
            // 
            // dataGridViewListenersProbes
            // 
            this.dataGridViewListenersProbes.AllowUserToAddRows = false;
            this.dataGridViewListenersProbes.AllowUserToDeleteRows = false;
            this.dataGridViewListenersProbes.AllowUserToResizeColumns = false;
            this.dataGridViewListenersProbes.AllowUserToResizeRows = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewListenersProbes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewListenersProbes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewListenersProbes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewListenersProbes.Location = new System.Drawing.Point(0, 20);
            this.dataGridViewListenersProbes.Name = "dataGridViewListenersProbes";
            this.dataGridViewListenersProbes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewListenersProbes.ShowEditingIcon = false;
            this.dataGridViewListenersProbes.Size = new System.Drawing.Size(877, 472);
            this.dataGridViewListenersProbes.TabIndex = 1;
            this.dataGridViewListenersProbes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewListenersProbes_CellContentClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 545);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "SnakeEyes Config";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPageFilters.ResumeLayout(false);
            this.tabPageFilters.PerformLayout();
            this.splitContainerFilters.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFilters)).EndInit();
            this.splitContainerFilters.ResumeLayout(false);
            this.tabPageListeners.ResumeLayout(false);
            this.tabPageListeners.PerformLayout();
            this.splitContainerListeners.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerListeners)).EndInit();
            this.splitContainerListeners.ResumeLayout(false);
            this.tabPageProbes.ResumeLayout(false);
            this.tabPageProbes.PerformLayout();
            this.splitContainerProbes.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProbes)).EndInit();
            this.splitContainerProbes.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageListenersProbes.ResumeLayout(false);
            this.tabPageListenersProbes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewListenersProbes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabPage tabPageFilters;
        private System.Windows.Forms.SplitContainer splitContainerFilters;
        private System.Windows.Forms.ListBox listBoxFilters;
        private System.Windows.Forms.TabPage tabPageListeners;
        private System.Windows.Forms.SplitContainer splitContainerListeners;
        private System.Windows.Forms.ListBox listBoxListeners;
        private System.Windows.Forms.TabPage tabPageProbes;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button buttonDownFilter;
        private System.Windows.Forms.Button buttonUpFilter;
        private System.Windows.Forms.Button buttonDeleteFilter;
        private System.Windows.Forms.Button buttonAddFilter;
        private System.Windows.Forms.Button buttonDownListener;
        private System.Windows.Forms.Button buttonUpListener;
        private System.Windows.Forms.Button buttonDeleteListener;
        private System.Windows.Forms.Button buttonAddListener;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageListenersProbes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainerProbes;
        private System.Windows.Forms.Button buttonDownProbe;
        private System.Windows.Forms.Button buttonUpProbe;
        private System.Windows.Forms.Button buttonDeleteProbe;
        private System.Windows.Forms.Button buttonAddProbe;
        private System.Windows.Forms.ListBox listBoxProbes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridViewListenersProbes;
    }
}

