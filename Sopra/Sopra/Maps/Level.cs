using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.ECS;

namespace Sopra.Maps
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Level
    {
        public Map Map { get; }
        public Engine Engine { get; }
        private readonly Rectangle mBackgroundRectangle;

        public Level(Map map, Engine engine)
        {
            Map = map;
            Engine = engine;
            mBackgroundRectangle = new Rectangle(-640, -360, Map.Width * 64 + 1280, Map.Height * 64 + 720);
        }

        public void Update(GameTime time)
        {
            Engine.Update(time);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Engine.CameraMatrix);

            batch.Draw(
                Engine.Content.Load<Texture2D>("mapobjects/background"),
                mBackgroundRectangle,
                Color.White);

            Map.Draw(batch);
            batch.End();
            Engine.Draw();
        }
    }
}