/*
 * File: AiBotScent.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a bot able to follow a scent map path.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class AiBotScent : AiBotBase
    {
        public AiBotScent(Texture2D texture, int x, int y)
            : base(texture, x, y)
        {

        }

        protected override void ChooseNextGridLocation(Map level, Player plr)
        {
            try
            {
                if (level.pathfinder.GetAlgorithm() != PathfinderAlgorithm.ScentMap)
                    throw new Exception("Wrong bot used for pathfinding algorithm.");

                // Get a copy of the scent map
                ScentMap map = (level.pathfinder as ScentMap);

                // Get neighbouring coordinates
                Coord2[] neighbours = GetNeighbours();

                Coord2 highest = new Coord2(0, 0);
                bool validFound = false;
                for (int i = 0; i < neighbours.Length; i++) // Ensures a valid position is initially selected
                {
                    if (level.ValidPosition(neighbours[i]))
                    {
                        highest = neighbours[i];
                        validFound = true;
                        break;
                    }
                }

                if (validFound) // Only update if at least one neighbouring positon is in fact valid on the map
                {
                    // Find the highest neighbouring coorindate
                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        if (level.ValidPosition(neighbours[i]))
                        {
                            if (map.Scents.data[neighbours[i].X, neighbours[i].Y] > map.Scents.data[highest.X, highest.Y])
                                highest = neighbours[i];
                        }
                    }

                    SetNextGridPosition(highest, level);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bot Exception: " + e.Message);
            }
        }

        private Coord2[] GetNeighbours()
        {
            Coord2[] neighbours = new Coord2[8];
            neighbours[0] = new Coord2(GridPosition.X + 1, GridPosition.Y + 1);
            neighbours[1] = new Coord2(GridPosition.X - 1, GridPosition.Y - 1);
            neighbours[2] = new Coord2(GridPosition.X - 1, GridPosition.Y + 1);
            neighbours[3] = new Coord2(GridPosition.X + 1, GridPosition.Y - 1);
            neighbours[4] = new Coord2(GridPosition.X, GridPosition.Y + 1);
            neighbours[5] = new Coord2(GridPosition.X + 1, GridPosition.Y);
            neighbours[6] = new Coord2(GridPosition.X - 1, GridPosition.Y);
            neighbours[7] = new Coord2(GridPosition.X, GridPosition.Y - 1);

            return neighbours;
        }
    }
}
