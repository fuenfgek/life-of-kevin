using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Input;

namespace Sopra.Screens
{
    /// <summary>
    /// An interface for all game screens.
    /// Screen manager will handle all updates and rendering of all game screens.
    /// </summary>
    /// <author>Michael Fleig</author>
    public interface IScreen
    {
        bool DrawLower { get; }

        bool UpdateLower { get; }

        /// <summary>
        /// This method draws the screen.
        /// Gets called automatically if the screen was added to the Screen Manager.
        /// Note that every screen has to call the begin and end method!
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// This method updates the screen.
        /// Gets called automatically if the screen was added to the Screen Manager.
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Handle the given key event and determine wheter the event should
        /// be passed to the next screen.
        /// </summary>
        /// <param name="keyEvent">event to handle</param>
        /// <returns>return true, if the event has been handled
        /// or false, if the event should be passed to the next screen</returns>
        bool HandleKeyEvent(KeyEvent keyEvent);

        /// <summary>
        /// Handle the given mouse event and determine wheter the event should
        /// be passed to the next screen.
        /// </summary>
        /// <param name="mouseEvent">event to handle</param>
        /// <returns>return true, if the event has been handled
        /// or false, if the event should be passed to the next screen</returns>
        bool HandleMouseEvent(MouseEvent mouseEvent);
    }
}