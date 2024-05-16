using System.Linq;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.Collision;

namespace Sopra.Logic.Pathfinding
{
    /// <summary>
    /// This class contains simple steering behaviours.
    /// Author: Nhat Minh Hoang
    /// </summary>
    internal sealed class Steering
    {
        private readonly Template mStaticTemplate = Template.All(typeof(StaticObjectC)).Build();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="movingDirection"></param>
        /// <param name="engine"></param>
        /// <param name="leftSide"></param>
        /// <returns></returns>
        internal bool CheckPossibleCollision(
            Vector2 currentPosition,
            Vector2 movingDirection,
            Engine engine,
            bool leftSide)
        {
            var perp = Collisions.OrthogonalVector(movingDirection, leftSide);

            var seekFlat = movingDirection + 2 * perp;
            seekFlat.Normalize();

            var seekBehindFlat = Collisions.OrthogonalVector(seekFlat, leftSide);

            var seekNarrow = 2 * movingDirection + perp;
            seekNarrow.Normalize();

            return CheckIntersection(currentPosition, perp, 64, engine)
                   || CheckIntersection(currentPosition, seekFlat, 64, engine)
                   || CheckIntersection(currentPosition, seekNarrow, 128, engine);
        }

        private bool CheckIntersection(Vector2 currentPosition, Vector2 seek, float length, Engine engine)
        {
            return engine.Collision
                .GetCollidingEntities(currentPosition, currentPosition + seek * length, mStaticTemplate).Any();
        }

        internal static Vector2 Avoid(
			SteeringC steerC,
			Vector2 movingDirection)
        {
            var leftPerp = Collisions.OrthogonalVector(movingDirection, true);
            var leftSeek = movingDirection + leftPerp;
            leftSeek.Normalize();
            var rightPerp = Collisions.OrthogonalVector(movingDirection, false);
            var rightSeek = movingDirection + rightPerp;
            rightSeek.Normalize();

            Vector2 next;

            if (steerC.TurnLeft)
            {
                next = movingDirection + leftPerp;
                next.Normalize();
                return next;
            }

            if (!steerC.TurnRight)
            {
                return Vector2.Zero;
            }

            next = movingDirection + rightPerp;
            next.Normalize();
            return next;
        }
    }
}