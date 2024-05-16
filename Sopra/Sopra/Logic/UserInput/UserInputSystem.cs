using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sopra.ECS;
using Sopra.Input;
using Sopra.Logic.Items;
using Sopra.Logic.Pathfinding;
using Sopra.Logic.RemoteControlled;
using Sopra.Logic.UserInteractable;


namespace Sopra.Logic.UserInput
{
    /// <summary>
    /// Handle the input from the user.
    /// Requires:
    ///     - UserControllableC
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Felix Vogt</author>
    internal sealed class UserInputSystem : IteratingEntitySystem
    {
        private LeftClickPressedE mLeftPressed;
        private LeftClickReleasedE mLeftReleased;
        private RightClickPressedeE mRightPressed;
        private RightClickReleasedE mRightReleased;
        private bool mLeftHeldDown;
        private bool mRightHeldDown;
        private int mMouseWheelDelta;
        private int mCommandPassedTime;

        public UserInputSystem() : base(new TemplateBuilder()
            .All(
                typeof(UserControllableC)))
        {
            Events.Instance.Subscribe<LeftClickPressedE>(OnLeftClickPressed);
            Events.Instance.Subscribe<LeftClickReleasedE>(OnLeftClickReleased);
            Events.Instance.Subscribe<RightClickPressedeE>(OnRightClickPressed);
            Events.Instance.Subscribe<RightClickReleasedE>(OnRightClickReleased);
            mMouseWheelDelta = Mouse.GetState().ScrollWheelValue;
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
            var inventoryC = entity.HasComponent<InventoryC>()
                ? entity.GetComponent<InventoryC>()
                : null;
            var activeItem = inventoryC?.GetActiveItem();
            var transformC = entity.GetComponent<TransformC>();


            var scrollValue = InputManager.Get().MouseState.ScrollWheelValue;
            if (inventoryC != null
                && mMouseWheelDelta != scrollValue)
            {
                inventoryC.ActiveSlot += -((scrollValue - mMouseWheelDelta) / 120);
                mMouseWheelDelta = scrollValue;
            }


            if (mRightPressed != null )
            {
                mRightHeldDown = true;
                mCommandPassedTime = 501;
                mRightPressed = null;
            }

            if (mRightHeldDown)
            {
                mCommandPassedTime += time.ElapsedGameTime.Milliseconds;
                if (mCommandPassedTime > 250)
                {
                    mCommandPassedTime = 0;
                    SendPathCommand(entity);
                }
            }

            if (mRightReleased != null)
            {
                mRightHeldDown = false;
                mRightReleased = null;
            }

            if (mLeftPressed != null)
            {
                mLeftHeldDown = true;
                activeItem?.StartUsingItem();
                mLeftPressed = null;
            }

            if (mLeftReleased != null)
            {
                mLeftHeldDown = false;
                transformC.RotationLocked = false;
                activeItem?.StopUsingItem();
                mLeftReleased = null;
            }

            if (mLeftHeldDown)
            {
                UpdatePlayerRotation(transformC);
            }
        }

        private void SendPathCommand(Entity entity)
        {
            var pathC = entity.GetComponent<PathFindingC>();
            var pos = InputManager.Get().MousePosInWorldCoord();
            pathC.SetNewTarget(pos);

            var clickedId = mEngine.Collision.GetClickedEntity(pos)
                                ?.Id ?? 0;
            var clickedEntity = mEngine.EntityManager.Get(clickedId);

            if (clickedEntity == null)
            {
                pathC.TargetId = 0;
                return;
            }

            if (!entity.HasComponent<InventoryC>()
                && !(entity.HasComponent<DroneC>() && clickedEntity.HasComponent<SwitchC>()))
            {
                return;
            }

            pathC.TargetId = clickedId;

            var targetEntity = mEngine.EntityManager.Get(pathC.TargetId);
            if (targetEntity.HasComponent<AssassinatableC>())
            {
                targetEntity.GetComponent<AssassinatableC>().IsFocusedBy = entity.Id;
            }
        }

        private void UpdatePlayerRotation(TransformC transformC)
        {
            transformC.RotationLocked = false;
            transformC.SetRotation(InputManager.Get().MousePosInWorldCoord() - transformC.CurrentPosition);
            transformC.RotationLocked = true;
        }

        private void OnLeftClickPressed(LeftClickPressedE e)
        {
            mLeftPressed = e;
        }

        private void OnLeftClickReleased(LeftClickReleasedE e)
        {
            mLeftReleased = e;
        }

        private void OnRightClickPressed(RightClickPressedeE e)
        {
            mRightPressed = e;
        }

        private void OnRightClickReleased(RightClickReleasedE e)
        {
            mRightReleased = e;
        }
    }
}