using Sopra.ECS;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Stores all data regarding a coffee machine
    /// </summary>
    /// <author>Marcel Ebbinghaus</author>
    /// <inheritdoc cref="IComponent"/>
    public sealed class CoffeeC : IComponent
    {
        private const int MaxCoffeeMachineCharges = 5;
        public int CurrentCoffeeMachineCharges {get; set;}

        public bool InteractingEntity { get; set; }


        public CoffeeC()
        {
            CurrentCoffeeMachineCharges = MaxCoffeeMachineCharges;
        }
    }
}
