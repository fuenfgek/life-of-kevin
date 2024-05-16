using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.KI
{
    /// <inheritdoc/>
    /// <summary>
    /// A noise event is used to prepresent a one time only noise at a given position.
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class NoiseEvent : IGameEvent
    {
        public Vector2 Position { get; }
        public int Volume { get; }
        public int CausingEntityId { get; }

        public NoiseEvent(int causingEntityId, Vector2 position, int volume)
        {
            Position = position;
            Volume = volume;
            CausingEntityId = causingEntityId;
        }
    }
}