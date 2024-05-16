using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sopra.UI;
using Microsoft.Xna.Framework.Content;
using Ninject;
using Sopra.Audio;
using Sopra.SaveState;

namespace Sopra.Screens.MenuScreens
{
    /// <summary>
    /// The Options screen class.
    /// </summary>
    /// <inheritdoc cref="AbstractScreen"/>
    /// <author>Anushe Glushik</author>

    // ReSharper disable once ClassNeverInstantiated.Global
    public class OptionMenu : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly ContentManager mContent;
        private readonly GraphicsDeviceManager mGraphics;
        private Texture2D mBackground;
        private Button mMusikAnAusBtn;
        private Button mSoundAnAusBtn;
        private Button mZurueckBtn;
        private Button mFullscreenBtn;
        private ButtonGroup mSoundLevel;
        private ButtonGroup mSongLevel;
        private Label mOptionsLabel;
        private readonly SoundManager mSound = SoundManager.Instance;
        private readonly SpriteBatch mSpriteBatch;
        private float mScreenscaleWidth;
        private float mScreenscaleHeight;
        private SpriteFont mFont;
        private SpriteFont mFont16;

        /// <summary>
        /// Options class constructor.
        /// </summary>
        /// <param name="menuActions"></param>
        /// <param name="content"></param>
        /// <param name="batch"></param>
        /// <param name="graphics"></param>
        public OptionMenu(MenuActions menuActions, ContentManager content, SpriteBatch batch, GraphicsDeviceManager graphics) : base(false, false)
        {
            mContent = content;
            mMenuActions = menuActions;
            mSpriteBatch = batch;
            mGraphics = graphics;
            // load textures for options screen
        }

        /// <summary>
        /// Initialize the Menu.
        /// </summary>
        public void Initialize()
        {
            LoadFonts();
            // Load a background texture
            mBackground = mContent.Load<Texture2D>("graphics/menu/background");
            CreateButtons();
            CreateLabel();
            CreateButtonGroup();
            Actions();
        }

        private void LoadFonts()
        {
            mFont = mContent.Load<SpriteFont>("fonts/arial_24");
            mFont16 = mContent.Load<SpriteFont>("fonts/arial_16");
        }
        private void CreateButtons()
        {
            // Load textures for buttons
            var musikAnAusBtnTe = mContent.Load<Texture2D>("graphics/options/button_musik-an-aus");
            var soundAnAusBtnTe = mContent.Load<Texture2D>("graphics/options/button_soundeffekte-an-aus");
            var zurueckBtnTe = mContent.Load<Texture2D>("graphics/options/button_zurueck");
            var fullscreenBtnTe = mContent.Load<Texture2D>("graphics/options/button_fullscreen");

            // Create buttons
            mMusikAnAusBtn = new Button(new Rectangle(500, 90, 240, 60), musikAnAusBtnTe, mFont);
            mSoundAnAusBtn = new Button(new Rectangle(500, 170, 240, 60), soundAnAusBtnTe, mFont);
            mFullscreenBtn = new Button(new Rectangle(500, 410, 240, 60), fullscreenBtnTe, mFont);
            mZurueckBtn = new Button(new Rectangle(500, 490, 240, 60), zurueckBtnTe, mFont);
        }

        private void CreateLabel()
        {
            // create label "Optionen"
            mOptionsLabel = new Label(new Rectangle(500, 30, 240, 60), mFont, "Optionen");
        }

        private void CreateButtonGroup()
        {
            // Load textires for the ButtonGroup
            var middleBtnTe = mContent.Load<Texture2D>("graphics/options/button");
            var leftBtnTe = mContent.Load<Texture2D>("graphics/options/triangle_left");
            var rightBtnTe = mContent.Load<Texture2D>("graphics/options/triangle_right");
            // all the possible sound levels list
            var soundList = new List<string> { "0%", "10%", "20%", "30%", "40%",
                                               "50%", "60%", "70%", "80%", "90%", "100%" };
            mSoundLevel = new ButtonGroup(new Rectangle(500, 330, 240, 60),
                                          leftBtnTe, middleBtnTe,
                                          rightBtnTe, mFont16,
                soundList, "Lautst. Effekte ");
            // all possible resolutions
            // var resolutionList = new List<string> { "400x300", "800x600", "1200x900" };
            mSongLevel = new ButtonGroup(new Rectangle(500, 250, 240, 60),
                                               leftBtnTe, middleBtnTe, rightBtnTe, mFont16,
                                               soundList, "Lautst. Musik ");
        }

        private void Actions()
        {
            // when "Zurueck" button is clicked - call action
            mZurueckBtn.mOnClick = ZuruekAction;
            mSoundLevel.mOnClickLeft = SoundLower;
            mSoundLevel.mOnClickRight = SoundHigher;

            // if button clicked do nothing by now
            mSongLevel.mOnClickLeft = SongLower;
            mSongLevel.mOnClickRight = SongHigher;

            mMusikAnAusBtn.mOnClick = DisableSong;
            mSoundAnAusBtn.mOnClick = DisableSound;

            mFullscreenBtn.mOnClick = ToggleFullscreen;

        }

        /// <summary>
        /// Draw every element in options screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, (int)(800 * mScreenscaleWidth), (int)(600 * mScreenscaleHeight)), Color.White);
            // mSteuerungBtn.Draw(spriteBatch, gameTime);
            mMusikAnAusBtn.Draw(spriteBatch);
            mSoundAnAusBtn.Draw(spriteBatch);
            // mTechDemoBtn.Draw(spriteBatch, gameTime);
            mZurueckBtn.Draw(spriteBatch);
            mOptionsLabel.Draw(spriteBatch);
            mSoundLevel.Draw(spriteBatch);
            mSongLevel.Draw(spriteBatch);
            mFullscreenBtn.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Updates buttons positions according to screen width.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            mScreenscaleWidth = mSpriteBatch.GraphicsDevice.Viewport.Width / 800f;
            mScreenscaleHeight = mSpriteBatch.GraphicsDevice.Viewport.Height / 600f;

            float screenWidth = mSpriteBatch.GraphicsDevice.Viewport.Width;
            var x = (int)(screenWidth - 300 * (screenWidth + 200) / 1000f);
            // mSteuerungBtn.Update(gameTime, x, mScreenscaleHeight);
            mMusikAnAusBtn.Update(x);
            mSoundAnAusBtn.Update(x);
            // mTechDemoBtn.Update(gameTime, x, mScreenscaleHeight);
            mZurueckBtn.Update(x);
            mSoundLevel.Update(x);
            mSongLevel.Update(x);
            mOptionsLabel.Update(x);
            mFullscreenBtn.Update(x);
        }

        /// <summary>
        /// Makes sound 10% higher.
        /// </summary>
        private void SoundHigher()
        {
            if (mSound.VolumeSound + 0.1f >= 1)
            {
                mSound.VolumeSound = 1f;
                mSound.SetSoundVolume();
            }
            else
            {
                mSound.VolumeSound += 0.1f;
                mSound.SetSoundVolume();
            }

            if (mSoundLevel.Index < mSoundLevel.mTextOnBtn.Count - 1)
            {
                mSoundLevel.Index++;
            }
        }


        /// <summary>
        /// Makes sound 10% lower.
        /// </summary>
        private void SoundLower()
        {
            if (mSound.VolumeSound - 0.1f <= 0)
            {
                mSound.VolumeSound = 0f;
                mSound.SetSoundVolume();

            }
            else
            {
                mSound.VolumeSound -= 0.1f;
                mSound.SetSoundVolume();
            }

            if (mSoundLevel.Index > 0)
            {
                mSoundLevel.Index--;
            }
        }

        /// <summary>
        /// Makes song 10% higher.
        /// </summary>
        private void SongHigher()
        {
            if (mSound.VolumeSong + 0.1f >= 1)
            {
                mSound.VolumeSong = 1f;
                mSound.SetSongVolume();
            }
            else
            {
                mSound.VolumeSong += 0.1f;
                mSound.SetSongVolume();
            }

            if (mSongLevel.Index < mSongLevel.mTextOnBtn.Count - 1)
            {
                mSongLevel.Index++;
            }
        }

        /// <summary>
        /// Makes song 10% lower.
        /// </summary>
        private void SongLower()
        {
            if (mSound.VolumeSong - 0.1f <= 0)
            {
                mSound.VolumeSong = 0f;
                mSound.SetSongVolume();

            }
            else
            {
                mSound.VolumeSong -= 0.1f;
                mSound.SetSongVolume();
            }

            if (mSongLevel.Index > 0)
            {
                mSongLevel.Index--;
            }
        }

        /// <summary>
        /// Disables the background song.
        /// </summary>
        private void DisableSong()
        {
            if (mSound.DisableSong == false)
            {
                mSound.StopSong();
                mMusikAnAusBtn.Color = Color.Gray;
                mSound.DisableSong = true;
            }
            else
            {
                mSound.DisableSong = false;
                mSound.EnableSong();
                mMusikAnAusBtn.Color = Color.White;
            }
        }

        /// <summary>
        /// Disables sounds effects.
        /// </summary>
        private void DisableSound()
        {
            mSound.DisableSound = !mSound.DisableSound;
            mSoundAnAusBtn.Color = mSound.DisableSound ? Color.Gray : Color.White;
        }

        /*
        private void SaveButtonGroup()
        {
            // Serialize the object data to a file
            Stream stream = File.Open("ButtonGroupData.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            // Send the object data to the file
            bf.Serialize(stream, mSoundLevel);
            stream.Close();

            // Delete the bowser data
            mSoundLevel = null;

            // Read object data from the file
            stream = File.Open("ButtonGroupData.dat", FileMode.Open);
            bf = new BinaryFormatter();

            mSoundLevel = (ButtonGroup)bf.Deserialize(stream);
            stream.Close();
        }*/

        private void ZuruekAction()
        {
            SaveState();
            mMenuActions.Remove(this);
        }

        /// <summary>
        /// The method saves Options Menu state in xml file.
        /// </summary>
        private void SaveState()
        {
            var state = new OptionsState
            {
                SoundIndex = mSoundLevel.Index,
                SongIndex = mSongLevel.Index,
                SoundAnAus = mSound.DisableSound,
                SongAnAus = mSound.DisableSong
            };
            state.Save("optionsState.xml");
        }

        /// <summary>
        /// The method loads Options Menu state from xml file.
        /// </summary>
        public void LoadState()
        {
            var state = OptionsState.LoadFromFile("optionsState.xml");
            mSoundLevel.Index = state.SoundIndex;
            mSongLevel.Index = state.SongIndex;
            mSound.VolumeSound = state.SoundIndex * 0.1f;
            mSound.VolumeSong = state.SongIndex * 0.1f;
            mSound.SetSongVolume();
            mSound.SetSoundVolume();
            mSound.DisableSound = state.SoundAnAus;
            mSound.DisableSong = state.SongAnAus;
            mSoundAnAusBtn.Color = mSound.DisableSound ? Color.Gray : Color.White;
            mMusikAnAusBtn.Color = mSound.DisableSong ? Color.Gray : Color.White;
        }

        public void SaveDefault()
        {
            var state = new OptionsState
            {
                SoundIndex = 5,
                SongIndex = 5
            };
            state.Save("optionsState.xml");
        }

        private void ToggleFullscreen()
        {
            mGraphics.ToggleFullScreen();
        }

    }
}