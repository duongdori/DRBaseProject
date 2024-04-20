#if UNITY_EDITOR
using System.Text.RegularExpressions;
using DR.DRSound.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound.Editor
{
    public class EditorWindowTools : EditorWindow
    {
        internal static void ChangeAudioClipImportSettings (AudioClip[] clips, CompressionPreset preset, bool forceMono)
        {
            foreach (AudioClip clip in clips)
            {
                AudioImporter importer = (AudioImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip));
                if (importer == null) return;
                
                AudioImporterSampleSettings sampleSettings = importer.defaultSampleSettings;
                switch (preset)
                {
                    case CompressionPreset.AmbientMusic:
                        bool shortDuration = clip.length < 10;
                        sampleSettings.loadType = shortDuration ? 
                            AudioClipLoadType.CompressedInMemory : AudioClipLoadType.Streaming;
                        sampleSettings.compressionFormat = shortDuration ? 
                            AudioCompressionFormat.ADPCM : AudioCompressionFormat.Vorbis;
                        sampleSettings.quality = 0.60f;
                        break;
                    case CompressionPreset.FrequentSound:
                        sampleSettings.loadType = AudioClipLoadType.DecompressOnLoad;
                        sampleSettings.compressionFormat = AudioCompressionFormat.ADPCM;
                        sampleSettings.quality = 1f;
                        break;
                    case CompressionPreset.OccasionalSound:
                        sampleSettings.loadType = AudioClipLoadType.CompressedInMemory;
                        sampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
                        sampleSettings.quality = 0.35f;
                        break;
                }
                importer.forceToMono = forceMono;
#if UNITY_2022_1_OR_NEWER
                sampleSettings.preloadAudioData = true;
#else
                importer.preloadAudioData = true;
#endif
                importer.loadInBackground = true;
                importer.defaultSampleSettings = sampleSettings;
                importer.SaveAndReimport();
            }
        }
        
        [MenuItem("Tools/DR/Setup DRSound")]
        private static void SetupDRSound()
        {
            bool allRight = true;
            
            string resourcesFolderPath = "Assets/Resources/DRSound";
            if (!AssetDatabase.IsValidFolder(resourcesFolderPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                AssetDatabase.CreateFolder("Assets/Resources", "DRSound");
            }
            
            SoundDataCollection soundDataCollection = Resources.Load<SoundDataCollection>("DRSound/SoundCollection");
            MusicDataCollection musicDataCollection = Resources.Load<MusicDataCollection>("DRSound/MusicCollection");
            
            if (soundDataCollection == null)
            {
                Debug.Log("Sound Data Collection was not found. A new one has been created at the following path:\n" + resourcesFolderPath);
                soundDataCollection = CreateInstance<SoundDataCollection>();
                AssetDatabase.CreateAsset(soundDataCollection, $"{resourcesFolderPath}/SoundCollection.asset");
                allRight = false;
            }
            if (musicDataCollection == null)
            {
                Debug.Log("Music Data Collection was not found. A new one has been created at the following path:\n" + resourcesFolderPath);
                musicDataCollection = CreateInstance<MusicDataCollection>();
                AssetDatabase.CreateAsset(musicDataCollection, $"{resourcesFolderPath}/MusicCollection.asset");
                allRight = false;
            }
            
            string path = "Assets/Resources/DRSound/Outputs";
            if (!AssetDatabase.IsValidFolder(path))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                if (!AssetDatabase.IsValidFolder("Assets/Resources/DRSound"))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", "DRSound");
                }
                AssetDatabase.CreateFolder("Assets/Resources/DRSound", "Outputs");
            }
            
            OutputCollection outputCollection = Resources.Load<OutputCollection>($"DRSound/Outputs/OutputCollection");
            if (outputCollection == null)
            {
                Debug.Log("Output Collection was not found. A new one has been created at the following path:\n" + path);
                outputCollection = CreateInstance<OutputCollection>();
                AssetDatabase.CreateAsset(outputCollection, $"{path}/OutputCollection.asset");
                EditorUtility.SetDirty(outputCollection);
                allRight = false;
            }

            AudioMixer mixer = Resources.Load<AudioMixer>("DRSound/Outputs/Master");
            if (mixer == null)
            {
                Debug.LogWarning($"Master Audio Mixer was not found. You must create " +
                                 $"it with the name 'Master' on the following path:\n{path}");
                allRight = false;
            }

            Debug.Log(!allRight ? "Setup DRSound successfully!" : "DRSound is already set up.");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        internal static bool IsTagValid (string tag)
        {
            if (string.IsNullOrEmpty(tag)) return false;
            if (Regex.IsMatch(tag, @"[^a-zA-Z0-9]")) return false;
            if (Regex.IsMatch(tag, "^[0-9]")) return false;
            if (!Regex.IsMatch(tag, @"[a-zA-Z]")) return false;
            return true;
        }
    }
}
#endif
