using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Sopra.Maps
{
    public sealed class Map
    {
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public TmxMap TmxMap { get; }

        private readonly List<Layer> mLayers;
        internal List<MapObject> MapObjectList { get; }
        internal List<MapObject> WallHitboxList { get; }
        internal List<MapObject> SpawnList { get; }
        internal List<MapObject> StoryList { get; }
        internal List<MapObject> EnemyPathList { get; }

        public Map(
            TmxMap tmxMap,
            string name,
            int width, 
            int height,
            List<Layer> layers,
            List<MapObject> wallHitboxList,
            List<MapObject> mapObjectList,
            List<MapObject> spawnList,
            List<MapObject> storyList,
            List<MapObject> enemyPathList)
        {
            TmxMap = tmxMap;
            Name = name;
            Width = width;
            Height = height;
            mLayers = layers;
            WallHitboxList = wallHitboxList;
            MapObjectList = mapObjectList;
            SpawnList = spawnList;
            StoryList = storyList;
            EnemyPathList = enemyPathList;
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