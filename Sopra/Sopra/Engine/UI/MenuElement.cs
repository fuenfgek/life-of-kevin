using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sopra.Engine.UI
{
    class MenuElement
    {
        public Rectangle ElementRectangle { get; set; }

        public MenuElement(Rectangle elementRectangle)
        {
            ElementRectangle = elementRectangle;
        }

        void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        void Update(GameTime gameTime)
        {

        }
    }
}
