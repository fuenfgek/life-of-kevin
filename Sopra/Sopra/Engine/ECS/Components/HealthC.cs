using Sopra.Engine.ECS;

namespace Sopra.Engine.ECS.Components
{
    /// <summary>
    /// Component for storing health data.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    sealed class HealthC : IComponent
    {
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }


        public HealthC(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}
