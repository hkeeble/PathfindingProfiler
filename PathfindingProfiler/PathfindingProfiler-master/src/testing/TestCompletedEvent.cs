using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    /// <summary>
    /// Test completed handler delegate functon.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">Arguments of the event.</param>
    public delegate void TestCompletedEventHandler(object source, TestCompletedArgs e);

    /// <summary>
    /// Arguments to pass to the text completed event handler.
    /// </summary>
    public class TestCompletedArgs : EventArgs
    {
        private TestResultCollection results;
        private TestConfig config;
        private bool cancelled;

        public TestCompletedArgs(TestResultCollection results, TestConfig config, bool cancelled)
        {
            this.cancelled = cancelled;
            this.results = results;
            this.config = config;
        }

        /// <summary>
        /// The results of the testing.
        /// </summary>
        public TestResultCollection Results { get { return results; } }

        public TestConfig Config { get { return config; } }

        /// <summary>
        /// Whether or not the test was cancelled before completion.
        /// </summary>
        public bool WasCancelled { get { return cancelled; } }
    }
}
