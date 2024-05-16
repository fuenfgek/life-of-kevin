using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Sopra.Audio
{
    /// <summary>
    ///The SoundManager is responsible for managing all the sounds in the game.
    /// <author>Konstantin Fünfgelt</author>
    /// </summary>
    public sealed class SoundManager
    {
        private static SoundManager sInstance;
        private readonly SoundDictionary mSoundDirectory = new SoundDictionary();
        private float mVolumeSound = 0.5f;
        private float mVolumeSong = 0.5f;
        private string mActualSong;
        private SoundManager()
        {
            SetSoundVolume();
            SetSongVolume();
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
        /// <summary>
        /// Setter and getter for the SoundVolume
        /// </summary>
        public float VolumeSound
        {
            get { return mVolumeSound; }
            set { mVolumeSound = value; }
        }

        /// <summary>
        /// Setter and getter for the SongVolume
        /// </summary>
        public float VolumeSong
        {
            get { return mVolumeSong; }
            set { mVolumeSong = value; }
        }

        /// <summary>
        ///Sets the acutal Volume
        /// </summary>
        public void SetSoundVolume()
        {
            mSoundDirectory.SetSoundVolume(mVolumeSound);
        }

        /// <summary>
        ///Sets the acutal Volume
        /// </summary>
        public void SetSongVolume()
        {
            mSoundDirectory.SetSongVolume(mVolumeSong);
        }

        /// <summary>
        /// Loading all soundfiles and adding them in the SoundDirectory
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            //In Progress
            var di = new DirectoryInfo($@"{content.RootDirectory}/audio/sounds");
            var files = di.GetFiles();
            var di1 = new DirectoryInfo($@"{content.RootDirectory}/audio/songs");
            var files1 = di1.GetFiles();
            foreach (var f in files)
            {
                var filename = Path.GetFileNameWithoutExtension(f.Name);
                mSoundDirectory.AddSound(
                    content.Load<SoundEffect>($@"audio/sounds/{filename}"),
                    filename);
            }

            foreach (var f in files1)
            {
                var filename = Path.GetFileNameWithoutExtension(f.Name);
                mSoundDirectory.AddSong(content.Load<Song>(@"audio/songs/" + filename), filename);
            }
        }

        public void Update()
        {
            mSoundDirectory.UpdateList();
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
        /// Play the entered SoundEffect
        /// </summary>
        /// <param name="name">Keyname for the desired SoundEffect</param>
        /// <param name="loop"></param>
        /// <param name="instance"></param>
        public void PlaySound(string name, bool loop, out SoundEffectInstance instance)
        {  
         mSoundDirectory.PlaySound(name, loop, out instance);
        }

        /// <summary>
        /// Stop the entered SoundEffect
        /// </summary>
        public void StopAllSounds()
        {
            mSoundDirectory.StopAllSounds();
        }

        /// <summary>
        /// Plays song choosen song
        /// </summary>
        /// <param name="name">Songname</param>
        public void PlaySong(string name)
        {
            mActualSong = name;
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
        /// <summary>
        /// Enable last played Song
        /// </summary>
        public void EnableSong()
        {
            if (mSoundDirectory.DisableSong)
            {
                return;
            }
            PlaySong(mActualSong);
        }

        /// <summary>
        /// Stops all Sounds
        /// </summary>
        public void StoploopSound(SoundEffectInstance s)
        {
           mSoundDirectory.StoploopSound(s);
        }
    }
}
