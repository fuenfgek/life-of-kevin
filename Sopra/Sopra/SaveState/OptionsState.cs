using System;
using System.IO;
using System.Xml.Serialization;

namespace Sopra.SaveState
{
    public sealed class OptionsState
    {
        public int SoundIndex { get; set; }
        public int SongIndex { get; set; }

        public bool SoundAnAus { get; set; }
        public bool SongAnAus { get; set; }

        public void Save(String fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                var xml = new XmlSerializer(typeof(OptionsState));
                xml.Serialize(stream, this);
            }
        }

        public static OptionsState LoadFromFile(String fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var xml = new XmlSerializer(typeof(OptionsState));
                return (OptionsState)xml.Deserialize(stream);
            }
        }
    }
}
