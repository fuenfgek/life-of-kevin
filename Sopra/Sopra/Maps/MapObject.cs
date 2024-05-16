using System.Collections.Generic;
using System.Collections.ObjectModel;
using TiledSharp;

namespace Sopra.Maps
{
    public sealed class MapObject
    {
        public string Name { get; }
        public string Type { get; }
        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }
        public Collection<TmxObjectPoint> Points { get; }
        public Dictionary<string, string> CustomProperties { get; }

        public MapObject(string name,
            string type,
            float x,
            float y,
            float width,
            float height,
            Collection<TmxObjectPoint> points,
            PropertyDict customProperties)
        {
            Name = name;
            Type = type;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Points = points;

            CustomProperties = new Dictionary<string, string>();
            foreach (var pair in customProperties)
            {
                CustomProperties.Add(pair.Key, pair.Value);
            }
        }
    }
}
