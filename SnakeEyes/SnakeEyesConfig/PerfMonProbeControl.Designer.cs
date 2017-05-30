namespace SnakeEyesConfig
{
    partial class PerfMonProbeControl
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
            this.textBoxServiceName = new System.Windows.Forms.TextBox();
            this.textBoxInstanceName = new System.Windows.Forms.TextBox();
            this.textBoxCounterName = new System.Windows.Forms.TextBox();
            this.textBoxCategoryName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMaxValue = new System.Windows.Forms.TextBox();
            this.textBoxMinValue = new System.Windows.Forms.TextBox();
            this.textBoxProbeFrequency = new System.Windows.Forms.TextBox();
            this.textBoxEventId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 222);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Event Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 196);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Event Id";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Probe Frequency";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Min Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Max Value";
            // 
            // comboBoxEventType
            // 
            this.comboBoxEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEventType.FormattingEnabled = true;
            this.comboBoxEventType.Location = new System.Drawing.Point(131, 219);
            this.comboBoxEventType.Name = "comboBoxEventType";
            this.comboBoxEventType.Size = new System.Drawing.Size(206, 21);
            this.comboBoxEventType.TabIndex = 32;
            this.comboBoxEventType.SelectedIndexChanged += new System.EventHandler(this.UpdateDataFromComboBox);
            // 
            // textBoxServiceName
            // 
            this.textBoxServiceName.Location = new System.Drawing.Point(131, 84);
            this.textBoxServiceName.Name = "textBoxServiceName";
            this.textBoxServiceName.Size = new System.Drawing.Size(206, 20);
            this.textBoxServiceName.TabIndex = 27;
            this.textBoxServiceName.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxInstanceName
            // 
            this.textBoxInstanceName.Location = new System.Drawing.Point(131, 58);
            this.textBoxInstanceName.Name = "textBoxInstanceName";
            this.textBoxInstanceName.Size = new System.Drawing.Size(206, 20);
            this.textBoxInstanceName.TabIndex = 26;
            this.textBoxInstanceName.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxCounterName
            // 
            this.textBoxCounterName.Location = new System.Drawing.Point(131, 32);
            this.textBoxCounterName.Name = "textBoxCounterName";
            this.textBoxCounterName.Size = new System.Drawing.Size(206, 20);
            this.textBoxCounterName.TabIndex = 25;
            this.textBoxCounterName.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxCategoryName
            // 
            this.textBoxCategoryName.Location = new System.Drawing.Point(131, 6);
            this.textBoxCategoryName.Name = "textBoxCategoryName";
            this.textBoxCategoryName.Size = new System.Drawing.Size(206, 20);
            this.textBoxCategoryName.TabIndex = 24;
            this.textBoxCategoryName.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Service Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Instance Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Counter Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Category Name";
            // 
            // textBoxMaxValue
            // 
            this.textBoxMaxValue.Location = new System.Drawing.Point(131, 112);
            this.textBoxMaxValue.Name = "textBoxMaxValue";
            this.textBoxMaxValue.Size = new System.Drawing.Size(206, 20);
            this.textBoxMaxValue.TabIndex = 44;
            this.textBoxMaxValue.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxMinValue
            // 
            this.textBoxMinValue.Location = new System.Drawing.Point(131, 139);
            this.textBoxMinValue.Name = "textBoxMinValue";
            this.textBoxMinValue.Size = new System.Drawing.Size(206, 20);
            this.textBoxMinValue.TabIndex = 45;
            this.textBoxMinValue.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxProbeFrequency
            // 
            this.textBoxProbeFrequency.Location = new System.Drawing.Point(131, 166);
            this.textBoxProbeFrequency.Name = "textBoxProbeFrequency";
            this.textBoxProbeFrequency.Size = new System.Drawing.Size(206, 20);
            this.textBoxProbeFrequency.TabIndex = 46;
            this.textBoxProbeFrequency.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxEventId
            // 
            this.textBoxEventId.Location = new System.Drawing.Point(131, 193);
            this.textBoxEventId.Name = "textBoxEventId";
            this.textBoxEventId.Size = new System.Drawing.Size(206, 20);
            this.textBoxEventId.TabIndex = 47;
            this.textBoxEventId.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // PerfMonProbeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxEventId);
            this.Controls.Add(this.textBoxProbeFrequency);
            this.Controls.Add(this.textBoxMinValue);
            this.Controls.Add(this.textBoxMaxValue);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxEventType);
            this.Controls.Add(this.textBoxServiceName);
            this.Controls.Add(this.textBoxInstanceName);
            this.Controls.Add(this.textBoxCounterName);
            this.Controls.Add(this.textBoxCategoryName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PerfMonProbeControl";
            this.Size = new System.Drawing.Size(517, 384);
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
        private System.Windows.Forms.TextBox textBoxServiceName;
        private System.Windows.Forms.TextBox textBoxInstanceName;
        private System.Windows.Forms.TextBox textBoxCounterName;
        private System.Windows.Forms.TextBox textBoxCategoryName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMaxValue;
        private System.Windows.Forms.TextBox textBoxMinValue;
        private System.Windows.Forms.TextBox textBoxProbeFrequency;
        private System.Windows.Forms.TextBox textBoxEventId;
    }
}
