using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class RectTransformExtension
    {
        public static void SetSizeByWidth(this RectTransform rectTransform, float width, float aspect)
        {
            rectTransform.sizeDelta = new Vector2(width, width * aspect);
        }

        public static void SetSizeByHeight(this RectTransform rectTransform, float height, float aspect)
        {
            rectTransform.sizeDelta = new Vector2(height / aspect, height);
        }

        public static void SetLeft(this RectTransform rectTransform, float left)
        {
            rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
        }

        public static void SetRight(this RectTransform rectTransform, float right)
        {
            rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        }

        public static void SetTop(this RectTransform rectTransform, float top)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rectTransform, float bottom)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        }

        public static void StretchFullParent(this RectTransform rectTransform)
        {
            rectTransform.transform.localPosition = Vector3.zero;
            rectTransform.transform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            Vector2 size = rectTransform.sizeDelta;
            Vector2 deltaPivot = rectTransform.pivot - pivot;
            Vector2 deltaPosition = new Vector2(size.x * deltaPivot.x, size.y * deltaPivot.y);
            rectTransform.pivot = pivot;
            rectTransform.anchoredPosition -= deltaPosition;
        }
        
        public static void SetPivot(this RectTransform rectTransform, float x, float y)
        {
            SetPivot(rectTransform, new Vector2(x, y));
        }
        
    }
}