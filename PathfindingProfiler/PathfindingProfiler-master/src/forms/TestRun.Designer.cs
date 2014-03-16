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
            this.percComplete = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBarTests
            // 
            this.progressBarTests.Location = new System.Drawing.Point(13, 13);
            this.progressBarTests.Name = "progressBarTests";
            this.progressBarTests.Size = new System.Drawing.Size(259, 23);
            this.progressBarTests.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Running Tests:";
            // 
            // percLabel
            // 
            this.percLabel.AutoSize = true;
            this.percLabel.Location = new System.Drawing.Point(196, 43);
            this.percLabel.Name = "percLabel";
            this.percLabel.Size = new System.Drawing.Size(14, 13);
            this.percLabel.TabIndex = 2;
            this.percLabel.Text = "#";
            // 
            // percComplete
            // 
            this.percComplete.AutoSize = true;
            this.percComplete.Location = new System.Drawing.Point(210, 43);
            this.percComplete.Name = "percComplete";
            this.percComplete.Size = new System.Drawing.Size(62, 13);
            this.percComplete.TabIndex = 3;
            this.percComplete.Text = "% Complete";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(94, 59);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel Tests";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // TestRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 90);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.percComplete);
            this.Controls.Add(this.percLabel);
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
        private System.Windows.Forms.Label percComplete;
        private System.Windows.Forms.Button buttonCancel;
    }
}