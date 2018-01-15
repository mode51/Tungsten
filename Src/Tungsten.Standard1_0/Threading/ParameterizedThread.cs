using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// The delegate, with variable arguments, which is called on a separate thread
    /// </summary>
    /// <param name="token">The CancellationToken used to cancel the thread</param>
    /// <param name="args">The arguments to pass into the thread procedure</param>
    public delegate void GenericThreadDelegate(CancellationToken token, params object[] args);

    /// <summary>
    /// A thread class which supports a variable number of arguments
    /// </summary>
    public partial class ParameterizedThread : Disposable
    {
        private System.Threading.ManualResetEventSlim _mreComplete = new System.Threading.ManualResetEventSlim(false);
        private System.Threading.ManualResetEventSlim _mreStarted = new System.Threading.ManualResetEventSlim(false);
        private GenericThreadDelegate _threadDelegate { get; set; }
        private System.Threading.CancellationTokenSource Cts; //= new System.Threading.CancellationTokenSource();
        private LockableSlim<bool> _startCalled = new LockableSlim<bool>();

        /// <summary>
        /// The underlying Task associated with this ParameterizedThread
        /// </summary>
        public System.Threading.Tasks.Task Task { get; private set; }

        ///// <summary>
        ///// Calls the action encapsulated by this thread.  This method can be overridden to provide more specific functionality.
        ///// </summary>
        ///// <param name="token">The CancellationToken, passed into the action, which can be used to cancel the thread Action</param>
        //protected abstract void CallAction(CancellationToken token, params object[] args);

        /// <summary>
        /// Dispose the thread and free resources
        /// </summary>
        protected override void OnDispose()
        {
            Stop();
            Cts.Dispose();
            _mreStarted.Dispose();
            _mreComplete.Dispose();
            //_startCalled.Dispose();
#if NET45
            Task.Dispose();
#endif
        }

        /// <summary>
        /// True if the thread has been started, otherwise False
        /// </summary>
        public bool IsStarted => _mreStarted.IsSet;
        /// <summary>
        /// True if the thread has completed, otherwise False
        /// </summary>
        /// <remarks>A True value does not indicate that the thread completed successfully</remarks>
        public bool IsComplete => _mreComplete.IsSet;
        /// <summary>
        /// True if the thread raised an exception, otherwise False
        /// </summary>
        public bool IsFaulted { get; private set; }
        /// <summary>
        /// The exception, if one was caught
        /// </summary>
        public Exception Exception { get; private set; }
        /// <summary>
        /// Not available until after Start has been called
        /// </summary>
        public CancellationToken Token { get { return Cts.Token; } }
        /// <summary>
        /// Block the calling thread until this thread object has completed
        /// </summary>
        public void Join()
        {
            _mreComplete.Wait();
        }
        /// <summary>
        /// Block the calling thread until this thread object has completed or until the timeout has occurred
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait, for the thread Action to complete, before timing out</param>
        /// <returns>True if the thread Action completed within the specified timeout, otherwise False</returns>
        public bool Join(int msTimeout)
        {
            return _mreComplete.Wait(msTimeout);
        }
        /// <summary>
        /// Start the thread
        /// </summary>
        public Task Start(params object[] args)
        {
            return Start(-1, args);
        }
        /// <summary>
        /// Start the thread with a CancellationToken which will timeout in the specified milliseconds
        /// </summary>
        /// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
        /// <param name="args">The arguments to pass into the thread procedure</param>
        public Task Start(int msLifetime, params object[] args)
        {
            if (!_startCalled.Value)
            {
                _startCalled.Value = true;
                if (msLifetime >= 0)
                    Cts = new System.Threading.CancellationTokenSource(msLifetime);
                else
                    Cts = new System.Threading.CancellationTokenSource();
                Task = Task.Factory.StartNew(() =>
                {
                    _mreComplete.Reset();
                    _mreStarted.Set();
                    _threadDelegate?.Invoke(Cts.Token, args);
                }, Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ContinueWith(task =>
                {
                    IsFaulted = task.IsFaulted;
                    Exception = task.Exception;
                    _mreComplete.Set();
                });
                _mreStarted.Wait();
            }
            else
                System.Diagnostics.Debugger.Break();//we shouldn't be re-entering this method
            return Task;
        }
        /// <summary>
        /// Stop the thread.  This calls Cancel on the CancellationToken
        /// </summary>
        public void Stop()
        {
            //if (IsStarted)
            //{
            _startCalled.Value = false;
            Cts?.Cancel(); //cts will be null if the thread hasn't been started (say...autostart == false)
            if (!_mreStarted.IsSet)
                _mreComplete.Set();
            _mreComplete.Wait();
            _mreStarted.Reset();
            //}
        }
        /// <summary>
        /// Constructs a new ParameterizedThread
        /// </summary>
        /// <param name="action">The action to call on a thread</param>
        public ParameterizedThread(GenericThreadDelegate action)
        {
            _threadDelegate = action;
        }
    }
    public partial class ParameterizedThread //static methods
    {
        /// <summary>
        /// Starts a new ParameterizedThread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <returns>A new Thread</returns>
        public static ParameterizedThread Create(GenericThreadDelegate action)
        {
            var result = new ParameterizedThread(action);
            return result;
        }
    }
}
