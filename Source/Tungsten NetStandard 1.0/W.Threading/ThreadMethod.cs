using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// Task-based multi-threading
    /// </summary>
    public partial class ThreadMethod : IDisposable
    {
        private Task _runTask;
        private CancellationTokenSource _ctsForceCancel = null;
        private CancellationTokenSource _ctsCancel = null;
        private ThreadMethodDelegate _action = null;
        private object _runLock = new object();
        private volatile bool _isStarted = false;
        private volatile bool _isComplete = false;
        private object _stateChangeLock = new object();
        private object[] _args = null;
        //private volatile bool _isDisposed = false;
        //private Lockers.MonitorLocker _locker = new Lockers.MonitorLocker();

        private void RunTheMethod()
        {
            try
            {
                if (_action != null)
                    _action(_ctsCancel.Token, _args);
            }
            finally
            {
                lock (_stateChangeLock)
                {
                    IsStarted = false;
                    IsComplete = true;
                }
            }
        }
        private void Initialize()
        {
            _ctsForceCancel = new CancellationTokenSource();
            _ctsCancel = new CancellationTokenSource();
//#if NET45
//            _runTask?.Dispose();
//#endif
            _runTask = new Task(RunTheMethod, _ctsForceCancel.Token, TaskCreationOptions.LongRunning);
        }
    }
    public partial class ThreadMethod : IDisposable
    {
        /// <summary>
        /// Delegate type used by ThreadMethod
        /// </summary>
        /// <param name="token">A CancellationToken which can be used to signal the threaded method to stop</param>
        /// <param name="args">Zero or more arguments to pass into the thread method</param>
        public delegate void ThreadMethodDelegate(CancellationToken token, params object[] args);

        /// <summary>
        /// A user-defined name for this object
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// True if the thread  is running or has completed
        /// </summary>
        private bool IsStarted { get { lock (_stateChangeLock) return _isStarted; } set { lock (_stateChangeLock) _isStarted = value; } }
        /// <summary>
        /// True if the thread is currently running and not complete, otherwise False
        /// </summary>
        private bool IsRunning { get { lock (_stateChangeLock) return _isStarted && !_isComplete; } }
        /// <summary>
        /// True if the thread has completed, otherwise False
        /// </summary>
        public bool IsComplete { get { lock (_stateChangeLock) return _isComplete; } private set { lock (_stateChangeLock) _isComplete = value; } }

        /// <summary>
        /// Waits a specified number of milliseconds for the thread to complete
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to complete.  A value of -1 indicates an infinite wait period.</param>
        /// <returns>True if the thread completes within the timeout period, otherwise False</returns>
        public bool Wait(int msTimeout = -1)
        {
            return System.Threading.SpinWait.SpinUntil(() => IsComplete == true, msTimeout);
            //if (msTimeout == -1)
            //{
            //    while(!_isComplete)
            //        W.Threading.Thread.Sleep(1);
            //}
            //else
            //{
            //    var sw = System.Diagnostics.Stopwatch.StartNew();
            //    while (true)
            //    {
            //        sw.Stop();
            //        if (_isComplete)
            //            break;
            //        if (sw.ElapsedMilliseconds > msTimeout)
            //            return false;
            //        sw.Start();
            //        W.Threading.Thread.Sleep(1);
            //    }
            //}
            //return _isComplete;
        }
        /// <summary>
        /// Starts the thread
        /// </summary>
        public void Start() { Start(null); }
        /// <summary>
        /// Starts the thread
        /// </summary>
        /// <param name="args">The arguments to pass into the thread procedure</param>
        public void Start(params object[] args)
        {
            lock (_runLock)
            {
                if (IsRunning)
                    return;
                _runTask?.Wait(_ctsCancel.Token);
                _args = args;
                Initialize();

                IsComplete = false;
                _runTask.Start();
                IsStarted = true;
            }
        }
        /// <summary>
        /// Signals the thread method via the CancellationToken to stop running and waits for it to complete
        /// </summary>
        public void Cancel() { _ctsCancel?.Cancel(); /*Wait();*/ }
        /// <summary>
        /// Disposes the ThreadMethod and releases resources
        /// </summary>
        public void Dispose()
        {
            _ctsCancel?.Cancel();
            if (!System.Threading.SpinWait.SpinUntil(() => IsComplete == true, 1000))
                _ctsForceCancel?.Cancel();
            _ctsCancel?.Dispose();
        }
        /// <summary>
        /// Constructs a new ThreadMethod
        /// </summary>
        /// <param name="threadProc">The thread proc</param>
        public ThreadMethod(ThreadMethodDelegate threadProc)
        {
            _action = threadProc;
        }
        /// <summary>
        /// Constructs a new ThreadMethod
        /// </summary>
        /// <param name="threadProc">The thread proc</param>
        public ThreadMethod(Action<CancellationToken> threadProc)
        {
            _action = new ThreadMethodDelegate((token, args) => threadProc.Invoke(token));
        }
        /// <summary>
        /// Constructs a new ThreadMethod
        /// </summary>
        /// <param name="threadProc">The thread proc</param>
        public static ThreadMethod Create(ThreadMethodDelegate threadProc)
        {
            return new ThreadMethod(threadProc);
        }
        /// <summary>
        /// Constructs a new ThreadMethod
        /// </summary>
        /// <param name="threadProc">The thread proc</param>
        public static ThreadMethod Create(Action<CancellationToken> threadProc)
        {
            return new ThreadMethod(threadProc);
        }
    }
}
