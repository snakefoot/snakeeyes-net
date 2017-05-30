namespace SnakeEyesConfig
{
    partial class MsmqTraceListenerControl
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
            this.checkBoxCreateQueue = new System.Windows.Forms.CheckBox();
            this.textBoxFormatBody = new System.Windows.Forms.TextBox();
            this.textBoxFormatLabel = new System.Windows.Forms.TextBox();
            this.textBoxQueueLabel = new System.Windows.Forms.TextBox();
            this.textBoxQueueName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxFilter = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // checkBoxCreateQueue
            // 
            this.checkBoxCreateQueue.AutoSize = true;
            this.checkBoxCreateQueue.Location = new System.Drawing.Point(156, 59);
            this.checkBoxCreateQueue.Name = "checkBoxCreateQueue";
            this.checkBoxCreateQueue.Size = new System.Drawing.Size(44, 17);
            this.checkBoxCreateQueue.TabIndex = 25;
            this.checkBoxCreateQueue.Text = "Yes";
            this.checkBoxCreateQueue.UseVisualStyleBackColor = true;
            // 
            // textBoxFormatBody
            // 
            this.textBoxFormatBody.Location = new System.Drawing.Point(156, 110);
            this.textBoxFormatBody.Name = "textBoxFormatBody";
            this.textBoxFormatBody.Size = new System.Drawing.Size(277, 20);
            this.textBoxFormatBody.TabIndex = 24;
            this.textBoxFormatBody.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxFormatLabel
            // 
            this.textBoxFormatLabel.Location = new System.Drawing.Point(156, 84);
            this.textBoxFormatLabel.Name = "textBoxFormatLabel";
            this.textBoxFormatLabel.Size = new System.Drawing.Size(277, 20);
            this.textBoxFormatLabel.TabIndex = 23;
            this.textBoxFormatLabel.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxQueueLabel
            // 
            this.textBoxQueueLabel.Location = new System.Drawing.Point(156, 32);
            this.textBoxQueueLabel.Name = "textBoxQueueLabel";
            this.textBoxQueueLabel.Size = new System.Drawing.Size(277, 20);
            this.textBoxQueueLabel.TabIndex = 22;
            this.textBoxQueueLabel.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxQueueName
            // 
            this.textBoxQueueName.Location = new System.Drawing.Point(156, 6);
            this.textBoxQueueName.Name = "textBoxQueueName";
            this.textBoxQueueName.Size = new System.Drawing.Size(277, 20);
            this.textBoxQueueName.TabIndex = 21;
            this.textBoxQueueName.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Format Body";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Format Label";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Create Queue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Queue Label";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Queue Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Filter";
            // 
            // comboBoxFilter
            // 
            this.comboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilter.FormattingEnabled = true;
            this.comboBoxFilter.Location = new System.Drawing.Point(156, 136);
            this.comboBoxFilter.Name = "comboBoxFilter";
            this.comboBoxFilter.Size = new System.Drawing.Size(277, 21);
            this.comboBoxFilter.TabIndex = 33;
            this.comboBoxFilter.SelectedIndexChanged += new System.EventHandler(this.UpdateFilterData);
            // 
            // MsmqTraceListenerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxFilter);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBoxCreateQueue);
            this.Controls.Add(this.textBoxFormatBody);
            this.Controls.Add(this.textBoxFormatLabel);
            this.Controls.Add(this.textBoxQueueLabel);
            this.Controls.Add(this.textBoxQueueName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MsmqTraceListenerControl";
            this.Size = new System.Drawing.Size(493, 217);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxCreateQueue;
        private System.Windows.Forms.TextBox textBoxFormatBody;
        private System.Windows.Forms.TextBox textBoxFormatLabel;
        private System.Windows.Forms.TextBox textBoxQueueLabel;
        private System.Windows.Forms.TextBox textBoxQueueName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxFilter;
    }
}
