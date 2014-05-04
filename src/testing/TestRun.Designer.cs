namespace Pathfinder
{
    partial class TestRun
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
            this.progressBarTests = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.percLabel = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.timeRemainingLabel = new System.Windows.Forms.Label();
            this.configTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // progressBarTests
            // 
            this.progressBarTests.Location = new System.Drawing.Point(11, 112);
            this.progressBarTests.Name = "progressBarTests";
            this.progressBarTests.Size = new System.Drawing.Size(259, 23);
            this.progressBarTests.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Running Tests:";
            // 
            // percLabel
            // 
            this.percLabel.AutoSize = true;
            this.percLabel.Location = new System.Drawing.Point(96, 142);
            this.percLabel.Name = "percLabel";
            this.percLabel.Size = new System.Drawing.Size(28, 13);
            this.percLabel.TabIndex = 2;
            this.percLabel.Text = "###";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(99, 190);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel Tests";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Estimated Time Remaining:";
            // 
            // timeRemainingLabel
            // 
            this.timeRemainingLabel.AutoSize = true;
            this.timeRemainingLabel.Location = new System.Drawing.Point(152, 166);
            this.timeRemainingLabel.Name = "timeRemainingLabel";
            this.timeRemainingLabel.Size = new System.Drawing.Size(68, 13);
            this.timeRemainingLabel.TabIndex = 2;
            this.timeRemainingLabel.Text = "Calculating...";
            // 
            // configTextBox
            // 
            this.configTextBox.Enabled = false;
            this.configTextBox.Location = new System.Drawing.Point(13, 13);
            this.configTextBox.Name = "configTextBox";
            this.configTextBox.Size = new System.Drawing.Size(257, 93);
            this.configTextBox.TabIndex = 5;
            this.configTextBox.Text = "";
            // 
            // TestRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 225);
            this.Controls.Add(this.configTextBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.timeRemainingLabel);
            this.Controls.Add(this.percLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBarTests);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TestRun";
            this.Text = "Tests Running...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarTests;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label percLabel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label timeRemainingLabel;
        private System.Windows.Forms.RichTextBox configTextBox;
    }
}