namespace SnakeEyesConfig
{
    partial class PingProbeControl
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxEventType = new System.Windows.Forms.ComboBox();
            this.textBoxDontFragment = new System.Windows.Forms.TextBox();
            this.textBoxTimeoutMs = new System.Windows.Forms.TextBox();
            this.textBoxIpAddress = new System.Windows.Forms.TextBox();
            this.textBoxHostName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBuffersize = new System.Windows.Forms.TextBox();
            this.textBoxSampleCount = new System.Windows.Forms.TextBox();
            this.textBoxProbeFrequency = new System.Windows.Forms.TextBox();
            this.textBoxEventId = new System.Windows.Forms.TextBox();
            this.textBoxMaxValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxTTL = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 273);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Event Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Event Id";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Probe Frequency";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Sample Count";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Buffer Size";
            // 
            // comboBoxEventType
            // 
            this.comboBoxEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEventType.FormattingEnabled = true;
            this.comboBoxEventType.Location = new System.Drawing.Point(131, 270);
            this.comboBoxEventType.Name = "comboBoxEventType";
            this.comboBoxEventType.Size = new System.Drawing.Size(206, 21);
            this.comboBoxEventType.TabIndex = 32;
            this.comboBoxEventType.SelectedIndexChanged += new System.EventHandler(this.UpdateDataFromComboBox);
            // 
            // textBoxDontFragment
            // 
            this.textBoxDontFragment.Location = new System.Drawing.Point(131, 110);
            this.textBoxDontFragment.Name = "textBoxDontFragment";
            this.textBoxDontFragment.Size = new System.Drawing.Size(206, 20);
            this.textBoxDontFragment.TabIndex = 27;
            this.textBoxDontFragment.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxTimeoutMs
            // 
            this.textBoxTimeoutMs.Location = new System.Drawing.Point(131, 57);
            this.textBoxTimeoutMs.Name = "textBoxTimeoutMs";
            this.textBoxTimeoutMs.Size = new System.Drawing.Size(206, 20);
            this.textBoxTimeoutMs.TabIndex = 26;
            this.textBoxTimeoutMs.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxIpAddress
            // 
            this.textBoxIpAddress.Location = new System.Drawing.Point(131, 32);
            this.textBoxIpAddress.Name = "textBoxIpAddress";
            this.textBoxIpAddress.Size = new System.Drawing.Size(206, 20);
            this.textBoxIpAddress.TabIndex = 25;
            this.textBoxIpAddress.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxHostName
            // 
            this.textBoxHostName.Location = new System.Drawing.Point(131, 6);
            this.textBoxHostName.Name = "textBoxHostName";
            this.textBoxHostName.Size = new System.Drawing.Size(206, 20);
            this.textBoxHostName.TabIndex = 24;
            this.textBoxHostName.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Don\'t Fragment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "TTL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "IP Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Host Name";
            // 
            // textBoxBuffersize
            // 
            this.textBoxBuffersize.Location = new System.Drawing.Point(131, 138);
            this.textBoxBuffersize.Name = "textBoxBuffersize";
            this.textBoxBuffersize.Size = new System.Drawing.Size(206, 20);
            this.textBoxBuffersize.TabIndex = 44;
            this.textBoxBuffersize.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxSampleCount
            // 
            this.textBoxSampleCount.Location = new System.Drawing.Point(131, 165);
            this.textBoxSampleCount.Name = "textBoxSampleCount";
            this.textBoxSampleCount.Size = new System.Drawing.Size(206, 20);
            this.textBoxSampleCount.TabIndex = 45;
            this.textBoxSampleCount.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxProbeFrequency
            // 
            this.textBoxProbeFrequency.Location = new System.Drawing.Point(131, 217);
            this.textBoxProbeFrequency.Name = "textBoxProbeFrequency";
            this.textBoxProbeFrequency.Size = new System.Drawing.Size(206, 20);
            this.textBoxProbeFrequency.TabIndex = 46;
            this.textBoxProbeFrequency.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxEventId
            // 
            this.textBoxEventId.Location = new System.Drawing.Point(131, 244);
            this.textBoxEventId.Name = "textBoxEventId";
            this.textBoxEventId.Size = new System.Drawing.Size(206, 20);
            this.textBoxEventId.TabIndex = 47;
            this.textBoxEventId.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxMaxValue
            // 
            this.textBoxMaxValue.Location = new System.Drawing.Point(131, 191);
            this.textBoxMaxValue.Name = "textBoxMaxValue";
            this.textBoxMaxValue.Size = new System.Drawing.Size(206, 20);
            this.textBoxMaxValue.TabIndex = 49;
            this.textBoxMaxValue.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 194);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 48;
            this.label10.Text = "Max Value";
            // 
            // textBoxTTL
            // 
            this.textBoxTTL.Location = new System.Drawing.Point(131, 84);
            this.textBoxTTL.Name = "textBoxTTL";
            this.textBoxTTL.Size = new System.Drawing.Size(206, 20);
            this.textBoxTTL.TabIndex = 51;
            this.textBoxTTL.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 50;
            this.label11.Text = "Timeout (ms)";
            // 
            // PingProbeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxTTL);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxMaxValue);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxEventId);
            this.Controls.Add(this.textBoxProbeFrequency);
            this.Controls.Add(this.textBoxSampleCount);
            this.Controls.Add(this.textBoxBuffersize);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxEventType);
            this.Controls.Add(this.textBoxDontFragment);
            this.Controls.Add(this.textBoxTimeoutMs);
            this.Controls.Add(this.textBoxIpAddress);
            this.Controls.Add(this.textBoxHostName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PingProbeControl";
            this.Size = new System.Drawing.Size(368, 318);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxEventType;
        private System.Windows.Forms.TextBox textBoxDontFragment;
        private System.Windows.Forms.TextBox textBoxTimeoutMs;
        private System.Windows.Forms.TextBox textBoxIpAddress;
        private System.Windows.Forms.TextBox textBoxHostName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBuffersize;
        private System.Windows.Forms.TextBox textBoxSampleCount;
        private System.Windows.Forms.TextBox textBoxProbeFrequency;
        private System.Windows.Forms.TextBox textBoxEventId;
        private System.Windows.Forms.TextBox textBoxMaxValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxTTL;
        private System.Windows.Forms.Label label11;
    }
}
