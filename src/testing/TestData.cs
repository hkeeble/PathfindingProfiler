/*
 * File: TestData.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines several data structures used by the pathfinding testing system.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    /// <summary>
    /// Contains error messages for the test configuration form.
    /// </summary>
    struct TCFG_ERROR_MSG
    {
        public const string NO_ALGORITHM = "Select an algorithm to use.\n";
        public const string NO_AVG_DIST = "Enter a path distance.\n";
        public const string NO_NUMBER_OF_RUNS = "Enter a number of test runs.\n";
        public const string NO_OUTPUT_FILE = "Enter a filename.\n";
        public const string RUNS_IS_ZERO = "Number of runs cannot be 0.\n";
        public const string DIST_IS_ZERO = "Path distance cannot be 0.\n";
    }

    /// <summary>
    /// Contains error messages for the map generation form.
    /// </summary>
    struct MG_ERROR_MSG
    {
        public const string NO_NAME = "Enter a map name.\n";
    }

    /// <summary>
    /// Represents a test configuration.
    /// </summary>
    public struct TestConfig
    {
        public TestConfig(int pathDist, int nRuns, string output, PathfinderAlgorithm al, Map map, string mapName)
        {
            this.pathDist = pathDist;
            this.mapName = mapName;
            nOfTestRuns = nRuns;
            outputFile = output;
            algo = al;
            startTime = DateTime.Now;
            endTime = new DateTime();

            // Calculate number of obstacles
            nOfObstacles = 0;
            nOfObstacles = CalcNumberOfObstacles(map);
        }

        /// <summary>
        /// Calculates the number of blocked spaces in the given map.
        /// </summary>
        private int CalcNumberOfObstacles(Map map)
        {
            int count = 0;
            for(int x = 0; x < map.GridSize; x++)
            {
                for(int y = 0; y < map.GridSize; y++)
                {
                    if(!map.ValidPosition(new Coord2(x, y)))
                        count++;
                }
            }

            return count;
        }

        public void SetEndTime(DateTime time)
        {
            endTime = time;
        }

        // Configuration data
        private DateTime startTime;
        private DateTime endTime;
        private string mapName;
        private int nOfObstacles;
        private int pathDist;
        private int nOfTestRuns;
        private string outputFile;
        private PathfinderAlgorithm algo;

        // Data accessors
        public int PathDistance { get { return pathDist; } }
        public int NumberOfTestRuns { get { return nOfTestRuns; } }
        public string OutputFile { get { return outputFile; } }
        public PathfinderAlgorithm Algorithm { get { return algo; } }
        public DateTime StartTime { get { return startTime; } }
        public DateTime EndTime { get { return endTime; } }
        public string MapName { get { return mapName; } }
        public int NumberOfObstacles { get { return nOfObstacles; } }
    }

    /// <summary>
    /// Represents the results of an individual test.
    /// </summary>
    public struct TestResult
    {
        public static double MS_PER_TICK = 0.0001;

        public TestResult(long ticks, int pLength, int nodesSearched)
        {
            ticksForPath = ticks;
            pathLength = pLength;
            this.nodesSearched = nodesSearched;
            failed = false;
        }

        private long ticksForPath;
        private int pathLength;
        private int nodesSearched;
        private bool failed;

        public long TicksForPath { get { return ticksForPath; } }
        public int PathLength { get { return pathLength; } }
        public bool Failed { get { return failed; } set { failed = value; } }
        public int NodesSearched { get { return nodesSearched; } }
    }

    /// <summary>
    /// Represents a collection of test results.
    /// </summary>
    public class TestResultCollection
    {
        private List<TestResult> results; /* The list of test results. */
        private bool cancelled;         /* Whether or not the test has been cancelled. */

        public TestResultCollection()
        {
            results = new List<TestResult>();
            cancelled = false;
        }

        public void Add(TestResult result)
        {
            results.Add(result);
        }

        /// <summary>
        /// Inform the results that the test was cancelled by the user before completion.
        /// </summary>
        public void Cancel()
        {
            cancelled = true;
        }

        /// <summary>
        /// Retrieve whether or not the test was cancelled by the user.
        /// </summary>
        public bool Cancelled { get { return cancelled; } }

        /// <summary>
        /// Retrieve the number of test results.
        /// </summary>
        public int Count { get { return results.Count; } }

        /// <summary>
        /// The current average path length in the test results.
        /// </summary>
        public int AverageLength
        {
            get
            {
                int tot = 0;
                foreach (TestResult r in results)
                    tot += r.PathLength;
                return tot / results.Count;
            }
        }

        /// <summary>
        /// The current average number of ticks required to find each path in the collection.
        /// </summary>
        public long AverageTicksForPath
        {
            get
            {
                long tot = 0;
                foreach (TestResult r in results)
                    tot += r.TicksForPath;
                return tot / results.Count;
            }
        }

        /// <summary>
        /// The current average number of nodes searched when finding each path in the collection.
        /// </summary>
        public int AveragedNodesSearched
        {
            get
            {
                int tot = 0;
                foreach (TestResult r in results)
                    tot += r.NodesSearched;
                return tot / results.Count;
            }
        }

        /// <summary>
        /// Returns the current standard deviation of the collection of path lengths.
        /// </summary>
        public double STDEVLength
        {
            get 
            {
                List<int> lengths = new List<int>();
                foreach(TestResult result in results)
                    lengths.Add(result.PathLength);
                return StandardDeviation<int>(lengths);
            }
        }

        /// <summary>
        /// Returns the current standard deviation of the time taken to find each path in milliseconds.
        /// </summary>
        public double STDEVMillisecondsTaken
        {
            get
            {
                List<double> ticks = new List<double>();
                foreach (TestResult result in results)
                    ticks.Add((double)result.TicksForPath * TestResult.MS_PER_TICK);
                return StandardDeviation<double>(ticks);
            }
        }

        /// <summary>
        /// Returns the current standard deviation of the number of nodes searched to find each path in the collection.
        /// </summary>
        public double STDEVNodesSearched
        {
            get
            {
                List<int> nodesSearched = new List<int>();
                foreach (TestResult result in results)
                    nodesSearched.Add(result.NodesSearched);
                return StandardDeviation<int>(nodesSearched);
            }
        }

        /// <summary>
        /// Returns the standard deviation of a collection of data. Generic function, accepts either double or int data types.
        /// </summary>
        /// <typeparam name="T">The type of data being processed.</typeparam>
        /// <param name="data">The collection of data to find the standard deviation for.</param>
        /// <returns></returns>
       private static double StandardDeviation<T>(List<T> data)
       {
           if (data.Count > 0)
           {
               if (data[0].GetType() == typeof(int)) // For integer values
               {
                   int mean;
                   int tot = 0;
                   foreach (T element in data)
                       tot += (int)Convert.ChangeType(element, typeof(int));
                   mean = tot / data.Count;
                   List<int> squaredDifferences = new List<int>();
                   foreach (T element in data)
                       squaredDifferences.Add((int)Math.Pow((int)Convert.ChangeType(element, typeof(int)) - mean, 2));
                   tot = 0;
                   foreach (int element in squaredDifferences)
                       tot += element;
                   double variance = tot / squaredDifferences.Count;
                   return Math.Sqrt(variance);
               }
               else if (data[0].GetType() == typeof(double)) // For long values
               {
                   double mean;
                   double tot = 0;
                   foreach (T element in data)
                       tot += (double)Convert.ChangeType(element, typeof(double));
                   mean = tot / data.Count;
                   List<double> squaredDifferences = new List<double>();
                   foreach (T element in data)
                       squaredDifferences.Add((long)Math.Pow((double)Convert.ChangeType(element, typeof(double)) - mean, 2));
                   tot = 0;
                   foreach (double element in squaredDifferences)
                       tot += element;
                   double variance = tot / squaredDifferences.Count;
                   return Math.Sqrt(variance);
               }
               else
               {
                   Console.WriteLine("Error! Type of data list for standard deviation calculation was unrecognized.");
                   return 0;
               }
           }
           else
           {
               Console.WriteLine("Error! Data passed to standadrd deviation is empty.");
               return 0;
           }
       }
    }

    /* Represents the progress of a test. */
    public struct TestProgress
    {
        /// <summary>
        /// Create a new test progress object, used for a worker thread to report progress through user state parameter.
        /// </summary>
        /// <param name="testsComplete">Number of tests completed.</param>
        /// <param name="testsToDo">Number of tests still required to complete.</param>
        /// <param name="averageTestDuration">Average duration of tests completed thus far.</param>
        public TestProgress(int testsComplete, int testsToDo, TimeSpan averageTestDuration)
        {
            this.testsComplete = testsComplete;
            this.testsToDo = testsToDo;
            this.averageTestDuration = averageTestDuration;
            this.percComplete = (int)((float)100 / (float)testsToDo) * testsComplete;
        }

        private int testsComplete; /* The number of tests completed */
        private int testsToDo; /* The number of tests still to do */
        private int percComplete; /* The percentage of tests completed thus far */
        private TimeSpan averageTestDuration; /* The current average test duration. */

        /// <summary>
        /// Retrieve the number of tests completed.
        /// </summary>
        public int TestsComplete { get { return testsComplete; } }

        /// <summary>
        /// Retrieve the numebr of tests that need completing.
        /// </summary>
        public int TestsToDo { get { return testsToDo; } }

        /// <summary>
        /// Return the current percentage of tests that have been completed.
        /// </summary>
        public int PercentComplete { get { return percComplete; } }

        /// <summary>
        /// Return the current average duration of tests in milliseconds.
        /// </summary>
        public TimeSpan AverageTestDuration { get { return averageTestDuration; } }
    }
}
