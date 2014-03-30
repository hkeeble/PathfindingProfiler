using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Pathfinder
{
    public partial class GenerateMap : Form
    {
        public GenerateMap()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            string errMsg = "";

            if (textBoxName.Text.Length == 0)
                errMsg += MG_ERROR_MSG.NO_NAME;

            if (errMsg.Length == 0)
            {
                // Obtain input data from form
                string name = textBoxName.Text;
                int gridSize = FormUtil.GetIntegerValue(numericUpDownGridSize);
                int percentCoverage = FormUtil.GetIntegerValue(numericUpDownPercBlocked);

                // Create tile array
                int[,] tiles = new int[gridSize, gridSize];

                // Initialize tile array
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        tiles[x, y] = 0;
                    }
                }

                // Calculate the number of tiles that need to be blocked
                int totalTiles = gridSize * gridSize;
                double totalTilesToCover = ((double)percentCoverage / 100f) * totalTiles;

                // Seed a random generator
                Random rand = new Random(DateTime.Now.Millisecond);

                // Ensure that number of tiles are blocked
                for (int i = 0; i < totalTilesToCover; i++)
                {
                    Coord2 newBlocked;

                    do
                    {
                        newBlocked = new Coord2(rand.Next(0, gridSize), rand.Next(0, gridSize));
                    } while (tiles[newBlocked.X, newBlocked.Y] == 1);

                    tiles[newBlocked.X, newBlocked.Y] = 1;
                }

                // Output the map data
                StreamWriter sw = new StreamWriter("Content/Maps/" + name + ".map");
                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        if (tiles[x, y] == 1)
                            sw.Write('#');
                        else
                            sw.Write('.');
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Flush();
                sw.Close();

                this.Close();
            }
            else
                MessageBox.Show("Invalid input: \n" + errMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
