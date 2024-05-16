using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.ECS;
using Sopra.Logic.Items;

namespace Sopra.Logic.Render
{
    /// <summary>
    /// Is responsible for drawing all entitys.
    /// Requires:
    ///     - TransformComponent
    /// One of:
    ///     - SimpleSpriteC
    ///     - AnimationContainerC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="SortingEntitySystem"/>
    internal sealed class EntityDrawSystem : SortingEntitySystem
    {
        private readonly SpriteBatch mSpriteBatch;
        private Effect mEffect;
        private int mCounterEffect1;
        private int mCounterEffect2;


        public EntityDrawSystem(SpriteBatch batch)
            : base(new TemplateBuilder()
                    .All(typeof(TransformC))
                    .One(typeof(AnimationC),
                        typeof(SimpleSpriteC)),
                new SortBackToFront())
        {
            mSpriteBatch = batch;
        }


        /// <summary>
        /// Run the system.
        /// This method will be called automatically by the engine during the draw phase.
        /// </summary>
        /// <inheritdoc cref="IteratingEntitySystem.Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
            var transformC = entity.GetComponent<TransformC>(TransformC.Type);

            if (entity.HasComponent(SimpleSpriteC.Type))
            {
                var simpleSpriteC = entity.GetComponent<SimpleSpriteC>(SimpleSpriteC.Type);
                DrawSimpleSprite(simpleSpriteC, transformC);
                return;
            }

            var animationC = entity.GetComponent<AnimationC>(AnimationC.Type);

            if (entity.HasComponent(InventoryC.Type) && animationC.UpdateItemAnimation)
            {
                var name = entity.GetComponent<InventoryC>(InventoryC.Type).GetActiveItem()?.Name;
                animationC.CurrentItem = name ?? "default";
            }

            var complexSprite = GetCurrentComplexSprite(animationC);

            // change current complex sprite
            if (animationC.CurrentActivity != animationC.NextActivity
                && (!complexSprite.AnimationUninterruptible
                    || complexSprite.AnimationFinished))
            {
                complexSprite.CurrentFrame = 0;
                complexSprite.ElapsedTime = 0;
                complexSprite.AnimationFinished = false;

                animationC.CurrentActivity = animationC.NextActivity;
                complexSprite = GetCurrentComplexSprite(animationC);

                if (animationC.OnlyOnce)
                {
                    animationC.NextActivity = "default";
                    animationC.OnlyOnce = false;
                }
            }

            if (complexSprite.IsAnimated)
            {
                DrawAnimatedSprite(entity, complexSprite, animationC, transformC, time);
            }
            else
            {
                DrawSimpleSprite(entity, complexSprite, transformC);
                complexSprite.AnimationFinished = true;
            }
        }


        /// <summary>
        /// Draw a simple sprite.
        /// </summary>
        private void DrawSimpleSprite(SimpleSpriteC sprite, TransformC transformC)
        {
            mSpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                mEngine.CameraMatrix);
            
            mSpriteBatch.Draw(
                mEngine.Content.Load<Texture2D>(sprite.SpritePath),
                new Rectangle(transformC.CurrentPosition.ToPoint(), sprite.SpriteSize.ToPoint()),
                sprite.SourceRectangle,
                Color.White,
                transformC.RotationRadians,
                sprite.Origin,
                SpriteEffects.None,
                0);
            
            mSpriteBatch.End();
        }

        /// <summary>
        /// Draw a non animated complexSprite.
        /// </summary>
        private void DrawSimpleSprite(Entity entity, ComplexSprite complexSprite, TransformC transformC)
        {
            var texture = mEngine.Content.Load<Texture2D>(complexSprite.SpritePath);
            mEffect = mEngine.Content.Load<Effect>(@"effects/effect1");

            if (entity.GetComponent<AnimationC>(AnimationC.Type).EffectCheck && entity.HasComponent(AnimationC.Type))
            {
                mSpriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    mEngine.CameraMatrix);
                mEffect.CurrentTechnique.Passes[0].Apply();
                mSpriteBatch.Draw(
                    texture,
                    new Rectangle(transformC.CurrentPosition.ToPoint(), complexSprite.SpriteSize.ToPoint()),
                    complexSprite.SourceRectangle,
                    Color.White,
                    transformC.RotationRadians,
                    complexSprite.Origin,
                    SpriteEffects.None,
                    0);
                mSpriteBatch.End();
                if (mCounterEffect2 >= 10)
                {
                    entity.GetComponent<AnimationC>().EffectCheck = false;
                    mCounterEffect2 = 0;
                }
                else
                {
                    mCounterEffect2++;
                }
            }
            else
            {
                mSpriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    mEngine.CameraMatrix);
                mSpriteBatch.Draw(
                    texture,
                    new Rectangle(transformC.CurrentPosition.ToPoint(), complexSprite.SpriteSize.ToPoint()),
                    complexSprite.SourceRectangle,
                    Color.White,
                    transformC.RotationRadians,
                    complexSprite.Origin,
                    SpriteEffects.None,
                    0);
                mSpriteBatch.End();
            }
        }


        /// <summary>
        /// Draw and update an animated complexSprite.
        /// </summary>
        private void DrawAnimatedSprite(
            Entity entity,
            ComplexSprite complexSprite,
            AnimationC animationC,
            TransformC transformC,
            GameTime time)
        {
            complexSprite.ElapsedTime += time.ElapsedGameTime.Milliseconds;

            var sourceRectangle = new Rectangle(
                complexSprite.CurrentFrame % complexSprite.FramesPerRow * (int) complexSprite.FrameSize.X,
                (int) (Math.Floor(complexSprite.CurrentFrame / (float) complexSprite.FramesPerRow) *
                       complexSprite.FrameSize.Y),
                (int) complexSprite.FrameSize.X,
                (int) complexSprite.FrameSize.Y);

            var texture = mEngine.Content.Load<Texture2D>(complexSprite.SpritePath);

            mEffect = mEngine.Content.Load<Effect>(@"effects/effect1");

            if (animationC.EffectCheck)
            {
                mSpriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    mEngine.CameraMatrix);
                mEffect.CurrentTechnique.Passes[0].Apply();
                mSpriteBatch.Draw(texture,
                    new Rectangle(transformC.CurrentPosition.ToPoint(), complexSprite.SpriteSize.ToPoint()),
                    sourceRectangle,
                    Color.White,
                    transformC.RotationRadians,
                    complexSprite.Origin,
                    SpriteEffects.None,
                    0);
                mSpriteBatch.End();
                if (mCounterEffect1 >= 20)
                {
                    animationC.EffectCheck = false;
                    mCounterEffect1 = 0;
                }
                else
                {
                    mCounterEffect1++;
                }
            }
            else
            {
                mSpriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    mEngine.CameraMatrix);
                mSpriteBatch.Draw(texture,
                    new Rectangle(transformC.CurrentPosition.ToPoint(), complexSprite.SpriteSize.ToPoint()),
                    sourceRectangle,
                    Color.White,
                    transformC.RotationRadians,
                    complexSprite.Origin,
                    SpriteEffects.None,
                    0);
                mSpriteBatch.End();
            }

            if (complexSprite.ElapsedTime <= complexSprite.MillisecPerFrame)
            {
                return;
            }

            complexSprite.CurrentFrame++;
            complexSprite.ElapsedTime -= complexSprite.MillisecPerFrame;

            if (complexSprite.CurrentFrame != complexSprite.NumberOfFrames)
            {
                return;
            }

            complexSprite.AnimationFinished = true;
            complexSprite.CurrentFrame = 0;

            if (!animationC.SelfDestruct)
            {
                return;
            }

            mEngine.EntityManager.Remove(entity);
        }

        private static ComplexSprite GetCurrentComplexSprite(AnimationC animationC)
        {
            return animationC.SpriteDict[string.Join("_", animationC.CurrentItem, animationC.CurrentActivity)];
        }
    }
}