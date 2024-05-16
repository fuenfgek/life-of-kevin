using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Boss;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Health;
using Sopra.Logic.Items;
using Sopra.Logic.Items.Projectiles;
using Sopra.Logic.KI;
using Sopra.Logic.Pathfinding;
using Sopra.Logic.RemoteControlled;
using Sopra.Logic.Render;
using Sopra.Logic.Stairs;
using Sopra.Logic.UserInteractable;
using Sopra.Maps.MapComponents;
using Sopra.Maps.Tutorial;

namespace Sopra.Logic
{
    /// <summary>
    ///     Create blueprints
    /// </summary>
    internal sealed class Blueprints
    {
        private readonly EntityFactory mFactory;

        internal Blueprints(EntityFactory factory)
        {
            mFactory = factory;
        }

        internal void CreateBlueprints()
        {
            #region Characters
            mFactory.AddBlueprint("Player",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(0, GetPlayerDict()))
                    .AddComponent(new PathFindingC(CollisionTemplate.InaccsessiblePlayerBlockade))
                    .AddComponent(new SteeringC())
                    .AddComponent(new UserControllableC())
                    .AddComponent(new InventoryC(5))
                    .AddComponent(new HealthC(100))
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CorpseC("PlayerCorpse"))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new KevinC())
                    .AddComponent(new ReinforcementC()));

            mFactory.AddBlueprint("PlayerTechdemo",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(0, GetPlayerDict()))
                    .AddComponent(new PathFindingC(CollisionTemplate.InaccsessiblePlayerBlockade))
                    .AddComponent(new SteeringC())
                    .AddComponent(new UserControllableC())
                    .AddComponent(new InventoryC(5))
                    .AddComponent(new HealthC(30000000))
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CorpseC("PlayerCorpse"))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new KevinC()));

            mFactory.AddBlueprint("PlayerCorpse",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(0.9f, "entitys/kevin/dead")));
            
            mFactory.AddBlueprint("Car",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(0, "items/icons/car_icon"))
                    .AddComponent(new PathFindingC(CollisionTemplate.InaccsessiblePlayerBlockade, 2))
                    .AddComponent(new SteeringC())
                    .AddComponent(new UserControllableC())
                    .AddComponent(new HitboxC(20, 20))
                    .AddComponent(new CarC())
                    .AddComponent(new HealthC(4))
                    .AddComponent(new CollisionInaccessibleC()));

            mFactory.AddBlueprint("Drone",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(-0.5f, GetDroneDict()))
                    .AddComponent(new PathFindingC(CollisionTemplate.InpenetrablePlayerBlockade, 4))
                    .AddComponent(new SteeringC())
                    .AddComponent(new UserControllableC())
                    .AddComponent(new HitboxC(20, 20))
                    .AddComponent(new DroneC()));

            #endregion

            #region Enemys

            mFactory.AddBlueprint("TechDemoEnemy",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(0,"entitys/roboter1/robot1"))            
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new PathFindingC(movementSpeed: 2))
                    .AddComponent(new SteeringC())
                    .AddComponent(new Enemy()));


            mFactory.AddBlueprint("Worker1",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(0, GetWorker1Dict(), false))
                    .AddComponent(new PathFindingC(movementSpeed: 2))
                    .AddComponent(new SteeringC())
                    .AddComponent(new HealthC(4))
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CorpseC("EnemyCorpse"))
                    .AddComponent(new AssassinatableC())
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new Enemy())
                    .AddComponent(new DropItemC("Worker1"))
                    .AddComponent(new InventoryC(new ItemPistol())));

            mFactory.AddBlueprint("Worker2",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(0, GetWorker2Dict(), false))
                    .AddComponent(new PathFindingC(movementSpeed: 2))
                    .AddComponent(new SteeringC())
                    .AddComponent(new HealthC(8))
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CorpseC("EnemyCorpse"))
                    .AddComponent(new AssassinatableC())
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new Enemy())
                    .AddComponent(new DropItemC("Worker2"))
                    .AddComponent(new InventoryC(new ItemRifle())));

            mFactory.AddBlueprint("Robot1",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(0, GetRobot1Dict(), false))
                    .AddComponent(new PathFindingC(movementSpeed: 3))
                    .AddComponent(new SteeringC())
                    .AddComponent(new HealthC(5))
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CorpseC("EnemyCorpse"))
                    .AddComponent(new AssassinatableC())
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new Enemy())
                    .AddComponent(new DropItemC("Robot1"))
                    .AddComponent(new InventoryC(new ItemMinigun())));

            mFactory.AddBlueprint("Robot2",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(0, GetRobot2Dict(), false))
                    .AddComponent(new PathFindingC(movementSpeed: 2))
                    .AddComponent(new SteeringC())
                    .AddComponent(new HealthC(40))
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CorpseC("EnemyCorpse"))
                    .AddComponent(new AssassinatableC())
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new Enemy())
                    .AddComponent(new DropItemC("Robot2"))
                    .AddComponent(new InventoryC(new ItemRocketlauncher())));

            mFactory.AddBlueprint("Robot3",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC(0, GetRobot3Dict(), false))
                    .AddComponent(new PathFindingC(movementSpeed: 1))
                    .AddComponent(new SteeringC())
                    .AddComponent(new HealthC(25))
                    .AddComponent(new HitboxC(30, 30))
                    .AddComponent(new CorpseC("EnemyCorpse"))
                    .AddComponent(new AssassinatableC())
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new Enemy())
                    .AddComponent(new DropItemC("Robot3"))
                    .AddComponent(new InventoryC(new ItemLasergun())));

            mFactory.AddBlueprint("EnemyCorpse",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(0.9f, "entitys/employee1/dead")));

            #endregion

            #region Story
            mFactory.AddBlueprint("Story",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new HitboxC(64,64))
                    .AddComponent(new StoryC())
                    .AddComponent(new AnimationC(3, GetStoryBoxDict())));
            #endregion

            #region MapObjects
            mFactory.AddBlueprint("ItemStack",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AnimationC((float)0.8, GetItemStackDict()))
                    .AddComponent(new HitboxC(40, 40))
                    .AddComponent(new ItemStackC())
                    .AddComponent(new UserInteractableC()));

            mFactory.AddBlueprint("Wall",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new HitboxC(64, 64))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new CollisionImpenetrableC())
                    .AddComponent(new StaticObjectC()));

            mFactory.AddBlueprint("PlayerBlockade",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new PlayerBlockadeC())
                    .AddComponent(new HitboxC(64, 64))
                    .AddComponent(new StaticObjectC()));

            mFactory.AddBlueprint("Door",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(0, "mapobjects/door"))
                    .AddComponent(new HitboxC(64f, 64f))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new CollisionImpenetrableC())
                    .AddComponent(new DoorC(false))
                    .AddComponent(new StaticObjectC()));

            mFactory.AddBlueprint("Table",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(0.5f, "mapobjects/table"))
                    .AddComponent(new HitboxC(64f, 64f))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new StaticObjectC()));

            mFactory.AddBlueprint("Chair",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(0.5f, "mapobjects/table"))
                    .AddComponent(new HitboxC(64f, 64f))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new ChairC(false))
                    .AddComponent(new StaticObjectC()));

            mFactory.AddBlueprint("Chests",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(1, "mapobjects/chest"))
                    .AddComponent(new ChestC())
                    .AddComponent(new DropItemC("Chest"))
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new HitboxC(64f, 64f)));

            mFactory.AddBlueprint("Stairs",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(1, "mapobjects/stairs"))
                    .AddComponent(new StairC())
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new HitboxC(64f, 64f)));

            mFactory.AddBlueprint("Upgrader",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(1, "mapobjects/upgrade_station"))
                    .AddComponent(new UpgradeMachineC())
                    .AddComponent(new UserInteractableC())
                    .AddComponent(new HitboxC(64f, 64f)));

            mFactory.AddBlueprint("Coffee",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(1, "mapobjects/coffee"))
                    .AddComponent(new HitboxC(64f, 64f))
                    .AddComponent(new CoffeeC())
                    .AddComponent(new UserInteractableC()));

            mFactory.AddBlueprint("Switch",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new SimpleSpriteC(1, "mapobjects/switch"))
                    .AddComponent(new HitboxC(64f, 64f))
                    .AddComponent(new SwitchC(0,0, false, "", GetMapobjectsDict()))
                    .AddComponent(new UserInteractableC()));
            #endregion

            #region Bullets/Explosion
            mFactory.AddBlueprint("PistolBullet",
                () => new Entity()
                    .AddComponent(new SimpleSpriteC(-1, "items/projectiles/simple_bullet"))
                    .AddComponent(new SimpleBulletC(6,  3000))
                    .AddComponent(new HitboxC(20, 20))
                    .AddComponent(new TransformC()));

            mFactory.AddBlueprint("RiflelBullet",
                () => new Entity()
                    .AddComponent(new SimpleSpriteC(-1, "items/projectiles/simple_bullet"))
                    .AddComponent(new SimpleBulletC(8,  3000))
                    .AddComponent(new HitboxC(20, 20))
                    .AddComponent(new TransformC()));


            mFactory.AddBlueprint("MinigunBullet",
                () => new Entity()
                    .AddComponent(new SimpleSpriteC(-1, "items/projectiles/simple_bullet"))
                    .AddComponent(new SimpleBulletC(15,  3000))
                    .AddComponent(new HitboxC(20, 20))
                    .AddComponent(new TransformC()));

            mFactory.AddBlueprint("RocketBullet",
                () => new Entity()
                    .AddComponent(new SimpleSpriteC(-1, "items/projectiles/rocket_launcher_missile"))
                    .AddComponent(new HitboxC(20, 20))
                    .AddComponent(new RocketBulletC(6, 3000))
                    .AddComponent(new TransformC()));

            mFactory.AddBlueprint("Explosion",
                () => new Entity()
                    .AddComponent(new AnimationC(
                        -1,
                        new Dictionary<string, ComplexSprite>
                        {
                            {
                                "default_default",
                                new ComplexSprite("items/projectiles/rocket_explosion_set", new Vector2(64,64), 9, 100)
                            }
                        }))
                    .AddComponent(new ExplosionC())
                    .AddComponent(new HitboxC(60,60))
                    .AddComponent(new TransformC()));

            mFactory.AddBlueprint("LaserBullet",
                () => new Entity()
                    .AddComponent(new SimpleSpriteC(-1, "items/projectiles/lasergun_shot"))
                    .AddComponent(new HitboxC(0, 0, 0, 0))
                    .AddComponent(new LaserBulletC())
                    .AddComponent(new TransformC()));

            mFactory.AddBlueprint("TurretLaserBullet",
                () => new Entity()
                    .AddComponent(new SimpleSpriteC(-1, "items/projectiles/turretLasergun_shot"))
                    .AddComponent(new HitboxC(0, 0, 0, 0))
                    .AddComponent(new LaserBulletC(6, 500, 1000))
                    .AddComponent(new TransformC()));
            #endregion

            #region Boss_Components
            mFactory.AddBlueprint("AutoTurret",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new AutoTurretC())
                    .AddComponent(new HitboxC(20, 20))
                    .AddComponent(new HealthC(10))
                    .AddComponent(new AnimationC(0, GetAutoTurretDict(), false))
                    .AddComponent(new InventoryC(1))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new Enemy()));

            mFactory.AddBlueprint("LaserTurret",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new LaserTurretC())
                    .AddComponent(new HitboxC(20,20))
                    .AddComponent(new HealthC(20, true))
                    .AddComponent(new AnimationC(0, GetLaserTurretDict()))
                    .AddComponent(new InventoryC(new ItemTurretLasergun(OffsetType.Turret)))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new Enemy()));

            mFactory.AddBlueprint("ElectricFloor",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new HitboxC(128, 128))
                    .AddComponent(new ElectricFloorC())
                    .AddComponent(new AnimationC(1, GetElectricFloorDict())));

            mFactory.AddBlueprint("EnemySpawnPoint",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new EnemySpawnC())
                    .AddComponent(new SimpleSpriteC(1, "entitys/boss/enemySpawnpoint")));

            mFactory.AddBlueprint("TurretSpawnPoint",
                () => new Entity()
                    .AddComponent(new TransformC())
                    .AddComponent(new HitboxC(64, 64))
                    .AddComponent(new TurretSpawnC()));

            mFactory.AddBlueprint("Boss",
                () => new Entity()
                    .AddComponent(new BossKiC())
                    .AddComponent(new HealthC(1000, true))
                    .AddComponent(new TransformC())
                    .AddComponent(new HitboxC(128, 128))
                    .AddComponent(new AnimationC(0, new Dictionary<string, ComplexSprite>
                    {
                        {"default_default",
                            new ComplexSprite("entitys/boss/default_default", new Vector2(128)) },
                        {"default_shielded",
                            new ComplexSprite("entitys/boss/default_shielded", new Vector2(128))}
                    }))
                    .AddComponent(new CollisionInaccessibleC())
                    .AddComponent(new StaticObjectC()));
            #endregion
        }

        #region Enemy_Anim_Dicts
        private static Dictionary<string, ComplexSprite> GetWorker1Dict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite(
                        "entitys/employee2/pistol_walk",
                        sourceRectangle: new Rectangle(0, 0, 64, 64))
                },

                {
                    "default_walk",
                    new ComplexSprite("entitys/employee2/pistol_walk",
                        new Vector2(64),
                        4,
                        500)
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetWorker2Dict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite(
                        "entitys/employee1/rifle_walk",
                        sourceRectangle: new Rectangle(0, 0, 64, 64))
                },

                {
                    "default_walk",
                    new ComplexSprite("entitys/employee1/rifle_walk",
                        new Vector2(64),
                        4,
                        500)
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetRobot1Dict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite(
                        "entitys/roboter1/robot1",
                        sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "default_walk",
                    new ComplexSprite("entitys/roboter1/robot1",
                        new Vector2(64),
                        1,
                        500)
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetRobot2Dict()
        {
            return new Dictionary<string, ComplexSprite>
            {

                {
                    "default_default",
                    new ComplexSprite(
                        "entitys/roboter2/robot2",
                        sourceRectangle: new Rectangle(0, 0, 64, 64))
                },

                {
                    "default_walk",
                    new ComplexSprite("entitys/roboter2/robot2",
                        new Vector2(64),
                        1,
                        500)
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetRobot3Dict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite(
                        "entitys/roboter3/robot3",
                        sourceRectangle: new Rectangle(0, 0, 64, 64))
                },

                {
                    "default_walk",
                    new ComplexSprite("entitys/roboter3/robot3",
                        new Vector2(64),
                        1,
                        500)
                }
            };
        }
        #endregion

        private static Dictionary<string, ComplexSprite> GetPlayerDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                // coffeecan
                {
                    "coffee_default",
                    new ComplexSprite("entitys/kevin/coffee_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "coffee_walk",
                    new ComplexSprite("entitys/kevin/coffee_walk", new Vector2(64), 4, 500)
                },
                {
                    "coffee_use",
                    new ComplexSprite("entitys/kevin/coffee_use", new Vector2(64), 5, 500, animationUninterruptible: true)
                },

                // default
                {
                    "default_default",
                    new ComplexSprite("entitys/kevin/default_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "default_walk",
                    new ComplexSprite("entitys/kevin/default_walk", new Vector2(64), 4, 500)
                },
                {
                    "default_use",
                    new ComplexSprite("entitys/kevin/default_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },

                // car
                {
                    "car_default",
                    new ComplexSprite("entitys/kevin/default_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "car_walk",
                    new ComplexSprite("entitys/kevin/default_walk", new Vector2(64), 4, 500)
                },
                {
                    "car_use",
                    new ComplexSprite("entitys/kevin/default_walk", new Vector2(64), 4, 500, animationUninterruptible: true)
                },

                // drone
                {
                    "drone_default",
                    new ComplexSprite("entitys/kevin/default_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "drone_walk",
                    new ComplexSprite("entitys/kevin/default_walk", new Vector2(64), 4, 500)
                },
                {
                    "drone_use",
                    new ComplexSprite("entitys/kevin/default_walk", new Vector2(64), 4, 500)
                },

                // lasergun
                {
                    "lasergun_default",
                    new ComplexSprite("entitys/kevin/lasergun_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "lasergun_walk",
                    new ComplexSprite("entitys/kevin/lasergun_walk", new Vector2(64), 4, 500)
                },
                {
                    "lasergun_use",
                    new ComplexSprite("entitys/kevin/lasergun_use", new Vector2(64), 2, 50, animationUninterruptible: true)
                },

                // minigun
                {
                    "minigun_default",
                    new ComplexSprite("entitys/kevin/minigun_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "minigun_walk",
                    new ComplexSprite("entitys/kevin/minigun_walk", new Vector2(64), 4, 500)
                },
                {
                    "minigun_use",
                    new ComplexSprite("entitys/kevin/minigun_use", new Vector2(64), 2, 50, animationUninterruptible: true)
                },

                // pistol
                {
                    "pistol_default",
                    new ComplexSprite("entitys/kevin/pistol_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "pistol_walk",
                    new ComplexSprite("entitys/kevin/pistol_walk", new Vector2(64), 4, 500)
                },
                {
                    "pistol_use",
                    new ComplexSprite("entitys/kevin/pistol_use", new Vector2(64), 2, 100, animationUninterruptible: true)
                },

                // rifle
                {
                    "rifle_default",
                    new ComplexSprite("entitys/kevin/rifle_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "rifle_walk",
                    new ComplexSprite("entitys/kevin/rifle_walk", new Vector2(64), 4, 500)
                },
                {
                    "rifle_use",
                    new ComplexSprite("entitys/kevin/rifle_use", new Vector2(64), 2, 100, animationUninterruptible: true)
                },

                // rocketlauncher
                {
                    "rocketlauncher_default",
                    new ComplexSprite("entitys/kevin/rocketlauncher_walk", sourceRectangle: new Rectangle(0, 0, 64, 64))
                },
                {
                    "rocketlauncher_walk",
                    new ComplexSprite("entitys/kevin/rocketlauncher_walk", new Vector2(64), 4, 500)
                },
                {
                    "rocketlauncher_use",
                    new ComplexSprite("entitys/kevin/rocketlauncher_use", new Vector2(64), 2, 100, animationUninterruptible: true)
                }

            };
        }

        private static Dictionary<string, ComplexSprite> GetDroneDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite("entitys/drone/drone_walk", new Vector2(64), 2, 500)
                },
                {
                    "default_walk",
                    new ComplexSprite("entitys/drone/drone_walk", new Vector2(64), 2, 500)
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetItemStackDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite("test_sprites/brown_pixel")
                },
                {
                    "coffee_default",
                    new ComplexSprite("items/icons/coffee_icon", new Vector2(32, 32))
                },
                {
                    "lasergun_default",
                    new ComplexSprite("items/icons/lasergun_icon")
                },
                {
                    "lasergun2_default",
                    new ComplexSprite("items/icons/lasergun2_icon")
                },
                {
                    "lasergun3_default",
                    new ComplexSprite("items/icons/lasergun3_icon")
                },
                {
                    "minigun_default",
                    new ComplexSprite("items/icons/minigun_icon")
                },
                {
                    "minigun2_default",
                    new ComplexSprite("items/icons/minigun2_icon")
                },
                {
                    "minigun3_default",
                    new ComplexSprite("items/icons/minigun3_icon")
                },
                {
                    "pistol_default",
                    new ComplexSprite("items/icons/pistol_icon")
                },
                {
                    "pistol2_default",
                    new ComplexSprite("items/icons/pistol2_icon")
                },
                {
                    "pistol3_default",
                    new ComplexSprite("items/icons/pistol3_icon")
                },
                {
                    "rifle_default",
                    new ComplexSprite("items/icons/rifle_icon")
                },
                {
                    "rifle2_default",
                    new ComplexSprite("items/icons/rifle2_icon")
                },
                {
                    "rifle3_default",
                    new ComplexSprite("items/icons/rifle3_icon")
                },
                {
                    "rocketlauncher_default",
                    new ComplexSprite("items/icons/rocketlauncher_icon")
                },
                {
                    "rocketlauncher2_default",
                    new ComplexSprite("items/icons/rocketlauncher2_icon")
                },
                {
                    "rocketlauncher3_default",
                    new ComplexSprite("items/icons/rocketlauncher3_icon")
                },
                {
                    "car_default",
                    new ComplexSprite("items/icons/car_icon")
                },
                {
                    "drone_default",
                    new ComplexSprite("items/icons/drone_icon")
                }
            };
        }
        
        private static Dictionary<string, ComplexSprite> GetMapobjectsDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite("mapobjects/door")
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetAutoTurretDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite("entitys/boss/turret/default_default")
                },
                {
                    "default_use",
                    new ComplexSprite("entitys/boss/turret/default_use")
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetLaserTurretDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "turretLasergun_default",
                    new ComplexSprite("entitys/boss/laserTurret/default_default")
                },
                {
                    "turretLasergun_charging",
                    new ComplexSprite("entitys/boss/laserTurret/default_charging", new Vector2(64), 3, 670, animationUninterruptible: true)
                },
                {
                    "turretLasergun_use",
                    new ComplexSprite("entitys/boss/laserTurret/default_use", new Vector2(64), 1, 1000, animationUninterruptible: true)
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetElectricFloorDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite("entitys/boss/floor/default_default", new Vector2(128))
                },
                {
                    "default_charging",
                    new ComplexSprite("entitys/boss/floor/default_charging", new Vector2(128))
                },
                {
                    "default_electric",
                    new ComplexSprite("entitys/boss/floor/default_electric", new Vector2(128))
                }
            };
        }

        private static Dictionary<string, ComplexSprite> GetStoryBoxDict()
        {
            return new Dictionary<string, ComplexSprite>
            {
                {
                    "default_default",
                    new ComplexSprite("storyboxes/StoryBox_Default")
                },
                {
                    "default_Intro",
                    new ComplexSprite("storyboxes/StoryBox_Intro", new Vector2(578, 152), 11, 4000, new Vector2(318, 84), new Vector2(460, -200), true)
                },
                {
                    "default_table",
                    new ComplexSprite("storyboxes/StoryBox_Table", new Vector2(578, 152), 5, 4000, new Vector2(318, 84),   new Vector2(-224, 12), true)
                },
                {
                    "default_chair",
                    new ComplexSprite("storyboxes/StoryBox_Chair", new Vector2(578, 152), 3, 4000, new Vector2(318, 84),   new Vector2( -100, 150), true)
                },
                {
                    "default_Switch",
                    new ComplexSprite("storyboxes/StoryBox_Switch", new Vector2(578, 152), 5, 4000, new Vector2(318, 84),   new Vector2( 240, -162), true)
                },
                {
                    "default_coffee",
                    new ComplexSprite("storyboxes/StoryBox_Coffee", new Vector2(578, 152), 9, 4000, new Vector2(318, 84),   new Vector2( 150, 0), true)
                },
                {
                    "default_stairs",
                    new ComplexSprite("storyboxes/StoryBox_Stairs", new Vector2(578, 152), 3, 4000, new Vector2(318, 84),   new Vector2( 140, -100), true)
                },
                {
                    "default_fight",
                    new ComplexSprite("storyboxes/StoryBox_Fight", new Vector2(578, 152), 2, 2000, new Vector2(318, 84),   new Vector2( 100, -250), true)
                },
                {
                    "default_stealthattack",
                    new ComplexSprite("storyboxes/StoryBox_Stealth", new Vector2(578, 152), 4, 4000, new Vector2(318, 84),   new Vector2( -220, 20), true)
                },
                {
                    "default_drone",
                    new ComplexSprite("storyboxes/StoryBox_Drone", new Vector2(578, 152), 4, 4000, new Vector2(318, 84),   new Vector2( 460, -200), true)
                }

            };
        }
    }
}