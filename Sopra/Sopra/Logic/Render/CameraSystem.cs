using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.ECS;

namespace Sopra.Logic.Render
{
    internal sealed class CameraSystem : IteratingEntitySystem
    {
        private readonly SpriteBatch mSpriteBatch;

        public CameraSystem(SpriteBatch spriteBatch) :
            base(new TemplateBuilder().All(
                typeof(TransformC),
                typeof(UserControllableC)))
        {
            mSpriteBatch = spriteBatch;
        }

        public override void Process(Entity entity, GameTime time)
        {
            var resolutionX = 800;
            var resolutionY = 600;
            var viewp = mSpriteBatch.GraphicsDevice.Viewport;

            var transformC = entity.GetComponent<TransformC>();

            var scaleWidth = viewp.Width / (float) resolutionX;
            var scaleHeight = viewp.Height / (float) resolutionY;

            var position = Matrix.CreateTranslation(
                -transformC.CurrentPosition.X,
                -transformC.CurrentPosition.Y,
                0);
            var offset = Matrix.CreateTranslation(
                viewp.Width * 0.5f,
                viewp.Height * 0.5f,
                0);
            var scaling = Matrix.CreateScale(new Vector3(scaleWidth, scaleHeight, 1));
            
            mEngine.CameraMatrix = position * scaling * offset;
        }
    }
}

