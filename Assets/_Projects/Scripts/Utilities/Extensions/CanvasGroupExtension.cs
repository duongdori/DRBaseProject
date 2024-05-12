using System;
using DG.Tweening;
using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class CanvasGroupExtension
    {
        public static void SetActive(this CanvasGroup canvasGroup, bool isActive, float fadeTime = 0.0f, Ease ease = Ease.Linear, Action onComplete = null)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(isActive ? 1.0f : 0.0f, fadeTime).SetEase(ease).OnComplete(() => onComplete?.Invoke());
            canvasGroup.blocksRaycasts = isActive;
        }
        
        public static void FadeOut(this CanvasGroup canvasGroup, float duration, Action onComplete = null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0f, duration).OnComplete((() => onComplete?.Invoke()));
        }
        
        public static void FadeIn(this CanvasGroup canvasGroup, float duration, Action onComplete = null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1f, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        public static void FadeTo(this CanvasGroup canvasGroup, float targetAlpha, float duration, Action onComplete = null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(targetAlpha, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        public static void FadeAndScale(this CanvasGroup canvasGroup, float targetAlpha, Vector3 targetScale, float duration, Action onComplete = null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(targetAlpha, duration);
            canvasGroup.transform.DOScale(targetScale, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        public static void FadeInOut(this CanvasGroup canvasGroup, float targetAlpha, float duration, Action onComplete = null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(targetAlpha, duration)
                .OnComplete(() =>
                {
                    canvasGroup.DOFade(1f - targetAlpha, duration)
                        .OnComplete(() => onComplete?.Invoke());
                });
        }

        public static void Pulse(this CanvasGroup canvasGroup, float minAlpha, float maxAlpha, float duration,
            int loops = -1)
        {
            canvasGroup.alpha = minAlpha;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(maxAlpha, duration / 2)
                .SetLoops(loops, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad);
        }
        
        public static void IsVisible(this CanvasGroup canvasGroup, bool isVisible, float minAlpha = 0.0f)
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = isVisible ? 1 : minAlpha;
            canvasGroup.blocksRaycasts = isVisible;
        }
    }
}