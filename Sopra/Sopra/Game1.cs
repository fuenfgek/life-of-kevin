using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Ninject.Extensions.Factory;
using Sopra.Audio;
using Sopra.ECS;
using Sopra.Input;
using Sopra.Logic;
using Sopra.Maps;
using Sopra.Screens;

namespace Sopra
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public sealed class Game1 : Game
    {
        private GraphicsDeviceManager Graphics { get; }
        private readonly IKernel mKernel;
        private SpriteBatch mSpriteBatch;
        private InputManager mInput;
        private ScreenManager mScreenManager;
        internal Game1()
        {
            Window.AllowUserResizing = true;

            Graphics = new GraphicsDeviceManager(this)
            {
                // set this value to the desired width of your window
                PreferredBackBufferWidth = 1280,
                // set this value to the desired height of your window
                PreferredBackBufferHeight = 720,
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            mKernel = new StandardKernel();
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
           
            SetBindings();

            LoadBlueprints();

            mInput = mKernel.Get<InputManager>();
            mKernel.Get<MenuActions>().OpenMainScreen();
            mScreenManager = mKernel.Get<ScreenManager>();
        }

        protected override void UnloadContent()
        {
        }

        private void SetBindings()
        {
            mKernel.Bind<Game1>().ToConstant(this);
            mKernel.Bind<SpriteBatch>().ToConstant(mSpriteBatch);
            mKernel.Bind<GraphicsDeviceManager>().ToConstant(Graphics);
            mKernel.Bind<ContentManager>().ToConstant(Content);


            mKernel.Bind<IScreenFactory>().ToFactory();

            mKernel.Bind<MapFactory>().ToSelf();
            mKernel.Bind<LevelManager>().ToSelf().InSingletonScope();
            
            SoundManager.Instance.LoadContent(Content);

            mKernel.Bind<InputManager>().ToMethod(context => InputManager.Get());
            mKernel.Bind<SoundManager>().ToMethod(context => SoundManager.Instance).InSingletonScope();
            mKernel.Bind<ScreenManager>().ToSelf().InSingletonScope();
            mKernel.Bind<Events>().ToConstant(Events.Instance);
            mKernel.Bind<EntityFactory>().ToSelf().InSingletonScope();
            mKernel.Bind<MenuActions>().ToSelf().InSingletonScope();

            mKernel.Bind<Engine>().ToProvider<EngineFactory>().InSingletonScope();
            mKernel.Get<Engine>();
        }

        private void LoadBlueprints()
        {
            var factory = mKernel.Get<EntityFactory>();
            new Blueprints(factory).CreateBlueprints();
        }

        protected override void Update(GameTime gameTime)
        {
            mInput.Update();
            mScreenManager.Update(gameTime);
            SoundManager.Instance.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            mScreenManager.Draw(mSpriteBatch);

            base.Draw(gameTime);
        }
    }
}