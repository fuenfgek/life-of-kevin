using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Collision
{
    /// <summary>
    /// Component for storing the hitbox type and size.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class HitboxC : IComponent
    {
        
        public static ComponentType Type { get; } = ComponentType.Of <HitboxC>();

        /// <summary>
        /// Size of a rectangular hitbox.
        /// Only set if IsLine == false.
        /// </summary>
        [XmlElement]
        public Vector2 Size { get; set; }
        [XmlElement]
        public Vector2 Startpoint { get; set; }
        [XmlElement]
        public Vector2 Endpoint { get; set; }
        [XmlElement]
        public bool IsLine { get; }

        internal HitboxC()
        {
        }

        internal HitboxC(float x, float y)
        {
            Size = new Vector2(x, y);
            IsLine = false;
        }

        internal HitboxC(float x1, float y1, float x2, float y2)
        {
            Startpoint = new Vector2(x1, y1);
            Endpoint = new Vector2(x2, y2);
            IsLine = true;
        }

        internal HitboxC(Vector2 point1, Vector2 point2)
        {
            Startpoint = point1;
            Endpoint = point2;
            IsLine = true;
        }
    }
}
