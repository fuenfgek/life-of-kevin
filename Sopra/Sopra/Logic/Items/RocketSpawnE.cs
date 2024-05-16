using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items
{   
    /// <summary>
    /// Stores data for RocketSpwan Event
    /// </summary>
    /// <author>Konstantin Fünfglet</author>
    public sealed class RocketSpawnE : IGameEvent
    {
        internal Entity UserEntity { get; }
        internal Vector2 Offset { get; }
        internal string Bulletname { get; }
        internal int Damage { get; }
        internal Vector2 ShotFiredPos { get; }

        public RocketSpawnE(Entity entity, Vector2 offset, string bulletname, int damage, Vector2 shotFiredPos)
        {
            UserEntity = entity;
            Damage = damage;
            Bulletname = bulletname;
            Offset = offset;
            ShotFiredPos = shotFiredPos;
        }

    }
}
