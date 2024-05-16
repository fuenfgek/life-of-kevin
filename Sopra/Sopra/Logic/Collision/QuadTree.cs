using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Logic.Collision
{
    /// <summary>
    /// This class is for subdividing the Map in Quads
    /// and settle the GameObjects
    /// In addition to optimize CollisionDetection.
    /// <author>Konstantin Fünfgelt</author>
    /// </summary>
    public sealed class QuadTree
    {
        private int mMaxObject = 5;

        private int mMaxLevel = 5;

        private readonly int mLevel;

        private Rectangle mBounds;

        private readonly QuadTree[] mNodes;

        // Objects in this tree
        private readonly List<QuadObject> mObjects;

        // Constructor for QuadTree objects
        public QuadTree(int level, Rectangle mapBounds)
        {
            mLevel = level;
            mObjects = new List<QuadObject>();
            mBounds = mapBounds;
            mNodes = new QuadTree[4];
        }

        /// <summary>
        /// Clears the whole QuadTree
        /// </summary>
        public void Clear()
        {
            mObjects.Clear();
            for (var i = 0; i < mNodes.Length; i++)
            {
                if (mNodes[i] != null)
                {
                    mNodes[i].Clear();
                    mNodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Divide the Node in four Subnodes
        /// </summary>
        private void Split()
        {
            var subWidth = mBounds.Width / 2;
            var subHeight = mBounds.Height / 2;
            var x = mBounds.X;
            var y = mBounds.Y;

            mNodes[0] = new QuadTree(mLevel + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            mNodes[1] = new QuadTree(mLevel + 1, new Rectangle(x, y, subWidth, subHeight));
            mNodes[2] = new QuadTree(mLevel + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            mNodes[3] = new QuadTree(mLevel + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /// <summary>
        /// Determine wich Object belong to wich node.
        /// -1 stands for Object does not fit in one Node
        /// and is part of Parent Node
        /// </summary>
        /// <param name="rect">Hitbox of Object</param>
        /// <returns></returns>
        private int GetIndex(Rectangle rect)
        {
            var index = -1;
            var verticalMid = mBounds.X + mBounds.Width / 2;
            var horizontalMid = mBounds.Y + mBounds.Height / 2;
            // Object can completely fit within the top quadrants
            var topQuadrant = rect.Y < horizontalMid && rect.Y + rect.Height < horizontalMid;
            // Object can completely fit within the bottom quadrants
            var bottomQuadrant = rect.Y > horizontalMid;

            //Object can completely fit in left quadrants
            if (rect.X < verticalMid && rect.X + rect.Width < verticalMid)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            //Object can completely fit in right quadrants
            else if (rect.X > verticalMid)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }
        /// <summary>
        /// Insert an QuadObject into QuadTree
        /// if the node exceeds capacity it will
        /// split and add object to their corresponding nodes
        /// </summary>
        /// <param name="object"></param>
        public void Insert(QuadObject @object)
        {
            if (mNodes[0] != null)
            {
                var index = GetIndex(@object.Rec);

                if (index != -1)
                {
                    mNodes[index].Insert(@object);
                    return;
                }
            }

            mObjects.Add(@object);

            if (mObjects.Count <= mMaxObject || mLevel >= mMaxLevel)
            {
                return;
            }

            {
                if (mNodes[0] == null)
                {
                    Split();
                }

                var i = 0;
                while (i < mObjects.Count)
                {
                    var index = GetIndex(mObjects[i].Rec);
                    if (index != -1)
                    {
                        mNodes[index].Insert(mObjects[i]);
                        mObjects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        public IEnumerable<QuadObject> Retrieve(Rectangle rect)
        {
            var objectlist = new List<QuadObject>(32);
            Retrieve(objectlist, rect);
            return objectlist;
        }

        /// <summary>
        /// Returns a List of QuadObject wich possibly could
        /// collide with the insertet QuadObject.
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private void Retrieve(ICollection<QuadObject> objectList, Rectangle rect)
        {
            if (!mBounds.Intersects(rect))
            {
                return;
            }

            foreach (var obj in mObjects)
            {
                if (rect.Intersects(obj.Rec))
                {
                    objectList.Add(obj);
                }
            }

            if (mNodes[0] == null)
            {
                return;
            }

            mNodes[0].Retrieve(objectList, rect);
            mNodes[1].Retrieve(objectList, rect);
            mNodes[2].Retrieve(objectList, rect);
            mNodes[3].Retrieve(objectList, rect);

        }
        
        //Example Code for drawing the quads
        public void DrawObject(SpriteBatch spriteBatch, Texture2D texture)//Texture or something
        {
            foreach (var obj in mObjects)
            {
                spriteBatch.Draw(texture, new Rectangle(obj.Rec.X + 1, obj.Rec.Y + 1, obj.Rec.Width - 2, obj.Rec.Height - 2), Color.White);
            }

            if (mNodes[0] != null)
            {
                mNodes[0].DrawObject(spriteBatch, texture);
                mNodes[1].DrawObject(spriteBatch, texture);
                mNodes[2].DrawObject(spriteBatch, texture);
                mNodes[3].DrawObject(spriteBatch, texture);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (mNodes[0] != null)
            {
                mNodes[0].Draw(spriteBatch);
                mNodes[1].Draw(spriteBatch);
                mNodes[2].Draw(spriteBatch);
                mNodes[3].Draw(spriteBatch);
                return;
            }
            
            DebugHelpers.DrawRect(spriteBatch, mBounds, Color.Red);
        }

    }
}