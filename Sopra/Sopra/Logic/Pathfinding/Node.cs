using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sopra.Logic.Pathfinding
{
    /// <summary>
    /// Node that stores an ID and a position.
    /// Author: Nhat Minh Hoang
    /// </summary>
    public sealed class Node
    {
        internal int Id { get; }
        internal Vector2 Position { get; }
        internal List<int> Neighbours { get; }

        internal Node(int id, Vector2 position)
        {
            Id = id;
            Position = position;
            Neighbours = new List<int>();
        }

        private void AddNeighbour(int neighbour)
        {
            if (!Neighbours.Contains(neighbour) && neighbour != Id)
            {
                Neighbours.Add(neighbour);
            }
        }

        internal void AddNeighbour(IEnumerable<int> neighbour)
        {
            foreach (var id in neighbour)
            {
                AddNeighbour(id);
            }
        }
    }
}