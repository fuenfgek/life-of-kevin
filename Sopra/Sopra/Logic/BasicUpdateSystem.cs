using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sopra.ECS;
using Sopra.Input;

namespace Sopra.Logic
{
    /// <summary>
    /// System for updating health.
    /// Requires:
    ///     - HealthComponent
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    internal sealed class BasicUpdateSystem : IteratingEntitySystem
    {
        public BasicUpdateSystem() :
            base(new TemplateBuilder()
                .All(typeof(HealthC),
                    typeof(TransformC)))
        {
        }


        /// <summary>
        /// Process every entity which matches the systems template.
        /// This method will be called automatically during the game loop in the update phase.
        /// </summary>
        /// <inheritdoc cref="IteratingEntitySystem.Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        public override void Process(Entity entity, GameTime time)
        {
            var healthComoponent = entity.GetComponent<HealthC>();
            if (healthComoponent.Health == 0)
            {
                mEngine.EntityManager.Remove(entity);
            }

            // Only for testing #Felix
            if (InputManager.Get().KeyReleased(Keys.H))
            {
                Console.WriteLine(
                    $"Entity {entity.Id} life: {healthComoponent.Health}/{healthComoponent.MaxHealth}");
            }
        }
    }
}