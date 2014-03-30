using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class Dijkstra : Pathfinder
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

        // Lines drawn to connect nodes within the path
        List<Line> pathConnectors;

        public Dijkstra(int gridSize, Map map)
        {
            Name = "Dijkstra";
            profile = new Profile("Dijkstra Path Time");
            GridSize = gridSize;
            nodes = new NodeCollection(gridSize);
            path = new List<Coord2>();
            pathConnectors = new List<Line>();
            this.map = map;
        }

        public override string GetName()
        {
            return Name;
        }

        public override void Build(Coord2 startPos, Coord2 targetPos)
        {
            path = new List<Coord2>();
            nodes = new NodeCollection(GridSize);

            this.start = nodes.Get(startPos);
            this.target = nodes.Get(targetPos);

            // Initialize bot position
            nodes.Get(startPos).cost = 0;
            bool firstLoop = true;

            while (!target.closed)
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

            // Trace the completed path, if target has been found
            if(target.parent != null)
                TracePath();
        }

        protected virtual List<Node> GetNeighours(Node node)
        {
            List<Node> list = new List<Node>();

            // Horizontal and vertical
            if (map.ValidPosition(node.position + new Coord2(1, 0)))
                list.Add(nodes.Get(node.position + new Coord2(1, 0)));

            if (map.ValidPosition(node.position + new Coord2(-1, 0)))
                list.Add(nodes.Get(node.position + new Coord2(-1, 0)));

            if (map.ValidPosition(node.position + new Coord2(0, 1)))
                list.Add(nodes.Get(node.position + new Coord2(0, 1)));

            if (map.ValidPosition(node.position + new Coord2(0, -1)))
                list.Add(nodes.Get(node.position + new Coord2(0, -1)));

            // Diagonal
            if (map.ValidPosition(node.position + new Coord2(1, 1)))
                list.Add(nodes.Get(node.position + new Coord2(1, 1)));

            if (map.ValidPosition(node.position + new Coord2(-1, -1)))
                list.Add(nodes.Get(node.position + new Coord2(-1, -1)));

            if (map.ValidPosition(node.position + new Coord2(1, -1)))
                list.Add(nodes.Get(node.position + new Coord2(1, -1)));

            if (map.ValidPosition(node.position + new Coord2(-1, 1)))
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
                if (nextClosed == start.position)
                    done = true;
            }

            // Create path line
            pathConnectors.Clear();
            for (int i = 0; i < path.Count; i++)
            {
                Point lstart;
                Point lend;

                if (i + 1 < path.Count)
                {
                    lstart = new Point(path[i].X * map.TileSize + (map.TileSize / 2), path[i].Y * map.TileSize + (map.TileSize / 2));
                    lend = new Point(path[i + 1].X * map.TileSize + (map.TileSize / 2), path[i + 1].Y * map.TileSize + (map.TileSize / 2));
                }
                else
                {
                    lstart = new Point(path[i].X * map.TileSize + (map.TileSize / 2), path[i].Y * map.TileSize + (map.TileSize / 2));
                    lend = new Point(start.position.X * map.TileSize + (map.TileSize / 2), start.position.Y * map.TileSize + (map.TileSize / 2));
                }

                pathConnectors.Add(new Line(lstart, lend, Color.Red, 3, GameUtils.GetUtil<GraphicsDevice>()));
            }
        }

        public override void DrawPath(SpriteBatch sb)
        {
            if(pathConnectors.Count > 0)
                foreach (Line l in pathConnectors)
                    l.Draw(sb);
        }

        public override bool IsInPath(int x, int y)
        {
            return path.Contains(new Coord2(x, y));
        }

        public List<Coord2> Path { get { return path; } }

        public override void Clear()
        {
            path.Clear();
            nodes.Clear();
            pathConnectors.Clear();
        }

        public override bool IsClosed(int x, int y)
        {
            return nodes.Get(x, y).closed;
        }

        public override bool IsClosed(Coord2 coord)
        {
            return nodes.Get(coord.X, coord.Y).closed;
        }

        public override List<Coord2> GetPath()
        {
            return path;
        }

        // Unused interface methods
        public int LowestValue() { return 0; }
        public int HighestValue() { return 0; }
    }
}
