using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items.Projectiles
{
    [Serializable]
    public sealed class LaserBulletC : IComponent
    {
        public float Damage { get; set; }
        [XmlElement]
        public int GunOwnerId { get; set; }
        [XmlElement]
        public int Range { get; set; }
        [XmlElement]
        public double PassedTime { get; set; }
        [XmlElement]
        public int LifetimeMs { get; set; }
        [XmlElement]
        public bool DealDamage { get; set; }
        [XmlElement]
        public Vector2 ShotFiredPos { get; set; }

        public LaserBulletC()
        {
            DealDamage = true;
        }

        public LaserBulletC(int damage, int range, int lifeTime)
        {
            Damage = damage;
            Range = range;
            LifetimeMs = lifeTime;
            DealDamage = true;
        }
    }
}

