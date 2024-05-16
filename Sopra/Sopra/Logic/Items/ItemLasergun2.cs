using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.KI;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Implements a lasergun.
    /// </summary>
    /// <author>Nico Greiner</author>
    /// <inheritdoc cref="Item"/>
    [Serializable]
    public sealed class ItemLasergun2 : Item
    {
        private const int MagazineSize = 1;
        private const int Damage = 10;
        private const int Range = 300;
        private const int Lifetime = 50;

        [XmlElement]
        public int MagazineCount { get; set; }
        [XmlElement]
        public Vector2 Offset { get; set; }

        private ItemLasergun2() { }

        internal ItemLasergun2(OffsetType offsetType = OffsetType.Player)
            : base("lasergun", "items/icons/lasergun2_icon", 500, 2000, true, 2)
        {
            MagazineCount = MagazineSize;

            switch (offsetType)
            {
                case OffsetType.Player:
                    Offset = new Vector2(6.5f, -35);
                    break;
                case OffsetType.Turret:
                    Offset = new Vector2(0, -20);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(offsetType), offsetType, null);
            }
        }

        protected override void UseItem(Entity entity)
        {
            var events = Events.Instance;
            var playerPos = entity.GetComponent<TransformC>().CurrentPosition;
            events.Fire(new LaserBulletSpawnE(entity, Offset, "LaserBullet", Damage, Range, Lifetime, playerPos));
            events.Fire(new NoiseEvent(entity.Id, playerPos, 50));
            SoundManager.Instance.PlaySound("laser");

            MagazineCount -= 1;
            if (MagazineCount > 0)
            {
                return;
            }
            MagazineCount = MagazineSize;
            Reload();
        }
    }
}