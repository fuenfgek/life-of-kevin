using Sopra.ECS;

namespace Sopra.Logic.RemoteControlled
{
    public sealed class CarC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of <CarC>();

        public int Lifetime { get; } = 10000;

        public int PassedTime { get; set; }

        public int PlayerId { get; set; }

        public int Damage { get; } = 20;
    }
}
