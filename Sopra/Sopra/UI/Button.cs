using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sopra.Audio;
using Sopra.Input;

namespace Sopra.UI
{
    /// <summary>
    /// A button class for UI.
    /// </summary>
    /// <author>Anushe Glushik</author>
    public sealed class Button : MenuElement
    {

        private readonly Texture2D mTexture;
        private readonly SpriteFont mFont;
        private bool mIsHovering;
        private MouseState mMouseState;
        readonly SoundManager mSound = SoundManager.Instance;
        public string Text { private get; set; }
        private Color PenColor { get; }
        readonly InputManager mInput = InputManager.Get();
        public Action mOnClick;
        public Color Color { private get; set; }
        private bool mDisabled;


        public Button(Rectangle elementRectangle, Texture2D buttonTexture, SpriteFont font) : base(elementRectangle)
        {
            PenColor = Color.White;
            Color = Color.White;
            mTexture = buttonTexture;
            mFont = font;
            Text = "";
            mDisabled = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            // buttton changes the color if the mouse is over it
            spriteBatch.Draw(
                mTexture,
                ElementRectangle,
                mIsHovering
                    ? Color.Gray
                    : Color);

            if (!string.IsNullOrEmpty(Text))
            {
                // center the button text in the button rectangle
                var x = ElementRectangle.X + ElementRectangle.Width / 2 -
                        mFont.MeasureString(Text).X / 2;
                var y = ElementRectangle.Y + ElementRectangle.Height / 2 -
                        mFont.MeasureString(Text).Y / 2;
                // draw button text
                spriteBatch.DrawString(mFont, Text, new Vector2(x, y), PenColor);
            }

            mMouseState = mInput.MouseState;
            mIsHovering = ElementRectangle.Contains(mMouseState.Position);

            if (ElementRectangle.Contains(mMouseState.Position) && mInput.LeftClicked())
            {
                if (!mDisabled)
                {
                    mOnClick?.Invoke();
                    mSound.PlaySound("button-1");
                }
            }
        }
        public void Update(int x)
        {
            if (x != ElementRectangle.X)
            {
                ElementRectangle = new Rectangle(x,
                ElementRectangle.Y,
                ElementRectangle.Width,
                ElementRectangle.Height);

            }          
        }

        public void Disable()
        {
            mDisabled = true;
            Color = Color.Gray;
        }

        public void Enable()
        {
            mDisabled = false;
            Color = Color.White;
        }
    }
}
