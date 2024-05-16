using Microsoft.Xna.Framework;
using Sopra.Engine.ECS;

namespace Sopra.Engine.ECS.Components
{
    /// <summary>
    /// Component for storing the current position
    /// </summary>
    /// <author>Felix Vogt</author>
    sealed class TransformC : IComponent
    {
        public Vector2 CurrentPosition { get ; set; }

        public TransformC(Vector2 startingPosition)
        {
            CurrentPosition = startingPosition;
        }
    }
}