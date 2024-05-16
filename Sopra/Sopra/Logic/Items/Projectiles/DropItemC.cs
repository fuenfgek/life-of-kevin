using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Logic.Items.Projectiles
{
    [Serializable]
    public sealed class DropItemC : IComponent
    {
        [XmlElement]
        public string Name { get; set; }


        private DropItemC()
        {
        }

        public DropItemC(string name)
        {
            Name = name;
  
        }
    }
}
