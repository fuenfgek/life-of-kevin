using Microsoft.Xna.Framework;

namespace Sopra.ECS
{
    /// <summary>
    /// Base class for EntitySystems which should not update every game loop, but after a given interval.
    /// </summary>
    /// <inheritdoc cref="EntitySystem"/>
    /// <author>Michael Fleig</author>
    public abstract class IntervalEntitySystem : EntitySystem
    {
        protected readonly int mInterval;
        private int mAccumulated;


        private IntervalEntitySystem(Template template, int interval)
            : base(template)
        {
            mInterval = interval;
        }

        protected IntervalEntitySystem(TemplateBuilder builder, int interval)
            : this(builder.Build(), interval)
        {
        }

        public override void ProcessSystem(GameTime time)
        {
            mAccumulated += time.ElapsedGameTime.Milliseconds;

            while (mAccumulated >= mInterval)
            {
                mAccumulated -= mInterval;
                ProcessInterval();
            }
        }

        protected abstract void ProcessInterval();
    }
}