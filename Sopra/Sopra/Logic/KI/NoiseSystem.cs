using System.Linq;
using Sopra.ECS;
using Sopra.Logic.Collision;

namespace Sopra.Logic.KI
{
    internal sealed class NoiseSystem : IntervalEntitySystem
    {
        internal const int SoundRange = 400;
        private static readonly HitboxC sSoundHitbox = new HitboxC(SoundRange * 2, SoundRange * 2);

        private readonly Template mEnemyTemplate = Template.All(typeof(Enemy)).Build();

        private Subscription mEnemySubscription;

        internal NoiseSystem()
            : base(Template.All(typeof(Enemy), typeof(TransformC)), 50)
        {
            Events.Instance.Subscribe<NoiseEvent>(OnNoise);
        }

        internal override void SetEngine(Engine engine)
        {
            base.SetEngine(engine);
            mEnemySubscription = new Subscription(engine, mEnemyTemplate);
        }

        private void OnNoise(NoiseEvent e)
        {
            var enemies = mEngine.Collision.GetCollidingEntities(sSoundHitbox, e.Position, mEnemyTemplate);

            foreach (var enemy in enemies)
            {
                if (enemy.Id == e.CausingEntityId)
                {
                    continue;
                }

                var enemyPos = enemy.GetComponent<TransformC>(TransformC.Type).CurrentPosition;

                if (!((enemyPos - e.Position).Length() < SoundRange)
                    || !(mEngine.PathFinder.GetPathLength(enemyPos, e.Position) <= SoundRange))
                {
                    return;
                }

                enemy.GetComponent<Enemy>().IncreaseTriggerLevel(e.Volume, e.Position);
            }
        }

        protected override void ProcessInterval()
        {
            var enemys = mEnemySubscription.GetEntites();

            var enumerable = enemys as Entity[] ?? enemys.ToArray();
            if (!enumerable.Any())
            {
                return;
            }

            foreach (var enemy in enumerable)
            {
                enemy.GetComponent<Enemy>(Enemy.Type)
                    ?.IncreaseTriggerLevel(
                        -0.01f * mInterval,
                        enemy.GetComponent<TransformC>(TransformC.Type).CurrentPosition);
            }
        }

        
    }
}