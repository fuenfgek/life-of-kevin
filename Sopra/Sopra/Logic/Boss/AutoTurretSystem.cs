using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Items;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// System for running auto turrets.
    /// Requires:
    ///     - AutoTurretC
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    public sealed class AutoTurretSystem : IteratingEntitySystem
    {
        private readonly Template mPlayerTemplate = Template.All(typeof(UserControllableC)).Build();
        private readonly Template mSightBlockingTemplate = Template.All(typeof(CollisionImpenetrableC)).Build();

        public AutoTurretSystem() :
            base(Template
                .All(typeof(AutoTurretC)))
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
            var turretC = entity.GetComponent<AutoTurretC>();
            var turretInvC = entity.GetComponent<InventoryC>();

            if (!turretC.Active)
            {
                turretInvC.GetActiveItem()?.StopUsingItem();
                return;
            }

            var turretTransC = entity.GetComponent<TransformC>();

            var target = UpdateTarget(turretC, turretTransC.CurrentPosition);

            if (target == null)
            {
                turretInvC.GetActiveItem()?.StopUsingItem();
                return;
            }

            var targetTransC = target.GetComponent<TransformC>();
            
            turretTransC.SetRotation(targetTransC.CurrentPosition - turretTransC.CurrentPosition);

            turretInvC.GetActiveItem()?.StartUsingItem();
        }


        private Entity UpdateTarget(AutoTurretC turretC, Vector2 turretPos)
        {
            var target = mEngine.EntityManager.Get(mPlayerTemplate).FirstOrDefault();

            if (target != null)
            {
                var targetPos = target.GetComponent<TransformC>().CurrentPosition;

                if ((targetPos - turretPos).Length() < turretC.Range)
                {
                    turretC.LineOfSight.Startpoint = turretPos;
                    turretC.LineOfSight.Endpoint = targetPos;
                    var col= mEngine.Collision.GetCollidingEntities(turretC.LineOfSight.Startpoint,
                        turretC.LineOfSight.Endpoint,
                        mSightBlockingTemplate);
                    if (!col.Any())
                    {
                        turretC.TargetId = target.Id;
                        return target;
                    }
                }
            }

            turretC.TargetId = 0;
            return null;
        }
    }
}