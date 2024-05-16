using Sopra.ECS;

namespace Sopra.Logic
{
    /// <summary>
    /// Marks an entity as user controllable.
    /// There must be exactly one entity which has this component at all time.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    public sealed class UserControllableC : IComponent
    {
        
        public static ComponentType Type { get; } = ComponentType.Of <UserControllableC>();
    }
}