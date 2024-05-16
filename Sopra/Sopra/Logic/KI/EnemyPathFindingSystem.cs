using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Pathfinding;

namespace Sopra.Logic.KI
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    public sealed class EnemyPathFindingSystem : IteratingEntitySystem
    {
        public const int TimeBetweenCalcs = 500;
        private const int DistanceDiffForNewCalc = 5;

        public EnemyPathFindingSystem()
            : base(Template
                .All(typeof(Enemy), typeof(PathFindingC)))
        {
        }

        protected override void Process(Entity entity, GameTime time)
        {
            var enemyC = entity.GetComponent<Enemy>(Enemy.Type);
            if (enemyC.Stance != EnemyStance.Idle)
            {
                CheckForNewCommand(entity, enemyC, time);

                if (!entity.HasComponent(PatrollingEnemyC.Type))
                {
                    return;
                }

                var patrollC = entity.GetComponent<PatrollingEnemyC>(PatrollingEnemyC.Type);
                if (patrollC.IsPatrolling)
                {
                    patrollC.IsPatrolling = false;
                }
                return;
            }

            if (entity.HasComponent(PatrollingEnemyC.Type))
            {
                ProcessPatrollingEnemy(entity, time);
            }
            else
            {
                ProcessStaticEnemy(entity, enemyC, time);
            }
        }

        /// <summary>
        /// Update the path finding target for a non patrolling enemy.
        /// The enemy is always trying to return to his spawn position.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="enemyC"></param>
        /// <param name="time"></param>
        private static void ProcessStaticEnemy(Entity entity, Enemy enemyC, GameTime time)
        {
            var pathC = entity.GetComponent<PathFindingC>(PathFindingC.Type);

            pathC.PassedTime += time.ElapsedGameTime.Milliseconds;
            if (pathC.PassedTime < TimeBetweenCalcs)
            {
                return;
            }

            var transformC = entity.GetComponent<TransformC>(TransformC.Type);

            if ((transformC.CurrentPosition - enemyC.DesiredPos).Length() < DistanceDiffForNewCalc)
            {
                return;
            }
            
            pathC.SetNewTarget(enemyC.DesiredPos);
            pathC.PassedTime = 0;
        }


        /// <summary>
        /// Update the path finding target for a patrolling enemy.
        /// The enemy is always trying to reach the next point on his patrol path.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        private static void ProcessPatrollingEnemy(Entity entity, GameTime time)
        {
            var pathC = entity.GetComponent<PathFindingC>(PathFindingC.Type);
            var patrollC = entity.GetComponent<PatrollingEnemyC>(PatrollingEnemyC.Type);

            if (!patrollC.IsPatrolling)
            {
                patrollC.IsPatrolling = true;
                pathC.SetNewTarget(patrollC.PatrolPath[patrollC.PatrolStep]);
                return;
            }

            var transformC = entity.GetComponent<TransformC>(TransformC.Type);
            pathC.PassedTime += time.ElapsedGameTime.Milliseconds;
            if (pathC.PassedTime > TimeBetweenCalcs)
            {
                pathC.HasNewCommand = true;
                pathC.PassedTime = 0;
            }

            if ((transformC.CurrentPosition - patrollC.PatrolPath[patrollC.PatrolStep]).Length() > 30)
            {
                return;
            }

            patrollC.PatrolStep = (patrollC.PatrolStep + 1) % patrollC.PatrolPath.Count;
            pathC.SetNewTarget(patrollC.PatrolPath[patrollC.PatrolStep]);
        }


        /// <summary>
        /// Called when the enemys stance is != idle.
        /// Updates the position for the path finding system if the desired position of the enemy is different
        /// from the current target saved inside the path finding component.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="enemyC"></param>
        /// <param name="time"></param>
        private static void CheckForNewCommand(Entity entity, Enemy enemyC, GameTime time)
        {
            var pathC = entity.GetComponent<PathFindingC>(PathFindingC.Type);
            pathC.PassedTime += time.ElapsedGameTime.Milliseconds;

            if (pathC.PassedTime < TimeBetweenCalcs)
            {
                return;
            }

            var transformC = entity.GetComponent<TransformC>(TransformC.Type);

            if ((transformC.CurrentPosition - enemyC.DesiredPos).Length() < DistanceDiffForNewCalc)
            {
                return;
            }

            pathC.PassedTime = 0;
            pathC.SetNewTarget(enemyC.DesiredPos);
        }
    }
}
