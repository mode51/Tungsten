using System;
using System.Threading;

namespace W.Threading
{
    /// <summary>
    /// Contains a generic extension method to quickly start a new thread
    /// </summary>
    public static class ThreadExtensions
    {
        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="this">The object to send into a new Thread</param>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<T> CreateThread<T>(this T @this, Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete)
        {
            var thread = W.Threading.Thread<T>.Create(action, onComplete, @this);
            return thread;
        }
        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="customData">The data to pass to the thread (Action)</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<T> CreateThread<T>(this object @this, Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete, T customData)
        {
            var thread = W.Threading.Thread<T>.Create(action, onComplete, customData);
            return thread;
        }
    }
}
