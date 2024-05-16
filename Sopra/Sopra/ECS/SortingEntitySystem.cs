using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Sopra.ECS
{
    /// <summary>
    /// Base class for systems processing a set of enties in a special order.
    /// Use <see cref="Template"/> to filter the entities.
    /// </summary>
    /// <inheritdoc cref="EntitySystem"/>
    /// <author>Felix Vogt</author>
    public abstract class SortingEntitySystem : EntitySystem
    {
        private readonly IComparer<Entity> mMyComparer;

        protected SortingEntitySystem(TemplateBuilder builder, IComparer<Entity> comparer) : base(builder)
        {
            mMyComparer = comparer;
        }

        /// <summary>
        /// Call <see cref="Process"/> for every entiy matching the systems template in the given order.
        /// </summary>
        /// <inheritdoc cref="System.ProcessSystem"/>
        /// <param name="time"></param>
        public override void ProcessSystem(GameTime time)
        {
            var entityList = GetEntities().ToList();
            entityList.Sort(mMyComparer);

            foreach (var entity in entityList)
            { 
                Process(entity, time);
            }
        }


        /// <summary>
        /// Process entity which matches the systems template.
        /// This method will be called automatically during the game loop in the update phase.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected abstract void Process(Entity entity, GameTime time);
    }
}