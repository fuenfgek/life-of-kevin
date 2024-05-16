using System.Collections.Generic;
using Sopra.Audio;

namespace Sopra.Logic.Achievements
{
    /// <summary>
    /// System for managing achievements
    /// </summary>
    /// <author>Marcel Ebbinghaus</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class AchievementSystem
    {
        // list to store and manage achievements
        public static readonly List<Achievement> sAchievementList;

        static AchievementSystem()
        {
            sAchievementList = new List<Achievement>
            {
                new Achievement("AFK", 300),
                new Achievement("Sadistic", 100),
                new Achievement("Wake up!", 3),
                new Achievement("Heartattack!!", 10),
                new Achievement("Obsessed of Coffee!!!", 15),
                new Achievement("Rookie", 10),
                new Achievement("War-Machine", 50),
                new Achievement("Terminator", 150),
                new Achievement("Let the Journey begin", 1),
                new Achievement("Do you have Friends", 18000),
                new Achievement("You definitly don't have Friends!", 25200),
                new Achievement("Alpha Kevin", 1),
                new Achievement("Easy Peasy Lemon Squeezy", 0),
                new Achievement("Agent 47", 15),
                new Achievement("First Challenge", 1),
                new Achievement("Lord of War", 8),
                new Achievement("Moneyboy", 100),
                new Achievement("Craftsman", 1)
            };          
        }

        /// <summary>
        /// Test the given achievement if its requirement is fulfilled
        /// Change its state to achieved if so
        /// </summary>
        /// <param name="index"></param>
        /// <param name="stat"></param>
        public static void TestAchievements(int index, float stat)
        {
            var stats = Stats.Instance;
            if (sAchievementList[index].TriggerValue <= stat && sAchievementList[index].Achieved == false)
            {
                sAchievementList[index].Achieved = true;
                stats.UpdateList();
                SoundManager.Instance.PlaySound(sAchievementList[index].Name);
                stats.IsActive = true;
                stats.ActiveAchievement = sAchievementList[index].Name;
                if (index == 14)
                {
                    Stats.Instance.Save("stats.xml");
                }
            }
        }
    }
}
