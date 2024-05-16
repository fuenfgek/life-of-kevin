using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Implements a lasergun.
    /// </summary>
    /// <author>Konstantin Fünfgelt </author>
    /// <inheritdoc cref="Item"/>
    [Serializable]
    public sealed class ItemTurretLasergun : Item
    {
        private Vector2 Offset { get; }
        [XmlElement]
        public const float Damage = 6;
        [XmlElement]
        public const int Range = 300;
        [XmlElement]
        public const int Lifetime = 1000;


        
        private ItemTurretLasergun() { }

        internal ItemTurretLasergun(OffsetType offsetType = OffsetType.Player)
            : base("turretLasergun", "items/icons/lasergun_icon", 500, 1000, true, 1)
        {
            switch (offsetType)
            {
                case OffsetType.Player:
                    Offset = new Vector2(6.5f, -35);
                    break;
                case OffsetType.Turret:
                    Offset = new Vector2(0, -25);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(offsetType), offsetType, null);
            }
        }

        protected override void UseItem(Entity entity)
        {
            var events = Events.Instance;
            events.Fire(new LaserBulletSpawnE(
                entity,
                Offset,
                "TurretLaserBullet",
                Damage,
                Range,
                Lifetime,
                entity.GetComponent<TransformC>().CurrentPosition));
            StopUsingItem();
            Reload();
        }
    }
}