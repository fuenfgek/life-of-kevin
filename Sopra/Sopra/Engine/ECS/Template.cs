using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopra.Engine.ECS
{
    /// <summary>
    /// Represents a set of <see cref="IComponent"/>.
    /// Templates are used to discribe which entites a <see cref="IteratingEntitySystem"/> should process.
    /// Templates should only be created by the <see cref="TemplateBuilder"/>.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Template
    {
        public static Template Empty { get; } = new TemplateBuilder().Build();

        private readonly HashSet<Type> mAllSet;
        private readonly HashSet<Type> mOneSet;
        private readonly HashSet<Type> mExclusionSet;

        internal Template(HashSet<Type> allSet, HashSet<Type> oneSet, HashSet<Type> exclusionSet)
        {
            mAllSet = allSet;
            mOneSet = oneSet;
            mExclusionSet = exclusionSet;
        }

        /// <summary>
        /// Check if an entity machtes the template.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Matches(Entity entity)
        {
            if (mAllSet.Count != 0 && !mAllSet.All(entity.HasComponent))
            {
                return false;
            }

            if (mOneSet.Count != 0 && !mOneSet.Any(entity.HasComponent))
            {
                return false;
            }

            if (mExclusionSet.Count != 0 && mExclusionSet.Any(entity.HasComponent))
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

        /// <summary>
        /// Create <see cref="TemplateBuilder"/>.
        /// </summary>
        /// <param name="types">An entity must not contain any of the given components.</param>
        /// <returns></returns>
        public static TemplateBuilder Exclude(params Type[] types)
        {
            return new TemplateBuilder().Exclude(types);
        }
    }
}