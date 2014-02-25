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
    abstract class AiBotBase : Entity
    {
        public int currentPathIndex; // Current index in the path given

        //constructor: requires initial position
        public AiBotBase(Texture2D texture, int x, int y) : base(texture, x, y, 400) { }

        //sets target position: the next grid location to move to
        //need to validate this position - so must be within 1 cell of current position(in x and y directions)
        //and must also be valid on the map: greater than 0, less than mapsize, and not a wall
        public bool SetNextGridPosition(Coord2 pos, Map level)
        {
            if (pos.X < (gridPosition.X - 1)) return false;
            if (pos.X > (gridPosition.X + 1)) return false;
            if (pos.Y < (gridPosition.Y - 1)) return false;
            if (pos.Y > (gridPosition.Y + 1)) return false;
            if (!level.ValidPosition(pos)) return false;
            targetPosition = pos;
            return true;
        }

        //Handles animation moving from current grid position (gridLocation) to next grid position (targetLocation)
        //When target location is reached, sets grid location to targetLocation, and then calls ChooseNextGridLocation
        //and resets animation timer
        public virtual void Update(GameTime gameTime, Map level, Player plr)
        {
            timerMs -= gameTime.ElapsedGameTime.Milliseconds;
            if (timerMs <= 0)
            {
                gridPosition = targetPosition;
                ChooseNextGridLocation(level, plr);
                timerMs = moveTime;
            }
            //calculate screen position
            screenPosition = (gridPosition * 15) + ((((targetPosition * 15) - (gridPosition * 15)) * (moveTime - timerMs)) / moveTime);
        }

        //this function is filled in by a derived class: must use SetNextGridLocation to actually move the bot
        protected abstract void ChooseNextGridLocation(Map level, Player plr);
    }
}
