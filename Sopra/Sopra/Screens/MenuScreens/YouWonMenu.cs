using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.UI;

namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// The screen, that opened when a user wins the game.
    /// </summary>
    /// <inheritdoc cref="AbstractScreen"/>
    /// <author>Anushe Glushik</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class YouWonMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private readonly SpriteBatch mSpriteBatch;

        private Label mPauseLabel;
        private Texture2D mBackground;
        private Button mStatsAchievBtn;
        private Button mHauptMenuBtn;
        private SpriteFont mFont;
        private Texture2D mKevin;

        /// <summary>
        /// Game over screen class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        public YouWonMenu(MenuActions menuActions, ContentManager content, SpriteBatch batch)
            : base(true, false)
        {
            mMenuActions = menuActions;
            mContent = content;
            mSpriteBatch = batch;

        }

        public void Initialize()
        {
            LoadBackground();
            LoadLabel();
            LoadButtons();
            LoadKevin();
            Actions();
        }

        private void LoadBackground()
        {
            mBackground = mContent.Load<Texture2D>("graphics/stats/background");
        }

        private void LoadLabel()
        {
            mFont = mContent.Load<SpriteFont>("fonts/arial_72");
            mPauseLabel = new Label(new Rectangle(280, 120, 240, 60), mFont, "Du Hast Gewonnen!");
        }

        private void LoadButtons()
        {
            var statsAchievBtnTe = mContent.Load<Texture2D>("graphics/menu/button_stats-achievements");
            var hauptMenuBtnTe = mContent.Load<Texture2D>("graphics/pause/button_hauptmenue");
            // Texture2D optionenBtnTe = mContent.Load<Texture2D>("graphics/pause/button_optionen");

            mStatsAchievBtn = new Button(new Rectangle(280, 250, 240, 60), statsAchievBtnTe, mFont);
            mHauptMenuBtn = new Button(new Rectangle(280, 320, 240, 60), hauptMenuBtnTe, mFont);
            // mOptionenBtn = new Button(new Rectangle(280, 390, 240, 60), optionenBtnTe, mFont);
        }

        private void LoadKevin()
        {
            mKevin = mContent.Load<Texture2D>("graphics/pause/Kevin_win");
        }

        private void Actions()
        {
            mStatsAchievBtn.mOnClick = () => mMenuActions.OpenStatScreen();
            mHauptMenuBtn.mOnClick = () => mMenuActions.OpenMainScreen();
        }

        /// <summary>
        /// Draw every element in options screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            var width = mSpriteBatch.GraphicsDevice.Viewport.Width;
            var height = mSpriteBatch.GraphicsDevice.Viewport.Height;
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.Draw(mKevin, new Rectangle(50, 200, 375, 450), Color.White);
            mPauseLabel.Draw(spriteBatch);
            mStatsAchievBtn.Draw(spriteBatch);
            mHauptMenuBtn.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Updates the position of labels and buttons according to screen height and width.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float screenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            var x = (int)(screenWidth / 2) - 120;
            mPauseLabel.Update(x);
            mStatsAchievBtn.Update(x);
            mHauptMenuBtn.Update(x);
        }
    }
}
