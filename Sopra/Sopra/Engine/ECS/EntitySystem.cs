using System.Linq;

namespace Sopra.Engine.ECS
{
    public abstract class EntitySystem : System
    {
        protected Template mTemplate;
        
        protected EntitySystem(Template template)
        {
            mTemplate = template;
        }

        protected EntitySystem(TemplateBuilder builder)
            : this(builder.Build())
        {
        }

        protected Entity[] GetEntities()
        {
            return mEngine.EntityManager
                .Entities
                .Select((pair, i) => pair.Value)
                .Where(entity => mTemplate.Matches(entity))
                .ToArray();
        }
    }
}