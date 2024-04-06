using System.Collections.Generic;

namespace DR.Framework.FSM
{
    public class ReverseComparer<T> : IComparer<T>
    {
        public static readonly ReverseComparer<T> _instance = new ReverseComparer<T>();

        private ReverseComparer() { }

        public int Compare(T x, T y) => Comparer<T>.Default.Compare(y, x);
    }
}