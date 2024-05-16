using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Render;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Implements a base class for all items.
    /// </summary>
    /// <author>Felix Vogt</author>
    
    [XmlInclude(typeof(ItemCar))]
    [XmlInclude(typeof(ItemCoffee))]
    [XmlInclude(typeof(ItemDrone))]
    [XmlInclude(typeof(ItemLasergun))]
    [XmlInclude(typeof(ItemLasergun2))]
    [XmlInclude(typeof(ItemLasergun3))]
    [XmlInclude(typeof(ItemTurretLasergun))]
    [XmlInclude(typeof(ItemMinigun))]
    [XmlInclude(typeof(ItemMinigun2))]
    [XmlInclude(typeof(ItemMinigun3))]
    [XmlInclude(typeof(ItemPistol))]
    [XmlInclude(typeof(ItemPistol2))]
    [XmlInclude(typeof(ItemPistol3))]
    [XmlInclude(typeof(ItemRifle))]
    [XmlInclude(typeof(ItemRifle2))]
    [XmlInclude(typeof(ItemRifle3))]
    [XmlInclude(typeof(ItemRocketlauncher))]
    [XmlInclude(typeof(ItemRocketlauncher2))]
    [XmlInclude(typeof(ItemRocketlauncher3))]
    [Serializable]
    public abstract class Item
    {
        [XmlElement]
        public string LogoPath { get; set; }
        [XmlElement]
        public string Name { get; set; }


        /// <summary>
        /// Don't set this manually. Use Reload() instead.
        /// </summary>
        [XmlElement]
        public bool IsReloading { get; set; }

        /// <summary>
        /// Don't set this manually. Test for "ItemSwapBlocked" if you want to know if changing the active item is allowed.
        /// </summary>
        [XmlElement]
        public bool BlockItemSwapDuringReload { get; set; }

        /// <summary>
        /// Indicates if it's allowed to change the currently active item.
        /// </summary>
        [XmlElement]
        public bool ItemSwapBlocked { get; set; }

        [XmlElement]
        public int ReloadDuration { get; set; }
        [XmlElement]
        public int ShotCooldown { get; set; }
        [XmlElement]
        public int PassedTime { get; set; }
        [XmlElement]
        public int Grade { get; set; }

        /// <summary>
        /// True as long the user holds the left button down.
        /// </summary>
        [XmlElement]
        public bool IsGettingUsed { get; set; }

        protected Item(){}

        /// <param name="name">Name of the item. Needs to be the same as in the dict in animationC of the entity using this item.</param>
        /// <param name="logoPath">Texture used to display the item in the inventory.</param>
        /// <param name="shotCooldown">Cooldown between single shots/uses (in ms).</param>
        /// <param name="reloadDuration">Duration of a reload (in ms).</param>
        /// <param name="blockItemSwapDuringReload">If true: Entity can't change the active weapon when he's reloading.</param>
        ///  <param name="grade">Upgrade level.</param>
        protected Item(string name, string logoPath, int shotCooldown, int reloadDuration, bool blockItemSwapDuringReload,int grade)
        {
            LogoPath = logoPath;
            Name = name;
            Grade = grade;
            ShotCooldown = shotCooldown;
            PassedTime = shotCooldown;
            ReloadDuration = reloadDuration;
            BlockItemSwapDuringReload = blockItemSwapDuringReload;
        }

        /// <summary>
        /// Gets called every frame, if this item is the active item.
        /// Implements a cooldown.
        /// </summary>
        /// <param name="entity">The entity carrying this item.</param>
        /// <param name="time">The current GameTime.</param>
        public void Update(Entity entity, GameTime time)
        {
            PassedTime += time.ElapsedGameTime.Milliseconds;
            
            if (IsReloading)
            {
                if (PassedTime >= ReloadDuration)
                {
                    PassedTime = ShotCooldown;
                    IsReloading = false;
                    ItemSwapBlocked = false;
                }
                else
                {
                    return;
                }
            }

            if (!IsGettingUsed) { return; }
            
            if (PassedTime < ShotCooldown)
            {
                return;
            }
            PassedTime = 0;

            var animC = entity.GetComponent<AnimationC>();
            if (animC.UpdateItemAnimation)
            {
                animC.OnlyOnce = true;
                animC.ChangeAnimationActivity("use");
            }

            UseItem(entity);
        }

        /// <summary>
        /// Start using this Item.
        /// Note: Don't call this inside an item!
        /// </summary>
        public void StartUsingItem()
        {
            IsGettingUsed = true;
        }

        /// <summary>
        /// Stop using this Item.
        /// Note: Don't call this inside an item!
        /// </summary>
        public void StopUsingItem()
        {
            if (!IsGettingUsed)
            {
                return;
            }
            if (!IsReloading || !BlockItemSwapDuringReload)
            {
                ItemSwapBlocked = false;
            }
            IsGettingUsed = false;
        }

        internal void Reload()
        {
            if (BlockItemSwapDuringReload)
            {
                ItemSwapBlocked = true;
            }
            IsReloading = true;
        }

        /// <summary>
        /// Gets called if the item gets used and the set cooldown is over.
        /// If the user continues to use the item, this method will be called periodically.
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void UseItem(Entity entity);
    }
}
