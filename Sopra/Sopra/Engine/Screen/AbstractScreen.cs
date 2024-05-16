using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.Input;

namespace Sopra.Engine.Screen
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

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
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