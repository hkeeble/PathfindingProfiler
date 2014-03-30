using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Pathfinder
{
    public partial class TestRun : Form
    {
        TestWorker worker; // The background worker, testing runs on it's own background thread

        private int percOnlastUpdate = 0;

        private const double MS_PER_TICK = 0.0001;

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

            // Initialize percentage on last update
            percOnlastUpdate = 0;

            // Run background worker thread
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Event handler. Used to update the form when the worker reports progress.
        /// </summary>
        private void workerReport(object sender, ProgressChangedEventArgs args)
        {
            TestWorker wrkr = sender as TestWorker;

            percLabel.Text = Convert.ToString(args.ProgressPercentage);
            progressBarTests.Increment(args.ProgressPercentage - percOnlastUpdate);
            percOnlastUpdate = args.ProgressPercentage;
            this.Refresh();
        }

        /// <summary>
        /// Event handler. Called when thread is completed.
        /// </summary>
        private void workerComplete(object sender, TestCompletedArgs e)
        {
            e.Config.SetEndTime(DateTime.Now);

            progressBarTests.Value = progressBarTests.Maximum;
            percLabel.Text = "100";
            this.Refresh();

            MessageBox.Show("Test completed!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            OutputResults(e.Results, e.Config);

            this.Close();
        }

        /// <summary>
        /// Outputs the given results to a text file.
        /// </summary>
        private void OutputResults(TestResultCollection results, TestConfig config)
        {
            StreamWriter sw;

            IEnumerable<string> dirs = Directory.EnumerateDirectories(Directory.GetCurrentDirectory());
            if(!dirs.Contains<string>("Test Results"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Test Results/");

            try {
                sw = new StreamWriter(Directory.GetCurrentDirectory() + "/Test Results/" + config.OutputFile + ".txt");
            } catch(Exception e) {
                Console.Write("Failed to open stream for test output file.\nException: " + e.Message + "\n");
                return;
            }

            sw.Write("Test Run On: " + config.StartTime.ToLongDateString() + " at " + config.EndTime.ToLongTimeString() + sw.NewLine + sw.NewLine);
            sw.Write("----- Configuration -----" + sw.NewLine);
            sw.Write("Map: " + config.MapName + sw.NewLine);
            sw.Write("Number of Obstacles on Map: " + config.NumberOfObstacles + sw.NewLine);
            sw.Write("Manhattan distance between start and target: " + config.PathDistance + sw.NewLine + sw.NewLine);
            sw.Write("----- Results ----" + sw.NewLine);
            sw.Write("Average path length: " + results.AverageLength + sw.NewLine);
            sw.Write("Average ms taken: " + ((double)results.AverageTicksForPath * MS_PER_TICK) + sw.NewLine);

            sw.Flush();
            sw.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            TestWorker.Cancel();
        }
    }
}
