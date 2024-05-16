using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items.Projectiles
{
    /// <summary>
    /// Stores all data regarding a Rocketbullet.
    /// </summary>
    /// <author>Konstantin Fünfgelt</author>
    /// <inheritdoc cref="IComponent"/>
    [Serializable]
    public sealed class RocketBulletC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of<RocketBulletC>();

        [XmlElement]
        public int LifetimeMs { get; set; }
        [XmlElement]
        public float Speed { get; set; }
        [XmlElement]
        public int Damage { get; set; }
        [XmlElement]
        public int GunOwnerId { get; set; }
        [XmlElement]
        public double PassedTime { get; set; }
        [XmlElement]
        public Vector2 ShotFiredPos { get; set; }

        public RocketBulletC() { }

        public RocketBulletC(float speed, int lifetimeMs)
        {
            Speed = speed;
            LifetimeMs = lifetimeMs;
        }
    }
}