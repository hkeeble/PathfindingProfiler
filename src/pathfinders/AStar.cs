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
        public AStar(int gridSize) : base(gridSize)
        {
            nodes = new NodeCollection(gridSize);
        }

        protected override void RecalculateCosts(Coord2[] neighbours, Coord2 pos)
        {
            for (int i = 0; i < 8; i++)
            {
                if(map.ValidPosition(neighbours[i]) && nodes.Get(neighbours[i].X, neighbours[i].Y).closed == false)
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
