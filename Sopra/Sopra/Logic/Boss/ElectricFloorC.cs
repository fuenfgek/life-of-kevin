using Sopra.ECS;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    public sealed class ElectricFloorC : IComponent
    {
        public bool Charging { get; set; }
        public bool Activ { get; set; }
        public float Damage { get; }
        public int PassedTime { get; set; }

        public ElectricFloorC()
        {
            Damage = 0.2f;
        }
    }
}
