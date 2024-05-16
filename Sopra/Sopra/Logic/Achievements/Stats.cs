using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Sopra.Audio;
using Sopra.Logic.Boss;
using Sopra.Logic.Health;
using Sopra.Logic.KI;

namespace Sopra.Logic.Achievements
{
    /// <summary>
    /// Statistics
    /// </summary>
    /// <author>Marcel Ebbinghaus</author>
    public sealed class Stats
    {
        private static Stats sInstance;

        public List<bool> mAchievementList;
        public float AfkTime { get; set; }
        public float KevinReceivedDmg { get; set; }
        public float EnemyReceivedDmg { get; set; }
        public float UsedCoffees { get; set; }
        public float KilledEnemies { get; set; }
        public float Time { get; set; }
        public float Stealthkills { get; set; }
        public float Deaths { get; set; }
        public float Flawless { get; set; }
        public float Items { get; set; }
        public float Boss { get; set; }
        public float Stairs { get; set; }
        public float Coins { get; set; }
        public float CurrentCoins { get; set; }
        public float AchievementTime { get; set; }
        public float Upgrade { get; set; }
        public bool Pistol { get; set; }
        public bool Rifle { get; set; }
        public bool Minigun { get; set; }
        public bool Rocketlauncher { get; set; }
        public bool Lasergun { get; set; }
        public bool Coffee { get; set; }
        public bool Car { get; set; }
        public bool Drone { get; set; }
        public bool IsActive { get; set; }
        public string ActiveAchievement { get; set; }

        /// <summary>
        /// Create an Instance of Stats
        /// </summary>
        public static Stats Instance
        {
            get
            {
                if (sInstance != null)
                {
                    return sInstance;
                }

                sInstance = new Stats();
                return sInstance;
            }
        }

        /// <summary>
        /// Update mAchievementList so it can be used for saving and loading
        /// </summary>
        public void UpdateList()
        {
            mAchievementList = new List<bool>();
            foreach (var achievement in AchievementSystem.sAchievementList)
            {
                    mAchievementList.Add(achievement.Achieved);
            }
        }

        /// <summary>
        /// Manage stats and achievements affiliated to the HealthSystem
        /// </summary>
        /// <param name="e"></param>
        public void EntityDiedStats(EntityDiedE e)
        {
            if (e.DeadEntity.HasComponent<Enemy>())
            {
                // increment Stat and update achievement
                var rnd = new Random();
                KilledEnemies += 1;
                var coin = rnd.Next(1, 6);
                Coins += coin;
                CurrentCoins += coin;
                AchievementSystem.TestAchievements(16, Coins);
                AchievementSystem.TestAchievements(5, KilledEnemies);
                AchievementSystem.TestAchievements(6, KilledEnemies);
                AchievementSystem.TestAchievements(7, KilledEnemies);
                SoundManager.Instance.PlaySound("giggle_kevin");
            }
            else if (e.DeadEntity.HasComponent<KevinC>())
            {
                // increment Stat and update disable Flawless Achievement
                Flawless -= 1;
                Deaths += 1;
            }
            else if (e.DeadEntity.HasComponent<BossKiC>())
            {
                // increment Stat and update achievement
                Boss += 1;
            }
        }

        /// <summary>
        /// Manage stats and achievements affiliated to the ItemStackSystem
        /// </summary>
        /// <param name="name"></param>
        public void ObtainedItem(string name)
        {
            if (name == "pistol" && Pistol == false)
            {
                Pistol = true;
                Items += 1;
            }
            else if (name == "rifle" && Rifle == false)
            {
                Rifle = true;
                Items += 1;
            }
            else if (name == "minigun" && Minigun == false)
            {
                Minigun = true;
                Items += 1;
            }
            else if (name == "rocketlauncher" && Rocketlauncher == false)
            {
                Rocketlauncher = true;
                Items += 1;
            }
            else if (name == "lasergun" && Lasergun == false)
            {
                Lasergun = true;
                Items += 1;
            }
            else if (name == "coffee" && Coffee == false)
            {
                Coffee = true;
                Items += 1;
            }
            else if (name == "car" && Car == false)
            {
                Car = true;
                Items += 1;
            }
            else if (name == "drone" && Drone == false)
            {
                Drone = true;
                Items += 1;
            }
            AchievementSystem.TestAchievements(15, Items);
        }

        /// <summary>
        /// Save every stat
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                var xml = new XmlSerializer(typeof(Stats));
                xml.Serialize(stream, this);
                stream.Close();
            }
        }

        /// <summary>
        /// load stats
        /// </summary>
        /// <param name="fileName"></param>
        public static Stats LoadFromFile(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var xml = new XmlSerializer(typeof(Stats));
                return (Stats)xml.Deserialize(stream);
            }
        }

        /// <summary>
        /// reset stats und achievements
        /// </summary>
        public void Reset()
        {
            AfkTime = 0;
            Time = 0;
            KevinReceivedDmg = 0;
            EnemyReceivedDmg = 0;
            Deaths = 0;
            Flawless = 0;
            Boss = 0;
            UsedCoffees = 0;
            KilledEnemies = 0;
            Stealthkills = 0;
            Pistol = false;
            Rifle = false;
            Minigun = false;
            Rocketlauncher = false;
            Lasergun = false;
            Coffee = false;
            Car = false;
            Drone = false;
            Items = 0;
            Stairs = 0;
            Coins = 0;
            CurrentCoins = 0;
            Upgrade = 0;
            foreach (var achievement in AchievementSystem.sAchievementList)
            {
                achievement.Achieved = false;
            }
        }
    }
}