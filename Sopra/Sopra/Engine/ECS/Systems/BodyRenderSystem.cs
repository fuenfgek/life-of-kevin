using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.ECS;
using Sopra.Engine.ECS.Components;

namespace Sopra.Engine.ECS.Systems
{
    /// <summary>
    /// System for rendering the body of an entity.
    /// Requires:
    ///     - BodyRenderComponent
    ///     - TransformComponent
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    sealed class BodyRenderSystem : IteratingEntitySystem
    {
        private readonly SpriteBatch mBatch;

        public BodyRenderSystem(SpriteBatch batch)
            : base(new TemplateBuilder().All(
                typeof(BodyRenderC),
                typeof(TransformC)))
        {
            this.mBatch = batch;
        }


        /// <summary>
        /// Process every entity which matches the systems template.
        /// This method will be called automatically during the update phase.
        /// </summary>
        /// <inheritdoc cref="IteratingEntitySystem.Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        public override void Process(Entity entity, GameTime time)
        {
            var bodyComoponent = entity.GetComponent<BodyRenderC>();
            var transformComponent = entity.GetComponent<TransformC>();

            mBatch.Draw(
                bodyComoponent.BodySprite,
                transformComponent.CurrentPosition,
                null,
                Color.White,
                0f,
                bodyComoponent.Origin,
                bodyComoponent.BodyScale,
                SpriteEffects.None, 0f);
        }
    }
}