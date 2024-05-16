using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;

namespace Sopra.Screens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TechDemoStatsScreen : AbstractScreen, IInitializable
    {
        private readonly ContentManager mContent;
        private SpriteFont mFont;
        private int mLastTick;
        private int mFpsCount;
        private int mFpsCounter;
        private readonly Texture2D mReloadingFilter;

        public TechDemoStatsScreen(ContentManager content)
        {
            mContent = content;
            mReloadingFilter = mContent.Load<Texture2D>("graphics/hud/reloading_filter");
        }

        public void Initialize()
        {
            mFont = mContent.Load<SpriteFont>("fonts/arial_24");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Environment.TickCount - mLastTick >= 1000)
            {
                mFpsCount = mFpsCounter;
                mFpsCounter = 0;
                mLastTick = Environment.TickCount;
            }
            mFpsCounter++;

            spriteBatch.Begin();
            var measures = mFont.MeasureString(mFpsCount.ToString());
            spriteBatch.Draw(
                mReloadingFilter, new Rectangle(
                    spriteBatch.GraphicsDevice.Viewport.Width - 50,
                    10,
                    (int)measures.X,
                    (int)measures.Y),
                Color.Black);
            spriteBatch.DrawString(mFont, mFpsCount.ToString(), new Vector2(spriteBatch.GraphicsDevice.Viewport.Width - 50, 10), Color.White );
            
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}