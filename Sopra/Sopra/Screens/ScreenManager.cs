using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Input;

namespace Sopra.Screens
{
    /// <summary>
    /// The ScreenManager is responsible for rendering and updating the different screens in the game,
    /// </summary>
    /// <author>Michael Fleig</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ScreenManager
    {
        private readonly Stack<IScreen> mScreens;
        private readonly InputManager mInput;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;

        public ScreenManager(InputManager input, GraphicsDeviceManager graphicsDeviceManager)
        {
            mInput = input;
            mScreens = new Stack<IScreen>();
            mGraphicsDeviceManager = graphicsDeviceManager;
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
        public void Draw(SpriteBatch spriteBatch)
        {
            var screens = new Stack<IScreen>();

            UpdateViewport();

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
                screen.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Update all game screens from high order to low order.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var screens = new Stack<IScreen>(mScreens).Reverse().ToList();
            
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

        private void HandleInputEvents(List<IScreen> screens)
        {
            HandleKeyEvents(screens);
            HandleMouseEvents(screens);
        }

        private void HandleMouseEvents(List<IScreen> screens)
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

                    if (!screen.UpdateLower)
                    {
                        break;
                    }
                }
            }
        }

        private void HandleKeyEvents(List<IScreen> screens)
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
                    if (!screen.UpdateLower)
                    {
                        break;
                    }
                }
            }
        }

        private void UpdateViewport()
        {
            var graphicsDevice = mGraphicsDeviceManager.GraphicsDevice;
            float windowX = graphicsDevice.PresentationParameters.Bounds.Width;
            float windowY = graphicsDevice.PresentationParameters.Bounds.Height;

            var resolutionX = 1152;
            var resolutionY = 648;

            var widthMult = windowX / resolutionX;
            var heightMult = windowY / resolutionY;

            var multiplier = Math.Min(widthMult, heightMult);
            var viewportX = widthMult < heightMult
                ? 0
                : (int)(windowX - resolutionX * multiplier) / 2;
            var viewportY = widthMult < heightMult
                ? (int)(windowY - resolutionY * multiplier) / 2
                : 0;

            InputManager.Get().InputOffset = new Point(viewportX, viewportY);

            graphicsDevice.Viewport = new Viewport(
                viewportX,
                viewportY,
                (int)(resolutionX * multiplier),
                (int)(resolutionY * multiplier));
        }
    }
}