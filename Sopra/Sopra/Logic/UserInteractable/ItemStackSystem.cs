using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Items;
using Sopra.Logic.Render;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Handles all interactions with an item stack.
    /// Requires:
    ///     - ItemStackC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    public sealed class ItemStackSystem : IteratingEntitySystem
    {
        public ItemStackSystem()
            : base(new TemplateBuilder()
                .All(typeof(ItemStackC), typeof(UserInteractableC)))
        {
        }


        /// <summary>
        /// Process every entity which matches the systems template.
        /// This method will be called automatically during the game loop in the update phase.
        /// </summary>
        /// <inheritdoc cref="IteratingEntitySystem.Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
            var userInteractableC = entity.GetComponent<UserInteractableC>();

            if (userInteractableC.InteractingEntityId == 0)
            {
                return;
            }

            var interactingEntity = mEngine.EntityManager.Get(userInteractableC.InteractingEntityId);
            userInteractableC.InteractingEntityId = 0;

            var inv = interactingEntity.GetComponent<InventoryC>();

            if (inv.GetActiveItem() != null
                && inv.GetActiveItem().ItemSwapBlocked)
            {
                return;
            }

            var stats = Stats.Instance;
            var itemStackC = entity.GetComponent<ItemStackC>();

            stats.ObtainedItem(itemStackC.StoredItem.Name);

            itemStackC.StoredItem = inv.AddItem(itemStackC.StoredItem);

            if (itemStackC.StoredItem == null)
            {
                mEngine.EntityManager.Remove(entity);
            }
            else
            {
                entity.GetComponent<AnimationC>().CurrentItem = itemStackC.StoredItem.Name;
            }
        }
    }
}