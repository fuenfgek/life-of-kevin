using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.UserInteractable;

namespace Sopra.Logic.Collision
{
    /// <summary>
    /// Implements collision detection.
    /// </summary>
    /// <author>Felix Vogt</author>
    public sealed class Collisions
    {
        private readonly QuadTree mQuadTree;
        private readonly Template mInteractableTemplate = Template.All(typeof(UserInteractableC)).Build();
        private readonly ComponentType mTransformType = ComponentType.Of<TransformC>();
        private readonly ComponentType mHitboxType = ComponentType.Of<HitboxC>();


        public Collisions(QuadTree quadTree)
        {
            mQuadTree = quadTree;
        }
        /// <summary>
        /// Get the clicked entity, if one was clicked.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns>The clicked entity or null.e</returns>
        internal Entity GetClickedEntity(Vector2 mousePos)
        {
            var list = GetCollidingEntities(new HitboxC(5, 5), mousePos, mInteractableTemplate);
            return list.FirstOrDefault();
        }


        /// <summary>
        /// Detectinc Collision with a given TransFormC, HitBoxC and Template
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="position"></param>
        /// <param name="template"></param>
        /// <param name="ignoreIds"></param>
        /// <returns></returns>

        #region Collision for TransformC and HitboxC
        internal List<Entity> GetCollidingEntities(
            HitboxC hitbox,
            TransformC position,
            Template template = null, int[] ignoreIds = null)
        {
            var hitboxCenter = position.CurrentPosition;

            return GetCollidingEntities(
                hitbox,
                 hitboxCenter,
                 template,
                ignoreIds);
        }
        #endregion

        #region Collision for Hitbox ,Vector2

        /// <summary>
        /// Collision Detection for HItbox and specific Vector 2
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="position"></param>
        /// <param name="template"></param>
        /// <param name="ignoreIds"></param>
        /// <returns></returns>
        internal List<Entity> GetCollidingEntities(HitboxC hitbox,
            Vector2 position,
            Template template = null, int[] ignoreIds = null)
        {
            var collidingEntities = new List<Entity>();

            var list = mQuadTree.Retrieve(ToBox(hitbox, position));

            foreach (var obstacle in list)
            {
                if (template != null && !template.Matches(obstacle.Entity))
                {
                    continue;
                }

                var obstacleHitbox = obstacle.Entity.GetComponent<HitboxC>(mHitboxType);
                var obstaclePosition = obstacle.Entity.GetComponent<TransformC>(mTransformType).CurrentPosition;

                if (ignoreIds != null && ignoreIds.Contains(obstacle.Entity.Id))
                {
                    continue;
                }

                if (!CheckCollison(hitbox, position, obstacleHitbox, obstaclePosition))
                {
                    continue;
                }

                collidingEntities.Add(obstacle.Entity);
            }

            return collidingEntities;
        }

        #endregion

        #region Collisions for Entities

        /// <summary>
        /// Detecting Collision with Entity with a given Template
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="template"></param>
        /// <param name="ignoreIds"></param>
        /// <returns></returns>
        internal List<Entity> GetCollidingEntities(Entity entity,
            Template template = null, int[] ignoreIds = null)
        {
            var hitbox = entity.GetComponent<HitboxC>(mHitboxType);
            var position = entity.GetComponent<TransformC>(mTransformType);
            int[] id;
            if (ignoreIds != null)
            {
                id = new int[ignoreIds.Length + 1];
                ignoreIds.CopyTo(id, 1);
                id[0] = entity.Id;
            }
            else
            {
                id = new[] {entity.Id};
            }
            return GetCollidingEntities(hitbox, position, template, id);
        }

        #endregion

        #region Collisions fo Rectangle

        /// <summary>
        /// Detecting Collision with Rectangle with a given Template
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        internal List<Entity> GetCollidingEntities(Rectangle rect,
            Template template = null)
        {
            var collidingEntities = new List<Entity>();
            var hitbox = new Vector2(rect.Width, rect.Height);
            var hitboxCenter = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            foreach (var obstacle in mQuadTree.Retrieve(rect))
            {
                if (template != null && !template.Matches(obstacle.Entity))
                {
                    continue;
                }

                var obstacleHitbox = obstacle.Entity.GetComponent<HitboxC>(mHitboxType);
                var obstaclePosition = obstacle.Entity.GetComponent<TransformC>(mTransformType).CurrentPosition;

                if (!CheckRec(hitbox, hitboxCenter, obstacleHitbox, obstaclePosition))
                {
                    continue;
                }

                collidingEntities.Add(obstacle.Entity);
            }

            return collidingEntities;
        }
        #endregion

        #region Collision for Line

        /// <summary>
        /// Collision Detection for a single Line with a given  Template
        /// </summary>
        /// <param name="spoint"></param>
        /// <param name="epoint"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        internal List<Entity> GetCollidingEntities(Vector2 spoint, Vector2 epoint, Template template = null)
        {
            var collidingEntities = new List<Entity>();
            var x = Math.Min(spoint.X, epoint.X);
            var y = Math.Min(spoint.Y, epoint.Y);
            var width = Math.Abs(spoint.X - epoint.X);
            var height = Math.Abs(spoint.Y - epoint.Y);
            var rectangle =  new Rectangle((int)x, (int)y, (int)width, (int)height);
           
            foreach (var obstacle in mQuadTree.Retrieve(rectangle))
            {
                if (template != null && !template.Matches(obstacle.Entity))
                {
                    continue;
                }

                var obstacleHitbox = obstacle.Entity.GetComponent<HitboxC>(mHitboxType);
                var obstaclePosition = obstacle.Entity.GetComponent<TransformC>(mTransformType).CurrentPosition;

                if (!CheckLine(spoint, epoint, obstacleHitbox, obstaclePosition))
                {
                    continue;
                }

                collidingEntities.Add(obstacle.Entity);
            }

            return collidingEntities;
        }


        #endregion




        #region CheckCollision for Rectangle
        private bool CheckRec(
            Vector2 rectangle,
            Vector2 reccenter,
            HitboxC obstacleHitbox,
            Vector2 obstaclePosition)
        {          

            if (obstacleHitbox.IsLine)
            {
                return CheckCollisionLineBox(obstacleHitbox.Startpoint,
                    obstacleHitbox.Endpoint,
                    reccenter,
                    rectangle);
            }

            return CheckCollisionBoxBox(
                reccenter,
                rectangle,
                obstaclePosition,
                obstacleHitbox.Size);
        }
        #endregion

        #region CheckCollision for Line

        private bool CheckLine(
            Vector2 spoint,
            Vector2 epoint,
            HitboxC obstacleHitbox,
            Vector2 obstaclePosition)
        {
            if (obstacleHitbox.IsLine)
            {
                return CheckCollisionLineLine(spoint,
                    epoint,
                    obstacleHitbox.Startpoint,
                    obstacleHitbox.Endpoint);
            }

            return CheckCollisionLineBox(spoint,
                epoint,
                obstaclePosition,
                obstacleHitbox.Size);

        }


        #endregion

        #region CheckCollision for Entities
        private bool CheckCollison(
           HitboxC entityHitbox,
           Vector2 entityPosition,
           HitboxC obstacleHitbox,
           Vector2 obstaclePosition)
        {
            if (entityHitbox.IsLine)
            {
                return CheckLine(entityHitbox.Startpoint,
                    entityHitbox.Endpoint,
                    obstacleHitbox,
                    obstaclePosition);
            }
           
            return CheckRec(entityHitbox.Size,
                entityPosition, obstacleHitbox,
                obstaclePosition);
        }

        #endregion

        #region Calculation for Intersecting
        
        #region Calculation of BoxBox Intersecting
        /// <summary>
        /// </summary>
        /// <param name="rect1Pos">Center of the rectangle.</param>
        /// <param name="rect1Size"></param>
        /// <param name="rect2Pos">Center of the rectangle.</param>
        /// <param name="rect2Size"></param>
        /// <returns></returns>
        private bool CheckCollisionBoxBox(
            Vector2 rect1Pos,
            Vector2 rect1Size,
            Vector2 rect2Pos,
            Vector2 rect2Size)
        {
            return rect1Pos.X - rect1Size.X / 2 < rect2Pos.X + rect2Size.X / 2
                   && rect2Pos.X - rect2Size.X / 2 < rect1Pos.X + rect1Size.X / 2
                   && rect1Pos.Y - rect1Size.Y / 2 < rect2Pos.Y + rect2Size.Y / 2
                   && rect2Pos.Y - rect2Size.Y / 2 < rect1Pos.Y + rect1Size.Y / 2;
        }


        #endregion

        #region Calculation of LineBox, LineCircle, LineLine Intersecting

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line1Sp"></param>
        /// <param name="line1Ep"></param>
        /// <param name="line2Sp"></param>
        /// <param name="line2Ep"></param>
        /// <returns></returns>
        private static bool CheckCollisionLineLine(
            Vector2 line1Sp,
            Vector2 line1Ep,
            Vector2 line2Sp,
            Vector2 line2Ep)
        {

            var o1 = Orientation(line1Sp, line1Ep, line2Sp);
            var o2 = Orientation(line1Sp, line1Ep, line2Ep);
            var o3 = Orientation(line2Sp, line2Ep, line1Sp);
            var o4 = Orientation(line2Sp, line2Ep, line1Ep);

            if (o1 != o2 && o3 != o4)
            {
                return true;
            }

            // Special Cases
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(line1Sp, line2Sp, line1Ep))
            {
                return true;
            }

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(line1Sp, line2Ep, line1Ep))
            {
                return true;
            }

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(line2Sp, line1Sp, line2Ep))
            {
                return true;
            }

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2
            return o4 == 0 && OnSegment(line2Sp, line1Ep, line2Ep);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineSp"></param>
        /// <param name="lineEp"></param>
        /// <param name="rectpos"></param>
        /// <param name="rectsize"></param>
        /// <returns></returns>
        private bool CheckCollisionLineBox(Vector2 lineSp,
            Vector2 lineEp,
            Vector2 rectpos,
            Vector2 rectsize)
        {
            var upperLeft = new Vector2(rectpos.X - rectsize.X / 2, rectpos.Y - rectsize.Y / 2);
            var bottomLeft = new Vector2(rectpos.X - rectsize.X / 2, rectpos.Y + rectsize.Y / 2);
            var upperRight = new Vector2(rectpos.X + rectsize.X / 2, rectpos.Y - rectsize.Y / 2);
            var bottomRight = new Vector2(rectpos.X + rectsize.X / 2, rectpos.Y + rectsize.Y / 2);

            var lineUp = CheckCollisionLineLine(lineSp, lineEp, upperLeft, upperRight);
            var lineDown = CheckCollisionLineLine(lineSp, lineEp, bottomLeft, bottomRight);
            var lineLeft = CheckCollisionLineLine(lineSp, lineEp, bottomLeft, upperLeft);
            var lineRight = CheckCollisionLineLine(lineSp, lineEp, upperRight, bottomRight);

            return lineUp || lineDown || lineLeft || lineRight;
        }

        private static bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                   q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }

        private static int Orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            var val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (Math.Abs(val) < 0.001)
            {
                return 0;
            }

            return val > 0 ? 1 : 2;
        }

        #endregion

        #endregion

        #region  ToRectangle

        private Rectangle ToBox(HitboxC hitbox, Vector2 position)
        {
            if (hitbox.IsLine)
            {
                var x = Math.Min(hitbox.Startpoint.X, hitbox.Endpoint.X);
                var y = Math.Min(hitbox.Startpoint.Y, hitbox.Endpoint.Y);
                var width = Math.Abs(hitbox.Startpoint.X - hitbox.Endpoint.X);
                var height = Math.Abs(hitbox.Startpoint.Y - hitbox.Endpoint.Y);

                return new Rectangle((int)x, (int)y, (int)width, (int)height);

            }
            else
            {
                var x = (int)(position.X - hitbox.Size.X / 2);
                var y = (int)(position.Y - hitbox.Size.Y / 2);
                return new Rectangle(x, y, (int)hitbox.Size.X, (int)hitbox.Size.Y);
            }
        }

        #endregion

        #region Geometry

        /// <summary>
        /// Calculate the perpendicular vector of given one.
        /// </summary>
        /// <param name="vector">The vector to be used.</param>
        /// <param name="left">Calculate the left perpendicular vector if set true, the right perpendicular vector otherwise.</param>
        /// <returns></returns>
        internal static Vector2 OrthogonalVector(Vector2 vector, bool left)
        {
            return left ? new Vector2(-vector.Y, vector.X) : new Vector2(vector.Y, -vector.X);
        }

        #endregion
    }
}