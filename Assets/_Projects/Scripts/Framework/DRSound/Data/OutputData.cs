using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound.Data
{
    [System.Serializable]
    public class OutputData
    {
        #region Serialized Fields

        [SerializeField] private string name;
        [SerializeField] private AudioMixerGroup output;

        #endregion

        #region Properties

        public string Name { get => name; set => name = value; }
        public AudioMixerGroup Output { get => output; set => output = value; }

        #endregion

        #region Public Methods

        public OutputData (string name, AudioMixerGroup output)
        {
            this.name = name;
            this.output = output;
        }

        #endregion
    }
}