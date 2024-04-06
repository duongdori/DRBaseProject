#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DR.Framework.FSM
{
    public static class StateMachineUtilities
    {
        public static T DoGenericField<T>(Rect area, string label, T value)
        {
            if (typeof(Object).IsAssignableFrom(typeof(T)))
            {
                return (T)(object)EditorGUI.ObjectField(
                    area, label, value as Object, typeof(T), true);
            }

            var stateName = value != null ? value.ToString() : "Null";
            EditorGUI.LabelField(area, label, stateName);
            return value;
        }
        
        public static void NextVerticalArea(ref Rect area)
        {
            if (area.height > 0)
                area.y += area.height + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
#endif