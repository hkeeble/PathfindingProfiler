/*
 * File: TestRun.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines the form showing test progress to the user, and runs the test thread upon opening.
 * */
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

        TimeSpan currentTimeEstimate;

        const int ITERATIONS_PER_ESTIMATION = 20;
        int currentIterations;

        TestConfig config;

        public TestRun(TestConfig config)
        {
            InitializeComponent();
            this.config = config; 
        }

        protected override void OnShown(EventArgs e)
        {
            Run();
            base.OnShown(e);
        }

        /// <summary>
        /// Runs the test and shows the progress window.
        /// </summary>
        /// <param name="config">The testing configuration to use.</param>
        public void Run()
        {
            // Ensure the menu operations cease when tests are running
            ProfilerMenu.testActive = true;

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

            // Initialize config display
            configTextBox.Text += "Algorithm: " + config.Algorithm.ToString() + "\n";
            configTextBox.Text += "Map: " + config.MapName + "\n";
            configTextBox.Text += "Path Distance: " + config.PathDistance + "\n";
            configTextBox.Text += "Running " + config.NumberOfTestRuns + " tests.\n";
            configTextBox.Text += "Outputting results to " + config.OutputFile + ".txt\n";

            // Initialize timing calculations
            currentTimeEstimate = TimeSpan.Zero;
            currentIterations = 0;

            // Run background worker thread
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Event handler. Used to update the form when the worker reports progress.
        /// </summary>
        private void workerReport(object sender, ProgressChangedEventArgs args)
        {
            TestWorker wrkr = sender as TestWorker;
            TestProgress progress = (TestProgress)args.UserState;

            EstimateTimeRemaining(progress);

            percLabel.Text = Convert.ToString(args.ProgressPercentage) + "%" + " (" + progress.TestsComplete + "/" + progress.TestsToDo + ")";
            progressBarTests.Increment(args.ProgressPercentage - percOnlastUpdate);
            percOnlastUpdate = args.ProgressPercentage;
            this.Refresh();
        }

        private void EstimateTimeRemaining(TestProgress progress)
        {
            currentIterations++;

            if (currentIterations >= ITERATIONS_PER_ESTIMATION)
            {
                TimeSpan currentTimeEstimate = TimeSpan.FromTicks(progress.AverageTestDuration.Ticks * (progress.TestsToDo - progress.TestsComplete));
                string timeString = "";
                if (currentTimeEstimate.Hours != 0)
                    timeString += currentTimeEstimate.Hours + " hour(s), ";
                if (currentTimeEstimate.Minutes != 0)
                    timeString += currentTimeEstimate.Minutes + " minute(s), ";
                timeString += currentTimeEstimate.Seconds + " second(s)";

                timeRemainingLabel.Text = timeString;

                currentIterations = 0;
            }
        }

        /// <summary>
        /// Event handler. Called when thread is completed.
        /// </summary>
        private void workerComplete(object sender, TestCompletedArgs e)
        {
            e.Config.SetEndTime(DateTime.Now);

            this.Refresh();

            if (e.Results.Count == e.Config.NumberOfTestRuns)
            {
                MessageBox.Show("Test completed!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OutputResults(e.Results, e.Config);
            }
            else if(e.Results.Cancelled)
                MessageBox.Show("Tests cancelled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Test worker exited unexpectedly!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Turn test flag off in profiler menu
            ProfilerMenu.testActive = false;

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
            sw.Write("Algorithm: " + config.Algorithm.ToString() + sw.NewLine);
            sw.Write("Map: " + config.MapName + sw.NewLine);
            sw.Write("Number of Obstacles on Map: " + config.NumberOfObstacles + sw.NewLine);
            sw.Write("Manhattan distance between start and target: " + config.PathDistance + sw.NewLine);
            sw.Write("Number of tests: " + config.NumberOfTestRuns + sw.NewLine + sw.NewLine);

            sw.Write("----- Results ----" + sw.NewLine);
            sw.Write("Average path length: \t\t" + results.AverageLength + " (STDEV: " + results.STDEVLength + ")" + sw.NewLine);
            sw.Write("Average nodes searched: \t" + results.AveragedNodesSearched + " (STDEV: " + results.STDEVNodesSearched + ")" + sw.NewLine);
            sw.Write("Average ms taken: \t\t" + ((double)results.AverageTicksForPath * TestResult.MS_PER_TICK) + " (STDEV: " + results.STDEVMillisecondsTaken + ")" + sw.NewLine);

            sw.Flush();
            sw.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            TestWorker.Cancel();
        }
    }
}
