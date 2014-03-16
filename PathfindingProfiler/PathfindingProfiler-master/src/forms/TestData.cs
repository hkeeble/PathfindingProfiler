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
        public TestConfig(int pathDist, int nRuns, string output, PathfinderAlgorithm al)
        {
            avgPathDist = pathDist;
            nOfTestRuns = nRuns;
            outputFile = output;
            algo = al;
        }

        private int avgPathDist;
        private int nOfTestRuns;
        private string outputFile;
        private PathfinderAlgorithm algo;

        public int AveragePathDistance { get { return avgPathDist; } }
        public int NumberOfTestRuns { get { return nOfTestRuns; } }
        public string OutputFile { get { return outputFile; } }
        public PathfinderAlgorithm Algorithm { get { return algo; } }
    }

    /* Represents the results of a given test */
    public struct TestResult
    {
        public TestResult(int msForPath, int pLength)
        {
            millisecondsForPath = msForPath;
            pathLength = pLength;
        }

        private int millisecondsForPath;
        private int pathLength;

        public int MillisecondsForPath { get { return millisecondsForPath; } }
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

        public int AverageMillisecondsForPath
        {
            get
            {
                int tot = 0;
                foreach (TestResult r in results)
                    tot += r.MillisecondsForPath;
                return tot / results.Count;
            }
        }
    }
}
