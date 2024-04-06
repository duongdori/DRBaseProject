namespace DR.Framework.FSM
{
    public interface IPrioritization : IState
    {
        float Priority { get; }
    }
}