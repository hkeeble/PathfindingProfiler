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
        TestConfig config;
        Random rand; // Random number generator used for testing
        bool cancel; // Whether or not the user has cancelled the test
        BackgroundWorker worker; // The background worker, testing runs on it's own background thread

        TestResultCollection results;

        public TestRun()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs the test and shows the progress window as a dialog to the specified owner.
        /// </summary>
        /// <param name="config">The testing configuration to use.</param>
        /// <param name="owner">The window that owns the test.</param>
        /// <returns>Results of the tests given the testing configuration.</returns>
        public void Run(TestConfig config, IWin32Window owner)
        {
            // Show as a dialog
            this.ShowDialog(owner);

            // Initialize progress bar
            progressBarTests.Maximum = 100;
            progressBarTests.Minimum = 0;
            progressBarTests.Value = 0;

            // Initialize percentage label
            percLabel.Text = "0";

            cancel = false;

            // Initialize background worker
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(backgroundWorker);
            worker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerReport);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerComplete);

            // Run background worker thread
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// A background worker, used to run testing on a background thread.
        /// </summary>
        private void backgroundWorker(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            for (int i = 0; i < config.NumberOfTestRuns; i++)
            {
                if (!cancel)
                {
                    // Calculate positions for this test run
                    Coord2 targetPos = new Coord2(0, 0);
                    Coord2 botPos = new Coord2(0, 0);

                    // Assign a random bot position
                    do
                    {
                        botPos.X = rand.Next(0, LevelHandler.Level.Map.GridSize);
                        botPos.Y = rand.Next(0, LevelHandler.Level.Map.GridSize);
                    } while (!LevelHandler.Level.Map.ValidPosition(botPos));

                    // Assign a target position the correct distance from the bot
                    do
                    {

                    } while (!LevelHandler.Level.Map.ValidPosition(targetPos));

                    // Run test on the loaded level
                    results.Add(LevelHandler.RunTest(config.Algorithm, new Coord2(0, 0), new Coord2(0, 0)));

                    // Report the thread's current progress
                    bw.ReportProgress((i / config.NumberOfTestRuns) * 100);
                }
                else
                    break;
            }
        }

        /// <summary>
        /// Event handler when the background thread reports it's progress.
        /// </summary>
        private void backgroundWorkerReport(object sender, ProgressChangedEventArgs args)
        {
            // Convert parameters
            BackgroundWorker bw = sender as BackgroundWorker;
            int perc = args.ProgressPercentage;

            // Increment progress bar
            progressBarTests.Increment(perc);

            // Set label
            percLabel.Text = Convert.ToString(perc);
        }

        /// <summary>
        /// Event handler when the background thread completes the given tests.
        /// </summary>
        private void backgroundWorkerComplete(object sender, RunWorkerCompletedEventArgs args)
        {
            if (cancel)
            {
                // TODO, WHAT TO DO IF TEST CANCELLED
            }
            else
            {
                // TODO, WHAT TO DO WHEN TEST IS COMPLETED
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }
    }
}
