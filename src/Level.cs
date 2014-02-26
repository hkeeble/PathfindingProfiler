using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public void LoadMap(string fileName)
        {
            map.Loadmap(fileName);
        }

        public void SetPathfindingAlgorithm(PathfinderAlgorithm algorithm)
        {
            map.SetPathfinder(algorithm);
        }

        public void SetPlayerPosition(Coord2 newPos)
        {
            player.SetPosition(newPos);
        }

        public void SetBotPosition(Coord2 newPos)
        {
            bot.SetPosition(newPos);
        }

        public void Update(GameTime gameTime)
        {
            Coord2 currentPos = new Coord2();
            currentPos = player.GridPosition;

            if (InputHandler.IsKeyDown(Keys.Up))
            {
                currentPos.Y -= 1;
                player.SetNextLocation(currentPos, map);
            }
            else if (InputHandler.IsKeyDown(Keys.Down))
            {
                currentPos.Y += 1;
                player.SetNextLocation(currentPos, map);
            }
            else if (InputHandler.IsKeyDown(Keys.Left))
            {
                currentPos.X -= 1;
                player.SetNextLocation(currentPos, map);
            }
            else if (InputHandler.IsKeyDown(Keys.Right))
            {
                currentPos.X += 1;
                player.SetNextLocation(currentPos, map);
            }
            else if (InputHandler.IsKeyDown(Keys.Enter))
                map.pathfinder.Build(map, bot, player);

            //update bot and player
            bot.Update(gameTime, map, player);
            player.Update(gameTime, map);
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
            // Draws the map grid
            int sz = map.GridSize;

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
                            sb.Draw(map.Tile1Texture, pos, Color.White);
                    }
                    else
                        sb.Draw(map.Tile2Texture, pos, Color.White);
                }
            }
        }

        // Set Accessor
        public Map Map { set { map = value; } }
        public Player Player { set { player = value; } }
        public AiBotBase Bot { set { bot = value; } }
    }
}
