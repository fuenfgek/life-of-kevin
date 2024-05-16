using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Pathfinding;

namespace Sopra.Logic.KI
{
    public sealed class AggressiveEnemySystem : IteratingEntitySystem
    {
        private Subscription mPlayerSubscription;

        public AggressiveEnemySystem()
            : base(Template.All(typeof(Enemy), typeof(TransformC), typeof(PathFindingC)))
        {
        }

        internal override void SetEngine(Engine engine)
        {
            base.SetEngine(engine);
            mPlayerSubscription = new Subscription(engine,
                Template.All(
                        typeof(UserControllableC),
                        typeof(TransformC))
                    .Build());
        }

        protected override void Process(Entity entity, GameTime time)
        {
            var enemy = entity.GetComponent<Enemy>(Enemy.Type);

            if (enemy.Stance != EnemyStance.Aggresive)
            {
                return;
            }

            var playerPosition = GetPlayerPosition();

            if (playerPosition == null)
            {
                return;
            }

            var transform = entity.GetComponent<TransformC>(TransformC.Type);

            if (mPlayerSubscription
                    .GetEntites()
                    .First()
                    .HasComponent<KevinC>()
                && enemy.IsAgressivOnCar)
            {
                enemy.Stance = EnemyStance.Alerted;
                enemy.SetAlerted(transform.CurrentPosition);
                enemy.IsAgressivOnCar = false;
                return;
            }

            var distance = playerPosition.Value - transform.CurrentPosition;
            distance.Normalize();

            enemy.DesiredPos = playerPosition.Value;
        }

        private Vector2? GetPlayerPosition()
        {
            return mPlayerSubscription
                .GetEntites()
                .FirstOrDefault()
                ?.GetComponent<TransformC>(TransformC.Type)
                .CurrentPosition;
        }
    }
}