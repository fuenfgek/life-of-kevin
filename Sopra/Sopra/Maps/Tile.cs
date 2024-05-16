using Microsoft.Xna.Framework;

namespace Sopra.Maps
{
    public sealed class Tile
    {
        public int Id { get; }
        public Tileset Tileset { get; }
        public Rectangle Rectangle { get; }
        public int Width => Tileset.TileWidth;
        public int Height => Tileset.TileHeight;

        public Tile(int id, Tileset tileset, Rectangle rectangle)
        {
            Id = id;
            Tileset = tileset;
            Rectangle = rectangle;
        }
    }
}