using Sopra.ECS;
using Sopra.Logic.Items;

namespace Sopra.Logic.RemoteControlled
{
    public sealed class CarSpawnE : IGameEvent
    {
        public Entity UserEntity { get; }
        public ItemCar Item { get; }

        public CarSpawnE(Entity entity, ItemCar item)
        {
            UserEntity = entity;
            Item = item;
        }
    }
}
