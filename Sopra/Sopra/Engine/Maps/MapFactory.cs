using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Sopra.Engine.Maps
{
    public class MapFactory
    {
        private ContentManager mContentManager;

        public MapFactory(ContentManager contentManager)
        {
            mContentManager = contentManager;
        }

        public Map Load(string name)
        {
            var tmxMap = new TmxMap(name);

            var tilesets = CreateTilesets(tmxMap);

            var layers = CreateLayers(tmxMap, tilesets);
            
            return new Map(name, tmxMap.Width, tmxMap.Height, layers);
        }

        private static List<Layer> CreateLayers(TmxMap tmxMap, List<Tileset> tilesets)
        {
            var layers = new List<Layer>();

            foreach (var tmxLayer in tmxMap.Layers)
            {
                var tiles = new Tile[tmxMap.Height, tmxMap.Width];

                foreach (var tmxTile in tmxLayer.Tiles)
                {
                    var tile = tilesets
                        .Select(tileset => tileset[tmxTile.Gid])
                        .FirstOrDefault(t => t != null);

                    tiles[tmxTile.Y, tmxTile.X] = tile;
                }

                layers.Add(new Layer(tmxLayer.Name, tiles));
            }

            return layers;
        }

        private List<Tileset> CreateTilesets(TmxMap tmxMap)
        {
            var tilesets = new List<Tileset>();
            foreach (var tmxTileset in tmxMap.Tilesets)
            {
                tilesets.Add(CreateTileset(tmxTileset));
            }

            return tilesets;
        }

        private Tileset CreateTileset(TmxTileset tileset)
        {
            var texture = mContentManager.Load<Texture2D>(Path.GetFileNameWithoutExtension(tileset.Image.Source));
            
            return new Tileset(
                tileset.Name,
                tileset.TileWidth,
                tileset.TileHeight,
                tileset.Spacing,
                tileset.Columns ?? 0,
                tileset.TileCount ?? 0,
                tileset.FirstGid,
                texture);
        }
    }
}