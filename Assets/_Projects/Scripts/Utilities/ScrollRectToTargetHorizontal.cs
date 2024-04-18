using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectToTargetHorizontal : MonoBehaviour
{
    [SerializeField] private float unit;
    [SerializeField] private float durationPerUnit;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    
    private int _targetIndex;
    private IEnumerator _scrolling;

    public void ScrollToTargetIndex(int index, int slotItemCount)
    {
        ScrollToTargetIndex(index, slotItemCount, scrollRect, content);
    }
    public void ScrollToTargetIndex(int index, int slotItemCount, ScrollRect scrollRect, RectTransform content)
    {
        _targetIndex = Mathf.Clamp(index, 0, slotItemCount - 1);
        StopScrolling();

        _scrolling = ScrollHorizontalToCenterElement(index, scrollRect, content);
        StartCoroutine(_scrolling);
    }
    
    private void StopScrolling()
    {
        if (_scrolling == null) return;
        StopCoroutine(_scrolling);
        _scrolling = null;
    }
    
    private IEnumerator ScrollHorizontalToLeftAlignedElement(ScrollRect scrollRect, RectTransform content)
    {
        yield return null;
        scrollRect.enabled = false;
        float minPosition = 0;
        float maxPosition = Mathf.Max(content.rect.width - scrollRect.viewport.rect.width, 0);
        var startPosition = content.anchoredPosition.x;
        var targetPosition = Mathf.Clamp(_targetIndex * unit, minPosition, maxPosition) * -1;
        var duration = Mathf.Abs(targetPosition - startPosition) / unit * durationPerUnit;
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var normalizedTime = elapsedTime / duration;
            var newPositionX = Mathf.LerpUnclamped(startPosition, targetPosition, curve.Evaluate(normalizedTime));
            content.anchoredPosition = new Vector2(newPositionX, content.anchoredPosition.y);
            yield return null;
        }

        content.anchoredPosition = new Vector2(targetPosition, content.anchoredPosition.y);
        scrollRect.enabled = true;
        _scrolling = null;
    }
    
    private IEnumerator ScrollHorizontalToCenterElement(int index, ScrollRect scrollRect, RectTransform content)
    {
        yield return null;
        scrollRect.enabled = false;
        float viewportWidth = scrollRect.viewport.rect.width;
        float targetPositionX = index * unit - viewportWidth / 2 + unit / 2;
        targetPositionX = Mathf.Clamp(targetPositionX, 0, content.rect.width - viewportWidth);
        
        float startPositionX = content.anchoredPosition.x;
        
        float timeElapsed = 0;

        while (timeElapsed < durationPerUnit)
        {
            timeElapsed += Time.deltaTime;
            float normalizedTime = timeElapsed / durationPerUnit;
            float newPositionX = Mathf.Lerp(startPositionX, -targetPositionX, curve.Evaluate(normalizedTime));
            content.anchoredPosition = new Vector2(newPositionX, content.anchoredPosition.y);
            yield return null;
        }

        content.anchoredPosition = new Vector2(-targetPositionX, content.anchoredPosition.y);
        scrollRect.enabled = true;
        _scrolling = null;
    }
}
