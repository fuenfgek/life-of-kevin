using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Logic.UserInteractable
{
    [Serializable]
    public sealed class UpgradeMachineC : IComponent
    {
        [XmlElement]
        public bool Check { get; set; }
    }
}
