namespace DR.Framework.FSM
{
    public abstract class State : IState
    {
        public virtual bool CanEnterState => true;

        public virtual bool CanExitState => true;

        public virtual void OnEnterState() { }

        public virtual void OnExitState() { }

    }
}