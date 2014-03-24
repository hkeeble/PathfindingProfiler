/*
 * File: Line.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a class to draw a line between two given points.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    /// <summary>
    /// A flexible line class that implements Bresenham's line algorithm.
    /// </summary>
    class Line
    {
        // Values to hold
        private Texture2D texture;
        private Color color;
        private Vector2 position;
        private Vector2 origin;
        private Point startPoint;
        private Point endPoint;
        private GraphicsDevice graphicsDevice;
        private int width;

        // This is a simplified implementation of Bresenham's line algorithm
        public Line(Point startPoint, Point endPoint, Color color, int width, GraphicsDevice graphicsDevice)
        {
            SetColor(color);
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.graphicsDevice = graphicsDevice;
            this.width = width;
            GenerateLine();
        }

        /// <summary>
        /// Generates a new line texture using Bresenham's line algorithm based upon the current start and end points.
        /// </summary>
        private void GenerateLine()
        {
            // Assign variables for clarity
            int x1 = startPoint.X;
            int y1 = startPoint.Y;
            int x2 = endPoint.X;
            int y2 = endPoint.Y;

            // Find i and j components
            int i = (int)MathHelper.Distance(x1, x2);
            int j = (int)MathHelper.Distance(y1, y2);

            // Create new texture
            texture = new Texture2D(graphicsDevice, i + 1 + width, j + 1 + width);

            // The draw position is the center of the start and end point
            Point center = Center(startPoint, endPoint);
            position = new Vector2(center.X, center.Y);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            // Create the color array, apply white color to all elements
            Color[] colors = new Color[width * width];
            for (int c = 0; c < colors.Length; c++)
                colors[c] = Color.White;

            // Calculate the length of the line
            int length = i + j;

            // Calculate the direction of the line
            int dx = Math.Abs(endPoint.X - startPoint.X);
            int dy = Math.Abs(endPoint.Y - startPoint.Y);


            // Slope values
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;

            int err = dx - dy;

            // Move the line to the origin
            while (x1 != 0)
            {
                x1--;
                x2--;
            }

            while (y1 != 0)
            {
                y1--;
                y2--;
            }

            // Ensure positive values
            if (y2 < 0)
            {
                y1 += (int)MathHelper.Distance(y2, 0);
                y2 += (int)MathHelper.Distance(y2, 0);
            }

            if (x2 < 0)
            {
                x1 += (int)MathHelper.Distance(x2, 0);
                x2 += (int)MathHelper.Distance(x2, 0);
            }

            while (true)
            {
                // Determine which pixels to set
                int xpos;
                int ypos;

                if (x1 - (width / 2) < 0)
                    xpos = x1;
                else
                    xpos = x1 - (width / 2);

                if (y1 - (width / 2) < 0)
                    ypos = y1;
                else
                    ypos = y1 - (width / 2);

                // Set pixels in texture
                texture.SetData<Color>(0, new Rectangle(xpos, ypos, width, width), colors, 0, colors.Length);

                // Continue with Bresenham's
                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err = err - dy;
                    x1 = x1 + sx;
                }

                if (e2 < dx)
                {
                    err = err + dx;
                    y1 = y1 + sy;
                }
            }
        }

        /// <summary>
        /// Moves the line start and end points.
        /// </summary>
        /// <param name="newStart">The new starting position of the line.</param>
        /// <param name="newEnd">The new end position of the line.</param>
        public void Move(Point newStart, Point newEnd)
        {
            bool regen = false;

            if (startPoint != newStart)
            {
                startPoint = newStart;
                regen = true;
            }

            if (endPoint != newEnd)
            {
                endPoint = newEnd;
                regen = true;
            }

            if(regen) // Only generate a new line if points have changed, helps with optimization
                GenerateLine();
        }

        /// <summary>
        /// Swaps the two given points around.
        /// </summary>
        private void SwapPoints(ref Point a, ref Point b)
        {
            Point tmp = a;
            a.X = b.X;
            a.Y = b.Y;
            b.X = tmp.X;
            b.Y = tmp.Y;
        }

        /// <summary>
        /// Finds the center of the two given points.
        /// </summary>
        private Point Center(Point a, Point b)
        {
            return new Point((a.X+b.X)/2, (a.Y+b.Y)/2);
        }

        /// <summary>
        /// Changes the color of the line.
        /// </summary>
        /// <param name="color">The color to draw the line as.</param>
        public void SetColor(Color color)
        {
            this.color = color;
        }

        /// <summary>
        /// Draws the line. Does not being a spritebatch, this must be done before making a call to this function.
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, null, color, 0.0f, origin, new Vector2(1, 1), SpriteEffects.None, 1.0f);
        }
        
        // Get/Set Accessors
        
        /// <summary>
        /// Gets or sets the line start point. If you are moving both start and end points, <c>Move()</c> is a more optimized function.
        /// </summary>
        public Point StartPoint
        {
            get { return startPoint; }
            set
            {
                if(value != startPoint)
                {
                    startPoint = value;
                    GenerateLine();
                }
            }
        }

        /// <summary>
        /// Gets or sets the line end point. If you are moving both start and end points, <c>Move()</c> is a more optimized function.
        /// </summary>
        private Point EndPoint
        {
            get { return endPoint; }
            set
            {
                if (value != endPoint)
                {
                    endPoint = value;
                    GenerateLine();
                }
            }
        }
    }
}
