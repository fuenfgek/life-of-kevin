using Microsoft.Xna.Framework;
using Sopra.Input;

namespace Sopra.Logic.Achievements
{
    /// <summary>
    /// class to manage stats and achievements affiliated to the time
    /// </summary>
    internal sealed class TimerSystem : ECS.System
    {
        public override void ProcessSystem(GameTime time)
        {
            var stats = Stats.Instance;
            // increment Stat and update achievement
            if (stats.IsActive)
            {
                stats.AchievementTime += (float)time.ElapsedGameTime.TotalSeconds;
            }  
            stats.Time += (float)time.ElapsedGameTime.TotalSeconds;
            AchievementSystem.TestAchievements(8, stats.Time);
            AchievementSystem.TestAchievements(9, stats.Time);
            AchievementSystem.TestAchievements(10, stats.Time);

            if (InputManager.Get().LeftClicked() || InputManager.Get().RightClicked())
            {
                stats.AfkTime = 0;
            }
            else
            {
                stats.AfkTime += (float)time.ElapsedGameTime.TotalSeconds;
                AchievementSystem.TestAchievements(0, stats.AfkTime);
            }
        }
    }
}
