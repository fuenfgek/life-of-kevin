using Sopra.ECS;

namespace Sopra.Logic.RemoteControlled
{
    public sealed class DroneC : IComponent
    {
        public int Lifetime { get; set; }

        public int PassedTime { get; set; }

        public int PlayerId { get; set; }
    }
}
