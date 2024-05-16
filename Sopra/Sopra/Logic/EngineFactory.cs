using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject.Activation;
using Sopra.ECS;
using Sopra.Logic.Achievements;
using Sopra.Logic.Boss;
using Sopra.Logic.Health;
using Sopra.Logic.Items;
using Sopra.Logic.Items.Projectiles;
using Sopra.Logic.KI;
using Sopra.Logic.Pathfinding;
using Sopra.Logic.RemoteControlled;
using Sopra.Logic.Render;
using Sopra.Logic.Stairs;
using Sopra.Logic.UserInput;
using Sopra.Logic.UserInteractable;
using Sopra.Maps.MapComponents;
using Sopra.Maps.Tutorial;

namespace Sopra.Logic
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class EngineFactory : IProvider<Engine>
    {
        private readonly SpriteBatch mSpriteBatch;
        private readonly ContentManager mContentManager;
        private readonly EntityFactory mEntityFactory;
        private readonly GraphicsDeviceManager mGraphics;

        public EngineFactory(SpriteBatch spriteBatch, ContentManager contentManager, EntityFactory entityFactory, GraphicsDeviceManager graphics)
        {
            mSpriteBatch = spriteBatch;
            mContentManager = contentManager;
            mEntityFactory = entityFactory;
            Type = typeof(EngineFactory);
            mGraphics = graphics;
        }

        public Type Type { get; }

        public object Create(IContext context)
        {
            var engine = new Engine(mContentManager, mEntityFactory);

            engine
                .AddSystem(new CameraSystem(mSpriteBatch, mGraphics))
                .AddSystem(new UserInputSystem())
                .AddSystem(new InventorySystem())
                .AddSystem(new AssassinatableSystem())
                .AddSystem(new ItemStackSystem())
                .AddSystem(new DropItemSystem())
                .AddSystem(new StairSystem())
                .AddSystem(new SwitchSystem())
                .AddSystem(new ChairSystem())
                .AddSystem(new CoffeeSystem())
                .AddSystem(new StorySystem())
                .AddSystem(new UpgradeSystem())
                .AddSystem(new ChestSystem())
                .AddSystem(new SimpleBulletSystem())
                .AddSystem(new RocketBulletSysstem())
                .AddSystem(new ExplosionSystem())
                .AddSystem(new LaserBulletSystem())
                .AddSystem(new CarSystem())
                .AddSystem(new DroneSystem())
                .AddSystem(new EnemyViewSystem())
                .AddSystem(new NoiseSystem())
                .AddSystem(new CallForHelpSystem())
                .AddSystem(new AggressiveEnemySystem())
                .AddSystem(new EnemyPathFindingSystem())
                .AddSystem(new EnemyAttackSystem())
                .AddSystem(new ElectricFloorSystem())
                .AddSystem(new BossKiSystem())
                .AddSystem(new CorpseSystem())
                .AddSystem(new AutoTurretSystem())  
                .AddSystem(new LaserTurretSystem())
                .AddSystem(new PathFindingSystem())
                .AddSystem(new ReinforcementSystem())
                .AddSystem(new HealthSystem())

                .AddRenderSystem(new TimerSystem())
                .AddRenderSystem(new EntityDrawSystem(mSpriteBatch));
           // engine.AddRenderSystem(new EnemyDebugSystem(mSpriteBatch));
           // engine.AddRenderSystem(new PathFindingDebugSystem(mSpriteBatch));
            //engine.AddRenderSystem(new HitboxDebugSystem(mSpriteBatch));
            return engine;
        }
    }
}