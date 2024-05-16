using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Pathfinding.NavMesh;
using TiledSharp;

namespace Sopra.Logic.Pathfinding
{
    /// <summary>
    /// The interface for getting the shortest path in the map.
    /// Author: Nhat Minh Hoang
    /// </summary>
    internal sealed class PathFinder
    {
        private NavMeshGraph Graph { get; }

        //private readonly Template mInaccessibleTemplate = Template.All(typeof(CollisionInaccessibleC)).Build();
        private readonly Template mStaticTemplate = Template.All(typeof(StaticObjectC), typeof(HitboxC)).Build();

        private readonly Subscription mStaticMapObjects;
        private readonly QuadTree mQuadTree;
        private readonly Collisions mCollisions;

        public PathFinder(TmxMap map, Engine engine)
        {            
            Graph = NavMeshCreator.CreateGraph(map);
            mStaticMapObjects = new Subscription(engine, mStaticTemplate);
            mQuadTree = new QuadTree(0, new Rectangle(0, 0, map.Width * map.TileWidth, map.Height * map.TileHeight));
            mCollisions = new Collisions(mQuadTree);
        }

        internal void Update()
        {
            mQuadTree.Clear();
            foreach (var ent in mStaticMapObjects.GetEntites())
            {
                mQuadTree.Insert(new QuadObject(ent));
            }
        }

        /// <summary>
        /// Smooth the path by removing unnecessary waypoints.
        /// </summary>
        /// <param name="path">The list of waypoint vectors.</param>
        /// <returns></returns>
        private List<Vector2> SmoothPath(List<Vector2> path)
        {
            var newPath = new List<Vector2>();
            if (path.Count <= 2)
            {
                return path;
            }

            var pointer = 0;
            var secondPointer = pointer + 2;
            while (secondPointer < path.Count)
            {
                #region Calculate lines and check intersection
                
                // Check whether the two parallel lines of the beeline intersect with any rectangle.
                var beeline = path[secondPointer] - path[pointer];
                beeline.Normalize();

                const int size = 21;

                var leftPerpVector = Collisions.OrthogonalVector(beeline, true);
                var rightPerpVector = Collisions.OrthogonalVector(beeline, false);

                var leftLineStart = path[pointer] + leftPerpVector * size;
                var leftLineEnd = path[secondPointer] + leftPerpVector * size;

                var rightLineStart = path[pointer] + rightPerpVector * size;
                var rightLineEnd = path[secondPointer] + rightPerpVector * size;
                
                #endregion

                if (mCollisions.GetCollidingEntities(leftLineStart, leftLineEnd).Any()
                    || mCollisions.GetCollidingEntities(rightLineStart, rightLineEnd).Any())
                {
                    if (!newPath.Contains(path[pointer]))
                    {
                        newPath.Add(path[pointer]);
                    }
                    newPath.Add(path[secondPointer - 1]);
                    pointer = secondPointer - 1 != pointer ? secondPointer - 1 : pointer + 1;
                    secondPointer = pointer + 2;
                }

                else
                {
                    secondPointer++;
                }
            }

            if (!newPath.Contains(path[pointer]))
            {
                newPath.Add(path[pointer]);
            }

            newPath.Add(path[path.Count - 1]);

            return newPath;
        }

        /// <summary>
        /// Calculate a path from the current position to the target position.
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        internal List<Vector2> GetPath(Vector2 currentPosition, Vector2 targetPosition)
        {
            var currentMesh = Graph.GetMesh(currentPosition);
            var targetMesh = Graph.GetMesh(targetPosition);

            if (currentMesh.Equals(targetMesh))
            {
                return new List<Vector2> {targetPosition};
            }

            if (targetMesh.Id == -1)
            {
                // If the player clicks a static map objects.
                if (mCollisions.GetCollidingEntities(new HitboxC(5, 5), targetPosition, mStaticTemplate).Any())
                {
                    return new List<Vector2>();
                }

                // Ensure that the player is able to move when he clicks a point lying in the offset.
                targetPosition = Graph.GetNearestPosition(targetPosition);
                targetMesh = Graph.GetMesh(targetPosition);

                // If the player clicks outside the map or inside an enclosed room.
                if (targetMesh.Id == -1)
                {
                    return new List<Vector2>();
                }
            }

            var nearestNodeToTarget = Graph.GetNearestNode(targetMesh, currentPosition);

            var nearestNodeToCurrent = currentMesh.Id == -1
                ? Graph.GetNearestNode(currentPosition)
                : Graph.GetNearestNode(currentMesh, targetPosition);

            var shortestPath = Graph.ShortestPath(nearestNodeToCurrent, nearestNodeToTarget);

            if (shortestPath.Count == 0)
            {
                return new List<Vector2>();
            }

            var path = new List<Vector2>
            {
                targetPosition
            };

            path.AddRange(from p in shortestPath select Graph.Nodes[p].Position);

            path.Add(currentPosition);
            path.Reverse();
            path = SmoothPath(path);
            return path;
        }

        internal float GetPathLength(Vector2 entityPos, Vector2 emitterPosition)
        {
            var pathList = GetPath(entityPos, emitterPosition);

            pathList.Insert(0, entityPos);
            var path = pathList.ToArray();

            var length = 0f;
            for (var i = 0; i < path.Length - 1; i++)
            {
                length += (path[i + 1] - path[i]).Length();
            }

            return length;
        }
    }
}