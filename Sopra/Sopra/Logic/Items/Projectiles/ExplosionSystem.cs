using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Collision;
using Sopra.Logic.Health;
using Sopra.Logic.KI;
using Sopra.Logic.Render;

namespace Sopra.Logic.Items.Projectiles
{
    /// <summary>
    /// Class for Calculating Explosion
    /// </summary>
    /// <author>Konstantin Fünfgelt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    internal sealed class ExplosionSystem : IteratingEntitySystem
    {
        private readonly Template mHealthTemplate = Template.All(typeof(HealthC)).Build();

        public ExplosionSystem()
            : base(new TemplateBuilder()
                .All(
                    typeof(TransformC),
                    typeof(HitboxC),
                    typeof(ExplosionC),
                    typeof (AnimationC)))
        {

        }
        /// <summary>
        /// Calculate damage to all HealthC Objects which collide with Explosion
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
            var explosionC = entity.GetComponent<ExplosionC>();

            if (!explosionC.DealDamage)
            {
                return;
            }

            var transC = entity.GetComponent<TransformC>();
            var hitboxC = entity.GetComponent<HitboxC>();

            var hitEnemy = mEngine.Collision.GetCollidingEntities(hitboxC, transC, mHealthTemplate);

            if (hitEnemy == null)
            {
                return;
            }
            
            SoundManager.Instance.PlaySound("explosion");
            Events.Instance.Fire(new NoiseEvent(explosionC.GunOwnerId, transC.CurrentPosition, 300));
            var stats = Stats.Instance;
            foreach (var enemy in hitEnemy)
            {
                if (enemy.HasComponent<AnimationC>())
                {
                    enemy.GetComponent<AnimationC>().EffectCheck = true;
                }
                enemy.GetComponent<HealthC>().ApplyDamage(
                    enemy,
                    explosionC.Damage, 
                    explosionC.GunOwnerId,
                    explosionC.ShotFiredPos,
                    explosionC.IsCarExplosion);
                if (enemy.HasComponent<UserControllableC>())
                {
                    // increment Stat and update achievement
                    stats.KevinReceivedDmg += explosionC.Damage;
                }
                else
                {
                    // increment Stat and update achievement
                    stats.EnemyReceivedDmg += explosionC.Damage;
                    AchievementSystem.TestAchievements(1, stats.EnemyReceivedDmg);
                }
            }

            explosionC.DealDamage = false;

        }
    }
}
