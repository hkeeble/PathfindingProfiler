using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    public class ScentMap : Pathfinder
    {
        public struct ScentBuffer
        {
            public int[,] data;
            public int gridSize;

            public ScentBuffer(int gridSize)
            {
                this.gridSize = gridSize;

                data = new int[gridSize, gridSize];

                for (int x = 0; x < gridSize; x++)
                    for (int y = 0; y < gridSize; y++)
                        data[x, y] = 0;
            }

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

                    // Add lowest to the path
                    path.Add(highest);

                    // Call recursively
                    Build(highest, targetPos, testMode);
                }
            }
        }
        
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

        public int GetValue(int x, int y)
        {
            return buffer1.data[x, y];
        }

        public override List<Coord2> GetPath()
        {
            return path;
        }

        public override void Clear()
        {
            path.Clear();
            buffer1 = new ScentBuffer(gridSize);
            buffer2 = new ScentBuffer(gridSize);
            isLive = true;
        }
        
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
