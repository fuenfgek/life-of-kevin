using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Stores data for 
    /// </summary>
    /// <author>Konstantin FÜnfgelt</author>
    internal sealed class LaserBulletSpawnE : IGameEvent
    {
        internal Entity UserEntity { get; }
        internal Vector2 Offset { get; }
        internal string BulletName { get; }
        internal float Damage { get; }
        internal int Range { get; }
        internal int Lifetime { get; }
        internal Vector2 ShotFiredPos { get; }

        public LaserBulletSpawnE(Entity userEntity, Vector2 offset, string bulletName,
            float damage, int range, int lifetime, Vector2 shotFiredPos)
        {
            UserEntity = userEntity;
            Offset = offset;
            BulletName = bulletName;
            Damage = damage;
            Range = range;
            Lifetime = lifetime;
            ShotFiredPos = shotFiredPos;
        }
    }
}
