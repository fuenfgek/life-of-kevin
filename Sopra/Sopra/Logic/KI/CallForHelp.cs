using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.KI
{
    [Serializable]
    public sealed class CallForHelp : IComponent
    {
        [XmlElement]
        public Vector2 TargetPos { get; set; }

        public CallForHelp(Vector2 targetPos)
        {
            TargetPos = targetPos;
        }
    }
}