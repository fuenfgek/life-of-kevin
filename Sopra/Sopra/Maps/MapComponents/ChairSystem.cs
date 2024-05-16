using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic;
using Sopra.Logic.Collision;
using Sopra.Logic.Health;
using Sopra.Logic.Render;

namespace Sopra.Maps.MapComponents
{
    /// <summary>
    /// Compute the Chair function
    /// Requires:
    ///        ChairC
    /// </summary>
    /// <author>Nico Greiner</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    internal sealed class ChairSystem : IteratingEntitySystem
    {
        private readonly Template mHealthTemplate = Template.All(typeof(HealthC)).Build();

        public ChairSystem()
            : base(new TemplateBuilder()
                .All(typeof(ChairC)))
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
            var chairC = entity.GetComponent<ChairC>();

            if (!chairC.Switched)
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

            foreach (var enemy in hitEnemy)
            {
                if (enemy.HasComponent<AnimationC>())
                {
                    enemy.GetComponent<AnimationC>().EffectCheck = true;
                }

                enemy.GetComponent<HealthC>().CurrentHealth = 0;
            }

            chairC.Switched = false;
        }
    }
}
