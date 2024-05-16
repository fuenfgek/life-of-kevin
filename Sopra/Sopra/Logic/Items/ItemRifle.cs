using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.KI;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Class for Rifle Action
    /// </summary>
    /// <author>Konstantin Fünfgelt </author>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="Item"/>
    [Serializable]
    public sealed class ItemRifle : Item
    {
        private const int MagazineSize = 15;
        private const int Damage = 3;

        [XmlElement]
        public int MagazineCount { get; set; }
        [XmlElement]
        public Vector2 Offset { get; set; }



        private ItemRifle()
            : this(OffsetType.Player)
        {
        }
        
        internal ItemRifle(OffsetType offsetType = OffsetType.Player)
            : base("rifle", "items/icons/rifle1_icon", 400, 2300, true,1)
        {
            MagazineCount = MagazineSize;

            switch (offsetType)
            {
                case OffsetType.Player:
                    Offset = new Vector2(5, -30);
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
            events.Fire(new SimpleBulletSpawnE(entity, Offset, "RiflelBullet",Damage, playerPos));
            events.Fire(new NoiseEvent(entity.Id, playerPos, 50));
            SoundManager.Instance.PlaySound("rifle");

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
