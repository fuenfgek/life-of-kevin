using System.Collections.Generic;
using System.Linq;

namespace Sopra.ECS
{
    /// <summary>
    /// Subscribe to a set of entites.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Subscription
    {
        private readonly Template mTemplate;
        private readonly Dictionary<int, Entity> mEntites;

        /// <summary>
        /// Create a new subscription.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="template"></param>
        public Subscription(Engine engine, Template template)
        {
            mTemplate = template;
            mEntites = new Dictionary<int, Entity>();

            engine.EntityManager.EntityModified += OnEntityModified;
            engine.EntityManager.EntityAdded += OnEntityAdded;
            engine.EntityManager.EntityRemoved += OnEntityRemoved;
            engine.EntityManager.EntitiesCleared += () => mEntites.Clear();

            foreach (var entity in engine.EntityManager.Get(template))
            {
                mEntites.Add(entity.Id, entity);
            }
        }

        private void OnEntityModified(Entity entity)
        {
            if (mTemplate.Matches(entity))
            {
                mEntites[entity.Id] = entity;
            }
            else if (mEntites.ContainsKey(entity.Id))
            {
                mEntites.Remove(entity.Id);
            }
        }

        private void OnEntityRemoved(Entity entitiy)
        {
            mEntites.Remove(entitiy.Id);
        }

        private void OnEntityAdded(Entity entity)
        {
            if (mTemplate.Matches(entity))
            {
                mEntites[entity.Id] = entity;
            }
        }

        /// <summary>
        /// Get all entites matching the template of the subscription.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> GetEntites()
        {
            return mEntites.Values.ToList();
        }
    }
}