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
    class AStar : Dijkstra
    {
        public AStar(int gridSize, Map map) : base(gridSize, map)
        {
            Name = "A Star";
        }

        protected override void RecalculateCosts(List<Coord2> neighbours, Coord2 pos)
        {
            for (int i = 0; i < neighbours.Count; i++)
            {
                if(map.ValidPosition(neighbours[i]) && nodes.Get(neighbours[i].X, neighbours[i].Y).closed == false)
                {
                    float costToAdd = 0.0f;

                    if (neighbours[i].X != 0 && neighbours[i].Y != 0)
                        costToAdd = D_COST;
                    else
                        costToAdd = HV_COST;

                   float newCost = nodes.Get(pos.X, pos.Y).cost + costToAdd;

                   if (newCost < nodes.Get(neighbours[i].X, neighbours[i].Y).cost)
                   {
                       nodes.Get(neighbours[i].X, neighbours[i].Y).cost = newCost;
                       nodes.Get(neighbours[i].X, neighbours[i].Y).link = pos;
                   }
                }
            }
        }

        protected override void FindLowestCost()
        {
            currentLowestPos = targetPos;

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    // If cost is lower than current, position not closed, and position is valid within level, new lowest is found
                    if (nodes.Get(currentLowestPos.X, currentLowestPos.Y).cost + manhattanDist(currentLowestPos, targetPos) >= nodes.Get(x, y).cost + manhattanDist(new Coord2(x, y), targetPos) &&
                        nodes.Get(x, y).closed == false && map.ValidPosition(new Coord2(x, y)))
                        currentLowestPos = new Coord2(x, y);
                }
            }
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
    }
}
