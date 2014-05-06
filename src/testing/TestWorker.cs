/*
 * File: TestWorker.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a background worker thread used to run tests on the algorithms.
 * */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathfinder
{
    class TestWorker : BackgroundWorker
    {
        TestConfig config; // The test configuration being used
        TestResultCollection results; // The list of results

        private static bool cancel;
        Random rand;

        // Event to call on completion
        public event TestCompletedEventHandler OnComplete;

        public TestWorker(TestConfig config)
        {
            this.config = config;
            cancel = false;
            rand = new Random(DateTime.Now.Millisecond);
            results = new TestResultCollection();
            WorkerReportsProgress = true;
        }

        /// <summary>
        /// A background worker, used to run testing on a background thread.
        /// </summary>
        protected override void OnDoWork(DoWorkEventArgs e)
        {
 	        base.OnDoWork(e);

            // Assign a random start position
            Coord2 startPos;
            do
            {
                startPos.X = rand.Next(0, LevelHandler.Level.Map.GridSize);
                startPos.Y = rand.Next(0, LevelHandler.Level.Map.GridSize);
            } while (!LevelHandler.Level.Map.ValidPosition(startPos));

            // Find the possible spawn points
            List<Coord2> possibleTargets = new List<Coord2>();
            Coord2 topLeft = new Coord2(startPos.X - config.PathDistance, startPos.Y - config.PathDistance);
            for(int x = 0; x < (config.PathDistance*2); x++)
            {
                possibleTargets.Add(new Coord2(topLeft.X + x, topLeft.Y));
                possibleTargets.Add(new Coord2(topLeft.X + x, topLeft.Y + (config.PathDistance*2)));
            }
            for(int y = 0; y < (config.PathDistance*2); y++)
            {
                possibleTargets.Add(new Coord2(topLeft.X, topLeft.Y + y));
                possibleTargets.Add(new Coord2(topLeft.X + (config.PathDistance*2), topLeft.Y + y));
            }
            
            for (int i = 0; i < config.NumberOfTestRuns; i++)
            {
                if (!CheckCancellation())
                {
                    // Find a random position
                    Coord2 targetPos = possibleTargets[rand.Next(0, possibleTargets.Count)];

                    // Run test on the loaded level
                    TestResult result = new TestResult();
                    do
                    {
                        result = LevelHandler.RunTest(config.Algorithm, startPos, possibleTargets);
                    } while (result.Failed == true); // If the result reported a failure, run it again

                    // Add the result
                    results.Add(result);

                    // Report the thread's current progress
                    ReportProgress((int)(((float)100 / (float)config.NumberOfTestRuns) * (i + 1)), new TestProgress(i + 1, config.NumberOfTestRuns,
                        new TimeSpan(results.AverageTicksForPath)));
                }
                else
                {
                    results.Cancel();
                    break;
                }
            }
        }

        /// <summary>
        /// Check if user has cancelled the tests.
        /// </summary>
        private static bool CheckCancellation()
        {
            return cancel;
        }

        /// <summary>
        /// User has cancelled the tests.
        /// </summary>
        public static void Cancel()
        {
            cancel = true;
        }

        /// <summary>
        /// Thread completion event.
        /// </summary>
        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
 	        base.OnRunWorkerCompleted(e);
            OnComplete(this, new TestCompletedArgs(results, config, CheckCancellation()));
        }
    }
}
