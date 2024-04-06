using System.Collections;
using UnityEngine;

namespace DR.Framework.FSM
{
    public interface IStateMachine
    {
        object CurrentState { get; }
        
        object PreviousState { get; }

        object NextState { get; }

        bool CanSetState(object state);

        object CanSetState(IList states);
        
        bool TrySetState(object state);
        
        bool TrySetState(IList states);

        bool TryResetState(object state);
        
        bool TryResetState(IList states);

        void ForceSetState(object state);

#if UNITY_ASSERTIONS
        bool AllowNullStates { get; }
#endif

        void SetAllowNullStates(bool allow = true);
        
#if UNITY_EDITOR

        int GUILineCount { get; }

        void DoGUI();

        void DoGUI(ref Rect area);
#endif
            
    }
}