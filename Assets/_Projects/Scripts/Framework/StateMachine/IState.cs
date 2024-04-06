using UnityEngine;

namespace DR.Framework.FSM
{
    public interface IState
    {
        // Can this state be entered?
        // Checked by CanSetState(), TrySetState() and TryResetState().
        // Not checked by ForceSetState().
        bool CanEnterState { get; }
        
        // Can this state be exited?
        // Checked by CanSetState(), TrySetState() and TryResetState().
        // Not checked by ForceSetState().
        bool CanExitState { get; }
        
        // Called when this state is entered.
        // Called by TrySetState(), TryResetState() and ForceSetState().
        void OnEnterState();
        
        // Called when this state is exited.
        // Called by TrySetState(), TryResetState() and ForceSetState().
        void OnExitState();
    }
}

