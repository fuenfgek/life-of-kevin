using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.UI;


namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// The game over screen.
    /// </summary>
    /// <inheritdoc cref="AbstractScreen"/>
    /// <author>Anushe Glushik</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GameOverMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private readonly SpriteBatch mSpriteBatch;

        private Label mPauseLabel;
        private Texture2D mBackground;
        private Button mSpielLadenBtn;
        private Button mHauptMenuBtn;
        private Button mBeendenBtn;
        private SpriteFont mFont;
        private Texture2D mKevin;

        /// <summary>
        /// Game over screen class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        public GameOverMenu(MenuActions menuActions, ContentManager content, SpriteBatch batch)
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
            mMenuActions.CheckIfGamesSaved(mSpielLadenBtn);
        }

        private void LoadBackground()
        {
            mBackground = mContent.Load<Texture2D>("graphics/stats/background");
        }

        private void LoadLabel()
        {
            mFont = mContent.Load<SpriteFont>("fonts/arial_72");
            mPauseLabel = new Label(new Rectangle(100, 250, 240, 60), mFont, "Du Bist Tot!");
        }

        private void LoadButtons()
        {
            var spielLadenBtnTe = mContent.Load<Texture2D>("graphics/menu/button_spiel-laden");
            var hauptMenuBtnTe = mContent.Load<Texture2D>("graphics/pause/button_hauptmenue");
            var beendenBtnTe = mContent.Load<Texture2D>("graphics/menu/button_spiel-beenden");

            mSpielLadenBtn = new Button(new Rectangle(100, 350, 240, 60), spielLadenBtnTe, mFont);
            mHauptMenuBtn = new Button(new Rectangle(100, 420, 240, 60), hauptMenuBtnTe, mFont);
            mBeendenBtn = new Button(new Rectangle(100, 490, 240, 60), beendenBtnTe, mFont);
        }

        private void LoadKevin()
        {
            mKevin = mContent.Load<Texture2D>("graphics/pause/Kevin_lose");
        }

        private void Actions()
        {
            mSpielLadenBtn.mOnClick = () => mMenuActions.OpenLoadingDeath();
            mHauptMenuBtn.mOnClick = () => mMenuActions.OpenMainScreen();
            mBeendenBtn.mOnClick = () => mMenuActions.Exit();
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
            mSpielLadenBtn.Draw(spriteBatch);
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
            mSpielLadenBtn.Update(x);
            mHauptMenuBtn.Update(x);
        }
    }
}
