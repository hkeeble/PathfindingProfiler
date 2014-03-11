using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pathfinder
{
    public partial class TestRun : Form
    {
        TestWorker worker; // The background worker, testing runs on it's own background thread

        public TestRun()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs the test and shows the progress window.
        /// </summary>
        /// <param name="config">The testing configuration to use.</param>
        public void Run(TestConfig config)
        {
            // Show as a dialog
            this.Show();

            // Initialize progress bar
            progressBarTests.Maximum = 100;
            progressBarTests.Minimum = 0;
            progressBarTests.Value = 0;

            // Initialize percentage label
            percLabel.Text = "0";

            // Initialize background worker
            worker = new TestWorker(config);
            worker.ProgressChanged += new ProgressChangedEventHandler(workerReport);
            worker.OnComplete += new TestCompletedEventHandler(workerComplete);

            // Run background worker thread
            worker.RunWorkerAsync();
        }

        private void workerReport(object sender, ProgressChangedEventArgs args)
        {
            TestWorker wrkr = sender as TestWorker;

            percLabel.Text = Convert.ToString(Convert.ToInt32(percLabel.Text) + args.ProgressPercentage);
            progressBarTests.Increment(args.ProgressPercentage);
            this.Refresh();
        }

        private void workerComplete(object sender, TestCompletedArgs e)
        {
            progressBarTests.Value = progressBarTests.Maximum;
            percLabel.Text = "100";
            this.Refresh();

            MessageBox.Show("Test completed!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            // TODO - WHEN TEST IS COMPLETED
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            TestWorker.Cancel();
        }
    }
}
