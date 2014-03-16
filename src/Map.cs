using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    /// <summary>
    /// Represents a map loaded from an ASCII text file.
    /// </summary>
    public class Map
    {
        private int gridSize;
        public int [,] tiles;

        private Texture2D tile1Texture;
        private Texture2D tile2Texture;

        public IPathfinder pathfinder;
        private PathfinderAlgorithm algorithm;

        private string name;

        public string Name { get { return name; } }

        /// <summary>
        /// Constructor initializes tile array and tile textures.
        /// </summary>
        /// <param name="tile1Texture">Texture for floor tile.</param>
        /// <param name="tile2Texture">Texture for blocked/wall tile.</param>
        public Map(Texture2D tile1Texture, Texture2D tile2Texture)
        {
            tiles = new int[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    tiles[i,j] = 0;

            pathfinder = null;

            this.tile1Texture = tile1Texture;
            this.tile2Texture = tile2Texture;
        }

        /// <summary>
        /// Set the current pathfinding algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to use in pathfinding.</param>
        public void SetPathfinder(PathfinderAlgorithm algorithm)
        {
            this.algorithm = algorithm;
            pathfinder = PathfinderFactory.CreatePathfinder(algorithm, gridSize, this);
            Console.WriteLine("Map.cs: Pathfinding algorithm set to " + pathfinder.GetName() + ".\n");
        }

        // Get Accessors
        public int GridSize { get { return gridSize; } }
        public Texture2D Tile1Texture { get { return tile1Texture; } }
        public Texture2D Tile2Texture { get { return tile2Texture; } }
        public PathfinderAlgorithm PathfindingAlgorithm { get { return algorithm; } }

        /// <summary>
        /// Returns whether or not a coordinate is a valid position in the map.
        /// </summary>
        /// <param name="pos">The position to check for validity.</param>
        /// <returns></returns>
        public bool ValidPosition(Coord2 pos)
        {
            if (pos.X < 0) return false;
            if (pos.X >= gridSize) return false;
            if (pos.Y < 0) return false;
            if (pos.Y >= gridSize) return false;
            return (tiles[pos.X,pos.Y] == 0);
        }

        /// <summary>
        /// Loads a map from a given text file.
        /// </summary>
        /// <param name="path">File path of the map file to load.</param>
        public void Loadmap(string path)
        {
            string[] sections = path.Split('/');
            name = sections[sections.Length-1];
            name.TrimEnd(new char[] { '.', 'm', 'a', 'p' });

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                // Read in the size of the map
                string line = reader.ReadLine();
                gridSize = line.Length;
                tiles = new int[gridSize, gridSize];
                for (int i = 0; i < gridSize; i++)
                    for (int j = 0; j < gridSize; j++)
                        tiles[i, j] = 0;

                Debug.Assert(line.Length == gridSize, String.Format("loaded map string line width must be {0}.", gridSize));
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }
            Debug.Assert(lines.Count == gridSize, String.Format("loaded map string must have {0} lines.",gridSize));

            // Loop over every tile position,
            for (int y = 0; y < gridSize; ++y)
            {
                for (int x = 0; x < gridSize; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    if (tileType == '.') tiles[x, y] = 0;
                    else tiles[x, y] = 1;
                }
            }
        }
    }
}
