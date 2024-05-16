using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.UI
{
    /// <summary>
    /// The class is a group of three buttons, a main button and left (lower) and right(higher) button.
    /// </summary>
    /// <author>Anushe Glushik</author>

    internal sealed class ButtonGroup
    {
        private readonly Button mLeftBtn;
        private readonly Button mMiddleBtn;
        private readonly Button mRightBtn;
        public readonly List<string> mTextOnBtn;
        private readonly String mText;

        public int Index { get; set; }
        public Action mOnClickRight;
        public Action mOnClickLeft;

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="elementRectangle"> a rectangle of the middle button,
        /// other two rectangles are calculated acoording to this button.</param>
        /// <param name="leftTe"></param>
        /// <param name="middleTe"></param>
        /// <param name="rightTe"></param>
        /// <param name="font"></param>
        /// <param name="textOnBtn"></param>
        /// <param name="text"></param>
        public ButtonGroup(Rectangle elementRectangle,
            Texture2D leftTe,
            Texture2D middleTe,
            Texture2D rightTe,
            SpriteFont font,
            List<string> textOnBtn,
            string text)
        {
            var elementRectangle1 = elementRectangle;
            var font1 = font;
            mTextOnBtn = textOnBtn;
            mText = text;
            var leftRect = new Rectangle(elementRectangle1.X - 50, elementRectangle1.Y, 60, 60);
            var rightRect = new Rectangle(elementRectangle1.X + 230, elementRectangle1.Y, 60, 60);
            mLeftBtn = new Button(leftRect, leftTe, font1);
            mMiddleBtn = new Button(elementRectangle1, middleTe, font1);
            mRightBtn = new Button(rightRect, rightTe, font1);

            //index of a currently shown item from the list
            Index = mTextOnBtn.Count / 2;
            mLeftBtn.mOnClick = () => mOnClickLeft?.Invoke();
            mRightBtn.mOnClick = () => mOnClickRight.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            mMiddleBtn.Text = mText + mTextOnBtn[Index];

            mLeftBtn.Draw(spriteBatch);
            mMiddleBtn.Draw(spriteBatch);
            mRightBtn.Draw(spriteBatch);

        }

        /*public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Index", Index);
        }

        public ButtonGroup(SerializationInfo info, StreamingContext context)
        {
            Index = (int)info.GetValue("Index", typeof(int));
        }*/

        public override string ToString()
        {
            return Index.ToString();
        }

        public void Update(int x)
        {
            mLeftBtn.Update(x - 60);
            mRightBtn.Update(x + 240);
            mMiddleBtn.Update(x);
            /*.X = (int)(screenWidth - 300 * (screenWidth + 200) / 1000f);
            mLeftRect.X = (int)(screenWidth - 300 * (screenWidth + 200) / 1000f);
            mElementRectangle.X = (int)(screenWidth - 300 * (screenWidth + 200) / 1000f);*/
        }
    }
}
