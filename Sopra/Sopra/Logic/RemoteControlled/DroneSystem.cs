using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.UserInput;

namespace Sopra.Logic.RemoteControlled
{
    /// <summary>
    /// Computes all existing simple bullets.
    /// Requires:
    ///     - TransformC
    ///     - HitboxC
    ///     - SimpleBulletC
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Nico Greiner </author>
    public sealed class DroneSystem : IteratingEntitySystem
    {
        private LeftClickPressedE mLeftPressed;
        private readonly SoundManager mX = SoundManager.Instance;
        private SoundEffectInstance mDroneSound;

        public DroneSystem()
            : base(new TemplateBuilder()
                .All(typeof(DroneC)))
        {
            Events.Instance.Subscribe<LeftClickPressedE>(OnLeftClickPressed);
            Events.Instance.Subscribe<DroneSpawnE>(Create);
        }

        private void Create(DroneSpawnE e)
        {
            var userEntity = e.UserEntity;
            var userTranC = userEntity.GetComponent<TransformC>();
            userTranC.RotationLocked = false;

            userEntity.RemoveComponent(typeof(UserControllableC));
            mLeftPressed = null;
            var drone = mEngine.EntityFactory.Create("Drone");

            drone.GetComponent<TransformC>().CurrentPosition =
                userTranC.CurrentPosition + userTranC.RotationVector * 10;

            var droneC = drone.GetComponent<DroneC>();
            droneC.PlayerId = userEntity.Id;
            droneC.Lifetime = 10000;
            
            mEngine.EntityManager.Add(drone);
            
            mX.PlaySound("drone",true, out mDroneSound);
            

        }

        /// <summary>
        /// Run the system.
        /// This method will be called automatically by the engine during the update phase.
        /// </summary>
        /// <inheritdoc cref="Process"/>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        protected override void Process(Entity entity, GameTime time)
        {

            var droneC = entity.GetComponent<DroneC>();
            const int minTime = 1000;
            droneC.PassedTime += time.ElapsedGameTime.Milliseconds;

            if (droneC.PassedTime > droneC.Lifetime)
            {
                mEngine.EntityManager.Get(droneC.PlayerId).AddComponent(new UserControllableC());
                mEngine.EntityManager.Remove(entity.Id);

            mX.StoploopSound(mDroneSound);

            }
            else
            {
                if (droneC.PassedTime < minTime || mLeftPressed == null)
                {
                    return;
                }
            }

            mEngine.EntityManager.Get(droneC.PlayerId).AddComponent(new UserControllableC());
            mEngine.EntityManager.Remove(entity.Id);
            mX.StoploopSound(mDroneSound);
            mLeftPressed = null;
        }

        private void OnLeftClickPressed(LeftClickPressedE e)
        {
            mLeftPressed = e;
        }
    }
}
