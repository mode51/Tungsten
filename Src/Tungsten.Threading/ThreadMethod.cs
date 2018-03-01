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
        private ManualResetEventSlim _mreIsComplete = new ManualResetEventSlim(false);
        private volatile bool _isStarted = false;
        private object[] _args = null;
        //private volatile bool _isDisposed = false;
        //private Lockers.MonitorLocker _locker = new Lockers.MonitorLocker();

        private void RunTheMethod()
        {
            try
            {
                _isStarted = true;
                _action.Invoke(_ctsCancel.Token, _args);
            }
            finally
            {
                _mreIsComplete.Set();
                _isStarted = false;
            }
        }
        private void Initialize()
        {
            _mreIsComplete.Reset();
            _ctsForceCancel = new CancellationTokenSource();
            _ctsCancel = new CancellationTokenSource();
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
        public bool IsStarted => _isStarted;
        /// <summary>
        /// True if the thread is currently running and not complete, otherwise False
        /// </summary>
        public bool IsRunning // => _isStarted && !_mreIsComplete.IsSet;
        {
            get
            {
                try { return _isStarted && !_mreIsComplete.IsSet; }
                catch { return false; }
            }
        }
        /// <summary>
        /// True if the thread has completed, otherwise False
        /// </summary>
        public bool IsComplete
        {
            get
            {
                try { return _mreIsComplete.IsSet; }
                catch { return true; }
            }
        }

        /// <summary>
        /// Starts the thread
        /// </summary>
        public void Start() { Start(null); }
        /// <summary>
        /// Starts the thread
        /// </summary>
        /// <param name="args">The arguments to pass into the thread procedure</param>
        public void Start(params object[] args) { lock (_runLock) { _args = args; if (_isStarted) return; Initialize(); _runTask.Start(); } }
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
            if (!_mreIsComplete.Wait(1000))
                _ctsForceCancel?.Cancel();
            //if (!_mreIsComplete.Wait(1000))
            //    Log.w("ThreadMethod.Dispose: Unable to stop the thread");
            _ctsCancel?.Dispose();
            _mreIsComplete.Dispose();
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
