using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Items.Projectiles;
using Sopra.Logic.Render;
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
    internal sealed class CarSystem : IteratingEntitySystem
    {
        private LeftClickPressedE mLeftPressed;
        private readonly Template mWallTemplate = Template.All(typeof(CollisionInaccessibleC)).Build();


        public CarSystem()
            : base(new TemplateBuilder()
                .All(typeof(CarC)))
        {

            Events.Instance.Subscribe<LeftClickPressedE>(OnLeftClickPressed);
            Events.Instance.Subscribe<CarSpawnE>(Create);


        }


        private void Create(CarSpawnE e)
        {   
            var userEntity = e.UserEntity;
            var userTranC = userEntity.GetComponent<TransformC>();
            userTranC.RotationLocked = false;

            mLeftPressed = null;
            var carPos = userTranC.CurrentPosition + userTranC.RotationVector * 50;

            if (mEngine.Collision.GetCollidingEntities(new Rectangle(carPos.ToPoint(), new Point(20, 20)), mWallTemplate).Count == 0)
            {
                var car = mEngine.EntityFactory.Create("Car");
                car.GetComponent<TransformC>().CurrentPosition = carPos;
                var carC = car.GetComponent<CarC>();
                carC.PlayerId = userEntity.Id;
                mEngine.EntityManager.Add(car);

                userEntity.RemoveComponent(typeof(UserControllableC));
                SoundManager.Instance.PlaySound("car");
            }
            else
            {
                e.Item.PassedTime = e.Item.ReloadDuration;
            }
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
            var carC = entity.GetComponent<CarC>();

            carC.PassedTime += time.ElapsedGameTime.Milliseconds;
            const int minTime = 1000;
            var transC = entity.GetComponent<TransformC>();
            var newPosition = transC.CurrentPosition + transC.RotationVector;

            if (carC.PassedTime > carC.Lifetime)
            {
                mEngine.EntityManager.Get(carC.PlayerId).AddComponent(new UserControllableC());
                mEngine.EntityManager.Remove(entity.Id);
            }
            else
            {
                if (carC.PassedTime < minTime || mLeftPressed == null)
                {
                    return;
                }
                
                //Explosion
                var explosion = mEngine.EntityFactory.Create("Explosion");
                explosion.GetComponent<TransformC>().CurrentPosition = newPosition;
                explosion.GetComponent<AnimationC>().SelfDestruct = true;
                var expolosionC = explosion.GetComponent<ExplosionC>();
                expolosionC.Damage = carC.Damage;
                expolosionC.GunOwnerId = carC.PlayerId;
                expolosionC.IsCarExplosion = true;
                mEngine.EntityManager.Add(explosion);

                mEngine.EntityManager.Remove(entity.Id);

                mEngine.EntityManager.Get(carC.PlayerId).AddComponent(new UserControllableC());

                mLeftPressed = null;
            }
            

        }

        private void OnLeftClickPressed(LeftClickPressedE e)
        {
            mLeftPressed = e;
        }

    }
}
