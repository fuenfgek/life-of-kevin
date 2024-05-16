using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.KI;

namespace Sopra.Logic.Items
{
    [Serializable]
    public sealed class ItemMinigun2 : Item
    {
        private const int MagazineSize = 30;
        private const int Damage = 2;

        [XmlElement]
        public int MagazineCount { get; set; }
        [XmlElement]
        public Vector2 Offset { get; set; }

        private ItemMinigun2()
            : this(OffsetType.Player)
        {
        }

        internal ItemMinigun2(OffsetType offsetType = OffsetType.Player)
            : base("minigun", "items/icons/minigun2_icon", 200, 10000, true,2)
        {
            MagazineCount = MagazineSize;

            switch (offsetType)
            {
                case OffsetType.Player:
                    Offset = new Vector2(5, -28);
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
            events.Fire(new SimpleBulletSpawnE(entity, Offset, "MinigunBullet", Damage, playerPos));
            events.Fire(new NoiseEvent(entity.Id, playerPos, 50));
            SoundManager.Instance.PlaySound("minigun");
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