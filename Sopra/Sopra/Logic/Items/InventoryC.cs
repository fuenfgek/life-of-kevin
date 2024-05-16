using System;
using System.Xml.Serialization;
using Sopra.Audio;
using Sopra.ECS;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Holds all items an entity is carrying auround.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    [Serializable]
    public sealed class InventoryC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of<InventoryC>();

        [XmlElement(IsNullable = true)]
        public Item[] InvSlots { get; set; }

        private int mActiveSlot;

        [XmlElement]
        public int Size { get; set; }
        
        /// <summary>
        /// Change the slot which is currently active.
        /// </summary>
        public int ActiveSlot
        {
            get { return mActiveSlot; }
            set
            {
                if (InvSlots[mActiveSlot] != null)
                {
                    if (InvSlots[mActiveSlot].ItemSwapBlocked)
                    {
                        return;
                    }
                    InvSlots[mActiveSlot].StopUsingItem();
                }

                mActiveSlot = value >= InvSlots.Length
                    ? 0
                    : value < 0
                        ? InvSlots.Length - 1
                        : value;
            }
        }

        private InventoryC()
        {
            InvSlots = new Item[1];
            Size = 1;
        }

        public InventoryC(int inventorySize)
        {
            Size = inventorySize;
            InvSlots = new Item[inventorySize];
        }

        public InventoryC(Item firstItem)
        {
            Size = 1;
            InvSlots = new[] {firstItem};
        }


        /// <summary>
        /// Change the position of two items in the inventory.
        /// </summary>
        /// <param name="slot1"></param>
        /// <param name="slot2"></param>
        public void SwapItemsInInv(int slot1, int slot2)
        {
            if (!(slot1 < Size && slot2 < Size))
            {
                return;
            }

            if (GetActiveItem() != null)
            {
                if (GetActiveItem().ItemSwapBlocked)
                {
                    return;
                }
                if (slot1 == ActiveSlot || slot2 == ActiveSlot)
                {
                    GetActiveItem().StopUsingItem();
                }
            }

            var item = InvSlots[slot1];
            InvSlots[slot1] = InvSlots[slot2];
            InvSlots[slot2] = item;
        }

        /// <summary>
        /// Exchange the item in the currently active slot with an new item.
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public Item ExchangeItem(Item newItem)
        {
            GetActiveItem()?.StopUsingItem();
            SoundManager.Instance.PlaySound("pickup");
            var oldItem = InvSlots[mActiveSlot];
            InvSlots[mActiveSlot] = newItem;
            return oldItem;
        }

        /// <summary>
        /// Add the item to the next free slot or exchange it with the current item,
        /// if all slots are blocked.
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public Item AddItem(Item newItem)
        {
            for (var i = 0; i < Size; i++)
            {
                if (InvSlots[i] != null)
                {
                    continue;
                }

                InvSlots[i] = newItem;
                return null;
            }

            return ExchangeItem(newItem);
        }

        /// <summary>
        /// Deletes the item in the given slot.
        /// </summary>
        /// <param name="slot"></param>
        public void DeleteItem(int slot)
        {
            if (slot < Size)
            {
                InvSlots[slot] = null;
            }
        }

        /// <summary>
        /// Return the item which is stored in the active slot.
        /// </summary>
        /// <returns></returns>
        public Item GetActiveItem()
        {
            return InvSlots[ActiveSlot];
        }
    }
}
