using System.Collections.Generic;
using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class TransformExtension
    {
        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            return transform.gameObject.TryGetComponent(out T component) ? component : transform.gameObject.AddComponent<T>();
        }
        
        public static IEnumerable<Transform> Children(this Transform parent) {
            foreach (Transform child in parent) {
                yield return child;
            }
        }
        
        public static void ForEveryChild(this Transform parent, System.Action<Transform> action) {
            for (var i = parent.childCount - 1; i >= 0; i--) {
                action(parent.GetChild(i));
            }
        }
        
        public static void Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        
        public static void DisableChildren(this Transform parent) {
            parent.ForEveryChild(child => child.gameObject.SetActive(false));
        }
        
        public static void EnableChildren(this Transform parent) {
            parent.ForEveryChild(child => child.gameObject.SetActive(true));
        }
        
        public static void SetLayerRecursively(this Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            transform.ForEveryChild(child => child.SetLayerRecursively(layer));
        }
        
        public static void SetParent(this Transform transform, Transform parent)
        {
            Vector3 localPosition = transform.localPosition;
            Vector3 localScale = transform.localScale;
            Quaternion localRotate = transform.localRotation;
            transform.SetParent(parent);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            transform.localRotation = localRotate;
        }
        
        // SmoothLookAt() use in Update() method
        public static void SmoothLookAt(this Transform tf, Transform target, float damping)     
        {
            var targetRotation = Quaternion.LookRotation(target.position - tf.position);
            tf.rotation = Quaternion.Slerp(tf.rotation, targetRotation, damping * Time.deltaTime);
        }

        #region DestroyChildren

        public static void DestroyAllChild(this Transform transform)
        {
            transform.ForEveryChild(child => Object.Destroy(child.gameObject));
        }
        
        public static void DestroyChildrenImmediate(this Transform parent) {
            parent.ForEveryChild(child => Object.DestroyImmediate(child.gameObject));
        }
        
        public static void DespawnAllChild(this Transform transform)
        {
            List<Transform> childToDespawn = new List<Transform>();

            foreach (Transform child in transform)
            {
                if (child.GetComponent<PoolElement>() != null)
                {
                    childToDespawn.Add(child);
                }
                else
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            foreach (Transform child in childToDespawn)
            {
                PoolManager.Instance.DespawnObject(child);
            }
        }

        #endregion

        #region SetPosition
        
        public static void SetPosition(this Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }
        
        public static void SetPosition(this Transform transform, Vector3 position)
        {
            transform.position = position;
        }

        public static void SetPositionX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        #endregion

        #region SetLocalPosition

        public static void SetLocalPosition(this Transform transform, float x, float y, float z)
        {
            transform.localPosition = new Vector3(x, y, z);
        }
        
        public static void SetLocalPosition(this Transform transform, Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }
        public static void SetLocalPositionX(this Transform transform, float x)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        public static void SetLocalPositionY(this Transform transform, float y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        public static void SetLocalPositionZ(this Transform transform, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        #endregion

        #region SetLocalScale

        public static void SetLocalScale(this Transform transform, float x, float y, float z)
        {
            transform.localScale = new Vector3(x, y, z);
        }
        
        public static void SetLocalScale(this Transform transform, Vector3 localScale)
        {
            transform.localPosition = localScale;
        }
        public static void SetLocalScaleX(this Transform transform, float x)
        {
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        public static void SetLocalScaleY(this Transform transform, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        }

        public static void SetLocalScaleZ(this Transform transform, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        }

        #endregion

        #region SetLocalRotation

        public static void SetLocalRotation(this Transform transform, float x, float y, float z)
        {
            transform.localRotation = Quaternion.Euler(x, y, z);
        }

        public static void SetLocalRotation(this Transform transform, Vector3 rotation)
        {
            transform.localRotation = Quaternion.Euler(rotation);
        }

        public static void SetLocalRotation(this Transform transform, Quaternion rotation)
        {
            transform.localRotation = rotation;
        }

        public static void SetLocalRotationX(this Transform transform, float x)
        {
            transform.localRotation =
                Quaternion.Euler(x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
        
        public static void SetLocalRotationY(this Transform transform, float y)
        {
            transform.localRotation =
                Quaternion.Euler(transform.localRotation.eulerAngles.x, y, transform.localRotation.eulerAngles.z);
        }
        
        public static void SetLocalRotationZ(this Transform transform, float z)
        {
            transform.localRotation =
                Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, z);
        }

        #endregion
        
    }
}
