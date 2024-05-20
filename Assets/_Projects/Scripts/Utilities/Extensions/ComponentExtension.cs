using System.Linq;
using UnityEngine;

namespace DRG.Utilities.Extensions
{
    public static class ComponentExtension
    {
        public static T CopyComponent<T>(this T original, GameObject destination) where T : Component
        {
            if (original == null || destination == null) return null;

            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;

            var fields = type.GetFields().Where(field => !field.IsStatic);
            foreach (var field in fields)
            {
                field.SetValue(dst, field.GetValue(original));
            }

            var props = type.GetProperties().Where(prop => prop.CanWrite && prop.CanRead && prop.Name != "name");
            foreach (var prop in props)
            {
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }

            return dst;
        }
    }
}