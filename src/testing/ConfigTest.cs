/*
 * File: ConfigTest.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines the event handlers for the test configuration windows form.
 */
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
                errorText += TCFG_ERROR_MSG.NO_OUTPUT_FILE;
            }

            if (numericUpDownDist.Value == 0)
            {
                valid = false;
                errorText += TCFG_ERROR_MSG.NO_AVG_DIST;
            }
            else if (FormUtil.GetIntegerValue(numericUpDownDist) == 0)
            {
                valid = false;
                errorText += TCFG_ERROR_MSG.DIST_IS_ZERO;
            }

            if (comboBoxAlgorithm.Text.Length == 0)
            {
                valid = false;
                errorText += TCFG_ERROR_MSG.NO_ALGORITHM;
            }

            if (numericUpDownNOfRuns.Value == 0)
            {
                valid = false;
                errorText += TCFG_ERROR_MSG.NO_NUMBER_OF_RUNS;
            }
            else if (FormUtil.GetIntegerValue(numericUpDownNOfRuns) == 0)
            {
                valid = false;
                errorText += TCFG_ERROR_MSG.RUNS_IS_ZERO;
            }

            if(!valid)
                MessageBox.Show("Invalid input: \n" + errorText, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return valid;
        }

        /// <summary>
        /// Run button click event function. This function initializes the tests, and shows the testing window dialog.
        /// </summary>
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
                else if (comboBoxAlgorithm.Text == "Scent Map")
                    algo = PathfinderAlgorithm.ScentMap;

                // Set up the test configuration
                TestConfig config = new TestConfig(Convert.ToInt32(numericUpDownDist.Value),
                                                Convert.ToInt32(numericUpDownNOfRuns.Text),
                                                textBoxOutputFilename.Text, algo,
                                                LevelHandler.Level.Map,
                                                LevelHandler.Level.Map.Name);

                // Run Tests...
                TestRun test = new TestRun(config);
                test.ShowDialog();
                this.Close();
            }
        }

        private void textBoxDistance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Prevent letters
            if (Char.IsLetter(e.KeyChar))
                e.Handled = true;
        }

        private void ConfigTest_Load(object sender, EventArgs e)
        {
            ProfilerMenu.configTestActive = true;
        }

        private void ConfigTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            ProfilerMenu.configTestActive = false;
        }
    }
}
