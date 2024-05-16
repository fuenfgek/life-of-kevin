using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Sopra.Logic.Render
{
    /// <summary>
    /// A Component to hold an animated simpleSprite with all its important information to draw it.
    /// </summary>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class ComplexSprite
    {
        [XmlElement]
        public string SpritePath { get; set; }
        [XmlElement]
        public Rectangle? SourceRectangle { get; set; }
        [XmlElement]
        public Vector2 SpriteSize { get; set; }
        [XmlElement]
        public bool IsAnimated { get; set; }
        [XmlElement]
        public Vector2 FrameSize { get; set; }
        [XmlElement]
        public int NumberOfFrames { get; set; }
        [XmlElement]
        public Vector2 Origin { get; set; }
        [XmlElement]
        public int MillisecPerFrame { get; set; }
        [XmlElement]
        public int FramesPerRow { get; set; }
        [XmlElement]
        public int ElapsedTime { get; set; }
        [XmlElement]
        public int CurrentFrame { get; set; }
        [XmlElement]
        public bool AnimationFinished { get; set; }
        [XmlElement]
        public bool AnimationUninterruptible { get; set; }
    
        private ComplexSprite()
        {
        }

        /// <summary>
        /// Create an animated spritePath.
        /// </summary>
        /// <param name="spritePath">The texture that contains all frames for the animation.</param>
        /// <param name="frameSize">Size of one frame in the given texture.</param>
        /// <param name="numberOfFrames">The number of frames the animation has.</param>
        /// <param name="millisecPerFrame">The duration how long one simpleSprite is shown. Exact up to one millisec.</param>
        /// <param name="spriteSize">Optional: The size to which the sprites should be scaled.</param>
        /// <param name="origin">Optional: A custom origin. Leave it if you want the origin to be centered.</param>
        /// <param name="animationUninterruptible">Opional: If this is true the animation can only be changed when it's finished playing.</param>
        /// <returns></returns>
        public ComplexSprite(
            string spritePath,
            Vector2 frameSize,
            int numberOfFrames,
            int millisecPerFrame,
            Vector2? spriteSize = null,
            Vector2? origin = null,
            bool animationUninterruptible = false)
        {
            IsAnimated = true;
            SpritePath = spritePath;
            FrameSize = frameSize;
            NumberOfFrames = numberOfFrames;
            MillisecPerFrame = millisecPerFrame;

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
            
            FramesPerRow = (int)Math.Ceiling(Math.Sqrt(NumberOfFrames));
            AnimationUninterruptible = animationUninterruptible;
        }

        /// <summary>
        /// Create an unanimated spritePath.
        /// </summary>
        /// <param name="spritePath">The texture that contains all frames for the animation.</param>
        /// <param name="spriteSize">Optional: The size to which the sprites should be scaled.</param>
        /// <param name="origin">Optional: A custom origin. Leave it if you want the origin to be centered.</param>
        /// <param name="sourceRectangle">Optional: The Area of the simpleSprite that should be drawn. Mandatory if the source sprite isn't 64x64.</param>
        public ComplexSprite(
            string spritePath,
            Vector2? spriteSize = null,
            Vector2? origin = null,
            Rectangle? sourceRectangle = null)
        {
            IsAnimated = false;
            SpritePath = spritePath;
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

