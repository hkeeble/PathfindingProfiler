/*
 * File: Button.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a UI button.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class Button : TextMenuItem
    {
        private Color currentColor, defaultColor, hoverColor;
        private MenuCommand command;
        private bool pressed;

        const float hoverScalar = 1.2f;

        /// <summary>
        /// Create a new button.
        /// </summary>
        /// <param name="position">Screen coordinates of the button.</param>
        /// <param name="dimensions">Screen dimensions of the button.</param>
        /// <param name="texture">Texture to display on the button.</param>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="font">Font to use for the button text.</param>
        /// <param name="defaultColor">The default colour to render text as on this button.</param>
        /// <param name="hoverColor">The colour to render text as when the user moves the mouse over this button.</param>
        /// <param name="command">The command triggered by this button when clicked.</param>
        public Button(Vector2 position, Vector2 dimensions, Texture2D texture, string text, SpriteFont font, Color defaultColor, Color hoverColor, MenuCommand command)
            : base(position, dimensions, texture, text, font)
        {
            // Set parameters
            this.defaultColor   = defaultColor;
            this.hoverColor     = hoverColor;
            this.currentColor   = defaultColor;
            this.command        = command;

            pressed = false;
        }

        /// <summary>
        /// Update the button, changing the current colour wheren neccesary and checking for user button clicks.
        /// </summary>
        public override void Update()
        {
            if (screenRect.Contains(InputHandler.MousePosition()))
            {
                currentColor = hoverColor;

                if (InputHandler.IsMouseButtonPressed(MouseButton.LeftButton))
                    pressed = true;
                else
                    pressed = false;
            }
            else
                currentColor = defaultColor;
        }

        /// <summary>
        /// Render this button.
        /// </summary>
        public override void Draw(SpriteBatch sb)
        {
            if(texture != null)
                sb.Draw(texture, screenRect, Color.White);
            if(currentColor == defaultColor)
                sb.DrawString(font, text, textPos, currentColor, 0, textOrigin, 1.0f, SpriteEffects.None, 1);
            else if (currentColor == hoverColor)
                sb.DrawString(font, text, textPos, currentColor, 0, textOrigin, hoverScalar, SpriteEffects.None, 1);
        }

        /// <summary>
        /// Button's on-screen rectangle, used for collision detection
        /// </summary>
        public Rectangle CollisionRect { get { return screenRect; } }

        /// <summary>
        /// The menu command held by the button
        /// </summary>
        public MenuCommand Command { get { return command; } }

       /// <summary>
       /// Is button pressed? Once handled, is toggled off
       /// </summary>
        public bool IsPressed
        {
            get
            {
                bool current = pressed;
                if (current)
                    pressed = false;
                return current;
            }
        }
    }
}
