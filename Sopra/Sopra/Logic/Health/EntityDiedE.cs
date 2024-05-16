using Sopra.ECS;

namespace Sopra.Logic.Health
{
    public sealed class EntityDiedE : IGameEvent
    {
        internal Entity DeadEntity { get; }

        public EntityDiedE(Entity deadEntity)
        {
            DeadEntity = deadEntity;
        }
    }
}
