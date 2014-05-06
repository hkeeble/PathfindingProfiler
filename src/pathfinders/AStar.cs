/*
 * File: AStar.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a class that employs the AStar pathfinding algorithm.
 * */
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
    /// <summary>
    /// A class that uses the AStar pathfinding algorithm to find a path between two given points.
    /// </summary>
    class AStar : Dijkstra
    {
        /// <summary>
        /// Create a new AStar pathfinder object.
        /// </summary>
        /// <param name="gridSize">The size of the grid being used.</param>
        /// <param name="map">The map object being used.</param>
        public AStar(int gridSize, Map map) : base(gridSize, map)
        {
            Name = "A Star";
        }

        /// <summary>
        /// Recalcualtes the costs in a set of neighbours based upon the given parent node.
        /// </summary>
        /// <param name="neighbours">The neighbours to recalculate costs for.</param>
        /// <param name="node">The parent node to use.</param>
        protected override void RecalculateCosts(List<Node> neighbours, Node node)
        {
            for (int i = 0; i < neighbours.Count; i++)
            {
                if(map.ValidPosition(neighbours[i].position) && neighbours[i].closed == false)
                {
                    float costToAdd = 0.0f;

                    Vector2 dirToNode = new Vector2(MathHelper.Clamp(neighbours[i].position.X - node.position.X, -1, 1),
                        MathHelper.Clamp(neighbours[i].position.Y - node.position.Y, -1, 1));

                    if (dirToNode.X != 0 && dirToNode.Y != 0)
                        costToAdd = D_COST;
                    else
                        costToAdd = HV_COST;

                   float newCost = nodes.Get(node.position.X, node.position.Y).cost + costToAdd;

                   if (newCost < neighbours[i].cost)
                   {
                       neighbours[i].cost = newCost;
                       neighbours[i].parent = node;
                   }
                }
            }
        }

        /// <summary>
        /// Finds the node with the lowest cost.
        /// </summary>
        protected override void FindLowestCost()
        {
            currentLowest = target;

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    // If cost is lower than current, position not closed, and position is valid within level, new lowest is found
                    if (nodes.Get(currentLowest.position.X, currentLowest.position.Y).cost + manhattanDist(currentLowest.position, target.position) >= nodes.Get(x, y).cost + manhattanDist(new Coord2(x, y), target.position) &&
                        nodes.Get(x,y).closed == false && map.ValidPosition(new Coord2(x, y)))
                        currentLowest = nodes.Get(x, y);
                }
            }
        }

        /// <summary>
        /// Finds the manhattan distance between two locations.
        /// </summary>
        protected int manhattanDist(Coord2 playerPos, Coord2 currentPos)
        {
            Coord2 dist = playerPos - currentPos;
            if (dist.X < 0)
                dist.X *= -1;
            if (dist.Y < 0)
                dist.Y *= -1;
            return dist.X + dist.Y;
        }

        public override PathfinderAlgorithm GetAlgorithm()
        {
            return PathfinderAlgorithm.AStar;
        }
    }
}
