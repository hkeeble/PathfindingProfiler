using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
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
        OpenScentAlgorithm,
        OpenMapFile
    }

    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const string title = "Pathfinding Profiler v1.0";

        SpriteBatch spriteBatch;
        List<Button> buttons;

        // Unecapsulated resources
        Texture2D backgroundTexture;
        SpriteFont titleFont;

        // Title position
        Vector2 titlePos;

        // The currently loaded map
        Map currentMap;

        public Menu(Game game)
            : base(game)
        {
        
        }

        protected override void LoadContent()
        {
            // Create sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Content
            titleFont               = Game.Content.Load<SpriteFont>("MenuTitleFont");
            backgroundTexture       = Game.Content.Load<Texture2D>("menuBack");
            Texture2D buttonTexture = Game.Content.Load<Texture2D>("button");
            SpriteFont buttonFont   = Game.Content.Load<SpriteFont>("MenuButtonFont");

            float centerX = Game.Window.ClientBounds.Width / 2;
            float quarterX = Game.Window.ClientBounds.Width / 4;

            // Create Buttons
            buttons = new List<Button>();
            buttons.Add(new Button(new Vector2(quarterX - 75, 200), new Vector2(150, 50), buttonTexture, "Dijkstra", buttonFont, Color.White, Color.Yellow, MenuCommand.OpenDijkstra));
            buttons.Add(new Button(new Vector2(quarterX - 75, 300), new Vector2(150, 70), buttonTexture, "A Star", buttonFont, Color.White, Color.Yellow, MenuCommand.OpenAStar));
            buttons.Add(new Button(new Vector2(quarterX - 75, 400), new Vector2(150, 70), buttonTexture, "  Scent\nAlgorithm", buttonFont, Color.White, Color.Yellow, MenuCommand.OpenScentAlgorithm));

            // Calculate positions
            titlePos = new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2), 0);

            base.LoadContent();
        }

        public override void Initialize()
        {
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Button b in buttons)
            {
                b.Update();

                if (b.IsPressed)
                {
                    if (b.Command == MenuCommand.OpenDijkstra || b.Command == MenuCommand.OpenAStar || b.Command == MenuCommand.OpenScentAlgorithm)
                    {

                    }

                    switch (b.Command)
                    {
                        case MenuCommand.OpenMapFile:
                            break;
                        case MenuCommand.OpenDijkstra:
                            if (currentMap == null) ShowNoMapError();
                            break;
                        case MenuCommand.OpenAStar:
                            if (currentMap == null) ShowNoMapError();
                            break;
                        case MenuCommand.OpenScentAlgorithm:
                            if (currentMap == null) ShowNoMapError();
                            break;
                        default:
                            Console.WriteLine("Unrecognized menu button command called.\n");
                            break;
                    }
                }
            }
            base.Update(gameTime);
        }

        private void ShowNoMapError()
        {
            MessageBox.Show("Please open a map file first.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            spriteBatch.DrawString(titleFont, title, titlePos, Color.Yellow);

            foreach (Button b in buttons)
                b.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void OpenMap()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Map File";
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + "\\content\\";
            ofd.Filter = "Map Files (*.map)|*.map";
            ofd.ShowDialog();

            if (ofd.CheckFileExists)
            {
                string fileName = ofd.SafeFileName;
                
            }
            else
            {
                MessageBox.Show("File not found.");
                currentMap = null;
            }
        }
    }
}
