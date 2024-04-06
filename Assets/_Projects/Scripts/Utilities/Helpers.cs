using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace DR.Utilities
{
    public static class Helpers
    {
        private static Camera _mainCamera;
        public static Camera MainCamera => _mainCamera ? _mainCamera : _mainCamera = Camera.main;
        
        private static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDict = new();

        private const int SortingOrderDefault = 5000;
        
        // Get Sorting order to set SpriteRenderer sortingOrder, higher position = lower sortingOrder
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = SortingOrderDefault) {
            return (int)(baseSortingOrder - position.y) + offset;
        }
        
        /// <summary>
        /// Returns a WaitForSeconds object for the specified duration. </summary>
        /// <param name="seconds">The duration in seconds to wait.</param>
        /// <returns>A WaitForSeconds object.</returns>
        public static WaitForSeconds GetWaitForSeconds(float seconds) {
            if (seconds < 1f / Application.targetFrameRate) return null;

            if (WaitForSecondsDict.TryGetValue(seconds, out var forSeconds)) return forSeconds;

            var waitForSeconds = new WaitForSeconds(seconds);
            WaitForSecondsDict[seconds] = waitForSeconds;
            return waitForSeconds;
        }
        
        /// <summary>
        /// Checks if the mouse pointer is over a UI element.
        /// </summary>
        /// <returns>True if the mouse pointer is over a UI element, false otherwise.</returns>
        public static bool IsPointerOverUI()
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            
            return results.Count > 0;
        }
        
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        
        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        
        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        
        public static Vector3 GetDirToMouse(Vector3 fromPosition) {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            return (mouseWorldPosition - fromPosition).normalized;
        }

        /// <summary>
        /// Gets the world position of a canvas element.
        /// </summary>
        /// <param name="element">The RectTransform of the canvas element.</param>
        /// <returns>The world position of the canvas element as a Vector2.</returns>
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, MainCamera,
                out var worldPosition);
            return worldPosition;
        }
        
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        
        public static string ToPercent(float num, int digit = 0)
        {
            return num.ToString("P" + digit).Replace(" ", string.Empty);
        }
        
        public static string GetPercentString(float num, bool includeSign = true) {
            return Mathf.RoundToInt(num * 100f) + (includeSign ? "%" : "");
        }
        
        public static string IntToBinaryString(int input, int stringLength)
        {
            return Convert.ToString(input, 2).PadLeft(stringLength, '0');
        }

        public static bool GetRating(float target)
        {
            return UnityEngine.Random.value <= target;
        }
        
        public static bool TestChance(int chance, int chanceMax = 100) {
            return UnityEngine.Random.Range(0, chanceMax) < chance;
        }
        
        // Interpolates between two vectors without clamping the interpolate between [0, 1].
        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(Mathf.LerpUnclamped(a.x, b.x, t), Mathf.LerpUnclamped(a.y, b.y, t), Mathf.LerpUnclamped(a.z, b.z, t));
        }
        
        // Generates a random point within the unit sphere.
        public static Vector3 RandomInUnitSphere()
        {
            return Random.insideUnitSphere;
        }
        
        public static Vector3 GetRandomPositionWithinRectangle(float xMin, float xMax, float yMin, float yMax) {
            return new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax));
        }

        public static Vector3 GetRandomPositionWithinRectangle(Vector3 lowerLeft, Vector3 upperRight) {
            return new Vector3(UnityEngine.Random.Range(lowerLeft.x, upperRight.x), UnityEngine.Random.Range(lowerLeft.y, upperRight.y));
        }
        
        public static Color GetRandomColor() {
            return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        }
        
        public static List<T> RemoveDuplicates<T>(List<T> arr) {
            List<T> list = new List<T>();
            foreach (T t in arr) {
                if (!list.Contains(t)) {
                    list.Add(t);
                }
            }
            return list;
        }
        
        public static T[] RemoveDuplicates<T>(T[] arr) {
            List<T> list = new List<T>();
            foreach (T t in arr) {
                if (!list.Contains(t)) {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }
        
        [System.Serializable]
        private class JsonDictionary {
            public List<string> keyList = new List<string>();
            public List<string> valueList = new List<string>();
        }

        // Take a Dictionary and return JSON string
        public static string SaveDictionaryJson<TKey, TValue>(Dictionary<TKey, TValue> dictionary) {
            JsonDictionary jsonDictionary = new JsonDictionary();
            foreach (TKey key in dictionary.Keys) {
                jsonDictionary.keyList.Add(JsonUtility.ToJson(key));
                jsonDictionary.valueList.Add(JsonUtility.ToJson(dictionary[key]));
            }
            string saveJson = JsonUtility.ToJson(jsonDictionary);
            return saveJson;
        }

        // Take a JSON string and return Dictionary<T1, T2>
        public static Dictionary<TKey, TValue> LoadDictionaryJson<TKey, TValue>(string saveJson) {
            JsonDictionary jsonDictionary = JsonUtility.FromJson<JsonDictionary>(saveJson);
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>();
            for (int i = 0; i < jsonDictionary.keyList.Count; i++) {
                TKey key = JsonUtility.FromJson<TKey>(jsonDictionary.keyList[i]);
                TValue value = JsonUtility.FromJson<TValue>(jsonDictionary.valueList[i]);
                ret[key] = value;
            }
            return ret;
        }
    }
}