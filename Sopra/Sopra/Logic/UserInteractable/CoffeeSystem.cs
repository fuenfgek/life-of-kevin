using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Health;
using Sopra.Logic.Items;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Compute the Coffeemachine
    /// Requires:
    ///        CoffeeC
    /// </summary>
    /// <author>Marcel Ebbinghaus</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    internal sealed class CoffeeSystem : IteratingEntitySystem
    {

        
        public CoffeeSystem()
            : base(new TemplateBuilder()
                .All(typeof(CoffeeC), typeof(UserInteractableC)))
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
            var coffeeC = entity.GetComponent<CoffeeC>();
            var userInteractableC = entity.GetComponent<UserInteractableC>();
            var stats = Stats.Instance;

            if (userInteractableC.InteractingEntityId == 0)
            {
                return;
            }
            coffeeC.InteractingEntity =
                mEngine.EntityManager.Get(userInteractableC.InteractingEntityId).HasComponent<InventoryC>();
            if (!coffeeC.InteractingEntity)
            {
                userInteractableC.InteractingEntityId = 0;
                return;
                
            }
            var healthComponent = mEngine.EntityManager.Get(userInteractableC.InteractingEntityId).GetComponent<HealthC>();
            userInteractableC.InteractingEntityId = 0;


            if (coffeeC.CurrentCoffeeMachineCharges > 0 && healthComponent.CurrentCoffeeCharges < HealthC.MaxCoffeeCharges && stats.CurrentCoins >= 10)
            {
                SoundManager.Instance.PlaySound("coffee_machine");
                coffeeC.CurrentCoffeeMachineCharges -= 1;
                healthComponent.CurrentCoffeeCharges += 1;
                stats.CurrentCoins -= 10;
            }
            else
            {
                SoundManager.Instance.PlaySound("coffee_machine_nope");
            }
        }
    }
}
