using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.UI
{
    /// <summary>
    /// A text label class for UI.
    /// </summary>
    /// <author>Anushe Glushik</author>
    class Label : MenuElement
    {

        private SpriteFont mFont;
        public Color PenColor { get; set; }
        public string mText;
        public Label(Rectangle elementRectangle, SpriteFont font, string text) : base(elementRectangle)
        {
            PenColor = Color.White;
            mFont = font;
            mText = text;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Color color = Color.White;
            // buttton changes the color if the mouse is over it

            if (!string.IsNullOrEmpty(mText))
            {
                // center the button text in the button rectangle
                var x = (ElementRectangle.X + (ElementRectangle.Width / 2)) -
                        mFont.MeasureString(mText).X / 2;
                var y = (ElementRectangle.Y + (ElementRectangle.Height / 2)) -
                        mFont.MeasureString(mText).Y / 2;
                // draw label text
                spriteBatch.DrawString(mFont, mText, new Vector2(x, y), PenColor);
            }

        }

    }
}
