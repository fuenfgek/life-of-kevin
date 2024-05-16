using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.ECS;
using Sopra.Engine.ECS.Components;


namespace Sopra.Engine.ECS.Systems
{
    /// <summary>
    /// Handles all entitys with a UserInteractableComponent that got clicked by the player.
    /// Requires:
    ///     - UserInteractableComponent
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    sealed class UserInteractableSystem : IteratingEntitySystem
    {
        public UserInteractableSystem() :
            base(new TemplateBuilder()
                .All(typeof(UserInteractableC)))
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
            var interactableComponent = entity.GetComponent<UserInteractableC>();

            if (interactableComponent.InteractingChar == null)
            {
                return;
            }

            // only true if the player is already near the entity
            // do stuff with the Entity interactableComponent.InteractingChar
            // UserInteractableComponent could save what effect should happen

            // only for testing #Felix
            // interactableComponent.Type == 0 => healing object
            if (interactableComponent.Type == 0 &&
                interactableComponent.InteractingChar.HasComponent(typeof(HealthC)))
            {
                Console.WriteLine("Entity 0 got healed");
                var healthData = interactableComponent.InteractingChar.GetComponent<HealthC>();
                healthData.CurrentHealth++;
            }
            //-----------------------

            interactableComponent.InteractingChar = null;
        }
    }
}