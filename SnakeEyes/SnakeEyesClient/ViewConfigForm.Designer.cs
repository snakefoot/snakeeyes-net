namespace SnakeEyesClient
{
    partial class ViewConfigForm
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
            System.Windows.Forms.ColumnHeader _columnListenerName;
            System.Windows.Forms.ColumnHeader _columnListenerType;
            System.Windows.Forms.ColumnHeader _columnProbeName;
            System.Windows.Forms.ColumnHeader _columnProbeType;
            this._btnViewStatus = new System.Windows.Forms.Button();
            this._btnOK = new System.Windows.Forms.Button();
            this._btnEditListener = new System.Windows.Forms.Button();
            this._btnDeleteListener = new System.Windows.Forms.Button();
            this._btnAddListener = new System.Windows.Forms.Button();
            this._groupBoxSources = new System.Windows.Forms.GroupBox();
            this._listenerList = new System.Windows.Forms.ListView();
            this._btnEditProbe = new System.Windows.Forms.Button();
            this._btnDeleteProbe = new System.Windows.Forms.Button();
            this._btnAddProbe = new System.Windows.Forms.Button();
            this._groupBoxProbes = new System.Windows.Forms.GroupBox();
            this._probesList = new System.Windows.Forms.ListView();
            this._btnCancel = new System.Windows.Forms.Button();
            _columnListenerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            _columnListenerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            _columnProbeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            _columnProbeType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._groupBoxSources.SuspendLayout();
            this._groupBoxProbes.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnViewStatus
            // 
            this._btnViewStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnViewStatus.Location = new System.Drawing.Point(8, 492);
            this._btnViewStatus.Name = "_btnViewStatus";
            this._btnViewStatus.Size = new System.Drawing.Size(75, 23);
            this._btnViewStatus.TabIndex = 11;
            this._btnViewStatus.Text = "View Status";
            this._btnViewStatus.UseVisualStyleBackColor = true;
            // 
            // _btnOK
            // 
            this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOK.Location = new System.Drawing.Point(362, 492);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(75, 23);
            this._btnOK.TabIndex = 9;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            // 
            // _btnEditListener
            // 
            this._btnEditListener.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnEditListener.Location = new System.Drawing.Point(423, 48);
            this._btnEditListener.Name = "_btnEditListener";
            this._btnEditListener.Size = new System.Drawing.Size(75, 23);
            this._btnEditListener.TabIndex = 5;
            this._btnEditListener.Text = "Edit...";
            this._btnEditListener.UseVisualStyleBackColor = true;
            // 
            // _btnDeleteListener
            // 
            this._btnDeleteListener.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnDeleteListener.Location = new System.Drawing.Point(423, 77);
            this._btnDeleteListener.Name = "_btnDeleteListener";
            this._btnDeleteListener.Size = new System.Drawing.Size(75, 23);
            this._btnDeleteListener.TabIndex = 4;
            this._btnDeleteListener.Text = "Delete";
            this._btnDeleteListener.UseVisualStyleBackColor = true;
            // 
            // _btnAddListener
            // 
            this._btnAddListener.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAddListener.Location = new System.Drawing.Point(423, 19);
            this._btnAddListener.Name = "_btnAddListener";
            this._btnAddListener.Size = new System.Drawing.Size(75, 23);
            this._btnAddListener.TabIndex = 3;
            this._btnAddListener.Text = "Add...";
            this._btnAddListener.UseVisualStyleBackColor = true;
            // 
            // _groupBoxSources
            // 
            this._groupBoxSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBoxSources.Controls.Add(this._btnEditListener);
            this._groupBoxSources.Controls.Add(this._btnDeleteListener);
            this._groupBoxSources.Controls.Add(this._btnAddListener);
            this._groupBoxSources.Controls.Add(this._listenerList);
            this._groupBoxSources.Location = new System.Drawing.Point(8, 329);
            this._groupBoxSources.Name = "_groupBoxSources";
            this._groupBoxSources.Size = new System.Drawing.Size(510, 157);
            this._groupBoxSources.TabIndex = 8;
            this._groupBoxSources.TabStop = false;
            this._groupBoxSources.Text = "Event Listeners";
            // 
            // _listenerList
            // 
            this._listenerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._listenerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _columnListenerName,
            _columnListenerType});
            this._listenerList.Location = new System.Drawing.Point(12, 19);
            this._listenerList.Name = "_listenerList";
            this._listenerList.Size = new System.Drawing.Size(405, 124);
            this._listenerList.TabIndex = 2;
            this._listenerList.UseCompatibleStateImageBehavior = false;
            this._listenerList.View = System.Windows.Forms.View.Details;
            // 
            // _columnListenerName
            // 
            _columnListenerName.Text = "Name";
            // 
            // _columnListenerType
            // 
            _columnListenerType.Text = "Type";
            // 
            // _btnEditProbe
            // 
            this._btnEditProbe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnEditProbe.Location = new System.Drawing.Point(423, 48);
            this._btnEditProbe.Name = "_btnEditProbe";
            this._btnEditProbe.Size = new System.Drawing.Size(75, 23);
            this._btnEditProbe.TabIndex = 3;
            this._btnEditProbe.Text = "Edit...";
            this._btnEditProbe.UseVisualStyleBackColor = true;
            // 
            // _btnDeleteProbe
            // 
            this._btnDeleteProbe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnDeleteProbe.Location = new System.Drawing.Point(423, 77);
            this._btnDeleteProbe.Name = "_btnDeleteProbe";
            this._btnDeleteProbe.Size = new System.Drawing.Size(75, 23);
            this._btnDeleteProbe.TabIndex = 2;
            this._btnDeleteProbe.Text = "Delete";
            this._btnDeleteProbe.UseVisualStyleBackColor = true;
            // 
            // _btnAddProbe
            // 
            this._btnAddProbe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAddProbe.Location = new System.Drawing.Point(423, 19);
            this._btnAddProbe.Name = "_btnAddProbe";
            this._btnAddProbe.Size = new System.Drawing.Size(75, 23);
            this._btnAddProbe.TabIndex = 1;
            this._btnAddProbe.Text = "Add...";
            this._btnAddProbe.UseVisualStyleBackColor = true;
            // 
            // _groupBoxProbes
            // 
            this._groupBoxProbes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBoxProbes.Controls.Add(this._btnEditProbe);
            this._groupBoxProbes.Controls.Add(this._btnDeleteProbe);
            this._groupBoxProbes.Controls.Add(this._btnAddProbe);
            this._groupBoxProbes.Controls.Add(this._probesList);
            this._groupBoxProbes.Location = new System.Drawing.Point(8, 11);
            this._groupBoxProbes.Name = "_groupBoxProbes";
            this._groupBoxProbes.Size = new System.Drawing.Size(510, 312);
            this._groupBoxProbes.TabIndex = 7;
            this._groupBoxProbes.TabStop = false;
            this._groupBoxProbes.Text = "Probes";
            // 
            // _probesList
            // 
            this._probesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._probesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _columnProbeName,
            _columnProbeType});
            this._probesList.Location = new System.Drawing.Point(12, 19);
            this._probesList.Name = "_probesList";
            this._probesList.Size = new System.Drawing.Size(405, 273);
            this._probesList.TabIndex = 0;
            this._probesList.UseCompatibleStateImageBehavior = false;
            this._probesList.View = System.Windows.Forms.View.Details;
            // 
            // _columnProbeName
            // 
            _columnProbeName.Text = "Name";
            // 
            // _columnProbeType
            // 
            _columnProbeType.Text = "Type";
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(443, 492);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 10;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // ViewConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 527);
            this.Controls.Add(this._btnViewStatus);
            this.Controls.Add(this._btnOK);
            this.Controls.Add(this._groupBoxSources);
            this.Controls.Add(this._groupBoxProbes);
            this.Controls.Add(this._btnCancel);
            this.Name = "ViewConfigForm";
            this.Text = "ViewConfigForm";
            this.Load += new System.EventHandler(this.ViewConfigForm_Load);
            this._groupBoxSources.ResumeLayout(false);
            this._groupBoxProbes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnViewStatus;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnEditListener;
        private System.Windows.Forms.Button _btnDeleteListener;
        private System.Windows.Forms.Button _btnAddListener;
        private System.Windows.Forms.GroupBox _groupBoxSources;
        private System.Windows.Forms.ListView _listenerList;
        private System.Windows.Forms.Button _btnEditProbe;
        private System.Windows.Forms.Button _btnDeleteProbe;
        private System.Windows.Forms.Button _btnAddProbe;
        private System.Windows.Forms.GroupBox _groupBoxProbes;
        private System.Windows.Forms.ListView _probesList;
        private System.Windows.Forms.Button _btnCancel;
    }
}