using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Logic
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DebugHelpers
    {
        public static void DrawRect(SpriteBatch batch, Rectangle rectangle, Color color)
        {
            var vertices = new[]
            {
                new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y, 1), color),
                new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y + rectangle.Height, 1), color),
                new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 1), color),
                new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y, 1), color),
                new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y, 1), color)
            };
            
            batch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
        }
    }
}