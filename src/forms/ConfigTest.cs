/*
 * File: ConfigTest.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines the event handlers for the test configuration windows form.
 * 
 * */
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
    public partial class ConfigTest : Form
    {
        public ConfigTest()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Returns whether or not inputs in the form are currently valid ones.
        /// </summary>
        private bool ValidateInput()
        {
            bool valid = true;

            string errorText = "";

            if (textBoxOutputFilename.Text.Length == 0)
            {
                valid = false;
                errorText += ERROR_MSG.NO_OUTPUT_FILE;
            }

            if (textBoxDistance.Text.Length == 0)
            {
                valid = false;
                errorText += ERROR_MSG.NO_AVG_DIST;
            } else if (GetIntegerValue(textBoxDistance) == 0) {
                valid = false;
                errorText += ERROR_MSG.DIST_IS_ZERO;
            }

            if (comboBoxAlgorithm.Text.Length == 0)
            {
                valid = false;
                errorText += ERROR_MSG.NO_ALGORITHM;
            }

            if (comboBoxNumOfTestRuns.Text.Length == 0)
            {
                valid = false;
                errorText += ERROR_MSG.NO_NUMBER_OF_RUNS;
            } else if (GetIntegerValue(comboBoxNumOfTestRuns) == 0) {
                valid = false;
                errorText += ERROR_MSG.RUNS_IS_ZERO;
            }

            if(!valid)
                MessageBox.Show("Invalid input: \n" + errorText, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return valid;
        }

        /// <summary>
        /// Converts the text in a textbox into an integer value and returns the result.
        /// </summary>
        private int GetIntegerValue(System.Windows.Forms.TextBox tb)
        {
            return Convert.ToInt32(tb.Text);
        }

        /// <summary>
        /// Converts the text in a combobox into an integer value and returns the result.
        /// </summary>
        private int GetIntegerValue(ComboBox cb)
        {
            return Convert.ToInt32(cb.Text);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // Check algorithm
                PathfinderAlgorithm algo = PathfinderAlgorithm.AStar;
                if (comboBoxAlgorithm.Text == "A*")
                    algo = PathfinderAlgorithm.AStar;
                else if (comboBoxAlgorithm.Text == "Dijkstra")
                    algo = PathfinderAlgorithm.Dijkstra;

                // Set up the test configuration
                TestConfig config = new TestConfig(Convert.ToInt32(textBoxDistance.Text),
                                                Convert.ToInt32(comboBoxNumOfTestRuns.Text),
                                                textBoxOutputFilename.Text, algo);

                // Run Tests...
                TestRun test = new TestRun();
                test.Run(config);
                this.Close();
            }
        }

        private void textBoxDistance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Prevent letters
            if (Char.IsLetter(e.KeyChar))
                e.Handled = true;
        }
    }
}
