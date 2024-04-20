using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound.Data
{
    public class OutputCollection : ScriptableObject
    {
        [SerializeField] private OutputData[] outputs = Array.Empty<OutputData>();

        private readonly Dictionary<string, OutputData> outputsDictionary = new();

        public OutputData[] Outputs => outputs;

        public void Init ()
        {
            foreach (OutputData outputData in outputs)
            {
                outputsDictionary.Add(outputData.Name, outputData);
            }
        }

        public void LoadOutputs(AudioMixer mixer)
        {
            // AudioMixer mixer = Resources.Load<AudioMixer>("DRSound/Outputs/Master");
            AudioMixerGroup[] mixerGroups = mixer.FindMatchingGroups(null);
            OutputData[] loadedOutputs = new OutputData[mixerGroups.Length];
            for (int i = 0; i < loadedOutputs.Length; i++)
            {
                OutputData newOutputData = new OutputData(mixerGroups[i].name.Replace(" ", ""), mixerGroups[i]);
                loadedOutputs[i] = newOutputData;
                Debug.Log($"Output {i} '{newOutputData.Name}' saved!");
            }
            outputs = loadedOutputs;
        }
        
        public AudioMixerGroup GetOutput (string name)
        {
            if (outputsDictionary.TryGetValue(name.Replace(" ", ""), out OutputData outputData)) return outputData.Output;
            
            Debug.LogWarning($"Output with tag '{name}' don't exist");
            return null;
        }
    }
}