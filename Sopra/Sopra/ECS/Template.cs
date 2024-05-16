using System;
using System.Collections.Generic;

namespace Sopra.ECS
{
    /// <summary>
    /// Represents a set of <see cref="IComponent"/>.
    /// Templates are used to discribe which entites a <see cref="IteratingEntitySystem"/> should process.
    /// Templates should only be created by the <see cref="TemplateBuilder"/>.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Template
    {
        
        private readonly Bits mAll;
        private readonly Bits mOne;
        private readonly Bits mExclude;

        internal Template(
            IEnumerable<Type> allSet,
            IEnumerable<Type> oneSet, 
            IEnumerable<Type> exclusionSet)
        {
            mAll = ToBits(allSet);
            mOne = ToBits(oneSet);
            mExclude = ToBits(exclusionSet);
        }

        private static Bits ToBits(IEnumerable<Type> components)
        {
            var bits = new Bits();
            
            foreach (var component in components)
            {
                bits.Set(ComponentType.GetIndex(component));
            }

            return bits;
        }

        /// <summary>
        /// Check if an entity machtes the template.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Matches(Entity entity)
        {
            var componentBits = entity.GetComponentBits();

            if (!componentBits.ContainsAll(mAll))
            {
                return false;
            }

            if (!mOne.IsEmpty() && !mOne.Intersects(componentBits))
            {
                return false;
            }

            if (!mExclude.IsEmpty() && mExclude.Intersects(componentBits))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create <see cref="TemplateBuilder"/>.
        /// </summary>
        /// <param name="types">An entity will have to contain all given components.</param>
        /// <returns>Templatebuilder</returns>
        public static TemplateBuilder All(params Type[] types)
        {
            return new TemplateBuilder().All(types);
        }

        /// <summary>
        /// Create <see cref="TemplateBuilder"/>.
        /// </summary>
        /// <param name="types">An entity will have to contain at least one of the given components.</param>
        /// <returns></returns>
        public static TemplateBuilder One(params Type[] types)
        {
            return new TemplateBuilder().One(types);
        }
    }
}