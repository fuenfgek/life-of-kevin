using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.KI
{
    /// <summary>
    /// Marks an enemy as patrolling and holds all necessary informations for that.
    /// </summary>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class PatrollingEnemyC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of <PatrollingEnemyC>();

        [XmlElement]
        public List<Vector2> PatrolPath { get; set; }
        [XmlElement]
        public bool IsPatrolling { get; set; }
        [XmlElement]
        public int PatrolStep { get; set; }
        
        private PatrollingEnemyC() { }

        internal PatrollingEnemyC(List<Vector2> patrolPath)
        {
            PatrolPath = patrolPath;
        }
    }
}
