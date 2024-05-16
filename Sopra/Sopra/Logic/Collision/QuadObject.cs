using System;
using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Collision
{   /// <summary>
    /// Class for storing Entity in a Object for the QuadTree
    /// <author>Konstantin Fünfgelt</author>
    /// </summary>
    public sealed class QuadObject
    {
        internal Entity Entity { get; }
        internal Rectangle Rec { get; }

        public QuadObject(Entity entity)
        {
            Entity = entity;
            Rec = ToBox(entity);
        }

        private static Rectangle ToBox(Entity entity)
        {
            var hitbox = entity.GetComponent<HitboxC>(HitboxC.Type);
            if (hitbox.IsLine)
            {
                var x = Math.Min(hitbox.Startpoint.X, hitbox.Endpoint.X);
                var y = Math.Min(hitbox.Startpoint.Y, hitbox.Endpoint.Y);
                var width = Math.Abs(hitbox.Startpoint.X - hitbox.Endpoint.X);
                var height = Math.Abs(hitbox.Startpoint.Y - hitbox.Endpoint.Y);

                return new Rectangle((int) x, (int) y, (int) width, (int) height);

            }
            else
            {
                var transformC = entity.GetComponent<TransformC>(TransformC.Type);
                var x = (int)(transformC.CurrentPosition.X - hitbox.Size.X / 2);
                var y = (int)(transformC.CurrentPosition.Y - hitbox.Size.Y / 2);

                return new Rectangle(x, y, (int)hitbox.Size.X, (int)hitbox.Size.Y);
            }
        }
    }
}
