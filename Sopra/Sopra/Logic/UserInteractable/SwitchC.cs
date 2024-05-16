using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Sopra.ECS;
using Sopra.Logic.Render;

namespace Sopra.Logic.UserInteractable
{
    /// <summary>
    /// Stores all data regarding a switch
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Nico Greiner</author>
    [Serializable]
    public sealed class SwitchC : IComponent
    {
        [XmlElement]
        public int Id { get; set; }
        [XmlElement]
        public int Id2 { get; set; }
        [XmlElement]
        public bool Check { get; set; }

        [XmlElement]
        public string ObjectName { get; set; }
        [XmlElement]
        public SerialzableDictionary<string, ComplexSprite> MapobjectsDict { get; set; }

        public SwitchC()
        { }

        public SwitchC(int id,int id2, bool check,string objectName, IDictionary<string, ComplexSprite> mapobjectsDict)
        {
            Id = id;
            Id2 = id2;
            Check = check;
            ObjectName = objectName;
            MapobjectsDict = new SerialzableDictionary<string, ComplexSprite>(mapobjectsDict);
        }
    }
}
