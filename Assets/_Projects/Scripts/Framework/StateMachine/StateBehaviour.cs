using UnityEngine;

namespace DR.Framework.FSM
{
    public class StateBehaviour : MonoBehaviour, IState
    {
        public virtual bool CanEnterState => true;
        public virtual bool CanExitState => true;

        public virtual void OnEnterState()
        {
#if UNITY_ASSERTIONS
            if (enabled)
                Debug.LogError($"{nameof(StateBehaviour)} was already enabled before {nameof(OnEnterState)}: {this}", this);
#endif
#if UNITY_EDITOR
            else
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif

            enabled = true;
        }
        
        public virtual void OnExitState()
        {
            if (this == null)
                return;

#if UNITY_ASSERTIONS
            if (!enabled)
                Debug.LogError($"{nameof(StateBehaviour)} was already disabled before {nameof(OnExitState)}: {this}", this);
#endif

            enabled = false;
        }
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            enabled = false;
        }
#endif
    }
}