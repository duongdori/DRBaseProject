using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DR.Framework.FSM
{
    [Serializable]
    public partial class StateMachine<TKey, TState> : StateMachine<TState>, IKeyedStateMachine<TKey>, IDictionary<TKey, TState>
        where TState : class, IState
    {
        public IDictionary<TKey, TState> Dictionary { get; set; }

        [SerializeField] private TKey currentKey;

        public TKey CurrentKey => currentKey;
        
        public TKey PreviousKey => KeyChange<TKey>.PreviousKey;

        public TKey NextKey => KeyChange<TKey>.NextKey;
        
        public StateMachine()
        {
            Dictionary = new Dictionary<TKey, TState>();
        }
        
        public StateMachine(IDictionary<TKey, TState> dictionary)
        {
            Dictionary = dictionary;
        }
        
        public StateMachine(TKey defaultKey, TState defaultState)
        {
            Dictionary = new Dictionary<TKey, TState>
            {
                { defaultKey, defaultState }
            };
            ForceSetState(defaultKey, defaultState);
        }
        
        public StateMachine(IDictionary<TKey, TState> dictionary, TKey defaultKey, TState defaultState)
        {
            Dictionary = dictionary;
            dictionary.Add(defaultKey, defaultState);
            ForceSetState(defaultKey, defaultState);
        }
        
        public override void InitializeAfterDeserialize()
        {
            if (CurrentState != null)
            {
                using (new KeyChange<TKey>(this, default, currentKey))
                using (new StateChange<TState>(this, null, CurrentState))
                    CurrentState.OnEnterState();
            }
            else if (Dictionary.TryGetValue(currentKey, out var state))
            {
                ForceSetState(currentKey, state);
            }

        }
        
        public bool TrySetState(TKey key, TState state)
        {
            return CurrentState == state || TryResetState(key, state);
        }
        
        public TState TrySetState(TKey key)
        {
            return EqualityComparer<TKey>.Default.Equals(currentKey, key) ? CurrentState : TryResetState(key);
        }
        
        object IKeyedStateMachine<TKey>.TrySetState(TKey key) => TrySetState(key);

        public bool TryResetState(TKey key, TState state)
        {
            using (new KeyChange<TKey>(this, currentKey, key))
            {
                if (!CanSetState(state))
                    return false;

                currentKey = key;
                ForceSetState(state);
                return true;
            }
        }
        
        public TState TryResetState(TKey key)
        {
            if (Dictionary.TryGetValue(key, out var state) && TryResetState(key, state)) return state;
            
            return null;
        }

        object IKeyedStateMachine<TKey>.TryResetState(TKey key) => TryResetState(key);

        public void ForceSetState(TKey key, TState state)
        {
            using (new KeyChange<TKey>(this, currentKey, key))
            {
                currentKey = key;
                ForceSetState(state);
            }
        }
        
        public TState ForceSetState(TKey key)
        {
            Dictionary.TryGetValue(key, out var state);
            ForceSetState(key, state);
            return state;
        }
        
        object IKeyedStateMachine<TKey>.ForceSetState(TKey key) => ForceSetState(key);

        #region Dictionary Wrappers

        public TState this[TKey key]
        {
            get => Dictionary[key];
            set => Dictionary[key] = value;
        }

        public bool TryGetValue(TKey key, out TState state) => Dictionary.TryGetValue(key, out state);


        public ICollection<TKey> Keys => Dictionary.Keys;

        public ICollection<TState> Values => Dictionary.Values;


        public int Count => Dictionary.Count;


        public void Add(TKey key, TState state) => Dictionary.Add(key, state);

        public void Add(KeyValuePair<TKey, TState> item) => Dictionary.Add(item);


        public bool Remove(TKey key) => Dictionary.Remove(key);

        public bool Remove(KeyValuePair<TKey, TState> item) => Dictionary.Remove(item);


        public void Clear() => Dictionary.Clear();


        public bool Contains(KeyValuePair<TKey, TState> item) => Dictionary.Contains(item);

        public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TState>> GetEnumerator() => Dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(KeyValuePair<TKey, TState>[] array, int arrayIndex) => Dictionary.CopyTo(array, arrayIndex);

        bool ICollection<KeyValuePair<TKey, TState>>.IsReadOnly => Dictionary.IsReadOnly;

        #endregion
        
        public TState GetState(TKey key)
        {
            TryGetValue(key, out var state);
            return state;
        }

        public void AddRange(TKey[] keys, TState[] states)
        {
            Debug.Assert(keys.Length == states.Length,
                $"The '{nameof(keys)}' and '{nameof(states)}' arrays must be the same size.");

            for (int i = 0; i < keys.Length; i++)
            {
                Dictionary.Add(keys[i], states[i]);
            }
        }

        public void SetFakeKey(TKey key) => currentKey = key;

        public override string ToString()
            => $"{GetType().FullName} -> {currentKey} -> {(CurrentState != null ? CurrentState.ToString() : "null")}";
        
#if UNITY_EDITOR

        public override int GUILineCount => 2;

        public override void DoGUI(ref Rect area)
        {
            area.height = UnityEditor.EditorGUIUtility.singleLineHeight;

            UnityEditor.EditorGUI.BeginChangeCheck();

            var key = StateMachineUtilities.DoGenericField(area, "Current Key", currentKey);

            if (UnityEditor.EditorGUI.EndChangeCheck())
                SetFakeKey(key);

            StateMachineUtilities.NextVerticalArea(ref area);

            base.DoGUI(ref area);
        }

#endif
    }
}