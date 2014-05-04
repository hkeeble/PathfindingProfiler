/*
 * File: TestData.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines several data structures used by the pathfinding testing system.
 * 
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
    /// Contains error messages for th map generation form.
    /// </summary>
    struct MG_ERROR_MSG
    {
        public const string NO_NAME = "Enter a map name.\n";
    }

    /* Represents a test configuration */
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

    /* Represents the results of a given test */
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

    /* Represents a collection of test results */
    public class TestResultCollection
    {
        private List<TestResult> results;

        public TestResultCollection()
        {
            results = new List<TestResult>();
        }

        public void Add(TestResult result)
        {
            results.Add(result);
        }

        public int Count { get { return results.Count; } }

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

       public static double StandardDeviation<T>(List<T> data)
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
        public TestProgress(int testsComplete, int testsToDo, TimeSpan averageTestDuration)
        {
            this.testsComplete = testsComplete;
            this.testsToDo = testsToDo;
            this.averageTestDuration = averageTestDuration;
            this.percComplete = (int)((float)100 / (float)testsToDo) * testsComplete;
        }

        private int testsComplete;
        private int testsToDo;
        private int percComplete;
        private TimeSpan averageTestDuration;

        public int TestsComplete { get { return testsComplete; } }
        public int TestsToDo { get { return testsToDo; } }
        public int PercentComplete { get { return percComplete; } }
        public TimeSpan AverageTestDuration { get { return averageTestDuration; } }
    }
}
