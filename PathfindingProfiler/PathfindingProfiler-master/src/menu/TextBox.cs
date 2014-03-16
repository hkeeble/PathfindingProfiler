using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class TextBox : TextMenuItem
    {
        // Text displayed may be not all of the actual stored text
        private string displayText;
        private Color displayColor;

        public TextBox(Vector2 position, Vector2 dimensions, Texture2D texture, string text, SpriteFont font, Color textColor)
            : base(position, dimensions, texture, text, font)
        {
            Text = text; // Create initial display text
            displayColor = textColor;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (texture != null)
                sb.Draw(texture, screenRect, Color.White);
            sb.DrawString(font, displayText, textPos, displayColor, 0, textOrigin, 1.0f, SpriteEffects.None, 1.0f);
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                displayText = "";

                // Ensure the displayed text fits within the text box, if not add ellipses at the end to indicate extra text
                foreach (char c in text)
                {
                    if (font.MeasureString(displayText + c).X < screenRect.Width)
                        displayText += c;
                    else
                    {
                        int curLength = displayText.Length - 1;
                        char[] array = displayText.ToArray<char>();
                        for (int i = 0; i < 3; i++)
                            array[curLength - i] = '.';
                        displayText = new string(array);
                    }
                }

                CalculateTextPosition(displayText);
            }
        }
    }
}
