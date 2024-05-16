using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class LaserTurretC : IComponent
    {
        [XmlElement]
        public bool IsLocked { get; set; }
        [XmlElement]
        public bool AttackCommand { get; set; }
        [XmlElement]
        public bool IsShooting { get; set; }
        [XmlElement]
        public int PassedTime { get; set; }
    }
}
