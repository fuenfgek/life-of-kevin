using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.ECS
{
    /// <summary>
    /// Base class for systems processing a set of entites.
    /// Use <see cref="Template"/> to filter the entities.
    /// </summary>
    /// <inheritdoc cref="System"/>
    /// <author>Michael Fleig</author>
    public abstract class IteratingEntitySystem : EntitySystem
    {
        
        protected IteratingEntitySystem(Template template) : base(template)
        {
        }

        protected IteratingEntitySystem(TemplateBuilder builder) : base(builder)
        {
        }

        /// <summary>
        /// Call <see cref="Process"/> for every entiy mathing the systems template.
        /// </summary>
        /// <inheritdoc cref="System.ProcessSystem"/>
        /// <param name="time"></param>
        public override void ProcessSystem(GameTime time)
        {
            foreach (var entity in GetEntities())
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
        public abstract void Process(Entity entity, GameTime time);
    }
}