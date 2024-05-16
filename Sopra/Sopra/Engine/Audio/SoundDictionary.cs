using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Sopra.Engine.Audio
{   /// <summary>This Class is storing SoundEffects
    /// and is responsible for playing, stoping, remove and update them.
    /// <author>Konstantin Fünfgelt</author>
    /// </summary>
    class SoundDictionary
    {
        private readonly Dictionary<string, SoundEffect> mSounds = new Dictionary<string, SoundEffect>();
        private Dictionary<string, Song> mSongs = new Dictionary<string, Song>();
        private readonly ArrayList mActiveSounds = new ArrayList();
        private readonly ArrayList mInactiveSounds = new ArrayList();
        private bool mDisableSound;
        private bool mDisableSong;
        /// <summary>
        /// Disable the sound in the game
        /// </summary>
        public bool DisableSound
        {
            get { return mDisableSound; }
            set { mDisableSound = value; }
        }
        /// <summary>
        /// Disable Backgorundmusic in the game
        /// </summary>
        public bool DisableSong
        {
            get { return mDisableSong; }
            set { mDisableSong = value; }

        }
        /// <summary>
        /// Adding Sounds to the Sound Dictionary
        /// </summary>
        /// <param name="sound">SoundEffect to add</param>
        /// <param name="name">Keyname of soundeffect</param>
        public void AddSound(SoundEffect sound, string name)
        {
            if (!mSounds.ContainsKey(name))
            {
                mSounds.Add(name, sound);
            }
        }
        /// <summary>
        /// Set the Master Volume for SoundEffects und Songs
        /// </summary>
        /// <param name="volume"></param>
        public void SetMasterVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;
            MediaPlayer.Volume = volume;

        }
        /// <summary>
        /// Adding Song to Song Song Dictionary
        /// </summary>
        /// <param name="music"> Song </param>
        /// <param name="name">Name of the Song</param>
        public void AddSong(Song music, string name)
        {
            if (!mSongs.ContainsKey(name))
            {
                mSongs.Add(name, music);
            }
        }
        /// <summary>
        /// Remove unused Sounds
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateList(GameTime gameTime)
        {
            foreach (SoundEffectInstance s in mActiveSounds)
            {
                if (s.State == SoundState.Stopped)
                {
                    mInactiveSounds.Add(s);
                }
            }

            if (mInactiveSounds.Count <= 0)
            {
                return;
            }

            {
                foreach (SoundEffectInstance s in mInactiveSounds)
                {
                    s.Dispose();
                    mActiveSounds.Remove(s);
                }
            }
        }
        /// <summary>
        /// Unload all SoundEffects
        /// </summary>
        public void UnloadContent()
        {
            foreach (SoundEffect s in mSounds.Values)
            {
                s.Dispose();
            }
        }
        /// <summary>
        /// Creates an Instance of a soundeffect and plays it
        /// </summary>
        /// <param name="name">Keyname of Soundeffect</param>
        public void PlaySound(string name)
        {
            if (!mSounds.ContainsKey(name) || DisableSound)
            {
                return;
            }

            var soundEffect = mSounds[name];
            var soundInstance = soundEffect.CreateInstance();
            soundInstance.Play();
            mActiveSounds.Add(soundInstance);
        }
        /// <summary>
        /// Plays choosen song in the Dictionary
        /// </summary>
        /// <param name="name">Songname</param>
        public void PlaySong(string name)
        {
            if (!mSongs.ContainsKey(name) || DisableSong)
            {
                return;
            }

            var song = mSongs[name];
            MediaPlayer.Play(song);
        }
        /// <summary>
        /// Stops all Sounds
        /// </summary>
        public void StopAllSounds()
        {
            foreach (SoundEffectInstance s in mActiveSounds)
            {
                s.Stop();
                s.Dispose();
            }
            mActiveSounds.Clear();

        }
        /// <summary>
        /// Check if soundeffect is already in dictionary
        /// </summary>
        /// <param name="name">keyname of Soundeffect</param>
        /// <returns></returns>
        public bool ContainsSound(string name)
        {
            return mSounds.ContainsKey(name);
        }
        /// <summary>
        /// Check if song is in Dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsSong(string name)
        {
            return mSongs.ContainsKey(name);
        }


    }
}
