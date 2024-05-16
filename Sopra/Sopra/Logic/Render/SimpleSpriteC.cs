using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Render
{
    /// <summary>
    /// A Component to hold a simpleSprite with all its important information to draw it.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    [Serializable]
    public sealed class SimpleSpriteC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of <SimpleSpriteC>();

        [XmlElement]
        public float Layer { get; set; }
        [XmlElement]
        public string SpritePath { get; set; }
        [XmlElement]
        public Vector2 SpriteSize { get; set; }
        [XmlElement]
        public Vector2 Origin { get; set; }
        [XmlElement]
        public Rectangle? SourceRectangle { get; set; }

        private SimpleSpriteC()
        {
        }

        /// <param name="layer">0 -> front, higher number -> further back</param>
        /// <param name="spritePath">The simpleSprite that should be drawn.</param>
        /// <param name="spriteSize">Optional: The size to which the sprites should be scaled.</param>
        /// <param name="origin">Optional: A custom origin. Leave it if you want the origin to be centered.</param>
        /// <param name="sourceRectangle">Optional: The Area of the simpleSprite that should be drawn. Mandatory if the source sprite isn't 64x64.</param>
        public SimpleSpriteC(
            float layer,
            string spritePath,
            Vector2? spriteSize = null,
            Vector2? origin = null,
            Rectangle? sourceRectangle = null)
        {
            SpritePath = spritePath;
            Layer = layer;
            SourceRectangle = sourceRectangle;
            
            if (spriteSize.HasValue)
            {
                SpriteSize = spriteSize.Value;
                Origin = origin ?? spriteSize.Value / 2;
            }
            else
            {
                SpriteSize = new Vector2(64);
                Origin = origin ?? new Vector2(32);
            }
        }
    }
}
