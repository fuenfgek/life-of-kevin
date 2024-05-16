using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic;
using Sopra.Logic.Render;

namespace Sopra.Maps.Tutorial
{
    internal sealed class StorySystem : IteratingEntitySystem
    {
        private readonly Template mUserCTemplate = Template.All(typeof(UserControllableC)).Build();

        private readonly Template mStoryBoxTemplate = Template.All(typeof(StoryC)).Build();
        private Subscription mStoryBoxes;
        private readonly ComponentType mAnimationType = ComponentType.Of<AnimationC>();
        private readonly ComponentType mStoryType = ComponentType.Of<StoryC>();


        public StorySystem()
            : base(new TemplateBuilder().All(typeof(StoryC)))
        {
            
        }

        internal override void SetEngine(Engine engine)
        {
            base.SetEngine(engine);
            mStoryBoxes = new Subscription(mEngine, mStoryBoxTemplate);
        }

        protected override void Process(Entity entity, GameTime time)
        {
            var ownStoryC = entity.GetComponent<StoryC>(mStoryType);

            if (mEngine.Collision.GetCollidingEntities(entity, mUserCTemplate).Count == 0)
            {
                ownStoryC.Active = false;
                return;
            }

            var animC = entity.GetComponent<AnimationC>(mAnimationType);
            if (ownStoryC.Active || animC.CurrentActivity.Equals(ownStoryC.Name))
            {
                return;
            }

            animC.ChangeAnimationActivity(ownStoryC.Name);
            animC.OnlyOnce = true;
            ownStoryC.Active = true;

            foreach (var storyBox in mStoryBoxes.GetEntites())
            {
                if (storyBox.Id == entity.Id)
                {
                    continue;
                }

                storyBox.GetComponent<AnimationC>(mAnimationType).BruteStopAnim();
            }
        }
    }
}