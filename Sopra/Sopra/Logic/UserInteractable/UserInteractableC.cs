using Sopra.ECS;

namespace Sopra.Logic.UserInteractable
{

    /// <summary>
    /// Marks an entity as user interactable.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    public sealed class UserInteractableC : IComponent
    {
        
        public static ComponentType Type { get; } = ComponentType.Of <UserInteractableC>();

        public int InteractingEntityId { get; set; }
    }
}
