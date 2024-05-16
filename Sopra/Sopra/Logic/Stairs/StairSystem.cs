using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.UserInteractable;

namespace Sopra.Logic.Stairs
{
    /// <summary>
    /// Computes all existing simple bullets.
    /// Requires:
    ///     - TransformC
    ///     - HitboxC
    ///     - SimpleBulletC
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Nico Greiner </author>
    internal sealed class StairSystem : IteratingEntitySystem
    {
        public StairSystem()
            : base(Template
                .All(typeof(StairC), typeof(UserInteractableC)))
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
            var stairC = entity.GetComponent<StairC>();
            var userInteractableC = entity.GetComponent<UserInteractableC>();

            if (userInteractableC.InteractingEntityId == 0)
            {
                return;
            }

            userInteractableC.InteractingEntityId = 0;
            var stats = Stats.Instance;
            Events.Instance.Fire(new UsedStair(stairC.StairDirection));
            // increment Stat and update achievement
            stats.Stairs += 1;
            AchievementSystem.TestAchievements(14, stats.Stairs);
        }
    }
}