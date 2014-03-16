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
            // Obtain input data from form
            string name = textBoxName.Text;
            int width = FormUtil.GetIntegerValue(textBoxWidth);
            int height = FormUtil.GetIntegerValue(textBoxHeight);
            int percentCoverage = FormUtil.GetIntegerValue(numericUpDownPercBlocked);

            // Create tile array
             int[,] tiles = new int[width, height];

            // Initialize tile array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = 0;
                }
            }

            // Calculate the number of tiles that need to be blocked
            int totalTiles = width * height;
            double totalTilesToCover = ((double)percentCoverage / 100f) * totalTiles;

            // Seed a random generator
            Random rand = new Random(DateTime.Now.Millisecond);

            // Ensure that number of tiles are blocked
            for(int i = 0; i < totalTilesToCover; i++)
            {
                Coord2 newBlocked;

                do {
                    newBlocked = new Coord2(rand.Next(0, width), rand.Next(0, height));
                } while (tiles[newBlocked.X, newBlocked.Y] == 1);

                tiles[newBlocked.X, newBlocked.Y] = 1;
            }

            // Output the map data
            StreamWriter sw = new StreamWriter("Content/Maps/" + name + ".map");
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
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
    }
}
