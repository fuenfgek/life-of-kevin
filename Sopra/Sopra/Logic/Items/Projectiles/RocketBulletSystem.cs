using System;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Health;
using Sopra.Logic.KI;
using Sopra.Logic.Render;

namespace Sopra.Logic.Items.Projectiles
{
    /// <summary>
    /// Computes all existing Rocket bullets.
    /// Requires:
    ///     - TransformC
    ///     - HitboxC
    ///     - RocketBulletC
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Konstantin Fünfgelt</author>
    internal sealed class RocketBulletSysstem : IteratingEntitySystem
    {
        private readonly Template mTemplate = Template
            .One(typeof(CollisionImpenetrableC), typeof(HealthC))
            .Build();

        public RocketBulletSysstem()
            : base(Template
                .All(
                    typeof(TransformC),
                    typeof(HitboxC),
                    typeof(RocketBulletC)))
        {
            Events.Instance.Subscribe<RocketSpawnE>(CreateBullet);
        }


        private void CreateBullet(RocketSpawnE e)
        {
            var userEntity = e.UserEntity;
            var transformC = userEntity.GetComponent<TransformC>();

            var bullet = mEngine.EntityFactory.Create(e.Bulletname);
            var bulletC = bullet.GetComponent<RocketBulletC>(RocketBulletC.Type);
            bulletC.GunOwnerId = userEntity.Id;
            bulletC.ShotFiredPos = e.ShotFiredPos;
            bullet.GetComponent<TransformC>().CurrentPosition =
                transformC.CurrentPosition + Rotate(e.Offset, transformC.RotationRadians);
            bullet.GetComponent<TransformC>().SetRotation(transformC.RotationVector);
            bullet.GetComponent<RocketBulletC>().Damage = e.Damage;
            mEngine.EntityManager.Add(bullet);
        }

        private static Vector2 Rotate(Vector2 v, double degrees)
        {
            return new Vector2(
                (float) (v.X * Math.Cos(degrees) - v.Y * Math.Sin(degrees)),
                (float) (v.X * Math.Sin(degrees) + v.Y * Math.Cos(degrees))
            );
        }


        /// <summary>
        /// Run the system.
        /// This method will be called automatically by the engine during the update phase.
        /// </summary>
        /// <inheritdoc cref="IComponent"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
            var bulletC = entity.GetComponent<RocketBulletC>();
            var transC = entity.GetComponent<TransformC>();

            bulletC.PassedTime += time.ElapsedGameTime.Milliseconds;
            if (bulletC.PassedTime > bulletC.LifetimeMs)
            {
                SpawnExplosion(transC.CurrentPosition, bulletC);
                mEngine.EntityManager.Remove(entity.Id);
            }
            else
            {
                var hitboxC = entity.GetComponent<HitboxC>();

                var newPosition = transC.CurrentPosition + transC.RotationVector * bulletC.Speed;

                var ignoredIds = new[] {entity.Id, bulletC.GunOwnerId};

                var collidingEntities =
                    mEngine.Collision.GetCollidingEntities(hitboxC, newPosition, mTemplate, ignoredIds);

                if (!collidingEntities.Any())
                {
                    transC.CurrentPosition = newPosition;
                }

                var bulletOwner = mEngine.EntityManager.Get(bulletC.GunOwnerId);
                if (collidingEntities.IsNullOrEmpty() 
                    || bulletOwner != null 
                    && bulletOwner.HasComponent<Enemy>() 
                    && collidingEntities.First().HasComponent<Enemy>())
                {
                    transC.CurrentPosition = newPosition;
                    return;
                }


                SpawnExplosion(newPosition, bulletC);
                mEngine.EntityManager.Remove(entity.Id);
            }
        }

        private void SpawnExplosion(Vector2 newPosition, RocketBulletC bulletC)
        {
            var explosion = mEngine.EntityFactory.Create("Explosion");

            var explosionC = explosion.GetComponent<ExplosionC>();
            explosionC.Damage = bulletC.Damage;
            explosionC.ShotFiredPos = bulletC.ShotFiredPos;
            explosionC.GunOwnerId = bulletC.GunOwnerId;

            explosion.GetComponent<TransformC>().CurrentPosition = newPosition;
            explosion.GetComponent<AnimationC>().SelfDestruct = true;
            mEngine.EntityManager.Add(explosion);
        }
    }
}