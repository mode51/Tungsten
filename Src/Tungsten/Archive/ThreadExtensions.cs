using System;
using System.Threading;
#if WINDOWS_UWP
using Windows.System.Threading;
#endif

namespace W.Threading
{
    /// <summary>
    /// Contains a generic extension method to quickly start a new thread
    /// </summary>
    public static class ThreadExtensions
    {
#if WINDOWS_PORTABLE || WINDOWS_UWP
        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="this">The custom data to be passed to the thread (Action)</param>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="cts">Can be used to control cancelation externally</param>
        /// <returns>Returns a reference to the new Thread</returns>
        public static W.Threading.Thread<T> CreateThread<T>(this T @this, Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete, CancellationTokenSource cts)
        {
            var thread = W.Threading.Thread<T>.Create(action, onComplete, cts, @this);
            return thread;
        }
#else
        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="this">The custom data to be passed to the thread (Action)</param>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="cts">Can be used to control cancelation externally</param>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<T> CreateThread<T>(this T @this, Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete, CancellationTokenSource cts)
        {
            var thread = W.Threading.Thread<T>.Create(action, onComplete, cts, @this);
            return thread;
        }
        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="this">The custom data to be passed to the thread (Action)</param>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="cts">Can be used to control cancelation externally</param>
        /// <param name="customData">The data to pass to the thread (Action)</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>A reference to the new W.Threading.Thread&lt;T&gt;</returns>
        public static W.Threading.Thread<T> CreateThread<T>(this object @this, Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete, CancellationTokenSource cts, T customData)
        {
            var thread = W.Threading.Thread<T>.Create(action, onComplete, cts, customData);
            return thread;
        }
#endif
    }
}
