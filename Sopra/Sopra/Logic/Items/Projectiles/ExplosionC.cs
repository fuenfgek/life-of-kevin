using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items.Projectiles
{
    /// <summary>
    /// Stores all data regarding a explosion
    /// </summary>
    /// <author>Konstantin Fünfgelt</author>
    /// <inheritdoc cref="IComponent"/>
    [Serializable]
    public sealed class ExplosionC : IComponent
    {
        [XmlElement]
        public int Damage { get; set; }
        [XmlElement]
        public int GunOwnerId { get; set; }
        [XmlElement]
        public bool DealDamage { get; set; }
        [XmlElement]
        public bool IsCarExplosion { get; set; }
        [XmlElement]
        public Vector2 ShotFiredPos { get; set; }

        public ExplosionC()
        {
            DealDamage = true;
        }
    }
}
