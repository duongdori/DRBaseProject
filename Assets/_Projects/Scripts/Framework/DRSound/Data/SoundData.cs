using UnityEngine;

namespace DR.DRSound.Data
{
    [System.Serializable]
    public class SoundData
    {
        #region Serialized Fields

        [SerializeField] private string tag;
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private CompressionPreset compressionPreset;

        [Tooltip("It will optimize the audio memory space, but it will no longer be stereo (not recommended for ambient music)")]
        [SerializeField] private bool forceToMono;

        #endregion

        #region Properties

        public string Tag { get => tag; set => tag = value; }
        public AudioClip[] Clips { get => clips; set => clips = value; }
        public CompressionPreset CompressionPreset { get => compressionPreset; set => compressionPreset = value; }
        public bool ForceToMono { get => forceToMono; set => forceToMono = value; }

        #endregion

        #region Public Methods

        public SoundData (string tag, AudioClip[] clips, CompressionPreset compressionPreset, bool forceToMono)
        {
            this.tag = tag;
            this.clips = clips;
            this.compressionPreset = compressionPreset;
            this.forceToMono = forceToMono;
        }

        public AudioClip GetClip ()
        {
            return clips[Random.Range(0, clips.Length)];
        }

        #endregion
    }
}