using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sopra.Engine.Input;
using Sopra.Engine.Maps;

namespace Sopra.Engine.Screen
{
    public class GameScreen : AbstractScreen
    {
        private readonly ContentManager mContent;
        private ECS.Engine mEngine;
        private World mWorld;
        private Map mMap;

        public GameScreen(ContentManager content)
            : base(false, false)
        {
            mContent = content;
            mEngine = new ECS.Engine();
            mWorld = new World(
                mEngine,
                new List<Layer>()
            );

            mMap = new MapFactory(content).Load("Content/untitled.tmx");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //mWorld.Draw(spriteBatch, gameTime);
            spriteBatch.Begin();
            mMap.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            //mWorld.Update(gameTime);
        }

        public override bool HandleKeyEvent(KeyEvent keyEvent)
        {
            return true;
        }

        public override bool HandleMouseEvent(MouseEvent mouseEvent)
        {
            return true;
        }
    }
}