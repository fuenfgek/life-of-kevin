using Microsoft.Xna.Framework;
using Sopra.Engine.ECS;

namespace Sopra.Engine.ECS.Components
{
    /// <summary>
    /// Component for Pathfinding.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    sealed class PathFindingC : IComponent
    {
        public Vector2 TargetPosition { get; set; }
        public Entity Target { get; set; }
        public float MovementSpeed { get; set; }
        public bool HasMovingCommand { get; set; }

        public PathFindingC(float movementSpeed = 10)
        {
            MovementSpeed = movementSpeed;
        }


        public PathFindingC(Vector2 targetPosition, float movementSpeed = 10)
        {
            MovementSpeed = movementSpeed;
            TargetPosition = targetPosition;
            HasMovingCommand = true;
        }
    }
}