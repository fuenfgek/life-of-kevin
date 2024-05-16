using Sopra.ECS;
using Sopra.Logic.Items;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Marks an entity as ItemStack and stores the item which is currently in the stack.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="UserInteractableC"/>
    public sealed class ItemStackC : IComponent
    {
        public Item StoredItem { get; set; }
    }
}
