using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Maps
{
    public sealed class Tileset
    {
        public int TileWidth { get; }
        public int TileHeight { get; }
        private int Spacing { get; }
        private int Count { get; }
        private int FirstId { get; }
        public Texture2D Texture { get; }
        private Dictionary<int, Tile> mTiles;

        public Tile this[int tileId] => mTiles[tileId];

        public Tileset(
            int tileWidth,
            int tileHeight,
            int spacing,
            int count,
            int firstId,
            Texture2D texture
        )
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Spacing = spacing;
            Count = count;
            Texture = texture;
            FirstId = firstId;

            CreateTiles();
        }

        private void CreateTiles()
        {
            mTiles = new Dictionary<int, Tile>();
            var x = 0;
            var y = 0;

            for (var i = 0; i < Count; i++)
            {
                var tile = new Tile(
                    FirstId + i,
                    this,
                    new Rectangle(
                        x,
                        y,
                        TileWidth,
                        TileHeight
                    ));

                mTiles.Add(tile.Id, tile);

                x = x + TileWidth + Spacing;
                if (x >= Texture.Width)
                {
                    x = 0;
                    y += TileHeight + Spacing;
                }
            }
        }
    }
}