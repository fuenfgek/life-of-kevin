using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Boss;
using Sopra.Logic.RemoteControlled;

namespace Sopra.Logic.Health
{
    /// <summary>
    /// System for updating health.
    /// Requires:
    ///     - HealthComponent
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    public sealed class HealthSystem : IteratingEntitySystem
    {
        public HealthSystem() :
            base(Template
                .All(typeof(HealthC),
                    typeof(TransformC)))
        {
            var stats = Stats.Instance;
            Events.Instance.Subscribe<EntityDiedE>(stats.EntityDiedStats);
        }


        /// <summary>
        /// Process every entity which matches the systems template.
        /// This method will be called automatically during the game loop in the update phase.
        /// </summary>
        /// <inheritdoc cref="IteratingEntitySystem.Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
            var healthC = entity.GetComponent<HealthC>();

            if (healthC.HasSpecialDamageLogic && healthC.UnhandeldDamage.Count != 0)
            {
                foreach (var pair in healthC.UnhandeldDamage)
                {
                    if (entity.HasComponent<BossKiC>())
                    {
                        HandleBossDamage(entity.GetComponent<BossKiC>(), healthC, mEngine.EntityManager.Get(pair.Key), pair.Value);
                    }
                    else if (entity.HasComponent<LaserTurretC>())
                    {
                        HandleLaserTurretDamage(healthC, mEngine.EntityManager.Get(pair.Key), pair.Value);
                    }
                }
                healthC.UnhandeldDamage.Clear();
            }

            if (healthC.CurrentHealth > 0)
            {
                return;
            }

            SoundManager.Instance.PlaySound("blood");
            Events.Instance.Fire(new EntityDiedE(entity));

            if (entity.HasComponent<CarC>())
            {
                mEngine.EntityManager.Get(entity.GetComponent<CarC>(CarC.Type).PlayerId).AddComponent(new UserControllableC());
            }

            mEngine.EntityManager.Remove(entity);
        }

        private void HandleBossDamage(BossKiC bossKiC, HealthC bossHealthC, Entity attackingEntity, float damage)
        {
            if (!bossKiC.IsInvinncible
                && attackingEntity != null
                && attackingEntity.HasComponent<KevinC>())
            {
                bossHealthC.CurrentHealth -= damage;
            }
        }

        private void HandleLaserTurretDamage(HealthC turretHealthC, Entity attackingEntity, float damage)
        {
            if (attackingEntity != null
                && attackingEntity.HasComponent<KevinC>())
            {
                turretHealthC.CurrentHealth -= damage;
            }
        }
    }
}