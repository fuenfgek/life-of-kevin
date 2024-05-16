using System;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Health;
using Sopra.Logic.Render;
using Sopra.Logic.UserInteractable;

namespace Sopra.Logic.Items.Projectiles
{
    internal sealed class DropItemSystem : IteratingEntitySystem
    {
        public DropItemSystem()
            : base(new TemplateBuilder().All(
                typeof(DropItemC)))
        {
            Events.Instance.Subscribe<EntityDiedE>(OnDeathCreateItem);
        }


        private void OnDeathCreateItem(EntityDiedE e)
        {
            if (!e.DeadEntity.HasComponent<DropItemC>())
            {
                return;
            }
           
            var name = e.DeadEntity.GetComponent<DropItemC>().Name;

            var rnd = new Random();
            var randNum = rnd.Next(0, 100);

            var itemStack = mEngine.EntityFactory.Create("ItemStack");
            itemStack.GetComponent<TransformC>().CurrentPosition =
                e.DeadEntity.GetComponent<TransformC>().CurrentPosition;
            
            if (name.Equals("Worker1"))
            {
                
                if (randNum < 60)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemPistol();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "pistol";

                }
 
            }

            if (name.Equals("Worker2"))
            {
                if (randNum < 20)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemPistol();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "pistol";

                }
                if (randNum >= 20 && randNum < 60)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRifle();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "rifle";
                }
            }

            if (name.Equals("Robot1"))
            {
                if (randNum < 50)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRifle();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "rifle";

                }
                if (randNum >= 50 && randNum < 70)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemMinigun();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "minigun";
                }
            }
            if (name.Equals("Robot2"))
            {
                if (randNum < 50)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRifle();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "rifle";

                }
                if (randNum >= 50 && randNum < 55)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemMinigun();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "minigun";
                }
                if (randNum >= 55 && randNum < 60)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRocketlauncher();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "rocketlauncher";
                }
            }
            if (name.Equals("Robot3"))
            {
                if (randNum < 20)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemMinigun();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "minigun";

                }
                if (randNum >= 20 && randNum < 30)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRocketlauncher();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "rocketlauncher";
                }
                if (randNum >= 30 && randNum < 35)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemLasergun();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "lasergun";
                }
            }

            if (name.Equals("Chest"))
            {
                itemStack.GetComponent<TransformC>().CurrentPosition = e.DeadEntity.GetComponent<TransformC>().CurrentPosition + new Vector2(0,65);
                if (randNum < 33)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemMinigun();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "minigun";
                }
                else if (randNum < 66)
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRocketlauncher();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "rocketlauncher";
                }
                else
                {
                    itemStack.GetComponent<ItemStackC>().StoredItem = new ItemLasergun();
                    itemStack.GetComponent<AnimationC>().CurrentItem = "lasergun";
                }

            }

            if (itemStack.GetComponent<ItemStackC>().StoredItem != null)
            {
                mEngine.EntityManager.Add(itemStack);
            }
        }

        protected override void Process(Entity entity, GameTime time)
        {
        }
    }
}
