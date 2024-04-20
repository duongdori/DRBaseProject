using System;
using UnityEngine;

namespace DR.DRSound.Data
{
    public class AudioToolsLibrary
    {
        #region Properties
        public SoundDataCollection SoundDataCollection { get; private set; }
        public MusicDataCollection MusicDataCollection { get; private set; }
        public OutputCollection OutputCollection { get; private set; }

        #endregion
        
        public AudioToolsLibrary ()
        {
            Init();
        }

        private void Init ()
        {
            try
            {
                SoundDataCollection = Resources.Load<SoundDataCollection>("DRSound/SoundCollection");
                MusicDataCollection = Resources.Load<MusicDataCollection>("DRSound/MusicCollection");
                OutputCollection = Resources.Load<OutputCollection>("DRSound/Outputs/OutputCollection");
            }
            catch (Exception e)
            {
                Debug.LogError("You're trying to initialize a Sound, Music, Playlist, Dynamic Music or Output in its declaration. " +
                               "Please, move the initialization to Start or Awake methods\n\n" + e);
                return;
            }

            if (SoundDataCollection == null || MusicDataCollection == null)
            {
                Debug.LogError("Audio resources haven't found. " +
                               "Open [Tools > DR > Setup DRSound] to solve this problem.");
                return;
            }

            SoundDataCollection.Init();
            MusicDataCollection.Init();
            OutputCollection.Init();
        }
        
    }
}