using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    /* Represents an entity within game space */
    abstract class Entity
    {
        protected Texture2D texture;
        protected Coord2 gridPosition;
        protected Coord2 targetPosition;
        protected Coord2 screenPosition;
        protected int moveTime;
        protected int timerMs;

        public Entity(Texture2D texture, int x, int y, int moveTime)
        {
            gridPosition = new Coord2(x, y);
            targetPosition = new Coord2(x, y);
            screenPosition = new Coord2(x, y);
            this.texture = texture;
            this.moveTime = moveTime;
            timerMs = moveTime;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, screenPosition, Color.White);
        }

        public void SetPosition(Coord2 pos)
        {
            gridPosition = pos;
            targetPosition = pos;
        }

        public Coord2 GridPosition { get { return gridPosition; } }
        public Coord2 ScreenPosition { get { return screenPosition; } }
    }
}
