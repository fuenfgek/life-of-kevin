using Microsoft.Xna.Framework;

namespace Sopra.ECS
{
    /// <summary>
    /// Base class for all systems.
    /// </summary>
    /// <author>Michael Fleig</author>
    public abstract class System
    {
        protected Engine mEngine;

        internal virtual void SetEngine(Engine engine)
        {
            mEngine = engine;
        }

        /// <summary>
        /// Run the system.
        /// This method will be called automatically by the engine during the update phase.
        /// </summary>
        /// <param name="time"></param>
        public abstract void ProcessSystem(GameTime time);
    }
}