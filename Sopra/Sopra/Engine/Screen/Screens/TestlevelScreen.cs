using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Sopra.Engine.ECS.Components;
using Sopra.Engine.ECS.Systems;
using Sopra.Engine.Screen;
using Sopra.Engine.UI;

namespace Sopra.Engine.Screen.Screens
{
    sealed class TestlevelScreen : AbstractScreen
    {
        private readonly Engine.ECS.Engine mEngine = new Engine.ECS.Engine();
        private Button mPauseBtn;
        public Action mOnClickPauseBtn;

        public TestlevelScreen(ContentManager content, SpriteBatch batch)
        {
            mEngine.AddSystem(new HealthSystem())
                .AddSystem(new PathFindingSystem())
                .AddSystem(new UserInputSystem())
                .AddSystem(new UserInteractableSystem());

            mEngine.AddRenderSystem(new BodyRenderSystem(batch));

            // If you change the order of player, enemy, healing the testing for Tuesday won't work anymore.
            // I marked every piece of code for testing with "Only for testing #Felix"
            var player = mEngine.EntityManager.Create()
                .AddComponent(new BodyRenderC(content.Load<Texture2D>("figure1"), 10.0f))
                .AddComponent(new TransformC(new Vector2(400, 100)))
                .AddComponent(new PathFindingC())
                .AddComponent(new HealthC(6))
                .AddComponent(new UserControllableC());

            var enemy = mEngine.EntityManager.Create()
                .AddComponent(new BodyRenderC(content.Load<Texture2D>("figure2"), 10.0f))
                .AddComponent(new TransformC(new Vector2(20, 20)))
                .AddComponent(new PathFindingC(new Vector2(400, 300)))
                .AddComponent(new HealthC(4));

            var healing = mEngine.EntityManager.Create()
                .AddComponent(new BodyRenderC(content.Load<Texture2D>("GreenBox"), 1.0f))
                .AddComponent(new TransformC(new Vector2(100, 350)))
                .AddComponent(new UserInteractableC(0));

            // add pause button (will be changed in the future)
            var pauseBtnTe = content.Load<Texture2D>("PauseScreen/Pause-Button");
            var font = content.Load<SpriteFont>("font1");
            mPauseBtn = new Button(new Rectangle(30, 30, 60, 60), pauseBtnTe, font);
            mPauseBtn.mOnClick = () => mOnClickPauseBtn?.Invoke();
        }


        /// <summary>
        /// This method draws the screen.
        /// Gets called automatically if the screen was added to the Screen Manager.
        /// Note that every screen has to call the begin and end method!
        /// </summary>
        /// <inheritdoc cref="IScreen"/>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            mEngine.Draw(spriteBatch, gameTime);
            spriteBatch.Begin();
            mPauseBtn.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }


        /// <summary>
        /// This method updates the screen.
        /// Gets called automatically if the screen was added to the Screen Manager.
        /// </summary>
        /// <inheritdoc cref="IScreen"/>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            mEngine.Update(gameTime);
        }
    }
}