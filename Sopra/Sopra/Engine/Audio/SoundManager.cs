using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Sopra.Engine.Audio
{
    /// <summary>
    ///The SoundManager is responsible for managing all the sounds in the game.
    /// <author>Konstantin Fünfgelt</author>
    /// </summary>
   public sealed class SoundManager
    {
        private static SoundManager sInstance;
        private readonly SoundDictionary mSoundDirectory = new SoundDictionary();
        private float mVolume;
        private SoundManager()
        { }

        /// <summary>
        ///Disable Sound in the game.
        /// </summary>
        public bool DisableSound
        {
            get { return mSoundDirectory.DisableSound; }
            set { mSoundDirectory.DisableSound = value; }
        }
        /// <summary>
        /// Disable the Songs in the game
        /// </summary>
        public bool DisableSong
        {
            get { return mSoundDirectory.DisableSong; }
            set { mSoundDirectory.DisableSong = value; }
        }

        public float Volume
        {
            get { return mVolume; }
            set { mVolume = value; }
        }

        public void SetMasterVolume()
        {
            mSoundDirectory.SetMasterVolume(mVolume);
        }
        /// <summary>
        /// Singleton for this class.
        /// </summary>
        public static SoundManager Instance
        {
            get
            {
                if (sInstance != null)
                {
                    return sInstance;
                }

                sInstance = new SoundManager();
                return sInstance;
            }
        }
        /// <summary>
        /// Loading all soundfiles and adding them in the SoundDirectory
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            //In Progress
            var di = new DirectoryInfo($@"{content.RootDirectory}\Audio\Sounds");
            var files = di.GetFiles();
            var di1 = new DirectoryInfo($@"{content.RootDirectory}\Audio\Songs");
            var files1 = di1.GetFiles();
            foreach (var f in files)
            {
                var filename = Path.GetFileNameWithoutExtension(f.Name);
                mSoundDirectory.AddSound(
                    content.Load<SoundEffect>($@"Audio\Sounds\{filename}"),
                    filename);
            }

            foreach (var f in files1)
            {
                var filename = Path.GetFileNameWithoutExtension(f.Name);
                mSoundDirectory.AddSong(content.Load<Song>(@"Audio\Songs\" + filename), filename);
            }
        }
        /// <summary>
        /// Unloading all SoundEffects
        /// </summary>
        public void UnloadContent()
        {
            mSoundDirectory.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            mSoundDirectory.UpdateList(gameTime);
        }
        /// <summary>
        /// Play the entered SoundEffect
        /// </summary>
        /// <param name="name">Keyname for the desired SoundEffect</param>
        public void PlaySound(string name)
        {
            mSoundDirectory.PlaySound(name);
        }
        /// <summary>
        /// Plays song choosen song
        /// </summary>
        /// <param name="name">Songname</param>
        public void PlaySong(string name)
        {
            mSoundDirectory.PlaySong(name);
            MediaPlayer.IsRepeating = true;
        }
        /// <summary>
        /// Stops actual song
        /// </summary>
        public void StopSong()
        {
            MediaPlayer.Stop();
        }

    }
}
