using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Logic.Render
{
    /// <summary>
    /// Holds all possible sprites for an entity.
    /// Allows simple switching between those.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class AnimationC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of <AnimationC>();
        
        [XmlElement]
        public float Layer { get; set; }

        /// <summary>
        /// Naming scheme for entrys in the dict for quick change: item_activity
        /// All possible combinations of different items and activitys should be covered.
        /// "default_default" is the standard start state of every AnimationC.
        /// Don't forget about "default_activity" and "item_default".
        /// </summary>
        [XmlElement]
        public SerialzableDictionary<string, ComplexSprite> SpriteDict { get; set; }

        /// <summary>
        /// This can be set manually.
        /// If this is true the entity to which this component is attached will get destroyed as soon as the current animation has ended.
        /// Default is false.
        /// </summary>
        [XmlElement]
        public bool SelfDestruct { get; set; }

        /// <summary>
        /// This can be set manually.
        /// If this is true the entity to which this component is attached will display the current animation only once.
        /// Default is false.
        /// </summary>
        [XmlElement]
        public bool OnlyOnce { get; set; }

        // Don't set these manually! #Felix
        [XmlElement]
        public string CurrentItem { get; set; }
        [XmlElement]
        public string CurrentActivity { get; set; }
        [XmlElement]
        public string NextActivity { get; set; }
        [XmlElement]
        public bool UpdateItemAnimation { get; set; }
        [XmlElement]
        public bool EffectCheck { get; set; }

        public AnimationC() {}


        /// <summary>
        /// </summary>
        /// <param name="layer">front, higher number -> further back</param>
        /// <param name="spriteDict"></param>
        /// <param name="updateItemAnimation"></param>
        public AnimationC(float layer, IDictionary<string, ComplexSprite> spriteDict, bool updateItemAnimation = true)
        {
            Layer = layer;
            SpriteDict = new SerialzableDictionary<string, ComplexSprite>(spriteDict);
            CurrentItem = "default";
            CurrentActivity = "default";
            NextActivity = "default";
            UpdateItemAnimation = updateItemAnimation;
        }

        /// <summary>
        /// Send the command to change the current acivity.
        /// Only call this if you know what you are doing.
        /// The animations are mostly managing themself.
        /// </summary>
        /// <param name="newActivity"></param>
        /// <returns></returns>
        public void ChangeAnimationActivity(string newActivity)
        {
            if (CurrentActivity == "use")
            {
                return;
            }

            NextActivity = newActivity;
        }

        public void BruteStopAnim()
        {
            var complexSprite = SpriteDict[string.Join("_", CurrentItem, CurrentActivity)];
            complexSprite.AnimationFinished = true;
            complexSprite.CurrentFrame = 0;
        }
    }
}
