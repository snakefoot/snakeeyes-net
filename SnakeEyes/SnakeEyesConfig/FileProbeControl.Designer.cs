namespace SnakeEyesConfig
{
    partial class FileProbeControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxEventType = new System.Windows.Forms.ComboBox();
            this.textBoxDefaultFileSize = new System.Windows.Forms.TextBox();
            this.textBoxMaxFileAge = new System.Windows.Forms.TextBox();
            this.textBoxMaxFileSize = new System.Windows.Forms.TextBox();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDefaultFileAge = new System.Windows.Forms.TextBox();
            this.textBoxProbeFrequency = new System.Windows.Forms.TextBox();
            this.textBoxEventId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Event Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Event Id";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Probe Frequency";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Default File Age";
            // 
            // comboBoxEventType
            // 
            this.comboBoxEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEventType.FormattingEnabled = true;
            this.comboBoxEventType.Location = new System.Drawing.Point(131, 191);
            this.comboBoxEventType.Name = "comboBoxEventType";
            this.comboBoxEventType.Size = new System.Drawing.Size(206, 21);
            this.comboBoxEventType.TabIndex = 32;
            this.comboBoxEventType.SelectedIndexChanged += new System.EventHandler(this.UpdateDataFromComboBox);
            // 
            // textBoxDefaultFileSize
            // 
            this.textBoxDefaultFileSize.Location = new System.Drawing.Point(131, 84);
            this.textBoxDefaultFileSize.Name = "textBoxDefaultFileSize";
            this.textBoxDefaultFileSize.Size = new System.Drawing.Size(206, 20);
            this.textBoxDefaultFileSize.TabIndex = 27;
            this.textBoxDefaultFileSize.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxMaxFileAge
            // 
            this.textBoxMaxFileAge.Location = new System.Drawing.Point(131, 58);
            this.textBoxMaxFileAge.Name = "textBoxMaxFileAge";
            this.textBoxMaxFileAge.Size = new System.Drawing.Size(206, 20);
            this.textBoxMaxFileAge.TabIndex = 26;
            this.textBoxMaxFileAge.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxMaxFileSize
            // 
            this.textBoxMaxFileSize.Location = new System.Drawing.Point(131, 32);
            this.textBoxMaxFileSize.Name = "textBoxMaxFileSize";
            this.textBoxMaxFileSize.Size = new System.Drawing.Size(206, 20);
            this.textBoxMaxFileSize.TabIndex = 25;
            this.textBoxMaxFileSize.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(131, 6);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(206, 20);
            this.textBoxFileName.TabIndex = 24;
            this.textBoxFileName.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Default File Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Max File Age";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Max File Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "File Name";
            // 
            // textBoxDefaultFileAge
            // 
            this.textBoxDefaultFileAge.Location = new System.Drawing.Point(131, 112);
            this.textBoxDefaultFileAge.Name = "textBoxDefaultFileAge";
            this.textBoxDefaultFileAge.Size = new System.Drawing.Size(206, 20);
            this.textBoxDefaultFileAge.TabIndex = 44;
            this.textBoxDefaultFileAge.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxProbeFrequency
            // 
            this.textBoxProbeFrequency.Location = new System.Drawing.Point(131, 138);
            this.textBoxProbeFrequency.Name = "textBoxProbeFrequency";
            this.textBoxProbeFrequency.Size = new System.Drawing.Size(206, 20);
            this.textBoxProbeFrequency.TabIndex = 46;
            this.textBoxProbeFrequency.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxEventId
            // 
            this.textBoxEventId.Location = new System.Drawing.Point(131, 165);
            this.textBoxEventId.Name = "textBoxEventId";
            this.textBoxEventId.Size = new System.Drawing.Size(206, 20);
            this.textBoxEventId.TabIndex = 47;
            this.textBoxEventId.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // FileProbeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxEventId);
            this.Controls.Add(this.textBoxProbeFrequency);
            this.Controls.Add(this.textBoxDefaultFileAge);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxEventType);
            this.Controls.Add(this.textBoxDefaultFileSize);
            this.Controls.Add(this.textBoxMaxFileAge);
            this.Controls.Add(this.textBoxMaxFileSize);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FileProbeControl";
            this.Size = new System.Drawing.Size(397, 240);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxEventType;
        private System.Windows.Forms.TextBox textBoxDefaultFileSize;
        private System.Windows.Forms.TextBox textBoxMaxFileAge;
        private System.Windows.Forms.TextBox textBoxMaxFileSize;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDefaultFileAge;
        private System.Windows.Forms.TextBox textBoxProbeFrequency;
        private System.Windows.Forms.TextBox textBoxEventId;
    }
}
