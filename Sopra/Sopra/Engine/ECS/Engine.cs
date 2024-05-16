using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.ECS
{
    /// <summary>
    /// The Engine is responsible for 
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Engine
    {
        public EntityManager EntityManager { get; }
        private readonly List<System> mSystems;
        private readonly List<System> mRenderSystems;


        public Engine()
        {
            EntityManager = new EntityManager(this);
            mSystems = new List<System>();
            mRenderSystems = new List<System>();
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
            mSystems.ForEach(system => system.ProcessSystem(time));
        }

        /// <summary>
        /// Process all rendering systems.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="time"></param>
        public void Draw(SpriteBatch batch, GameTime time)
        {
            batch.Begin();
            
            mRenderSystems.ForEach(system => system.ProcessSystem(time));
            
            batch.End();
        }
    }
}