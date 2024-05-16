using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopra.ECS
{
    public delegate void EntityRemovedDelegate(Entity entity);

    public delegate void EntityAddedDelegate(Entity entity);

    public delegate void EntityModifiedDelegate(Entity entity);


    /// <summary>
    /// The EntityManager is resposible for all entites of the engine.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class EntityManager
    {
        public event EntityAddedDelegate EntityAdded;
        public event EntityRemovedDelegate EntityRemoved;
        public event EntityModifiedDelegate EntityModified;
        public event Action EntitiesCleared;

        internal Dictionary<int, Entity> Entities { get; }
        private readonly Engine mEngine;

        private readonly EntityOperationPool mOperationPool = new EntityOperationPool();
        private readonly List<EntityOperation> mOperations = new List<EntityOperation>();


        public EntityManager(Engine engine)
        {
            mEngine = engine;
            Entities = new Dictionary<int, Entity>();
        }

        /// <summary>
        /// Add a detached entity to the entity manager.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Add(Entity entity)
        {
            var operation = mOperationPool.Optain();
            operation.Entity = entity;
            operation.Type = EntityOperation.OperationType.Add;
            mOperations.Add(operation);
        }

        /// <summary>
        /// Remove an entity from the entity manager.
        /// The entity will get deleted when the current frame is finished.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Remove(Entity entity)
        {
            Remove(entity.Id);
        }

        /// <summary>
        /// Remove an entity from the entity manager.
        /// The entity will get deleted when the current frame is finished.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Remove(int id)
        {
            var operation = mOperationPool.Optain();
            operation.Entity = Get(id);
            operation.Type = EntityOperation.OperationType.Remove;
            mOperations.Add(operation);
        }

        /// <summary>
        /// Get an entity by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity with id or null if it does not exist.</returns>
        public Entity Get(int id)
        {
            Entity entity;
            Entities.TryGetValue(id, out entity);
            return entity;
        }

        /// <summary>
        /// Get all entities matching a given template.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public Entity[] Get(Template template)
        {
            return Entities
                .Select((pair, i) => pair.Value)
                .Where((entity, i) => template.Matches(entity))
                .ToArray();
        }

        /// <summary>
        /// Remove all entities.
        /// </summary>
        public void Clear()
        {
            Entities.Clear();
            EntitiesCleared?.Invoke();
        }

        internal void ProcessOperations()
        {
            foreach (var operation in mOperations)
            {
                switch (operation.Type)
                {
                    case EntityOperation.OperationType.Add:
                        AddEntityInternal(operation.Entity);
                        break;
                    case EntityOperation.OperationType.Remove:
                        RemoveEntityInternal(operation.Entity);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                mOperationPool.Free(operation);
            }
            mOperations.Clear();
        }

        private void RemoveEntityInternal(Entity entity)
        {
            Entities.Remove(entity.Id);
            EntityRemoved?.Invoke(entity);
        }

        private void AddEntityInternal(Entity entity)
        {
            Entities[entity.Id] = entity;
            entity.Attach(mEngine);
            EntityAdded?.Invoke(entity);
        }

        internal void ModifiedEntity(Entity entity)
        {
            EntityModified?.Invoke(entity);
        }

        private sealed class EntityOperation : IPoolable
        {
            internal enum OperationType
            {
                Add,
                Remove
            }

            internal OperationType Type { get; set; }
            internal Entity Entity { get; set; }

            public void Reset()
            {
                Entity = null;
            }
        }

        private sealed class EntityOperationPool : Pool<EntityOperation>
        {
            protected override EntityOperation NewObject()
            {
                return new EntityOperation();
            }
        }
    }
}