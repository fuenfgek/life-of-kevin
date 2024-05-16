using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopra.Engine.ECS
{
    /// <summary>
    /// An Entity represents an object in the game world.
    /// Entites serve as containers for components.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Entity
    {
        private readonly int mId;
        private readonly Engine mEngine;
        private readonly Dictionary<Type, IComponent> mComponents;

        public int Id => mId;

        public Entity(Engine engine, int id)
        {
            mEngine = engine;
            mId = id;
            mComponents = new Dictionary<Type, IComponent>();
        }

        /// <summary>
        /// Add given component to the entity.
        /// If the entity already has a component of the given type it will overwrite the old one.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Entity AddComponent(IComponent component)
        {
            mComponents[component.GetType()] = component;
            return this;
        }

        /// <summary>
        /// Remove the component of the given type from the entity.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Entity RemoveComponent(Type type)
        {
            mComponents.Remove(type);
            return this;
        }

        /// <summary>
        /// Check if entity has a component of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IComponent
        {
            return HasComponent(typeof(T));
        }

        /// <summary>
        /// Check if the entity has a component of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasComponent(Type type)
        {
            return mComponents.ContainsKey(type);
        }

        /// <summary>
        /// Get entity component by type.
        /// Return null, if the entity does not have the reqeusted component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : IComponent
        {
            return (T) mComponents[typeof(T)];
        }

        /// <summary>
        /// Get entity component by type.
        /// Return null, if the entity does not have the reqeusted component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IComponent GetComponent(Type type)
        {
            return mComponents[type];
        }

        /// <summary>
        /// Returns a list of all component types of the entity.
        /// </summary>
        /// <returns></returns>
        public List<Type> ComponentTypes()
        {
            return mComponents.Keys.ToList();
        }
        
    }
}