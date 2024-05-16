using System;
using System.Collections.Generic;

namespace Sopra.Engine.ECS
{
    /// <summary>
    /// The EntityManager is resposible for all entites of the engine.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class EntityManager
    {
        public Dictionary<int, Entity> Entities { get; }
        private Engine mEngine;
        private int mNextId;


        public EntityManager(Engine engine)
        {
            mEngine = engine;
            Entities = new Dictionary<int, Entity>();
        }


        /// <summary>
        /// Create a new entity with a unique ID and add to the entity manager.
        /// </summary>
        /// <returns>new enttiy</returns>
        public Entity Create()
        {
            var entity = new Entity(mEngine, mNextId++);
            Entities.Add(entity.Id, entity);
            return entity;
        }

        /// <summary>
        /// Add a detached entity to the entity manager.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityManager Add(Entity entity)
        {
            Entities[entity.Id] = entity;
            return this;
        }

        /// <summary>
        /// Remove an entity from the entity manager.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityManager Remove(Entity entity)
        {
            return Remove(entity.Id);
        }

        /// <summary>
        /// Remove an entity from the entity manager
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityManager Remove(int id)
        {
            Entities.Remove(id);
            return this;
        }

        /// <summary>
        /// Check if the entity manager contains the given entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Contains(Entity entity)
        {
            return Contains(entity.Id);
        }

        
        /// <summary>
        /// Check if the entity manager contains an entity with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(int id)
        {
            return Entities.ContainsKey(id);
        }

        public Entity Get(int id)
        {
            return Entities[id];
        }
    }
}