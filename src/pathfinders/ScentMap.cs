/*
 * File: ScentMap.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a class that uses a scent map algorithm.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    /// <summary>
    /// Represents and manages a scent map.
    /// </summary>
    public class ScentMap : Pathfinder
    {
        /// <summary>
        /// An individual scent buffer.
        /// </summary>
        public struct ScentBuffer
        {
            public int[,] data;
            public int gridSize;

            /// <summary>
            /// Create a new scent buffer.
            /// </summary>
            /// <param name="gridSize">The size of the grid used by this buffer.</param>
            public ScentBuffer(int gridSize)
            {
                this.gridSize = gridSize;

                data = new int[gridSize, gridSize];

                for (int x = 0; x < gridSize; x++)
                    for (int y = 0; y < gridSize; y++)
                        data[x, y] = 0;
            }

            /// <summary>
            /// Copy the given scent buffer into this one.
            /// </summary>
            public void Copy(ScentBuffer param)
            {
                this.gridSize = param.gridSize;

                data = new int[gridSize, gridSize];

                for (int x = 0; x < gridSize; x++)
                    for (int y = 0; y < gridSize; y++)
                        data[x, y] = param.data[x,y];
            }
        }

        private int gridSize;
        private Map map;
        private string name;
        private ScentBuffer buffer1, buffer2;
        private bool isLive;
        private List<Coord2> path;
        private int sourceValue;
        private Color scentColor;

        /// <summary>
        /// Creates a new scent map.
        /// </summary>
        /// <param name="gridSize">The size of the grid being used by this scent map.</param>
        /// <param name="map">The map object being used by this scent map.</param>
        public ScentMap(int gridSize, Map map)
        {
            name = "Scent Map";
            this.gridSize = gridSize;
            buffer1 = new ScentBuffer(gridSize);
            buffer2 = new ScentBuffer(gridSize);
            sourceValue = 1;
            this.map = map;
            path = new List<Coord2>();
            isLive = true;
            scentColor = Color.Red;
        }

        public override string GetName()
        {
            return name;
        }

        /// <summary>
        /// Builds/Updates a scent map given a starting position and target position. If test mode is active, finds an entire
        /// path. Otherwise, performs a single update.
        /// <param name="startPos">Starting position.</param>
        /// <param name="targetPos">The current target positon.</param>
        /// <param name="testMode">Whether or not to find an entire path, or update the map once.</param>
        public override void Build(Coord2 startPos, Coord2 targetPos, bool testMode = false)
        {
            if (!path.Contains(targetPos))
            {
                buffer2.Copy(buffer1); // Copy Buffers

                // Increment player's value
                sourceValue = sourceValue + 1;

                // Update data in player's location
                buffer1.data[targetPos.X, targetPos.Y] = sourceValue;

                Coord2 currentLoc;

                // Update all locations
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        currentLoc = new Coord2(x, y);
                        if (map.ValidPosition(currentLoc)) // Ignore blocked spaces
                            ProcessLocation(currentLoc, targetPos);
                    }
                }

                // Update visuals
                UpdateVisualization();

                // Only recursive if not live - that is, is being used for non-visual profiling
                if (testMode)
                {
                    // Find best move position
                    List<Coord2> neighbours = GetNeighbours(startPos);
                    Coord2 highest = neighbours[0];
                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        if (map.ValidPosition(neighbours[i]))
                        {
                            if (buffer2.data[neighbours[i].X, neighbours[i].Y] > buffer2.data[highest.X, highest.Y])
                                highest = neighbours[i];
                        }
                    }

                    // Add lowest to the path, if scent has reached this point
                    path.Add(highest);

                    // Call recursively
                    Build(highest, targetPos, testMode);
                }
            }
        }
        
        /// <summary>
        /// Updates the visualization of the algorithm.
        /// </summary>
        private void UpdateVisualization()
        {
            int highestVal = HighestValue();
            int lowestVal = LowestValue();

            for(int x = 0; x < gridSize; x++)
            {
                for(int y = 0; y < gridSize; y++)
                {
                    map.SetRenderColor(new Coord2(x, y), Color.Lerp(Color.White, scentColor, (float)buffer1.data[x, y] / (float)highestVal));
                }
            }
        }

        /// <summary>
        /// Finds the highest current scent value.
        /// </summary>
        public int HighestValue()
        {
            int currentVal = 0;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (buffer1.data[x, y] > currentVal)
                        currentVal = buffer1.data[x, y];
                }
            }

            return currentVal;
        }

        /// <summary>
        /// Finds the lowest current scent value.
        /// </summary>
        public int LowestValue()
        {
            int currentVal = 1000000;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (buffer1.data[x, y] < currentVal)
                        currentVal = buffer1.data[x, y];
                }
            }

            return currentVal;
        }

        /// <summary>
        /// Processes an individual location and updates it's scent value.
        /// </summary>
        /// <param name="location">The location to update.</param>
        /// <param name="targetPos">The current position of the target.</param>
        private void ProcessLocation(Coord2 location, Coord2 targetPos)
        {
            List<Coord2> neighbours = GetNeighbours(location);

            for (int i = 0; i < neighbours.Count; i++)
            {
                if(map.ValidPosition(neighbours[i]))
                    if (buffer2.data[neighbours[i].X, neighbours[i].Y] > buffer1.data[location.X, location.Y])
                        buffer1.data[location.X, location.Y] = buffer2.data[neighbours[i].X, neighbours[i].Y]-1;
            }
        }

        /// <summary>
        /// Get all valid neighbours of the given location.
        /// </summary>
        protected List<Coord2> GetNeighbours(Coord2 location)
        {
            List<Coord2> neighbours = new List<Coord2>();
            if(map.ValidPosition(new Coord2(location.X + 1, location.Y + 1)))
                neighbours.Add(new Coord2(location.X + 1, location.Y + 1));

            if (map.ValidPosition(new Coord2(location.X - 1, location.Y - 1)))
                neighbours.Add(new Coord2(location.X - 1, location.Y - 1));

            if (map.ValidPosition(new Coord2(location.X - 1, location.Y + 1)))
                neighbours.Add(new Coord2(location.X - 1, location.Y + 1));

            if (map.ValidPosition(new Coord2(location.X + 1, location.Y - 1)))
                neighbours.Add(new Coord2(location.X + 1, location.Y - 1));

            if (map.ValidPosition(new Coord2(location.X, location.Y + 1)))
                neighbours.Add(new Coord2(location.X, location.Y + 1));

            if (map.ValidPosition(new Coord2(location.X + 1, location.Y)))
                neighbours.Add(new Coord2(location.X + 1, location.Y));

            if (map.ValidPosition(new Coord2(location.X - 1, location.Y)))
                neighbours.Add(new Coord2(location.X - 1, location.Y));

            if (map.ValidPosition(new Coord2(location.X, location.Y - 1)))
                neighbours.Add(new Coord2(location.X, location.Y - 1));

            return neighbours;
        }

        /// <summary>
        /// Get the current scent value of the given location.
        /// </summary>
        public int GetValue(int x, int y)
        {
            return buffer1.data[x, y];
        }

        /// <summary>
        /// Get the current path that has been generated. (Used only in test mode)
        /// </summary>
        public override List<Coord2> GetPath()
        {
            return path;
        }

        /// <summary>
        /// Clear all scent buffers.
        /// </summary>
        public override void Clear()
        {
            path.Clear();
            buffer1 = new ScentBuffer(gridSize);
            buffer2 = new ScentBuffer(gridSize);
            isLive = true;
        }
        
        /// <summary>
        /// Return whether or not the given location is within the path.  (Used only in test mode)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool IsInPath(int x, int y)
        {
            return path.Contains(new Coord2(x, y));
        }

        public override PathfinderAlgorithm GetAlgorithm()
        {
            return PathfinderAlgorithm.ScentMap;
        }

        /// <summary>
        /// Retrieve current scent grid data from the primary buffer.
        /// </summary>
        public ScentBuffer Scents { get { return buffer1; } }

        // Unused method implementations
        public override bool IsClosed(int x, int y) { return false; }
        public override bool IsClosed(Coord2 coord) { return false; }
        public override int NodesSearched()         { return 0; }
        public override void DrawPath(SpriteBatch sb) { }
    }
}
