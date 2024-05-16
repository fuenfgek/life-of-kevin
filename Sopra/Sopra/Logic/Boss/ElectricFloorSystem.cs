using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Health;
using Sopra.Logic.Render;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// System for running electric floor tiles.
    /// Requires:
    ///     - ElectricFloorC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    public sealed class ElectricFloorSystem : IteratingEntitySystem
    {
        private SoundEffectInstance mBosswarning;
        private bool mOnlyOnce;
        private readonly Template mDamagableTemplate = Template.All(typeof(HealthC)).Build();

        public ElectricFloorSystem() :
            base(Template
                .All(typeof(ElectricFloorC)))
        {

        }

        /// <summary>
        /// Process every entity which matches the systems template.
        /// This method will be called automatically during the game loop in the update phase.
        /// </summary>
        /// <inheritdoc cref="IteratingEntitySystem.Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {
            var floorC = entity.GetComponent<ElectricFloorC>();

            if (floorC.Activ)
            {

                DealDamage(entity, floorC, time);
            }
            else if (floorC.Charging)
            {
                if (!mOnlyOnce)
                {
                    SoundManager.Instance.PlaySound("bosswarning", true, out mBosswarning);
                    mOnlyOnce = true;
                }         
                floorC.PassedTime += time.ElapsedGameTime.Milliseconds ;
                if (floorC.PassedTime > 1000)
                {
                 
                    floorC.Charging = false;
                    floorC.Activ = true;
                    mOnlyOnce = false;
                    floorC.PassedTime = 0;
                    SoundManager.Instance.StoploopSound(mBosswarning);
                    SoundManager.Instance.PlaySound("electricfield");
                }
            }

            entity.GetComponent<AnimationC>()
                .ChangeAnimationActivity(floorC.Activ
                    ? "electric"
                    : floorC.Charging
                        ? "charging"
                        : "default");
        }

        private void DealDamage(Entity floorEntity, ElectricFloorC floorC, GameTime time)
        {
           var coll = mEngine.Collision.GetCollidingEntities(floorEntity.GetComponent<HitboxC>(),
                floorEntity.GetComponent<TransformC>(),
                mDamagableTemplate);

            foreach (var ent in coll)
            {
                ent.GetComponent<HealthC>().ApplyDamage(ent, floorC.Damage, 0, Vector2.Zero, false);
            }

            floorC.PassedTime += time.ElapsedGameTime.Milliseconds;

            if (floorC.PassedTime <= 1000)
            {
                return;
            }

            floorC.PassedTime = 0;
            floorC.Activ = false;
        }
    }
}
