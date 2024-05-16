using Sopra.ECS;

namespace Sopra.Logic.Collision.CollisionTypes
{
    /// <summary>
    /// Mark an entity as impenetrable for all bullets.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    public sealed class CollisionImpenetrableC : IComponent
    {
        
        public static ComponentType Type { get; } = ComponentType.Of <CollisionImpenetrableC>();

    }
}
