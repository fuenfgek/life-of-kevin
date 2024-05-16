using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Sopra.SaveState
{
    public sealed class State
    {
        public TimeSpan GameTimeSec { get; set; }

        public string Name { get; set; }
        public List<EntityState> Entities { get; set; }

        public void Save(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                var xml = new XmlSerializer(typeof(State));
                xml.Serialize(stream, this);
                stream.Close();
            }
        }

        public static State LoadFromFile(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var xml = new XmlSerializer(typeof(State));
                return (State)xml.Deserialize(stream);
            }
        }
    }
}
