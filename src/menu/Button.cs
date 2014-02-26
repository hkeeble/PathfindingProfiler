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

        public override void Update()
        {
            if (screenRect.Contains(InputHandler.MousePosition()))
            {
                currentColor = hoverColor;

                if (InputHandler.IsMouseButtonPressed(MouseButton.LeftButton))
                    pressed = true;
            }
            else
                currentColor = defaultColor;
        }

        public override void Draw(SpriteBatch sb)
        {
            if(texture != null)
                sb.Draw(texture, screenRect, Color.White);
            if(currentColor == defaultColor)
                sb.DrawString(font, text, textPos, currentColor, 0, textOrigin, 1.0f, SpriteEffects.None, 1);
            else if (currentColor == hoverColor)
                sb.DrawString(font, text, textPos, currentColor, 0, textOrigin, hoverScalar, SpriteEffects.None, 1);
        }

        // Button's on-screen rectangle, used for collision detection
        public Rectangle CollisionRect { get { return screenRect; } }

        // The menu command held by the button
        public MenuCommand Command { get { return command; } }

        // Is button pressed? Once handled, is toggled off
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
