namespace SnakeEyesConfig
{
    partial class DelayStateFilterControl
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
            this.textBoxDelayTriggerTime = new System.Windows.Forms.TextBox();
            this.textBoxNextTriggerTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxDelayTriggerTime
            // 
            this.textBoxDelayTriggerTime.Location = new System.Drawing.Point(131, 33);
            this.textBoxDelayTriggerTime.Name = "textBoxDelayTriggerTime";
            this.textBoxDelayTriggerTime.Size = new System.Drawing.Size(206, 20);
            this.textBoxDelayTriggerTime.TabIndex = 29;
            this.textBoxDelayTriggerTime.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // textBoxNextTriggerTime
            // 
            this.textBoxNextTriggerTime.Location = new System.Drawing.Point(131, 7);
            this.textBoxNextTriggerTime.Name = "textBoxNextTriggerTime";
            this.textBoxNextTriggerTime.Size = new System.Drawing.Size(206, 20);
            this.textBoxNextTriggerTime.TabIndex = 28;
            this.textBoxNextTriggerTime.TextChanged += new System.EventHandler(this.UpdateDataFromTextBox);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Delay Trigger Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Next Trigger Time";
            // 
            // DelayStateFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxDelayTriggerTime);
            this.Controls.Add(this.textBoxNextTriggerTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DelayStateFilterControl";
            this.Size = new System.Drawing.Size(404, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDelayTriggerTime;
        private System.Windows.Forms.TextBox textBoxNextTriggerTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
