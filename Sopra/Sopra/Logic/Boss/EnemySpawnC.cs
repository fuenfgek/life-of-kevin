using System;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.Logic.Boss
{
    /// <summary>
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class EnemySpawnC : IComponent
    {
        [XmlElement]
        public string EnemyType { get; set; }
        [XmlElement]
        public bool Spawn { get; set; }
    }
}
