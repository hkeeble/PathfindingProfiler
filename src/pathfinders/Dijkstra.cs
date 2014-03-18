using System;
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
        protected Coord2 startPos;
        protected Coord2 targetPos;

        protected List<Coord2> path;

        protected NodeCollection nodes;

        protected const float HV_COST = 1.0f;
        protected const float D_COST = 1.4f;

        protected Coord2 currentLowestPos;

        private Profile profile;

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

        public void Build(Coord2 startPos, Coord2 targetPos)
        {
            if(InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                this.startPos = startPos;
                this.targetPos = targetPos;
                path = new List<Coord2>();

                nodes = new NodeCollection(GridSize);

                // Initialize bot position
                nodes.Get(startPos.X, startPos.Y).cost = 0;
                bool firstLoop = true;

                while (nodes.Get(targetPos.X, targetPos.Y).closed == false)
                {
                    if (firstLoop)
                    {
                        currentLowestPos = startPos;
                        firstLoop = false;
                    }
                    else
                        FindLowestCost(); // Find lowest cost

                    // Mark lowest cost as closed
                    nodes.Get(currentLowestPos.X, currentLowestPos.Y).closed = true;

                    // Find the neigbour positions
                    Coord2[] neighbours = GetNeighbours(currentLowestPos);

                    // Recalculate Costs
                    RecalculateCosts(neighbours, currentLowestPos);
                }

                // Trace the completed path
                TracePath();
            }
        }

        protected virtual List<Coord2> GetNeighbours(Coord2 location)
        {
            List<Coord2> neighbours = new List<Coord2>();

            // Diagonal neighbours
            neighbours.Add(new Coord2(location.X + 1, location.Y + 1));
            neighbours.Add(new Coord2(location.X - 1, location.Y - 1));
            neighbours.Add(new Coord2(location.X - 1, location.Y + 1));
            neighbours.Add(new Coord2(location.X + 1, location.Y - 1));

            // Horizontal neighbours
            neighbours.Add(new Coord2(location.X, location.Y + 1));
            neighbours.Add(new Coord2(location.X + 1, location.Y));
            neighbours.Add(new Coord2(location.X - 1, location.Y));
            neighbours.Add(new Coord2(location.X, location.Y - 1));

            return neighbours;
        }

        protected virtual void FindLowestCost()
        {
            currentLowestPos = targetPos;

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    // If cost is lower than current, position not closed, and position is valid within level, new lowest is found
                    if (nodes.Get(currentLowestPos.X, currentLowestPos.Y).cost >= nodes.Get(x, y).cost && nodes.Get(x, y).closed == false &&
                        map.ValidPosition(new Coord2(x, y)))
                            currentLowestPos = new Coord2(x, y);
                }
            }
        }

        protected virtual void RecalculateCosts(Coord2[] neighbours, Coord2 pos)
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

                   float newCost = nodes.Get(pos.X, pos.Y).cost + costToAdd;

                   if (newCost < nodes.Get(neighbours[i].X, neighbours[i].Y).cost)
                   {
                       nodes.Get(neighbours[i].X, neighbours[i].Y).cost = newCost;
                       nodes.Get(neighbours[i].X, neighbours[i].Y).link = pos;
                   }
                }
            }
        }

        protected void TracePath()
        {
            bool done = false;
            Coord2 nextClosed = targetPos;
            while (!done)
            {
                nodes.Get(nextClosed.X, nextClosed.Y).inPath = true;
                path.Add(nextClosed);
                nextClosed = nodes.Get(nextClosed.X, nextClosed.Y).link;
                if (nextClosed == startPos)
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
