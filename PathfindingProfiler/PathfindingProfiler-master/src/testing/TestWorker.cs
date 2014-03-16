using System;
using System.ComponentModel;
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

            for (int i = 0; i < config.NumberOfTestRuns; i++)
            {
                if (!CheckCancellation())
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

                    // Calculate area in which the target should not be
                    Rectangle noSpawnArea = new Rectangle(botPos.X - config.PathDistance, botPos.Y - config.PathDistance, config.PathDistance * 2, config.PathDistance * 2);

                    // Assign a target position the correct distance from the bot
                    do
                    {
                        targetPos.X = rand.Next(0, LevelHandler.Level.Map.GridSize);
                        targetPos.Y = rand.Next(0, LevelHandler.Level.Map.GridSize);
                    } while (!LevelHandler.Level.Map.ValidPosition(targetPos) && noSpawnArea.Contains(new Point(targetPos.X, targetPos.Y)));

                    // Run test on the loaded level
                    results.Add(LevelHandler.RunTest(config.Algorithm, botPos, targetPos));

                    // Report the thread's current progress
                    ReportProgress((int)(100/config.NumberOfTestRuns)*i, null);
                }
                else
                    break;
            }
        }

        private static bool CheckCancellation()
        {
            return cancel;
        }

        public static void Cancel()
        {
            cancel = true;
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
 	        base.OnRunWorkerCompleted(e);
            OnComplete(this, new TestCompletedArgs(results, config, CheckCancellation()));
        }
    }
}
