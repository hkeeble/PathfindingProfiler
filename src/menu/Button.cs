using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class Button
    {
        private Texture2D texture;
        private string text;
        private SpriteFont font;
        private Rectangle screenRect;
        private Vector2 textPos, textOrigin;
        private Color currentColor, defaultColor, hoverColor;
        private MenuCommand command;
        private bool pressed;

        const float hoverScalar = 1.2f;

        public Button(Vector2 position, Vector2 dimensions, Texture2D texture, string text, SpriteFont font, Color defaultColor, Color hoverColor, MenuCommand command)
        {
            this.screenRect = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);
            this.texture = texture;
            this.text = text;
            this.font = font;
            this.defaultColor = defaultColor;
            this.hoverColor = hoverColor;
            this.currentColor = defaultColor;
            this.command = command;

            pressed = false;

            // Calculate text position and origin
            textPos = new Vector2 (screenRect.X + (screenRect.Width / 2), screenRect.Y + (screenRect.Height / 2));
            textOrigin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
        }

        public void Update()
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

        public void Draw(SpriteBatch sb)
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
