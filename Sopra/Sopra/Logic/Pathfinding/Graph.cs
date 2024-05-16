﻿using System;
using System.Collections.Generic;
using System.Linq;
 using Microsoft.Xna.Framework;

namespace Sopra.Logic.Pathfinding
{
    /// <summary>
    /// A graph storing lists of nodes and edges.
    /// Author: Nhat Minh Hoang
    /// </summary>
    public class Graph
    {
        internal Node[] Nodes { get; }
        //private readonly int mTileWidth;
        //private readonly int mTileHeight;

        //private readonly Dictionary<int, int> mEntityPosition = new Dictionary<int, int>();

        private readonly int[] mScoreCache;
        internal readonly int[] mClosedSet;
        internal readonly int[] mCameFrom;
        internal readonly int[] mFScore;
        internal readonly int[] mGScore;
        internal readonly int[] mClosedSetReset;
        internal readonly int mNodesByteSize;

        protected Graph(Node[] nodes)
        {
            Nodes = nodes;
            //mTileWidth = tileWidth;
            //mTileHeight = tileHeight;
            mScoreCache = Enumerable.Repeat(int.MaxValue, nodes.Length).ToArray();
            mClosedSet = new int[nodes.Length];
            mCameFrom = new int[nodes.Length];
            mFScore = new int[nodes.Length];
            mGScore = new int[nodes.Length];
            mClosedSetReset = new int[nodes.Length];
            mNodesByteSize = nodes.Length * sizeof(int);
        }

        /// <summary>
        /// Calculates the euclidian distance between a node and the targetId node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        internal static int Euclidian(Node node, Node goal)
        {
            return Euclidian(node.Position, goal.Position);
        }

        internal static int Euclidian(Vector2 currentPosition, Vector2 targetPosition)
        {
            return (int) (currentPosition - targetPosition).Length();
        }

        /// <summary>
        /// Creates an empty array.
        /// </summary>
        /// <returns></returns>
        internal void PopulateScore(int[] arr)
        {
            Buffer.BlockCopy(mScoreCache, 0, arr, 0, mNodesByteSize);
            //Array.Copy(mScoreCache, arr, mScoreCache.Length);
        }

        /*/// <summary>
        /// Check if the node is an horizontal or vertical neighbour.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="neighbourId"></param>
        /// <returns></returns>
        private bool CheckNeighbourHorizontalOrVertical(int nodeId, int neighbourId)
        {
            return Math.Abs(nodeId - neighbourId) == 1 || Math.Abs(nodeId - neighbourId) == Width;
        }*/

        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="neighbour"></param>
        /// <returns></returns>
        private bool CheckDirectPathToNeighbour(int current, int neighbour)
        {
            if (CheckNeighbourHorizontalOrVertical(current, neighbour))
            {
                return true;
            }

            var currentNode = Nodes[current];
            return (currentNode.Left == -1 || Nodes[currentNode.Left].Walkable) 
                   && (currentNode.Upper == -1 || Nodes[currentNode.Upper].Walkable) 
                   && (currentNode.Right == -1 || Nodes[currentNode.Right].Walkable) 
                   && (currentNode.Lower == -1 || Nodes[currentNode.Lower].Walkable);
        }*/

        /*/// <summary>
        /// Calculates the shortest path of a startID node and targetId node.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="startId"></param>
        /// <param name="targetId"></param>
        /// <returns>A list of nodes along the shortest path.</returns>
        internal List<int> ShortestPath(Entity entity, int startId, int targetId)
        {
            Buffer.BlockCopy(mClosedSetReset, 0, mClosedSet, 0, mNodesByteSize);
            var openSet = new StablePriorityQueue<QueueNode>(Nodes.Length);
            PopulateScore(mCameFrom);

            // The actual cost from the start node current node
            PopulateScore(mGScore);
            mGScore[startId] = 0;

            // Actual cost from start node to current node + euclidian distance from current node to goal node
            PopulateScore(mFScore);
            mFScore[startId] = Euclidian(Nodes[startId], Nodes[targetId]);

            openSet.Enqueue(new QueueNode(startId), mFScore[startId]);
            
            var hasDrone = entity.HasComponent<DroneC>();

            while (openSet.Count != 0)
            {
                var current = openSet.Dequeue();
                if (current.Id.Equals(targetId))
                {
                    return ReconstructPath(mCameFrom, targetId);
                }

                mClosedSet[current.Id] = 1;

                // Search through all neighbours of the current node.
                foreach (var node in Nodes[current.Id].Neighbours)
                {
                    var tentativeGScore = mGScore[current.Id] + 64;

                    if (!Nodes[node].Walkable && !hasDrone
                        || !Nodes[node].Flyable && hasDrone
                        || mClosedSet[node] == 1 || tentativeGScore >= mGScore[node]
                        || !CheckDirectPathToNeighbour(current.Id, node))
                    {
                        continue;
                    }

                    var tentativeFScore = tentativeGScore + Euclidian(Nodes[node], Nodes[targetId]);
                    openSet.Enqueue(new QueueNode(node), tentativeFScore);


                    mCameFrom[node] = current.Id;
                    mGScore[node] = tentativeGScore;
                    mFScore[node] = tentativeFScore;
                }
            }

            return new List<int>();
        }*/

        /// <summary>
        /// Reconstructs the shortest path from the given node to the start node.
        /// </summary>
        /// <param name="cameFrom">A dictionary which holds information about a node's predecessor.</param>
        /// <param name="currentNodeId">The end node of the path.</param>
        /// <returns>A path of nodes.</returns>
        internal static List<int> ReconstructPath(int[] cameFrom, int currentNodeId)
        {
            var path = new List<int>();
            var current = currentNodeId;
            path.Add(current);
            while (!cameFrom[current].Equals(int.MaxValue))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            return path;
        }

        /*/// <summary>
        /// Return the tile id of the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal int GetNodeId(Vector2 position)
        {
            return (int) (position.X / mTileWidth) + (int) (position.Y / mTileHeight) * Width;
        }*/

        /*/// <summary>
        /// Mark tiles that are occupied by an entity as unwalkable and mark them as walkable again
        /// if the occupying entity moved to another tile.
        /// </summary>
        internal void UpdateGraph()
        {
            var inaccessibleEntities = mStaticEntitySubscription.GetEntites().ToArray();
            
            foreach (var entity in inaccessibleEntities)
            {
                var tile = GetNodeId(entity.GetComponent<TransformC>().CurrentPosition);
                if (!mEntityPosition.ContainsKey(entity.Id))
                {
                    mEntityPosition.Add(entity.Id, tile);
                    Nodes[tile].Walkable = false;
                    Nodes[tile].Flyable = !entity.HasComponent<CollisionImpenetrableC>();
                }
                else
                {
                    Nodes[mEntityPosition[entity.Id]].Walkable = true;
                    Nodes[tile].Walkable = false;
                    Nodes[tile].Flyable = !entity.HasComponent<CollisionImpenetrableC>();
                    mEntityPosition[entity.Id] = tile;
                }
            }

            var inaccessibleEntitiesId = new int[inaccessibleEntities.Length];
            for (var i = 0; i < inaccessibleEntities.Length; i++)
            {
                inaccessibleEntitiesId[i] = inaccessibleEntities.ElementAt(i).Id;
            }

            var removedEntities = mEntityPosition.Keys.Except(inaccessibleEntitiesId).ToArray();
            foreach (var id in removedEntities)
            {
                Nodes[mEntityPosition[id]].Walkable = true;
                Nodes[mEntityPosition[id]].Flyable = true;
                mEntityPosition.Remove(id);
            }
        }*/
    }
}