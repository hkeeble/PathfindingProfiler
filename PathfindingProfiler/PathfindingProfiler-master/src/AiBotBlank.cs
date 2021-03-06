﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class AiBotBlank : AiBotBase
    {
        public AiBotBlank(Texture2D texture, int x, int y) : base(texture, x, y) { }

        /// <summary>
        /// Sets target position: the next grid location to move to
        /// </summary>
        /// <param name="pos">Next grid position. Must be within 1 cell of current position(in x and y directions) and a valid map position.</param>
        /// <param name="level">The map being moved on.</param>
        /// <returns></returns>
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

        protected override void ChooseNextGridLocation(Map level, Player plr)
        {
            SetNextGridPosition(gridPosition, level);
        }
    }
}
