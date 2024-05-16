using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sopra.Engine.Input;

namespace Sopra.Engine.UI
{
    /// <summary>
    /// The class is a group of three buttons, a main button and left (lower) and right(higher) button.
    /// </summary>
    /// <author>Anushe Glushik</author>
    class ButtonGroup
    {
        private Texture2D mLeftTe;
        private Texture2D mMiddleTe;
        private Texture2D mRightTe;
        private SpriteFont mFont;
        private Rectangle mElementRectangle;
        private Rectangle mLeftRect;
        private Rectangle mRightRect;
        private Button mLeftBtn;
        private Button mMiddleBtn;
        private Button mRightBtn;
        private MouseState mMouseState;
        InputManager mInput = InputManager.Get();
        private List<string> textOnBtn;
        private int mIndex;

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="elementRectangle"> a rectangle of the middle button,
        /// other two rectangles are calculated acoording to this button.</param>
        /// <param name="leftTe"></param>
        /// <param name="middleTe"></param>
        /// <param name="rightTe"></param>
        /// <param name="font"></param>
        public ButtonGroup(Rectangle elementRectangle,
            Texture2D leftTe,
            Texture2D middleTe,
            Texture2D rightTe,
            SpriteFont font)
        {
            mElementRectangle = elementRectangle;
            mLeftTe = leftTe;
            mMiddleTe = middleTe;
            mRightTe = rightTe;
            mFont = font;
            mLeftRect = new Rectangle(mElementRectangle.X - 50, mElementRectangle.Y, 60, 60);
            mRightRect = new Rectangle(mElementRectangle.X + 230, mElementRectangle.Y, 60, 60);
            mLeftBtn = new Button(mLeftRect, mLeftTe, mFont);
            mMiddleBtn = new Button(mElementRectangle, mMiddleTe, mFont);
            mRightBtn = new Button(mRightRect, mRightTe, mFont);

            // will probably give a this list of possible texts drawn on the button in the constructor
            // different lists for resolution and sounds 
            textOnBtn = new List<string> { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };
            //index of a currently shown item from the list
            mIndex = 5;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            mMouseState = mInput.MouseState;
            
            // if left triangle is clicked, the value on the buttons goes lower, if possible
            if (mLeftRect.Contains(mMouseState.Position) && mInput.LeftClicked())
            {
                if (mIndex > 0)
                {
                    mIndex--;
                }
            }
            // if right triangle is clicked, the value on the buttons goes higher, if possible
            if (mRightRect.Contains(mMouseState.Position) && mInput.LeftClicked())
            {
                if (mIndex < textOnBtn.Count - 1)
                {
                    mIndex++;
                }
            }

            mMiddleBtn.Text = "Sound Level " + textOnBtn[mIndex];

            mLeftBtn.Draw(spriteBatch, gameTime);
            mMiddleBtn.Draw(spriteBatch, gameTime);
            mRightBtn.Draw(spriteBatch, gameTime);

        }
    }
}
