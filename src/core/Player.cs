/*
 * File: Player.cs
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines the player entity.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;


namespace Pathfinder
{
    class Player : Entity
    {
        public Player(Texture2D texture, int x, int y) : base(texture, x, y, 200) { }

        //Handles animation moving from current grid position (gridLocation) to next grid position (targetLocation)
        public void Update(GameTime gameTime, Map map)
        {
            if (timerMs > 0)
            {
                timerMs -= gameTime.ElapsedGameTime.Milliseconds;
                if (timerMs <= 0)
                {
                    timerMs = 0;
                    gridPosition = targetPosition;
                }
            }
           
            //calculate screen position
            screenPosition = (gridPosition * 15) + ((((targetPosition * 15) - (gridPosition * 15)) * (moveTime - timerMs)) / moveTime); 
        }

        //sets next position for player to move to: called by keyboard processing functions. validates new position against level,
        //so can't move to blocked position, or position off grid
        public void SetNextLocation(Coord2 newLoc, Map level)
        {
            if (timerMs > 0) return;
            if (level.ValidPosition(newLoc))
            {
                targetPosition = newLoc;
                timerMs = moveTime;
            }
        }

    }
}
