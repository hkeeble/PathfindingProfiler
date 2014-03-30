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
using System.Windows.Forms;

namespace Pathfinder
{
    /// <summary>
    /// The level handler component handles the current level. 
    /// </summary>
    class LevelHandler : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Spritebatch
        SpriteBatch spriteBatch;

        // The currently loaded level.
        private static Level level;

        // Tile Textures
        private Texture2D tileTexture1;
        private Texture2D tileTexture2;
        private Texture2D playerTexture;
        private Texture2D botTexture;

        // Strings to display
        SpriteFont mainFont;
        const float hudScale = 0.8f;
        bool showInstructions = false;
        private const string Instructions = "Right Click - Set Bot Position\nLeft Click - Set Target Position\nEnter - Find Path\nEsc - Return to Menu\nArrow Keys - Move Target Manually\nTab - Hide/Show Instructions";
        private const string HideShow = "Tab - Hide/Show Instructions";

        // Current mouse coordinate position
        Point mousePos;

        public static void SetMap(string mapFile)
        {
            level.LoadMap(mapFile);
        }

        public static void SetPathfindingAlgorithm(PathfinderAlgorithm algorithm)
        {
            level.SetPathfindingAlgorithm(algorithm);
        }

        public static void SetPlayerPosition(Coord2 pos)
        {
            level.SetPlayerPosition(pos);
        }

        public static void SetBotPosition(Coord2 pos)
        {
            level.SetBotPosition(pos);
        }

        public LevelHandler(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            tileTexture1    = Game.Content.Load<Texture2D>("Images/tile1");
            tileTexture2    = Game.Content.Load<Texture2D>("Images/tile2");
            playerTexture   = Game.Content.Load<Texture2D>("Images/target");
            botTexture      = Game.Content.Load<Texture2D>("Images/ai");

            // Load Font
            mainFont = Game.Content.Load<SpriteFont>("Fonts/bmpFont");

            level = new Level(new Map(tileTexture1, tileTexture2),
                              new Player(playerTexture, 0, 0),
                              new AiBotBlank(botTexture, 0, 0));

            // Initialize mouse position
            mousePos = new Point(0, 0);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw Level
            level.Draw(spriteBatch);

            // Draw the HUD
            DrawHUD();

            base.Draw(gameTime);
        }

        private void DrawHUD()
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(mainFont, "Algorithm: " + level.Map.pathfinder.GetName(), Vector2.Zero, Color.Violet);

            if (showInstructions)
                spriteBatch.DrawString(mainFont, Instructions, new Vector2(0, Game.Window.ClientBounds.Height - (mainFont.MeasureString(Instructions).Y * hudScale)),
                    Color.Violet, 0.0f, Vector2.Zero, hudScale, SpriteEffects.None, 1.0f);
            else
                spriteBatch.DrawString(mainFont, HideShow, new Vector2(0, Game.Window.ClientBounds.Height - (mainFont.MeasureString(HideShow).Y * hudScale)),
                    Color.Violet, 0.0f, Vector2.Zero, hudScale, SpriteEffects.None, 1.0f);

            // Draw mouse position
            string mousePosString = (mousePos.X < level.Map.GridSize ? Convert.ToString(mousePos.X) : "-") + "," +
                    (mousePos.Y < level.Map.GridSize ? Convert.ToString(mousePos.Y) : "-");
            
            spriteBatch.DrawString(mainFont, mousePosString, new Vector2(Game.Window.ClientBounds.Width - mainFont.MeasureString(mousePosString).X - 5,
                Game.Window.ClientBounds.Height - mainFont.MeasureString(mousePosString).Y), Color.Violet);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            level.Update(gameTime);

            if (Game.IsActive)
            {
                // Check if user wishes to return to menu
                if (InputHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    if (MessageBox.Show("Return to the menu?", "Return to Menu", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Main.SetState(typeof(Menu));
                        SetBotPosition(new Coord2(0, 0));
                        SetPlayerPosition(new Coord2(1, 0));
                    }
                }

                // Check for mouse clicks
                if (InputHandler.IsMouseInWindow(GraphicsDevice.Viewport.Bounds))
                {
                    // Check for mouse clicks
                    if (InputHandler.IsMouseButtonPressed(MouseButton.LeftButton) || InputHandler.IsMouseButtonPressed(MouseButton.RightButton))
                    {
                        ClearAll();

                        Coord2 mp = new Coord2(InputHandler.MousePosition().X / level.Map.TileSize, InputHandler.MousePosition().Y / level.Map.TileSize);

                        if (level.Map.ValidPosition(mp))
                        {
                            if (InputHandler.IsMouseButtonPressed(MouseButton.LeftButton))
                            {
                                if (level.Bot.GridPosition != mp)
                                    level.SetPlayerPosition(mp);
                            }
                            else
                            {
                                if (level.Player.GridPosition != mp)
                                    level.SetBotPosition(mp);
                            }
                        }
                    }

                    // Update mouse position
                    mousePos = new Point(InputHandler.MousePosition().X / level.Map.TileSize, InputHandler.MousePosition().Y / level.Map.TileSize);
                }

                // Check for hide/show instructions
                if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Tab))
                    showInstructions = !showInstructions;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Runs a test and returns the result.
        /// </summary>
        /// <param name="algorithm">The pathfinding algorithm to use.</param>
        /// <param name="startPos">The starting position to use for the test.</param>
        /// <param name="pathLength">The length in manhattan distance that the target position should be from the starting position.</param>
        /// <param name="rand">A seeded random number generator to used to calculate a target position.</param>
        /// <returns></returns>
        public static TestResult RunTest(PathfinderAlgorithm algorithm, Coord2 startPos, Coord2 targetPos)
        {
            // Set the pathfinding algorithm
            Level.SetPathfindingAlgorithm(algorithm);

            TimeSpan timeTaken;

            do
            {
                // Get Start Time
                DateTime startTime = DateTime.Now;

                // Find Path
                Level.Map.pathfinder.Build(startPos, targetPos);

                // Calculate time taken
                DateTime finishTime = DateTime.Now;
                timeTaken = finishTime - startTime;
            } while (Level.Map.pathfinder.GetPath().Count() == 0); // If no path was found, try again

            // Return results
            TestResult result = new TestResult(timeTaken.Ticks, Level.Map.pathfinder.GetPath().Count);
            if(result.PathLength == 0)
                result.Failed = true;

            return result;
        }

        /// <summary>
        /// Clears all previous data from the pathfinder and map colors.
        /// </summary>
        private void ClearAll()
        {
            level.Map.ClearColor();
            level.Map.pathfinder.Clear();
        }

        public static Level Level { get { return level; } }
        public static int MapGridSize { get { return level.Map.GridSize; } }
    }
}
