//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace W.Threading
//{
//    public partial class Thread : ThreadBase
//    {
//        /// <summary>
//        /// Blocks the current thread for the given number of milliseconds
//        /// </summary>
//        /// <param name="msDuration">The number of milliseconds to block</param>
//        /// <remarks>This method is provided to simplify cross-platform code.</remarks>
//        public static void Sleep(int msDuration)
//        {
//#if WINDOWS_PORTABLE || WINDOWS_UWP
//            System.Threading.Tasks.Task.Delay(msDuration).Wait();
//#else
//            System.Threading.Thread.Sleep(msDuration);
//#endif
//        }

//        /// <summary>
//        /// Starts a new thread
//        /// </summary>
//        /// <param name="action">Action to call on a thread</param>
//        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
//        /// <param name="cts">Can be used to control cancelation externally</param>
//        /// <returns></returns>
//        public static IThread Create(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null, CancellationTokenSource cts = null)
//        {
//            var result = new Thread(action, onComplete, cts);
//            return result;
//        }
//    }
//    public partial class Thread<T> : Thread
//    {
//        /// <summary>
//        /// Starts a new thread
//        /// </summary>
//        /// <param name="action">Action to call on a thread</param>
//        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
//        /// <param name="cts">can be use dto cancel the thread</param>
//        /// <param name="customData">The custom data to pass to the thread (Action)</param>
//        /// <returns></returns>
//        public static W.Threading.Thread<T> Create(Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete = null, CancellationTokenSource cts = null, T customData = default(T))
//        {
//            var thread = new W.Threading.Thread<T>(action, onComplete, cts, customData);
//            return thread;
//        }
//    }
//}
