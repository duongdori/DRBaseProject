using UnityEngine;
using Object = UnityEngine.Object;

namespace DR.Utilities.Extensions
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent(out T component) ? component : gameObject.AddComponent<T>();
        }
        
        public static bool GetComponentInParentOrChildren<T>(this GameObject gameObject, ref T component) where T : class
        {
            if (component != null &&
                (!(component is Object obj) || obj != null))
                return false;

            component = gameObject.GetComponentInParentOrChildren<T>();
            return !(component is null);
        }
        
        public static T GetComponentInParentOrChildren<T>(this GameObject gameObject) where T : class
        {
            var component = gameObject.GetComponentInParent<T>();
            if (component != null)
                return component;

            return gameObject.GetComponentInChildren<T>();
        }
        
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            gameObject.transform.ForEveryChild(child => child.gameObject.SetLayerRecursively(layer));
        }
        
        public static T SetParent<T>(this T gameObject, Transform parent) where T : MonoBehaviour
        {
            Vector3 localPosition = gameObject.transform.localPosition;
            Vector3 localScale = gameObject.transform.localScale;
            Quaternion localRotate = gameObject.transform.localRotation;
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.localScale = localScale;
            gameObject.transform.localRotation = localRotate;
            return gameObject;
        }
        
        public static void HideInHierarchy(this GameObject gameObject) {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        
        public static T OrNull<T>(this T obj) where T : Object
        {
            return obj ? obj : null;
        }
        
        public static void EnableChildren(this GameObject gameObject) {
            gameObject.transform.EnableChildren();
        }
        
        public static void DisableChildren(this GameObject gameObject) {
            gameObject.transform.DisableChildren();
        }
        
        public static void ResetTransformation(this GameObject gameObject) {
            gameObject.transform.Reset();
        }

        #region DestroyChildren

        public static void DestroyAllChild(this GameObject gameObject)
        {
            gameObject.transform.DestroyAllChildren();
        }
        
        public static void DestroyAllChildImmediate(this GameObject gameObject)
        {
            gameObject.transform.DestroyChildrenImmediate();
        }
        
        public static void DespawnAllChild(this GameObject gameObject)
        {
            gameObject.transform.DespawnAllChildren();
        }

        #endregion

        #region SetPosition
        
        public static T SetPosition<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.position = new Vector3(x, y, z);
            return gameObject;
        }
        
        public static T SetPosition<T>(this T gameObject, Vector3 position) where T : MonoBehaviour
        {
            gameObject.transform.position = position;
            return gameObject;
        }

        public static T SetPositionX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
            return gameObject;
        }

        public static T SetPositionY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
            return gameObject;
        }

        public static T SetPositionZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z);
            return gameObject;
            
        }

        #endregion
        
        #region SetLocalPosition

        public static T SetLocalPosition<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.localPosition = new Vector3(x, y, z);
            return gameObject;
        }
        
        public static T SetLocalPosition<T>(this T gameObject, Vector3 localPosition) where T : MonoBehaviour
        {
            gameObject.transform.localPosition = localPosition;
            return gameObject;
        }

        public static T SetLocalPositionX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.localPosition = new Vector3(x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            return gameObject;
        }

        public static T SetLocalPositionY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y, gameObject.transform.localPosition.z);
            return gameObject;
        }

        public static T SetLocalPositionZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, z);
            return gameObject;
        }

        #endregion
        
        #region SetLocalRotation

        public static T SetLocalRotation<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.localRotation = Quaternion.Euler(x, y, z);
            return gameObject;
        }

        public static T SetLocalRotation<T>(this T gameObject, Vector3 rotation) where T : MonoBehaviour
        {
            gameObject.transform.localRotation = Quaternion.Euler(rotation);
            return gameObject;
        }

        public static T SetLocalRotation<T>(this T gameObject, Quaternion rotation) where T : MonoBehaviour
        {
            gameObject.transform.localRotation = rotation;
            return gameObject;
        }

        public static T SetLocalRotationX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.localRotation = Quaternion.Euler(x, gameObject.transform.localRotation.eulerAngles.y, 
                gameObject.transform.localRotation.eulerAngles.z);
            
            return gameObject;
        }
        
        public static T SetLocalRotationY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.localRotation = Quaternion.Euler(gameObject.transform.localRotation.eulerAngles.x, y, 
                gameObject.transform.localRotation.eulerAngles.z);
            
            return gameObject;
        }
        
        public static T SetLocalRotationZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.localRotation = Quaternion.Euler(gameObject.transform.localRotation.eulerAngles.x, 
                gameObject.transform.localRotation.eulerAngles.y, z);
            
            return gameObject;
        }

        #endregion
        
    }
}

