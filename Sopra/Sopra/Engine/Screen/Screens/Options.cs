using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.UI;
using Microsoft.Xna.Framework.Content;

namespace Sopra.Engine.Screen.Screens
{
    /// <summary>
    /// The Options screen class.
    /// </summary>
    /// <author>Anushe Glushik</author>
    class Options : AbstractScreen
    {
        public ContentManager mContent;
        private Texture2D mBackground;
        private Button mSteuerungBtn;
        private Button mMusikAnAusBtn;
        private Button mSoundAnAusBtn;
        private Button mTechDemoBtn;
        private Button mZurueckBtn;
        private Button mTriangle;
        private ButtonGroup mSoundLevel;
        private Label mOptionsLabel;
        public Action mOnClickZurueckBtn;

        /// <summary>
        /// Options class constructor.
        /// </summary>
        /// <param name="content"></param>
        public Options(ContentManager content)
        {
            mContent = content;
            // load textures for options screen
            mBackground = mContent.Load<Texture2D>("Menu/AI1920");
            var steuerungBtnTe = mContent.Load<Texture2D>("Options/button_steuerung");
            var musikAnAusBtnTe = mContent.Load<Texture2D>("Options/button_musik-an-aus");
            var soundAnAusBtnTe = mContent.Load<Texture2D>("Options/button_sound-an-aus");
            var techDemoBtnTe = mContent.Load<Texture2D>("Options/button_tech-demo");
            var zurueckBtnTe = mContent.Load<Texture2D>("Options/button_zurueck");
            var middleBtnTe = mContent.Load<Texture2D>("Options/button");
            var leftBtnTe = mContent.Load<Texture2D>("Options/triangle_left");
            var rightBtnTe = mContent.Load<Texture2D>("Options/triangle_right");

            var font = mContent.Load<SpriteFont>("font1");
            var font12 = mContent.Load<SpriteFont>("Options/font12");

            // Create buttons
            mSteuerungBtn = new Button(new Rectangle(500, 90, 240, 60), steuerungBtnTe, font);
            mMusikAnAusBtn = new Button(new Rectangle(500, 160, 240, 60), musikAnAusBtnTe, font);
            mSoundAnAusBtn = new Button(new Rectangle(500, 230, 240, 60), soundAnAusBtnTe, font);
            mTechDemoBtn = new Button(new Rectangle(500, 300, 240, 60), techDemoBtnTe, font);
            mZurueckBtn = new Button(new Rectangle(500, 370, 240, 60), zurueckBtnTe, font);

            mSoundLevel = new ButtonGroup(new Rectangle(500,440, 240, 60), leftBtnTe, middleBtnTe, rightBtnTe, font12);

            // not sure if need to draw a text ove a button
            //_changeGameBtn.Text = "";

            // create label "Optionen"
            mOptionsLabel = new Label(new Rectangle(500, 30, 240, 60), font, "Optionen");
            // when "Zurueck" button is clicked - call action
            mZurueckBtn.mOnClick = () => mOnClickZurueckBtn?.Invoke();
        }

        /// <summary>
        /// Draw every element in options screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, 800, 600), Color.White);
            mSteuerungBtn.Draw(spriteBatch, gameTime);
            mMusikAnAusBtn.Draw(spriteBatch, gameTime);
            mSoundAnAusBtn.Draw(spriteBatch, gameTime);
            mTechDemoBtn.Draw(spriteBatch, gameTime);
            mZurueckBtn.Draw(spriteBatch, gameTime);
            mOptionsLabel.Draw(spriteBatch, gameTime);
            mSoundLevel.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
