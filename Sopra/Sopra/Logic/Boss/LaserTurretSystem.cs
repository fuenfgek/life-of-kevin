using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Items;
using Sopra.Logic.Render;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// System for running auto turrets.
    /// Requires:
    ///     - LaserTurretC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    public sealed class LaserTurretSystem : IteratingEntitySystem
    {
        private readonly Template mPlayerTemplate = Template.All(typeof(KevinC)).Build();

        private const int TimeLocked = 1500;

        public LaserTurretSystem() :
            base(Template
                .All(typeof(LaserTurretC)))
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
            var turretC = entity.GetComponent<LaserTurretC>();
            var turretTransC = entity.GetComponent<TransformC>();

            var target = mEngine.EntityManager.Get(mPlayerTemplate).FirstOrDefault();

            if (target == null)
            {
                return;
            }

            var targetTransC = target.GetComponent<TransformC>();
            
            if (turretC.IsShooting)
            {
                turretC.PassedTime += time.ElapsedGameTime.Milliseconds;
                if (turretC.PassedTime <= ItemTurretLasergun.Lifetime)
                {
                    return;
                }
                turretC.PassedTime = 0;

                turretC.IsShooting = false;
                entity.GetComponent<InventoryC>().GetActiveItem().StopUsingItem();
                return;
            }

            if (turretC.IsLocked)
            {
                turretC.PassedTime += time.ElapsedGameTime.Milliseconds;
                if (turretC.PassedTime < TimeLocked)
                {
                    return;
                }
                turretC.PassedTime = 0;

                SoundManager.Instance.PlaySound("laser");
                turretC.IsLocked = false;
                turretC.IsShooting = true;
                entity.GetComponent<InventoryC>().GetActiveItem().StartUsingItem();
                return;
            }

            turretTransC.SetRotation(targetTransC.CurrentPosition - turretTransC.CurrentPosition);

            if (!turretC.AttackCommand)
            {
                return;
            }

            turretC.AttackCommand = false;

            if (!((turretTransC.CurrentPosition - targetTransC.CurrentPosition).Length() < ItemTurretLasergun.Range))
            {
                return;
            }

            turretC.IsLocked = true;
            var animC = entity.GetComponent<AnimationC>();
            animC.OnlyOnce = true;
            animC.ChangeAnimationActivity("charging");
        }
    }
}