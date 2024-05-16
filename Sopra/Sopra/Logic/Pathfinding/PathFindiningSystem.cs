using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.KI;
using Sopra.Logic.Render;
using Sopra.Logic.UserInteractable;

namespace Sopra.Logic.Pathfinding
{
    /// <summary>
    /// Implements pathfining.
    /// Requires:
    ///     - TransformComponent
    ///     - PathFindingComponent
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    internal sealed class PathFindingSystem : IteratingEntitySystem
    {
        private readonly Template[] mCollisionTemplates = {
            Template.All(typeof(CollisionInaccessibleC)).Build(),
            Template.One(typeof(CollisionInaccessibleC), typeof(CollisionPlayerBlockade)).Build(),
            Template.One(typeof(CollisionImpenetrableC), typeof(CollisionPlayerBlockade)).Build()
        };
        
        private readonly ComponentType mHitboxType = ComponentType.Of<HitboxC>();
        private readonly ComponentType mTransformType = ComponentType.Of<TransformC>();
        private readonly ComponentType mEnemyType = ComponentType.Of<Enemy>();
        private readonly ComponentType mUserControllableType = ComponentType.Of<UserControllableC>();
        private readonly ComponentType mAnimationType = ComponentType.Of<AnimationC>();
        private readonly ComponentType mSteeringType = ComponentType.Of<SteeringC>();

        private PathFinder mPathFinder;
        private readonly Steering mSteering;

        public PathFindingSystem()
            : base(Template.All(
                typeof(TransformC),
                typeof(PathFindingC)))
        {
            mSteering = new Steering();
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
            var pathC = entity.GetComponent<PathFindingC>();
            mPathFinder = mEngine.PathFinder;

            if (pathC.HasNewCommand)
            {
                pathC.HasNewCommand = false;
                pathC.PathStep = 0;
                pathC.CurrentPath = mPathFinder.GetPath(entity.GetComponent<TransformC>(mTransformType).CurrentPosition,
                    pathC.TargetPosition);
            }

            if (pathC.CurrentPath == null || pathC.CurrentPath.Count == 0)
            {
                return;
            }

            if (entity.HasComponent(mAnimationType))
            {
                var animC = entity.GetComponent<AnimationC>(mAnimationType);
                if (animC.CurrentActivity != "walk")
                {
                    animC.ChangeAnimationActivity("walk");
                }
            }

            MakeStep(entity, pathC, entity.GetComponent<TransformC>(mTransformType));
        }

        /// <summary>
        /// Move an entity closer to a given target position.
        /// The direction in which the entity is moving is the direct line between its position and the target position.
        /// One step can't be further than the stored speed in the PathFindingComponent.
        /// </summary>
        private void MakeStep(Entity entity, PathFindingC pathC, TransformC transformC)
        {
            if (pathC.PathStep == pathC.CurrentPath.Count)
            {
                ArriveDestination(entity, pathC);
                return;
            }

            /* If the first waypoint equals the current player position then the beeline is the zero vector.
             Normalizing the zero vector yields a vector with NaN entries. This causes the black screen error. */
            if (pathC.PathStep == 0 && Vector2Equals(pathC.CurrentPath[0], transformC.CurrentPosition))
            {
                pathC.PathStep++;
                return;
            }

            var beeline = pathC.CurrentPath[pathC.PathStep] - transformC.CurrentPosition;
            var movingDirection = beeline;
            movingDirection.Normalize();

            if (beeline.Length() > 0)
            {
                transformC.SetRotation(movingDirection);
            }

            var newPosition = beeline.Length() > pathC.MovementSpeed
                ? transformC.CurrentPosition + movingDirection * pathC.MovementSpeed
                : transformC.CurrentPosition + beeline;

            var steerC = entity.GetComponent<SteeringC>(mSteeringType);

            if (entity.HasComponent(mHitboxType))
            {
                var hitboxC = entity.GetComponent<HitboxC>(mHitboxType);
                
                var entities = mEngine.Collision.GetCollidingEntities(
                    hitboxC,
                    newPosition,
                    (Template) mCollisionTemplates.GetValue((int) pathC.CollisionNumber),
                    new[] {entity.Id});

                if (entities.Any())
                {
                    #region Steering
                    
                    var enemies =
                        (from e in entities
                            where e.HasComponent(mEnemyType) || e.HasComponent(mUserControllableType)
                            select e).ToList();

                    if (!steerC.Collision || enemies.Count > 1)
                    {
                        steerC.TurnLeft = !mSteering.CheckPossibleCollision(
                            transformC.CurrentPosition,
                            movingDirection,
                            mEngine,
                            true);

                        steerC.TurnRight = !mSteering.CheckPossibleCollision(
                            transformC.CurrentPosition,
                            movingDirection,
                            mEngine,
                            false);
                    }

					steerC.Collision = true;
                    if (enemies.Any() && !entity.HasComponent(mEnemyType))
                    {
                        var enemyPosition = enemies[0].GetComponent<TransformC>(mTransformType).CurrentPosition;
                        
                        // Prevent the player from spinning when close to an enemy.
                        if ((pathC.TargetPosition - enemyPosition).Length() < 64)
                        {
                            pathC.TargetId = 0;
                            ArriveDestination(entity, pathC);
                            return;
                        }

                        if (entity.HasComponent(mUserControllableType))
                        {
                            var fleeForce = Steering.Avoid(
                                steerC,
                                movingDirection);

                            newPosition = transformC.CurrentPosition + fleeForce * pathC.MovementSpeed;

                            /* This prevents the player to spin as well. If an enemy is near an waypoint,
                             then the player would steer away from it causing it to spin because the player
                             never reached the waypoint itself. */
                            if ((pathC.CurrentPath[pathC.PathStep] - transformC.CurrentPosition).Length()
                                < (newPosition - transformC.CurrentPosition).Length())
                            {
                                newPosition = beeline.Length() > pathC.MovementSpeed
                                    ? transformC.CurrentPosition + movingDirection * pathC.MovementSpeed
                                    : transformC.CurrentPosition + beeline;
                            }
                        }
                    }
                    #endregion

                    // When no enemy is nearby
                    else
                    {
                        pathC.TargetId = 0;
                        ArriveDestination(entity, pathC);
                        return;
                    }
                }

                else
                {
                    steerC.Collision = false;
                }

                transformC.CurrentPosition = newPosition;
            }
            else
            {
                transformC.CurrentPosition = newPosition;
            }

            if (transformC.CurrentPosition == pathC.CurrentPath[pathC.PathStep])
            {
                pathC.PathStep++;
            }
        }


        /// <summary>
        /// Should be called once the given destination is reached.
        /// If the user gave an entity he can interact with as target, he will now interact with that.
        /// </summary>
        private void ArriveDestination(Entity entity, PathFindingC pathFindingC)
        {
            pathFindingC.PathStep = 0;
            pathFindingC.CurrentPath = null;


            if (entity.HasComponent(mAnimationType))
            {
                entity.GetComponent<AnimationC>(mAnimationType).ChangeAnimationActivity("default");
            }

            if (pathFindingC.TargetId == 0)
            {
                return;
            }

            var targetEntity = mEngine.EntityManager.Get(pathFindingC.TargetId);
            pathFindingC.TargetId = 0;

            var interactableC = targetEntity?.GetComponent<UserInteractableC>(UserInteractableC.Type);
            if (interactableC != null)
            {
                interactableC.InteractingEntityId = entity.Id;                
            }
        }

        private static bool Vector2Equals(Vector2 vector, Vector2 anotherVector)
        {
            return Math.Abs(vector.X - anotherVector.X) < 0.1f && Math.Abs(vector.Y - anotherVector.Y) < 0.1f;
        }
    }
}