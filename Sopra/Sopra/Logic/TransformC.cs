using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic
{
    /// <summary>
    /// Component for storing the current position
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class TransformC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of<TransformC>();

        /// <summary>
        /// The center of this entity.
        /// </summary>
        [XmlElement]
        public Vector2 CurrentPosition { get ; set; }
        [XmlElement]
        public bool RotationLocked { get; set; }

        /// <summary>
        /// The current rotation of the entity. 0 is at 12:00 and it increases clockwise.
        /// If it reaches the maximum at 2*Pi one cicle is done, and it starts over at 0.
        /// If you change this value, the value of "RotationVector" will automatically get updated too.
        /// </summary>
        [XmlElement]
        public float RotationRadians { get; set; }

        /// <summary>
        /// The direction the entity is currently facing. Does not need to be normalized.
        /// If you change this value, the value of "RotationRadians" will automatically get updated too.
        /// </summary>
        [XmlElement]
        public Vector2 RotationVector { get; set; }

        internal TransformC()
        {
            RotationVector = new Vector2(0, -1);
        }

        public void SetRotation(Vector2 rotationVector)
        {
            if (RotationLocked)
            {
                return;
            }

            // Fix invalid rotatians #Felix
            if (Math.Abs(rotationVector.X) < 0.01
                && Math.Abs(rotationVector.Y) < 0.01)
            {
                RotationRadians = 0;
                RotationVector = new Vector2(0, -1);
                return;
            }

            rotationVector.Normalize();
            var x = Math.Atan2(rotationVector.Y, rotationVector.X) + 0.5 * Math.PI;
            if (x < 0)
            {
                x += 2 * Math.PI;
            }

            RotationRadians = (float)x;
            RotationVector = rotationVector;
        }
    }
}