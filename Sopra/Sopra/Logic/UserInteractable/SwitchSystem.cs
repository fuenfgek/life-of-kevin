using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Collision.CollisionTypes;
using Sopra.Logic.Render;
using Sopra.Maps.MapComponents;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Compute the Switches and Riddles
    /// Requires:
    ///        SwitchC
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Nico Greiner</author>
    internal sealed class SwitchSystem : IteratingEntitySystem
    {
       
        public SwitchSystem()
            : base(new TemplateBuilder()
                .All(typeof(SwitchC), typeof(UserInteractableC)))
        {
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
            var switchC = entity.GetComponent<SwitchC>();
            var userInteractableC = entity.GetComponent<UserInteractableC>();

            #region Open/Close

            if (userInteractableC.InteractingEntityId == 0)
            {
                return;
            }

            userInteractableC.InteractingEntityId = 0;

            SoundManager.Instance.PlaySound("switch");
            if (switchC.Id != 0)
            {

                var affectedEntity = mEngine.EntityManager.Get(switchC.Id);
                var affectedEx = affectedEntity.GetComponent<TransformC>().CurrentPosition.X;
                var affectedEy = affectedEntity.GetComponent<TransformC>().CurrentPosition.Y;

                if (switchC.ObjectName.Equals("Chair1"))
                {

                    if (!affectedEntity.GetComponent<ChairC>().Check)
                    {


                        affectedEntity.GetComponent<TransformC>().CurrentPosition =
                            new Vector2(affectedEx + 128, affectedEy);
                        affectedEntity.GetComponent<ChairC>().Switched = true;

                        affectedEntity.GetComponent<ChairC>().Check = true;

                    }
                    else
                    {
                        affectedEntity.GetComponent<TransformC>().CurrentPosition =
                            new Vector2(affectedEx - 128, affectedEy);

                        affectedEntity.GetComponent<ChairC>().Switched = true;
                        affectedEntity.GetComponent<ChairC>().Check = false;
                    }

                }
                else if (switchC.ObjectName.Equals("Chair2"))
                {

                    if (!affectedEntity.GetComponent<ChairC>().Check)
                    {
                        affectedEntity.GetComponent<TransformC>().CurrentPosition =
                            new Vector2(affectedEx - 128, affectedEy);

                        affectedEntity.GetComponent<ChairC>().Switched = true;
                        affectedEntity.GetComponent<ChairC>().Check = true;
                    }
                    else
                    {
                        affectedEntity.GetComponent<TransformC>().CurrentPosition =
                            new Vector2(affectedEx + 128, affectedEy);

                        affectedEntity.GetComponent<ChairC>().Switched = true;
                        affectedEntity.GetComponent<ChairC>().Check = false;
                    }


                }
                else if (switchC.ObjectName.Equals("Chair3"))
                {
                    if (!affectedEntity.GetComponent<ChairC>().Check)
                    {
                        affectedEntity.GetComponent<TransformC>().CurrentPosition =
                            new Vector2(affectedEx, affectedEy + 128);

                        affectedEntity.GetComponent<ChairC>().Switched = true;
                        affectedEntity.GetComponent<ChairC>().Check = true;
                    }
                    else
                    {

                        affectedEntity.GetComponent<TransformC>().CurrentPosition =
                            new Vector2(affectedEx, affectedEy - 128);

                        affectedEntity.GetComponent<ChairC>().Switched = true;
                        affectedEntity.GetComponent<ChairC>().Check = false;

                    }

                }
                    else
                    {
                        if (!affectedEntity.GetComponent<DoorC>().Check)
                        {

                            affectedEntity.RemoveComponent(!affectedEntity.HasComponent<AnimationC>()
                                ? typeof(SimpleSpriteC)
                                : typeof(AnimationC));

                            affectedEntity.RemoveComponent(typeof(CollisionInaccessibleC));
                            affectedEntity.RemoveComponent(typeof(CollisionImpenetrableC));
                            affectedEntity.RemoveComponent(typeof(StaticObjectC));

                            affectedEntity.GetComponent<DoorC>().Check = true;
                        }
                        else
                        {
                            if (switchC.ObjectName.Equals("Door1"))
                            {
                                affectedEntity
                                    .AddComponent(new AnimationC(0, switchC.MapobjectsDict));
                            }
                            else
                            {
                                affectedEntity
                                    .AddComponent(new AnimationC(0, switchC.MapobjectsDict));
                                affectedEntity.GetComponent<TransformC>().SetRotation(new Vector2(0, 1));
                            }

                            affectedEntity.AddComponent(new CollisionInaccessibleC());
                            affectedEntity.AddComponent(new CollisionImpenetrableC());
                            affectedEntity.AddComponent(new StaticObjectC());

                            affectedEntity.GetComponent<DoorC>().Check = false;
                        }
                }

                
                #endregion

                if (!switchC.Check)
                {
                    mEngine.EntityManager.Get(entity.Id).GetComponent<TransformC>().SetRotation(new Vector2(0, 1));
                    switchC.Check = true;

                }
                else
                {
                    mEngine.EntityManager.Get(entity.Id).GetComponent<TransformC>().SetRotation(new Vector2(0, -1));
                    switchC.Check = false;
                }
            }
        }
    }
}
