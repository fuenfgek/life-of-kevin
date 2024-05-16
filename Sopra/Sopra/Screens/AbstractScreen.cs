using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Input;

namespace Sopra.Screens
{
    /// <summary>
    /// Base class for screens
    /// </summary>
    /// <author>Michael Fleig</author>
    public abstract class AbstractScreen : IScreen
    {
        public bool DrawLower { get; }
        public bool UpdateLower { get; }

        protected AbstractScreen(bool drawLower = true, bool updateLower = true)
        {
            DrawLower = drawLower;
            UpdateLower = updateLower;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual bool HandleKeyEvent(KeyEvent keyEvent)
        {
            return false;
        }

        public virtual bool HandleMouseEvent(MouseEvent mouseEvent)
        {
            return false;
        }
    }
}