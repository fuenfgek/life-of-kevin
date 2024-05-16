using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.UI
{
    /// <summary>
    /// A text label class for UI.
    /// </summary>
    /// <author>Anushe Glushik</author>
    public sealed class Label : MenuElement
    {
        private readonly SpriteFont mFont;
        private Color PenColor { get; }
        private readonly string mText;
        private Rectangle mElementRectangle;

        public Label(Rectangle elementRectangle, SpriteFont font, string text) : base(elementRectangle)
        {
            PenColor = Color.White;
            mFont = font;
            mText = text;
            mElementRectangle = elementRectangle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Color color = Color.White;
            // buttton changes the color if the mouse is over it

            if (string.IsNullOrEmpty(mText))
            {
                return;
            }

            // center the button text in the button rectangle
            var x = mElementRectangle.X + mElementRectangle.Width / 2 -
                    mFont.MeasureString(mText).X / 2;
            var y = mElementRectangle.Y + mElementRectangle.Height / 2 -
                    mFont.MeasureString(mText).Y / 2;
            // draw label text
            spriteBatch.DrawString(mFont, mText, new Vector2(x, y), PenColor);

        }
        public void Update(int x)
        {
            mElementRectangle.X = x;
        }
    }
}
