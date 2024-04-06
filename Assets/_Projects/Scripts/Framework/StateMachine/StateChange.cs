using System;
using UnityEngine;

namespace DR.Framework.FSM
{
    public struct StateChange<TState> : IDisposable where TState : class, IState
    {
        private static StateChange<TState> _current;

        private StateMachine<TState> _stateMachine;
        private TState _previousState;
        private TState _nextState;
        
        public static bool IsActive => _current._stateMachine != null;
        
        public static StateMachine<TState> StateMachine => _current._stateMachine;

        public static TState PreviousState
        {
            get
            {
#if UNITY_ASSERTIONS
                if (!IsActive)
                    throw new InvalidOperationException(StateExtensions.GetChangeError(typeof(TState), typeof(StateMachine<>)));
#endif
                return _current._previousState;
            }
        }
        
        public static TState NextState
        {
            get
            {
#if UNITY_ASSERTIONS
                if (!IsActive)
                    throw new InvalidOperationException(StateExtensions.GetChangeError(typeof(TState), typeof(StateMachine<>)));
#endif
                return _current._nextState;
            }
        }
        
        internal StateChange(StateMachine<TState> stateMachine, TState previousState, TState nextState)
        {
            this = _current;

            _current._stateMachine = stateMachine;
            _current._previousState = previousState;
            _current._nextState = nextState;
        }
        
        public void Dispose()
        {
            _current = this;
        }
        
        public override string ToString() => IsActive ?
            $"{nameof(StateChange<TState>)}<{typeof(TState).FullName}" +
            $">({nameof(PreviousState)}='{_previousState}'" +
            $", {nameof(NextState)}='{_nextState}')" :
            $"{nameof(StateChange<TState>)}<{typeof(TState).FullName}(Not Currently Active)";

        public static string CurrentToString() => _current.ToString();
    }
}

