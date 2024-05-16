using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Maps
{
    public sealed class Layer
    {
        private int Width { get; }
        private int Height { get; }
        private Tile[,] Tiles { get; }

        public Layer(Tile[,] tiles)
        {
            Tiles = tiles;
            Height = tiles.GetLength(0);
            Width = tiles.GetLength(1);
        }

        public void Draw(SpriteBatch batch)
        {
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    var tile = Tiles[i, j];

                    if (tile == null)
                    {
                        continue;
                    }

                    var position = new Vector2(
                        j * tile.Width,
                        i * tile.Height);

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