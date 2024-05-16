using Sopra.ECS;
using Sopra.Logic.RemoteControlled;

namespace Sopra.Logic.Items
{
    public sealed class ItemCar : Item
    {
        internal ItemCar()
            : base("car", "items/icons/car_icon", 0, 45000, false, 1)
        {
        }


        protected override void UseItem(Entity entity)
        {
            Events.Instance.Fire(new CarSpawnE(entity, this));        
            Reload();
        }
    }
}