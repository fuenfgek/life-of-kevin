using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Sopra.Audio
{   /// <summary>This Class is storing SoundEffects
    /// and is responsible for playing, stoping, remove and update them.
    /// <author>Konstantin Fünfgelt</author>
    /// </summary>
    internal sealed class SoundDictionary
    {
        private readonly Dictionary<string, SoundEffect> mSounds = new Dictionary<string, SoundEffect>();
        private readonly Dictionary<string, Song> mSongs = new Dictionary<string, Song>();
        private readonly ArrayList mActiveSounds = new ArrayList();
        private readonly ArrayList mInactiveSounds = new ArrayList();

        private const int MaxInstaces = 10;

        /// <summary>
        /// Disable the sound in the game
        /// </summary>
        public bool DisableSound { get; set; }

        /// <summary>
        /// Disable Backgorundmusic in the game
        /// </summary>
        public bool DisableSong { get; set; }

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
        /// Set the Master Volume for SoundEffects 
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;

        }

        /// <summary>
        /// Set the Master Volume for Songs
        /// </summary>
        /// <param name="volume"></param>
        public void SetSongVolume(float volume)
        {
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
        public void UpdateList()
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
                    s.Stop();
                    mActiveSounds.Remove(s);
                }

                mInactiveSounds.Clear();
            }
        }

        /// <summary>
        /// Creates an Instance of a soundeffect and plays it
        /// </summary>
        /// <param name="name">Keyname of Soundeffect</param>
        public void PlaySound(string name)
        {
            if (!mSounds.ContainsKey(name) || DisableSound || mActiveSounds.Count >= MaxInstaces)
            {
                return;
            }
            var soundEffect = mSounds[name];
            var soundInstance = soundEffect.CreateInstance();
            soundInstance.Play();
            mActiveSounds.Add(soundInstance);
        }

        /// <summary>
        /// Creates an Instance of a soundeffect and plays it
        /// </summary>
        /// <param name="name">Keyname of Soundeffect</param>
        /// <param name="looped"></param>
        /// <param name="soundInstance"></param>
        public void PlaySound(string name, bool looped, out SoundEffectInstance soundInstance)
        {
            if (!mSounds.ContainsKey(name) || DisableSound || mActiveSounds.Count >= MaxInstaces)
            {
                soundInstance = null;
                return;
            }
            var soundEffect = mSounds[name];
            soundInstance = soundEffect.CreateInstance();
            soundInstance.IsLooped = looped;
            
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
        /// Stop Sound
        /// </summary>
        public void StoploopSound(SoundEffectInstance s)
        {
            if (!mActiveSounds.Contains(s))
            {
                return;
            }

            s.Stop();
            s.Dispose();
            mActiveSounds.Remove(s);
        }
    }
}
