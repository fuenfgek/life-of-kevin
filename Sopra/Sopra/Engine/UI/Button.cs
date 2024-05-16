using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sopra.Engine.Input;

namespace Sopra.Engine.UI
{
    /// <summary>
    /// A button class for UI.
    /// </summary>
    /// <author>Anushe Glushik</author>
    class Button : MenuElement
    {

        private Texture2D mTexture;
        private SpriteFont mFont;
        private bool mIsHovering;
        private MouseState mMouseState;
        public string Text { get; set; }
        public Color PenColor { get; set; }
        InputManager mInput = InputManager.Get();
        public Action mOnClick;

        public Button(Rectangle elementRectangle, Texture2D buttonTexture, SpriteFont font) : base(elementRectangle)
        {
            PenColor = Color.White;
            mTexture = buttonTexture;
            mFont = font;
            Text = "";
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Color color = Color.White;
            // buttton changes the color if the mouse is over it
            if (mIsHovering)
            {
                color = Color.Gray;
            }

            spriteBatch.Draw(mTexture, ElementRectangle, color);

            if (!string.IsNullOrEmpty(Text))
            {
                // center the button text in the button rectangle
                var x = (ElementRectangle.X + (ElementRectangle.Width / 2)) -
                        mFont.MeasureString(Text).X / 2;
                var y = (ElementRectangle.Y + (ElementRectangle.Height / 2)) -
                        mFont.MeasureString(Text).Y / 2;
                // draw button text
                spriteBatch.DrawString(mFont, Text, new Vector2(x, y), PenColor);
            }

            mMouseState = mInput.MouseState;
            mIsHovering = ElementRectangle.Contains(mMouseState.Position);

            if (ElementRectangle.Contains(mMouseState.Position) && mInput.LeftClicked())
            {
                mOnClick?.Invoke();
            }
        }
    }
}
