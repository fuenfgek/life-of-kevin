using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace Sopra.Logic.Pathfinding.NavMesh
{
    /// <summary>
    /// Interface for creating a navmesh.
    ///
    /// The naming convention for every layer as followed:
    /// "NavMesh": The layer containing the rectangular mesh objects.
    /// "NavMeshNodes": The layer containing all circular node objects lying on a mesh's corner.
    /// </summary>
    internal static class NavMeshCreator
    {
        /// <summary>
        /// Parse the meshes and nodes from the map. Then create edges for every node.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        internal static NavMeshGraph CreateGraph(TmxMap map)
        {
            var nodes = new List<Node>();
            var nodeCounter = 0;

            // Create Nodes
            foreach (var meshNode in map.ObjectGroups["NavMeshNodes"].Objects)
            {
                var nodePosition = new Vector2((float)meshNode.X, (float)meshNode.Y);
                nodes.Add(new Node(nodeCounter, nodePosition));

                nodeCounter++;
            }

            var nodeArray = nodes.ToArray();
            var meshes = new List<Mesh>();
            CreateMesh(nodeArray, meshes, map.ObjectGroups["NavMesh"].Objects);
            var meshArray = meshes.ToArray();
            CreateEdges(nodes, meshArray, map.ObjectGroups["NavMeshEdges"].Objects);

            return new NavMeshGraph(nodeArray, meshArray);
        }

        /// <summary>
        /// Every node in a mesh should be in neighbourhood with each other.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="meshes"></param>
        /// <param name="objectLayer"></param>
        private static void CreateMesh(IReadOnlyList<Node> nodes, ICollection<Mesh> meshes, IEnumerable<TmxObject> objectLayer)
        {
            var meshCounter = 0;
            foreach (var mesh in objectLayer)
            {
                var position = new Vector2((float)mesh.X, (float)mesh.Y);
                var size = new Vector2((float)mesh.Width, (float)mesh.Height);

                var cornerPositions = new[]
                {
                    position,
                    position + new Vector2(size.X, 0),
                    position + size,
                    position + new Vector2(0, size.Y)
                };

                var cornerId = (
                    from corner in cornerPositions
                    from node in nodes
                    where CircleContains(corner, node.Position)
                    select node.Id).ToArray();

                var newMesh = new Mesh(position, size, cornerId, meshCounter);
                meshCounter++;
                meshes.Add(newMesh);
            }
        }

        /// <summary>
        /// Every node lying on a segment between two nodes should also be connected with every other node the mesh contains.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="meshes"></param>
        /// <param name="objectLayer"></param>
        private static void CreateEdges(IReadOnlyList<Node> nodes, Mesh[] meshes, IEnumerable<TmxObject> objectLayer)
        {
            foreach (var node in nodes)
            {
                foreach (var mesh in meshes)
                {
                    if (!mesh.ContainsPoint(node.Position))
                    {
                        continue;
                    }

                    mesh.AddCorner(node.Id);
                }
            }

            foreach (var obj in objectLayer)
            {
                var start = new Vector2((float) obj.X, (float) obj.Y);
                var end = new Vector2((float) (obj.X + obj.Width), (float) (obj.Y + obj.Height));
                var neighbours =
                    (from node in nodes
                        where CircleContains(node.Position, start) || CircleContains(node.Position, end)
                        select node.Id).ToList();

                nodes[neighbours[0]].AddNeighbour(neighbours);
                nodes[neighbours[1]].AddNeighbour(neighbours);
            }
        }

        /// <summary>
        /// Check whether a circle with radius 5 contains a point.
        /// </summary>
        /// <param name="circlePosition"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static bool CircleContains(Vector2 circlePosition, Vector2 point)
        {
            return (circlePosition - point).Length() < 5f;
        }
    }
}