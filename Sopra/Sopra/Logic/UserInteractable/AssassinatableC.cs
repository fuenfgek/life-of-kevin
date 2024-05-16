using Sopra.ECS;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Marks an entity as assassinatable and stores data for that.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    public sealed class AssassinatableC : IComponent
    {
        
        public static ComponentType Type { get; } = ComponentType.Of <AssassinatableC>();

        public int IsFocusedBy { get; set; }
        public int PassedTime { get; set; }
    }
}
