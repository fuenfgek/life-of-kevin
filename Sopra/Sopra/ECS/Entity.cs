using System;
using System.Collections.Generic;

namespace Sopra.ECS
{
    /// <summary>
    /// An Entity represents an object in the game world.
    /// Entites serve as containers for components.
    /// </summary>
    /// <inheritdoc cref="IEquatable{T}"/>
    /// <author>Michael Fleig</author>
    public sealed class Entity : IEquatable<Entity>
    {
        private static int sIdCounter = 1;
        
        private readonly int mId;
        private EntityManager mEntityManager;
        private readonly Bag<IComponent> mComponents = new Bag<IComponent>(16);
        private readonly Bits mComponentBits = new Bits();

        public int Id => mId;

        public Entity()
        : this(sIdCounter)
        {
        }

        public Entity(int id)
        {
            mId = id;
            if (sIdCounter <= id)
            {
                sIdCounter = id + 1;
            }
        }

        /// <summary>
        /// Attach an <see cref="Engine"/> and its <see cref="EntityManager"/> to this entity.
        /// This method is called, when adding an existing entity to an <see cref="EntityManager"/>.
        /// Do not call this method yourself!
        /// </summary>
        /// <param name="engine"></param>
        internal void Attach(Engine engine)
        {
            mEntityManager = engine.EntityManager;
        }

        /// <summary>
        /// Add given component to the entity.
        /// If the entity already has a component of the given type it will overwrite the old one.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Entity AddComponent(IComponent component)
        {
            var index = ComponentType.GetIndex(component.GetType());
            mComponents.Set(index, component);
            mComponentBits.Set(index);
            mEntityManager?.ModifiedEntity(this);
            return this;
        }

        /// <summary>
        /// Remove the component of the given type from the entity.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void RemoveComponent(Type type)
        {
            var index = ComponentType.GetIndex(type);
            
            if (mComponentBits.GetAndClear(index))
            {
                mComponents.Set(index, null);
                mEntityManager?.ModifiedEntity(this);
            }
        }

        /// <summary>
        /// Check if entity has a component of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IComponent
        {
            return mComponentBits.Get(ComponentType.GetIndex<T>());
        }

        public bool HasComponent(ComponentType type)
        {
            return mComponentBits.Get(type.Index);
        }

        /// <summary>
        /// Get entity component by type.
        /// Return null, if the entity does not have the reqeusted component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : IComponent
        {
            return GetComponent<T>(ComponentType.Of<T>());
        }

        public T GetComponent<T>(ComponentType type)
        {
            if (mComponents.Capazity <= type.Index)
            {
                return default(T);
            }
            return (T) mComponents.Get(type.Index);
        }

        /// <summary>
        /// Get all components of the entity
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IComponent> GetComponents()
        {
            return mComponents.ToList();
        }

        public Bits GetComponentBits()
        {
            return mComponentBits;
        }

        public bool Equals(Entity other)
        {
            if (other == null) { return false;}

            return mId == other.Id;
        }
    }
}