using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sopra.Input
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
        
        public Point InputOffset { private get; set; }
        public Matrix CameraMatrix { private get; set; }

        private readonly List<Keys> mObservingKeys = new List<Keys>
        {
            Keys.Escape,
            Keys.A,
            Keys.S,
            Keys.D
        };

        private readonly List<KeyEvent> mKeyEvents = new List<KeyEvent>();

        private readonly List<MouseButton> mObservingMouseButtons = new List<MouseButton>
        {
            MouseButton.Left,
            MouseButton.Middle,
            MouseButton.Right
        };

        private readonly Dictionary<MouseButton, MouseEvent> mLastMouseEvents =
            new Dictionary<MouseButton, MouseEvent>();

        private readonly List<MouseEvent> mMouseEvents = new List<MouseEvent>();


        public MouseState MouseState { get; private set; }

        private MouseState PreviousMouseState { get; set; }

        private KeyboardState KeyboardState { get; set; }

        private KeyboardState PreviousKeyboardState { get; set; }

        public IEnumerable<KeyEvent> UnhandledKeyEvents => mKeyEvents.Where(keyEvent => !keyEvent.Handled);

        public IEnumerable<MouseEvent> UnhandledMouseEvents => mMouseEvents.Where(mouseEvent => !mouseEvent.Handled);

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
            
            MouseState = ShiftInput(Mouse.GetState());
            
            PreviousMouseState = MouseState;
            PreviousKeyboardState = Keyboard.GetState();
            KeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Get access to the global instance.
        /// </summary>
        /// <returns></returns>
        public static InputManager Get()
        {
            return sInstance ?? (sInstance = new InputManager());
        }

        /// <summary>
        /// Check if the left mouse button has been clicked
        /// in the means that its state changed from pressed to released.
        /// </summary>
        /// <returns></returns>
        public bool LeftClicked()
        {
            return MouseState.LeftButton == ButtonState.Released &&
                   PreviousMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Check if the right mouse button has been clicked
        /// in the means that its state changed from pressed to released.
        /// </summary>
        /// <returns></returns>
        public bool RightClicked()
        {
            return MouseState.RightButton == ButtonState.Released &&
                   PreviousMouseState.RightButton == ButtonState.Pressed;
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
            PreviousMouseState = MouseState;

            MouseState = ShiftInput(Mouse.GetState());

            mMouseEvents.Clear();

            foreach (var button in mObservingMouseButtons)
            {
                var state = GetButtonState(MouseState, button);
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
                        MouseState.Position,
                        null);
                }
                else
                {
                    mouseEvent = new MouseEvent(
                        button,
                        state,
                        mLastMouseEvents[button].PostitionPressed,
                        MouseState.Position);
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
            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            mKeyEvents.Clear();

            foreach (var key in mObservingKeys)
            {
                var state = KeyboardState[key];

                if (state != PreviousKeyboardState[key])
                {
                    mKeyEvents.Add(new KeyEvent(key, state));
                }
            }
        }

        public Vector2 MousePosInWorldCoord()
        {
            return Vector2.Transform(MouseState.Position.ToVector2(), Matrix.Invert(CameraMatrix));
        }

        private MouseState ShiftInput(MouseState tempState)
        {
            return new MouseState(
                tempState.X - InputOffset.X,
                tempState.Y - InputOffset.Y,
                tempState.ScrollWheelValue,
                tempState.LeftButton,
                tempState.MiddleButton,
                tempState.RightButton,
                tempState.XButton1,
                tempState.XButton2);
        }
    }
}