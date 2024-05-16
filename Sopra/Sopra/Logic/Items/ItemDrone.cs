using Sopra.ECS;
using Sopra.Logic.RemoteControlled;

namespace Sopra.Logic.Items
{
    public sealed class ItemDrone : Item
    {
        internal ItemDrone()
            : base("drone", "items/icons/drone_icon", 0, 40000, false,1)
        {
        }

        protected override void UseItem(Entity entity)
        {
            Events.Instance.Fire(new DroneSpawnE(entity));
            Reload();
        }
    }
}