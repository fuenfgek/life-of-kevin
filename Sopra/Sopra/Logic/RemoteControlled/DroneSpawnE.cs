using Sopra.ECS;

namespace Sopra.Logic.RemoteControlled
{
    public sealed class DroneSpawnE : IGameEvent
    {
        public Entity UserEntity { get; }

        public DroneSpawnE(Entity userEntity)
        {
            UserEntity = userEntity;
        }
    }
}
