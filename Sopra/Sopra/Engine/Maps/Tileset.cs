using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.Maps
{
    public class Tileset
    {
        public string Name { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public int Spacing { get; }
        public int Columns { get; }
        public int Count { get; }
        public int FirstId { get; }
        public Texture2D Texture { get; }
        private Dictionary<int, Tile> mTiles;

        public Tile this[int tileId] => mTiles[tileId];

        public Tileset(
            string name,
            int tileWidth,
            int tileHeight,
            int spacing,
            int columns,
            int count,
            int firstId,
            Texture2D texture
        )
        {
            Name = name;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Spacing = spacing;
            Columns = columns;
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