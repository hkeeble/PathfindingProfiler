/*
 * File: TextBox.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a text box item.
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
    /// Represents a box used to display the given text.
    /// </summary>
    class TextBox : TextMenuItem
    {
        // Text displayed may be not all of the actual stored text
        private string displayText;
        private Color displayColor;
        
        /// <summary>
        /// Create a new text box.
        /// </summary>
        /// <param name="position">The on screen coordinates of the text box.</param>
        /// <param name="dimensions">The dimensions of the text box.</param>
        /// <param name="texture">The texture used for the background of the text box.</param>
        /// <param name="text">The text to display in the text box.</param>
        /// <param name="font">The font to use to display the text in the text box.</param>
        /// <param name="textColor">The colour to use to render the text in this box.</param>
        public TextBox(Vector2 position, Vector2 dimensions, Texture2D texture, string text, SpriteFont font, Color textColor)
            : base(position, dimensions, texture, text, font)
        {
            Text = text; // Create initial display text
            displayColor = textColor;
        }

        /// <summary>
        /// Draw this text box to the screen.
        /// </summary>
        public override void Draw(SpriteBatch sb)
        {
            if (texture != null)
                sb.Draw(texture, screenRect, Color.White);
            sb.DrawString(font, displayText, textPos, displayColor, 0, textOrigin, 1.0f, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// Gets or sets the text to display in this text box.
        /// </summary>
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
