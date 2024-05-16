using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Ninject;
using Sopra.ECS;
using Sopra.Logic;
using Sopra.Logic.Achievements;
using Sopra.Logic.Collision;
using Sopra.Logic.Items;
using Sopra.Logic.KI;
using Sopra.Logic.Render;
using Sopra.Logic.Stairs;
using Sopra.Logic.UserInteractable;
using Sopra.Maps.Tutorial;
using Sopra.SaveState;
using Sopra.Screens;
using TiledSharp;

namespace Sopra.Maps
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Michael Fleig</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LevelManager
    {
        private readonly MapFactory mMapFactory;
        private readonly ScreenManager mScreenManager;
        private readonly IScreenFactory mScreenFactory;
        private readonly IKernel mKernel;

        private readonly string[] mMapList;
        private readonly List<Riddle> mRiddleList;

        private readonly Stats mStats = Stats.Instance;

        internal Level CurrentLevel { get; set; }
        public int CurrentLevelNumber { get; private set; }
        private Entity mPlayer;
        public bool IsNotSavedYet { get; set; }


        public LevelManager(
            MapFactory mapFactory,
            List<Riddle> riddleList,
            ScreenManager screenManager,
            IScreenFactory screenFactory,
            IKernel kernel)
        {
            mMapFactory = mapFactory;
            mRiddleList = riddleList;
            mScreenManager = screenManager;
            mScreenFactory = screenFactory;
            mKernel = kernel;

            mMapList = new[]
            {
                "Content/maps/leveltutorial.tmx",
                "Content/maps/level1.tmx",
                "Content/maps/level2.tmx",
                "Content/maps/level3.tmx",
                "Content/maps/level4.tmx",
                "Content/maps/level5.tmx",
                "Content/maps/level6.tmx",
                "Content/maps/level7.tmx",
                "Content/maps/level8.tmx",
                "Content/maps/level9.tmx",
                "Content/maps/levelboss.tmx"
            };
            
            Events.Instance.Subscribe<UsedStair>(OnUsedStair);
        }

        internal void StartNewGame()
        {
            // need to assign Player to null, because only create
            // a new Player if there were no player created before
            mPlayer = null;
            CurrentLevelNumber = 0;
            CurrentLevel = LoadLevel(mMapList[0], 1);
            CreateScreens(CurrentLevel, CurrentLevelNumber);
            IsNotSavedYet = true;
            // Create folders for saving the game if they don't exist.
            CreateSavingFolders();
            // Clear NewGame folder.
            ClearNewGame();
            SaveState("beginEbene.xml");
        }

        /// <summary>
        /// Delete everything from NewGame folder, because need to
        /// overwrite it with new new game.
        /// </summary>
        private void ClearNewGame()
        {
            var sourcePath = Directory.GetCurrentDirectory() + "/NewGame";
            var fileName = "*.xml";
            foreach (var file in new DirectoryInfo(sourcePath).GetFiles(fileName))
            {
                file.Delete();
            }
        }

        private void CreateSavingFolders()
        {
            var exists = Directory.Exists(Directory.GetCurrentDirectory() + "/NewGame");

            if (!exists)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/NewGame");
            }

            var existsOld = Directory.Exists(Directory.GetCurrentDirectory() + "/OldGame");

            if (!existsOld)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/OldGame");
            }
        }

        private void OnUsedStair(UsedStair usedStair)
        {
            LoadNextLevel(usedStair.Direction);
        }
        
        /// <summary>
        /// When Kevin is on the stairs - he goes to the next level.
        /// </summary>
        private void LoadNextLevel(int direction)
        {
            // Save the state of the old level, to be able to come back in this level in the future
            // The current level is saved in xml file.
            string filename;
            if (IsNotSavedYet)
            {
                filename = "NewGame/" + mMapList[CurrentLevelNumber].Split('/')[2].Split('.')[0] + ".xml";
            }
            else
            {
                filename = "OldGame/" + mMapList[CurrentLevelNumber].Split('/')[2].Split('.')[0] + ".xml";
            }
            SaveState(filename);
            mStats.Save("stats.xml");
            mScreenManager.Clear();

            var nextLevelNumber = (CurrentLevelNumber + direction) % mMapList.Length;
            var levelPath = GetDirPath(nextLevelNumber);
            var levelFile = Directory.GetCurrentDirectory() + levelPath;

            if (nextLevelNumber < 0 || nextLevelNumber >= mMapList.Length)
            {
                return;
            }

            CurrentLevelNumber = nextLevelNumber;

            // Create or load new level.
            // If file with saved game exists - load it.
            if (File.Exists(levelFile))
            {
                // the level is loaded from the file, but the player stays old
                var oldPlayer = mPlayer;
                CurrentLevel = LoadState(levelFile);

                foreach (var ent in CurrentLevel.Engine.EntityManager.Entities)
                {
                    if (ent.Value.HasComponent<KevinC>())
                    {
                        CurrentLevel.Engine.EntityManager.Remove(ent.Value);
                    }
                }

                foreach (var o in CurrentLevel.Map.SpawnList)
                {
                    if (o.Type.Equals("Player"))
                    {
                        if (direction == 1
                            && o.Name.Equals("FromBelow"))
                        {
                            oldPlayer.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                        }

                        if (direction == -1
                            && o.Name.Equals("FromAbove"))
                        {
                            oldPlayer.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                        }
                    }
                }
                mPlayer = oldPlayer;
                CurrentLevel.Engine.EntityManager.Add(mPlayer);

                var reinforcementC = mPlayer.GetComponent<ReinforcementC>(ReinforcementC.Type);
                reinforcementC.WaitTimer = 0;
                reinforcementC.SpawnTimer = ReinforcementSystem.MinSpawnTimeSec;
            }
            // If no file with level saved - create a level
            else
            {
                CurrentLevel = LoadLevel(mMapList[CurrentLevelNumber], direction);
            }

            CreateScreens(CurrentLevel, CurrentLevelNumber);
            CurrentLevel.Engine.CurrentLevelNumber = CurrentLevelNumber;
            SaveState("beginEbene.xml");
            mStats.Save("stats.xml");
        }

        private void CreateScreens(Level currentLevel, int currentLevelNumber)
        {
            mScreenManager.Clear();

            var gameScreen = mScreenFactory.CreateGameScreen();
            gameScreen.AssignLevel(currentLevel);
            mScreenManager.AddScreen(gameScreen);

            var hudScreen = mScreenFactory.CreateHudScreen();
            hudScreen.AssignLevel(currentLevel);
            hudScreen.UpdateLevelLabel(currentLevelNumber);
            mScreenManager.AddScreen(hudScreen);
            // mEngine.EntityManager.Add(mPlayer);
            mScreenManager.Update(new GameTime());
        }

        /// <summary>
        /// Get the path of the directory from where the level will be loaded.
        /// Either NewGame or OldGAme.
        /// </summary>
        /// <param name="nextLevelNumber"></param>
        /// <returns></returns>
        private string GetDirPath(int nextLevelNumber)
        {
            string levelPath;
            if (IsNotSavedYet)
            {
                levelPath = "/NewGame/" + mMapList[nextLevelNumber].Split('/')[2].Split('.')[0] + ".xml";
            }
            else
            {
                levelPath = "/OldGame/" + mMapList[nextLevelNumber].Split('/')[2].Split('.')[0] + ".xml";
            }

            return levelPath;
        }

        private Engine GetEngine()
        {
            return mKernel.Get<Engine>();
        }


        private Level LoadLevel(string mapPath, int direction)
        {
            var map = mMapFactory.Load(mapPath);

            // Disable song #Felix
            //SoundManager.Instance.StopSong();

            var engine = GetEngine();
            engine.EntityManager.Clear();
            engine.SetPathfinder(map);

            var enemyPatrollPathDict = new Dictionary<int, List<Vector2>>();

            #region Walls
            foreach (var o in map.WallHitboxList)
            {
                if (o.Type.Equals("PlayerBlockade"))
                {
                    var blockade = engine.EntityFactory.Create("PlayerBlockade");
                    blockade.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    blockade.GetComponent<HitboxC>().Size = new Vector2(o.Width, o.Height);
                    engine.EntityManager.Add(blockade);
                }
                else
                {
                    var wall = engine.EntityFactory.Create("Wall");
                    wall.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    wall.GetComponent<HitboxC>().Size = new Vector2(o.Width, o.Height);
                    engine.EntityManager.Add(wall);
                }
            }
            #endregion

            #region EnemyPaths
            foreach (var o in map.EnemyPathList)
            {
                if (!o.CustomProperties.ContainsKey("EnemyPathID"))
                {
                    continue;
                }

                enemyPatrollPathDict.Add(
                    int.Parse(o.CustomProperties["EnemyPathID"]),
                    ConvertPointsToPath(o.Points, o.X, o.Y));
            }
            #endregion

            #region SpawnList
            foreach (var o in map.SpawnList)
            {
                #region Items
                if (o.Type.Equals("Item"))
                {
                    var itemStack = engine.EntityFactory.Create("ItemStack");
                    itemStack.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);

                    switch (o.Name)
                    {
                        case "Coffee":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemCoffee();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "coffee";
                            break;

                        case "Lasergun":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemLasergun();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "lasergun";
                            break;

                        case "Minigun":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemMinigun();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "minigun";
                            break;

                        case "Pistol":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemPistol();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "pistol";
                            break;

                        case "Rifle":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRifle();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "rifle";
                            break;

                        case "RocketLauncher":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemRocketlauncher();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "rocketlauncher";
                            break;

                        case "Car":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemCar();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "car";
                            break;

                        case "Drone":
                            itemStack.GetComponent<ItemStackC>().StoredItem = new ItemDrone();
                            itemStack.GetComponent<AnimationC>().CurrentItem = "drone";
                            break;
                    }

                    engine.EntityManager.Add(itemStack);
                }
                #endregion

                #region Characters
                if (o.Type.Equals("Player"))
                {
                    if (mPlayer == null)
                    {
                        mPlayer = engine.EntityFactory.Create("Player");
                    }

                    if (direction == 1
                        && o.Name.Equals("FromBelow"))
                    {
                        mPlayer.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                        mPlayer.GetComponent<ReinforcementC>().SpawnPoints[CurrentLevelNumber] = new Vector2(o.X, o.Y);
                    }

                    if (direction == -1
                        && o.Name.Equals("FromAbove"))
                    {
                        mPlayer.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    }

                    if (direction == 0)
                    {
                        mPlayer.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    }

                    engine.EntityManager.Add(mPlayer);
                }

                if (o.Type.Equals("Enemy"))
                {
                    Entity enemy;
                    switch (o.Name)
                    {
                        case "Worker1":
                            enemy = engine.EntityFactory.Create("Worker1");
                            break;

                        case "Worker2":
                            enemy = engine.EntityFactory.Create("Worker2");
                            break;

                        case "Robot1":
                            enemy = engine.EntityFactory.Create("Robot1");
                            break;

                        case "Robot2":
                            enemy = engine.EntityFactory.Create("Robot2");
                            break;

                        case "Robot3":
                            enemy = engine.EntityFactory.Create("Robot3");
                            break;

                        default:
                            throw new Exception("Error: Enemy without valid name Found: " + o.Name);
                    }

                    enemy.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    enemy.GetComponent<Enemy>().SetSpawnPos(new Vector2(o.X, o.Y));
     
                    if (o.CustomProperties.ContainsKey("RotType"))
                    {
                        switch (o.CustomProperties["RotType"])
                        {
                            case "1":
                                enemy.GetComponent<TransformC>().SetRotation(new Vector2(0, 1));
                                break;

                            case "2":
                                enemy.GetComponent<TransformC>().SetRotation(new Vector2(0, -1));
                                break;

                            case "3":
                                enemy.GetComponent<TransformC>().SetRotation(new Vector2(1, 0));
                                break;

                            case "4":
                                enemy.GetComponent<TransformC>().SetRotation(new Vector2(-1, 0));
                                break;
                        }
                    }

                    if (o.CustomProperties.ContainsKey("EnemyPathID"))
                    {
                        var key = int.Parse(o.CustomProperties["EnemyPathID"]);
                        if (enemyPatrollPathDict.ContainsKey(key))
                        {
                            enemy.AddComponent(new PatrollingEnemyC(enemyPatrollPathDict[key]));
                        }
                    }

                    engine.EntityManager.Add(enemy);
                }
                #endregion

                #region BossEntitys
                if (o.Type.Equals("Turret"))
                {
                    var turret = engine.EntityFactory.Create("AutoTurret");
                    turret.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(turret);

                    switch (o.Name)
                    {
                        case "Rifle":
                            turret.GetComponent<InventoryC>().ExchangeItem(new ItemRifle(OffsetType.Turret));
                            break;
                        case "RocketLauncher":
                            turret.GetComponent<InventoryC>().ExchangeItem(new ItemRocketlauncher(OffsetType.Turret));
                            break;
                        case "Minigun":
                            turret.GetComponent<InventoryC>().ExchangeItem(new ItemMinigun(OffsetType.Turret));
                            break;
                        case "Pistol":
                            turret.GetComponent<InventoryC>().ExchangeItem(new ItemPistol(OffsetType.Turret));
                            break;
                    }
                }

                if (o.Type.Equals("LaserTurret"))
                {
                    var turret = engine.EntityFactory.Create("LaserTurret");
                    turret.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(turret);
                }

                if (o.Type.Equals("EnemySpawnPoint"))
                {
                    var spawnPoint = engine.EntityFactory.Create("EnemySpawnPoint");
                    spawnPoint.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(spawnPoint);
                }

                if (o.Type.Equals("TurretSpawnPoint"))
                {
                    var spawnPoint = engine.EntityFactory.Create("TurretSpawnPoint");
                    spawnPoint.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(spawnPoint);
                }

                if (o.Type.Equals("Boss"))
                {
                    var boss = engine.EntityFactory.Create("Boss");
                    boss.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(boss);
                }
                #endregion

            }
            #endregion

            #region StoryList
            foreach (var o in map.StoryList)
            {
                var story = engine.EntityFactory.Create("Story");
                story.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                story.GetComponent<HitboxC>().Size = new Vector2(o.Width, o.Height);
                story.GetComponent<StoryC>().Name = o.Name;
                engine.EntityManager.Add(story);
            }
            #endregion

            #region Mapobjects
            foreach (var o in map.MapObjectList)
            {
                #region

                #region doors
                if (o.Type.Equals("Door1"))
                {
                    var door = engine.EntityFactory.Create("Door");
                    door.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(door);


                    mRiddleList
                        .Add(new Riddle(o.Name, door.Id, o.Type));
                }

                if (o.Type.Equals("Door2"))
                {
                    var door2 = engine.EntityFactory.Create("Door");
                    door2.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(door2);
                    door2.GetComponent<TransformC>().SetRotation(new Vector2(-1, 0));

                    mRiddleList
                        .Add(new Riddle(o.Name, door2.Id, o.Type));
                }

                #endregion

                #region Chairs
                if (o.Type.Equals("Chair1"))
                {
                    var chair = engine.EntityFactory.Create("Chair");
                    chair.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(chair);
                    chair.GetComponent<TransformC>().SetRotation(new Vector2(-1, 0));

                    mRiddleList
                        .Add(new Riddle(o.Name, chair.Id, o.Type));
                }
                if (o.Type.Equals("Chair2"))
                {
                    var chair = engine.EntityFactory.Create("Chair");
                    chair.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(chair);
                    chair.GetComponent<TransformC>().SetRotation(new Vector2(-1, 0));

                    mRiddleList
                        .Add(new Riddle(o.Name, chair.Id, o.Type));
                }
                if (o.Type.Equals("Chair3"))
                {
                    var chair = engine.EntityFactory.Create("Chair");
                    chair.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(chair);
                    

                    mRiddleList
                        .Add(new Riddle(o.Name, chair.Id, o.Type));
                }
                #endregion Chairs

                #region Tables
                if (o.Type.Equals("Table1"))
                {
                    var table = engine.EntityFactory.Create("Table");
                    table.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(table);

                }
                if (o.Type.Equals("Table2"))
                {
                    var table = engine.EntityFactory.Create("Table");
                    table.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(table);
                    table.GetComponent<TransformC>().SetRotation(new Vector2(-1, 0));

                }
                if (o.Type.Equals("Table3"))
                {
                    var table = engine.EntityFactory.Create("Table");
                    table.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(table);
                    table.GetComponent<TransformC>().SetRotation(new Vector2(0, 1));

                }
                if (o.Type.Equals("Table4"))
                {
                    var table = engine.EntityFactory.Create("Table");
                    table.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(table);
                    table.GetComponent<TransformC>().SetRotation(new Vector2(1, 0));

                }
                #endregion

                #endregion
            }

            foreach (var o in map.MapObjectList)
            {
                #region
                if (o.Type.Equals("Switch"))
                {
                    var Switch = engine.EntityFactory.Create("Switch");
                    Switch.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    foreach (var r in mRiddleList)
                    {
                        if (r.Keyword.Equals(o.Name))
                        {

                            Switch.GetComponent<SwitchC>().Id = r.RiddleItem1;
                            Switch.GetComponent<SwitchC>().ObjectName = r.Type;
                        }
                    }

                    engine.EntityManager.Add(Switch);
                }

                if (o.Type.Equals("Chest"))
                {
                    var chest = engine.EntityFactory.Create("Chests");
                    chest.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(chest);

                }
                if (o.Type.Equals("Upgrader"))
                {
                    var upgrader = engine.EntityFactory.Create("Upgrader");
                    upgrader.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(upgrader);

                }

                if (o.Type.Equals("Stair"))
                {
                    var stairs = engine.EntityFactory.Create("Stairs");
                    stairs.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);

                    direction = 0;
                    if (o.Name.Equals("up"))
                    {
                        direction = 1;
                    }
                    else if (o.Name.Equals("down"))
                    {
                        direction = -1;

                    }

                    stairs.GetComponent<StairC>().StairDirection = direction;
                    engine.EntityManager.Add(stairs);

                }

                if (o.Type.Equals("Coffee"))
                {

                    var coffee = engine.EntityFactory.Create("Coffee");
                    coffee.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(coffee);
                }

                if (o.Type.Equals("ElectricFloor"))
                {
                    var floor = engine.EntityFactory.Create("ElectricFloor");
                    floor.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                    engine.EntityManager.Add(floor);
                }
                #endregion
            }
            #endregion
            return new Level(map, engine);
        }


        /// <summary>
        /// Save the current state of the game in xml file.
        /// </summary>
        private void SaveState(string filename)
        {
            var level = CurrentLevel;
            var name = level.Map.Name;
            var entities = level.Engine.EntityManager.Entities; // dict->list
            var listEntitieStates = new List<EntityState>();
            foreach (var i in entities)
            {
                var entity = new EntityState(i.Value);
                listEntitieStates.Add(entity);
            }

            var state = new State
            {
                Name = name,
                Entities = listEntitieStates,
                GameTimeSec = new GameTime().TotalGameTime
            };

            state.Save(filename);
            //mStats.Save("stats.xml");
        }

        public Level LoadState(string filename)
        {
            var state = State.LoadFromFile(filename);
            var map = mMapFactory.Load(state.Name);
            CurrentLevelNumber = Array.IndexOf(mMapList, state.Name);
            // load gametime
            var gameTimeSec = state.GameTimeSec;
            
            var engine = GetEngine();
            engine.EntityManager.Clear();
            engine.SetPathfinder(map);
            
            foreach (var entityState in state.Entities)
            {
                var entity = new Entity(entityState.Id);
                foreach (var component in entityState.mComponents)
                {
                    entity.AddComponent(component.Value);

                }
                engine.EntityManager.Add(entity);

                if (entityState.mComponents.ContainsKey(typeof(KevinC)))
                {
                    mPlayer = entity;
                }
            }
            var gameTime = new GameTime {TotalGameTime = gameTimeSec};
            mScreenManager.Update(gameTime);
            var level = new Level(map, engine);
            return level;
        }

        /// <summary>
        /// </summary>
        /// <param name="tmxPointCollection"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <author>Felix Vogt</author>
        private static List<Vector2> ConvertPointsToPath(
            IEnumerable<TmxObjectPoint> tmxPointCollection,
            float x,
            float y)
        {
            var pointsList = tmxPointCollection.Select(tmxPoint => new Vector2((float)tmxPoint.X + x, (float)tmxPoint.Y + y)).ToList();

            List<Vector2> path;
            if (pointsList[0] == pointsList[pointsList.Count - 1])
            {
                pointsList.RemoveAt(0);
                path = pointsList.ToList();
            }
            else
            {
                path = pointsList.ToList();
                path.RemoveAt(0);
                for (var i = pointsList.Count - 2; i > -1; i--)
                {
                    path.Add(pointsList[i]);
                }
            }

            return path;
        }
    }
}