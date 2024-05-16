using System.Collections.Generic;

namespace Sopra.ECS
{
    /// <summary>
    /// Delegate for entity blueprints.
    /// </summary>
    public delegate Entity Blueprint();

    /// <summary>
    /// The EntityFactory allows easy creation of entites based on
    /// pre defined blueprints.
    ///
    /// Please note that the entities created from a blueprint must be
    /// added manually to the <see cref="EntityManager"/>.
    /// </summary>
    /// <author>Michael Fleig</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EntityFactory
    {
        private readonly Dictionary<string, Blueprint> mBlueprints = new Dictionary<string, Blueprint>();

        /// <summary>
        /// Create an entity from a blueprint.
        /// The entity is not attached to a EntityManager.
        /// You have to add it yourself!
        /// </summary>
        /// <param name="name">name of the blueprint.</param>
        /// <returns>a new entity.</returns>
        public Entity Create(string name)
        {
            return mBlueprints[name].Invoke();
        }

        /// <summary>
        /// Add a new blueprint to the factory.
        /// </summary>
        /// <param name="name">Name of the blueprint.</param>
        /// <param name="blueprint">blueprint delegate method.</param>
        /// <returns>Current EnttiyFactory (builder)</returns>
        public void AddBlueprint(string name, Blueprint blueprint)
        {
            mBlueprints[name] = blueprint;
        }
    }
}