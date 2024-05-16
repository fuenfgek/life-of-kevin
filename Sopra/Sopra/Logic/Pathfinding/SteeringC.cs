using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Logic.Pathfinding
{
    [Serializable]
    public sealed class SteeringC : IComponent
    {
        //public static ComponentType Type { get; } = ComponentType.Of <SteeringC>();

        [XmlElement]
        public bool Collision { get; set; }
        [XmlElement]
        public bool TurnLeft { get; set; }
        [XmlElement]
        public bool TurnRight { get; set; }

        public SteeringC()
        {
            TurnLeft = true;
            TurnRight = true;
        }
    }
}