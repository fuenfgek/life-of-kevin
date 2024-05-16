using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using Sopra.Input;
using Sopra.Logic.Achievements;
using Sopra.Maps;
using Sopra.SaveState;
using Sopra.UI;


namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// The screen that appears when Pause button or ESC are pressed.
    /// The game frozes while Pause Screen is opened.
    /// Only used in Techdemo.
    /// </summary>
    /// <inheritdoc cref="AbstractScreen"/>
    /// <author>Anushe Glushik</author>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SimplePauseMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private readonly SpriteBatch mSpriteBatch;
        private readonly LevelManager mLevelManager;
        private GameTime mGameTime;
        readonly Stats mStats = Stats.Instance;

        private Label mPauseLabel;
        private Texture2D mBackground;
        private Button mWeiterspielenBtn;
        private Button mHauptMenuBtn;
        private Button mOptionenBtn;
        private Button mBeendenBtn;
        private SpriteFont mFont;
        private float mScreenscaleWidth;
        private float mScreenscaleHeight;
        private int mWait;
        private bool mLoad;
        private SpriteFont mFont72;
        // private bool mIsSaving;
        // private Label mSaveLabel;

        /// <summary>
        /// Options class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        /// <param name="levelManager"></param>
        /// <param name="gameTime"></param>
        public SimplePauseMenu(MenuActions menuActions, ContentManager content, SpriteBatch batch, LevelManager levelManager, GameTime gameTime)
            : base(true, false)
        {
            mMenuActions = menuActions;
            mContent = content;
            mSpriteBatch = batch;
            mLevelManager = levelManager;
            mGameTime = gameTime;

            // load textures for options screen
        }

        public void Initialize()
        {
            CreateLabels();
            CreateButtons();
            mBackground = mContent.Load<Texture2D>("graphics/pause/grey");
            // mIsSaving = false;
            // mSaveLabel = new Label(new Rectangle(280, 250, 500, 200), mFont, "Saving...");
            Actions();
        }

        private void CreateLabels()
        {
            mFont = mContent.Load<SpriteFont>("fonts/arial_24");
            mFont72 = mContent.Load<SpriteFont>("fonts/arial_72");
            mPauseLabel = new Label(new Rectangle(280, 100, 240, 60), mFont, "Pause");
        }

        private void CreateButtons()
        {
            var weiterspielenBtnTe = mContent.Load<Texture2D>("graphics/pause/button_weiterspielen");
            var hauptMenuBtnTe = mContent.Load<Texture2D>("graphics/pause/button_hauptmenue");
            var optionenBtnTe = mContent.Load<Texture2D>("graphics/pause/button_optionen");
            var beendenBtnTe = mContent.Load<Texture2D>("graphics/menu/button_spiel-beenden");

            mWeiterspielenBtn = new Button(new Rectangle(280, 200, 240, 60), weiterspielenBtnTe, mFont);
            mHauptMenuBtn = new Button(new Rectangle(280, 270, 240, 60), hauptMenuBtnTe, mFont);
            mOptionenBtn = new Button(new Rectangle(280, 340, 240, 60), optionenBtnTe, mFont);
            mBeendenBtn = new Button(new Rectangle(280, 410, 240, 60), beendenBtnTe, mFont);
        }

        private void Actions()
        {
            // when "Zurueck" button is clicked - call action
            mWeiterspielenBtn.mOnClick = () => mMenuActions.Remove(this);
            mHauptMenuBtn.mOnClick = () => mMenuActions.OpenMainScreen();
            mOptionenBtn.mOnClick = () => mMenuActions.OpenOptionScreen();
            mBeendenBtn.mOnClick = () => mMenuActions.Exit();
        }

        /// <summary>
        /// Draw every element in options screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mSpriteBatch.GraphicsDevice.Viewport.Width,
                mSpriteBatch.GraphicsDevice.Viewport.Height), new Color(50, 50, 50, 50));
            mPauseLabel.Draw(spriteBatch);
            mWeiterspielenBtn.Draw(spriteBatch);
            mHauptMenuBtn.Draw(spriteBatch);
            mOptionenBtn.Draw(spriteBatch);
            mBeendenBtn.Draw(spriteBatch);
            if (mLoad)
            {
                var gameLoad = new Label(new Rectangle((int)(400 * mScreenscaleWidth) - 120, (int)(300 * mScreenscaleHeight) - 15, 240, 60), mFont72, "Loading...");
                gameLoad.Draw(spriteBatch);
                mWait++;
                IfSaveState();
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            mGameTime = gameTime;
            mScreenscaleWidth = mSpriteBatch.GraphicsDevice.Viewport.Width / 800f;
            mScreenscaleHeight = mSpriteBatch.GraphicsDevice.Viewport.Height / 600f;

            float screenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            var x = (int)(screenWidth / 2) - 120;
            mPauseLabel.Update(x);
            mWeiterspielenBtn.Update(x);
            mHauptMenuBtn.Update(x);
            mOptionenBtn.Update(x);
            mBeendenBtn.Update(x);
        }

        public override bool HandleKeyEvent(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Keys.Escape && keyEvent.State == KeyState.Up)
            {
                mMenuActions.Remove(this);
            }
            
            return false;
        }

        private void IfSaveState()
        {
            mLoad = true;
            if (mWait == 1)
            {
                SaveState();
            }
        }

        private void SaveState()
        {
            mLoad = false;
            var level = mLevelManager.CurrentLevel;
            
            var name = level.Map.Name;
            var entities = level.Engine.EntityManager.Entities; // dict->list
            var listEntitieStates = new List<EntityState>();
            foreach (var i in entities)
            {
                var entity = new EntityState(i.Value);
                listEntitieStates.Add(entity);
            }
            var state = new State
            {
                Name = name,
                Entities = listEntitieStates,
                GameTimeSec = mGameTime.TotalGameTime
            };
            if (mLevelManager.IsNotSavedYet) { MoveFilesFromNewToOld(); }
            
            state.Save("OldGame/hey.xml");
            mLevelManager.IsNotSavedYet = false;

            SaveStats();
        }

        private void SaveStats()
        {

            mStats.Save("stats.xml");
        }

        private void MoveFilesFromNewToOld()
        {
            // all xml files
            var fileName = "*.xml";
            var sourcePath = Directory.GetCurrentDirectory() + "/NewGame";
            var targetPath = Directory.GetCurrentDirectory() + "/OldGame";
            // if want to save new game need to delete old game saving first.
            foreach (var file in new DirectoryInfo(targetPath).GetFiles(fileName))
            {
                file.Delete();
            }
            // copy all the files from nes games to old games
            foreach (var file in new DirectoryInfo(sourcePath).GetFiles(fileName))
            {
                //var targetFolderName = file.CreationTime.ToString("OldGame");
                // var dir = Directory.CreateDirectory(Path.Combine(targetPath, targetFolderName));
                file.CopyTo(Path.Combine(targetPath, file.Name));
                // file.Delete();
            }
        }
    }
}