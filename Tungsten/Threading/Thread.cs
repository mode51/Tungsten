using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
#if WINDOWS_PORTABLE || WINDOWS_UWP
    public class Thread : ThreadBase
    {
        /// <summary>
        /// The Task/Thread which was created
        /// </summary>
        public Task Task { get; private set; }

        protected override Exception CallInvokeAction()
        {
            Exception ex = null;
            try
            {
                CallInvokeAction();
                Success.Value = true;
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
            return ex;
        }

        public override void Join()
        {
            Task.Wait();
        }
        public override bool Join(int msTimeout)
        {
            return Task.Wait(msTimeout);
        }

        public Thread(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null) : base(action, onComplete)
        {
            Task = Task.Factory.StartNew(() => { ThreadProc(); }, Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <returns></returns>
        public static Thread Create(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null)
        {
            var result = new Thread(action, onComplete);
            return result;
        }
    }

    public class Thread<T> : Thread
    {
        protected T CustomData { get; set; }
        protected new Action<T, CancellationTokenSource> Action { get; set; }

        protected override void InvokeAction()
        {
            Action?.Invoke(CustomData, Cts);
        }

        public Thread(Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete = null, T customData = default(T)) : base(null, onComplete)
        {
            Action = action;
            CustomData = customData;
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <returns></returns>
        public static Thread<T> Create<T>(Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete = null, T customData = default(T))
        {
            var result = new Thread<T>(action, onComplete, customData);
            return result;
        }
    }
    //public class Thread<T>
    //{
    //    private class ThreadData<T>
    //    {
    //        public T CustomData { get; set; }
    //        public Action<T, CancellationTokenSource> Action { get; set; }
    //        public Action<bool, Exception> OnComplete { get; set; }
    //        public System.Threading.CancellationTokenSource Cts { get; set; }
    //        public Lockable<bool> IsRunning { get; set; }
    //    }

    //    private System.Threading.CancellationTokenSource _cts;
    //    private Lockable<bool> _isRunning = new Lockable<bool>();

    //    private static void ThreadProc(object data)
    //    {
    //        var dt = data as ThreadData<T>;
    //        bool success = false;
    //        Exception ex = null;
    //        if (dt == null)
    //            return;
    //        try
    //        {
    //            dt.Action?.Invoke(dt.CustomData, dt.Cts);
    //            success = true;
    //        }
    //        catch (TaskSchedulerException e)
    //        {
    //            ex = e;
    //            System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
    //        }
    //        catch (TaskCanceledException e)
    //        {
    //            ex = e;
    //            //System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc Exception:  " + e.Message);
    //        }
    //        catch (Exception e)
    //        {
    //            ex = e;
    //            System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc General Exception:  " + e.Message);
    //        }

    //        try
    //        {
    //            dt.IsRunning.Value = false;
    //            dt.OnComplete?.Invoke(success, ex);
    //        }
    //        catch (Exception e)
    //        {
    //            System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc.OnComplete Exception:  " + e.Message);
    //        }
    //    }

    //    /// <summary>
    //    /// <para>
    //    /// Cancels the thread by calling Cancel on the CancellationTokenSource.  The value should be checked in the code in the specified Action parameter.
    //    /// </para>
    //    /// </summary>
    //    public void Cancel()
    //    {
    //        _cts.Cancel();
    //    }
    //    /// <summary>
    //    /// True if the thread is running, otherwise false
    //    /// </summary>
    //    public bool IsRunning => _isRunning.Value;
    //    /// <summary>
    //    /// The Task/Thread which was created
    //    /// </summary>
    //    public Task Task { get; private set; }

    //    /// <summary>
    //    /// Create a new thread (via TaskCreationOptions.LongRunning)
    //    /// </summary>
    //    /// <param name="customData">Data to be passed to the thread</param>
    //    /// <param name="action">Action to call on a thread</param>
    //    /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
    //    /// <returns></returns>
    //    public Thread(Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete = null, T customData = default(T))
    //    {
    //        _cts = new CancellationTokenSource();
    //        var data = new ThreadData<T>() { CustomData = customData, @Action = action, OnComplete = onComplete, Cts = _cts, IsRunning = _isRunning};
    //        _isRunning.Value = true;
    //        Task = Task.Factory.StartNew(() => { ThreadProc(data); }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    //    }

    //    /// <summary>
    //    /// Starts a new thread (via TaskCreationOptions.LongRunning)
    //    /// </summary>
    //    /// <param name="action">Action to call on a thread</param>
    //    /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
    //    /// <returns></returns>
    //    public static Thread<T> Create(Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete = null, T customData = default(T))
    //    {
    //        var result = new Thread<T>(action, onComplete, customData);
    //        return result;
    //    }
    //}
#else
    /// <summary>
    /// A thread wrapper which makes multi-threading easier
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Thread : ThreadBase
    {
        private readonly System.Threading.Thread _thread = null;

        /// <summary>
        /// Wraps the call to InvokeAction with try/catch block to catch exceptions
        /// </summary>
        /// <returns></returns>
        protected override Exception CallInvokeAction()
        {
            Exception ex = null;
            try
            {
                InvokeAction();
                Success.Value = true;
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
            return ex;
        }

        /// <summary>
        /// <para>
        /// Cancels the thread by calling Cancel on the CancellationTokenSource.  The value should be checked in the code in the specified Action parameter.
        /// </para>
        /// </summary>
        /// <param name="msForceAbortDelay">Abort the thread if it doesn't terminate before the specified number of milliseconds elapse</param>
        public override void Cancel()
        {
            base.Cancel();

            if (!Join(5000)) //give the thread 5 seconds to close down, otherwise force it
            {
                _thread?.Abort();
            }
        }
        /// <summary>
        /// <para>
        /// Cancels the thread by calling Cancel on the CancellationTokenSource.  The value should be checked in the code in the specified Action parameter.
        /// </para>
        /// </summary>
        /// <param name="msForceAbortDelay">Abort the thread if it doesn't terminate before the specified number of milliseconds elapse</param>
        public void Cancel(int msForceAbortDelay)
        {
            base.Cancel();

            if (!Join(msForceAbortDelay)) //give the thread 5 seconds to close down, otherwise force it
            {
                _thread?.Abort();
            }
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates
        /// </summary>
        public override void Join()
        {
            _thread?.Join();
        }
        /// <summary>
        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
        public override bool Join(int msTimeout)
        {
            return _thread?.Join(msTimeout) ?? true;
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <returns></returns>
        public Thread(Action<System.Threading.CancellationTokenSource> action, Action<bool, Exception> onComplete = null) : base(action, onComplete)
        {
            _thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadProc));
            _thread.Start();
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose()
        {
            Cancel(3000);
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <returns></returns>
        public static Thread Create(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null)
        {
            var result = new Thread(action, onComplete);
            return result;
        }

        ///// <summary>
        ///// Starts a new thread
        ///// </summary>
        ///// <param name="action">Action to call on a thread</param>
        ///// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        ///// <param name="customData">The custom data to pass to the thread (Action)</param>
        ///// <returns></returns>
        //public static W.Threading.Thread Create(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null)
        //{
        //    var thread = new W.Threading.Thread(action, onComplete);
        //    return thread;
        //}
    }

    /// <summary>
    /// A thread wrapper which makes multi-threading easier
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Thread<T> : Thread
    {
        public Lockable<T> CustomData { get; }
        protected new Action<T, CancellationTokenSource> Action { get; set; }

        /// <summary>
        /// Overridden implementation which calls Action with CustomData
        /// </summary>
        protected override void InvokeAction()
        {
            Action?.Invoke(CustomData.Value, Cts);
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="customData">The data to pass to the call to the thread (Action)</param>
        /// <returns></returns>
        public Thread(Action<T, System.Threading.CancellationTokenSource> action, Action<bool, Exception> onComplete = null, T customData = default(T)) : base(null, onComplete)
        {
            Action = action;
            CustomData = new Lockable<T>(customData);
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="customData">The custom data to pass to the thread (Action)</param>
        /// <returns></returns>
        public static W.Threading.Thread<T> Create(Action<T, CancellationTokenSource> action, Action<bool, Exception> onComplete = null, T customData = default(T))
        {
            var thread = new W.Threading.Thread<T>(action, onComplete, customData);
            return thread;
        }
    }
#endif

}
