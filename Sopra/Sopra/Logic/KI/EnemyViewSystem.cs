using System;
using System.Linq;
using Sopra.ECS;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.RemoteControlled;

namespace Sopra.Logic.KI
{
    internal sealed class EnemyViewSystem : IntervalEntitySystem
    {
        private Entity mPlayer;
        private Subscription mPlayerSusbcription;
        private readonly Template mWallTemplate = Template.All(typeof(CollisionImpenetrableC)).Build();

        public EnemyViewSystem()
            : base(Template.All(typeof(Enemy), typeof(TransformC)), 50)
        {
        }

        internal override void SetEngine(Engine engine)
        {
            base.SetEngine(engine);
            mPlayerSusbcription = new Subscription(engine, Template.All(typeof(UserControllableC)).One(typeof(CarC), typeof(KevinC)).Build());
        }

        protected override void ProcessInterval()
        {
            mPlayer = mPlayerSusbcription.GetEntites().FirstOrDefault();

            if (mPlayer == null)
            {
                return;
            }

            foreach (var entity in GetEntities())
            {
                Process(entity);
            }
        }

        private void Process(Entity entity)
        {
            var enemy = entity.GetComponent<Enemy>(Enemy.Type);

            var transform = entity.GetComponent<TransformC>(TransformC.Type);
            var playerTransform = mPlayer.GetComponent<TransformC>();
            var distance = playerTransform.CurrentPosition - transform.CurrentPosition;

            if (distance.Length() > enemy.SightDistance)
            {
                return;
            }

            var wallsBetween = mEngine.Collision.GetCollidingEntities(
                playerTransform.CurrentPosition,
                transform.CurrentPosition,
                mWallTemplate);

            if (wallsBetween.Count > 0)
            {
                return;
            }

            distance.Normalize();

            var distAng = Math.Atan2(distance.Y, distance.X) + 0.5 * Math.PI;
            if (distAng < 0)
            {
                distAng += 2 * Math.PI;
            }

            var angDiff = Math.Abs(transform.RotationRadians - distAng);
            if (angDiff > Math.PI)
            {
                angDiff = 2 * Math.PI - angDiff;
            }

            if (angDiff < 1.2)
            {
                enemy.SetAgressiv(entity, playerTransform.CurrentPosition, mPlayer.HasComponent<CarC>());
            }
        }
    }
}