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
    /* Contains testing error messages */
    struct ERROR_MSG
    {
        public const string NO_ALGORITHM = "Select an algorithm to use.\n";
        public const string NO_AVG_DIST = "Enter a path distance.\n";
        public const string NO_NUMBER_OF_RUNS = "Enter a number of test runs.\n";
        public const string NO_OUTPUT_FILE = "Enter a filename.\n";
        public const string RUNS_IS_ZERO = "Number of runs cannot be 0.\n";
        public const string DIST_IS_ZERO = "Path distance cannot be 0.\n";
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
        public TestResult(long ticks, int pLength)
        {
            ticksForPath = ticks;
            pathLength = pLength;
        }

        private long ticksForPath;
        private int pathLength;

        public long TicksForPath { get { return ticksForPath; } }
        public int PathLength { get { return pathLength; } }
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
    }
}
