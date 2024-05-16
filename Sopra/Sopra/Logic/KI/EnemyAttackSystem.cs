using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Boss;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Items;

namespace Sopra.Logic.KI
{
    internal sealed class EnemyAttackSystem : IteratingEntitySystem
    {
        private readonly Template mWallTemplate = Template.All(typeof(CollisionImpenetrableC)).Build();
        private Subscription mPlayerSubscription;

        public EnemyAttackSystem()
            : base(Template.All(typeof(Enemy), typeof(InventoryC)).Exclude(typeof(LaserTurretC)))
        {
        }

        internal override void SetEngine(Engine engine)
        {
            base.SetEngine(engine);
            mPlayerSubscription = new Subscription(
                engine,
                Template.All(
                        typeof(TransformC),
                        typeof(UserControllableC))
                    .Build());
        }

        protected override void Process(Entity entity, GameTime time)
        {
            var transform = entity.GetComponent<TransformC>(TransformC.Type);
            var enemy = entity.GetComponent<Enemy>(Enemy.Type);
            var inventory = entity.GetComponent<InventoryC>(InventoryC.Type);

            inventory.GetActiveItem().StopUsingItem();
            
            if (enemy.Stance != EnemyStance.Aggresive)
            {
                if (transform.RotationLocked)
                {
                    transform.RotationLocked = false;
                }

                return;
            }

            var player = mPlayerSubscription.GetEntites().FirstOrDefault();

            if (player == null)
            {
                return;
            }

            var playerPosition = player.GetComponent<TransformC>().CurrentPosition;
            var distance = playerPosition - transform.CurrentPosition;

            if (distance.Length() > enemy.SightDistance)
            {
                if (transform.RotationLocked)
                {
                    transform.RotationLocked = false;
                }

                return;
            }

            var wallsBetween = mEngine.Collision.GetCollidingEntities(
                new HitboxC(playerPosition, transform.CurrentPosition),
                Vector2.Zero,
                mWallTemplate);

            if (wallsBetween.Count > 0)
            {
                if (transform.RotationLocked)
                {
                    transform.RotationLocked = false;
                }

                return;
            }

            transform.RotationLocked = false;
            transform.SetRotation(distance);
            transform.RotationLocked = true;

            inventory.GetActiveItem().StartUsingItem();
        }
    }
}