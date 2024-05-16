using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sopra.ECS
{
    [Serializable]
    public sealed class ComponentType
    {
        private static readonly Dictionary<Type, ComponentType> sTypes = new Dictionary<Type, ComponentType>();
        private static int sTypeIndex;

        [XmlElement]
        public int Index { get; set; }

        private ComponentType()
        {
            Index = sTypeIndex++;
        }

        public static ComponentType Of<T>() where T : IComponent
        {
            return Of(typeof(T));
        }

        private static ComponentType Of(Type type)
        {
            if (!typeof(IComponent).IsAssignableFrom(type))
            {
                throw new Exception("ComponentType can only be assigned to a Type of IComponent.");
            }
            
            ComponentType componentType;
            sTypes.TryGetValue(type, out componentType);

            if (componentType == null)
            {
                componentType = new ComponentType();
                sTypes.Add(type, componentType);
            }

            return componentType;
        }

        public static int GetIndex<T>() where T : IComponent
        {
            return Of<T>().Index;
        }

        public static int GetIndex(Type type)
        {
            return Of(type).Index;
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }
}