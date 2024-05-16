using Microsoft.Xna.Framework;

namespace Sopra.UI
{
    public class MenuElement
    {
        public Rectangle ElementRectangle { get; set; }

        protected MenuElement(Rectangle elementRectangle)
        {
            ElementRectangle = elementRectangle;
        }
    }
}
