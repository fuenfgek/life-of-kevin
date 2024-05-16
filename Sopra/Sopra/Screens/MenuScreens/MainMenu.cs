using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.UI;

namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// A main Menu class, the game starts with menu window.
    /// </summary>
    /// <author>Anushe Glushik</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private Button mStartBtn;
        private Button mChangeGameBtn;
        private Button mOptionsBtn;
        private Button mStatistikBtn;
        private Button mTechDemoBtn;
        private Button mCreditsBtn;
        private Button mBeendenBtn;
        private Texture2D mBackground;
        private Label mMenuLabel;
        private readonly SpriteBatch mSpriteBatch;
        private float mScreenscaleWidth;
        private float mScreenscaleHeight;
        private SpriteFont mFont;
        private SpriteFont mFont72;
        private int mWait;
        private bool mLoad;
        // private bool mIfGamesSaved;

        /// <summary>
        /// Menu class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        public MainMenu(
            MenuActions menuActions,
            ContentManager content,
            SpriteBatch batch)
        {
            mMenuActions = menuActions;
            mContent = content;
            mSpriteBatch = batch;

        }

        /// <summary>
        /// The initialisation class.
        /// </summary>
        public void Initialize()
        {
            LoadFonts();
            // load texture for menu background
            mBackground = mContent.Load<Texture2D>("graphics/menu/background");
            CreateButtons();
            // create label "Menu"
            mMenuLabel = new Label(new Rectangle(500, 30, 240, 60), mFont, "Menu");
            // redirect button clicked actions
            Actions();
            mMenuActions.CheckIfGamesSaved(mChangeGameBtn);
        }

        private void LoadFonts()
        {
            // Load fonts
            mFont = mContent.Load<SpriteFont>("fonts/arial_24");
            mFont72 = mContent.Load<SpriteFont>("fonts/arial_72");
        }

        private void CreateButtons()
        {
            // Load buttons textures
            var startBtnTe = mContent.Load<Texture2D>("graphics/menu/button_neues-spiel-starten");
            var gameChangeBtnTe = mContent.Load<Texture2D>("graphics/menu/button_spiel-laden");
            var optionsBtnTe = mContent.Load<Texture2D>("graphics/menu/button_optionen");
            var statistikBtnTe = mContent.Load<Texture2D>("graphics/menu/button_stats-achievements");
            var techDemoBtnTe = mContent.Load<Texture2D>("graphics/options/button_tech-demo");
            var creditsBtnTe = mContent.Load<Texture2D>("graphics/menu/button_credits");
            var beendenBtnTe = mContent.Load<Texture2D>("graphics/menu/button_spiel-beenden");

            // Create buttons
            mStartBtn = new Button(new Rectangle(500, 90, 240, 60), startBtnTe, mFont);
            mChangeGameBtn = new Button(new Rectangle(500, 160, 240, 60), gameChangeBtnTe, mFont);
            mOptionsBtn = new Button(new Rectangle(500, 230, 240, 60), optionsBtnTe, mFont);
            mStatistikBtn = new Button(new Rectangle(500, 300, 240, 60), statistikBtnTe, mFont);
            mTechDemoBtn = new Button(new Rectangle(500, 370, 240, 60), techDemoBtnTe, mFont);
            mBeendenBtn = new Button(new Rectangle(500, 440, 240, 60), beendenBtnTe, mFont);
            mCreditsBtn = new Button(new Rectangle(500, 510, 240, 60), creditsBtnTe, mFont);
        }

        /// <summary>
        /// Redirect button clicked actions.
        /// </summary>
        private void Actions()
        {
            mStartBtn.mOnClick = StartNewGame;
            mOptionsBtn.mOnClick = () => mMenuActions.OpenOptionScreen();
            mBeendenBtn.mOnClick = () => mMenuActions.Exit();
            mChangeGameBtn.mOnClick = () => mMenuActions.OpenLoading();
            mStatistikBtn.mOnClick = () => mMenuActions.OpenStatScreen();
            mCreditsBtn.mOnClick = () => mMenuActions.OpenCreditsScreen();
            mTechDemoBtn.mOnClick = () => mMenuActions.StartTechDemo();
        }

        private void StartNewGame()
        {
            mLoad = true;
            if (mWait == 1)
            {
                mMenuActions.StartNewGame();
            }
        }

        /// <summary>
        /// Draw every element in the menu.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, (int)(800 * mScreenscaleWidth), (int)(600 * mScreenscaleHeight)), Color.White);
            mStartBtn.Draw(spriteBatch);
            mChangeGameBtn.Draw(spriteBatch);
            mOptionsBtn.Draw(spriteBatch);
            mStatistikBtn.Draw(spriteBatch);
            mTechDemoBtn.Draw(spriteBatch);
            mCreditsBtn.Draw(spriteBatch);
            mBeendenBtn.Draw(spriteBatch);
            mMenuLabel.Draw(spriteBatch);
            if (mLoad)
            {
                var gameLoad = new Label(new Rectangle((int)(400 * mScreenscaleWidth) - 120, (int)(300 * mScreenscaleHeight) - 15, 240, 60), mFont72, "Loading...");
                gameLoad.Draw(spriteBatch);
                mWait++;
                StartNewGame();
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Updates the menu screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Get the screen position.
            mScreenscaleWidth = mSpriteBatch.GraphicsDevice.Viewport.Width / 800f;
            mScreenscaleHeight = mSpriteBatch.GraphicsDevice.Viewport.Height / 600f;

            // Get the screen position.
            float screenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            var x = (int) (screenWidth - 300 * (screenWidth + 200) / 1000f);
            mStartBtn.Update(x);
            mChangeGameBtn.Update(x);
            mOptionsBtn.Update(x);
            mStatistikBtn.Update(x);
            mTechDemoBtn.Update(x);
            mCreditsBtn.Update(x);
            mBeendenBtn.Update(x);
            mMenuLabel.Update(x);
        }
    }
}