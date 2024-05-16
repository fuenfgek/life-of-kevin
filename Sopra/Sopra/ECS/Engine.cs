using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sopra.Logic.Collision;
using Sopra.Logic.Pathfinding;
using Sopra.Maps;

namespace Sopra.ECS
{
    /// <summary>
    /// The Engine is responsible for 
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Engine
    {
        internal EntityManager EntityManager { get; }
        internal ContentManager Content { get; }
        internal EntityFactory EntityFactory { get; }
        internal Collisions Collision { get; private set; }
        internal PathFinder PathFinder { get; private set; }
        
        internal int CurrentLevelNumber { get; set; }
        
        private readonly List<System> mSystems;
        private readonly List<System> mRenderSystems;
        internal QuadTree mQuadTree;
        private readonly Subscription mSubscription;
        internal Matrix CameraMatrix { get; set; }
        private GameTime mGameTime;

        public Engine(ContentManager content, EntityFactory entityFactory)
        {
            EntityManager = new EntityManager(this);
            mSystems = new List<System>();
            mRenderSystems = new List<System>();
            mGameTime = new GameTime();
            mQuadTree = new QuadTree(0, new Rectangle(0, 0, 1920, 1920));
            Collision = new Collisions(mQuadTree);
            mSubscription = new Subscription(this, Template.All(typeof(HitboxC)).Build());
            Content = content;
            EntityFactory = entityFactory;
        }

        public void SetPathfinder(Map map)
        {
            mQuadTree = new QuadTree(0, new Rectangle(0, 0,  map.TmxMap.Width * map.TmxMap.TileWidth, map.TmxMap.Height * map.TmxMap.TileHeight));
            Collision = new Collisions(mQuadTree);
            PathFinder = new PathFinder(map.TmxMap, this);
        }

        /// <summary>
        /// Add a system which should be processed during the update phase.
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public Engine AddSystem(System system)
        {
            mSystems.Add(system);
            system.SetEngine(this);
            return this;
        }

        /// <summary>
        /// Add a system which should be processed during the render phase.
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public Engine AddRenderSystem(System system)
        {
            mRenderSystems.Add(system);
            system.SetEngine(this);
            return this;
        }
        
        /// <summary>
        /// Process all systems.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {   
            mQuadTree.Clear();
            foreach (var ent in mSubscription.GetEntites())
            {
                mQuadTree.Insert(new QuadObject(ent));
            }
            
            PathFinder.Update();
            
            mSystems.ForEach(system => system.ProcessSystem(time));

            EntityManager.ProcessOperations();

            mGameTime = time;
        }

        /// <summary>
        /// Process all rendering systems.
        /// </summary>
        public void Draw()
        {
            mRenderSystems.ForEach(system => system.ProcessSystem(mGameTime));
            mGameTime = new GameTime(mGameTime.TotalGameTime, new TimeSpan(0));
        }
    }
}