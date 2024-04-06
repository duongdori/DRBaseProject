using System;
using UnityEngine;

namespace DR.Framework.FSM
{
    partial class StateMachine<TKey, TState>
    {
        [Serializable]
        public new class WithDefault : StateMachine<TKey, TState>
        {
            [SerializeField] private TKey defaultKey;

            public TKey DefaultKey
            {
                get => defaultKey;
                set
                {
                    defaultKey = value;
                    if (CurrentState == null && value != null)
                        ForceSetState(value);
                }
            }
           
            public readonly Action ForceSetDefaultState;
            
            public WithDefault()
            {
                ForceSetDefaultState = () => ForceSetState(defaultKey);
            }

            public WithDefault(TKey defaultKey)
                : this()
            {
                this.defaultKey = defaultKey;
                ForceSetState(defaultKey);
            }

            public override void InitializeAfterDeserialize()
            {
                if (CurrentState != null)
                {
                    using (new KeyChange<TKey>(this, default, defaultKey))
                    using (new StateChange<TState>(this, null, CurrentState))
                        CurrentState.OnEnterState();
                }
                else
                {
                    ForceSetState(defaultKey);
                }

            }
            
            public TState TrySetDefaultState() => TrySetState(defaultKey);
            
            public TState TryResetDefaultState() => TryResetState(defaultKey);

#if UNITY_EDITOR

            public override int GUILineCount => 2;

            public override void DoGUI(ref Rect area)
            {
                area.height = UnityEditor.EditorGUIUtility.singleLineHeight;

                UnityEditor.EditorGUI.BeginChangeCheck();

                var state = StateMachineUtilities.DoGenericField(area, "Default Key", DefaultKey);

                if (UnityEditor.EditorGUI.EndChangeCheck())
                    DefaultKey = state;

                StateMachineUtilities.NextVerticalArea(ref area);

                base.DoGUI(ref area);
            }
#endif
        }
    }
}