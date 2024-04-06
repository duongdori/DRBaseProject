using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DR.Framework.FSM
{
        [Serializable]
        public partial class StateMachine<TState> : IStateMachine where TState : class, IState
        {
                [SerializeField] private TState currentState;

                public TState CurrentState => currentState;

                public TState PreviousState => StateChange<TState>.PreviousState;

                public TState NextState => StateChange<TState>.NextState;

                public StateMachine()
                {
                }

                public StateMachine(TState state)
                {
#if UNITY_ASSERTIONS
                        if (state == null)
                                throw new ArgumentNullException(nameof(state), NullNotAllowed);
#endif

                        using (new StateChange<TState>(this, null, state))
                        {
                                currentState = state;
                                state.OnEnterState();
                        }
                }

                public virtual void InitializeAfterDeserialize()
                {
                        if (currentState == null) return;
                        using (new StateChange<TState>(this, null, currentState))
                                currentState.OnEnterState();
                }

                public bool CanSetState(TState state)
                {
#if UNITY_ASSERTIONS
                        if (state == null && !AllowNullStates)
                                throw new ArgumentNullException(nameof(state), NullNotAllowed);
#endif

                        using (new StateChange<TState>(this, currentState, state))
                        {
                                if (currentState != null && !currentState.CanExitState) return false;

                                return state == null || state.CanEnterState;
                        }
                }

                public TState CanSetState(IList<TState> states)
                {
                        var count = states.Count;
                        for (int i = 0; i < count; i++)
                        {
                                var state = states[i];
                                if (CanSetState(state)) return state;
                        }

                        return null;
                }

                public bool TrySetState(TState state)
                {
                        if (currentState != state) return TryResetState(state);
#if UNITY_ASSERTIONS
                        if (state == null && !AllowNullStates)
                                throw new ArgumentNullException(nameof(state), NullNotAllowed);
#endif
                        return true;
                }

                public bool TrySetState(IList<TState> states)
                {
                        var count = states.Count;
                        for (int i = 0; i < count; i++)
                        {
                                if (TrySetState(states[i])) return true;
                        }

                        return false;
                }

                public bool TryResetState(TState state)
                {
                        if (!CanSetState(state)) return false;

                        ForceSetState(state);
                        return true;
                }

                public bool TryResetState(IList<TState> states)
                {
                        var count = states.Count;
                        for (int i = 0; i < count; i++)
                                if (TryResetState(states[i]))
                                        return true;

                        return false;
                }

                public void ForceSetState(TState state)
                {
#if UNITY_ASSERTIONS
                        if (state == null)
                        {
                                if (!AllowNullStates)
                                        throw new ArgumentNullException(nameof(state), NullNotAllowed);
                        }
                        else if (state is IOwnedState<TState> owned && owned.OwnerStateMachine != this)
                        {
                                throw new InvalidOperationException(
                                        $"Attempted to use a state in a machine that is not its owner." +
                                        $"\n• State: {state}" +
                                        $"\n• Machine: {this}");
                        }
#endif
                        using (new StateChange<TState>(this, currentState, state))
                        {
                                currentState?.OnExitState();

                                currentState = state;

                                state?.OnEnterState();
                        }
                }

                public override string ToString() => $"{GetType().Name} -> {currentState}";


#if UNITY_ASSERTIONS
                public bool AllowNullStates { get; private set; }

                private const string NullNotAllowed =
                        "This " + nameof(StateMachine<TState>) + " does not allow its state to be set to null." +
                        " Use " + nameof(SetAllowNullStates) + " to allow it if this is intentional.";
#endif

                [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
                public void SetAllowNullStates(bool allow = true)
                {
#if UNITY_ASSERTIONS
                        AllowNullStates = allow;
#endif
                }

                #region GUI

#if UNITY_EDITOR

                public virtual int GUILineCount => 1;


                public void DoGUI()
                {
                        var spacing = UnityEditor.EditorGUIUtility.standardVerticalSpacing;
                        var lines = GUILineCount;
                        var height =
                                UnityEditor.EditorGUIUtility.singleLineHeight * lines +
                                spacing * (lines - 1);

                        var area = GUILayoutUtility.GetRect(0, height);
                        area.height -= spacing;

                        DoGUI(ref area);
                }


                public virtual void DoGUI(ref Rect area)
                {
                        area.height = UnityEditor.EditorGUIUtility.singleLineHeight;

                        UnityEditor.EditorGUI.BeginChangeCheck();

                        var state = StateMachineUtilities.DoGenericField(area, "Current State", currentState);

                        if (UnityEditor.EditorGUI.EndChangeCheck())
                        {
                                if (Event.current.control)
                                        ForceSetState(state);
                                else
                                        TrySetState(state);
                        }

                        StateMachineUtilities.NextVerticalArea(ref area);
                }

#endif

                #endregion

                #region IStateMachine

                object IStateMachine.CurrentState => currentState;

                object IStateMachine.PreviousState => PreviousState;

                object IStateMachine.NextState => NextState;

                object IStateMachine.CanSetState(IList states) => CanSetState((List<TState>)states);

                bool IStateMachine.CanSetState(object state) => CanSetState((TState)state);

                void IStateMachine.ForceSetState(object state) => ForceSetState((TState)state);

                bool IStateMachine.TryResetState(IList states) => TryResetState((List<TState>)states);

                bool IStateMachine.TryResetState(object state) => TryResetState((TState)state);

                bool IStateMachine.TrySetState(IList states) => TrySetState((List<TState>)states);

                bool IStateMachine.TrySetState(object state) => TrySetState((TState)state);

                void IStateMachine.SetAllowNullStates(bool allow) => SetAllowNullStates(allow);

                #endregion
        }
}
