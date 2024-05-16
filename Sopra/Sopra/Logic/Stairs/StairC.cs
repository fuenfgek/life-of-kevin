using Sopra.ECS;

namespace Sopra.Logic.Stairs
{
    /// <summary>
    /// Stores all data regarding a stair.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Nico Greiner</author>
    public sealed class StairC : IComponent
    {
        public int StairDirection { get; set; }
    }
}
