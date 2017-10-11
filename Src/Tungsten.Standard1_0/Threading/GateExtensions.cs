using System;
using System.Threading;

namespace W.Threading
{
    /// <summary>
    /// Extension methods on Action to Create a Gate
    /// </summary>
    public static class GateMethods
    {
        private class Test
        {
            private Action<int, CancellationToken> Action { get; set;}
            public Test()
            {
                Action += (value, token) =>
                {
                    System.Diagnostics.Debug.WriteLine("Value = {0}", value);
                };
                Action.Invoke(5, CancellationToken.None);

                var gate = Action.AsGate<int>();
                gate.Run(5);
                gate.Join();
                gate.Run(50);
                gate.Join();
            }
        }
        
        /// <summary>
        /// Creates a Gate with the supplied action
        /// </summary>
        /// <param name="action">The Action to call when the gate is relased (when Run is called)</param>
        /// <returns>A reference to a new Gate</returns>
        public static Gate AsGate(this Action<CancellationToken> @action)
        {
            return new Gate(action);
        }
        /// <summary>
        /// Creates a Gate with the supplied action
        /// </summary>
        /// <typeparam name="TParameterType">The Type of the parameter to be passed in</typeparam>
        /// <param name="action">The Action to call when the gate is released (when Run is called)</param>
        /// <returns>A reference to a new Gate</returns>
        public static Gate<TParameterType> AsGate<TParameterType>(this Action<TParameterType, CancellationToken> @action)
        {
            return new Gate<TParameterType>(action);
        }
    }
}
