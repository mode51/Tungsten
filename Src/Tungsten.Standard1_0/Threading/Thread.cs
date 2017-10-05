using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    public class Thread<TParameterType> : Thread
    {
        private new Action<TParameterType, CancellationToken> _action;
        private TParameterType _defaultArg;
        private TParameterType _arg;

        protected override void CallAction(CancellationToken token)
        {
            _action.Invoke(_arg, token);
            //base.CallAction(token);
        }

        public override void Start()
        {
            Start(_defaultArg);
        }
        public override void Start(int msLifetime)
        {
            Start(_defaultArg, msLifetime);
        }
        public void Start(TParameterType arg)
        {
            Start(arg, -1);
        }
        public void Start(TParameterType arg, int msLifetime)
        {
            _arg = arg;
            base.Start(msLifetime);
        }

        public Thread(Action<TParameterType, CancellationToken> action) : this(action, default(TParameterType), true)
        {
        }
        public Thread(Action<TParameterType, CancellationToken> action, bool autoStart) : this(action, default(TParameterType), autoStart)
        {
        }
        public Thread(Action<TParameterType, CancellationToken> action, TParameterType defaultArg) : this(action, defaultArg, true)
        {
            _action = action;
            _defaultArg = defaultArg;
        }
        public Thread(Action<TParameterType, CancellationToken> action, TParameterType defaultArg, bool autoStart) : base(null, autoStart)
        {
            _action = action;
            _defaultArg = defaultArg;
        }
    }

    public partial class Thread : IDisposable
    {
        private Action<System.Threading.CancellationToken> _action;
        private System.Threading.Tasks.Task _task;
        private System.Threading.ManualResetEventSlim _mreComplete = new System.Threading.ManualResetEventSlim(true);
        private System.Threading.ManualResetEventSlim _mreStarted = new System.Threading.ManualResetEventSlim(false);
        private System.Threading.CancellationTokenSource cts; //= new System.Threading.CancellationTokenSource();
        private object _startLock = new object();

        private void ThreadProc()
        {
            try
            {
                _mreComplete.Reset();
                _mreStarted.Set();
                CallAction(cts.Token);
            }
            catch (Exception e)
            {
                IsFaulted = true;
                Exception = e;
            }
            finally
            {
                _mreComplete.Set();
            }
        }

        protected virtual void CallAction(CancellationToken token)
        {
            _action.Invoke(token);
        }

        public bool IsStarted => _mreStarted.IsSet;
        public bool IsComplete => _mreComplete.IsSet;
        public bool IsFaulted { get; private set; }
        public Exception Exception { get; private set; }
        /// <summary>
        /// Not available until after Start has been called
        /// </summary>
        public CancellationToken Token { get { return cts.Token; } }
        public void Join()
        {
            _mreComplete.Wait();
        }
        public bool Join(int msTimeout)
        {
            return _mreComplete.Wait(msTimeout);
        }
        public virtual void Start()
        {
            Start(msLifetime: -1);
        }
        public virtual void Start(int msLifetime)
        {
            lock (_startLock)
            {
                if (IsStarted)
                    return;
                //    throw new InvalidOperationException("The thread has already been started.  It cannot be restarted");
                if (msLifetime >= 0)
                    cts = new System.Threading.CancellationTokenSource(msLifetime);
                else
                    cts = new System.Threading.CancellationTokenSource();
                _task = Task.Factory.StartNew(ThreadProc, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                _mreStarted.Wait();
            }
        }
        public void Stop()
        {
            //if (IsStarted)
            //{
                cts?.Cancel(); //cts will be null if the thread hasn't been started (say...autostart == false)
                _mreComplete.Wait();
            //}
        }
        public void Dispose()
        {
            Stop();
            cts.Dispose();
            _mreStarted.Dispose();
            _mreComplete.Dispose();
#if NET45
            _task.Dispose();
#endif
        }
        public Thread(Action<System.Threading.CancellationToken> action) : this(action, true)
        {
        }
        public Thread(Action<System.Threading.CancellationToken> action, bool autoStart)
        {
            _action = action;
            if (autoStart)
                Start();
        }
    }
    public partial class Thread //backwards compatability
    {
        [Obsolete("Call Stop or Dispose instead.")]
        public void Cancel()
        {
            cts.Cancel();
            _mreComplete.Wait();
        }
        [Obsolete("Use IsStarted instead")]
        public bool IsRunning => IsStarted && !IsComplete;
    }

    public partial class Thread //static and factory methods
    {
        /// <summary>
        /// Blocks the calling thread for the specified time
        /// </summary>
        /// <param name="msDelay">The number of milliseconds to block the thread</param>
        public static void Sleep(int msDelay)
        {
            Sleep(msDelay, false);
        }
        /// <summary>
        /// Blocks the calling thread for the specified time
        /// </summary>
        /// <param name="msDelay">The number of milliseconds to block the thread</param>
        /// <param name="useSpinWait">If True, a SpinWait.SpinUntil will be used instead of a call to Thread.Sleep (or Task.Delay).  Note that SpinWait should only be used on multi-core/cpu machines.</param>
        public static void Sleep(int msDelay, bool useSpinWait)
        {
#if NET45
            try
            {
                if (useSpinWait)
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, msDelay);
                else
                    System.Threading.Thread.Sleep(msDelay);
            }
            catch (System.MissingMethodException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
#else
            if (useSpinWait)
                System.Threading.SpinWait.SpinUntil(() => { return false; }, msDelay);
            else
                System.Threading.Tasks.Task.Delay(msDelay);
#endif
        }
        /// <summary>
        /// Attempts to free the CPU for other processes, based on the desired level.  Consequences will vary depending on your hardware architecture.  The more processors/cores you have, the better performance you will have by selecting LowCPU.  Likewise, on a single-core processor, you may wish to select HighCPU.
        /// </summary>
        /// <param name="level">The desired level of CPU usage</param>
        /// <remarks>Note results may vary.  LowCPU will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture.  This may not always be the case.</remarks>
        public static void Sleep(CPUProfileEnum level)
        {
            switch (level)
            {
#if NET45
                case CPUProfileEnum.Yield:
                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
                    break;
#endif
                case CPUProfileEnum.SpinWait0:
                    //#if NET45
                    //                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
                    //#else
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 0);
                    //#endif
                    break;
                case CPUProfileEnum.Sleep:
                    W.Threading.Thread.Sleep(1);
                    break;
                case CPUProfileEnum.SpinWait1:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
                    break;
            }
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <returns>A new Thread</returns>
        public static Thread Create(Action<CancellationToken> action)
        {
            var result = new Thread(action);
            return result;
        }
        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="autoStart">If True, the thread immediately starts</param>
        /// <returns>A new Thread</returns>
        public static Thread Create(Action<CancellationToken> action, bool autoStart)
        {
            var result = new Thread(action, autoStart);
            return result;
        }
        /// <summary>
        /// Creates and starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <returns>A new Thread</returns>
        public static Thread<TParameterType> Create<TParameterType>(Action<TParameterType, CancellationToken> action)
        {
            var result = new Thread<TParameterType>(action, true);
            return result;
        }
        /// <summary>
        /// Creates a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="autoStart">If True, the thread is immediately started</param>
        /// <returns>A new Thread</returns>
        public static Thread<TParameterType> Create<TParameterType>(Action<TParameterType, CancellationToken> action, bool autoStart)
        {
            var result = new Thread<TParameterType>(action, autoStart);
            return result;
        }
    }
}


//using System;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;

//namespace W.Threading
//{
//    /// <summary>
//    /// A thread wrapper which makes multi-threading easier
//    /// </summary>
//    public class Thread : ThreadBase
//    {
//#if NET45
//        private System.Threading.Thread _thread = null;
//#else
//        private Task _task;
//#endif

//#if NET45
//        /// <summary>
//        /// Aborts the thread.  This will raise a System.Threading.ThreadAbortException in the thread procedure.
//        /// </summary>
//        public void Abort()
//        {
//            _thread?.Abort();
//        }
//#endif

//        /// <summary>
//        /// Blocks the calling thread until the thread terminates
//        /// </summary>
//        public override void Join()
//        {
//#if NET45
//            _thread?.Join();
//#else
//            try
//            {
//                _task.Wait();
//            }
//            catch (AggregateException)
//            {
//                //the task was cancelled
//            }
//#endif
//        }

//        /// <summary>
//        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
//        /// </summary>
//        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
//        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
//        public override bool Join(int msTimeout)
//        {
//#if NET45
//            return _thread?.Join(msTimeout) ?? true;
//#else
//            try
//            {
//                return _task.Wait(msTimeout);
//            }
//            catch (AggregateException)
//            {
//                //the task was cancelled
//            }
//            return true; //true because the thread/task actually did end
//#endif
//        }

//        /// <summary>
//        /// Starts a new thread
//        /// </summary>
//        /// <param name="action">Action to call on a thread</param>
//        /// <param name="onExit">Action to call upon comletion.  Executes on the same thread as Action.</param>
//        /// <param name="cts">A CancellationTokenSource to use instead of the default</param>
//        /// <returns>A new Thread</returns>
//        public static Thread Create(Action<CancellationToken> action, Action<bool, Exception> onExit = null, CancellationTokenSource cts = null)
//        {
//            var result = new Thread(action, onExit, cts);
//            return result;
//        }

//        /// <summary>
//        /// Blocks the calling thread for the specified time
//        /// </summary>
//        /// <param name="msDelay">The number of milliseconds to block the thread</param>
//        public static void Sleep(int msDelay)
//        {
//            Sleep(msDelay, false);
//        }
//        /// <summary>
//        /// Blocks the calling thread for the specified time
//        /// </summary>
//        /// <param name="msDelay">The number of milliseconds to block the thread</param>
//        /// <param name="useSpinWait">If True, a SpinWait.SpinUntil will be used instead of a call to Thread.Sleep (or Task.Delay).  Note that SpinWait should only be used on multi-core/cpu machines.</param>
//        public static void Sleep(int msDelay, bool useSpinWait)
//        {
//#if NET45
//            try
//            {
//                if (useSpinWait)
//                    System.Threading.SpinWait.SpinUntil(() => { return false; }, msDelay);
//                else
//                    System.Threading.Thread.Sleep(msDelay);
//            }
//            catch (System.MissingMethodException e)
//            {
//                System.Diagnostics.Debug.WriteLine(e.ToString());
//            }
//#else
//            if (useSpinWait)
//                System.Threading.SpinWait.SpinUntil(() => { return false; }, msDelay);
//            else
//                System.Threading.Tasks.Task.Delay(msDelay);
//#endif
//        }
//        /// <summary>
//        /// Attempts to free the CPU for other processes, based on the desired level.  Consequences will vary depending on your hardware architecture.  The more processors/cores you have, the better performance you will have by selecting LowCPU.  Likewise, on a single-core processor, you may wish to select HighCPU.
//        /// </summary>
//        /// <param name="level">The desired level of CPU usage</param>
//        /// <remarks>Note results may vary.  LowCPU will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture.  This may not always be the case.</remarks>
//        public static void Sleep(CPUProfileEnum level)
//        {
//            switch (level)
//            {
//#if NET45
//                case CPUProfileEnum.Yield:
//                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
//                    break;
//#endif
//                case CPUProfileEnum.SpinWait0:
//                    //#if NET45
//                    //                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
//                    //#else
//                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 0);
//                    //#endif
//                    break;
//                case CPUProfileEnum.Sleep:
//                    W.Threading.Thread.Sleep(1);
//                    break;
//                case CPUProfileEnum.SpinWait1:
//                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
//                    break;
//            }
//        }

//        /// <summary>
//        /// Starts the thread if it's not already running
//        /// </summary>
//        public void Start()
//        {
//            if (!IsRunning)
//            {
//#if NET45
//                _thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadProc));
//                _thread.Start();
//#else
//                _task = Task.Factory.StartNew(ThreadProc, Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
//#endif
//            }
//        }
//        /// <summary>
//        /// Disposes the Thread and releases resources
//        /// </summary>
//        public override void Dispose()
//        {
//#if NET45
//            Cancel();
//#endif
//            base.Dispose();
//        }
//        /// <summary>
//        /// Constructs the Thread object
//        /// </summary>
//        /// <param name="action">The Action to be called on a new thread</param>
//        public Thread(Action<CancellationToken> action) : base(action, null, null)
//        {
//            Start();
//        }
//        /// <summary>
//        /// Constructs the Thread object
//        /// </summary>
//        /// <param name="action">The Action to be called on a new thread</param>
//        /// <param name="onExit">The Action to be called when the thread completes</param>
//        public Thread(Action<CancellationToken> action, Action<bool, Exception> onExit) : base(action, onExit, null)
//        {
//            Start();
//        }
//        /// <summary>
//        /// Constructs the Thread object
//        /// </summary>
//        /// <param name="action">The Action to be called on a new thread</param>
//        /// <param name="onExit">The Action to be called when the thread completes</param>
//        /// <param name="cts">A CancellationTokenSource to use instead of the default</param>
//        public Thread(Action<CancellationToken> action, Action<bool, Exception> onExit, CancellationTokenSource cts) : base(action, onExit, cts)
//        {
//            Start();
//        }
//        /// <summary>
//        /// Constructs the Thread object
//        /// </summary>
//        /// <param name="action">The Action to be called on a new thread</param>
//        /// <param name="onExit">The Action to be called when the thread completes</param>
//        /// <param name="cts">A CancellationTokenSource to use instead of the default</param>
//        /// <param name="autoStart">If True, the thread will automatically start, otherwise a call to Start is required.</param>
//        public Thread(Action<CancellationToken> action, Action<bool, Exception> onExit, CancellationTokenSource cts, bool autoStart) : base(action, onExit, cts)
//        {
//            if (autoStart)
//                Start();
//        }
//    }
//}
