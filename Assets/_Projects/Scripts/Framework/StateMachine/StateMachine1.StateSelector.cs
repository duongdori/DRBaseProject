using System.Collections.Generic;

namespace DR.Framework.FSM
{
    public partial class StateMachine<TState>
    {
        public class StateSelector : SortedList<float, TState>
        {
            public StateSelector() : base(ReverseComparer<float>._instance) { }

            public void Add<TPrioritization>(TPrioritization state)
                where TPrioritization : TState, IPrioritization
                => Add(state.Priority, state);
        }
    }
}