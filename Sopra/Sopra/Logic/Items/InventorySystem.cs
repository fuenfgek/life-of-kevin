﻿using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// System for updating all items stored in inventorys.
    /// Requires:
    ///     - InventoryC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    internal sealed class InventorySystem : IteratingEntitySystem
    {
        public InventorySystem()
        : base(new TemplateBuilder().All(typeof(InventoryC)))
        {

        }

        protected override void Process(Entity entity, GameTime time)
        {
            foreach (var item in entity.GetComponent<InventoryC>(InventoryC.Type).InvSlots)
            {
                item?.Update(entity, time);
            }
        }
    }
}
