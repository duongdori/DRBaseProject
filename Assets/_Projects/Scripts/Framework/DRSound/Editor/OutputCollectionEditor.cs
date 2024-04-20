#if UNITY_EDITOR
using DR.DRSound.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound.Editor
{
    [CustomEditor(typeof(OutputCollection))]
    public class OutputCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("UPDATE OUTPUTS"))
            {
                OutputCollection outputCollection = (OutputCollection)target;
                AudioMixer mixer = Resources.Load<AudioMixer>("DRSound/Outputs/Master");
                if(mixer == null)
                {
                    Debug.LogWarning("Master mixer not found!");
                    return;
                }
                
                outputCollection.LoadOutputs(mixer);
                GenerateEnum(outputCollection);
                
                EditorUtility.SetDirty(outputCollection);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        
        private void GenerateEnum(OutputCollection outputCollection)
        {
            string[] outputNames = new string[outputCollection.Outputs.Length];

            for (int i = 0; i < outputCollection.Outputs.Length; i++)
            {
                outputNames[i] = outputCollection.Outputs[i].Name.Replace(" ", "");
            }
            
            using (EnumGenerator enumGenerator = new EnumGenerator())
            {
                enumGenerator.GenerateEnum("Output", outputNames);
            }
        }
    }
}
#endif
