using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Pathfinder
{
    class JPS : AStar
    {
        public JPS(int gridSize, Map map) : base(gridSize, map)
        {
            Name = "Jump Point Search";
        }

        public override void Build(Coord2 startPos, Coord2 targetPos)
        {
            path = new List<Coord2>();
            nodes = new NodeCollection(GridSize);

            this.start = nodes.Get(startPos.X, startPos.Y);
            this.target = nodes.Get(targetPos.X, targetPos.Y);

            // Initialize bot position
            nodes.Get(startPos.X, startPos.Y).cost = 0;
            bool firstLoop = true;

            while (nodes.Get(targetPos.X, targetPos.Y).closed == false)
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

                // Find the neigbour positions
                List<Node> successors = GetSuccessors(currentLowest);

                foreach (Node n in successors)
                    map.SetRenderColor(n.position, Color.Cyan);

                if(successors.Contains(target))
                {
                    target.closed = true;
                    target.parent = currentLowest;
                    break;
                }

                // Recalculate Costs
                RecalculateCosts(successors, currentLowest);
            }

            // Trace the completed path
            TracePath();
        }

        protected List<Node> GetSuccessors(Node node)
        {
            List<Node> successors = new List<Node>();
            List<Node> neighbours = GetNeighours(node);

            foreach (Node neighbour in neighbours)
            {
                // Get direction from current node to neighbour
                Vector2 dirToNeighbour = new Vector2(MathHelper.Clamp(neighbour.position.X - node.position.X, -1, 1),
                    MathHelper.Clamp(neighbour.position.Y - node.position.Y, -1, 1));

                if (map.ValidPosition(node.position))
                {
                    // Call Jump and search for a jump point
                    Coord2? jumpPoint = Jump(node.position, dirToNeighbour);
                    if (jumpPoint.HasValue)
                    {
                        nodes.Get(jumpPoint.Value).parent = node;
                        successors.Add(nodes.Get(jumpPoint.Value));
                    }
                }
            }

            return successors;
        }

        /// <summary>
        /// Get Neighbour override that prunes unneccesary neighbours
        /// </summary>
        /// <param name="node">The node to find the neighbours for.</param>
        /// <returns></returns>
        protected override List<Node> GetNeighours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            // Only prune if the node has a parent
            if (node.parent != null)
            {
                // Get direction to parent
                int dx = (node.position.X - node.parent.position.X) / Math.Max(Math.Abs(node.position.X - node.parent.position.X), 1);
                int dy = (node.position.Y - node.parent.position.Y) / Math.Max(Math.Abs(node.position.Y - node.parent.position.Y), 1);

                if(dx != 0 && dy != 0) // Diagonal search
                {
                    if (!map.IsBlocked(node.position.X, node.position.Y + (int)dy))
                        neighbours.Add(nodes.Get(node.position.X, node.position.Y + dy));

                    if (!map.IsBlocked(node.position.X + dx, node.position.Y))
                        neighbours.Add(nodes.Get(node.position.X + dx, node.position.Y));

                    if(!map.IsBlocked(node.position.X, node.position.Y + dy) ||
                        !map.IsBlocked(node.position.X + dx, node.position.Y))
                        neighbours.Add(nodes.Get(node.position.X + dx, node.position.Y + dy));

                    if (map.IsBlocked(node.position.X - dx, node.position.Y) &&
                        !map.IsBlocked(node.position.X, node.position.Y + dy))
                        neighbours.Add(nodes.Get(node.position.X - dx, node.position.Y + dy));

                    if (map.IsBlocked(node.position.X, node.position.Y - dy) &&
                        !map.IsBlocked(node.position.X + dx, node.position.Y))
                        neighbours.Add(nodes.Get(node.position.X + dx, node.position.Y - dy));
                }
                else // Horizontal and vertical searches
                {
                    if(dx == 0)
                    {
                        if (!map.IsBlocked(node.position.X, node.position.Y + dy))
                        {
                            if (!map.IsBlocked(node.position.X, node.position.Y + dy))
                                neighbours.Add(nodes.Get(node.position.X, node.position.Y + dy));
                            if (map.IsBlocked(node.position.X + 1, node.position.Y))
                                neighbours.Add(nodes.Get(node.position.X + 1, node.position.Y + dy));
                            if (map.IsBlocked(node.position.X - 1, node.position.Y))
                                neighbours.Add(nodes.Get(node.position.X + 1, node.position.Y + dy));
                        }
                    }
                    else
                    {
                        if (!map.IsBlocked(node.position.X, node.position.Y + dy))
                        {
                            if (!map.IsBlocked(node.position.X + dx, node.position.Y))
                                neighbours.Add(nodes.Get(node.position.X + dx, node.position.Y));
                            if (map.IsBlocked(node.position.X, node.position.Y + 1))
                                neighbours.Add(nodes.Get(node.position.X + dx, node.position.Y + 1));
                            if (map.IsBlocked(node.position.X, node.position.Y - 1))
                                neighbours.Add(nodes.Get(node.position.X + dx, node.position.Y - 1));
                        }
                    }


                }
            }
            else // If no parent, return all neighbours
                neighbours = base.GetNeighours(node);

            return neighbours;
        }

        private Coord2? Jump(Coord2 pos, Vector2 dir)
        {
            if(nodes.Get(pos.X, pos.Y).closed == false)
                map.SetRenderColor(pos, Color.GhostWhite);

            // Position of the new node that will be considered
            Coord2 newLoc = new Coord2(pos.X + (int)dir.X, pos.Y + (int)dir.Y);

            // If the position is blocked or invalid it cannot be jumped to
            if (LevelHandler.Level.Map.ValidPosition(newLoc) == false)
                return null;

            // Check if the node that is found is actually the goal
            if (target.position == newLoc)
                return newLoc;

            if (dir.X != 0 && dir.Y != 0) // Node is diagonal from parent
            {
                if ((!map.IsBlocked(new Coord2(newLoc.X - (int)dir.X, newLoc.Y + (int)dir.Y)) && map.IsBlocked(new Coord2(newLoc.X - (int)dir.X, newLoc.Y))) ||
                    (!map.IsBlocked(new Coord2(newLoc.X + (int)dir.X, newLoc.Y - (int)dir.Y)) && map.IsBlocked(new Coord2(newLoc.X, newLoc.Y - (int)dir.Y))))
                    return newLoc;
            }

            else
            {
                if (dir.X != 0)
                {
                    if ((!map.IsBlocked(new Coord2(newLoc.X + (int)dir.X, newLoc.Y + 1)) && map.IsBlocked(new Coord2(newLoc.X, newLoc.Y + 1))) ||
                         (!map.IsBlocked(new Coord2(newLoc.X + (int)dir.X, newLoc.Y - 1)) && map.IsBlocked(new Coord2(newLoc.X, newLoc.Y - 1))))
                        return newLoc;
                }
                else
                {
                    if ((!map.IsBlocked(new Coord2(newLoc.X + 1, newLoc.Y + (int)dir.Y)) && map.IsBlocked(new Coord2(newLoc.X + 1, newLoc.Y))) ||
                         (!map.IsBlocked(new Coord2(newLoc.X - 1, newLoc.Y + (int)dir.Y)) && map.IsBlocked(new Coord2(newLoc.X - 1, newLoc.Y))))
                        return newLoc;
                }
            }

            if (dir.X != 0 && dir.Y != 0)
            {
                /* Recursive check for horizontal/vertical directions */
                if (Jump(newLoc, new Vector2(dir.X, 0)) != null ||
                    Jump(newLoc, new Vector2(0, dir.Y)) != null)
                    return newLoc;
            }

            // If no forced neighbour, resursively call into new jump point
            return Jump(newLoc, dir);
        }
    }
}
