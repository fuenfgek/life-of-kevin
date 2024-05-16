using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items.Projectiles
{
    /// <summary>
    /// Stores all data regarding a simple bullet.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class SimpleBulletC : IComponent
    {

        public static ComponentType Type { get; } = ComponentType.Of<SimpleBulletC>();

        [XmlElement]
        public int LifetimeMs { get; set; }
        [XmlElement]
        public float Speed { get; set; }
        [XmlElement]
        public float Damage { get; set; }
        [XmlElement]
        public int GunOwnerId { get; set; }
        [XmlElement]
        public double PassedTime { get; set; }
        [XmlElement]
        public Vector2 ShotFiredPos { get; set; }

        public SimpleBulletC() { }

        public SimpleBulletC(float speed,  int lifetimeMs)
        {
            Speed = speed;
            LifetimeMs = lifetimeMs;
        }
    }
}
