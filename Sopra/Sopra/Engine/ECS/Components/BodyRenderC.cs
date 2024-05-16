using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.ECS;

namespace Sopra.Engine.ECS.Components
{
    /// <summary>
    /// Component for rendering the body of an entity.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    sealed class BodyRenderC : IComponent
    {
        public Texture2D BodySprite { get; set; }
        public float BodyScale { get; set; }
        public Vector2 Origin { get; set; }


        public BodyRenderC(Texture2D bodySprite, float bodyScale)
        {
            BodySprite = bodySprite;
            BodyScale = bodyScale;
            Origin = new Vector2(bodySprite.Width / 2f, bodySprite.Height / 2f);
        }

        public BodyRenderC(Texture2D bodySprite, float bodyScale, Vector2 origin)
        {
            BodySprite = bodySprite;
            BodyScale = bodyScale;
            Origin = origin;
        }
    }
}