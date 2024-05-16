using Sopra.ECS;

namespace Sopra.Logic.Stairs
{
    public sealed class UsedStair : IGameEvent
    {
        internal int Direction { get; }

        public UsedStair(int direction)
        {
            Direction = direction;
        }
    }
}