using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Health;

namespace Sopra.Logic.Items
{
    /// <summary>
    /// Class for coffee action
    /// </summary>
    /// <author>Marcel Ebbinghaus</author>
    /// <inheritdoc cref="Item"/>
    public sealed class ItemCoffee : Item
    {
        internal ItemCoffee()
            : base("coffee", "items/icons/coffee_icon", 0, 2500, true, 1)
        {
        }
        

        protected override void UseItem(Entity entity)
        {
            var healthC = entity.GetComponent<HealthC>();

            if (healthC.CurrentCoffeeCharges == 0 || healthC.CurrentHealth >= healthC.MaxHealth)
            {
                return;
            }
            SoundManager.Instance.PlaySound("coffee_burp");
            healthC.ApplyDamageSimple(-healthC.MaxHealth / 5f);
            var stats = Stats.Instance;
            stats.UsedCoffees += 1;
            AchievementSystem.TestAchievements(2, stats.UsedCoffees);
            AchievementSystem.TestAchievements(3, stats.UsedCoffees);
            AchievementSystem.TestAchievements(4, stats.UsedCoffees);

            healthC.CurrentCoffeeCharges--;

            Reload();
        }
    }
}