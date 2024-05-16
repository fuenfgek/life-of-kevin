using Sopra.ECS;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    public sealed class BossKiC : IComponent
    {
        public int PassedTimeFloor { get; set; } = 200000;
        public int PassedTimeTurrets { get; set; } = 200000;
        public int PassedTimeTurretSpawn { get; set; } = 200000;
        public int PassedTimeEnemySpawn { get; set; } = 200000;
        public int PassedTimeShielded { get; set; }
        public int Phase { get; set; }
        public bool IsActiv { get; set; }
        public bool IsShielded { get; set; }
        public bool IsInvinncible { get; set; } = true;

        /// <summary>
        /// Increase all timers stored in this component by a given ammount.
        /// </summary>
        /// <param name="passedTime"></param>
        internal void IncreaseTimers(int passedTime)
        {
            PassedTimeFloor += passedTime;
            PassedTimeTurrets += passedTime;
            PassedTimeTurretSpawn += passedTime;
            PassedTimeEnemySpawn += passedTime;
            PassedTimeShielded += passedTime;
        }
    }
}
