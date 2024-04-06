namespace DR.Framework.FSM
{
    public interface IOwnedState<TState> : IState where TState : class, IState
    {
        StateMachine<TState> OwnerStateMachine { get; }
    }
}