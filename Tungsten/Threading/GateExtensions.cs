using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    public static class GateMethods
    {
        /// <summary>
        /// Creates a Gate with the supplied action
        /// </summary>
        /// <param name="action">The Action to call when the gate is relased (when Run is called)</param>
        /// <returns>A reference to a new Gate</returns>
        public static Gate CreateGate(this Action<CancellationTokenSource> @action)
        {
            return new Gate(action);
        }
        /// <summary>
        /// Creates a Gate with the supplied action
        /// </summary>
        /// <param name="action">The Action to call when the gate is relased (when Run is called)</param>
        /// <returns>A reference to a new Gate</returns>
        public static Gate<T> CreateGate<T>(this Action<T, CancellationTokenSource> @action)
        {
            return new Gate<T>(action);
        }
    }
}
