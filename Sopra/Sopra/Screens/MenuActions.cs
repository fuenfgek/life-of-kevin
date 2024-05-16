using System.IO;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic;
using Sopra.Logic.Achievements;
using Sopra.Logic.Boss;
using Sopra.Logic.Health;
using Sopra.Maps;
using Sopra.UI;

namespace Sopra.Screens
{
    
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// 
    /// </summary>
    /// <author>Michael Fleig</author>
    public class MenuActions
    {
        private readonly Game1 mGame;
        private readonly ScreenManager mScreenManager;
        private readonly IScreenFactory mScreenFactory;
        private readonly LevelManager mLevelManager;
        private readonly TechDemo mTechDemo;
        private readonly SoundManager mSound;
        private bool mTransparentScreen;
        readonly Stats mStats = Stats.Instance;

        public MenuActions(
            Game1 game,
            ScreenManager screenManager,
            IScreenFactory screenFactory,
            LevelManager levelManager,
            TechDemo techdemo,
            SoundManager soundManager)
        {
            mGame = game;
            mScreenManager = screenManager;
            mScreenFactory = screenFactory;
            mLevelManager = levelManager;
            mTechDemo = techdemo;
            mSound = soundManager;
            
            Events.Instance.Subscribe<EntityDiedE>(EntityDied);
        }


        public void OpenMainScreen()
        {
            mTransparentScreen = false;
            var options = mScreenFactory.CreateOptionScreen();
            try
            {
                options.LoadState();
            }
            catch (FileNotFoundException)
            {
                options.SaveDefault();
            }
            options.LoadState();
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateMainScreen());
            mSound.PlaySong("robot");

        }

        public void OpenOptionScreen()
        {
            var options = mScreenFactory.CreateOptionScreen();
            try
            {
                options.LoadState();
            }
            catch (FileNotFoundException)
            {
                options.SaveDefault();
            }
            options.LoadState();
            mScreenManager.AddScreen(options);
        }

        public void OpenPauseScreen()
        {
            if (!mTransparentScreen)
            {
                SoundManager.Instance.StopAllSounds();
                var screen = mScreenFactory.CreatePauseScreen();
                mScreenManager.AddScreen(screen);
                mTransparentScreen = true;
            }
        }

        public void OpenSimplePauseScreen()
        {
            if (!mTransparentScreen)
            {
                SoundManager.Instance.StopAllSounds();
                var screen = mScreenFactory.CreateSimplePauseScreen();
                mScreenManager.AddScreen(screen);
                mTransparentScreen = true;
            }
        }

        private void OpenGameOverScreen()
        {
            Stats.Instance.Save("stats.xml");
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateGameOverScreen());
        }

        private void OpenYouWonScreen()
        {
            SoundManager.Instance.StopAllSounds();
            SoundManager.Instance.PlaySound("bossdied");
            AchievementSystem.TestAchievements(11, Stats.Instance.Boss);
            AchievementSystem.TestAchievements(12, Stats.Instance.Flawless);
            Stats.Instance.Save("stats.xml");
            mScreenManager.AddScreen(mScreenFactory.CreateYouWonScreen());
        }

        public void OpenStatScreen()
        {
            try
            {
                var stats = Stats.LoadFromFile("stats.xml");
                mStats.AfkTime = stats.AfkTime;
                mStats.Boss = stats.Boss;
                mStats.Deaths = stats.Deaths;
                mStats.EnemyReceivedDmg = stats.EnemyReceivedDmg;
                mStats.KevinReceivedDmg = stats.KevinReceivedDmg;
                mStats.KilledEnemies = stats.KilledEnemies;
                mStats.Flawless = stats.Flawless;
                mStats.Stairs = stats.Stairs;
                mStats.Stealthkills = stats.Stealthkills;
                mStats.UsedCoffees = stats.UsedCoffees;
                mStats.Time = stats.Time;
                mStats.Items = stats.Items;
                mStats.Pistol = stats.Pistol;
                mStats.Rifle = stats.Rifle;
                mStats.Minigun = stats.Minigun;
                mStats.Rocketlauncher = stats.Rocketlauncher;
                mStats.Lasergun = stats.Lasergun;
                mStats.Coffee = stats.Coffee;
                mStats.Car = stats.Car;
                mStats.Drone = stats.Drone;
                mStats.CurrentCoins = stats.CurrentCoins;
                mStats.Coins = stats.Coins;
                mStats.Upgrade = stats.Upgrade;
                mStats.mAchievementList = stats.mAchievementList;
                foreach (var achievement in AchievementSystem.sAchievementList)
                {
                    achievement.Achieved =
                        stats.mAchievementList[AchievementSystem.sAchievementList.IndexOf(achievement)];
                }
            }
            catch (FileNotFoundException)
            {
            }

            mScreenManager.AddScreen(mScreenFactory.CreateStatScreen());

        }

        public void OpenAchievementScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateAchievementScreen());
        }

        public void OpenCreditsScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateCreditsScreen());
        }

        public void Remove(IScreen screen)
        {
            mScreenManager.RemoveScreen(screen);
            mTransparentScreen = false;
        }

        public void Exit()
        {
            // var options = mScreenFactory.CreateOptionScreen();
            // options.SaveState();
            mGame.Exit();
        }

        public void StartNewGame()
        {
            mStats.Reset();
            mScreenManager.Clear();
            mLevelManager.StartNewGame();
        }
        public void StartTechDemo()
        {
            mScreenManager.Clear();
            mTechDemo.StartNewGame();
        }

        public void LoadLastSaved()
        {
            try
            {
                var stats = Stats.LoadFromFile("stats.xml");
                mStats.AfkTime = stats.AfkTime;
                mStats.Boss = stats.Boss;
                mStats.Deaths = stats.Deaths;
                mStats.EnemyReceivedDmg = stats.EnemyReceivedDmg;
                mStats.KevinReceivedDmg = stats.KevinReceivedDmg;
                mStats.KilledEnemies = stats.KilledEnemies;
                mStats.Flawless = stats.Flawless;
                mStats.Stairs = stats.Stairs;
                mStats.Stealthkills = stats.Stealthkills;
                mStats.UsedCoffees = stats.UsedCoffees;
                mStats.Time = stats.Time;
                mStats.Items = stats.Items;
                mStats.Pistol = stats.Pistol;
                mStats.Rifle = stats.Rifle;
                mStats.Minigun = stats.Minigun;
                mStats.Rocketlauncher = stats.Rocketlauncher;
                mStats.Lasergun = stats.Lasergun;
                mStats.Coffee = stats.Coffee;
                mStats.Car = stats.Car;
                mStats.Drone = stats.Drone;
                mStats.CurrentCoins = stats.CurrentCoins;
                mStats.Coins = stats.Coins;
                mStats.Upgrade = stats.Upgrade;
                mStats.mAchievementList = stats.mAchievementList;
                foreach (var achievement in AchievementSystem.sAchievementList)
                {
                    achievement.Achieved =
                        stats.mAchievementList[AchievementSystem.sAchievementList.IndexOf(achievement)];
                }
            }
            catch (FileNotFoundException)
            {

            }
            try
            {
                var currentLevel = mLevelManager.LoadState("OldGame/hey.xml");
                mLevelManager.IsNotSavedYet = false;
                // Assign a new loaded level to LevelManager
                mLevelManager.CurrentLevel = currentLevel;
                var gameScreen = mScreenFactory.CreateGameScreen();
                var hudScreen = mScreenFactory.CreateHudScreen();
                mScreenManager.Clear();
                gameScreen.AssignLevel(currentLevel);
                hudScreen.AssignLevel(currentLevel);
                hudScreen.UpdateLevelLabel(mLevelManager.CurrentLevelNumber);

                mScreenManager.AddScreen(gameScreen);
                mScreenManager.AddScreen(hudScreen);
            }
            catch (FileNotFoundException)
            {
            }
        }

        public void LoadBeginEbene()
        {
            var currentLevel = mLevelManager.LoadState("beginEbene.xml");
            mLevelManager.IsNotSavedYet = false;
            // Assign a new loaded level to LevelManager
            mLevelManager.CurrentLevel = currentLevel;
            var gameScreen = mScreenFactory.CreateGameScreen();
            var hudScreen = mScreenFactory.CreateHudScreen();
            mScreenManager.Clear();
            gameScreen.AssignLevel(currentLevel);
            hudScreen.AssignLevel(currentLevel);
            hudScreen.UpdateLevelLabel(mLevelManager.CurrentLevelNumber);

            mScreenManager.AddScreen(gameScreen);
            mScreenManager.AddScreen(hudScreen);
        }

        public void OpenLoading()
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateLoadingScreen());
        }

        public void OpenLoadingDeath()
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateLoadingScreenDeath());
        }

        public void GoBackFromLoading()
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateMainScreen());
        }

        public void GoBackFromLoadingDeath()
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateGameOverScreen());
        }

        /// <summary>
        /// Just for testing.
        /// </summary>
        /*public void YouWon()
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateYouWonScreen());
        }*/

        private void EntityDied(EntityDiedE e)
        {
            if (e.DeadEntity.HasComponent<BossKiC>())
            {
                OpenYouWonScreen();
            }
            else if (e.DeadEntity.HasComponent<KevinC>())
            {
                OpenGameOverScreen();
            }
        }

        public void CheckIfGamesSaved(Button button)
        {
            var path = Directory.GetCurrentDirectory() + "/OldGame";
            var existsOld = Directory.Exists(path);

            if (!existsOld)
            {
                Directory.CreateDirectory(path);
            }

            if (Directory.GetFiles(path).Length == 0)
            {
                // mIfGamesSaved = false;
                button.Disable();
            }
            else
            {
                // mIfGamesSaved = true;
                button.Enable();
            }
        }
    }
}