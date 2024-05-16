using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Maps.MapComponents
{
    [Serializable]
    public sealed class ChairC : IComponent
    {
        [XmlElement]
        public bool Check { get; set; }

        [XmlElement]
        public bool Switched { get; set; }

        public ChairC()
        {

        }

        public ChairC(bool check)
        {
            Check = check;
        }
        
    }
}