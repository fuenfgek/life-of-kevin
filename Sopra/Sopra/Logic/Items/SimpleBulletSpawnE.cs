using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items
{
    /// <summary>
    ///Stores data for SimpleBulletSpawn Event
    /// </summary>
    public sealed class SimpleBulletSpawnE : IGameEvent
    {
        internal Entity UserEntity { get; }
        internal Vector2 Offset { get; }
        internal string BulletName { get; }
        internal int Damage { get; }
        internal Vector2 ShotFiredPos { get; }


        public SimpleBulletSpawnE(Entity entity, Vector2 offset, string name,int damage, Vector2 shotFiredPos)
        {
            UserEntity = entity;
            Damage = damage;
            Offset = offset;
            BulletName = name;
            ShotFiredPos = shotFiredPos;
        }
    }
}
