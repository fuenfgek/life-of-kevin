using Microsoft.Xna.Framework.Input;

namespace Sopra.Engine.Input
{
    /// <summary>
    /// Key event.
    /// </summary>
    /// <author>Michael Fleig</author>
    public class KeyEvent : IEvent
    {
        /// <summary>
        /// Key which issued the event.
        /// </summary>
        public Keys Key { get; }
        
        /// <summary>
        /// State of the key (up - down)
        /// </summary>
        public KeyState State { get; }
        
        /// <summary>
        /// Determine wheter the event was handled or not.
        /// </summary>
        public bool Handled { get; set; }

        public KeyEvent(Keys key, KeyState state)
        {
            Key = key;
            State = state;
            Handled = false;
        }

        public override string ToString()
        {
            return $"{Key} - {State} - {Handled}";
        }
    }
}