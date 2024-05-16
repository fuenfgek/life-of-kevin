using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Priority_Queue;

namespace Sopra.Logic.Pathfinding.NavMesh
{
    internal sealed class NavMeshGraph : Graph
    {
        private Mesh[] Meshes { get; }

        internal NavMeshGraph(Node[] nodes, Mesh[] meshes)
            : base(nodes)
        {
            Meshes = meshes;
        }

        /// <summary>
        /// Return the desired mesh containing the point.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal Mesh GetMesh(Vector2 position)
        {
            foreach (var mesh in Meshes)
            {
                if (!mesh.ContainsPoint(position))
                {
                    continue;
                }

                return mesh;
            }

            return new Mesh(-Vector2.One, Vector2.One, new int[0], -1);
        }

        /// <summary>
        /// Return the point of a mesh which is lined with the position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal Vector2 GetNearestPosition(Vector2 position)
        {
            foreach (var mesh in Meshes)
            {
                if (mesh.ContainsPoint(position + new Vector2(22, 0)))
                {
                    return position + new Vector2(22, 0);
                }

                if (mesh.ContainsPoint(position + new Vector2(-22, 0)))
                {
                    return position + new Vector2(-22, 0);
                }

                if (mesh.ContainsPoint(position + new Vector2(0, 22)))
                {
                    return position + new Vector2(0, 22);
                }

                if (mesh.ContainsPoint(position + new Vector2(0, -22)))
                {
                    return position + new Vector2(0, -22);
                }

                if (mesh.ContainsPoint(position + new Vector2(32, 32)))
                {
                    return position + new Vector2(32, 32);
                }

                if (mesh.ContainsPoint(position + new Vector2(-32, 32)))
                {
                    return position + new Vector2(-32, 32);
                }

                if (mesh.ContainsPoint(position + new Vector2(32, -32)))
                {
                    return position + new Vector2(32, -32);
                }
            }

            return position + new Vector2(-32, -32);
        }

        /// <summary>
        /// Return the node in a given mesh which is nearest to the position.
        /// The position should be in the given mesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        internal int GetNearestNode(Mesh mesh, Vector2 position)
        {
            var nearestNode = mesh.Corners[0];

            var corners = new List<int>();
            corners.AddRange(mesh.Corners);
            corners.AddRange(mesh.CornersLyingOnEdge);

            foreach (var corner in corners)
            {
                if (Euclidian(Nodes[nearestNode].Position, position) > Euclidian(Nodes[corner].Position, position))
                {
                    nearestNode = corner;
                }
            }

            return nearestNode;
        }

        /// <summary>
        /// Return the node of all meshes which is nearest to the position.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal int GetNearestNode(Vector2 target)
        {
            var nearestNode = Nodes[0];
            foreach (var node in Nodes)
            {
                if (Euclidian(nearestNode.Position, target) > Euclidian(node.Position, target))
                {
                    nearestNode = node;
                }
            }

            return nearestNode.Id;
        }

        /// <summary>
        /// Calculates the shortest path of a startID node and targetId node.
        /// The algorithm is based on the A* search algorithm.
        /// </summary>
        /// <param name="startId"></param>
        /// <param name="targetId"></param>
        /// <returns>A list of nodes along the shortest path.</returns>
        internal List<int> ShortestPath(int startId, int targetId)
        {
            Buffer.BlockCopy(mClosedSetReset, 0, mClosedSet, 0, mNodesByteSize);
            var openSet = new StablePriorityQueue<QueueNode>(short.MaxValue - 1);
            PopulateScore(mCameFrom);

            // The actual cost from the start node current node
            PopulateScore(mGScore);
            mGScore[startId] = 0;

            // Actual cost from start node to current node + euclidian distance from current node to goal node
            PopulateScore(mFScore);
            mFScore[startId] = Euclidian(Nodes[startId], Nodes[targetId]);

            openSet.Enqueue(new QueueNode(startId), mFScore[startId]);

            while (openSet.Count != 0)
            {
                var current = openSet.Dequeue();
                if (current.Id.Equals(targetId))
                {
                    return ReconstructPath(mCameFrom, targetId);
                }

                mClosedSet[current.Id] = 1;

                // Search through all neighbours of the current node
                foreach (var node in Nodes[current.Id].Neighbours)
                {
                    var tentativeGScore = mGScore[current.Id] + Euclidian(Nodes[current.Id], Nodes[node]);

                    if (mClosedSet[node] == 1 || tentativeGScore >= mGScore[node])
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

            // When no path is found
            return new List<int>();
        }
    }
}