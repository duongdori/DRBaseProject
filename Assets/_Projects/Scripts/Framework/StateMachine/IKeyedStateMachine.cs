namespace DR.Framework.FSM
{
    public interface IKeyedStateMachine<TKey>
    {
        TKey CurrentKey { get; }

        TKey PreviousKey { get; }

        TKey NextKey { get; }
        
        object TrySetState(TKey key);
        
        object TryResetState(TKey key);

        object ForceSetState(TKey key);

    }
}