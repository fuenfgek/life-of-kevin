using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.Logic.Achievements;
using Sopra.UI;

namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// The stat screen class.
    /// </summary>
    /// <inheritdoc cref="AbstractScreen"/>
    /// <author>Marcel Ebbinghaus</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StatMenu : AbstractScreen, IInitializable
    {
        private readonly Stats mStats = Stats.Instance;
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private Button mZurueckBtn;
        private Button mAchievementsBtn;
        private Texture2D mStatTexture;
        private Texture2D mReceivedDmgTexture;
        private Texture2D mDealtDmgTexture;
        private Texture2D mUsedCoffeesTexture;
        private Texture2D mKillsTexture;
        private Texture2D mTimeTexture;
        private Texture2D mStealthkillsTexture;
        private Texture2D mDeathsTexture;
        private Texture2D mWeaponsTexture;
        private Texture2D mCoinsTexture;
        private Label mStatKevinReceivedDmg;
        private Label mStatEnemyReceivedDmg;
        private Label mStatUsedCoffees;
        private Label mStatKilledEnemies;
        private Label mStatTimeHours;
        private Label mStatTimeMinutes;
        private Label mStatTimeSeconds;
        private Label mStatStealthkills;
        private Label mStatDeaths;
        private Label mStatWeapons;
        private Label mStatCoins;
        private readonly SpriteBatch mSpriteBatch;
        private float mScreenscaleWidth;
        private float mScreenscaleHeight;
        private float mScreenWidth;
        private float mScreenHeight;
        private SpriteFont mFont;
        private Texture2D mBackground;

        /// <summary>
        /// Stats class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        public StatMenu(MenuActions menuActions, ContentManager content, SpriteBatch batch) : base(false, false)
        {
            mContent = content;
            mMenuActions = menuActions;
            mSpriteBatch = batch;
        }

        /// <summary>
        /// Initialize the stat Menu.
        /// </summary>
        /// /// <inheritdoc cref="IInitializable"/>
        public void Initialize()
        {
            LoadFonts();
            mBackground = mContent.Load<Texture2D>("graphics/stats/background");
            mStatTexture = mContent.Load<Texture2D>("graphics/stats/statistiken");
            mReceivedDmgTexture = mContent.Load<Texture2D>("graphics/stats/erlittener_schaden");
            mDealtDmgTexture = mContent.Load<Texture2D>("graphics/stats/zugefuegter_schaden");
            mUsedCoffeesTexture = mContent.Load<Texture2D>("graphics/stats/getrunkene_kaffeeschluecke");
            mKillsTexture = mContent.Load<Texture2D>("graphics/stats/getoetete_gegner");
            mTimeTexture = mContent.Load<Texture2D>("graphics/stats/gespielte_zeit");
            mStealthkillsTexture = mContent.Load<Texture2D>("graphics/stats/lautlos_getoetete_gegner");
            mDeathsTexture = mContent.Load<Texture2D>("graphics/stats/tode");
            mWeaponsTexture = mContent.Load<Texture2D>("graphics/stats/aufgesammelte_waffen");
            mCoinsTexture = mContent.Load<Texture2D>("graphics/stats/gesammeltes_geld");
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
            var achievementsBtnTe = mContent.Load<Texture2D>("graphics/stats/button_achievements");

            mZurueckBtn = new Button(new Rectangle(150, 510, 240, 60), zurueckBtnTe, mFont);
            mAchievementsBtn = new Button(new Rectangle(410, 510, 240, 60), achievementsBtnTe, mFont);
        }

        private void CreateLabel()
        {
            var hours = Math.Floor(mStats.Time / 3600);
            var minutes = Math.Floor((mStats.Time - 3600 * hours) / 60);
            var seconds = Math.Round(mStats.Time - 3600 * hours - 60 * minutes);
            mStatKevinReceivedDmg = new Label(new Rectangle(540, 80, 20, 30), mFont, ((int)mStats.KevinReceivedDmg).ToString());
            mStatEnemyReceivedDmg = new Label(new Rectangle(550, 130, 20, 30), mFont, ((int)mStats.EnemyReceivedDmg).ToString());
            mStatUsedCoffees = new Label(new Rectangle(570, 180, 20, 30), mFont, ((int)mStats.UsedCoffees).ToString());
            mStatKilledEnemies = new Label(new Rectangle(510, 230, 20, 30), mFont, ((int)mStats.KilledEnemies).ToString());
            mStatTimeHours = new Label(new Rectangle(370, 280, 20, 30), mFont, ((int)hours).ToString());
            mStatTimeMinutes = new Label(new Rectangle(480, 280, 20, 30), mFont, ((int)minutes).ToString());
            mStatTimeSeconds = new Label(new Rectangle(550, 280, 20, 30), mFont, ((int)seconds).ToString());
            mStatStealthkills = new Label(new Rectangle(560, 330, 20, 30), mFont, ((int)mStats.Stealthkills).ToString());
            mStatDeaths = new Label(new Rectangle(440, 380, 20, 30), mFont, ((int)mStats.Deaths).ToString());
            mStatWeapons = new Label(new Rectangle(550, 430, 20, 30), mFont, ((int)mStats.Items).ToString());
            mStatCoins = new Label(new Rectangle(550, 480, 20, 30), mFont, ((int)mStats.Coins).ToString());
        }

        private void Actions()
        {
            mZurueckBtn.mOnClick = ZuruekAction;
            mAchievementsBtn.mOnClick = () => mMenuActions.OpenAchievementScreen();
        }

        private void ZuruekAction()
        {
            mMenuActions.Remove(this);
        }

        /// <summary>
        /// Draw every element in stat screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, (int)(800 * mScreenscaleWidth), (int)(600 * mScreenscaleHeight)), Color.White);
            spriteBatch.Draw(mStatTexture, new Rectangle((int)(mScreenWidth / 2 - 120), 0, 240, 60), Color.White);
            spriteBatch.Draw(mReceivedDmgTexture, new Rectangle((int)(mScreenWidth / 2 - 120), 80, 240, 30), Color.White);
            spriteBatch.Draw(mDealtDmgTexture, new Rectangle((int)(mScreenWidth / 2 - 130), 130, 260, 30), Color.White);
            spriteBatch.Draw(mUsedCoffeesTexture, new Rectangle((int)(mScreenWidth / 2 - 170), 180, 340, 30), Color.White);
            spriteBatch.Draw(mKillsTexture, new Rectangle((int)(mScreenWidth / 2 - 110), 230, 220, 30), Color.White);
            spriteBatch.Draw(mTimeTexture, new Rectangle((int)(mScreenWidth / 2 - 210), 280, 420, 30), Color.White);
            spriteBatch.Draw(mStealthkillsTexture, new Rectangle((int)(mScreenWidth / 2 - 160), 330, 320, 30), Color.White);
            spriteBatch.Draw(mDeathsTexture, new Rectangle((int)(mScreenWidth / 2 - 40), 380, 80, 30), Color.White);
            spriteBatch.Draw(mWeaponsTexture, new Rectangle((int)(mScreenWidth / 2 - 150), 430, 300, 30), Color.White);
            spriteBatch.Draw(mCoinsTexture, new Rectangle((int)(mScreenWidth / 2 - 130), 480, 260, 30), Color.White);
            mZurueckBtn.Draw(spriteBatch);
            mAchievementsBtn.Draw(spriteBatch);
            mStatKevinReceivedDmg.Draw(spriteBatch);
            mStatEnemyReceivedDmg.Draw(spriteBatch);
            mStatUsedCoffees.Draw(spriteBatch);
            mStatKilledEnemies.Draw(spriteBatch);
            mStatTimeHours.Draw(spriteBatch);
            mStatTimeMinutes.Draw(spriteBatch);
            mStatTimeSeconds.Draw(spriteBatch);
            mStatStealthkills.Draw(spriteBatch);
            mStatDeaths.Draw(spriteBatch);
            mStatWeapons.Draw(spriteBatch);
            mStatCoins.Draw(spriteBatch);
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

            mScreenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            mScreenHeight = mSpriteBatch.GraphicsDevice.Viewport.Height;
            mZurueckBtn.ElementRectangle =
                new Rectangle((int)(mScreenWidth / 2) - 250, (int)(mScreenHeight - (mScreenHeight / 20 + 60)), 240, 60);
            mAchievementsBtn.ElementRectangle = 
                new Rectangle((int)(mScreenWidth / 2) + 10, (int)(mScreenHeight - (mScreenHeight / 20 + 60)), 240, 60);
            mStatKevinReceivedDmg.Update((int)(mScreenWidth / 2) + 140);
            mStatEnemyReceivedDmg.Update((int)(mScreenWidth / 2) + 150);
            mStatUsedCoffees.Update((int)(mScreenWidth / 2) + 190);
            mStatKilledEnemies.Update((int)(mScreenWidth / 2) + 130);
            mStatTimeHours.Update((int)(mScreenWidth / 2) - 15);
            mStatTimeMinutes.Update((int)(mScreenWidth / 2) + 36);
            mStatTimeSeconds.Update((int)(mScreenWidth / 2) + 118);
            mStatStealthkills.Update((int)(mScreenWidth / 2) + 180);
            mStatDeaths.Update((int)(mScreenWidth / 2) + 60);
            mStatWeapons.Update((int)(mScreenWidth / 2) + 170);
            mStatCoins.Update((int)(mScreenWidth / 2) + 150);
        }

    }
}
