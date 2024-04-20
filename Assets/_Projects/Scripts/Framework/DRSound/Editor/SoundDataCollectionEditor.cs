#if UNITY_EDITOR
using DR.DRSound.Data;
using UnityEditor;
using UnityEngine;

namespace DR.DRSound.Editor
{
    [CustomEditor(typeof(SoundDataCollection))]
    public class SoundDataCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            SoundDataCollection soundDataCollection = (SoundDataCollection)target;

            if (GUILayout.Button("UPDATE SOUND SETTINGS"))
            {
                foreach (SoundData soundData in soundDataCollection.Sounds)
                {
                    EditorWindowTools.ChangeAudioClipImportSettings(soundData.Clips, soundData.CompressionPreset, soundData.ForceToMono);
                }
                EditorUtility.SetDirty(soundDataCollection);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            GUILayout.Space(10f);
            
            if (GUILayout.Button("GENERATE TAGS"))
            {
                var tags = new string[soundDataCollection.Sounds.Length];
                
                for (int i = 0; i < soundDataCollection.Sounds.Length; i++)
                {
                    SoundData soundData = soundDataCollection.Sounds[i];
                    if (string.IsNullOrEmpty(soundData.Tag))
                    {
                        Debug.LogError($"Tag of SoundData {i} cannot be empty");
                        return;
                    }

                    if (!string.IsNullOrEmpty(soundData.Tag) && !EditorWindowTools.IsTagValid(soundData.Tag))
                    {
                        Debug.LogError(
                            $"Tag: {soundData.Tag} of SoundData {i} cannot contain special characters or start with a number");
                        return;
                    }
                    
                    tags[i] = soundData.Tag;
                }
                
                using (EnumGenerator enumGenerator = new EnumGenerator())
                {
                    enumGenerator.GenerateEnum("SFX", tags);
                }
                EditorUtility.SetDirty(soundDataCollection);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }   
        }
    }
}
#endif
