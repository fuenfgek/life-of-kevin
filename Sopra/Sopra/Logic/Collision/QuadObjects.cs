using Microsoft.Xna.Framework;
using Sopra.ECS;

namespace Sopra.Logic.Collision
{
    public class QuadObjects
    {
        internal Entity Entity { set; get; }
        internal Rectangle Rec { set; get; }
        internal Vector2 Center { set; get; }
        public QuadObjects(Entity entity)
        {
            Entity = entity;
            Rec = ToBox(entity);
            Center = ToCenter(entity);
        }

        private static Rectangle ToBox(Entity entity)
        {
            Rectangle rectangle;
            var hitbox = entity.GetComponent<HitboxC>();
            int x;
            int y;
            if (hitbox.IsLine)
            {
                Vector2 lp;
                Vector2 rp;
                if (hitbox.Startpoint.X < hitbox.Endpoint.X)
                {
                    lp = hitbox.Startpoint;
                    rp = hitbox.Endpoint;
                }
                else
                {
                    lp = hitbox.Endpoint;
                    rp = hitbox.Startpoint;
                }
                rectangle = lp.Y < rp.Y
                    ? new Rectangle((int)lp.X, (int)lp.Y, (int)(rp.X - lp.X), (int)(rp.Y - lp.Y))
                    : new Rectangle((int)lp.X, (int)lp.Y, (int)(rp.X - lp.X), (int)(lp.Y - rp.Y));
            }
            else
            {
                var transformC = entity.GetComponent<TransformC>();
                x = (int)(transformC.CurrentPosition.X - (hitbox.Size.X / 2));
                y = (int)(transformC.CurrentPosition.Y - (hitbox.Size.Y / 2));
                rectangle = new Rectangle(x, y, (int)hitbox.Size.X, (int)hitbox.Size.Y);
            }

            return rectangle;
        }

        private static Vector2 ToCenter(Entity entity)
        {
            var hitbox = entity.GetComponent<HitboxC>();
            if (hitbox.IsLine)
            {
                return new Vector2((hitbox.Startpoint.X + hitbox.Endpoint.X) / 2,
                    hitbox.Startpoint.Y + hitbox.Endpoint.Y / 2);
            }

            var transformC = entity.GetComponent<TransformC>();
            return transformC.CurrentPosition;
        }

    }
}
