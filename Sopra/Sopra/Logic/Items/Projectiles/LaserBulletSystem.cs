using System;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Health;
using Sopra.Logic.Render;


namespace Sopra.Logic.Items.Projectiles
{
    internal sealed class LaserBulletSystem : IteratingEntitySystem
    {
        private readonly Template mHealthTemplate = Template.All(typeof(HealthC)).Build();
        private readonly Template mObstacle = Template.All(typeof(CollisionImpenetrableC)).Build();
        public LaserBulletSystem()
            : base(Template
                .All(
                    typeof(HitboxC),
                    typeof(LaserBulletC)))
        {
            Events.Instance.Subscribe<LaserBulletSpawnE>(CreateBullet);
        }

        private void CreateBullet(LaserBulletSpawnE e)
        {
            var userEntity = e.UserEntity;
            var transformC = userEntity.GetComponent<TransformC>();

            var bullet = mEngine.EntityFactory.Create(e.BulletName);

            var laser = bullet.GetComponent<LaserBulletC>();
            laser.Damage = e.Damage;
            laser.Range = e.Range;
            laser.LifetimeMs = e.Lifetime;
            laser.GunOwnerId = userEntity.Id;
            laser.ShotFiredPos = e.ShotFiredPos;
            var sprite = bullet.GetComponent<SimpleSpriteC>();
            var hitbox = bullet.GetComponent<HitboxC>();
            var startPoint = transformC.CurrentPosition + Rotate(e.Offset, transformC.RotationRadians);
            var rotation = transformC.RotationVector;
            var testbox = new HitboxC(startPoint.X, startPoint.Y, startPoint.X, startPoint.Y  );
            var f = 0;
            var check = true;
            
            while ((testbox.Startpoint - testbox.Endpoint).Length() <=
                   laser.Range && check)
            {
                f += 10;
                testbox.Endpoint = startPoint + rotation * f;
                if (mEngine.Collision.GetCollidingEntities(testbox, transformC, mObstacle).Count > 0)
                {
                    check = false;
                }
            }
            hitbox.Startpoint = startPoint;
            hitbox.Endpoint = testbox.Endpoint;
            var length = (startPoint - hitbox.Endpoint).Length();
            sprite.SpriteSize = new Vector2(sprite.SpriteSize.X, length);
            bullet.GetComponent<TransformC>().SetRotation(transformC.RotationVector);
            bullet.GetComponent<TransformC>().CurrentPosition =
                startPoint + transformC.RotationVector * (float) ((hitbox.Startpoint - hitbox.Endpoint).Length() / 2.0) ;
            mEngine.EntityManager.Add(bullet);

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

            var bulletC = entity.GetComponent<LaserBulletC>();

            bulletC.PassedTime += time.ElapsedGameTime.Milliseconds;

            if (bulletC.PassedTime > bulletC.LifetimeMs)
            {
                mEngine.EntityManager.Remove(entity.Id);
            }

            var hitEnemy = mEngine.Collision.GetCollidingEntities(entity, mHealthTemplate);
            if (hitEnemy.Count == 0)
            {
                return;
            }

            if (!bulletC.DealDamage)
            {
                return;
            }
            foreach (var enemy in hitEnemy)
            {
                if (enemy.HasComponent<AnimationC>())
                {
                    enemy.GetComponent<AnimationC>().EffectCheck = true;
                }
                var stats = Stats.Instance;
                enemy.GetComponent<HealthC>().ApplyDamage(enemy, bulletC.Damage, bulletC.GunOwnerId, bulletC.ShotFiredPos, false);
                // increment Stat and update achievement
                stats.EnemyReceivedDmg += bulletC.Damage;
                AchievementSystem.TestAchievements(1, stats.EnemyReceivedDmg);
            }

            bulletC.DealDamage = false;
        }

        private static Vector2 Rotate(Vector2 v, double degrees)
        {
            return new Vector2(
                (float)(v.X * Math.Cos(degrees) - v.Y * Math.Sin(degrees)),
                (float)(v.X * Math.Sin(degrees) + v.Y * Math.Cos(degrees))
            );
        }
    }
}
