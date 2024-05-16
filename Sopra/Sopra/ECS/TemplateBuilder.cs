using System;
using System.Collections.Generic;

namespace Sopra.ECS
{
    /// <summary>
    /// Builder for <see cref="Template"/>.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class TemplateBuilder
    {
        private readonly HashSet<Type> mAllSet = new HashSet<Type>();
        private readonly HashSet<Type> mOneSet = new HashSet<Type>();
        private readonly HashSet<Type> mExclusionSet = new HashSet<Type>();

        /// <param name="types">An entity will have to contain all given components.</param>
        /// <returns></returns>
        public TemplateBuilder All(params Type[] types)
        {
            foreach (var type in types)
            {
                mAllSet.Add(type);
            }

            return this;
        }

        /// <param name="types">An entity will have to contain at least one of the given components.</param>
        /// <returns></returns>
        public TemplateBuilder One(params Type[] types)
        {
            foreach (var type in types)
            {
                mOneSet.Add(type);
            }

            return this;
        }

        /// <param name="types">An entity must not contain any of the given components</param>
        /// <returns></returns>
        public TemplateBuilder Exclude(params Type[] types)
        {
            foreach (var type in types)
            {
                mExclusionSet.Add(type);
            }

            return this;
        }

        /// <summary>
        /// Create a <see cref="Template"/> instance with the specified requirements.
        /// </summary>
        /// <returns></returns>
        public Template Build()
        {
            return new Template(mAllSet, mOneSet, mExclusionSet);
        }
    }
}