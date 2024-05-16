using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.ECS;
using Sopra.Input;
using Sopra.Logic;
using Sopra.Logic.Achievements;
using Sopra.Logic.Boss;
using Sopra.Logic.Health;
using Sopra.Logic.Items;
using Sopra.Logic.RemoteControlled;
using Sopra.Maps;
using Sopra.UI;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Sopra.Screens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HudScreen : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private readonly GraphicsDeviceManager mGraphics;
        private Template mControllableTemplate = Template.All(typeof(UserControllableC)).Build();
        private readonly Template mBossTemplate = Template.All(typeof(BossKiC)).Build();
        private EntityManager mEntityManager;

        private Texture2D mHealthBar;
        private Texture2D mHealthBarFrame;
        private Texture2D mTaskBar;
        private Texture2D mActiveItemFrame;
        private Texture2D mReloadingFilter;
        private Texture2D mCoin;
        private SpriteFont mArial12;
        private SpriteFont mArial24;

        private Entity mControllable;
        private Entity mBoss;
        private Button mPauseButton;
        private List<Rectangle> mInvSlots;
        private Rectangle mTaskBarRect;
        private int mPressedSlot;
        private int mFpsCounter;
        private int mFpsCount;
        private long mLastTick;
        private bool mDraggingItem;
        private string mLevelNameString;

        public HudScreen(MenuActions menuActions, ContentManager content, GraphicsDeviceManager graphics)
        {
            mMenuActions = menuActions;
            mContent = content;
            mGraphics = graphics;
        }

        public void Initialize()
        {
            mControllableTemplate = new TemplateBuilder().All(typeof(UserControllableC)).Build();

            mHealthBar = mContent.Load<Texture2D>("graphics/hud/health_bar");
            mHealthBarFrame = mContent.Load<Texture2D>("graphics/hud/item_bar");
            mTaskBar = mContent.Load<Texture2D>("graphics/hud/inventory");
            mActiveItemFrame = mContent.Load<Texture2D>("graphics/hud/active_item_box_green");
            mReloadingFilter = mContent.Load<Texture2D>("graphics/hud/reloading_filter");
            mCoin = mContent.Load<Texture2D>("graphics/hud/coin");
            mArial12 = mContent.Load<SpriteFont>("fonts/arial_12");
            mArial24 = mContent.Load<SpriteFont>("fonts/arial_24");

            mPauseButton = new Button(
                    Rectangle.Empty,
                    mContent.Load<Texture2D>("graphics/pause/button_pause"),
                    mArial12)
                { mOnClick = mMenuActions.OpenPauseScreen };

            mLevelNameString = "Level 1";
        }

        internal void AssignLevel(Level level)
        {
            mEntityManager = level.Engine.EntityManager;
        }

        /// <summary>
        /// Draw the HUD.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            var viewpWidth = mGraphics.GraphicsDevice.Viewport.Width;
            var viewpHeight = mGraphics.GraphicsDevice.Viewport.Height;
            var optimalWidth = mGraphics.PreferredBackBufferWidth;
            var optimalHeight = mGraphics.PreferredBackBufferHeight;

            if (Environment.TickCount - mLastTick >= 1000)
            {
                mFpsCount = mFpsCounter;
                mFpsCounter = 0;
                mLastTick = Environment.TickCount;
            }
            mFpsCounter++;

            spriteBatch.Begin();

            var measures = mArial24.MeasureString(mLevelNameString);
            spriteBatch.Draw(
                mReloadingFilter, new Rectangle(
                    (int)(viewpWidth * (50.0 / optimalWidth)),
                    10,
                    (int)measures.X,
                    (int)measures.Y),
                Color.Black);
            spriteBatch.DrawString(
                mArial24,
                mLevelNameString,
                new Vector2(
                    (int)(viewpWidth * (50.0 / optimalWidth)),
                    10),
                Color.White);

            var fpsStr = mFpsCount.ToString();
            measures = mArial24.MeasureString(fpsStr);
            spriteBatch.Draw(
                mReloadingFilter, new Rectangle(
                    viewpWidth - 50,
                    10,
                    (int)measures.X,
                    (int)measures.Y),
                Color.Black);
            spriteBatch.DrawString(
                mArial24,
                fpsStr,
                new Vector2(viewpWidth - 50, 10),
                Color.White);

            if (mBoss != null
                && mBoss.GetComponent<BossKiC>().IsActiv)
            {
                DrawBossHealth(spriteBatch, viewpWidth, viewpHeight, optimalWidth, optimalHeight);
            }

            if (mControllable == null)
            {
                spriteBatch.End();
                return;
            }

            mPauseButton.ElementRectangle = new Rectangle(
                4,
                4,
                (int)(viewpWidth * (40.0 / optimalWidth)),
                (int)(viewpHeight * (40.0 / optimalHeight)));
            mPauseButton.Draw(spriteBatch);
            
            if (!mControllable.HasComponent<InventoryC>())
            {
                DrawItemBar(spriteBatch, viewpWidth, viewpHeight, optimalWidth, optimalHeight);
            }
            else
            {
                DrawInventory(spriteBatch, viewpWidth, viewpHeight, optimalWidth, optimalHeight);
                DrawAchievementNotification(spriteBatch, viewpWidth, viewpHeight, optimalWidth, optimalHeight);
                DrawCoins(spriteBatch, viewpWidth, viewpHeight, optimalWidth, optimalHeight);
            }

            spriteBatch.End();
        }

        private void DrawCoins(SpriteBatch spriteBatch, int screenWidth, int screenHeight, int optimalWidth, int optimalHeight)
        {
            var stats = Stats.Instance;
            spriteBatch.Draw(mCoin, new Rectangle(
                (int)(screenWidth / 2.0 + screenWidth * (128.0 / optimalWidth)),
                (int)(screenHeight - screenHeight * (62.0 / optimalHeight)),
                40,
                40),
                Color.White);
            spriteBatch.DrawString(
                mArial24,
                ((int)stats.CurrentCoins).ToString(),
                new Vector2(
                    (int)(screenWidth / 2.0 + screenWidth * (173.0 / optimalWidth)),
                    (int)(screenHeight - screenHeight * (61.0 / optimalHeight))),
                Color.Gold);
        }
        private void DrawAchievementNotification(SpriteBatch spriteBatch, int screenWidth, int screenHeight, int optimalWidth, int optimalHeight)
        {
            var stats = Stats.Instance;
            if (stats.IsActive)
            {
                var notification = "Achievement " + stats.ActiveAchievement + " unlocked!";
                var measures = mArial12.MeasureString(notification);
                spriteBatch.Draw(
                    mReloadingFilter,
                    new Rectangle(
                        (int)(screenWidth * (20.0 / optimalWidth)),
                        (int)(screenHeight * (80.0 / optimalHeight)),
                        (int)measures.X,
                        (int)measures.Y),
                    Color.Black);
                spriteBatch.DrawString(
                    mArial12,
                    notification,
                    new Vector2((int)(screenWidth * (20.0 / optimalWidth)),
                        (int)(screenHeight * (80.0 / optimalHeight))),
                    Color.Gold);
            }
            if (stats.AchievementTime >= 2)
            {
                stats.IsActive = false;
                stats.AchievementTime = 0;
            }
        }

        private void DrawItemBar(SpriteBatch spriteBatch, int screenWidth, int screenHeight, int optimalWidth, int optimalHeight)
        {
            var frameRect = new Rectangle(
                (int)(screenWidth / 2.0 - screenWidth * (128.0 / optimalWidth)),
                (int)(screenHeight - screenHeight * (51.0 / optimalHeight)),
                (int)(screenWidth * (256.0 / optimalWidth)),
                (int)(screenHeight * (16.0 / optimalHeight)));
            spriteBatch.Draw(mHealthBarFrame, frameRect, Color.White);

            Rectangle barRect;
            if (mControllable.HasComponent<CarC>())
            {
                var carC = mControllable.GetComponent<CarC>();
                barRect = new Rectangle(
                    (int)(frameRect.X + 1.0 / 256.0 * frameRect.Width),
                    (int)(frameRect.Y + 1.0 / 16.0 * frameRect.Height),
                    (int)((carC.Lifetime - (double)carC.PassedTime) / carC.Lifetime * frameRect.Width),
                    (int)(14.0 / 16.0 * frameRect.Height));
            }
            else
            {
                var droneC = mControllable.GetComponent<DroneC>();
                barRect = new Rectangle(
                    (int)(frameRect.X + 1.0 / 256.0 * frameRect.Width),
                    (int)(frameRect.Y + 1.0 / 16.0 * frameRect.Height),
                    (int)((droneC.Lifetime - (double)droneC.PassedTime) / droneC.Lifetime * frameRect.Width),
                    (int)(14.0 / 16.0 * frameRect.Height));
            }

            spriteBatch.Draw(mHealthBar, barRect, Color.White);
            spriteBatch.End();
        }

        private void DrawBossHealth(SpriteBatch spriteBatch, int screenWidth, int screenHeight, int optimalWidth, int optimalHeight)
        {
            var frameRect = new Rectangle(
                (int)(screenWidth / 2f - screenWidth * (128.0 / optimalWidth)),
                (int)(screenHeight * (20.0 / optimalHeight)),
                (int)(screenWidth * (256.0 / optimalWidth)),
                (int)(screenHeight * (16.0 / optimalHeight)));
            spriteBatch.Draw(mHealthBarFrame, frameRect, Color.White);

            var healthC = mBoss.GetComponent<HealthC>();
            var barRect = new Rectangle(
                (int)(frameRect.X + 1.0 / 256.0 * frameRect.Width),
                (int)(frameRect.Y + 1.0 / 16.0 * frameRect.Height),
                (int)(healthC.CurrentHealth / (double)healthC.MaxHealth * (mTaskBarRect.Width - 2)),
                (int)(14.0 / 16.0 * frameRect.Height));

            spriteBatch.Draw(mHealthBar, barRect, Color.White);
        }

        private void DrawInventory(SpriteBatch spriteBatch, int viewpWidth, int viewpHeight, int optimalWidth, int optimalHeight)
        {
            mTaskBarRect = new Rectangle(
               (int)(viewpWidth / 2.0 - viewpWidth * (128.0 / optimalWidth)),
               (int)(viewpHeight - viewpHeight * (64.0 / optimalHeight)),
               (int)(viewpWidth * (256.0 / optimalWidth)),
               (int)(viewpHeight * (64.0 / optimalHeight)));
            spriteBatch.Draw(mTaskBar, mTaskBarRect, Color.White);

            var healthC = mControllable.GetComponent<HealthC>();
            var healthBarRectangle = new Rectangle(
                (int)(mTaskBarRect.X + 1.0 / 256.0 * mTaskBarRect.Width),
                (int)(mTaskBarRect.Y + 1.0 / 64.0 * mTaskBarRect.Height),
                (int)(healthC.CurrentHealth / (double)healthC.MaxHealth * (mTaskBarRect.Width - 2)),
                (int)(14.0 / 64.0 * mTaskBarRect.Height));
            spriteBatch.Draw(mHealthBar, healthBarRectangle, Color.White);

            var invC = mControllable.GetComponent<InventoryC>();
            mInvSlots = new List<Rectangle>();
            for (var i = 0; i < invC.Size; i++)
            {
                mInvSlots.Add(new Rectangle(
                    (int)(mTaskBarRect.X + (i * 48.0 + 17.0) / 256.0 * mTaskBarRect.Width),
                    (int)(mTaskBarRect.Y + 24.0 / 64.0 * mTaskBarRect.Height),
                    (int)(mTaskBarRect.Width * (30.0 / 256.0)),
                    (int)(mTaskBarRect.Height * (30.0 / 64.0))));
            }

            spriteBatch.Draw(
                mActiveItemFrame,
                new Rectangle(
                    (int)(mTaskBarRect.X + (invC.ActiveSlot * 48.0 + 12.0) / 256.0 * mTaskBarRect.Width),
                    (int)(mTaskBarRect.Y + 19.0 / 64.0 * mTaskBarRect.Height),
                    (int)(mTaskBarRect.Width * (40.0 / 256.0)),
                    (int)(mTaskBarRect.Height * (40.0 / 64.0))),
                Color.White);

            var items = invC.InvSlots;
            for (var i = 0; i < invC.Size; i++)
            {
                if (items[i] == null)
                {
                    continue;
                }
                spriteBatch.Draw(
                    mContent.Load<Texture2D>(items[i].LogoPath), 
                    mInvSlots[i], 
                    Color.White);

                if (items[i].IsReloading)
                {
                    spriteBatch.Draw(
                        mReloadingFilter,
                        new Rectangle(
                            mInvSlots[i].X,
                            mInvSlots[i].Y,
                            (int)(items[i].PassedTime / (double)items[i].ReloadDuration * mInvSlots[i].Width),
                            mInvSlots[i].Height),
                        Color.White);
                }
            }

            if (mDraggingItem && items[mPressedSlot] != null)
            {
                spriteBatch.Draw(
                    mContent.Load<Texture2D>(items[mPressedSlot].LogoPath),
                    InputManager.Get().MouseState.Position.ToVector2(),
                    Color.White);
            }
        }

        /// <summary>
        /// Updates the screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            mControllable = mEntityManager.Get(mControllableTemplate).FirstOrDefault();
            mBoss = mEntityManager.Get(mBossTemplate).FirstOrDefault();
        }

        /// <summary>
        /// Handle all mouse events targeting the task bar.
        /// Implements:
        ///     - Choosing active item by clicking on slot.
        ///     - Change position of two items by draging an item to another slot.    
        /// </summary>
        /// <param name="mouseEvent"></param>
        /// <returns></returns>
        public override bool HandleMouseEvent(MouseEvent mouseEvent)
        {
            if (mControllable == null
                || !MouseInRect(mouseEvent.PostitionPressed, mTaskBarRect)
                && !mPauseButton.ElementRectangle.Contains(mouseEvent.PostitionPressed))
            {
                return false;
            }

            if (!mouseEvent.PositionReleased.HasValue)
            {
                mPressedSlot = -1;
                for (var i = 0; i < mInvSlots.Count; i++)
                {
                    if (!MouseInRect(mouseEvent.PostitionPressed, mInvSlots[i]))
                    {
                        continue;
                    }
                    mPressedSlot = i;
                    mDraggingItem = true;
                }
            }
            else if (mPressedSlot != -1)
            {
                mDraggingItem = false;
                var releasedSlot = -1;
                for (var i = 0; i < mInvSlots.Count; i++)
                {
                    if (MouseInRect(mouseEvent.PositionReleased.Value, mInvSlots[i]))
                    {
                        releasedSlot = i;
                    }
                }

                var invC = mControllable.GetComponent<InventoryC>();
                if (releasedSlot != -1 && releasedSlot == mPressedSlot)
                {
                    invC.ActiveSlot = mPressedSlot;
                    return true;
                }

                if(mPressedSlot != -1 && releasedSlot != -1)
                {
                    invC.SwapItemsInInv(mPressedSlot, releasedSlot);
                    return true;
                }
            }
            return true;
        }

        private static bool MouseInRect(Point mousePos, Rectangle rect)
        {
            return rect.X < mousePos.X 
                   && mousePos.X < rect.X + rect.Width
                   && rect.Y < mousePos.Y
                   && mousePos.Y < rect.Y + rect.Height;
        }

        /// <summary>
        /// Update current level label.
        /// </summary>
        /// <param name="levelNum"></param>
        public void UpdateLevelLabel(int levelNum)
        {
            mLevelNameString = "Level " + levelNum;
        }
    }
}