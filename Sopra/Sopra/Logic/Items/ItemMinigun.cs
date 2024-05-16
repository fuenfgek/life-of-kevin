using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.KI;

namespace Sopra.Logic.Items
{   
    /// <summary>
    /// Class for minigun action
    /// </summary>
    /// <author>Konstantin Fünfgelt</author>
    /// <inheritdoc cref="Item"/>
    [Serializable]
    public sealed class ItemMinigun : Item
    {
        private const int MagazineSize = 30;
        private const int Damage = 1;

        [XmlElement]
        public int MagazineCount { get; set; }
        [XmlElement]
        public Vector2 Offset { get; set; }

        private ItemMinigun()
            : this(OffsetType.Player)
        {
        }

        internal ItemMinigun(OffsetType offsetType = OffsetType.Player)
            : base("minigun", "items/icons/minigun1_icon", 100, 4000, true, 1)
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