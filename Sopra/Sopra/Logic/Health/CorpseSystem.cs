using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Health
{
    /// <summary>
    /// System for spawning corpses.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="System"/>
    internal sealed class CorpseSystem : ECS.System
    {
        public CorpseSystem()
        {
           Events.Instance.Subscribe<EntityDiedE>(SpawnCorpse);
        }

        private void SpawnCorpse(EntityDiedE e)
        {
            if (!e.DeadEntity.HasComponent<CorpseC>())
            {
                return;
            }

            var corpse = mEngine.EntityFactory.Create(
                e.DeadEntity.GetComponent<CorpseC>().CorpseEntityName);
            corpse.GetComponent<TransformC>().CurrentPosition =
                e.DeadEntity.GetComponent<TransformC>().CurrentPosition;
            mEngine.EntityManager.Add(corpse);
        }

        public override void ProcessSystem(GameTime time)
        {
        }
    }
}
