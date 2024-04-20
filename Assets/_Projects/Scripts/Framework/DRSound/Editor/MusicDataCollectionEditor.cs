#if UNITY_EDITOR
using DR.DRSound.Data;
using UnityEditor;
using UnityEngine;

namespace DR.DRSound.Editor
{
    [CustomEditor(typeof(MusicDataCollection))]
    public class MusicDataCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            MusicDataCollection musicDataCollection = (MusicDataCollection)target;

            if (GUILayout.Button("UPDATE MUSIC SETTINGS"))
            {
                foreach (SoundData soundData in musicDataCollection.MusicTracks)
                {
                    EditorWindowTools.ChangeAudioClipImportSettings(soundData.Clips, soundData.CompressionPreset, soundData.ForceToMono);
                }
                EditorUtility.SetDirty(musicDataCollection);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            GUILayout.Space(10f);
            
            if (GUILayout.Button("GENERATE TAGS"))
            {
                var tags = new string[musicDataCollection.MusicTracks.Length];
                
                for (int i = 0; i < musicDataCollection.MusicTracks.Length; i++)
                {
                    SoundData musicTrack = musicDataCollection.MusicTracks[i];
                    if (string.IsNullOrEmpty(musicTrack.Tag))
                    {
                        Debug.LogError($"Tag of MusicTrack {i} cannot be empty");
                        return;
                    }

                    if (!string.IsNullOrEmpty(musicTrack.Tag) && !EditorWindowTools.IsTagValid(musicTrack.Tag))
                    {
                        Debug.LogError(
                            $"Tag: {musicTrack.Tag} of MusicTrack {i} cannot contain special characters or start with a number");
                        return;
                    }
                    
                    tags[i] = musicTrack.Tag;
                }
                
                using (EnumGenerator enumGenerator = new EnumGenerator())
                {
                    enumGenerator.GenerateEnum("Track", tags);
                }
                EditorUtility.SetDirty(musicDataCollection);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }   
        }
    }
}
#endif
