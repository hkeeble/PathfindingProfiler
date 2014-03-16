using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    /* Represents an object with text on a background texture, derive for more specific functionality */
    abstract class TextMenuItem
    {
        protected Texture2D texture;
        protected string text;
        protected SpriteFont font;
        protected Rectangle screenRect;
        protected Vector2 textPos, textOrigin;

        public TextMenuItem(Vector2 position, Vector2 dimensions, Texture2D texture, string text, SpriteFont font)
        {
            // Set parameters
            this.screenRect = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);
            this.texture    = texture;
            this.text       = text;
            this.font       = font;

            // Calculate text position and origin
            CalculateTextPosition(text);
        }

        /* Calculates a new textPos and textOrigin using the given string */
        protected void CalculateTextPosition(string newText)
        {
            textPos = new Vector2(screenRect.X + (screenRect.Width / 2), screenRect.Y + (screenRect.Height / 2));
            textOrigin = new Vector2(font.MeasureString(newText).X / 2, font.MeasureString(newText).Y / 2);
        }

        public virtual void Update()
        {
            // Override this
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (texture != null)
                sb.Draw(texture, screenRect, Color.White);
            sb.DrawString(font, text, textPos, Color.White, 0, textOrigin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}

