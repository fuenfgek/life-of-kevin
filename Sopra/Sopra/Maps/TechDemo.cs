using Microsoft.Xna.Framework;
using Ninject;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic;
using Sopra.Logic.Collision;
using Sopra.Logic.KI;
using Sopra.Logic.Pathfinding;
using Sopra.Screens;

namespace Sopra.Maps
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Nico Greiner</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TechDemo
    {
        private readonly MapFactory mMapFactory;
        private readonly ScreenManager mScreenManager;
        private readonly IScreenFactory mScreenFactory;
        private readonly IKernel mKernel;

        private Level CurrentLevel { get; set; }
        private Entity mPlayer;



        public TechDemo(
            MapFactory mapFactory,
            ScreenManager screenManager,
            IScreenFactory screenFactory,
            IKernel kernel)
        {
            mMapFactory = mapFactory;

            mScreenManager = screenManager;
            mScreenFactory = screenFactory;
            mKernel = kernel;
        }

        internal void StartNewGame()
        {
            // need to assign Player to null, because only create
            // a new Player if there were no player created before
            mPlayer = null;
            CurrentLevel = LoadLevel("Content/maps/techdemo.tmx");

            var gameScreen = mScreenFactory.CreateGameScreen();
            gameScreen.AssignLevel(CurrentLevel);
            gameScreen.IsTechDemo = true;
            mScreenManager.AddScreen(gameScreen);
            mScreenManager.AddScreen(mScreenFactory.CreatTechDemoStatsScreen());
            mScreenManager.AddScreen(mScreenFactory.CreateQuadScreen());
        }

      


        private Engine GetEngine()
        {
            return mKernel.Get<Engine>();
        }


        private Level LoadLevel(string mapPath)
        {
            var map = mMapFactory.Load(mapPath);

            SoundManager.Instance.StopSong();

            var engine = GetEngine();
            engine.EntityManager.Clear();
            engine.SetPathfinder(map);


            mPlayer = engine.EntityFactory.Create("PlayerTechdemo");
            mPlayer.GetComponent<TransformC>().CurrentPosition = new Vector2(200, 100);
            engine.EntityManager.Add(mPlayer);

            foreach (var o in map.WallHitboxList)
            {
                var wall = engine.EntityFactory.Create("Wall");
                wall.GetComponent<TransformC>().CurrentPosition = new Vector2(o.X, o.Y);
                wall.GetComponent<HitboxC>().Size = new Vector2(o.Width, o.Height);
                engine.EntityManager.Add(wall);
            }

            var x = 0;
            var z = 0;
            for (var i = 0; i < 1000; i++)
            {
                if (x < 40)
                {
                    var enemy = engine.EntityFactory.Create("Worker1");
                    enemy.GetComponent<TransformC>().CurrentPosition = new Vector2(200+64*x,200+64*z);
                    enemy.GetComponent<Enemy>().SetSpawnPos(new Vector2(200 + 64 * x, 200 + 64 * z));
                    enemy.GetComponent<Enemy>().AlwaysAgressiv();
                    enemy.GetComponent<PathFindingC>().PassedTime = (int) (i / (1000.0 / EnemyPathFindingSystem.TimeBetweenCalcs));
                    engine.EntityManager.Add(enemy);
                    x++;
                }
                else
                {
                    z++;
                    x = 0;
                }
            }
            return new Level(map, engine);
        }
     
    }
}