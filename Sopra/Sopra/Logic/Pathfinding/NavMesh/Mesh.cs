using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Sopra.Logic.Pathfinding.NavMesh
{
    /// <summary>
    /// Data structure marking the walkable area of the map.
    /// </summary>
    internal sealed class Mesh
    {
        internal int Id { get; }
        internal int[] Corners { get; }
        internal List<int> CornersLyingOnEdge { get; }
        private Vector2 Position { get; }
        private Vector2 Size { get; }

        internal Mesh(Vector2 position, Vector2 size, int[] corners, int id)
        {
            Position = position;
            Size = size;
            Corners = corners;
            CornersLyingOnEdge = new List<int>();
            Id = id;
        }

        internal bool ContainsPoint(Vector2 point)
        {
            return (int) point.X >= (int) Position.X && (int) point.Y >= (int) Position.Y &&
                   (int) point.X <= (int) (Position + Size).X && (int) point.Y <= (int) (Position + Size).Y;
        }

        internal bool Equals(Mesh anotherMesh)
        {
            return Id.Equals(anotherMesh.Id);
        }

        internal void AddCorner(int id)
        {
            if (!CornersLyingOnEdge.Contains(id) && !Corners.Contains(id))
            {
                CornersLyingOnEdge.Add(id);
            }
        }
    }
}