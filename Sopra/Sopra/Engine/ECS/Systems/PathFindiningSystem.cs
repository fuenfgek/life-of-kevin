using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.ECS.Components;


namespace Sopra.Engine.ECS.Systems
{
    /// <summary>
    /// Implements a very simple pathfinding.
    /// Requires:
    ///     - TransformComponent
    ///     - PathFindingComponent
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    sealed class PathFindingSystem : IteratingEntitySystem
    {
        private Entity mEntity;
        private TransformC mTransData;
        private PathFindingC mPathFindingData;

        public PathFindingSystem()
            : base(new TemplateBuilder().All(
                    typeof(TransformC),
                    typeof(PathFindingC)))
        {
        }


        /// <summary>
        /// Process every entity which matches the systems template.
        /// This method will be called automatically during the game loop in the update phase.
        /// </summary>
        /// <inheritdoc cref="IteratingEntitySystem.Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        public override void Process(Entity entity, GameTime time)
        {
            mPathFindingData = entity.GetComponent<PathFindingC>();

            if (!mPathFindingData.HasMovingCommand)
            {
                return;
            }

            mEntity = entity;
            mTransData = entity.GetComponent<TransformC>();

            MakeStep();
        }


        /// <summary>
        /// Should be called once the given destination is reached.
        /// If the user gave an entity he can interact with as target, he will now interact with that.
        /// </summary>
        private void ArriveDestination()
        {
            mPathFindingData.HasMovingCommand = false;

            if (mPathFindingData.Target == null)
            {
                return;
            }

            mPathFindingData.Target.GetComponent<UserInteractableC>().InteractingChar = mEntity;
        }


        /// <summary>
        /// Move an entity closer to a given target position.
        /// The direction in which the entity is moving is the direct line between its position and the target position.
        /// One step can't be further than the stored speed in the PathFindingComponent.
        /// This function will later be replaced with a better pathfinding algorithm.
        /// </summary>
        private void MakeStep()
        {
            var beeline = mPathFindingData.TargetPosition - mTransData.CurrentPosition;
            var movingDirection = beeline;
            movingDirection.Normalize();

            if (beeline.Length() > mPathFindingData.MovementSpeed)
            {
                mTransData.CurrentPosition += movingDirection * mPathFindingData.MovementSpeed;
            }
            else
            {
                mTransData.CurrentPosition += beeline;
                ArriveDestination();
            }
        }
    }
}