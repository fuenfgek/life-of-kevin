using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sopra.Engine.Input
{
    /// <summary>
    /// The InputManager wraps the XNA Keyboard and Mouse api's and provides convienient access to input devices.
    /// In order to use the InputManager you have to call the Update method <see cref="Update"/> at the beginning
    /// of each game loop iteration.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class InputManager
    {
        private static InputManager sInstance;

        private MouseState mMouseState;
        private MouseState mPreviousMouseState;
        private KeyboardState mKeyboardState;
        private KeyboardState mPreviousKeyboardState;

        private List<Keys> mObservingKeys = new List<Keys>
        {
            Keys.A,
            Keys.S,
            Keys.D
        };

        private List<KeyEvent> mKeyEvents = new List<KeyEvent>();

        private List<MouseButton> mObservingMouseButtons = new List<MouseButton>
        {
            MouseButton.Left,
            MouseButton.Middle,
            MouseButton.Right
        };

        private Dictionary<MouseButton, MouseEvent> mLastMouseEvents = new Dictionary<MouseButton, MouseEvent>();
        private List<MouseEvent> mMouseEvents = new List<MouseEvent>();


        public MouseState MouseState => mMouseState;

        public MouseState PreviousMouseState => mPreviousMouseState;

        public KeyboardState KeyboardState => mKeyboardState;

        public KeyboardState PreviousKeyboardState => mPreviousKeyboardState;

        public List<KeyEvent> KeyEvents => mKeyEvents;

        public List<KeyEvent> UnhandledKeyEvents => mKeyEvents.Where(keyEvent => !keyEvent.Handled).ToList();

        public List<MouseEvent> MouseEvents => mMouseEvents;

        public List<MouseEvent> UnhandledMouseEvents => mMouseEvents.Where(mouseEvent => !mouseEvent.Handled).ToList();

        private InputManager()
        {
            foreach (var button in mObservingMouseButtons)
            {
                mLastMouseEvents[button] = new MouseEvent(
                    button,
                    ButtonState.Released,
                    Point.Zero,
                    null);
            }

            mPreviousMouseState = Mouse.GetState();
            mMouseState = Mouse.GetState();
            mPreviousKeyboardState = Keyboard.GetState();
            mKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Get access to the global instance.
        /// </summary>
        /// <returns></returns>
        public static InputManager Get()
        {
            if (sInstance == null)
            {
                sInstance = new InputManager();
            }

            return sInstance;
        }

        /// <summary>
        /// Check if the given key has been pressed in the means that its state changed from up to down.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyPressed(Keys key)
        {
            return mKeyboardState.IsKeyDown(key) && mPreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Check if all the given keys have been pressed.
        /// <see cref="KeyPressed(Microsoft.Xna.Framework.Input.Keys)"/>
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyPressed(params Keys[] keys)
        {
            return keys.All(KeyPressed);
        }

        /// <summary>
        /// Check if the given key has been released in the means that its state changed from down to up.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyReleased(Keys key)
        {
            return mKeyboardState.IsKeyUp(key) && mPreviousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Check if the given key is down.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyDown(Keys key)
        {
            return mKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Check if all keys are down.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyDown(params Keys[] keys)
        {
            return keys.All(KeyDown);
        }

        /// <summary>
        /// Check if the given key is up.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyUp(Keys key)
        {
            return mKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Check if all given keys are up.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyUp(params Keys[] keys)
        {
            return keys.All(KeyUp);
        }

        /// <summary>
        /// Check if the left mouse button has been clicked
        /// in the means that its state changed from pressed to released.
        /// </summary>
        /// <returns></returns>
        public bool LeftClicked()
        {
            return mMouseState.LeftButton == ButtonState.Released &&
                   mPreviousMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Check if the right mouse button has been clicked
        /// in the means that its state changed from pressed to released.
        /// </summary>
        /// <returns></returns>
        public bool RightClicked()
        {
            return mMouseState.RightButton == ButtonState.Released &&
                   mPreviousMouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Update the mouse and keyboard states.
        /// This method should be called at the beginning of each game loop iteration.
        /// </summary>
        public void Update()
        {
            UpdateKeyboard();
            UpdateMouse();
        }

        private void UpdateMouse()
        {
            mPreviousMouseState = mMouseState;
            mMouseState = Mouse.GetState();
            MouseEvents.Clear();

            foreach (var button in mObservingMouseButtons)
            {
                var state = GetButtonState(mMouseState, button);

                if (state == mLastMouseEvents[button].State)
                {
                    continue;
                }

                MouseEvent mouseEvent;

                if (state == ButtonState.Pressed)
                {
                    mouseEvent = new MouseEvent(
                        button,
                        state,
                        mMouseState.Position,
                        null);
                }
                else
                {
                    mouseEvent = new MouseEvent(
                        button,
                        state,
                        mLastMouseEvents[button].PostitionPressed,
                        mMouseState.Position);
                }

                mLastMouseEvents[button] = mouseEvent;
                mMouseEvents.Add(mouseEvent);
            }
        }

        private ButtonState GetButtonState(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton;
                case MouseButton.Middle:
                    return state.MiddleButton;
                case MouseButton.Right:
                    return state.RightButton;
                default:
                    return state.LeftButton;
            }
        }

        private void UpdateKeyboard()
        {
            mPreviousKeyboardState = mKeyboardState;
            mKeyboardState = Keyboard.GetState();
            mKeyEvents.Clear();

            foreach (var key in mObservingKeys)
            {
                var state = mKeyboardState[key];

                if (state != mPreviousKeyboardState[key])
                {
                    mKeyEvents.Add(new KeyEvent(key, state));
                }
            }
        }
    }
}