using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Health;
using Sopra.Logic.KI;
using Sopra.Logic.Render;


namespace Sopra.Logic.Items.Projectiles
{
    /// <summary>
    /// Computes all existing simple bullets.
    /// Requires:
    ///     - TransformC
    ///     - HitboxC
    ///     - SimpleBulletC
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    internal sealed class SimpleBulletSystem : IteratingEntitySystem
    {


        private readonly Template mTemplate = Template
            .One(
                typeof(CollisionImpenetrableC),
                typeof(HealthC))
            .Build();

        public SimpleBulletSystem()
            : base(Template
                .All(
                    typeof(TransformC),
                    typeof(HitboxC),
                    typeof(SimpleBulletC)))
        {
            Events.Instance.Subscribe<SimpleBulletSpawnE>(CreateBullet);
        }


        private void CreateBullet(SimpleBulletSpawnE e)
        {
            var userEntity = e.UserEntity;

            var transformC = userEntity.GetComponent<TransformC>();
            var bullet = mEngine.EntityFactory.Create(e.BulletName);


            var bulletC = bullet.GetComponent<SimpleBulletC>(SimpleBulletC.Type);
            bulletC.GunOwnerId = userEntity.Id;
            bulletC.ShotFiredPos = e.ShotFiredPos;
            bullet.GetComponent<TransformC>().CurrentPosition =
                transformC.CurrentPosition + Rotate(e.Offset, transformC.RotationRadians);
            bullet.GetComponent<TransformC>().SetRotation(transformC.RotationVector);
            bullet.GetComponent<SimpleBulletC>().Damage = e.Damage;
            mEngine.EntityManager.Add(bullet);

        }


        private static Vector2 Rotate(Vector2 v, double degrees)
        {
            return new Vector2(
                (float)(v.X * Math.Cos(degrees) - v.Y * Math.Sin(degrees)),
                (float)(v.X * Math.Sin(degrees) + v.Y * Math.Cos(degrees))
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
            var bulletC = entity.GetComponent<SimpleBulletC>(SimpleBulletC.Type);

            bulletC.PassedTime += time.ElapsedGameTime.Milliseconds;

            if (bulletC.PassedTime > bulletC.LifetimeMs)
            {
                mEngine.EntityManager.Remove(entity.Id);
                return;
            }

            var transC = entity.GetComponent<TransformC>(TransformC.Type);
            var hitboxC = entity.GetComponent<HitboxC>(HitboxC.Type);

            var newPosition = transC.CurrentPosition + transC.RotationVector * bulletC.Speed;

            var ignoredIds = new[] {entity.Id, bulletC.GunOwnerId};
                
            var entityList = mEngine.Collision.GetCollidingEntities(hitboxC, newPosition, mTemplate, ignoredIds);

            if (!entityList.Any())
            {
                transC.CurrentPosition = newPosition;
                return;
            }

            if (entityList.Any(ent => ent.HasComponent(CollisionImpenetrableC.Type)))
            {
                mEngine.EntityManager.Remove(entity.Id);
                return;
            }

            var firstColEntity = entityList.First();

            // enemy bullets do not hit other enemies
            var bulletOwner = mEngine.EntityManager.Get(bulletC.GunOwnerId);
            if (bulletOwner != null && bulletOwner.HasComponent(Enemy.Type) && firstColEntity.HasComponent(Enemy.Type))
            {
                transC.CurrentPosition = newPosition;
                return;
            }
            
            SoundManager.Instance.PlaySound("hit");
            
            firstColEntity.GetComponent<HealthC>(HealthC.Type)
                .ApplyDamage(firstColEntity, bulletC.Damage, bulletC.GunOwnerId, bulletC.ShotFiredPos, false);

            if (firstColEntity.HasComponent(AnimationC.Type))
            {
                firstColEntity.GetComponent<AnimationC>(AnimationC.Type).EffectCheck = true;
            }

            var stats = Stats.Instance;
            if (entityList.First().HasComponent(UserControllableC.Type))
            {
                // increment Stat and update achievement
                stats.KevinReceivedDmg += bulletC.Damage;
            }
            else
            {
                // increment Stat and update achievement
                stats.EnemyReceivedDmg += bulletC.Damage;
                AchievementSystem.TestAchievements(1, stats.EnemyReceivedDmg);
            }

            mEngine.EntityManager.Remove(entity.Id);
        }
    }
}