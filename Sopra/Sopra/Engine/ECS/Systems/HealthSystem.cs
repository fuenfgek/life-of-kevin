using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sopra.Engine.ECS.Components;
using Sopra.Engine.Input;

namespace Sopra.Engine.ECS.Systems
{
    /// <summary>
    /// System for updating health.
    /// Requires:
    ///     - HealthComponent
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    sealed class HealthSystem : IteratingEntitySystem
    {
        public HealthSystem() :
            base(new TemplateBuilder()
                .All(typeof(HealthC)))
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
            // Only for testing #Felix
            if (InputManager.Get().KeyReleased(Keys.D))
            {
                var healthComoponent = entity.GetComponent<HealthC>();
                MakeDamage(healthComoponent, 1);
                Console.WriteLine(
                    $"Entity {entity.Id} life: {healthComoponent.CurrentHealth}/{healthComoponent.MaxHealth}");
            }
            //-----------------------
        }


        /// <summary>
        /// Reduce the health of a given HealthComponent for a given ammount.
        /// If the Life would drop under 0, it is set to 0 instead.
        /// </summary>
        /// <param name="healthData"></param>
        /// <param name="damage"></param>
        private void MakeDamage(HealthC healthData, int damage)
        {
            if (healthData.CurrentHealth > damage)
            {
                healthData.CurrentHealth--;
            }
            else
            {
                healthData.CurrentHealth = 0;
            }
        }
    }
}