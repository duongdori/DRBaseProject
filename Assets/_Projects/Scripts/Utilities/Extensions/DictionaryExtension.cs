using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DRG.Utilities.Extensions
{
    public static class DictionaryExtension
    {
        public static bool TryGetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value, out TKey key)
        {
            key = default;
            foreach (var pair in dictionary.Where(pair => pair.Value.Equals(value)))
            {
                key = pair.Key;
                return true;
            }
            return false;
        }

        public static List<TKey> GetKeysByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            List<TKey> keys = new List<TKey>();
        
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (!pair.Value.Equals(value)) continue;
            
                keys.Add(pair.Key);
            }
            return keys;
        }

        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return dictionary == null || dictionary.Count == 0;
        }
    
        public static bool IsNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) => dictionary == null;
    
        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) => dictionary.Count == 0;
    
    }

}
