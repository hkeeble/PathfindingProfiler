/*
 * File: ProfilerMenu.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines the main component used to present the user with a menu. This menu allows the user to:
 *          - Load maps.
 *          - Open a map using an algorithm to visualize the algorithm.
 *          - Generate a new random map based on the given inputs.
 *          - Configure and run a set of tests designed to profile pathfinding algorithms.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pathfinder
{
    enum MenuCommand
    {
        OpenDijkstra,
        OpenAStar,
        OpenScentMap,
        OpenMapFile,
        OpenTestConfig,
        OpenGenerateMap
    }

    public class ProfilerMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const string title           = "Pathfinding Profiler v1.0";
        const string textBoxTitle    = "Current map: ";
        const string algorithmsTitle = "Choose an algorithm: ";
        const string testTitle       = "Configure a Test for this map: ";
        const string mapGenTitle     = "Generate a new map: ";

        SpriteBatch spriteBatch;
        List<Button> buttons;

        // Text box
        TextBox textBox;

        // Unecapsulated resources
        Texture2D backgroundTexture;
        Texture2D buttonTexture;
        Texture2D textBoxTexture;

        SpriteFont titleFont;
        SpriteFont subTitleFont;
        SpriteFont buttonFont;

        // Title position
        Vector2 titlePos;

        // Other text positions
        Vector2 textBoxTitlePos;
        Vector2 algorithmsTitlePos;
        Vector2 testTitlePos;
        Vector2 genMapTitlePos;

        // Is the menu currently active?
        public static bool isActive;

        // Form active booleans - used emulate modal form system for the game window
        public static bool mapGenActive;
        public static bool configTestActive;
        public static bool testActive;

        // The test configuration window
        ConfigTest testConfigBox;

        // Generate map window
        GenerateMap mapGen;

        // Reference to game
        Game game;

        public ProfilerMenu(Game game)
            : base(game)
        {
            isActive = true;
            this.game = game;

            mapGenActive = false;
            configTestActive = false;
            testActive = false;
        }

        protected override void LoadContent()
        {
            // Create sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Content
            titleFont           = Game.Content.Load<SpriteFont>("Fonts/MenuTitleFont");
            subTitleFont        = Game.Content.Load<SpriteFont>("Fonts/MenuSubTitleFont");
            buttonFont          = Game.Content.Load<SpriteFont>("Fonts/MenuButtonFont");
            backgroundTexture   = Game.Content.Load<Texture2D> ("Images/menuBack");
            buttonTexture       = Game.Content.Load<Texture2D> ("Images/button");
            textBoxTexture      = Game.Content.Load<Texture2D> ("Images/textbox");

            Init();

            base.LoadContent();
        }

        /// <summary>
        /// Initializes the profiler menu.
        /// </summary>
        private void Init()
        {
            // Useful values for positioning
            float centerX = Game.Window.ClientBounds.Width / 2;
            float quarterX = Game.Window.ClientBounds.Width / 4;

            // Create Algorithm Buttons
            buttons = new List<Button>();
            buttons.Add(new Button(new Vector2(quarterX - 85, 235), new Vector2(150, 50), buttonTexture, "Dijkstra", buttonFont, Color.White, Color.Yellow,
                MenuCommand.OpenDijkstra));

            buttons.Add(new Button(new Vector2(quarterX + 75, 235), new Vector2(150, 50), buttonTexture, "A Star", buttonFont, Color.White, Color.Yellow,
                MenuCommand.OpenAStar));

            buttons.Add(new Button(new Vector2(quarterX + 235, 235), new Vector2(150, 50), buttonTexture, "Scent Map", buttonFont, Color.White, Color.Yellow,
                MenuCommand.OpenScentMap));

            // Create Textbox
            Vector2 textBoxPos = new Vector2(70, 120);
            Vector2 textBoxDims = new Vector2(350, 50);
            textBox = new TextBox(textBoxPos, textBoxDims, textBoxTexture, "", buttonFont, Color.Black);

            // Create load map button
            buttons.Add(new Button(new Vector2(textBoxPos.X + textBoxDims.X, textBoxPos.Y), new Vector2(100, textBoxDims.Y), buttonTexture, "Load",
                buttonFont, Color.White, Color.Yellow, MenuCommand.OpenMapFile));

            // Create test configuration button
            buttons.Add(new Button(new Vector2(centerX - ((Game.Window.ClientBounds.Width - 120) / 2), Game.Window.ClientBounds.Height - 220), new Vector2(Game.Window.ClientBounds.Width - 120, textBoxDims.Y),
                buttonTexture, "Configure Test", buttonFont, Color.White, Color.Yellow, MenuCommand.OpenTestConfig));
            
            buttons.Add(new Button(new Vector2(centerX - ((Game.Window.ClientBounds.Width - 120) / 2), Game.Window.ClientBounds.Height - 120), new Vector2(Game.Window.ClientBounds.Width - 120, textBoxDims.Y),
                buttonTexture, "Generate Map", buttonFont, Color.White, Color.Yellow, MenuCommand.OpenGenerateMap));

            // Calculate positions
            titlePos = new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2), 0);
            textBoxTitlePos = new Vector2(textBoxPos.X, textBoxPos.Y - subTitleFont.MeasureString(textBoxTitle).Y);
            algorithmsTitlePos = new Vector2(textBoxTitlePos.X, textBoxTitlePos.Y + (textBoxDims.Y * 2) + 5);
            testTitlePos = new Vector2(algorithmsTitlePos.X, algorithmsTitlePos.Y + (textBoxDims.Y * 3));
            genMapTitlePos = new Vector2(algorithmsTitlePos.X, algorithmsTitlePos.Y + (textBoxDims.Y * 5));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Update function for this component.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                foreach (Button b in buttons)
                {
                    b.Update();

                    if (b.IsPressed)
                    {
                        if (b.Command == MenuCommand.OpenDijkstra || b.Command == MenuCommand.OpenAStar || b.Command == MenuCommand.OpenScentMap)
                        {
                            if (LoadMap())
                            {
                                if (b.Command == MenuCommand.OpenDijkstra)
                                    LevelHandler.SetPathfindingAlgorithm(PathfinderAlgorithm.Dijkstra);
                                else if (b.Command == MenuCommand.OpenAStar)
                                    LevelHandler.SetPathfindingAlgorithm(PathfinderAlgorithm.AStar);
                                else if (b.Command == MenuCommand.OpenScentMap)
                                    LevelHandler.SetPathfindingAlgorithm(PathfinderAlgorithm.ScentMap);
                                else
                                {
                                    Console.WriteLine("Menu.cs: Error, attempted to set unrecognized pathfinding algorithm. Defaulting to Dijkstra.");
                                    LevelHandler.SetPathfindingAlgorithm(PathfinderAlgorithm.Dijkstra);
                                }
                                Main.SetState(typeof(LevelHandler));
                            }
                        }
                        else if (b.Command == MenuCommand.OpenMapFile)
                            OpenMap();
                        else if (b.Command == MenuCommand.OpenTestConfig)
                        {
                            if (LoadMap())
                                RunTestConfig();
                        }
                        else if (b.Command == MenuCommand.OpenGenerateMap)
                            RunGenerateMap();
                        else
                            Console.WriteLine("Unrecognized menu button command called.\n");
                    }
                }
            }
            else
            {
                 if(InputHandler.IsMouseButtonPressed(MouseButton.LeftButton) && game.IsActive)
                 {
                     if (mapGenActive)
                     {
                         if (Form.ActiveForm != mapGen)
                         {
                             mapGen.Activate();
                             SystemSounds.Exclamation.Play();
                         }
                     }
                     if (configTestActive && !testActive)
                     {
                         if (Form.ActiveForm != testConfigBox)
                         {
                             testConfigBox.Activate();
                             SystemSounds.Exclamation.Play();
                         }
                     }
                 }
            }

            base.Update(gameTime);
         }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (this.Enabled)
                Init();

            base.OnEnabledChanged(sender, args);
        }

        /// <summary>
        /// Loads a map into the level handler based on the map selected in the menu.
        /// </summary>
        private bool LoadMap()
        {
            if (textBox.Text != "")
            {
                LevelHandler.SetMap("Content/Maps/" + textBox.Text);
                LevelHandler.SetPlayerPosition(new Coord2(0, 0));
                LevelHandler.SetBotPosition(new Coord2(10, 10));
                return true;
            }
            else
            {
                ShowNoMapError();
                return false;
            }
        }

        /// <summary>
        /// Displays an error showing there is no loaded map.
        /// </summary>
        private void ShowNoMapError()
        {
            MessageBox.Show("Please open a map file first.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        /// <summary>
        /// Rendering function for this component.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            // Draw background and title
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            spriteBatch.DrawString(titleFont, title, titlePos, Color.Yellow);

            // Draw text above textbox
            spriteBatch.DrawString(subTitleFont, textBoxTitle, textBoxTitlePos, Color.Yellow);

            // Draw text above buttons
            spriteBatch.DrawString(subTitleFont, algorithmsTitle, algorithmsTitlePos, Color.Yellow);

            // Draw text above test button
            spriteBatch.DrawString(subTitleFont, testTitle, testTitlePos, Color.Yellow);

            // Draw text above generate map button
            spriteBatch.DrawString(subTitleFont, mapGenTitle, genMapTitlePos, Color.Yellow);

            // Draw Buttons
            foreach (Button b in buttons)
                b.Draw(spriteBatch);
            
            // Draw Text Box
            textBox.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Open and manages the open map dialog with an OpenFileDialog.
        /// </summary>
        public void OpenMap()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Map File";
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + "\\content\\Maps\\";
            ofd.Filter = "Map Files (*.map)|*.map";
            ofd.ShowDialog();

            if (ofd.CheckFileExists)
            {
                string fileName = ofd.SafeFileName;
                textBox.Text = fileName;
            }
            else
            {
                MessageBox.Show("File not found.");
                textBox.Text = "";
            }
        }

        /// <summary>
        /// Runs the generate map form.
        /// </summary>
        private void RunGenerateMap()
        {
            mapGen = new GenerateMap();
            mapGen.FormClosed += new FormClosedEventHandler(dialogClosed);
            mapGen.Show();
            isActive = false;
        }

        /// <summary>
        /// Runs the test config form.
        /// </summary>
        private void RunTestConfig()
        {
            testConfigBox = new ConfigTest();
            testConfigBox.FormClosed += new FormClosedEventHandler(dialogClosed);
            testConfigBox.Show();
            isActive = false;
        }

        private void dialogClosed(object sender, FormClosedEventArgs args)
        {
            isActive = true;
        }
    }
}
