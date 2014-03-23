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
            if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
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
                    List<Coord2> neighbours = GetNeighbours(currentLowestPos);

                    // Recalculate Costs
                    RecalculateCosts(neighbours, currentLowestPos);
                }

                // Trace the completed path
                TracePath();
            }
        }

        protected override List<Coord2> GetNeighbours(Coord2 location)
        {
            List<Coord2> successors = new List<Coord2>();
            List<Coord2> neighbours = base.GetNeighbours(location);

            foreach (Coord2 node in neighbours)
            {
                // Get direction from current node to neighbour
                Vector2 dirToNeighbour = new Vector2(MathHelper.Clamp(node.X - location.X, -1, 1),
                    MathHelper.Clamp(node.Y - location.Y, -1, 1));

                // Call Jump and search for a jump point
                Coord2? jumpPoint = Jump(location, dirToNeighbour);
                if (jumpPoint.HasValue)
                    successors.Add(new Coord2(jumpPoint.Value.X, jumpPoint.Value.Y));
            }

            UpdateVisualization(successors);

            return successors;
        }

        protected override void UpdateVisualization(List<Coord2> currentSuccessors)
        {
            foreach(Coord2 c in currentSuccessors)
                map.SetRenderColor(c, Color.Red);
        }

        private Coord2? Jump(Coord2 pos, Vector2 dir)
        {
            // Position of the new node that will be considered
            Coord2 newLoc = new Coord2(pos.X + (int)dir.X, pos.Y + (int)dir.Y);

            // If the position is blocked or invalid it cannot be jumped to
            if (LevelHandler.Level.Map.ValidPosition(newLoc) == false)
                return null;

            // Check if the node that is found is actually the goal
            if (targetPos == newLoc)
                return newLoc;

            if(dir.X == 0 || dir.Y == 0) // Horizontal/Vertical checks
            {
                if (dir.X != 0)
                {
                    if (ForcedCheck(newLoc, dir))
                        return newLoc;
                }
                else
                {
                    if (ForcedCheck(newLoc, dir))
                        return newLoc;
                }
            }
            else if(dir.X != 0 && dir.Y != 0) // Node is diagonal from parent
            {
                if(ForcedCheck(newLoc, dir))

                /* Recursive check for horizontal/vertical directions */
                    if(Jump(newLoc, new Vector2(dir.X, 0)) != null ||
                        Jump(newLoc, new Vector2(0, dir.Y)) != null)
                        return newLoc;
            }

            // If no forced neighbout, resursively call into new jump point
            return Jump(newLoc, dir);
        }

        bool ForcedCheck(Coord2 pos, Vector2 dir)
        {
            if(dir.X == 0 || dir.Y == 0)
            {
                // Diagonal checks
                if (!map.ValidPosition(pos + new Coord2(1, 1)))
                    return true;
                else if (!map.ValidPosition(pos + new Coord2(1, -1)))
                    return true;
                else if (!map.ValidPosition(pos + new Coord2(-1, 1)))
                    return true;
                else if (!map.ValidPosition(pos + new Coord2(-1, -1)))
                    return true;

                // Horizontal/Vertical checks
                if(dir.X != 0)
                {
                    if (!map.ValidPosition(pos + new Coord2(0, 1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(0, -1)))
                        return true;
                }
                else
                {
                    if (!map.ValidPosition(pos + new Coord2(1, 0)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(-1, 0)))
                        return true;
                }
            }
            else if (dir.X != 0 && dir.Y != 0) // Diagonal Checks
            {
                // Decide which diagonal direction we are moving in
                if(dir.X == -1 && dir.Y == -1) // Up to the left
                {
                    if (!map.ValidPosition(pos + new Coord2(1, 0)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(0, -1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(1, -1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(-1, 1)))
                        return true;
                }
                else if(dir.X == 1 && dir.Y == -1) // Up to the right
                {
                    if (!map.ValidPosition(pos + new Coord2(-1, 0)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(0, -1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(-1, -1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(1, 1)))
                        return true;
                }
                else if(dir.X == -1 && dir.Y == 1) // Down to the left
                {
                    if (!map.ValidPosition(pos + new Coord2(1, 0)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(0, -1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(-1, -1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(1, 1)))
                        return true;
                }
                else if(dir.X == 1 && dir.Y == 1) // Down to the right
                {
                    if (!map.ValidPosition(pos + new Coord2(-1, 0)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(0, 1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(1, 1)))
                        return true;
                    else if (!map.ValidPosition(pos + new Coord2(-1, 1)))
                        return true;
                }
            }

            // Only reached if there are no forced neighbours
            return false;
        }
    }
}
