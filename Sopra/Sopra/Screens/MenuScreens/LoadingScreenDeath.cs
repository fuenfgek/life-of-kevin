using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.UI;

namespace Sopra.Screens.MenuScreens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LoadingScreenDeath : AbstractScreen, IInitializable

    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private readonly SpriteBatch mSpriteBatch;
        // private Texture2D mRotateTexture;
        // private float mAngle;
        private float mScreenWidth;
        private float mScreenHeight;
        private Label mSaveLabel;
        private SpriteFont mFont;
        private Button mLetzterSpeicherBtn;
        private Button mBeginEbeneBtn;
        private Button mZurueckBtn;
        private Texture2D mBackground;
        private SpriteFont mFont72;
        private int mWait;
        private bool mLoad;
        private int mWait2;
        private bool mLoad2;

        public LoadingScreenDeath(MenuActions menuActions, ContentManager content, SpriteBatch batch)
        {
            mContent = content;
            mSpriteBatch = batch;
            mMenuActions = menuActions;
        }

        public void Initialize()
        {

            mScreenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            mScreenHeight = mSpriteBatch.GraphicsDevice.Viewport.Height;
            // mAngle = 0f;
            mBackground = mContent.Load<Texture2D>("graphics/menu/background");
            CreateButtons();
            CreateLabel();
            Actions();
        }

        private void CreateButtons()
        {
            var letzteSpeicherBtnTe = mContent.Load<Texture2D>("graphics/menu/button_letzter-speicherpunkt");
            var beginEbeneBtnTe = mContent.Load<Texture2D>("graphics/menu/button_begin-der-ebene");
            var zurueckBtnTe = mContent.Load<Texture2D>("graphics/options/button_zurueck");
            mLetzterSpeicherBtn = new Button(new Rectangle(500, 180, 240, 60), letzteSpeicherBtnTe, mFont);
            mBeginEbeneBtn = new Button(new Rectangle(500, 260, 240, 60), beginEbeneBtnTe, mFont);
            mZurueckBtn = new Button(new Rectangle(500, 340, 240, 60), zurueckBtnTe, mFont);
        }

        private void CreateLabel()
        {
            mFont = mContent.Load<SpriteFont>("fonts/arial_24");
            mFont72 = mContent.Load<SpriteFont>("fonts/arial_72");
            mSaveLabel = new Label(new Rectangle(500, 80, 240, 60), mFont, "Spiel Laden");
        }

        private void Actions()
        {
            mLetzterSpeicherBtn.mOnClick = LoadLastSaved;
            mBeginEbeneBtn.mOnClick = LoadBeginEbene;
            mZurueckBtn.mOnClick = () => mMenuActions.GoBackFromLoadingDeath();
        }

        private void LoadLastSaved()
        {
            mLoad = true;
            if (mWait == 1)
            {
                mMenuActions.LoadLastSaved();
            }
        }
        private void LoadBeginEbene()
        {
            mLoad2 = true;
            if (mWait2 == 1)
            {
                mMenuActions.LoadBeginEbene();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, (int)mScreenWidth, (int)mScreenHeight), Color.White);
            mSaveLabel.Draw(spriteBatch);
            mLetzterSpeicherBtn.Draw(spriteBatch);
            mBeginEbeneBtn.Draw(spriteBatch);
            mZurueckBtn.Draw(spriteBatch);
            // mSaveLabel.Draw(spriteBatch, gameTime);
            /*var origin = new Vector2(mRotateTexture.Width / 2f, mRotateTexture.Height / 2f);
            
            // The sprite rotates, indicating that the process is running
            spriteBatch.Draw(mRotateTexture, new Rectangle((int)mScreenWidth / 2, (int)mScreenHeight / 2, 200, 200), null, Color.White, mAngle, origin, SpriteEffects.None, 0f);*/

            if (mLoad)
            {
                var gameLoad = new Label(new Rectangle((int)mScreenWidth / 2 - 120, (int)mScreenHeight / 2 - 15, 240, 60), mFont72, "Loading...");
                gameLoad.Draw(spriteBatch);
                mWait++;
                LoadLastSaved();
            }

            if (mLoad2)
            {
                var gameLoad = new Label(new Rectangle((int)mScreenWidth / 2 - 120, (int)mScreenHeight / 2 - 15, 240, 60), mFont72, "Loading...");
                gameLoad.Draw(spriteBatch);
                mWait2++;
                LoadBeginEbene();
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            mScreenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            mScreenHeight = mSpriteBatch.GraphicsDevice.Viewport.Height;
            var x = (int)(mScreenWidth - 300 * (mScreenWidth + 200) / 1000f);
            mSaveLabel.Update(x);
            mLetzterSpeicherBtn.Update(x);
            mBeginEbeneBtn.Update(x);
            mZurueckBtn.Update(x);
            // mAngle += 0.05f;
        }

    }
}
