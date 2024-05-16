using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.Input;

namespace Sopra.Engine.Screen
{
    /// <summary>
    /// The ScreenManager is responsible for rendering and updating the different screens in the game,
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class ScreenManager
    {
        private readonly Stack<IScreen> mScreens;
        private readonly InputManager mInput;

        public ScreenManager(InputManager input)
        {
            mInput = input;
            mScreens = new Stack<IScreen>();
        }


        /// <summary>
        /// Add a screen to the <see cref="ScreenManager"/>.
        /// The screen will put on top of the currently managed screens.
        /// </summary>
        /// <param name="screen">screen to add</param>
        public void AddScreen(IScreen screen)
        {
            mScreens.Push(screen);
        }

        /// <summary>
        /// Remove the screen if it is currently on top.
        /// </summary>
        /// <param name="screen">Screen to remove</param>
        public void RemoveScreen(IScreen screen)
        {
            if (mScreens.Count > 0 && mScreens.Peek().Equals(screen))
            {
                mScreens.Pop();
            }
        }

        
        /// <summary>
        /// Remove all screens from the <see cref="ScreenManager"/>.
        /// </summary>
        public void Clear()
        {
            mScreens.Clear();
        }

        /// <summary>
        /// Render all game screens from high order to low order.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var screens = new Stack<IScreen>();

            foreach (var screen in mScreens)
            {
                screens.Push(screen);
                if (!screen.DrawLower)
                {
                    break;
                }
            }

            foreach (var screen in screens)
            {
                screen.Draw(spriteBatch, gameTime);
            }
        }

        /// <summary>
        /// Update all game screens from high order to low order.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var screens = new Stack<IScreen>(mScreens);

            HandleInputEvents(screens);

            foreach (var screen in screens)
            {
                screen.Update(gameTime);
                if (!screen.UpdateLower)
                {
                    break;
                }
            }
        }

        private void HandleInputEvents(Stack<IScreen> screens)
        {
            HandleKeyEvents(screens);
            HandleMouseEvents(screens);
        }

        private void HandleMouseEvents(Stack<IScreen> screens)
        {
            foreach (var mouseEvent in mInput.UnhandledMouseEvents)
            {
                foreach (var screen in screens)
                {
                    if (screen.HandleMouseEvent(mouseEvent))
                    {
                        mouseEvent.Handled = true;
                        break;
                    }
                }
            }
        }

        private void HandleKeyEvents(Stack<IScreen> screens)
        {
            foreach (var keyEvent in mInput.UnhandledKeyEvents)
            {
                foreach (var screen in screens)
                {
                    if (screen.HandleKeyEvent(keyEvent))
                    {
                        keyEvent.Handled = true;
                        break;
                    }
                }
            }
        }
    }
}