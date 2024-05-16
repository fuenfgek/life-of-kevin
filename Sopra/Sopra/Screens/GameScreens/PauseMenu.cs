﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.UI;

namespace Sopra.Screens.GameScreens
{
    /// <summary>
    /// The screen that appears when Pause button or ESC are pressed.
    /// The game frozes while Pause Screen is opened.
    /// </summary>
    /// <author>Anushe Glushik</author>
    public class PauseMenu : AbstractScreen, IInitializable
    {
        private MenuActions mMenuActions;
        public ContentManager mContent;
        
        private Label mPauseLabel;
        private Texture2D mBackground;
        private Button mWeiterspielenBtn;
        private Button mHauptMenuBtn;
        private Button mOptionenBtn;
        private Button mSpeichernBtn;

        /// <summary>
        /// Options class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        public PauseMenu(MenuActions menuActions, ContentManager content)
        {
            mMenuActions = menuActions;
            mContent = content;
            // load textures for options screen
            
        }

        public void Initialize()
        {
            mBackground = mContent.Load<Texture2D>("PauseScreen/grey");
            var weiterspielenBtnTe = mContent.Load<Texture2D>("PauseScreen/button_weiterspielen");
            var hauptMenuBtnTe = mContent.Load<Texture2D>("PauseScreen/button_hauptmenue");
            var optionenBtnTe = mContent.Load<Texture2D>("PauseScreen/button_optionen");
            var speichernBtnTe = mContent.Load<Texture2D>("PauseScreen/button_spiel-speichern");

            var font = mContent.Load<SpriteFont>("PauseScreen/fontPause");

            // create label "Pause"
            mPauseLabel = new Label(new Rectangle(250, 150, 240, 60), font, "Pause");
            mWeiterspielenBtn = new Button(new Rectangle(250, 250, 240, 60), weiterspielenBtnTe, font);
            mHauptMenuBtn = new Button(new Rectangle(250, 320, 240, 60), hauptMenuBtnTe, font);
            mOptionenBtn = new Button(new Rectangle(250, 390, 240, 60), optionenBtnTe, font);
            mSpeichernBtn = new Button(new Rectangle(250, 460, 240, 60), speichernBtnTe, font);
            // when "Zurueck" button is clicked - call action
            mWeiterspielenBtn.mOnClick = () => mMenuActions.Remove(this);
            mHauptMenuBtn.mOnClick = () => mMenuActions.OpenMainScreen();
            mOptionenBtn.mOnClick = () => mMenuActions.OpenOptionScreen();
        }

        /// <summary>
        /// Draw every element in options screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, 1000, 1000), new Color(50, 50, 50, 150));
            mPauseLabel.Draw(spriteBatch, gameTime);
            mWeiterspielenBtn.Draw(spriteBatch, gameTime);
            mHauptMenuBtn.Draw(spriteBatch, gameTime);
            mOptionenBtn.Draw(spriteBatch, gameTime);
            mSpeichernBtn.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}