using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.KI
{
    public sealed class CallForHelpSystem : IteratingEntitySystem
    {
        private readonly Template mEnemyTemplate = Template.All(typeof(Enemy)).Build();
        private Subscription mEnemySubscription;

        public CallForHelpSystem()
            : base(Template.All(typeof(CallForHelp), typeof(Enemy)))
        {
        }

        internal override void SetEngine(Engine engine)
        {
            base.SetEngine(engine);
            mEnemySubscription = new Subscription(engine, mEnemyTemplate);
        }

        protected override void Process(Entity entity, GameTime time)
        {
            var position = entity.GetComponent<TransformC>().CurrentPosition;

            var enemies = mEnemySubscription
                .GetEntites();

            foreach (var enemy in enemies)
            {
                if (enemy.Id == entity.Id)
                {
                    continue;
                }

                var enemyPos = enemy.GetComponent<TransformC>(TransformC.Type).CurrentPosition;

                if (!((enemyPos - position).Length() < NoiseSystem.SoundRange)
                    || !(mEngine.PathFinder.GetPathLength(enemyPos, position) < NoiseSystem.SoundRange))
                {
                    continue;
                }

                enemy.GetComponent<Enemy>(Enemy.Type).SetAlerted(entity.GetComponent<CallForHelp>().TargetPos);
            }

            entity.RemoveComponent(typeof(CallForHelp));
        }
    }
}