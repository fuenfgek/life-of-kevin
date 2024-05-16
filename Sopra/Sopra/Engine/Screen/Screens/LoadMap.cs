using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Sopra.Engine.Screen.Screens
{
    /// <summary>
    /// The class, that loads Tiled based Maps from XML and render those.
    /// </summary>
    /// <author>Nico Greiner</author>
    class LoadMap : AbstractScreen
    {
        private ContentManager mContent;
        private String mMapPath;
        private String mTilesetPath;

        private Texture2D mTileset;
        private TmxMap mMap;

        private int mTileWidth;
        private int mTileHeight;
        private int mTilesetTilesWide;
        private int mLayers;
        private List<Rectangle> mCollisionObjects = new List<Rectangle>();

        public LoadMap(ContentManager content, String mapPath, String tilesetPath, int layers)
        {
            /// <summary>
            /// LoadMap class constructor.
            /// </summary>
            /// <param name="content"></param>
            /// <param name="mapPath"></param>
            /// <param name="tilesetPath"></param>
            /// <param name="layers"></param>

            mContent = content;
            mMapPath = mapPath;
            mTilesetPath = tilesetPath;
            mLayers = layers;

            LoadContentMap();
        }

        private void LoadContentMap()
        {

            mMap = new TmxMap(mMapPath);

            mTileset = mContent.Load<Texture2D>(mTilesetPath);

            mTileWidth = mMap.Tilesets[0].TileWidth;
            mTileHeight = mMap.Tilesets[0].TileHeight;

            mTilesetTilesWide = mTileset.Width / mTileWidth;

            foreach (var o in mMap.ObjectGroups["Collision"].Objects)
            {
                mCollisionObjects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));

            }

            // Console.WriteLine(GetMapObjects().Count);
        }

        public List<Rectangle> GetMapObjects()
        {
            return mCollisionObjects;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            spriteBatch.Begin();

            int layersCount = 0;
            while (layersCount < mLayers)
            {
                for (var i = 0; i < mMap.Layers[layersCount].Tiles.Count; i++)
                {
                    int gid = mMap.Layers[layersCount].Tiles[i].Gid;

                    // Empty tile, do nothing
                    if (gid == 0)
                    {

                    }
                    else
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % mTilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / mTilesetTilesWide);

                        float x = (i % mMap.Width) * mMap.TileWidth;
                        float y = (float)Math.Floor(i / (double)mMap.Width) * mMap.TileHeight;

                        Rectangle tilesetRec =
                            new Rectangle(mTileWidth * column, mTileHeight * row, mTileWidth, mTileHeight);

                        spriteBatch.Draw(mTileset,
                            new Rectangle((int)x, (int)y, mTileWidth, mTileHeight),
                            tilesetRec,
                            Color.White);
                    }
                }

                layersCount++;
            }

            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
