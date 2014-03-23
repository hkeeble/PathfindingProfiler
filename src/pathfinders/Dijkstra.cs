﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pathfinder
{
    class Dijkstra : IPathfinder
    {
        protected int GridSize { get; set; }
        protected string Name { get; set; }

        protected Map map;
        protected Node start;
        protected Node target;

        protected List<Coord2> path;

        protected NodeCollection nodes;

        protected const float HV_COST = 1.0f;
        protected const float D_COST = 1.4f;

        protected Node currentLowest;

        private Profile profile;

        // Visualization Colors
        private Color CLOSED_COLOR    = Color.Cyan;
        private Color NEIGHBOUR_COLOR = Color.Green;
        private Color PATH_COLOR      = Color.Red;

        public Dijkstra(int gridSize, Map map)
        {
            Name = "Dijkstra";
            profile = new Profile("Dijkstra Path Time");
            GridSize = gridSize;
            nodes = new NodeCollection(gridSize);
            path = new List<Coord2>();
            this.map = map;
        }

        public virtual string GetName()
        {
            return Name;
        }

        public virtual void Build(Coord2 startPos, Coord2 targetPos)
        {
            if(InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                path = new List<Coord2>();
                nodes = new NodeCollection(GridSize);

                this.start = nodes.Get(startPos);
                this.target = nodes.Get(targetPos);

                // Initialize bot position
                nodes.Get(startPos).cost = 0;
                bool firstLoop = true;

                while (nodes.Get(targetPos).closed == false)
                {
                    if (firstLoop)
                    {
                        currentLowest = start;
                        firstLoop = false;
                    }
                    else
                        FindLowestCost(); // Find lowest cost

                    // Mark lowest cost as closed
                    currentLowest.closed = true;
                    map.SetRenderColor(currentLowest.position, CLOSED_COLOR);

                    // Find the neigbour positions
                    List<Node> neighbours = GetNeighours(currentLowest);

                    // Update visualization
                    UpdateVisualization(neighbours);

                    // Recalculate Costs
                    RecalculateCosts(neighbours, currentLowest);
                }

                // Trace the completed path
                TracePath();
            }
        }

        protected virtual List<Node> GetNeighours(Node node)
        {
            List<Node> list = new List<Node>();

            // Horizontal and vertical
            if (nodes.IsValid(node.position + new Coord2(1, 0)))
                list.Add(nodes.Get(node.position + new Coord2(1, 0)));

            if (nodes.IsValid(node.position + new Coord2(-1, 0)))
                list.Add(nodes.Get(node.position + new Coord2(-1, 0)));

            if (nodes.IsValid(node.position + new Coord2(0, 1)))
                list.Add(nodes.Get(node.position + new Coord2(0, 1)));

            if (nodes.IsValid(node.position + new Coord2(0, -1)))
                list.Add(nodes.Get(node.position + new Coord2(0, -1)));

            // Diagonal
            if (nodes.IsValid(node.position + new Coord2(1, 1)))
                list.Add(nodes.Get(node.position + new Coord2(1, 1)));

            if (nodes.IsValid(node.position + new Coord2(-1, -1)))
                list.Add(nodes.Get(node.position + new Coord2(-1, -1)));

            if (nodes.IsValid(node.position + new Coord2(1, -1)))
                list.Add(nodes.Get(node.position + new Coord2(1, -1)));

            if (nodes.IsValid(node.position + new Coord2(-1, 1)))
                list.Add(nodes.Get(node.position + new Coord2(-1, 1)));

            return list;
        }

        /// <summary>
        /// Updates the current visualization.
        /// </summary>
        /// <param name="currentNeighbours">The neighbours on the current search iteration.</param>
        protected virtual void UpdateVisualization(List<Node> currentNeighbours)
        {
            for (int i = 0; i < currentNeighbours.Count; i++)
            {
                if(map.ValidPosition(currentNeighbours[i].position))
                {
                    if (currentNeighbours[i].closed == false)
                        map.SetRenderColor(currentNeighbours[i].position, NEIGHBOUR_COLOR); 
                }
            }
        }

        protected virtual void FindLowestCost()
        {
            currentLowest = target;

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    // If cost is lower than current, position not closed, and position is valid within level, new lowest is found
                    if (nodes.Get(currentLowest.position).cost >= nodes.Get(x, y).cost && nodes.Get(x, y).closed == false &&
                        map.ValidPosition(new Coord2(x, y)))
                            currentLowest = nodes.Get(x, y);
                }
            }
        }

        protected virtual void RecalculateCosts(List<Node> neighbours, Node node)
        {
            for (int i = 0; i < neighbours.Count; i++)
            {
                if(map.ValidPosition(neighbours[i].position) && neighbours[i].closed == false)
                {
                    float costToAdd = 0.0f;

                    if (neighbours[i].position.X != 0 && neighbours[i].position.Y != 0)
                        costToAdd = D_COST;
                    else
                        costToAdd = HV_COST;

                   float newCost = nodes.Get(node.position).cost + costToAdd;

                   if (newCost < neighbours[i].cost)
                   {
                       neighbours[i].cost = newCost;
                       neighbours[i].parent = node;
                   }
                }
            }
        }

        protected void TracePath()
        {
            bool done = false;
            Coord2 nextClosed = target.position;
            while (!done)
            {
                nodes.Get(nextClosed).inPath = true;
                path.Add(nextClosed);
                nextClosed = nodes.Get(nextClosed).parent.position;
                map.SetRenderColor(nextClosed, PATH_COLOR);
                if (nextClosed == start.position)
                    done = true;
            }
        }

        public bool IsInPath(int x, int y)
        {
            return path.Contains(new Coord2(x, y));
        }

        public List<Coord2> Path { get { return path; } }

        public void Clear()
        {
            path.Clear();
            nodes.Clear();
        }

        public int GetValue(int x, int y)
        {
            return Convert.ToInt16(nodes.Get(x, y).closed);
        }

        public List<Coord2> GetPath()
        {
            return path;
        }

        // Unused interface methods
        public int LowestValue() { return 0; }
        public int HighestValue() { return 0; }
    }
}
