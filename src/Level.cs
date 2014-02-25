using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class Level
    {
        private Map map;
        private AiBotBase bot;
        private Player player;

        public Level(Map map, Player plr, AiBotBase bot)
        {
            this.map = map;
            this.player = plr;
            this.bot = bot;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            DrawGrid(sb);
            bot.Draw(sb);
            player.Draw(sb);
            sb.End();
        }

        public void DrawGrid(SpriteBatch sb)
        {
            //draws the map grid
            int sz = map.GridSize;

            // Get current highest value in scent map
            int hVal = map.scentMap.HighestValue();

            for (int x = 0; x < sz; x++)
            {
                for (int y = 0; y < sz; y++)
                {
                    Coord2 pos = new Coord2((x * 15), (y * 15));

                    if (map.tiles[x, y] == 0)
                    {
                        if (map.pathfinder.IsInPath(x, y) == true)
                            sb.Draw(map.Tile1Texture, pos, Color.Red);
                        else
                        {
                            // Visualize scent map
                            int cVal = map.scentMap.Buffer2.data[x, y];
                            float weight = (float)cVal / (float)hVal;
                            sb.Draw(map.Tile1Texture, pos, Color.Lerp(Color.White, Color.Red, weight));
                        }
                    }
                    else
                        sb.Draw(map.Tile2Texture, pos, Color.White);
                }
            }
        }
    }
}
