using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Sopra.ECS;

namespace Sopra.SaveState
{
    [Serializable]
    public sealed class EntityState : IXmlSerializable
    {
        public int Id { get; private set; }
        public readonly Dictionary<Type, IComponent> mComponents = new Dictionary<Type, IComponent>();
        
        public EntityState(Entity entity)
        {
            Id = entity.Id;
            mComponents = new Dictionary<Type, IComponent>();
            
            foreach (var component in entity.GetComponents())
            {
                mComponents.Add(component.GetType(), component);
            }
        }

        private EntityState()
        {

        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Id = Convert.ToInt32(reader.GetAttribute("id"));
             
            reader.Read();
            reader.ReadStartElement("components");
            
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var typeName = reader.GetAttribute("type");
                reader.ReadStartElement("component");

                if (typeName == null)
                {
                    return;
                }
                
                var type = Type.GetType(typeName);

                if (type == null)
                {
                    return;
                }

                var componentSerializer = new XmlSerializer(type);

                var value = (IComponent) componentSerializer.Deserialize(reader);

                mComponents.Add(type, value);

                reader.ReadEndElement();
            }
            reader.ReadEndElement();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", Id.ToString());
                        
            writer.WriteStartElement("components");

            foreach (var entry in mComponents)
            {
                var componentSerializer = new XmlSerializer(entry.Key);
                
                writer.WriteStartElement("component");
                writer.WriteAttributeString("type", entry.Key.ToString());
                componentSerializer.Serialize(writer, entry.Value);
                writer.WriteEndElement();
            }
            
            writer.WriteEndElement();
        }
    }
}
