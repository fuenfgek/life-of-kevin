using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.Maps
{
    public class World
    {
        private ECS.Engine mEngine;
        private List<Layer> mLayers;

        public World(ECS.Engine engine, List<Layer> layers)
        {
            mEngine = engine;
            mLayers = layers;
        }


        public void Draw(SpriteBatch batch, GameTime time)
        {
            mLayers.ForEach(layer => layer.Draw(batch));
            mEngine.Draw(batch, time);
        }

        public void Update(GameTime time)
        {
            mEngine.Update(time);
        }
    }
}