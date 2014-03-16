using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pathfinder
{
    public class ScentMap : IPathfinder
    {
        public struct ScentBuffer
        {
            public int[,] data;

            public ScentBuffer(int gridSize)
            {
                data = new int[gridSize, gridSize];

                for (int x = 0; x < gridSize; x++)
                    for (int y = 0; y < gridSize; y++)
                        data[x, y] = 0;
            }
        }

        private int gridSize;
        private Map map;
        private string name;
        private ScentBuffer buffer1, buffer2;
        int sourceValue;

        public ScentMap(int gridSize, Map map)
        {
            name = "Scent Map";
            this.gridSize = gridSize;
            buffer1 = new ScentBuffer(gridSize);
            buffer2 = new ScentBuffer(gridSize);
            sourceValue = 0;
            this.map = map;
        }

        public virtual string GetName()
        {
            return name;
        }

        public void Build(Coord2 startPos, Coord2 targetPos)
        {
            buffer2 = buffer1; // Copy Buffers

            // Increment player's value
            sourceValue = sourceValue + 1;

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

            // Update data in player's location
            buffer1.data[targetPos.X, targetPos.Y] = sourceValue;
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
            Coord2[] neighbours = GetNeighbours(location);

            for (int i = 0; i < neighbours.Length; i++)
            {
                if(map.ValidPosition(neighbours[i]) && targetPos != neighbours[i])
                    if (buffer2.data[neighbours[i].X, neighbours[i].Y] > buffer1.data[location.X, location.Y])
                        buffer1.data[location.X, location.Y] = buffer2.data[neighbours[i].X, neighbours[i].Y]-1;
            }
        }

        protected Coord2[] GetNeighbours(Coord2 location)
        {
            Coord2[] neighbours = new Coord2[8];
            neighbours[0] = new Coord2(location.X + 1, location.Y + 1);
            neighbours[1] = new Coord2(location.X - 1, location.Y - 1);
            neighbours[2] = new Coord2(location.X - 1, location.Y + 1);
            neighbours[3] = new Coord2(location.X + 1, location.Y - 1);
            neighbours[4] = new Coord2(location.X, location.Y + 1);
            neighbours[5] = new Coord2(location.X + 1, location.Y);
            neighbours[6] = new Coord2(location.X - 1, location.Y);
            neighbours[7] = new Coord2(location.X, location.Y - 1);

            return neighbours;
        }

        public int GetValue(int x, int y)
        {
            return buffer1.data[x, y];
        }

        public List<Coord2> GetPath()
        {
            return null;
        }

        // Unused methods
        public void Clear() { }
        public bool IsInPath(int x, int y) { return false; }
    }
}
