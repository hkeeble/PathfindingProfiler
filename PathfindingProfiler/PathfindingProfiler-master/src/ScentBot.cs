using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class ScentBot : AiBotBase
    {
        public ScentBot(Texture2D texture, int x, int y)
            : base(texture, x, y)
        {

        }

        protected override void ChooseNextGridLocation(Map level, Player plr)
        {
            //Coord2[] neighbours = GetNeighbours();

            //int cVal = 0;
            //Coord2 cPos = new Coord2(0, 0);
            //for (int i = 0; i < neighbours.Length; i++)
            //{
            //    if(level.ValidPosition(neighbours[i]))
            //    {
            //        int lVal = level.scentMap.Buffer1.data[neighbours[i].X, neighbours[i].Y];
            //        if (lVal > cVal)
            //        {
            //            cVal = lVal;
            //            cPos = new Coord2(neighbours[i].X, neighbours[i].Y);
            //        }
            //    }
            //}

            //SetNextGridPosition(cPos, level);
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
