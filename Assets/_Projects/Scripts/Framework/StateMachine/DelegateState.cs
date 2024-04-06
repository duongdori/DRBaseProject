using System;
using UnityEngine;

namespace DR.Framework.FSM
{
    public class DelegateState : IState
    {
        public Func<bool> CanEnter;
        
        public virtual bool CanEnterState => CanEnter == null || CanEnter();
    
        public Func<bool> CanExit;
    
        public virtual bool CanExitState => CanExit == null || CanExit();
    
        public Action OnEnter;
    
        public virtual void OnEnterState() => OnEnter?.Invoke();
    
        public Action OnExit;
    
        public virtual void OnExitState() => OnExit?.Invoke();
    }
}