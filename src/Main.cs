using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Pathfinder
{
    /// This is the main type for the game
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Level currentLevel;
        
        // Components
        Menu menu;
        InputHandler input;

        //screen size and frame rate
        private const int TargetFrameRate = 50;
        private const int BackBufferWidth = 600;
        private const int BackBufferHeight = 600;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = BackBufferHeight;
            graphics.PreferredBackBufferWidth = BackBufferWidth;
            Window.Title = "Pathfinding Profiler v1.0";
            Content.RootDirectory = "content";
            this.IsMouseVisible = true;

            // Add Components
            menu = new Menu(this);
            input = new InputHandler(this);
            
            this.Components.Add(menu);
            this.Components.Add(input);

            // Set frame rate
            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Load textures
            Texture2D tile1Texture = Content.Load<Texture2D>("tile1");
            Texture2D tile2Texture = Content.Load<Texture2D>("tile2");
            Texture2D aiTexture = Content.Load<Texture2D>("ai");
            Texture2D playerTexture = Content.Load<Texture2D>("target");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ////player movement: read keyboard
            //KeyboardState keyState = Keyboard.GetState();
            //Coord2 currentPos = new Coord2();
            //currentPos = player.GridPosition;

            //if(keyState.IsKeyDown(Keys.Up))
            //{
            //    currentPos.Y -= 1;
            //    player.SetNextLocation(currentPos, level);
            //}
            //else if (keyState.IsKeyDown(Keys.Down))
            //{
            //    currentPos.Y += 1;
            //    player.SetNextLocation(currentPos, level);
            //}
            //else if (keyState.IsKeyDown(Keys.Left))
            //{
            //    currentPos.X -= 1;
            //    player.SetNextLocation(currentPos, level);
            //}
            //else if (keyState.IsKeyDown(Keys.Right))
            //{
            //    currentPos.X += 1;
            //    player.SetNextLocation(currentPos, level);
            //}
            //else if (keyState.IsKeyDown(Keys.Enter))
            //    BuildNewBotPath();

            ////update bot and player
            //bot.Update(gameTime, level, player);
            //player.Update(gameTime, level);

            //// Update scent map objects
            //scentBot.Update(gameTime, level, player);
            //level.scentMap.Update(level, player);

            base.Update(gameTime);
        }

        private void BuildNewBotPath()
        {
            //level.pathfinder.Build(level, bot, player);
            //bot.currentPathIndex = level.pathfinder.Path.Count - 1;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin();
            ////draw level map
            //DrawGrid();
            ////draw bot
            //spriteBatch.Draw(aiTexture, bot.ScreenPosition, Color.White);
            //// draw scent bot
            //spriteBatch.Draw(aiTexture, bot.GridPosition, Color.White);
            ////drawe player
            //spriteBatch.Draw(playerTexture, player.ScreenPosition, Color.White);
            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
