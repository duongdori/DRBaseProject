using System.Collections.Generic;
using System.Linq;
using DR.DRSound.Data;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound
{
    public class AudioManager : MonoBehaviour
    {
        #region Fields

        private static AudioToolsLibrary _audioToolsLibrary = new();

        private static readonly List<SourcePoolElement> AudioSourcePool = new();

        private static GameObject _audioSourcesPoolParent;

        #endregion

        #region Internal Static Methods

        internal static SourcePoolElement GetSource()
        {
            return GetSoundSourceFromPool();
        }

        internal static AudioMixerGroup GetOutput(Output output)
        {
            if (_audioToolsLibrary.OutputCollection == null) return null;
            return _audioToolsLibrary.OutputCollection.GetOutput(output.ToString());
        }

        internal static AudioClip GetSFX(string tag)
        {
            if (_audioToolsLibrary.SoundDataCollection == null) return null;
            SoundData soundData = _audioToolsLibrary.SoundDataCollection.GetSound(tag);
            return soundData.GetClip();
        }

        internal static AudioClip GetTrack(string tag)
        {
            if (_audioToolsLibrary.MusicDataCollection == null) return null;
            SoundData soundData = _audioToolsLibrary.MusicDataCollection.GetMusicTrack(tag);
            return soundData.GetClip();
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Get last saved output volume.
        /// </summary>
        /// <param name="output">Target output</param>
        public static float GetLastSavedOutputVolume (Output output)
        {
            if (PlayerPrefs.HasKey(output.ToString())) return PlayerPrefs.GetFloat(output.ToString());
            Debug.LogWarning($"The {output.ToString()}'s volume has not been saved yet");
            return 0.5f;
        }
        
        /// <summary>
        /// Change Output volume.
        /// </summary>
        /// <param name="output">Target output</param>
        /// <param name="value">Target volume: min 0, Max: 1</param>
        public static void ChangeOutputVolume (Output output, float value)
        {
            AudioMixer mixer = GetOutput(output).audioMixer;

            if (mixer == null)
            {
                Debug.LogError($"Can't change mixer volume because {output.ToString()} don't exist." +
                               $"Make sure you have updated the outputs database on Outputs Manager window");
                return;
            }
            
            GetOutput(output).audioMixer.SetFloat(output.ToString(), 
                Mathf.Log10(Mathf.Clamp(value, 0.001f, 0.99f)) * 20);
            PlayerPrefs.SetFloat(output.ToString(), value);
        }

        /// <summary>
        /// Pause all sounds, music, dynamic music and playlists.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void PauseAll (float fadeOutTime = 0)
        {
            PauseAllSounds(fadeOutTime);
            PauseAllMusic(fadeOutTime);
        }
        
        /// <summary>
        /// Pause all sounds.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void PauseAllSounds (float fadeOutTime = 0)
        {
            foreach (SourcePoolElement sourcePoolElement in AudioSourcePool)
            {
                sourcePoolElement.Pause(fadeOutTime);
            }
        }
        
        /// <summary>
        /// Pause sound without having the sound reference.
        /// </summary>
        /// <param name="id">The Id you've set to the sound</param>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void PauseSound (string id, float fadeOutTime = 0)
        {
            var sourcePoolElement = AudioSourcePool.FirstOrDefault(sourceElement => sourceElement.Id == id);
            if (sourcePoolElement == null || sourcePoolElement.Using)
            {
                Debug.LogWarning($"There is no sound reproducing with the id '{id}'");
                return;
            }
            sourcePoolElement.Pause(fadeOutTime);
        }
        
        /// <summary>
        /// Pause all music, dynamic music and playlists.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void PauseAllMusic (float fadeOutTime = 0)
        {
            foreach (SourcePoolElement sourcePoolElement in AudioSourcePool)
            {
                sourcePoolElement.Pause(fadeOutTime);
            }
        }
        
        /// <summary>
        /// Pause music, dynamic music or playlist without having the sound reference.
        /// </summary>
        /// <param name="id">The Id you've set to the music</param>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void PauseMusic (string id, float fadeOutTime = 0)
        {
            var sourcePoolElement = AudioSourcePool.FirstOrDefault(sourceElement => sourceElement.Id == id);
            if (sourcePoolElement == null) 
            {
                Debug.LogWarning($"There is no music with the id '{id}'");
                return;
            }
            sourcePoolElement.Pause(fadeOutTime);
        }
        
        
        /// <summary>
        /// Stop all sounds, music, dynamic music and playlists.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void StopAll (float fadeOutTime = 0)
        {
            StopAllSounds(fadeOutTime);
            StopAllMusic(fadeOutTime);
        }
        
        /// <summary>
        /// Stop all sounds.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void StopAllSounds (float fadeOutTime = 0)
        {
            foreach (SourcePoolElement sourcePoolElement in AudioSourcePool)
            {
                sourcePoolElement.Stop(fadeOutTime);
            }
        }
        
        /// <summary>
        /// Stop sound without having the sound reference.
        /// </summary>
        /// <param name="id">The Id you've set to the sound</param>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void StopSound (string id, float fadeOutTime = 0)
        {
            var sourcePoolElement = AudioSourcePool.FirstOrDefault(sourceElement => sourceElement.Id == id);
            if (sourcePoolElement == null || sourcePoolElement.Using)
            {
                Debug.LogWarning($"There is no sound reproducing with the id '{id}'");
                return;
            }
            sourcePoolElement.Stop(fadeOutTime);
        }
        
        /// <summary>
        /// Stop all music, dynamic music and playlists.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void StopAllMusic (float fadeOutTime = 0)
        {
            foreach (SourcePoolElement sourcePoolElement in AudioSourcePool)
            {
                sourcePoolElement.Stop(fadeOutTime);
            }
        }
        
        /// <summary>
        /// Stop music, dynamic music or playlist without having the sound reference.
        /// </summary>
        /// <param name="id">The Id you've set to the music</param>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public static void StopMusic (string id, float fadeOutTime = 0)
        {
            var sourcePoolElement = AudioSourcePool.FirstOrDefault(sourceElement => sourceElement.Id == id);
            if (sourcePoolElement == null) 
            {
                Debug.LogWarning($"There is no music with the id '{id}'");
                return;
            }
            sourcePoolElement.Stop(fadeOutTime);
        }
        #endregion

        #region Private Methods

        private static SourcePoolElement GetSoundSourceFromPool ()
        {
            if (_audioSourcesPoolParent == null || !_audioSourcesPoolParent.activeInHierarchy)
            {
                _audioSourcesPoolParent = new GameObject("Sources Pool Parent");
                DontDestroyOnLoad(_audioSourcesPoolParent);
            }

            if (AudioSourcePool.Count != _audioSourcesPoolParent.transform.childCount)
            {
                AudioSourcePool.Clear();
            }
            
            foreach (SourcePoolElement element in AudioSourcePool)
            {
                if (!element.Using) return element;
            }

            GameObject newSourceInstance = new GameObject($"Audio Source {AudioSourcePool.Count}");
            DontDestroyOnLoad(newSourceInstance);
            newSourceInstance.transform.SetParent(_audioSourcesPoolParent.transform);

            AudioSource source = newSourceInstance.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.dopplerLevel = 0;
            SourcePoolElement sourceElement = newSourceInstance.AddComponent<SourcePoolElement>().Init(source);

            AudioSourcePool.Add(sourceElement);
            return sourceElement;
        }

        #endregion
    }
}