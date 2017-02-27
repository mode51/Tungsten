using System;
using System.Threading;

namespace W.Threading
{
    /// <summary>
    /// Extension methods on Action to Create a Gate
    /// </summary>
    public static class GateMethods
    {
        /// <summary>
        /// Creates a Gate with the supplied action
        /// </summary>
        /// <param name="action">The Action to call when the gate is relased (when Run is called)</param>
        /// <returns>A reference to a new Gate</returns>
        public static Gate AsGate(this Action<CancellationTokenSource> @action)
        {
            return new Gate(action);
        }
        /// <summary>
        /// Creates a Gate with the supplied action
        /// </summary>
        /// <param name="action">The Action to call when the gate is relased (when Run is called)</param>
        /// <returns>A reference to a new Gate</returns>
        public static Gate<T> AsGate<T>(this Action<T, CancellationTokenSource> @action)
        {
            return new Gate<T>(action);
        }
    }
}
