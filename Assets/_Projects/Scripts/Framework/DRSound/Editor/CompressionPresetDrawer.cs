#if UNITY_EDITOR
using DR.DRSound.Data;
using UnityEditor;
using UnityEngine;

namespace DR.DRSound.Editor
{
    [CustomPropertyDrawer(typeof(CompressionPreset))]
    public class CompressionPresetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            CompressionPreset currentEnum = (CompressionPreset)property.enumValueIndex;

            property.enumValueIndex = EditorGUI.Popup(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                label.text, property.enumValueIndex, property.enumDisplayNames);

            string infoText = GetInfoText(currentEnum);
        
            EditorGUI.LabelField(
                new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight * 2),
                infoText, EditorStyles.helpBox);
        
            EditorGUI.EndProperty();
        }
    
        private string GetInfoText(CompressionPreset type)
        {
            switch (type)
            {
                case CompressionPreset.AmbientMusic:
                    return "Music that is generally long and heavy that will be played for a long time.";
                case CompressionPreset.FrequentSound:
                    return "Sound that is generally short, not very heavy and will be played many times (shot, steps, UI...)";
                case CompressionPreset.OccasionalSound:
                    return "A sound that is generally short, not very heavy, and will not be played very frequently";
                default:
                    return "";
            }
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2;
        }
    }
}
#endif


