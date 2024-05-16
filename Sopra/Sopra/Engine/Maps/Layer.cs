using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.Maps
{
    public class Layer
    {
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public Tile[,] Tiles { get; }

        public Layer(string name, Tile[,] tiles)
        {
            Name = name;
            Tiles = tiles;
            Height = tiles.GetLength(0);
            Width = tiles.GetLength(1);
        }

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var tile = Tiles[i, j];

                    if (tile == null)
                    {
                        continue;
                    }

                    var position = new Vector2(
                        j * tile.Height,
                        i * tile.Width);

                    batch.Draw(
                        tile.Tileset.Texture,
                        position,
                        tile.Rectangle,
                        Color.White);
                }
            }
        }
    }
}