using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Sopra.ECS;

namespace Sopra.Screens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class QuadScreen : AbstractScreen, IInitializable
    {
        private readonly Engine mEngine;
        private readonly ContentManager mContent;
        private Texture2D mTexture;
        private readonly Matrix mMatrix;

        public QuadScreen(Engine engine, ContentManager content)
        {
            mEngine = engine;
            mContent = content;
            mMatrix =  Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(0f, 25f, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: mMatrix);
            mEngine.mQuadTree.Draw(spriteBatch);
            mEngine.mQuadTree.DrawObject(spriteBatch, mTexture);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void Initialize()
        {
            mTexture = mContent.Load<Texture2D>("test_sprites/brown_pixel");
        }
    }
}