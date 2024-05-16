using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.Logic.Achievements;
using Sopra.UI;

namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// The achievement screen class.
    /// </summary>
    /// <inheritdoc cref="AbstractScreen"/>
    /// <author>Marcel Ebbinghaus</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AchievementMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private Button mZurueckBtn;
        private readonly SpriteBatch mSpriteBatch;
        private float mScreenscaleWidth;
        private float mScreenscaleHeight;
        private SpriteFont mFont;
        private Texture2D mBackground;
        private Texture2D mAfk;
        private Texture2D mEnemyDmg;
        private Texture2D mCoffee1;
        private Texture2D mCoffee2;
        private Texture2D mCoffee3;
        private Texture2D mKilledEnemies1;
        private Texture2D mKilledEnemies2;
        private Texture2D mKilledEnemies3;
        private Texture2D mStart;
        private Texture2D mTime1;
        private Texture2D mTime2;
        private Texture2D mBoss;
        private Texture2D mFlawless;
        private Texture2D mStealthkills;
        private Texture2D mFloor1;
        private Texture2D mItems;
        private Texture2D mCoins;
        private Texture2D mUpgrade;
        private Texture2D mAchievements;
        private float mScreenWidth;
        private float mScreenHeight;
        private readonly List<Color> mColorList = new List<Color>();
        private int mCounter;
        private int mInitializer;

        /// <summary>
        /// Achievements class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        public AchievementMenu(MenuActions menuActions, ContentManager content, SpriteBatch batch) : base(false, false)
        {
            mContent = content;
            mMenuActions = menuActions;
            mSpriteBatch = batch;
        }

        /// <summary>
        /// Initialize the achievement Menu.
        /// </summary>
        /// /// <inheritdoc cref="IInitializable"/>
        public void Initialize()
        {
            LoadFonts();
            mBackground = mContent.Load<Texture2D>("graphics/stats/background");
            mAfk = mContent.Load<Texture2D>("graphics/achievements/afk");
            mEnemyDmg = mContent.Load<Texture2D>("graphics/achievements/sadistic");
            mCoffee1 = mContent.Load<Texture2D>("graphics/achievements/wakeup");
            mCoffee2 = mContent.Load<Texture2D>("graphics/achievements/heartattack");
            mCoffee3 = mContent.Load<Texture2D>("graphics/achievements/obsessed");
            mKilledEnemies1 = mContent.Load<Texture2D>("graphics/achievements/rookie");
            mKilledEnemies2 = mContent.Load<Texture2D>("graphics/achievements/war_machine");
            mKilledEnemies3 = mContent.Load<Texture2D>("graphics/achievements/terminator");
            mTime1 = mContent.Load<Texture2D>("graphics/achievements/friends");
            mTime2 = mContent.Load<Texture2D>("graphics/achievements/nofriends");
            mBoss = mContent.Load<Texture2D>("graphics/achievements/alpha_kevin");
            mFlawless = mContent.Load<Texture2D>("graphics/achievements/easy_peasy");
            mStealthkills = mContent.Load<Texture2D>("graphics/achievements/agent47");
            mFloor1 = mContent.Load<Texture2D>("graphics/achievements/challenge");
            mStart = mContent.Load<Texture2D>("graphics/achievements/journey");        
            mItems = mContent.Load<Texture2D>("graphics/achievements/lord_of_war");
            mCoins = mContent.Load<Texture2D>("graphics/achievements/moneyboy");
            mAchievements = mContent.Load<Texture2D>("graphics/achievements/achievements");
            mUpgrade = mContent.Load<Texture2D>("graphics/achievements/craftsman");

            while (mInitializer <= 17)
            {
                mColorList.Add(Color.Black);
                mInitializer += 1;
            }
            CreateButtons();
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

        /// <summary>
        /// Draw every element in achievement screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, (int)(800 * mScreenscaleWidth), (int)(600 * mScreenscaleHeight)), Color.White);
            spriteBatch.Draw(mAfk, new Rectangle((int)(mScreenWidth / 2 - 260), 60, 200, 40), mColorList[0]);
            spriteBatch.Draw(mEnemyDmg, new Rectangle((int)(mScreenWidth / 2 - 280), 110, 240, 40), mColorList[1]);
            spriteBatch.Draw(mCoffee1, new Rectangle((int)(mScreenWidth / 2 - 280), 160, 240, 40), mColorList[2]);
            spriteBatch.Draw(mCoffee2, new Rectangle((int)(mScreenWidth / 2 - 280), 210, 240, 40), mColorList[3]);
            spriteBatch.Draw(mCoffee3, new Rectangle((int)(mScreenWidth / 2 - 280), 260, 240, 40), mColorList[4]);
            spriteBatch.Draw(mKilledEnemies1, new Rectangle((int)(mScreenWidth / 2 - 280), 310, 240, 40), mColorList[5]);
            spriteBatch.Draw(mKilledEnemies2, new Rectangle((int)(mScreenWidth / 2 - 260), 360, 200, 40), mColorList[6]);
            spriteBatch.Draw(mKilledEnemies3, new Rectangle((int)(mScreenWidth / 2 - 260), 410, 200, 40), mColorList[7]);
            spriteBatch.Draw(mStart, new Rectangle((int)(mScreenWidth / 2 + 30), 60, 260, 40), mColorList[8]);
            spriteBatch.Draw(mTime1, new Rectangle((int)(mScreenWidth / 2 + 40), 110, 240, 40), mColorList[9]);
            spriteBatch.Draw(mTime2, new Rectangle((int)(mScreenWidth / 2 + 20), 160, 300, 40), mColorList[10]);
            spriteBatch.Draw(mBoss, new Rectangle((int)(mScreenWidth / 2 + 40), 210, 240, 40), mColorList[11]);
            spriteBatch.Draw(mFlawless, new Rectangle((int)(mScreenWidth / 2 + 40), 260, 240, 40), mColorList[12]);
            spriteBatch.Draw(mStealthkills, new Rectangle((int)(mScreenWidth / 2 + 40), 310, 240, 40), mColorList[13]);
            spriteBatch.Draw(mFloor1, new Rectangle((int)(mScreenWidth / 2 + 40), 360, 240, 40), mColorList[14]);
            spriteBatch.Draw(mItems, new Rectangle((int)(mScreenWidth / 2 + 40), 410, 240, 40), mColorList[15]);
            spriteBatch.Draw(mCoins, new Rectangle((int)(mScreenWidth / 2 + 40), 460, 240, 40), mColorList[16]);
            spriteBatch.Draw(mUpgrade, new Rectangle((int)(mScreenWidth / 2 - 280), 460, 240, 40), mColorList[17]);
            spriteBatch.Draw(mAchievements, new Rectangle((int)(mScreenWidth / 2 - 130), 0, 260, 60), Color.White);
            mZurueckBtn.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Updates buttons and label positions according to screen width.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            mScreenscaleWidth = mSpriteBatch.GraphicsDevice.Viewport.Width / 800f;
            mScreenscaleHeight = mSpriteBatch.GraphicsDevice.Viewport.Height / 600f;
            mCounter = 0;
            foreach (var achievement in AchievementSystem.sAchievementList)
            {
                if (achievement.Achieved)
                {
                    mColorList[mCounter] = Color.Gold;
                }
                else
                {
                    mColorList[mCounter] = Color.White;
                }

                mCounter += 1;
            }
            mCounter = 0;
            mScreenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            mScreenHeight = mSpriteBatch.GraphicsDevice.Viewport.Height;
            mZurueckBtn.ElementRectangle =
                new Rectangle((int)(mScreenWidth / 2) - 120, (int)(mScreenHeight - (mScreenHeight / 20 + 60)), 240, 60);
        }
    }
}
