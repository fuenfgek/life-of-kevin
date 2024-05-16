using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;

namespace Sopra.Logic.KI
{
    public sealed  class ReinforcementSystem : IteratingEntitySystem
    {
        private const int SecPerLevel = 60;
        public const int MinSpawnTimeSec = 10;
        private const int MaxSpawnTimeSec = 60;
        private static readonly string[] sEnemyTypes = { "Worker1", "Worker2"};

        private readonly Template mInaccsessibleTemplate = Template.All(typeof(CollisionInaccessibleC)).Build();
        private readonly Random mRnd;

        public ReinforcementSystem()
            : base(Template.All(typeof(ReinforcementC)))
        {
            mRnd = new Random();
        }

        protected override void Process(Entity entity, GameTime time)
        {
            var component = entity.GetComponent<ReinforcementC>();

            if (mEngine.CurrentLevelNumber == 0 || mEngine.CurrentLevelNumber == 10)
            {
                return;
            }

            if (component.MaxLevel < mEngine.CurrentLevelNumber)
            {
                component.MaxLevel = mEngine.CurrentLevelNumber;
                component.SpawnTimer = MaxSpawnTimeSec;
                component.WaitTimer = SecPerLevel;
                component.TimeInLevel = 0;
            }

            if (component.WaitTimer > 0)
            {
                component.WaitTimer -= time.ElapsedGameTime.TotalSeconds;
                return;
            }

            if (component.MaxLevel == mEngine.CurrentLevelNumber
                && component.TimeInLevel < 1000)
            {
                component.TimeInLevel += time.ElapsedGameTime.TotalSeconds;
            }

            component.SpawnTimer -= time.ElapsedGameTime.TotalSeconds;
            if (component.SpawnTimer > 0)
            {
                return;
            }

            component.CollisionTestTimer -= time.ElapsedGameTime.Milliseconds;
            if (component.CollisionTestTimer > 0)
            {
                return;
            }

            component.CollisionTestTimer = 500;
            var spawnPoint = component.SpawnPoints[mEngine.CurrentLevelNumber];
            var enemy = mEngine.EntityFactory.Create(sEnemyTypes[mRnd.Next(sEnemyTypes.Length)]);

            if (mEngine.Collision.GetCollidingEntities(
                    enemy.GetComponent<HitboxC>(),
                    spawnPoint,
                    mInaccsessibleTemplate)
                .Any())
            {
                return;
            }
            component.CollisionTestTimer = 0;

            enemy.GetComponent<TransformC>().CurrentPosition = spawnPoint;
            enemy.GetComponent<Enemy>().AlwaysAgressiv();
            mEngine.EntityManager.Add(enemy);

            var x = mEngine.CurrentLevelNumber == component.MaxLevel
                ? (int)Math.Floor(-0.2 * component.TimeInLevel + (MaxSpawnTimeSec - MinSpawnTimeSec))
                : 0;

            component.SpawnTimer = x <= 0
                ? MinSpawnTimeSec
                : MinSpawnTimeSec + x;
        }
    }
}
