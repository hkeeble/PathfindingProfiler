/*
 * File: TextMenuItem.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines an abstract class used for menu items containing text.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    /// <summary>
    /// Represents an object with text on a background texture, derive for more specific functionality
    /// </summary>
    abstract class TextMenuItem
    {
        protected Texture2D texture;
        protected string text;
        protected SpriteFont font;
        protected Rectangle screenRect;
        protected Vector2 textPos, textOrigin;

        /// <summary>
        /// Create a new text menu item.
        /// </summary>
        /// <param name="position">The position of the menu item.</param>
        /// <param name="dimensions">The dimensions of the menu item.</param>
        /// <param name="texture">The texture in the background of the menu item.</param>
        /// <param name="text">The text of the menu item.</param>
        /// <param name="font">The font to use for the text in this menu item.</param>
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

        /// <summary>
        /// Calculates a new textPos and textOrigin using the given string.
        /// </summary>
        /// <param name="newText">The new text being displayed.</param>
        protected void CalculateTextPosition(string newText)
        {
            textPos = new Vector2(screenRect.X + (screenRect.Width / 2), screenRect.Y + (screenRect.Height / 2));
            textOrigin = new Vector2(font.MeasureString(newText).X / 2, font.MeasureString(newText).Y / 2);
        }

        public virtual void Update()
        {
            // Override this
        }

        /// <summary>
        /// Render this text menu item.
        /// </summary>
        public virtual void Draw(SpriteBatch sb)
        {
            if (texture != null)
                sb.Draw(texture, screenRect, Color.White);
            sb.DrawString(font, text, textPos, Color.White, 0, textOrigin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}

