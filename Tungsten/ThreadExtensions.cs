using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.System.Threading;
#endif

namespace W
{
    /// <summary>
    /// Contains a generic extension method to quickly start a new thread
    /// </summary>
    public static class ThreadExtensions
    {
        private class ThreadData<T>
        {
            public T Sender { get; set; }
            public Action<T> Action { get; set; }
            public Action<bool, Exception> OnComplete { get; set; }
        }
#if WINDOWS_PORTABLE || WINDOWS_UWP
        private static void ThreadProc<T>(object data)
        {
            var dt = data as ThreadData<T>;
            bool success = false;
            Exception ex = null;
            if (dt == null)
                return;
            try
            {
                dt.Action?.Invoke(dt.Sender);
                success = true;
            }
            catch (TaskSchedulerException e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
            }
            catch (TaskCanceledException e)
            {
                ex = e;
                //System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc Exception:  " + e.Message);
            }
            catch (Exception e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc General Exception:  " + e.Message);
            }
            try
            {
                dt.OnComplete?.Invoke(success, ex);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc.OnComplete Exception:  " + e.Message);
            }
        }
        /// <summary>
        /// Starts a new thread (via TaskCreationOptions.LongRunning)
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <returns></returns>
        public static Task Thread<T>(this T @this, Action<T> action, Action<bool, Exception> onComplete)
        {
            var cts = new CancellationTokenSource();
            var data = new ThreadData<T>() { Sender = @this, @Action = action, OnComplete = onComplete };
            return Task.Factory.StartNew(() => { ThreadProc<T>(data); }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
#else
        private static void ThreadProc<T>(object data)
        {
            var dt = data as ThreadData<T>;
            bool success = false;
            Exception ex = null;
            if (dt == null)
                return;
            try
            {
                dt.Action?.Invoke(dt.Sender);
                success = true;
            }
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (Exception e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc General Exception:  " + e.Message);
            }
            try
            {
                dt.OnComplete?.Invoke(success, ex);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc.OnComplete Exception:  " + e.Message);
            }
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <returns></returns>
        public static System.Threading.Thread Thread<T>(this T @this, Action<T> action, Action<bool, Exception> onComplete)
        {
            var thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(o => ThreadProc<T>(o)));
            var data = new ThreadData<T>() { Sender = @this, @Action = action, OnComplete = onComplete };
            thread.Start(data);
            return thread;
        }
#endif
    }
}
