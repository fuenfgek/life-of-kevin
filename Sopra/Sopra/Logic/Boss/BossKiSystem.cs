using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Health;
using Sopra.Logic.KI;
using Sopra.Logic.Render;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// System for running the boss.
    /// Requires:
    ///     - BossKiC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IntervalEntitySystem"/>
    public sealed class BossKiSystem : IteratingEntitySystem
    {
        private const int FloorCooldown = 4000;
        private const int TurretCooldown = 4000;
        private const int TurretSpawnCooldown = 60000;
        private const int EnemySpawnCooldown = 25000;
        private const int ShieldDuration = 5000;

        private Rectangle mFloorSpawnRectangle;
        private Rectangle mStartArea;

        private readonly Template mFloorTemplate = Template.All(typeof(ElectricFloorC)).Build();
        private readonly Template mTurretSpawnTemplate = Template.All(typeof(TurretSpawnC)).Build();
        private readonly Template mTurretsTemplate = Template.All(typeof(LaserTurretC)).Build();
        private readonly Template mEnemySpawnTemplate = Template.All(typeof(EnemySpawnC)).Build();
        private readonly Template mKevinTemplate = Template.All(typeof(KevinC)).Build();
        private readonly Template mInaccessibleTemplate = Template.All(typeof(CollisionInaccessibleC)).Build();

        private SoundEffectInstance mShieldUp;
        private static readonly string[] sEnemyTypes = { "Worker1", "Worker2", "Robot1", "Robot2", "Robot3"};
        private readonly Random mRnd;

        internal BossKiSystem()
            : base(Template
                .All(typeof(BossKiC)))
        {
            mFloorSpawnRectangle = new Rectangle(832, 832, 896, 896);
            mStartArea = new Rectangle(1152, 1664, 256, 896);

            Events.Instance.Subscribe<EntityDiedE>(EntityDied);
            mRnd = new Random();
        }


        /// <summary>
        /// Process every entity which matches the systems template.
        /// This method will be called automatically during the game loop in the update phase.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        /// <inheritdoc cref="Process"/>
        protected override void Process(Entity entity, GameTime time)
        {
            var kiC = entity.GetComponent<BossKiC>();
            var player = mEngine.EntityManager.Get(mKevinTemplate).FirstOrDefault();

            if (!kiC.IsActiv)
            {
                CheckForStart(entity, kiC, player);
                return;
            }

            var healtC = entity.GetComponent<HealthC>();
            kiC.IncreaseTimers(time.ElapsedGameTime.Milliseconds);

            if (kiC.Phase == 0 && healtC.CurrentHealth < 2 * healtC.MaxHealth / 3)
            {
                SoundManager.Instance.PlaySound("nextround");
                SetUpShield(entity, kiC);
                kiC.Phase = 1;
                
            }

            if (kiC.Phase == 1 && healtC.CurrentHealth < healtC.MaxHealth / 3)
            {
                SoundManager.Instance.PlaySound("nextround");
                SetUpShield(entity, kiC);
                kiC.Phase = 2;
               
            }

            CheckForShield(entity, kiC);

            switch (kiC.Phase)
            {
                case 0:
                    UpdateEnemySpawn(kiC);
                    break;

                case 1:
                    UpdateFloors(kiC, player);
                    UpdateEnemySpawn(kiC);
                    break;

                case 2:
                    UpdateFloors(kiC, player);
                    UpdateEnemySpawn(kiC);
                    UpdateTurrets(kiC);
                    break;

                default:
                    throw new Exception("Error: The boss has an invalid phase: " + kiC.Phase);
            }
        }

        private void UpdateFloors(BossKiC kiC, Entity player)
        {
            if (kiC.PassedTimeFloor < FloorCooldown)
            {
                return;
            }
            kiC.PassedTimeFloor = 0;

            var playerPos = player.GetComponent<TransformC>().CurrentPosition;
            var floorList = mEngine.EntityManager.Get(mFloorTemplate);
            
            for (var i = 0; i < floorList.Length; i++)
            {
                var pos = new Vector2();

                switch (i)
                {
                    case 0:
                        pos = playerPos;
                        break;
                    case 1:
                        pos = playerPos + new Vector2(128, 128);
                        break;
                    case 2:
                        pos = playerPos + new Vector2(-128, 128);
                        break;
                    case 3:
                        pos = playerPos + new Vector2(128, -128);
                        break;
                    case 4:
                        pos = playerPos + new Vector2(-128, -128);
                        break;
                }

                if (!mFloorSpawnRectangle.Contains(pos.ToPoint()))
                {
                    continue;
                }

                floorList[i].GetComponent<ElectricFloorC>().Charging = true;
                floorList[i].GetComponent<TransformC>().CurrentPosition = pos;
            }
        }

        private void UpdateTurrets(BossKiC kiC)
        {
            UpdateTurretSpawn(kiC);

            if (kiC.PassedTimeTurrets < TurretCooldown)
            {
                return;
            }
            kiC.PassedTimeTurrets = 0;

            var turretsList = mEngine.EntityManager.Get(mTurretsTemplate);

            foreach (var turret in turretsList)
            {
                turret.GetComponent<LaserTurretC>().AttackCommand = true;
            }
        }

        private void UpdateTurretSpawn(BossKiC kiC)
        {
            if (kiC.PassedTimeTurretSpawn < TurretSpawnCooldown)
            {
                return;
            }
            kiC.PassedTimeTurretSpawn = 0;

            var spawnPointList = mEngine.EntityManager.Get(mTurretSpawnTemplate);

            foreach (var spawnPoint in spawnPointList)
            {
                if (mEngine.Collision.GetCollidingEntities(spawnPoint.GetComponent<HitboxC>(),
                        spawnPoint.GetComponent<TransformC>(),
                        mInaccessibleTemplate,
                        new[] { spawnPoint.Id }).Count != 0)
                {
                    continue;
                }

                var enemy = mEngine.EntityFactory.Create("LaserTurret");
                enemy.GetComponent<TransformC>().CurrentPosition =
                    spawnPoint.GetComponent<TransformC>().CurrentPosition;
                mEngine.EntityManager.Add(enemy);
            }
        }

        private void UpdateEnemySpawn(BossKiC kiC)
        {
            if (kiC.PassedTimeEnemySpawn < EnemySpawnCooldown)
            {
                return;
            }
            kiC.PassedTimeEnemySpawn = 0;

            var spawnPointList = mEngine.EntityManager.Get(mEnemySpawnTemplate);

            foreach (var spawnPoint in spawnPointList)
            {
                var enemy = mEngine.EntityFactory.Create(sEnemyTypes[mRnd.Next(sEnemyTypes.Length)]);
                enemy.GetComponent<TransformC>().CurrentPosition =
                    spawnPoint.GetComponent<TransformC>().CurrentPosition;
                enemy.GetComponent<Enemy>().AlwaysAgressiv();
                mEngine.EntityManager.Add(enemy);
            }
        }

        private void CheckForStart(Entity boss, BossKiC kiC, Entity player)
        {
            if (mStartArea.Contains(player.GetComponent<TransformC>().CurrentPosition))
            {
                return;
            }

            kiC.IsActiv = true;
            kiC.IsInvinncible = false;
            SetUpShield(boss, kiC);

            foreach (var blockade in mEngine.EntityManager.Get(Template.All(typeof(PlayerBlockadeC)).Build()))
            {
                blockade.AddComponent(new CollisionPlayerBlockade());
            }
        }

        private void CheckForShield(Entity ki, BossKiC kiC)
        {
            if (!kiC.IsShielded)
            {
                return;
            }
            if (kiC.PassedTimeShielded <= ShieldDuration)
            {
                return;
            }
            SoundManager.Instance.StoploopSound(mShieldUp);
            kiC.IsShielded = false;
            kiC.IsInvinncible = false;
            ki.GetComponent<AnimationC>().ChangeAnimationActivity("default");
        }

        private void SetUpShield(Entity ki, BossKiC kiC)
        {
            kiC.PassedTimeShielded = 0;
            kiC.IsInvinncible = true;
            kiC.IsShielded = true;
            SoundManager.Instance.PlaySound("shieldup", true, out mShieldUp);
            ki.GetComponent<AnimationC>().ChangeAnimationActivity("shielded");
        }

        private void EntityDied(EntityDiedE e)
        {
            if (!e.DeadEntity.HasComponent<BossKiC>())
            {
                return;
            }

            foreach (var blockade in mEngine.EntityManager.Get(Template.All(typeof(PlayerBlockadeC)).Build()))
            {
                SoundManager.Instance.PlaySound("bossdied");
                blockade.RemoveComponent(typeof(CollisionPlayerBlockade));
            }
        }
    }
}
