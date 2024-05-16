using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.KI;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Class for Rocket Launcher Action
    /// </summary>
    /// <author>Konstantin FÜnfgelt </author>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="Item"/>
    [Serializable]
    public sealed class ItemRocketlauncher : Item
    {
        private const int Damage = 6;

        [XmlElement]
        public Vector2 Offset { get; set; }

        private ItemRocketlauncher()
            : this(OffsetType.Player)
        {
            
        }

        internal ItemRocketlauncher(OffsetType offsetType = OffsetType.Player)
            : base("rocketlauncher", "items/icons/rocketlauncher1_icon", 0, 1800, true, 1)
        {
            switch (offsetType)
            {
                case OffsetType.Player:
                    Offset = new Vector2(5, -30);
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
            var playerPos = entity.GetComponent<TransformC>().CurrentPosition;
            events.Fire(new RocketSpawnE(entity, Offset, "RocketBullet", Damage, playerPos));
            events.Fire(new NoiseEvent(entity.Id, playerPos, 100));
            SoundManager.Instance.PlaySound("rocket");
            Reload();
        }
    }
}
