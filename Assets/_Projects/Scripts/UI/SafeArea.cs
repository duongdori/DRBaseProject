using UnityEngine;

public class SafeArea : MonoSingleton<SafeArea>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;

    private Rect _safeArea = Rect.zero;

    private void Start()
    {
        HandleSafeArea();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(_safeArea != Screen.safeArea)
        {
            HandleSafeArea();
        }
    }
#endif

    private void HandleSafeArea()
    {
        _safeArea = Screen.safeArea;
        Vector2 anchorMin = _safeArea.position;
        Vector2 anchorMax = _safeArea.position + _safeArea.size;

        var pixelRect = canvas.pixelRect;
        anchorMin.x /= pixelRect.width;
        anchorMin.y /= pixelRect.height;

        anchorMax.x /= pixelRect.width;
        anchorMax.y /= pixelRect.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
