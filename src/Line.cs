using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class Line
    {
        struct Pixel
        {
            private Texture2D texture;
            private Vector2 position;
            private Color color;

            public Pixel(Vector2 position, Color color, GraphicsDevice graphicsDevice)
            {
                this.position = position;
                this.color = color;
                texture = new Texture2D(graphicsDevice, 1, 1);
                texture.SetData<Color>(new Color[1] { color });
            }

            public void Draw(SpriteBatch sb)
            {
                sb.Draw(texture, position, color);
            }
        }

        private List<Pixel> pixels;

        // This is a simplified implementation of Bresenham's line algorithm
        public Line(Point startPoint, Point endPoint, Color color, GraphicsDevice graphicsDevice)
        {
            int x1 = startPoint.X;
            int y1 = startPoint.Y;
            int x2 = endPoint.X;
            int y2 = endPoint.Y;

            // Calculate the length of the line
            int length = Math.Abs((x2 - x1) + (y2 - y1));
            pixels = new List<Pixel>();

            // Calculate the direction of the line
            int dx = Math.Abs(endPoint.X - startPoint.X);
            int dy = Math.Abs(endPoint.Y - startPoint.Y);

            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;

            int err = dx - dy;

            while(true)
            {
                pixels.Add(new Pixel(new Vector2(x1, y1), color, graphicsDevice));

                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err = err - dy;
                    x1 = x1 + sx;
                }

                if(e2 < dx)
                {
                    err = err + dx;
                    y1 = y1 + sy;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Pixel p in pixels)
                p.Draw(sb);
        }
    }
}
