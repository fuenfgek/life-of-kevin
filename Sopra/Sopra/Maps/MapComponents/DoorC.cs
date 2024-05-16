using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Maps.MapComponents
{
    [Serializable]
    public sealed class DoorC : IComponent
    {
        [XmlElement]
        public bool Check { get; set; }

        public DoorC()
        {

        }

        public DoorC(bool check)
        {
            Check = check;
        }
    }
}