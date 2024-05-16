using System;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Health;
using Sopra.Logic.Render;

namespace Sopra.Logic.UserInteractable
{

    internal sealed class ChestSystem : IteratingEntitySystem
    {
        public ChestSystem()
            : base(Template
                .All(typeof(ChestC), typeof(UserInteractableC)))
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
            var chestC = entity.GetComponent<ChestC>();
            var userInteractableC = entity.GetComponent<UserInteractableC>();

            if (userInteractableC.InteractingEntityId == 0)
            {
                return;
            }
            userInteractableC.InteractingEntityId = 0;

            if (!chestC.Check)
            {
                return;
            }
            var rnd = new Random();
            var randNum = rnd.Next(22, 50);

            entity.GetComponent<SimpleSpriteC>().SpritePath = "mapobjects/chestopen";
            Events.Instance.Fire(new EntityDiedE(entity));
            Stats.Instance.Coins += randNum;
            Stats.Instance.CurrentCoins += randNum;
            SoundManager.Instance.PlaySound("chest");
            AchievementSystem.TestAchievements(16, Stats.Instance.Coins);
            chestC.Check = false;

        }
    }
}
