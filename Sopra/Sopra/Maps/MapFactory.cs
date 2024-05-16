using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Sopra.Maps
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MapFactory
    {
        private readonly ContentManager mContentManager;

        public MapFactory(ContentManager contentManager)
        {
            mContentManager = contentManager;
        }

        public Map Load(string name)
        {
            var tmxMap = new TmxMap(name);

            return new Map(
                tmxMap,
                name,
                tmxMap.Width,
                tmxMap.Height,
                CreateLayers(tmxMap, CreateTilesets(tmxMap)),
                CreateWallHitboxList(tmxMap),
                CreateMapObjectList(tmxMap, name),
                CreateSpawnList(tmxMap, name),
                CreateStoryList(tmxMap, name),
                CreateEnemyPathList(tmxMap, name));
        }

        private static List<MapObject> CreateWallHitboxList(TmxMap map)
        {
            var wallHitboxList = new List<MapObject>();


            foreach (var o in map.ObjectGroups["Walls"].Objects)
            {
                wallHitboxList
                    .Add(new MapObject(
                        o.Name,
                        o.Type,
                        (float)(o.X + o.Width / 2),
                        (float)(o.Y + o.Height / 2), 
                        (float)o.Width,
                        (float)o.Height,
                        o.Points,
                        o.Properties));
            }

            return wallHitboxList;
        }

        
        private static List<MapObject> CreateStoryList(TmxMap map, string name)
        {
            var storyObjects = new List<MapObject>();

            if (!name.Equals("Content/maps/leveltutorial.tmx"))
            {
                return storyObjects;
            }

            foreach (var o in map.ObjectGroups["Story"].Objects)
            {
                storyObjects
                    .Add(new MapObject(
                        o.Name,
                        o.Type,
                        (float)(o.X + o.Width / 2),
                        (float)(o.Y + o.Height / 2),
                        (float)o.Width,
                        (float)o.Height,
                        o.Points,
                        o.Properties));
            }

            return storyObjects;
        }


        private static List<MapObject> CreateSpawnList(TmxMap map,string name)
        {
            var spawnObjects = new List<MapObject>();

            if (name.Equals("Content/maps/techdemo.tmx"))
            {
                return spawnObjects;
            }
            foreach (var o in map.ObjectGroups["Spawns"].Objects)
            {
                spawnObjects
                    .Add(new MapObject(
                        o.Name,
                        o.Type, 
                        (float)(o.X + o.Width / 2),
                        (float)(o.Y - o.Height / 2),
                        (float)o.Width, (float)o.Height,
                        o.Points,
                        o.Properties));
            }

            return spawnObjects;
        }


        private static List<MapObject> CreateMapObjectList(TmxMap map,string name)
        {
            var mapObjectList = new List<MapObject>();

            if (name.Equals("Content/maps/techdemo.tmx"))
            {
                return mapObjectList;
            }

            foreach (var o in map.ObjectGroups["MapObjects"].Objects)
            {
                mapObjectList
                    .Add(new MapObject(
                        o.Name, 
                        o.Type,
                        (float)(o.X + o.Width / 2),
                        (float)(o.Y - o.Height / 2),
                        (float)o.Width, (float)o.Height,
                        o.Points,
                        o.Properties));
            }
            return mapObjectList;
        }

        private static List<MapObject> CreateEnemyPathList(TmxMap map,string name)
        {
            var enemyPathList = new List<MapObject>();

            if (name.Equals("Content/maps/techdemo.tmx"))
            {
                return enemyPathList;
            }
            foreach (var o in map.ObjectGroups["EnemyPaths"].Objects)
            {
                enemyPathList
                    .Add(new MapObject(
                        o.Name,
                        o.Type,
                        (float)(o.X + o.Width / 2),
                        (float)(o.Y - o.Height / 2), 
                        (float)o.Width, (float)o.Height,
                        o.Points,
                        o.Properties));
            }
            return enemyPathList;
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

                layers.Add(new Layer(tiles));
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
            var texture = mContentManager.Load<Texture2D>("maps/" + Path.GetFileNameWithoutExtension(tileset.Image.Source));
            
            return new Tileset(
                tileset.TileWidth,
                tileset.TileHeight,
                tileset.Spacing,
                tileset.TileCount ?? 0,
                tileset.FirstGid,
                texture);
        }
    }
}