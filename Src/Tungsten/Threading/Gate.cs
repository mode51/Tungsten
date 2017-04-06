using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// <para>
    /// A Gated thread.  Execution of the Action will proceed when the Run method is called.
    /// </para>
    /// </summary>
    public class Gate : W.Threading.Thread
    {
        private AutoResetEvent Event { get; } = new AutoResetEvent(false);

        /// <summary>
        /// <para>
        /// Used to wrap the call to InvokeAction with try/catch handlers.  This method should call InvokeAction.
        /// </para>
        /// </summary>
        /// <returns>An Exception if on occurs, otherwise null</returns>
        protected override Exception CallInvokeAction()
        {
            Exception ex = null;
            try
            {
                while (!Cts?.Token.IsCancellationRequested ?? false)
                {
                    if (Event?.WaitOne(1000) ?? false) //just release the thread periodically
                    {
                        InvokeAction();
                    }
                }
            }
#if WINDOWS_UWP || WINDOWS_PORTABLE
            catch (TaskSchedulerException e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
            }
            catch (TaskCanceledException e)
            {
                ex = e;
            }
#else
            catch (System.Threading.ThreadAbortException e)
            {
                ex = e;
                System.Threading.Thread.ResetAbort();
            }
#endif
            catch (Exception e)
            {
                ex = e;
            }
            return ex;
        }
        /// <summary>
        /// Allows the Action to be called
        /// </summary>
        public void Run()
        {
            Event.Set();
        }

        /// <summary>
        /// Construct a Gate
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onComplete"></param>
        public Gate(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null) : base(action, onComplete)
        {
        }
    }

    /// <summary>
    /// <para>
    /// A Gated thread.  Execution of the Action will proceed when the Run method is called.
    /// </para>
    /// </summary>
    public class Gate<T> : W.Threading.Thread<T>
    {
        private AutoResetEvent Event { get; } = new AutoResetEvent(false);

        /// <summary>
        /// <para>
        /// Used to wrap the call to InvokeAction with try/catch handlers.  This method should call InvokeAction.
        /// </para>
        /// </summary>
        /// <returns>An Exception if on occurs, otherwise null</returns>
        protected override Exception CallInvokeAction()
        {
            Exception ex = null;
            try
            {
                while (!Cts?.Token.IsCancellationRequested ?? false)
                {
                    if (Event?.WaitOne(1000) ?? false) //just release the thread periodically
                    {
                        InvokeAction();
                    }
                }
            }
#if WINDOWS_UWP || WINDOWS_PORTABLE
            catch (TaskSchedulerException e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
            }
            catch (TaskCanceledException e)
            {
                ex = e;
            }
#else
            catch (System.Threading.ThreadAbortException e)
            {
                ex = e;
                System.Threading.Thread.ResetAbort();
            }
#endif
            catch (Exception e)
            {
                ex = e;
            }
            return ex;
        }
        /// <summary>
        /// Allows the Action to be called
        /// </summary>
        public void Run()
        {
            Event.Set();
        }

        /// <summary>
        /// Construct a Gate
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onComplete"></param>
        /// <param name="cts">Can be used to control cancelation externally</param>
        /// <param name="args">Arguments to pass into the underlying thread</param>
        public Gate(Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete = null, CancellationTokenSource cts = null, T args = default(T)) : base(action, onComplete, cts, args)
        {
        }
    }
}