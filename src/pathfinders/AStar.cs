using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO; 

namespace Pathfinder
{
    class AStar
    {
        private NodeCollection nodes;

        private int gridSize;

        private const float HV_COST = 1.0f;
        private const float D_COST = 1.4f;

        private Map level;
        private AiBotBase bot;
        private Player plr;

        private Coord2 currentLowestPos;

        private List<Coord2> path;

        public AStar(int gridSize)
        {
            nodes = new NodeCollection(gridSize);

            this.gridSize = gridSize;
        }

        public void Build(Map level, AiBotBase bot, Player plr)
        {
            path = new List<Coord2>();

            this.level = level;
            this.bot = bot;
            this.plr = plr;

            nodes = new NodeCollection(gridSize);

            // Initialize bot position
            nodes.Get(bot.GridPosition.X, bot.GridPosition.Y).cost = 0;
            bool firstLoop = true;

            while (nodes.Get(plr.GridPosition.X, plr.GridPosition.Y).closed == false)
            {
                if (firstLoop)
                {
                    currentLowestPos = bot.GridPosition;
                    firstLoop = false;
                }
                else
                    FindLowestCost(); // Find lowest cost

                // Mark lowest cost as closed
                nodes.Get(currentLowestPos.X, currentLowestPos.Y).closed = true;

                // Find the neigbour positions
                Coord2[] neighbours = FindNeighbours(currentLowestPos);

                // Recalculate Costs
                RecalculateCosts(neighbours, currentLowestPos);
            }

            // Trace the completed path
            TracePath();
        }

        private void FindLowestCost()
        {
            currentLowestPos = plr.GridPosition;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    // If cost is lower than current, position not closed, and position is valid within level, new lowest is found
                    if (nodes.Get(currentLowestPos.X, currentLowestPos.Y).cost >= nodes.Get(x, y).cost && nodes.Get(x, y).closed == false &&
                        level.ValidPosition(new Coord2(x, y)))
                            currentLowestPos = new Coord2(x, y);
                }
            }
        }

        private Coord2[] FindNeighbours(Coord2 pos)
        {
            Coord2[] neighbours = new Coord2[8];
            neighbours[0] = new Coord2(pos.X + 1, pos.Y + 1);
            neighbours[1] = new Coord2(pos.X - 1, pos.Y - 1);
            neighbours[2] = new Coord2(pos.X - 1, pos.Y + 1);
            neighbours[3] = new Coord2(pos.X + 1, pos.Y - 1);
            neighbours[4] = new Coord2(pos.X, pos.Y + 1);
            neighbours[5] = new Coord2(pos.X + 1, pos.Y);
            neighbours[6] = new Coord2(pos.X - 1, pos.Y);
            neighbours[7] = new Coord2(pos.X, pos.Y - 1);

            return neighbours;
        }

        private void RecalculateCosts(Coord2[] neighbours, Coord2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                if(level.ValidPosition(neighbours[i]) && nodes.Get(neighbours[i].X, neighbours[i].Y).closed == false)
                {
                    float costToAdd = 0.0f;

                    if (neighbours[i].X != 0 && neighbours[i].Y != 0)
                        costToAdd = D_COST;
                    else
                        costToAdd = HV_COST;

                   float newCost = nodes.Get(pos.X, pos.Y).cost + costToAdd + manhattanDist(neighbours[i], currentLowestPos);

                   if (newCost < nodes.Get(neighbours[i].X, neighbours[i].Y).cost)
                   {
                       nodes.Get(neighbours[i].X, neighbours[i].Y).cost = newCost;
                       nodes.Get(neighbours[i].X, neighbours[i].Y).link = pos;
                   }
                }
            }
        }

        private void TracePath()
        {
            bool done = false;
            Coord2 nextClosed = plr.GridPosition;
            while (!done)
            {
                nodes.Get(nextClosed.X, nextClosed.Y).inPath = true;
                path.Add(nextClosed);
                nextClosed = nodes.Get(nextClosed.X, nextClosed.Y).link;
                if (nextClosed == bot.GridPosition)
                    done = true;
            }
        }

        public bool IsInPath(int x, int y)
        {
            if (nodes != null)
                return nodes.Get(x, y).inPath;
            else
                return false;
        }

        private int manhattanDist(Coord2 playerPos, Coord2 currentPos)
        {
            Coord2 dist = playerPos - currentPos;
            if (dist.X < 0)
                dist.X *= -1;
            if (dist.Y < 0)
                dist.Y *= -1;
            return dist.X + dist.Y;
        }

        public List<Coord2> Path { get { return path; } }
    }
}
