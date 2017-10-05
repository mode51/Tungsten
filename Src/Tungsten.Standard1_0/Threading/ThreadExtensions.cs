using System;
using System.Threading;

namespace W.Threading
{
    /// <summary>
    /// Contains a generic extension method to quickly start a new thread
    /// </summary>
    public static class ThreadExtensions
    {
        private class Test
        {
            public Test()
            {
                int value = 100;
                value.CreateThread<int>((v, token) => { });
            }
        }
        /// <summary>
        /// Creates and starts a new thread
        /// </summary>
        /// <param name="this">The object to send into a new Thread</param>
        /// <param name="action">Action to call on a thread</param>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<TParameterType> CreateThread<TParameterType>(this TParameterType @this, Action<TParameterType, CancellationToken> action)
        {
            var thread = new W.Threading.Thread<TParameterType>(action, true);
            return thread;
        }
        /// <summary>
        /// Creates a new thread
        /// </summary>
        /// <param name="this">The object to send into a new Thread</param>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="autoStart">If True, the thread will immediately start running</param>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<TParameterType> CreateThread<TParameterType>(this TParameterType @this, Action<TParameterType, CancellationToken> action, bool autoStart)
        {
            var thread = new W.Threading.Thread<TParameterType>(action, autoStart);
            return thread;
        }
        /// <summary>
        /// Creates and starts a new thread and 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action">Action to call on a thread</param>
        /// <typeparam name="TParameterType"></typeparam>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<TParameterType> CreateThread<TParameterType>(this object @this, Action<TParameterType, CancellationToken> action)
        {
            var thread = new W.Threading.Thread<TParameterType>(action, true);
            return thread;
        }
        /// <summary>
        /// Creates a new thread
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="autoStart">If True, the thread will immediately start running</param>
        /// <typeparam name="TParameterType"></typeparam>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<TParameterType> CreateThread<TParameterType>(this object @this, Action<TParameterType, CancellationToken> action, bool autoStart)
        {
            var thread = new W.Threading.Thread<TParameterType>(action, autoStart);
            return thread;
        }
    }
}
