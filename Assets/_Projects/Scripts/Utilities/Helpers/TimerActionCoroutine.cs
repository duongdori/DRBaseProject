using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DR.Utilities
{
    public class TimerActionCoroutine : MonoSingleton<TimerActionCoroutine>
    {
        private readonly Dictionary<Action, Coroutine> _runningCoroutines = new();
        private readonly Dictionary<Action, bool> _pausedCoroutines = new();
        private readonly Dictionary<Action, float> _elapsedTimes = new();
        
        public void StartTimedAction(float delay, Action action)
        {
            if(action == null) return;
            
            Coroutine coroutine = StartCoroutine(TimedActionCoroutine(delay, action));
            _runningCoroutines[action] = coroutine;
            _pausedCoroutines[action] = false;
            _elapsedTimes[action] = 0f;
        }
        
        public void StopAction(Action action)
        {
            if (!_runningCoroutines.TryGetValue(action, out Coroutine coroutine)) return;
            
            StopCoroutine(coroutine);
            _runningCoroutines.Remove(action);
            _pausedCoroutines.Remove(action);
            _elapsedTimes.Remove(action);
        }
        
        public void PauseAction(Action action)
        {
            if (!_runningCoroutines.ContainsKey(action)) return;
            
            _pausedCoroutines[action] = true;
        }
        
        public void ResumeAction(Action action)
        {
            if (!_runningCoroutines.ContainsKey(action)) return;
            
            _pausedCoroutines[action] = false;
        }
        
        public float GetElapsedTime(Action action)
        {
            return _elapsedTimes.GetValueOrDefault(action, 0f);
        }
        
        private IEnumerator TimedActionCoroutine(float delay, Action action)
        {
            float elapsed = 0f;
            while (elapsed < delay)
            {
                if(_pausedCoroutines.TryGetValue(action, out bool paused) && !paused)
                {
                    elapsed += Time.deltaTime;
                    _elapsedTimes[action] = elapsed;
                }
                
                yield return null;
            }
            
            if (action == null) yield break;
            action.Invoke();
            _runningCoroutines.Remove(action);
            _pausedCoroutines.Remove(action);
            _elapsedTimes.Remove(action);
        }
    }
}

