using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    class ScentMap
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
        private ScentBuffer buffer1, buffer2;
        int sourceValue;

        public ScentMap(int gridSize)
        {
            this.gridSize = gridSize;
            buffer1 = new ScentBuffer(gridSize);
            buffer2 = new ScentBuffer(gridSize);
            sourceValue = 0;
        }

        public void Update(Map lvl, Player plr)
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
                    if (lvl.ValidPosition(currentLoc)) // Ignore blocked spaces
                        ProcessLocation(currentLoc, lvl, plr);
                }
            }

            // Update data in player's location
            buffer1.data[plr.GridPosition.X, plr.GridPosition.Y] = sourceValue;
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

        private void ProcessLocation(Coord2 location, Map lvl, Player plr)
        {
            Coord2[] neighbours = GetNeighbours(location);

            for (int i = 0; i < neighbours.Length; i++)
            {
                if(lvl.ValidPosition(neighbours[i]) && plr.GridPosition != neighbours[i])
                    if (buffer2.data[neighbours[i].X, neighbours[i].Y] > buffer1.data[location.X, location.Y])
                        buffer1.data[location.X, location.Y] = buffer2.data[neighbours[i].X, neighbours[i].Y]-1;
            }
        }

        private Coord2[] GetNeighbours(Coord2 location)
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

        public ScentBuffer Buffer1 { get { return buffer1; } }
        public ScentBuffer Buffer2 { get { return buffer2; } }
    }
}
