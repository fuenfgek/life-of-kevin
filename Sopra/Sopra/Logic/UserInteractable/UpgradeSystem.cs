using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Items;
using Sopra.Logic.Render;

namespace Sopra.Logic.UserInteractable
{
    internal sealed class UpgradeSystem : IteratingEntitySystem
    {
       
        public UpgradeSystem()
            : base(Template
                .All(typeof(UpgradeMachineC), typeof(UserInteractableC)))
        {

           
        }

        /// <summary>
        /// Run the system.
        /// This method will be called automatically by the engine during the update phase.
        /// </summary>
        /// <inheritdoc cref="Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
           
            var userInteractableC = entity.GetComponent<UserInteractableC>();
            var user = mEngine.EntityManager.Get(userInteractableC.InteractingEntityId);
            var stats = Stats.Instance;

            if (userInteractableC.InteractingEntityId == 0)
            {
                return;
            }
            userInteractableC.InteractingEntityId = 0;

            var usedItem = user.GetComponent<InventoryC>().GetActiveItem();
        
            if (usedItem == null)
            {
                return;
            }

            if ( stats.CurrentCoins >= 100)
            {
                

                var actSlot = user.GetComponent<InventoryC>().ActiveSlot;

                var itemStack = mEngine.EntityFactory.Create("ItemStack");
                itemStack.GetComponent<TransformC>().CurrentPosition = entity.GetComponent<TransformC>().CurrentPosition + new Vector2(-65,0);
  


                switch (usedItem.Name)
                {
                    case "pistol":
                        switch (usedItem.Grade)
                        {
                            case 1:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemPistol2();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "pistol";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;
                            case 2:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemPistol3();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "pistol";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;
                        }                        
                        break;

                    case "rifle":
                        switch (usedItem.Grade)
                        {
                            case 1:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRifle2();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "rifle";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;
                            case 2:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRifle3();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "rifle";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;

                        }
                        break;

                    case "minigun":
                        switch (usedItem.Grade)
                        {
                            case 1:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemMinigun2();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "minigun";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;
                            case 2:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemMinigun3();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "minigun";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;


                        }
                        break;

                    case "lasergun":
                        switch (usedItem.Grade)
                        {
                            case 1:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemLasergun2();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "lasergun";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;
                            case 2:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemLasergun3();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "lasergun";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;


                        }
                        break;

                    case "rocketlauncher":
                        switch (usedItem.Grade)
                        {
                            case 1:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRocketlauncher2();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "rocketlauncher";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;
                            case 2:
                                user.GetComponent<InventoryC>().DeleteItem(actSlot);
                                itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRocketlauncher3();
                                itemStack.GetComponent<AnimationC>().CurrentItem = "rocketlauncher";
                                stats.CurrentCoins -= 100;
                                stats.AchievementTime = -2;
                                SoundManager.Instance.PlaySound("upgrade");
                                stats.Upgrade += 1;
                                AchievementSystem.TestAchievements(17, stats.Upgrade);
                                break;


                        }
                        break;
                }

                if (itemStack.GetComponent<ItemStackC>().StoredItem != null)
                {

                    mEngine.EntityManager.Add(itemStack);

                }
            }
            else
            {

                SoundManager.Instance.PlaySound("coffee_machine_nope");

            }

        }
    }
}
