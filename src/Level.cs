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
            // Handle player input for movement
            HandlePlayerMovement();

            // Update the pathfinder
            map.pathfinder.Build(bot.GridPosition, player.GridPosition);

            // Update bot and player
            bot.Update(gameTime, map, player);
            player.Update(gameTime, map);
        }

        private void HandlePlayerMovement()
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
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            DrawGrid(sb);
            bot.Draw(sb);
            player.Draw(sb);
            sb.End();
        }

        /// <summary>
        /// Draws the grid and any neccesary pathfinding visual representation.
        /// </summary>
        /// <param name="sb">The spritebatch to use.</param>
        public void DrawGrid(SpriteBatch sb)
        {
            int sz = map.GridSize;

            // Draw grid the same way for both dijkstra and astar
            if (map.PathfindingAlgorithm == PathfinderAlgorithm.Dijkstra || map.PathfindingAlgorithm == PathfinderAlgorithm.AStar)
            {
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
                                if (map.pathfinder.GetValue(x, y) == 0)
                                    sb.Draw(map.Tile1Texture, pos, Color.White);
                                else
                                    sb.Draw(map.Tile1Texture, pos, Color.Blue);
                            }
                        }
                        else
                            sb.Draw(map.Tile2Texture, pos, Color.White);
                    }
                }
            }
            else if (map.PathfindingAlgorithm == PathfinderAlgorithm.ScentMap)
            {
                int highestValue = map.pathfinder.HighestValue();

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
                                sb.Draw(map.Tile1Texture, pos, Color.Lerp(Color.White, Color.Red, map.pathfinder.GetValue(x, y)/highestValue));
                        }
                        else
                            sb.Draw(map.Tile2Texture, pos, Color.White);
                    }
                }
            }
        }

        // Set Accessors
        public Map Map { set { map = value; } get { return map; } }
        public Player Player { set { player = value; } }
        public AiBotBase Bot { set { bot = value; } }
    }
}
