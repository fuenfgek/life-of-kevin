using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.Maps
{
    public class Map
    {
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }

        private List<Layer> mLayers;

        public Map(string name, int width, int height, List<Layer> layers)
        {
            mLayers = layers;
            Name = name;
            Width = width;
            Height = height;
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var layer in mLayers)
            {
                layer.Draw(batch);
            }
        }
        
    }
}