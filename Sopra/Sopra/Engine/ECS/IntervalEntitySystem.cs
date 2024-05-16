using Microsoft.Xna.Framework;

namespace Sopra.Engine.ECS
{
    /// <summary>
    /// Base class for EntitySystems which should not update every game loop, but after a given interval.
    /// </summary>
    /// <inheritdoc cref="IteratingEntitySystem"/>
    /// <author>Michael Fleig</author>
    public abstract class IntervalEntitySystem : IteratingEntitySystem
    {
        private readonly int mInterval;
        private int mAccumulated;


        public IntervalEntitySystem(Template template, int interval)
            : base(template)
        {
            mInterval = interval;
        }

        public IntervalEntitySystem(TemplateBuilder builder, int interval)
            : this(builder.Build(), interval)
        {
        }

        public override void ProcessSystem(GameTime time)
        {
            mAccumulated += time.ElapsedGameTime.Milliseconds;

            while (mAccumulated >= mInterval)
            {
                mAccumulated -= mInterval;
                base.ProcessSystem(time);
            }
        }
    }
}