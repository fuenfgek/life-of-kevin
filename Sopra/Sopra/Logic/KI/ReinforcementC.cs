using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.KI
{
    [Serializable]
    public sealed class ReinforcementC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of <ReinforcementC>();
        [XmlElement]
        public double SpawnTimer { get; set; }
        [XmlElement]
        public double WaitTimer { get; set; }
        [XmlElement]
        public double TimeInLevel { get; set; }
        [XmlElement]
        public int CollisionTestTimer { get; set; }
        [XmlElement]
        public SerialzableDictionary<int, Vector2> SpawnPoints { get; set; }
        [XmlElement]
        public int MaxLevel { get; set; }

        public ReinforcementC()
        {
            SpawnPoints = new SerialzableDictionary<int, Vector2>();
        }
    }
}
