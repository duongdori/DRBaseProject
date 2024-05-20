using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty<T>(this IList<T> list) {
            return list == null || !list.Any();
        }
        
        public static bool IsNull<T>(this IList<T> list) {
            return list == null;
        }
        
        public static bool IsEmpty<T>(this IList<T> list) {
            return !list.Any();
        }
        
        public static bool IsIndexOutOfList<T>(this IList<T> list, int index)
        {
            return (index < 0) || (index >= list.Count);
        }
        
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");

            return list[Random.Range(0, list.Count)];
        }
        public static List<T> Clone<T>(this IList<T> listData)
        {
            List<T> clone = new List<T>();
            foreach (var item in listData)
            {
                clone.Add(item);
            }

            return clone; 
        }

        public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }

        public static List<T> RemoveList<T>(this List<T> listData, List<T> listRemove)
        {
            foreach (var item in listRemove)
            {
                if (listData.Contains(item))
                {
                    listData.Remove(item);
                }
            }
            return listData;
        }
        
        public static T RemoveRandom<T>(this IList<T> list)
        {
            if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
            
            int index = UnityEngine.Random.Range(0, list.Count);
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }
        
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}


