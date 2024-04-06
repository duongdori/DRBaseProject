using System;
using UnityEngine;

namespace DR.Framework.FSM
{
    public partial class StateMachine<TState>
    {
        [Serializable]
        public class WithDefault : StateMachine<TState>
        {

            [SerializeField] private TState defaultState;

            public TState DefaultState
            {
                get => defaultState;
                set
                {
                    defaultState = value;
                    if (currentState == null && value != null)
                        ForceSetState(value);
                }
            }

            public readonly Action ForceSetDefaultState;

            public WithDefault()
            {
                ForceSetDefaultState = () => ForceSetState(defaultState);
            }


            public WithDefault(TState defaultState) : this()
            {
                this.defaultState = defaultState;
                ForceSetState(defaultState);
            }


            public override void InitializeAfterDeserialize()
            {
                if (currentState != null)
                {
                    using (new StateChange<TState>(this, null, currentState))
                        currentState.OnEnterState();
                }
                else if (defaultState != null)
                {
                    using (new StateChange<TState>(this, null, CurrentState))
                    {
                        currentState = defaultState;
                        currentState.OnEnterState();
                    }
                }

            }

            public bool TrySetDefaultState() => TrySetState(DefaultState);

            public bool TryResetDefaultState() => TryResetState(DefaultState);

#if UNITY_EDITOR

            public override int GUILineCount => 2;


            public override void DoGUI(ref Rect area)
            {
                area.height = UnityEditor.EditorGUIUtility.singleLineHeight;

                UnityEditor.EditorGUI.BeginChangeCheck();

                var state = StateMachineUtilities.DoGenericField(area, "Default State", DefaultState);

                if (UnityEditor.EditorGUI.EndChangeCheck())
                    DefaultState = state;

                StateMachineUtilities.NextVerticalArea(ref area);

                base.DoGUI(ref area);
            }

#endif
        }
    }
}