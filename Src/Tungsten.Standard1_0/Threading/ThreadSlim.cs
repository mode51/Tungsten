using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// Thread delegate used by ThreadSlim
    /// </summary>
    /// <param name="token">A CancellationToken which can be used to signal the threaded method to stop</param>
    /// <param name="args">Arguments to pass into the thread method</param>
    public delegate void ThreadDelegate(CancellationToken token, params object[] args);

    /// <summary>
    /// A lighter thread class than W.Threading.Thread
    /// </summary>
    public partial class ThreadSlim : IDisposable
    {
        private object _disposeLock = new object();
        private ThreadMethod _method;
        private CancellationTokenSource _ctsCancelThread;

        /// <summary>
        /// A user-defined name for this object
        /// </summary>
        public string Name { get { return _method.Name; } set { _method.Name = value; } }

        /// <summary>
        /// True if the thread  is running or has completed
        /// </summary>
        public bool IsStarted => _method.IsStarted;
        /// <summary>
        /// True if the thread is currently running and not complete, otherwise False
        /// </summary>
        public bool IsRunning => _method.IsRunning;
        /// <summary>
        /// True if the thread has completed, otherwise False
        /// </summary>
        public bool IsComplete => _method.IsComplete;

        /// <summary>
        /// Signals the thread method to stop running
        /// </summary>
        public void SignalToStop()
        {
            if (IsRunning && !IsComplete)
                _ctsCancelThread?.Cancel();
        }
        /// <summary>
        /// Signals the thread method to stop running and waits for it to complete
        /// </summary>
        public void Stop()
        {
            if (IsRunning && !IsComplete)
            {
                _ctsCancelThread?.Cancel();
                Wait();
            }
        }
        /// <summary>
        /// Signals the thread method to stop running and waits the specified number of milliseconds for it to complete before returning
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread method to comlete</param>
        /// <returns>True if the thread method completes within the specified number of milliseconds, otherwise False</returns>
        public bool Stop(int msTimeout)
        {
            if (IsRunning && !IsComplete)
            {
                _ctsCancelThread?.Cancel();
                return Wait(msTimeout);
            }
            return true;
        }
        /// <summary>
        /// Signals the thread method to stop running and waits for the method to complete, while observing the specified CancellationToken
        /// </summary>
        /// <param name="token">The CancellationToken to observe while waiting</param>
        /// <returns>True if the thread method completes before the CancellationToken cancels, otherwise False</returns>
        public void Stop(CancellationToken token)
        {
            if (IsRunning && !IsComplete)
            {
                _ctsCancelThread?.Cancel();
                Wait(token);
            }
        }

        /// <summary>
        /// Starts the thread and waits for it to complete
        /// </summary>
        /// <param name="args">The arguments to pass into the thread procedure</param>
        public async void Start(params object[] args)
        {
            await StartAsync(args).ConfigureAwait(false);
        }
        /// <summary>
        /// Starts the thread and returns the Task associated with it 
        /// </summary>
        /// <param name="args">The arguments to pass into the thread procedure</param>
        /// <returns></returns>
        public async Task StartAsync(params object[] args)
        {
            if (IsRunning)
                throw new InvalidOperationException("The thread is already running");
            _ctsCancelThread?.Dispose();
            _ctsCancelThread = new CancellationTokenSource();
            await _method.StartAsync(args).ConfigureAwait(false);
        }
        /// <summary>
        /// Wait for the thread to complete
        /// </summary>
        public void Wait()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            _method.Wait();
#if NETSTANDARD1_0 || WINDOWS_UWP || WINDOWS_PORTABLE
            System.Diagnostics.Debug.WriteLine("ThreadSlim.Wait: Elapsed = {0}ms", sw.ElapsedMilliseconds);
#else
            Console.WriteLine("ThreadSlim.Wait: Elapsed = {0}ms", sw.ElapsedMilliseconds);
#endif
        }
        /// <summary>
        /// Wait for the thread to complete
        /// </summary>
        /// <param name="msTimeout">The amount of milliseconds to wait for the thread to complete</param>
        /// <returns>True if the thread completes within the specified timeout period, otherwise False</returns>
        public bool Wait(int msTimeout)
        {
            return _method.Wait(msTimeout);
        }
        /// <summary>
        /// Wait for the thread to complete
        /// </summary>
        /// <param name="token">A CancellationToken to observe while waiting</param>
        /// <returns>True if the thread completed before the CancellationToken was canceled, otherwise FAlse</returns>
        public bool Wait(CancellationToken token)
        {
            return _method.Wait(token);
        }

        public void Dispose()
        {
            lock (_disposeLock)
            {
                if (_method == null)
                    return;
                Stop();
                _method.Dispose();
            }
        }
        /// <summary>
        /// Constructs a new ThreadSlim object
        /// </summary>
        /// <param name="delegate">The ThreadDelegate to run on a separate thread</param>
        public ThreadSlim(W.Threading.ThreadDelegate @delegate)
        {
            _method = new ThreadMethod(args =>
            {
                @delegate.Invoke(_ctsCancelThread.Token, args);
            });
        }
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <param name="action">The action to run on a separate thread</param>
        public ThreadSlim(Action<CancellationToken> action)
        {
            _method = new ThreadMethod(args =>
            {
                action.Invoke(_ctsCancelThread.Token);
            });
        }
    }

    public partial class ThreadSlim //static members
    {
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <param name="delegate">The delegate to run on a seprate thread</param>
        /// <returns></returns>
        public static ThreadSlim Create(W.Threading.ThreadDelegate @delegate)
        {
            return new ThreadSlim(@delegate);
        }
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <param name="action">The action to run on a separate thread</param>
        public static ThreadSlim Create(Action<CancellationToken> action)
        {
            return new ThreadSlim(action);
        }
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <typeparam name="TArg">The Type of the argument to be passed into the thread method</typeparam>
        /// <param name="action">The action to run on a separate thread</param>
        public static ThreadSlim Create<TArg>(Action<CancellationToken, TArg> action)
        {
            return new ThreadSlim((cts, args) =>
            {
                action.Invoke((CancellationToken)args[0], (TArg)args[1]);
            });
        }
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <typeparam name="TArg1">The Type of the first argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg2">The Type of the second argument to be passed into the thread method</typeparam>
        /// <param name="action">The action to run on a separate thread</param>
        public static ThreadSlim Create<TArg1, TArg2>(Action<CancellationToken, TArg1, TArg2> action)
        {
            return new ThreadSlim((cts, args) =>
            {
                action.Invoke((CancellationToken)args[0], (TArg1)args[1], (TArg2)args[2]);
            });
        }
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <typeparam name="TArg1">The Type of the first argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg2">The Type of the second argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg3">The Type of the third argument to be passed into the thread method</typeparam>
        /// <param name="action">The action to run on a separate thread</param>
        public static ThreadSlim Create<TArg1, TArg2, TArg3>(Action<CancellationToken, TArg1, TArg2, TArg3> action)
        {
            return new ThreadSlim((cts, args) =>
            {
                action.Invoke((CancellationToken)args[0], (TArg1)args[1], (TArg2)args[2], (TArg3)args[3]);
            });
        }
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <typeparam name="TArg1">The Type of the first argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg2">The Type of the second argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg3">The Type of the third argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg4">The Type of the fourth argument to be passed into the thread method</typeparam>
        /// <param name="action">The action to run on a separate thread</param>
        public static ThreadSlim Create<TArg1, TArg2, TArg3, TArg4>(Action<CancellationToken, TArg1, TArg2, TArg3, TArg4> action)
        {
            return new ThreadSlim((cts, args) =>
            {
                action.Invoke((CancellationToken)args[0], (TArg1)args[1], (TArg2)args[2], (TArg3)args[3], (TArg4)args[4]);
            });
        }
        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <typeparam name="TArg1">The Type of the first argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg2">The Type of the second argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg3">The Type of the third argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg4">The Type of the fourth argument to be passed into the thread method</typeparam>
        /// <typeparam name="TArg5">The Type of the fifth argument to be passed into the thread method</typeparam>
        /// <param name="action">The action to run on a separate thread</param>
        public static ThreadSlim Create<TArg1, TArg2, TArg3, TArg4, TArg5>(Action<CancellationToken, TArg1, TArg2, TArg3, TArg4, TArg5> action)
        {
            return new ThreadSlim((cts, args) =>
            {
                action.Invoke((CancellationToken)args[0], (TArg1)args[1], (TArg2)args[2], (TArg3)args[3], (TArg4)args[4], (TArg5)args[5]);
            });
        }
    }

    ///// <summary>
    ///// A lighter thread wrapper than W.Threading.Thread
    ///// </summary>
    //public class ThreadSlim : IDisposable
    //{
    //    private object _lock = new object();
    //    //protected virtual void ThreadProc(ThreadDelegate del, params object[] args)
    //    //{
    //    //    if (_isAction)
    //    //    {
    //    //        del.Invoke(_cts.Token);
    //    //    }
    //    //    else
    //    //    {
    //    //        del.Invoke(_cts.Token, args);
    //    //    }
    //    //}
    //    protected ThreadMethod _threadProc = null;
    //    protected CancellationTokenSource _cts;
    //    protected bool _isAction = false;
    //    protected Task Task { get; set; }

    //    /// <summary>
    //    /// A user-defined name for this object
    //    /// </summary>
    //    public string Name { get; set; }
    //    /// <summary>
    //    /// True if the thread has started, otherwise False
    //    /// </summary>
    //    public bool IsStarted => _threadProc.IsRunning || _threadProc.IsComplete;
    //    /// <summary>
    //    /// True if the thread is currently running, oherwise False
    //    /// </summary>
    //    public bool IsRunning => _threadProc.IsRunning;
    //    /// <summary>
    //    /// True if the thread method has run to completion, otherwise False
    //    /// </summary>
    //    /// <remarks>This value is reset when Start is called</remarks>
    //    public bool IsComplete => _threadProc.IsComplete;

    //    /// <summary>
    //    /// Signals the thread method to stop running
    //    /// </summary>
    //    public void SignalToStop()
    //    {
    //        lock (_lock)
    //            _cts?.Cancel();
    //    }
    //    /// <summary>
    //    /// Signals the thread method to stop running and waits for it to complete
    //    /// </summary>
    //    public void Stop()
    //    {
    //        //if (IsRunning)
    //        //{
    //        lock (_lock)
    //            _cts?.Cancel();
    //        Wait();
    //        //}
    //    }
    //    /// <summary>
    //    /// Signals the thread method to stop running and waits the specified number of milliseconds for it to complete before returning
    //    /// </summary>
    //    /// <param name="msTimeout">The number of milliseconds to wait for the thread method to comlete</param>
    //    /// <returns>True if the thread method completes within the specified number of milliseconds, otherwise False</returns>
    //    public bool Stop(int msTimeout)
    //    {
    //        //if (IsRunning)
    //        //{
    //        lock (_lock)
    //            _cts?.Cancel();
    //        return Wait(msTimeout);
    //        //}
    //        //return true;
    //    }
    //    /// <summary>
    //    /// Signals the thread method to stop running and waits for the method to complete, while observing the specified CancellationToken
    //    /// </summary>
    //    /// <param name="token">The CancellationToken to observe while waiting</param>
    //    /// <returns>True if the thread method completes before the CancellationToken cancels, otherwise False</returns>
    //    public void Stop(CancellationToken token)
    //    {
    //        //if (IsRunning)
    //        //{
    //        lock (_lock)
    //            _cts?.Cancel();
    //        Wait(token);
    //        //}
    //    }
    //    ///// <summary>
    //    ///// Starts the thread
    //    ///// </summary>
    //    //public async void Start()
    //    //{
    //    //    Start(null);
    //    //}
    //    ///// <summary>
    //    ///// Starts the thread
    //    ///// </summary>
    //    ///// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
    //    //public void Start(int msLifetime)
    //    //{
    //    //    StartAsync(msLifetime, null);
    //    //}
    //    ///// <summary>
    //    ///// Starts the thread while passing in the specified values
    //    ///// </summary>
    //    ///// <param name="args">Values to pass into the thread method</param>
    //    //public void Start(params object[] args)
    //    //{
    //    //    StartAsync(args);
    //    //    //_cts?.Dispose();
    //    //    //_cts = new CancellationTokenSource();
    //    //    //_threadProc.Run(_cts.Token, TaskCreationOptions.LongRunning, args);
    //    //}
    //    ///// <summary>
    //    ///// Starts the thread while passing in the specified values
    //    ///// </summary>
    //    ///// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
    //    ///// <param name="args">Values to pass into the thread method</param>
    //    //public void Start(int msLifetime, params object[] args)
    //    //{
    //    //    StartAsync(msLifetime, args);
    //    //    //_cts?.Dispose();
    //    //    //_cts = new CancellationTokenSource();
    //    //    //_threadProc.Run(_cts.Token, TaskCreationOptions.LongRunning, args);
    //    //}
    //    ///// <summary>
    //    ///// Asynchronoously starts the thread
    //    ///// </summary>
    //    ///// <returns>The Task associated with the thread</returns>
    //    //public async Task StartAsync()
    //    //{
    //    //    _cts?.Dispose();
    //    //    _cts = new CancellationTokenSource();
    //    //    await _threadProc.RunAsync(_cts.Token, TaskCreationOptions.LongRunning, null);
    //    //}

    //    /// <summary>
    //    /// Starts the thread
    //    /// </summary>
    //    public void Start()
    //    {
    //        if (IsRunning)
    //            return;
    //        lock (_lock)
    //        {
    //            _cts?.Dispose();
    //            _cts = new CancellationTokenSource();
    //            Task = _threadProc.RunAsync(_cts.Token, TaskCreationOptions.LongRunning, null);
    //            Task.ContinueWith(task =>
    //            {
    //                if (task.Exception != null)
    //                    System.Diagnostics.Debugger.Break();
    //            });
    //        }
    //    }
    //    /// <summary>
    //    /// Asynchronoously starts the thread
    //    /// </summary>
    //    /// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
    //    /// <returns>The Task associated with the thread</returns>
    //    public void Start(int msLifetime)
    //    {
    //        if (IsRunning)
    //            return;
    //        lock (_lock)
    //        {
    //            _cts?.Dispose();
    //            if (msLifetime < 0)
    //                _cts = new CancellationTokenSource();
    //            else
    //                _cts = new CancellationTokenSource(msLifetime);
    //            Task = _threadProc.RunAsync(_cts.Token, TaskCreationOptions.LongRunning, null);
    //            Task.ContinueWith(task =>
    //            {
    //                if (task.Exception != null)
    //                    System.Diagnostics.Debugger.Break();
    //            });
    //        }
    //    }
    //    /// <summary>
    //    /// Asynchronoously starts the thread while passing in the specified values
    //    /// </summary>
    //    /// <param name="args">Values to pass into the thread method</param>
    //    /// <returns>The Task associated with the thread</returns>
    //    public void Start(params object[] args)
    //    {
    //        if (IsRunning)
    //            return;
    //        lock (_lock)
    //        {
    //            _cts?.Dispose();
    //            _cts = new CancellationTokenSource();
    //            if (_isAction) //ignore the arguments if it was created with an Action<CancellationToken>
    //                args = null;
    //            Task = _threadProc.RunAsync(_cts.Token, TaskCreationOptions.LongRunning, args);
    //            Task.ContinueWith(task =>
    //            {
    //                if (task.Exception != null)
    //                    System.Diagnostics.Debugger.Break();
    //            });
    //        }
    //    }
    //    /// <summary>
    //    /// Asynchronoously starts the thread while passing in the specified values
    //    /// </summary>
    //    /// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
    //    /// <param name="args">Values to pass into the thread method</param>
    //    /// <returns>The Task associated with the thread</returns>
    //    public void Start(int msLifetime, params object[] args)
    //    {
    //        if (IsRunning)
    //            return;
    //        lock (_lock)
    //        {
    //            _cts?.Dispose();
    //            if (msLifetime < 0)
    //                _cts = new CancellationTokenSource();
    //            else
    //                _cts = new CancellationTokenSource(msLifetime);

    //            if (_isAction) //ignore the arguments if it was created with an Action<CancellationToken>
    //                args = null;
    //            Task = _threadProc.RunAsync(_cts.Token, TaskCreationOptions.LongRunning, args);
    //            Task.ContinueWith(task =>
    //            {
    //                if (task.Exception != null)
    //                    System.Diagnostics.Debugger.Break();
    //            });
    //        }
    //    }
    //    /// <summary>
    //    /// Waits for the thread to complete
    //    /// </summary>
    //    public void Wait()
    //    {
    //        _threadProc.Wait();
    //    }
    //    /// <summary>
    //    /// Waits for the thread to complete
    //    /// </summary>
    //    /// <param name="msTimeout">The number of milliseconds to wait before failing</param>
    //    /// <returns>True if the thread completes within the specified number of milliseconds, otherwise False</returns>
    //    public bool Wait(int msTimeout)
    //    {
    //        return _threadProc.Wait(msTimeout);
    //    }
    //    /// <summary>
    //    /// Waits for the thread to complete
    //    /// </summary>
    //    /// <param name="token">A CancellationToken to observe while waiting</param>
    //    /// <returns>True if the thread completes before the CancellationToken is cancelled, otherwise False</returns>
    //    public bool Wait(CancellationToken token)
    //    {
    //        _threadProc.Wait(token);
    //        return _threadProc.IsComplete;
    //    }

    //    /// <summary>
    //    /// Disposes the ThreadSlim instance and releases resources
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        //_threadProc.Dispose();
    //        _cts?.Dispose();
    //    }

    //    internal ThreadSlim() { }
    //    /// <summary>
    //    /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
    //    /// </summary>
    //    /// <param name="action">The action to run on a separate thread</param>
    //    public ThreadSlim(Action<CancellationToken> action)
    //    {
    //        _isAction = true;
    //        _threadProc = new ThreadMethod(args => action.Invoke(_cts.Token));
    //    }
    //    /// <summary>
    //    /// Constructs a new ThreadSlim using a ThreadDelegate as the thread method
    //    /// </summary>
    //    /// <param name="delegate">The ThreadDelegate to run on a separate thread</param>
    //    public ThreadSlim(ThreadDelegate @delegate)
    //    {
    //        _threadProc = new ThreadMethod(args => @delegate.Invoke(_cts.Token, args));
    //    }
    //}
}
