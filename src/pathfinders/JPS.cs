using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pathfinder.pathfinders
{
    class JPS : AStar
    {
        protected List<Coord2> GetSuccessors(Coord2 location)
        {
            List<Coord2> successors = new List<Coord2>();
            List<Coord2> neighbours = GetNeighbours(location);

            foreach (Coord2 node in neighbours)
            {
                // Get direction from current node to neighbour
                Vector2 dirToNeighbour = new Vector2(MathHelper.Clamp(node.X - location.X, -1, 1),
                    MathHelper.Clamp(node.Y - location.Y, -1, 1));

                Coord2 jumpPoint = Jump(location, dirToNeighbour);

                if (jumpPoint != null)
                    successors.Add(jumpPoint);
            }

            return successors;
        }

        private Coord2 Jump(Coord2 pos, Vector2 dir)
        {
            // Position of the new node that will be considered
            Coord2 newLoc = new Coord2(pos.X + (int)dir.X, pos.Y + (int)dir.Y);

            // If the position is blocked it cannot be jumped to
            if (LevelHandler.Level.Map.ValidPosition(newLoc) == false)
                return null;

            // Check if the node that is found is actually the goal
            if (targetPos == newLoc)
                return newLoc;

            if(dir.X != 0 && dir.Y != 0) // Node is diagonal from parent
            {
                if(/* check for forced neighbours here */)

                /* Recursive check for horizontal/vertical directions */
                    if(Jump(newLoc, new Vector2(dir.X, 0)) != null ||
                        Jump(newLoc, new Vector2(0, dir.Y)) != null)
                        return newLoc;
            }
            else
            {
                if(dir.X != 0)
                {
                    if(/* Horizontal forced neighbour check */)
                        return newLoc;
                }
                else
                {
                    if(/* Vertical check for forced neighbours here */)
                        return newLoc;
                }
            }

            // If no forced neighbout, resursively call into new jump point
            return Jump(newLoc, dir);
        }
    }
}
