using System;
using System.Xml.Serialization;
using Sopra.ECS;
using Sopra.Logic.Collision;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class AutoTurretC : IComponent
    {
        [XmlElement]
        public int TargetId { get; set; }
        [XmlElement]
        public int Range { get; set; }
        [XmlElement]
        public bool Active { get; set; }
        [XmlElement]
        public HitboxC LineOfSight { get; set; }

        public AutoTurretC()
        {
            LineOfSight = new HitboxC(0, 0, 0, 0);
            Range = 300;
            Active = true;
        }
    }
}
