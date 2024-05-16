using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.Input;
using Sopra.UI;

namespace Sopra.Screens.GameScreens
{
    /// <summary>
    /// A main Menu class, the game starts with menu window.
    /// </summary>
    /// <author>Anushe Glushik</author>
    public class MainMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        public ContentManager mContent;
        private Button mStartBtn;
        private Button mChangeGameBtn;
        private Button mOptionsBtn;
        private Button mStatistikBtn;
        private Button mAchievmentsBtn;
        private Button mCreditsBtn;
        private Button mBeendenBtn;
        private Texture2D mBackground;
        private Label mMenuLabel;

        /// <summary>
        /// Menu class constructor.
        /// </summary>
        /// <param name="content"></param>
        public MainMenu(
            MenuActions menuActions,
            ContentManager content)
        {
            mMenuActions = menuActions;
            mContent = content;
            // load textures for menu screen
        }

        public void Initialize()
        {
            mBackground = mContent.Load<Texture2D>("Menu/AI1920");
            var startBtnTe = mContent.Load<Texture2D>("Menu/button_spiel-starten");
            var gameChangeBtnTe = mContent.Load<Texture2D>("Menu/button_spielstand-wechseln");
            var optionsBtnTe = mContent.Load<Texture2D>("Menu/button_optionen");
            var statistikBtnTe = mContent.Load<Texture2D>("Menu/button_statistiken");
            var achievmentsBtnTe = mContent.Load<Texture2D>("Menu/button_achievements");
            var creditsBtnTe = mContent.Load<Texture2D>("Menu/button_credits");
            var beendenBtnTe = mContent.Load<Texture2D>("Menu/button_beenden");

            var font = mContent.Load<SpriteFont>("font1");

            // Create buttons
            mStartBtn = new Button(new Rectangle(500, 90, 240, 60), startBtnTe, font);
            mChangeGameBtn = new Button(new Rectangle(500, 160, 240, 60), gameChangeBtnTe, font);
            mOptionsBtn = new Button(new Rectangle(500, 230, 240, 60), optionsBtnTe, font);
            mStatistikBtn = new Button(new Rectangle(500, 300, 240, 60), statistikBtnTe, font);
            mAchievmentsBtn = new Button(new Rectangle(500, 370, 240, 60), achievmentsBtnTe, font);
            mCreditsBtn = new Button(new Rectangle(500, 440, 240, 60), creditsBtnTe, font);
            mBeendenBtn = new Button(new Rectangle(500, 510, 240, 60), beendenBtnTe, font);
            // not sure if need to draw a text ove a button
            //_changeGameBtn.Text = "";

            // create label "Optionen"
            mMenuLabel = new Label(new Rectangle(500, 30, 240, 60), font, "Menu");

            // redirect button clicked actions
            mStartBtn.mOnClick = () => mMenuActions.StartNewGame();
            mOptionsBtn.mOnClick = () => mMenuActions.OpenOptionScreen();
            mBeendenBtn.mOnClick = () => mMenuActions.Exit();
        }

        /// <summary>
        /// Draw every element in the menu.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, 800, 600), Color.White);
            mStartBtn.Draw(spriteBatch, gameTime);
            mChangeGameBtn.Draw(spriteBatch, gameTime);
            mOptionsBtn.Draw(spriteBatch, gameTime);
            mStatistikBtn.Draw(spriteBatch, gameTime);
            mAchievmentsBtn.Draw(spriteBatch, gameTime);
            mCreditsBtn.Draw(spriteBatch, gameTime);
            mBeendenBtn.Draw(spriteBatch, gameTime);
            mMenuLabel.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override bool HandleKeyEvent(KeyEvent keyEvent)
        {
            return base.HandleKeyEvent(keyEvent);
        }

        public override bool HandleMouseEvent(MouseEvent mouseEvent)
        {
            return base.HandleMouseEvent(mouseEvent);
        }
    }
}