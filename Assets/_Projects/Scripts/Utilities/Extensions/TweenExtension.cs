using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class TweenExtension
    {
        public static void DoTransform(this Transform tf, Transform target, float duration, float delay = 0f, System.Action onCompleted = null)
        {
            tf.DOMove(target.position, duration)
                .SetDelay(delay);

            tf.DORotateQuaternion(target.rotation, duration)
                .SetDelay(delay)
                .OnComplete(() => { onCompleted?.Invoke(); });
        }

        public static void DoLocalTransform(this Transform tf, Transform target, float duration, float delay = 0f, System.Action onCompleted = null)
        {
            tf.DOLocalMove(target.localPosition, duration)
                .SetDelay(delay);

            tf.DOLocalRotateQuaternion(target.localRotation, duration)
                .SetDelay(delay)
                .OnComplete(() => { onCompleted?.Invoke(); });
        }

        public static void DoPunchScaleEff(this Transform target, float punch = 0.17f, Ease ease = Ease.Linear)
        {
            if (target == null) return;
        
            target.DOKill();
            target.DOPunchScale(new Vector3(punch, punch, punch), 0.15f, 2).SetEase(ease);
        }
        public static void DoProgress(this RectTransform rtf, float max, float percent, MonoBehaviour monoBehaviour = null, float duration = 0f)
        {
            float width = percent * max;
            if (duration > 0f)
            {
                Vector2 start = rtf.sizeDelta;
                Vector2 end = new Vector2(width, start.y);
                // MEC.Timing.RunCoroutine(I_Progress(rtf, start, end, duration));
                if (monoBehaviour != null) monoBehaviour.StartCoroutine(I_Progress(rtf, start, end, duration));
            }
            else
            {
                rtf.sizeDelta = new Vector2(width, rtf.sizeDelta.y);
            }
        }
        
        private static IEnumerator<float> I_Progress(RectTransform rtf, Vector2 start, Vector2 end, float duration)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                rtf.sizeDelta = Vector2.Lerp(start, end, t);
                yield return 0f;
            }
        }
    }
}