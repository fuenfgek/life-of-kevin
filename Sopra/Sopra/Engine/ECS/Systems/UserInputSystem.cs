using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sopra.Engine.ECS;
using Sopra.Engine.ECS.Components;
using Sopra.Engine.Input;

namespace Sopra.Engine.ECS.Systems
{
    /// <summary>
    /// Handle the input from the user, if he klicks into the window.
    /// Requires:
    ///     - UserControllableComponent
    /// Need one of:
    ///     - PathFindingComponent
    ///     - TransformComponent
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    sealed class UserInputSystem : IteratingEntitySystem
    {
        public UserInputSystem() : base(new TemplateBuilder()
                                .All(typeof(UserControllableC))
                                .One(typeof(PathFindingC),
                                     typeof(TransformC)))
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
            var inputManager = InputManager.Get();

            /* TODO: implement this behaviour
            klick in world:
                left click:
                    shoot
                right click:
                    pathComponent.TargetPosition = mouse position
                    pathComponent.HasMovingCommand = true
                    if entity was clicked
                        check if entity has UserInteractableComponent
                            save entity as target in PathFindingComponent
                        else
                            target in PathFindingComponent = null
            klick in inventory
                do stuff
            klick on pause button
                ignore
            */

            if (inputManager.RightClicked())
            {
                RightClickInWorld(entity);
            }
        }


        /// <summary>
        /// Try to set the position of the mouse as target for pathfinding.
        /// </summary>
        /// <param name="entity"></param>
        private void RightClickInWorld(Entity entity)
        {
            var inputManager = InputManager.Get();
            
            if (!entity.HasComponent(typeof(PathFindingC))) { return; }

            var pathComponent = (PathFindingC) entity.GetComponent(typeof(PathFindingC));

            // only for testing #Felix
            // replicates case, that user clicks on entity 0 (green box(healing))
            if (inputManager.KeyDown(Keys.H) && entity.Id == 0)
            {
                pathComponent.Target = mEngine.EntityManager.Get(2);
            }
            else
            {
                pathComponent.Target = null;
            }
            //-----------------------

            pathComponent.TargetPosition = inputManager.MouseState.Position.ToVector2();
            pathComponent.HasMovingCommand = true;
        }
    }
}