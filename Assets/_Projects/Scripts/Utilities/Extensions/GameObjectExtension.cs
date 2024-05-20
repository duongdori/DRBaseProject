using System;
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

        public static bool GetComponentInParentOrChildren<T>(this GameObject gameObject, ref T component)
            where T : class
        {
            if (component != null && (!(component is Object obj) || obj != null)) return false;

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

        public static void HideInHierarchy(this GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        public static T OrNull<T>(this T obj) where T : Object
        {
            return obj ? obj : null;
        }

        public static void ResetTransformation(this GameObject gameObject)
        {
            gameObject.transform.Reset();
        }

        #region Enable & Disable Children

        public static void EnableChildren(this GameObject gameObject)
        {
            gameObject.transform.EnableChildren();
        }

        public static void DisableChildren(this GameObject gameObject)
        {
            gameObject.transform.DisableChildren();
        }

        #endregion

        #region DestroyChildren

        public static void DestroyAllChild(this GameObject gameObject)
        {
            gameObject.transform.DestroyAllChildren();
        }

        public static void DestroyAllChildren(this GameObject gameObject, Type exceptChildType = null,
            string exceptChildName = null,
            StringExtension.StringMatchType matchType = StringExtension.StringMatchType.Exactly)
        {
            gameObject.transform.DestroyAllChildren(exceptChildType, exceptChildName, matchType);
        }

        public static void DestroyAllChildrenExceptIndex(this GameObject gameObject, int exceptChildIndex)
        {
            gameObject.transform.DestroyAllChildrenExceptIndex(exceptChildIndex);
        }

        public static void DestroyAllChildImmediate(this GameObject gameObject)
        {
            gameObject.transform.DestroyChildrenImmediate();
        }

        #endregion

        #region DespawnChildren

        public static void DespawnAllChild(this GameObject gameObject)
        {
            gameObject.transform.DespawnAllChildren();
        }

        public static void DespawnAllChildren(this GameObject gameObject, Type exceptChildType = null,
            string exceptChildName = null,
            StringExtension.StringMatchType matchType = StringExtension.StringMatchType.Exactly)
        {
            gameObject.transform.DespawnAllChildren(exceptChildType, exceptChildName, matchType);
        }

        public static void DespawnAllChildrenExceptIndex(this GameObject gameObject, int exceptChildIndex)
        {
            gameObject.transform.DespawnAllChildrenExceptIndex(exceptChildIndex);
        }

        #endregion

        #region SetPosition

        public static T SetPosition<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetPosition(x, y, z);
            return gameObject;
        }

        public static T SetPosition<T>(this T gameObject, Vector3 position) where T : MonoBehaviour
        {
            gameObject.transform.SetPosition(position);
            return gameObject;
        }

        public static T SetPositionX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.SetPositionX(x);
            return gameObject;
        }

        public static T SetPositionY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.SetPositionY(y);
            return gameObject;
        }

        public static T SetPositionZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetPositionZ(z);
            return gameObject;
        }

        public static void SetPosition(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.SetPosition(x, y, z);
        }

        public static void SetPosition(this GameObject gameObject, Vector3 position)
        {
            gameObject.transform.SetPosition(position);
        }

        public static void SetPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetPositionX(x);
        }

        public static void SetPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetPositionY(y);
        }

        public static void SetPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetPositionZ(z);
        }

        #endregion

        #region SetLocalPosition

        public static T SetLocalPosition<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalPosition(x, y, z);
            return gameObject;
        }

        public static T SetLocalPosition<T>(this T gameObject, Vector3 localPosition) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalPosition(localPosition);
            return gameObject;
        }

        public static T SetLocalPositionX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalPositionX(x);
            return gameObject;
        }

        public static T SetLocalPositionY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalPositionY(y);
            return gameObject;
        }

        public static T SetLocalPositionZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalPositionZ(z);
            return gameObject;
        }

        public static void SetLocalPosition(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.SetLocalPosition(x, y, z);
        }

        public static void SetLocalPosition(this GameObject gameObject, Vector3 localPosition)
        {
            gameObject.transform.SetLocalPosition(localPosition);
        }

        public static void SetLocalPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetLocalPositionX(x);
        }

        public static void SetLocalPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetLocalPositionY(y);
        }

        public static void SetLocalPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetLocalPositionZ(z);
        }

        #endregion

        #region SetLocalScale

        public static T SetLocalScale<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalScale(x, y, z);
            return gameObject;
        }

        public static T SetLocalScale<T>(this T gameObject, Vector3 localScale) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalScale(localScale);
            return gameObject;
        }

        public static T SetLocalScaleX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalScaleX(x);
            return gameObject;
        }

        public static T SetLocalScaleY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalScaleY(y);
            return gameObject;
        }

        public static T SetLocalScaleZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalScaleZ(z);
            return gameObject;
        }

        public static void SetLocalScale(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.SetLocalScale(x, y, z);
        }

        public static void SetLocalScale(this GameObject gameObject, Vector3 localScale)
        {
            gameObject.transform.SetLocalScale(localScale);
        }

        public static void SetLocalScaleX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetLocalScaleX(x);
        }

        public static void SetLocalScaleY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetLocalScaleY(y);
        }

        public static void SetLocalScaleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetLocalScaleZ(z);
        }

        #endregion

        #region SetLocalRotation

        public static T SetLocalRotation<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalRotation(x, y, z);
            return gameObject;
        }

        public static T SetLocalRotation<T>(this T gameObject, Vector3 rotation) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalRotation(rotation);
            return gameObject;
        }

        public static T SetLocalRotation<T>(this T gameObject, Quaternion rotation) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalRotation(rotation);
            return gameObject;
        }

        public static T SetLocalRotationX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalRotationX(x);
            return gameObject;
        }

        public static T SetLocalRotationY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalRotationY(y);
            return gameObject;
        }

        public static T SetLocalRotationZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalRotationZ(z);
            return gameObject;
        }

        public static void SetLocalRotation(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.SetLocalRotation(x, y, z);
        }

        public static void SetLocalRotation(this GameObject gameObject, Vector3 rotation)
        {
            gameObject.transform.SetLocalRotation(rotation);
        }

        public static void SetLocalRotation(this GameObject gameObject, Quaternion rotation)
        {
            gameObject.transform.SetLocalRotation(rotation);
        }

        public static void SetLocalRotationX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetLocalRotationX(x);
        }

        public static void SetLocalRotationY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetLocalRotationY(y);
        }

        public static void SetLocalRotationZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetLocalRotationZ(z);
        }

        #endregion

        #region SetEulerAngles

        public static T SetEulerAngle<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetEulerAngle(x, y, z);
            return gameObject;
        }

        public static T SetEulerAngle<T>(this T gameObject, Vector3 eulerAngle) where T : MonoBehaviour
        {
            gameObject.transform.SetEulerAngle(eulerAngle);
            return gameObject;
        }

        public static T SetEulerAngleX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.SetEulerAngleX(x);
            return gameObject;
        }

        public static T SetEulerAngleY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.SetEulerAngleY(y);
            return gameObject;
        }

        public static T SetEulerAngleZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetEulerAngleZ(z);
            return gameObject;
        }

        public static void SetEulerAngle(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.SetEulerAngle(x, y, z);
        }

        public static void SetEulerAngle(this GameObject gameObject, Vector3 eulerAngle)
        {
            gameObject.transform.SetEulerAngle(eulerAngle);
        }

        public static void SetEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetEulerAngleX(x);
        }

        public static void SetEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetEulerAngleY(y);
        }

        public static void SetEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetEulerAngleZ(z);
        }

        #endregion

        #region SetLocalEulerAngles

        public static T SetLocalEulerAngle<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalEulerAngle(x, y, z);
            return gameObject;
        }

        public static T SetLocalEulerAngle<T>(this T gameObject, Vector3 localEulerAngle) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalEulerAngle(localEulerAngle);
            return gameObject;
        }

        public static T SetLocalEulerAngleX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalEulerAngleX(x);
            return gameObject;
        }

        public static T SetLocalEulerAngleY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalEulerAngleY(y);
            return gameObject;
        }

        public static T SetLocalEulerAngleZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.SetLocalEulerAngleZ(z);
            return gameObject;
        }

        public static void SetLocalEulerAngle(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.SetLocalEulerAngle(x, y, z);
        }

        public static void SetLocalEulerAngle(this GameObject gameObject, Vector3 localEulerAngle)
        {
            gameObject.transform.SetLocalEulerAngle(localEulerAngle);
        }

        public static void SetLocalEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetLocalEulerAngleX(x);
        }

        public static void SetLocalEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetLocalEulerAngleY(y);
        }

        public static void SetLocalEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetLocalEulerAngleZ(z);
        }

        #endregion

        #region AddPosition

        public static T AddPosition<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddPosition(x, y, z);
            return gameObject;
        }

        public static T AddPosition<T>(this T gameObject, Vector3 position) where T : MonoBehaviour
        {
            gameObject.transform.AddPosition(position);
            return gameObject;
        }

        public static T AddPositionX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.AddPositionX(x);
            return gameObject;
        }

        public static T AddPositionY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.AddPositionY(y);
            return gameObject;
        }

        public static T AddPositionZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddPositionZ(z);
            return gameObject;
        }

        public static void AddPosition(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.AddPosition(x, y, z);
        }

        public static void AddPosition(this GameObject gameObject, Vector3 position)
        {
            gameObject.transform.AddPosition(position);
        }

        public static void AddPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddPositionX(x);
        }

        public static void AddPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddPositionY(y);
        }

        public static void AddPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddPositionZ(z);
        }

        #endregion

        #region AddLocalPosition

        public static T AddLocalPosition<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalPosition(x, y, z);
            return gameObject;
        }

        public static T AddLocalPosition<T>(this T gameObject, Vector3 localPosition) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalPosition(localPosition);
            return gameObject;
        }

        public static T AddLocalPositionX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalPositionX(x);
            return gameObject;
        }

        public static T AddLocalPositionY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalPositionY(y);
            return gameObject;
        }

        public static T AddLocalPositionZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalPositionZ(z);
            return gameObject;
        }

        public static void AddLocalPosition(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.AddLocalPosition(x, y, z);
        }

        public static void AddLocalPosition(this GameObject gameObject, Vector3 localPosition)
        {
            gameObject.transform.AddLocalPosition(localPosition);
        }

        public static void AddLocalPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddLocalPositionX(x);
        }

        public static void AddLocalPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddLocalPositionY(y);
        }

        public static void AddLocalPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddLocalPositionZ(z);
        }

        #endregion

        #region AddLocalScale

        public static T AddLocalScale<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalScale(x, y, z);
            return gameObject;
        }

        public static T AddLocalScale<T>(this T gameObject, Vector3 localScale) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalScale(localScale);
            return gameObject;
        }

        public static T AddLocalScaleX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalScaleX(x);
            return gameObject;
        }

        public static T AddLocalScaleY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalScaleY(y);
            return gameObject;
        }

        public static T AddLocalScaleZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalScaleZ(z);
            return gameObject;
        }

        public static void AddLocalScale(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.AddLocalScale(x, y, z);
        }

        public static void AddLocalScale(this GameObject gameObject, Vector3 localScale)
        {
            gameObject.transform.AddLocalScale(localScale);
        }

        public static void AddLocalScaleX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddLocalScaleX(x);
        }

        public static void AddLocalScaleY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddLocalScaleY(y);
        }

        public static void AddLocalScaleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddLocalScaleZ(z);
        }

        #endregion

        #region AddLocalRotation

        public static T AddLocalRotation<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalRotation(x, y, z);
            return gameObject;
        }

        public static T AddLocalRotation<T>(this T gameObject, Vector3 localRotation) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalRotation(localRotation);
            return gameObject;
        }

        public static T AddLocalRotationX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalRotationX(x);
            return gameObject;
        }

        public static T AddLocalRotationY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalRotationY(y);
            return gameObject;
        }

        public static T AddLocalRotationZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalRotationZ(z);
            return gameObject;
        }

        public static void AddLocalRotation(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.AddLocalRotation(x, y, z);
        }

        public static void AddLocalRotation(this GameObject gameObject, Vector3 localRotation)
        {
            gameObject.transform.AddLocalRotation(localRotation);
        }

        public static void AddLocalRotationX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddLocalRotationX(x);
        }

        public static void AddLocalRotationY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddLocalRotationY(y);
        }

        public static void AddLocalRotationZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddLocalRotationZ(z);
        }

        #endregion

        #region AddEulerAngles

        public static T AddEulerAngle<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddEulerAngle(x, y, z);
            return gameObject;
        }

        public static T AddEulerAngle<T>(this T gameObject, Vector3 eulerAngle) where T : MonoBehaviour
        {
            gameObject.transform.AddEulerAngle(eulerAngle);
            return gameObject;
        }

        public static T AddEulerAngleX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.AddEulerAngleX(x);
            return gameObject;
        }

        public static T AddEulerAngleY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.AddEulerAngleY(y);
            return gameObject;
        }

        public static T AddEulerAngleZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddEulerAngleZ(z);
            return gameObject;
        }

        public static void AddEulerAngle(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.AddEulerAngle(x, y, z);
        }

        public static void AddEulerAngle(this GameObject gameObject, Vector3 eulerAngle)
        {
            gameObject.transform.AddEulerAngle(eulerAngle);
        }

        public static void AddEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddEulerAngleX(x);
        }

        public static void AddEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddEulerAngleY(y);
        }

        public static void AddEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddEulerAngleZ(z);
        }

        #endregion

        #region AddLocalEulerAngles

        public static T AddLocalEulerAngle<T>(this T gameObject, float x, float y, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalEulerAngle(x, y, z);
            return gameObject;
        }

        public static T AddLocalEulerAngle<T>(this T gameObject, Vector3 localEulerAngle) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalEulerAngle(localEulerAngle);
            return gameObject;
        }

        public static T AddLocalEulerAngleX<T>(this T gameObject, float x) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalEulerAngleX(x);
            return gameObject;
        }

        public static T AddLocalEulerAngleY<T>(this T gameObject, float y) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalEulerAngleY(y);
            return gameObject;
        }

        public static T AddLocalEulerAngleZ<T>(this T gameObject, float z) where T : MonoBehaviour
        {
            gameObject.transform.AddLocalEulerAngleZ(z);
            return gameObject;
        }

        public static void AddLocalEulerAngle(this GameObject gameObject, float x, float y, float z)
        {
            gameObject.transform.AddLocalEulerAngle(x, y, z);
        }

        public static void AddLocalEulerAngle(this GameObject gameObject, Vector3 localEulerAngle)
        {
            gameObject.transform.AddLocalEulerAngle(localEulerAngle);
        }

        public static void AddLocalEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddLocalEulerAngleX(x);
        }

        public static void AddLocalEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddLocalEulerAngleY(y);
        }

        public static void AddLocalEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddLocalEulerAngleZ(z);
        }

        #endregion
    }
}