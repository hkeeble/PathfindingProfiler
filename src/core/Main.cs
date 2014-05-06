/*
 * File: Main.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines the main game component for this project.
 * */

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
        
        // Components
        ProfilerMenu menu;
        InputHandler input;
        LevelHandler levelHandler;

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

            // Set frame rate
            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);

            // Add Components
            menu = new ProfilerMenu(this);
            input = new InputHandler(this);
            levelHandler = new LevelHandler(this);

            GameUtils.AddUtil<GameComponentCollection>(Components);
            GameUtils.GetUtil<GameComponentCollection>().Add(menu);
            GameUtils.GetUtil<GameComponentCollection>().Add(input);
            GameUtils.GetUtil<GameComponentCollection>().Add(levelHandler);

            levelHandler.Enabled = false;
            levelHandler.Visible = false;

            SetState(typeof(ProfilerMenu));
        }

        protected override void Initialize()
        {

            GameUtils.AddUtil<GraphicsDevice>(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public static void SetState(Type state)
        {
            foreach (GameComponent component in GameUtils.GetUtil<GameComponentCollection>())
            {
                if (component is DrawableGameComponent)
                {
                    if (component.GetType() == state)
                    {
                        component.Enabled = true;
                        (component as DrawableGameComponent).Visible = true;
                    }
                    else
                    {
                        component.Enabled = false;
                        (component as DrawableGameComponent).Visible = false;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
