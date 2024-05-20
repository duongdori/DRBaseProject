using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DR.Utilities.Extensions
{
    public static class TransformExtension
    {
        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            return transform.gameObject.TryGetComponent(out T component)
                ? component
                : transform.gameObject.AddComponent<T>();
        }

        public static IEnumerable<Transform> Children(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                yield return child;
            }
        }

        public static int GetChildCount(this Transform trans, bool includeInactive)
        {
            if (includeInactive)
            {
                return trans.childCount;
            }

            int count = 0;
            for (int i = 0; i < trans.childCount; ++i)
            {
                if (trans.GetChild(i).gameObject.activeSelf)
                {
                    ++count;
                }
            }

            return count;
        }

        public static void ForEveryChild(this Transform parent, System.Action<Transform> action)
        {
            for (var i = parent.childCount - 1; i >= 0; i--)
            {
                action(parent.GetChild(i));
            }
        }

        public static void Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
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

        #region Enable & Disable Children

        public static void DisableChildren(this Transform parent)
        {
            parent.ForEveryChild(child => child.gameObject.SetActive(false));
        }

        public static void EnableChildren(this Transform parent)
        {
            parent.ForEveryChild(child => child.gameObject.SetActive(true));
        }

        #endregion

        #region Find Child By Name

        public static Transform FindDeepChildBFS(this Transform parent, string childName,
            StringExtension.StringMatchType matchType = StringExtension.StringMatchType.Exactly)
        {
            if (childName == null)
                return null;

            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(parent);
            while (queue.Count > 0)
            {
                var child = queue.Dequeue();
                if (child.name.IsMatchWith(childName, matchType) && child != parent)
                    return child;

                foreach (Transform t in child)
                {
                    queue.Enqueue(t);
                }
            }

            return null;
        }

        public static Transform FindDeepChildDFS(this Transform parent, string childName,
            StringExtension.StringMatchType matchType = StringExtension.StringMatchType.Exactly)
        {
            if (childName == null)
                return null;

            foreach (Transform child in parent)
            {
                if (child.name.IsMatchWith(childName, matchType))
                    return child;

                var result = child.FindDeepChildDFS(childName);
                if (result != null)
                    return result;
            }

            return null;
        }

        #endregion

        #region DestroyChildren

        public static void DestroyAllChildren(this Transform parent)
        {
            if (Application.isPlaying)
            {
                parent.ForEveryChild(child => Object.Destroy(child.gameObject));
            }
            else
            {
                parent.ForEveryChild(child => Object.DestroyImmediate(child.gameObject));
            }
        }

        public static void DestroyAllChildren(this Transform parent, Type exceptChildType = null, string exceptChildName = null,
            StringExtension.StringMatchType matchType = StringExtension.StringMatchType.Exactly)
        {
            if (Application.isPlaying)
            {
                parent.ForEveryChild(child =>
                {
                    if (exceptChildName != null && child.name.IsMatchWith(exceptChildName, matchType))
                    {
                        return;
                    }

                    if (exceptChildType != null && child.GetComponent(exceptChildType) != null)
                    {
                        return;
                    }

                    Object.Destroy(child.gameObject);
                });
            }
            else
            {
                parent.ForEveryChild(child =>
                {
                    if (exceptChildName != null && child.name.IsMatchWith(exceptChildName, matchType))
                    {
                        return;
                    }

                    if (exceptChildType != null && child.GetComponent(exceptChildType) != null)
                    {
                        return;
                    }

                    Object.DestroyImmediate(child.gameObject);
                });
            }
        }

        public static void DestroyAllChildrenExceptIndex(this Transform parent, int exceptChildIndex)
        {
            if (Application.isPlaying)
            {
                parent.ForEveryChild(child =>
                {
                    if (child.GetSiblingIndex() != exceptChildIndex)
                    {
                        Object.Destroy(child.gameObject);
                    }
                });
            }
            else
            {
                parent.ForEveryChild(child =>
                {
                    if (child.GetSiblingIndex() != exceptChildIndex)
                    {
                        Object.DestroyImmediate(child.gameObject);
                    }
                });
            }
        }

        public static void DestroyChildrenImmediate(this Transform parent)
        {
            parent.ForEveryChild(child => Object.DestroyImmediate(child.gameObject));
        }

        #endregion

        #region DespawnChildren

        public static void DespawnAllChildren(this Transform parent)
        {
            List<Transform> childToDespawn = new List<Transform>();

            foreach (Transform child in parent)
            {
                if (child.GetComponent<PoolElement>() != null)
                {
                    childToDespawn.Add(child);
                }
                else
                {
                    Object.Destroy(child.gameObject);
                }
            }

            foreach (Transform child in childToDespawn)
            {
                PoolManager.Instance.DespawnObject(child);
            }
        }

        public static void DespawnAllChildren(this Transform parent, Type exceptChildType = null, string exceptChildName = null,
            StringExtension.StringMatchType matchType = StringExtension.StringMatchType.Exactly)
        {
            List<Transform> childToDespawn = new List<Transform>();

            foreach (Transform child in parent)
            {
                if (exceptChildName != null && child.name.IsMatchWith(exceptChildName, matchType))
                {
                    continue;
                }

                if (exceptChildType != null && child.GetComponent(exceptChildType) != null)
                {
                    continue;
                }

                if (child.GetComponent<PoolElement>() != null)
                {
                    childToDespawn.Add(child);
                }
                else
                {
                    Object.Destroy(child.gameObject);
                }
            }

            foreach (Transform child in childToDespawn)
            {
                PoolManager.Instance.DespawnObject(child);
            }
        }

        public static void DespawnAllChildrenExceptIndex(this Transform parent, int exceptChildIndex)
        {
            List<Transform> childToDespawn = new List<Transform>();

            foreach (Transform child in parent)
            {
                if (child.GetSiblingIndex() == exceptChildIndex) continue;

                if (child.GetComponent<PoolElement>() != null)
                {
                    childToDespawn.Add(child);
                }
                else
                {
                    Object.Destroy(child.gameObject);
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
            transform.localScale = localScale;
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

        #region SetEulerAngles
        
        public static void SetEulerAngle(this Transform transform, float x, float y, float z)
        {
            transform.eulerAngles = new Vector3(x, y, z);
        }
        
        public static void SetEulerAngle(this Transform transform, Vector3 rotation)
        {
            transform.eulerAngles = rotation;
        }
        
        public static void SetEulerAngleX(this Transform transform, float x)
        {
            transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        public static void SetEulerAngleY(this Transform transform, float y)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
        }

        public static void SetEulerAngleZ(this Transform transform, float z)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);
        }

        #endregion
        
        #region SetLocalEulerAngles
        
        public static void SetLocalEulerAngle(this Transform transform, float x, float y, float z)
        {
            transform.localEulerAngles = new Vector3(x, y, z);
        }
        
        public static void SetLocalEulerAngle(this Transform transform, Vector3 rotation)
        {
            transform.localEulerAngles = rotation;
        }
        
        public static void SetLocalEulerAngleX(this Transform transform, float x)
        {
            transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        
        public static void SetLocalEulerAngleY(this Transform transform, float y)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
        }
        
        public static void SetLocalEulerAngleZ(this Transform transform, float z)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
        }
        
        #endregion
        
        #region AddPosition
        
        public static void AddPosition(this Transform transform, float x, float y, float z)
        {
            transform.SetPosition(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        }
        
        public static void AddPosition(this Transform transform, Vector3 position)
        {
            transform.SetPosition(transform.position + position);
        }
        
        public static void AddPositionX(this Transform transform, float value)
        {
            transform.SetPositionX(transform.position.x + value);
        }
        
        public static void AddPositionY(this Transform transform, float value)
        {
            transform.SetPositionY(transform.position.y + value);
        }
        
        public static void AddPositionZ(this Transform transform, float value)
        {
            transform.SetPositionZ(transform.position.z + value);
        }
        
        #endregion
        
        #region AddLocalPosition
        
        public static void AddLocalPosition(this Transform transform, float x, float y, float z)
        {
            transform.SetLocalPosition(transform.localPosition.x + x, transform.localPosition.y + y, transform.localPosition.z + z);
        }
        
        public static void AddLocalPosition(this Transform transform, Vector3 position)
        {
            transform.SetLocalPosition(transform.localPosition + position);
        }
        
        public static void AddLocalPositionX(this Transform transform, float value)
        {
            transform.SetLocalPositionX(transform.localPosition.x + value);
        }
        
        public static void AddLocalPositionY(this Transform transform, float value)
        {
            transform.SetLocalPositionY(transform.localPosition.y + value);
        }
        
        public static void AddLocalPositionZ(this Transform transform, float value)
        {
            transform.SetLocalPositionZ(transform.localPosition.z + value);
        }
        
        #endregion
        
        #region AddLocalScale
        
        public static void AddLocalScale(this Transform transform, float x, float y, float z)
        {
            transform.SetLocalScale(transform.localScale.x + x, transform.localScale.y + y, transform.localScale.z + z);
        }
        
        public static void AddLocalScale(this Transform transform, Vector3 scale)
        {
            transform.SetLocalScale(transform.localScale + scale);
        }
        
        public static void AddLocalScaleX(this Transform transform, float value)
        {
            transform.SetLocalScaleX(transform.localScale.x + value);
        }

        public static void AddLocalScaleY(this Transform transform, float value)
        {
            transform.SetLocalScaleY(transform.localScale.y + value);
        }

        public static void AddLocalScaleZ(this Transform transform, float value)
        {
            transform.SetLocalScaleZ(transform.localScale.z + value);
        }
        #endregion
        
        #region AddLocalRotation
        
        public static void AddLocalRotation(this Transform transform, float x, float y, float z)
        {
            transform.SetLocalRotation(transform.localEulerAngles.x + x, transform.localEulerAngles.y + y, transform.localEulerAngles.z + z);
        }
        
        public static void AddLocalRotation(this Transform transform, Vector3 rotation)
        {
            transform.SetLocalRotation(transform.localEulerAngles + rotation);
        }
        
        public static void AddLocalRotationX(this Transform transform, float value)
        {
            transform.SetLocalRotationX(transform.localEulerAngles.x + value);
        }
        
        public static void AddLocalRotationY(this Transform transform, float value)
        {
            transform.SetLocalRotationY(transform.localEulerAngles.y + value);
        }
        
        public static void AddLocalRotationZ(this Transform transform, float value)
        {
            transform.SetLocalRotationZ(transform.localEulerAngles.z + value);
        }
        
        #endregion

        #region AddEulerAngles
        
        public static void AddEulerAngle(this Transform transform, float x, float y, float z)
        {
            transform.SetEulerAngle(transform.eulerAngles.x + x, transform.eulerAngles.y + y, transform.eulerAngles.z + z);
        }
        
        public static void AddEulerAngle(this Transform transform, Vector3 rotation)
        {
            transform.SetEulerAngle(transform.eulerAngles + rotation);
        }
        
        public static void AddEulerAngleX(this Transform transform, float value)
        {
            transform.SetEulerAngleX(transform.eulerAngles.x + value);
        }
        
        public static void AddEulerAngleY(this Transform transform, float value)
        {
            transform.SetEulerAngleY(transform.eulerAngles.y + value);
        }
        
        public static void AddEulerAngleZ(this Transform transform, float value)
        {
            transform.SetEulerAngleZ(transform.eulerAngles.z + value);
        }

        #endregion
        
        #region AddLocalEulerAngles
        
        public static void AddLocalEulerAngle(this Transform transform, float x, float y, float z)
        {
            transform.SetLocalEulerAngle(transform.localEulerAngles.x + x, transform.localEulerAngles.y + y, transform.localEulerAngles.z + z);
        }
        
        public static void AddLocalEulerAngle(this Transform transform, Vector3 rotation)
        {
            transform.SetLocalEulerAngle(transform.localEulerAngles + rotation);
        }
        
        public static void AddLocalEulerAngleX(this Transform transform, float value)
        {
            transform.SetLocalEulerAngleX(transform.localEulerAngles.x + value);
        }
        
        public static void AddLocalEulerAngleY(this Transform transform, float value)
        {
            transform.SetLocalEulerAngleY(transform.localEulerAngles.y + value);
        }
        
        public static void AddLocalEulerAngleZ(this Transform transform, float value)
        {
            transform.SetLocalEulerAngleZ(transform.localEulerAngles.z + value);
        }
        
        #endregion
    }
}