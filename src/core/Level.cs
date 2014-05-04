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
            SetBotType(algorithm);
        }

        private void SetBotType(PathfinderAlgorithm algorithm)
        {
            if (algorithm == PathfinderAlgorithm.Dijkstra || algorithm == PathfinderAlgorithm.AStar)
                bot = new AiBotDijkstra(bot.Texture, bot.GridPosition.X, bot.GridPosition.Y);
            else if (algorithm == PathfinderAlgorithm.ScentMap)
                bot = new AiBotScent(bot.Texture, bot.GridPosition.X, bot.GridPosition.Y);
            else
                Console.WriteLine("Could not set bot type, unrecognized algorithm.");
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
            if (map.pathfinder.GetAlgorithm() == PathfinderAlgorithm.ScentMap)
                map.pathfinder.Build(bot.GridPosition, player.GridPosition);
            else
            {
                if (InputHandler.IsKeyPressed(Keys.Enter))
                    map.pathfinder.Build(bot.GridPosition, player.GridPosition);
            }

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
            map.Draw(sb);
            bot.Draw(sb);
            player.Draw(sb);
            sb.End();
        }

        // Set Accessors
        public Map Map { set { map = value; } get { return map; } }
        public Player Player { set { player = value; } get { return player; } }
        public AiBotBase Bot { set { bot = value; } get { return bot; } }
    }
}
