using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DR.Utilities.Extensions
{
    public static class MonoBehaviourExtension
    {
        // Runs the Callback at the end of the current frame, after GUI rendering
        public static Coroutine OnEndOfFrame(this MonoBehaviour self, UnityAction callback)
        {
            return self.StartCoroutine(EndOfFrameCoroutine(callback));
        }

        private static IEnumerator EndOfFrameCoroutine(UnityAction callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }
        
        // Runs the Callback after the next Update completes
        public static Coroutine OnUpdate(this MonoBehaviour self, UnityAction callback)
        {
            return self.InUpdates(1, callback);
        }

        // Runs the Callback after a given number of Updates complete
        public static Coroutine InUpdates(this MonoBehaviour self, int updates, UnityAction callback)
        {
            return self.StartCoroutine(InUpdatesCoroutine(updates, callback));
        }

        private static IEnumerator InUpdatesCoroutine(int updates, UnityAction callback)
        {
            for (int i = 0; i < updates; i++)
            {
                yield return null;
            }
            callback?.Invoke();
        }
        
        // Runs the Callback after the next FixedUpdate completes
        public static Coroutine OnFixedUpdate(this MonoBehaviour self, UnityAction callback)
        {
            return self.InFixedUpdates(1, callback);
        }

        // Runs the Callback after a given number of FixedUpdates complete
        public static Coroutine InFixedUpdates(this MonoBehaviour self, int ticks, UnityAction callback)
        {
            return self.StartCoroutine(InFixedUpdatesCoroutine(ticks, callback));
        }

        private static IEnumerator InFixedUpdatesCoroutine(int ticks, UnityAction callback)
        {
            for (int i = 0; i < ticks; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            callback?.Invoke();
        }
        
        // Runs the Callback after a given number of seconds, after the Update completes
        public static Coroutine InSeconds(this MonoBehaviour self, float seconds, UnityAction callback)
        {
            return self.StartCoroutine(InSecondsCoroutine(seconds, callback));
        }

        private static IEnumerator InSecondsCoroutine(float seconds, UnityAction callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
        
        public static Coroutine InSecondsInterval(this MonoBehaviour self, float seconds, float interval, UnityAction callback)
        {
            return self.StartCoroutine(InSecondsIntervalCoroutine(self, seconds, interval, callback));
        }

        private static IEnumerator InSecondsIntervalCoroutine(MonoBehaviour self, float seconds, float interval, UnityAction callback)
        {
            yield return new WaitForSeconds(seconds);
            while (self.gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(interval);
                callback?.Invoke();
            }
        }
        
        public static void SmoothLookAtInSeconds(this MonoBehaviour self, Transform tf, Transform target, float damping, float duration, System.Action onCompleted = null)
        {
            self.StartCoroutine(I_SmoothLookAtInSeconds(tf, target, damping, duration, onCompleted));
        }

        private static IEnumerator I_SmoothLookAtInSeconds(Transform tf, Transform target, float damping, float duration, System.Action onCompleted)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                var targetRotation = Quaternion.LookRotation(target.position - tf.position);
                tf.rotation = Quaternion.Slerp(tf.rotation, targetRotation, t /* * damping */);
                yield return null;
            }
            onCompleted?.Invoke();
        }
        
        public static void InterpolateColor(this MonoBehaviour self, Renderer renderer, Color color1, Color color2, float duration = 0f, System.Action onCompleted = null)
        {
            self.StartCoroutine(I_SetColor(renderer, color1, color2, duration, onCompleted));
        }

        private static IEnumerator I_SetColor(Renderer renderer, Color color1, Color color2, float duration, System.Action onCompleted)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                propBlock.SetColor("_Color", Color.Lerp(color1, color2, Mathf.SmoothStep(0f, 1f, t)));
                renderer.SetPropertyBlock(propBlock);
                yield return null;
            }
            onCompleted?.Invoke();
        }
    }
}