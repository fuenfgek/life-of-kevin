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
    /// <author>Nico Greiner </author>
    /// <inheritdoc cref="Item"/>
    [Serializable]
    public sealed class ItemRocketlauncher2 : Item
    {
        private const int Damage = 11;

        [XmlElement]
        public Vector2 Offset { get; set; }

        private ItemRocketlauncher2()
            : this(OffsetType.Player)
        {

        }

        internal ItemRocketlauncher2(OffsetType offsetType = OffsetType.Player)
            : base("rocketlauncher", "items/icons/rocketlauncher2_icon", 0, 1800, true, 2)
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
