﻿using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Health;
using Sopra.Logic.KI;
using Sopra.Logic.Pathfinding;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Computes if an assassinatable entity gets assassinated
    /// Requires:
    ///     - AssassinatableC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    internal sealed class AssassinatableSystem : IteratingEntitySystem
    {
        private const int Damage = 2000;

        public AssassinatableSystem()
            : base(new TemplateBuilder()
                .All(typeof(AssassinatableC), typeof(Enemy)))
        {
        }

        /// <summary>
        /// Run the system.
        /// This method will be called automatically by the engine during the update phase.
        /// </summary>
        /// <inheritdoc cref="Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
            var assasC = entity.GetComponent<AssassinatableC>(AssassinatableC.Type);
            var enemy = entity.GetComponent<Enemy>(Enemy.Type);

            if (assasC.IsFocusedBy == 0)
            {
                return;
            }

            assasC.PassedTime += time.ElapsedGameTime.Milliseconds;

            if (assasC.PassedTime > 5000)
            {
                assasC.IsFocusedBy = 0;
                assasC.PassedTime = 0;
                return;
            }

            var assasinator = mEngine.EntityManager.Get(assasC.IsFocusedBy);
            var assasinatorPos = assasinator.GetComponent<TransformC>(TransformC.Type).CurrentPosition;
            var ownPos = entity.GetComponent<TransformC>(TransformC.Type).CurrentPosition;
            var stats = Stats.Instance;
            if ((assasinatorPos - ownPos).Length() < 80 && enemy.Stance == EnemyStance.Idle)
            {
                entity.GetComponent<HealthC>(HealthC.Type).ApplyDamageSimple(Damage);
                // increment Stat and update achievement
                stats.Stealthkills += 1;
                AchievementSystem.TestAchievements(13, stats.Stealthkills);
                assasinator.GetComponent<PathFindingC>(PathFindingC.Type).TargetId = 0;
            }
        }
    }
}
