using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sopra.Input
{
    /// <summary>
    /// Mouse event.
    /// </summary>
    /// <inheritdoc cref="IEvent"/>
    /// <author>Michael Fleig</author>
    public sealed class MouseEvent : IEvent
    {
        /// <summary>
        /// Mouse button which issued the event.
        /// </summary>
        public MouseButton Button { get; }

        /// <summary>
        /// State of the mouse button (pressed or released).
        /// </summary>
        public ButtonState State { get; }

        /// <summary>
        /// The position at the time the button was pressed.
        /// </summary>
        public Point PostitionPressed { get; }

        /// <summary>
        /// The position were the mouse was released.
        /// Value is only set if it is a release event.
        /// </summary>
        public Point? PositionReleased { get; }

        /// <summary>
        /// Determine wheter the event was handled or not.
        /// </summary>
        public bool Handled { get; set; }

        public MouseEvent(
            MouseButton button,
            ButtonState state,
            Point postitionPressed,
            Point? positionReleased)
        {
            Button = button;
            State = state;
            PostitionPressed = postitionPressed;
            PositionReleased = positionReleased;
            Handled = false;
        }

        public override string ToString()
        {
            return $"{Button} - {State} - {PostitionPressed} - {PositionReleased} - {Handled}";
        }
    }
}