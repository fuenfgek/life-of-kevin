using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.ECS;
using Sopra.Input;

namespace Sopra.Logic
{
    /// <summary>
    /// System to lock the camera at Kevins position
    /// </summary>
    /// <author>Marcel Ebbinghaus</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    internal sealed class CameraSystem : IteratingEntitySystem
    {
        private readonly SpriteBatch mSpriteBatch;
        private readonly GraphicsDeviceManager mGraphics;

        public CameraSystem(SpriteBatch batch, GraphicsDeviceManager graphics) :
            base(new TemplateBuilder().All(
                typeof(TransformC),
                typeof(UserControllableC)))
        {
            mSpriteBatch = batch;
            mGraphics = graphics;
        }

        protected override void Process(Entity entity, GameTime time)
        {
            var transform = entity.GetComponent<TransformC>(TransformC.Type);
            var viewp = mSpriteBatch.GraphicsDevice.Viewport;
            var screenscaleWidth = viewp.Width / (float)mGraphics.PreferredBackBufferWidth;
            var screenscaleHeight = viewp.Height / (float)mGraphics.PreferredBackBufferHeight;
            var position = Matrix.CreateTranslation(
                -(float)Math.Round(transform.CurrentPosition.X),
                -(float)Math.Round(transform.CurrentPosition.Y),
                0);
            var offset = Matrix.CreateTranslation(
                viewp.Width * 0.5f,
                viewp.Height * 0.5f,
                0);
            var scaling = Matrix.CreateScale(screenscaleWidth, screenscaleHeight, 1);
            var matrix = position * scaling * offset;

            //minimalize flickering
            matrix.M41 = (float)Math.Ceiling(matrix.M41);
            matrix.M42 = (float)Math.Ceiling(matrix.M42);
            matrix.M43 = (float)Math.Ceiling(matrix.M43);
            matrix.M44 = (float)Math.Ceiling(matrix.M44);

            mEngine.CameraMatrix = matrix;
            InputManager.Get().CameraMatrix = matrix;
        }
    }
}

