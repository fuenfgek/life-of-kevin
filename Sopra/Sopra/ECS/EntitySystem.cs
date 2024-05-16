using System.Collections.Generic;

namespace Sopra.ECS
{
    public abstract class EntitySystem : System
    {
        private readonly Template mTemplate;
        private Subscription mSubscription;
        
        protected EntitySystem(Template template)
        {
            mTemplate = template;
        }

        protected EntitySystem(TemplateBuilder builder)
            : this(builder.Build())
        {
        }

        internal override void SetEngine(Engine engine)
        {
            base.SetEngine(engine);
            mSubscription = new Subscription(engine, mTemplate);
        }

        protected IEnumerable<Entity> GetEntities()
        {
            return mSubscription.GetEntites();
        }
    }
}