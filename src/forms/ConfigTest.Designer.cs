namespace Pathfinder
{
    partial class ConfigTest
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
            this.comboBoxAlgorithm = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDistance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxNumOfTestRuns = new System.Windows.Forms.ComboBox();
            this.textBoxOutputFilename = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxAlgorithm
            // 
            this.comboBoxAlgorithm.FormattingEnabled = true;
            this.comboBoxAlgorithm.Items.AddRange(new object[] {
            "A*",
            "Dijkstra"});
            this.comboBoxAlgorithm.Location = new System.Drawing.Point(135, 6);
            this.comboBoxAlgorithm.Name = "comboBoxAlgorithm";
            this.comboBoxAlgorithm.Size = new System.Drawing.Size(114, 21);
            this.comboBoxAlgorithm.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Algorithm";
            // 
            // textBoxDistance
            // 
            this.textBoxDistance.Location = new System.Drawing.Point(135, 37);
            this.textBoxDistance.MaxLength = 3;
            this.textBoxDistance.Name = "textBoxDistance";
            this.textBoxDistance.Size = new System.Drawing.Size(83, 20);
            this.textBoxDistance.TabIndex = 2;
            this.textBoxDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDistance_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Path Distance";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Number of Test Runs";
            // 
            // comboBoxNumOfTestRuns
            // 
            this.comboBoxNumOfTestRuns.FormattingEnabled = true;
            this.comboBoxNumOfTestRuns.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.comboBoxNumOfTestRuns.Location = new System.Drawing.Point(135, 66);
            this.comboBoxNumOfTestRuns.Name = "comboBoxNumOfTestRuns";
            this.comboBoxNumOfTestRuns.Size = new System.Drawing.Size(114, 21);
            this.comboBoxNumOfTestRuns.TabIndex = 5;
            // 
            // textBoxOutputFilename
            // 
            this.textBoxOutputFilename.Location = new System.Drawing.Point(135, 93);
            this.textBoxOutputFilename.Name = "textBoxOutputFilename";
            this.textBoxOutputFilename.Size = new System.Drawing.Size(114, 20);
            this.textBoxOutputFilename.TabIndex = 6;
            this.textBoxOutputFilename.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOutputFilename_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Output Filename";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(174, 124);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 8;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(12, 124);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "tiles";
            // 
            // ConfigTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 159);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxOutputFilename);
            this.Controls.Add(this.comboBoxNumOfTestRuns);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDistance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxAlgorithm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxAlgorithm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxNumOfTestRuns;
        private System.Windows.Forms.TextBox textBoxOutputFilename;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label5;
    }
}