using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.KI;

namespace Sopra.Logic.Pathfinding
{
    /// <summary>
    /// Component for Pathfinding.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    [Serializable]
    public sealed class PathFindingC : IComponent
    {
        
        public static ComponentType Type { get; } = ComponentType.Of <PathFindingC>();
        
        [XmlElement]
        public List<Vector2> CurrentPath { get; set; }
        [XmlElement]
        public List<Vector2> PatrolPath { get; set; }
        [XmlElement]
        public int PathStep { get; set; }
        [XmlElement]
        public Vector2 TargetPosition { get; set; }
        [XmlElement]
        public int TargetId { get; set; }
        [XmlElement]
        public float MovementSpeed { get; set; }
        [XmlElement]
        public bool HasNewCommand { get; set; }
        [XmlElement]
        public CollisionTemplate CollisionNumber { get; set; }

        [XmlElement]
        public int PassedTime { get; set; } = new Random().Next(EnemyPathFindingSystem.TimeBetweenCalcs);

        public PathFindingC()
        {
            MovementSpeed = 3;
            CollisionNumber = CollisionTemplate.Inaccsessible;
        }

        public PathFindingC(CollisionTemplate collisionTemplate = CollisionTemplate.Inaccsessible, float movementSpeed = 3)
        {
            MovementSpeed = movementSpeed;
            CollisionNumber = collisionTemplate;
        }

        /// <summary>
        /// Set a new target position for the path finding system.
        /// </summary>
        /// <param name="newTarget"></param>
        internal void SetNewTarget(Vector2 newTarget)
        {
            HasNewCommand = true;
            TargetPosition = newTarget;
        }
    }
}