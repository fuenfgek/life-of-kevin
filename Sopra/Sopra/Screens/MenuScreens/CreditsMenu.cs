using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.UI;

namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// The credits screen class
    /// </summary>
    /// <inheritdoc cref="AbstractScreen"/>
    /// <author>Marcel Ebbinghaus</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CreditsMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private readonly SpriteBatch mSpriteBatch;
        private SpriteFont mFont;
        private Button mZurueckBtn;
        private Texture2D mBackground;
        private Texture2D mCreditsTexture;
        private Texture2D mCredits;
        private float mScreenscaleWidth;
        private float mScreenscaleHeight;
        private float mScreenWidth;
        private float mScreenHeight;

        /// <summary>
        /// Credits class constructor
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        public CreditsMenu(MenuActions menuActions, ContentManager content, SpriteBatch batch) : base(false, false)
        {
            mContent = content;
            mMenuActions = menuActions;
            mSpriteBatch = batch;
        }

        /// <summary>
        /// Initialize the credits menu
        /// </summary>
        /// <inheritdoc cref="IInitializable"/>
        public void Initialize()
        {
            LoadFonts();
            mBackground = mContent.Load<Texture2D>("graphics/stats/background");
            mCreditsTexture = mContent.Load<Texture2D>("graphics/credits/credits");
            mCredits = mContent.Load<Texture2D>("graphics/credits/credits_screen");
            CreateButtons();
            CreateLabel();
            Actions();
        }

        private void LoadFonts()
        {
            mFont = mContent.Load<SpriteFont>("fonts/arial_24");
        }

        private void CreateButtons()
        {
            var zurueckBtnTe = mContent.Load<Texture2D>("graphics/stats/button_zurueck");

            mZurueckBtn = new Button(new Rectangle(280, 510, 240, 60), zurueckBtnTe, mFont);
        }

        private void Actions()
        {
            mZurueckBtn.mOnClick = ZuruekAction;
        }

        private void ZuruekAction()
        {
            mMenuActions.Remove(this);
        }

        private void CreateLabel()
        {

        }

        /// <summary>
        /// Draw every element in credits screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, (int)(800 * mScreenscaleWidth), (int)(600 * mScreenscaleHeight)), Color.White);
            spriteBatch.Draw(mCreditsTexture, new Rectangle((int)(mScreenWidth / 2 - 120), 0, 240, 60), Color.White);
            spriteBatch.Draw(mCredits, new Rectangle(80, 40, (int)(600 * mScreenscaleWidth),(int)(600 * mScreenscaleHeight)), Color.White);
            mZurueckBtn.Draw(spriteBatch);
            spriteBatch.End();            
        }

        /// <summary>
        /// Updates buttons and label positions according to screen width
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            mScreenscaleWidth = mSpriteBatch.GraphicsDevice.Viewport.Width / 800f;
            mScreenscaleHeight = mSpriteBatch.GraphicsDevice.Viewport.Height / 600f;

            mScreenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            mScreenHeight = mSpriteBatch.GraphicsDevice.Viewport.Height;
            mZurueckBtn.ElementRectangle =
                new Rectangle((int)(mScreenWidth / 2) - 120, (int)(mScreenHeight - (mScreenHeight / 20 + 60)), 240, 60);
        }
    }
}