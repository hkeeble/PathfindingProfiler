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


namespace Pathfinder
{
    public enum MouseButton
    {
        LeftButton,
        RightButton
    }

    public class InputHandler : Microsoft.Xna.Framework.GameComponent
    {
        public static KeyboardState currentKeyboardState, previousKeyboardState;
        public static MouseState currentMouseState, previousMouseState;

        public InputHandler(Game game)
            : base(game)
        {
            currentKeyboardState = new KeyboardState();
            previousKeyboardState = new KeyboardState();
            currentMouseState = new MouseState();
            previousMouseState = new MouseState();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Save previous states
            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;

            // Obtain new states
            currentKeyboardState = Keyboard.GetState(PlayerIndex.One);
            currentMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        /* Checks if the given key is held down */
        public static bool IsKeyDown(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyDown(key);
        }

        /* Checks if the given key has just been pressed */
        public static bool IsKeyPressed(Keys key)
        {
            return (previousKeyboardState.IsKeyDown(key) == false) && currentKeyboardState.IsKeyDown(key);
        }

        /* Checks if the given key is not pressed */
        public static bool IsKeyUp(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key);
        }

        /* Checks if the given key has just been released */
        public static bool IsKeyReleased(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && (currentKeyboardState.IsKeyDown(key) == false);
        }

        /* Get the current mouse position */
        public static Point MousePosition()
        {
            return new Point(currentMouseState.X, currentMouseState.Y);
        }

        /* Checks if the given mouse button is currently held down */
        public static bool IsMouseButtonDown(MouseButton button)
        {
            if (button == MouseButton.LeftButton)
                return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed;
            else if (button == MouseButton.RightButton)
                return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed;
            else
            {
                Console.WriteLine("InputHandler: Unrecognized mouse button status requested.");
                return false;
            }
        }

        /* Checks if the given mouse button is currently up */
        public static bool IsMouseButtonUp(MouseButton button)
        {
            if (button == MouseButton.LeftButton)
                return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Released;
            else if (button == MouseButton.RightButton)
                return currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Released;
            else
            {
                Console.WriteLine("InputHandler: Unrecognized mouse button status requested.");
                return false;
            }
        }

        /* Checks if the given mouse button has just been pressed */
        public static bool IsMouseButtonPressed(MouseButton button)
        {
            if (button == MouseButton.LeftButton)
                return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
            else if (button == MouseButton.RightButton)
                return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
            else
            {
                Console.WriteLine("InputHandler: Unrecognized mouse button status requested.");
                return false;
            }
        }

        /* Checks if the given mouse button has just been released */
        public static bool IsMouseButtonReleased(MouseButton button)
        {
            if (button == MouseButton.LeftButton)
                return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
            else if (button == MouseButton.RightButton)
                return currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;
            else
            {
                Console.WriteLine("InputHandler: Unrecognized mouse button status requested.");
                return false;
            }
        }

        /* Returns whether or not the mouse is currently contained within the window bounds */
        public static bool IsMouseInWindow(Rectangle windowBounds)
        {
            return windowBounds.Contains(MousePosition());
        }
    }
}