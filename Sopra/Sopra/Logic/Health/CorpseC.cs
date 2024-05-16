using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Logic.Health
{
    /// <summary>
    /// Marks entitys which are spawning a corpse upon death.
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComponent"/>
    [Serializable]
    public sealed class CorpseC : IComponent
    {
        [XmlElement]
        public string CorpseEntityName { get; set; }

        public CorpseC() { }

        public CorpseC(string corpseEntityName)
        {
            CorpseEntityName = corpseEntityName;
        }
    }
}
