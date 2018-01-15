using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Threading.ThreadExtensions;

namespace W.Threading
{
    /// <summary>
    /// The delegate Type used by ThreadMethod
    /// </summary>
    /// <param name="args"></param>
    public delegate void AnyMethodDelegate(params object[] args);

    ///// <summary>
    ///// Wraps an ThreadMethodDelegate with functionality to prevent re-entrancy and to know when the method is running and completed
    ///// </summary>
    //public partial class ThreadMethod : IDisposable
    //{
    //    private object _lock = new object();
    //    private ManualResetEventSlim _mreComplete = new ManualResetEventSlim(false);
    //    private ManualResetEventSlim _mreIsRunning = new ManualResetEventSlim(false);
    //    private ThreadMethodDelegate _delegate { get; set; }

    //    /// <summary>
    //    /// The exception, if one was caught
    //    /// </summary>
    //    public Exception Exception { get; private set; }
    //    /// <summary>
    //    /// True if the method is currently running, otherwise False
    //    /// </summary>
    //    public bool IsRunning { get { lock (_lock) return _mreIsRunning.IsSet; } }
    //    /// <summary>
    //    /// True if the method has completed, otherwise False.  The value is False initially and reset to False when Run is called.
    //    /// </summary>
    //    public bool IsComplete { get { return _mreComplete.IsSet; } }

    //    /// <summary>
    //    /// Block the calling thread until the method completes
    //    /// </summary>
    //    public void Wait()
    //    {
    //        if (IsRunning)
    //            _mreComplete.Wait();
    //    }
    //    /// <summary>
    //    /// Block the calling thread until the method completes
    //    /// </summary>
    //    /// <param name="msTimeout">The number of milliseconds to block before returning a False value.</param>
    //    /// <returns>True if the method completed within the specified timeout period, otherwise False</returns>
    //    public bool Wait(int msTimeout)
    //    {
    //        if (IsRunning)
    //            return _mreComplete.Wait(msTimeout);
    //        return true;
    //    }
    //    /// <summary>
    //    /// Block the calling thread until the method completes
    //    /// </summary>
    //    /// <param name="token">A CancellationToken which can be used to stop waiting for the method to complete</param>
    //    /// <returns>True if the method completed before the CancellatioToken was cancelled, otherwise False</returns>
    //    public void Wait(CancellationToken token)
    //    {
    //        if (IsRunning)
    //            _mreComplete.Wait(token);
    //    }

    //    /// <summary>
    //    /// Synchronously calls the delegate with the specified arguments.  These arguments must be accurate in number and in type to the arguments expected by the delegate.
    //    /// </summary>
    //    /// <param name="args">An array of arguments to be passed into the delegate</param>
    //    public void Run(params object[] args)
    //    {
    //        lock (_lock)
    //        {
    //            if (_mreIsRunning.IsSet)
    //                return;
    //            _mreIsRunning.Set();
    //            _mreComplete.Reset();
    //        }
    //        Exception ex = null;
    //        try
    //        {
    //            if (args?.Length > 0)
    //                _delegate?.Invoke(args);
    //            else
    //                _delegate?.Invoke();
    //        }
    //        catch (Exception e)
    //        {
    //            ex = e;
    //        }
    //        try
    //        {
    //            if (ex != null)
    //            {
    //                Exception = ex;
    //                //throw new Exception("The action raised an unhandled exception", ex);
    //            }
    //        }
    //        finally
    //        {
    //            lock (_lock)
    //                _mreIsRunning.Reset();
    //            _mreComplete.Set();
    //        }
    //    }
    //    /// <summary>
    //    /// Asynchronously calls the delegate with the specified arguments.  These arguments must be accurate in number and in type to the arguments expected by the delegate.
    //    /// </summary>
    //    /// <param name="args">An array of arguments to be passed into the delegate</param>
    //    /// <returns>The Task associated with this asynchronous operation</returns>
    //    public async Task RunAsync(params object[] args)
    //    {
    //        await RunAsync(CancellationToken.None, args);
    //    }
    //    /// <summary>
    //    /// Calls the delegate with the specified arguments.  These arguments must be accurate in number and in type to the arguments expected by the delegate.
    //    /// </summary>
    //    /// <param name="token">A CancellationToken which can be used to interrupt and stop execution of the delegate</param>
    //    /// <param name="args">An array of arguments to be passed into the delegate</param>
    //    /// <returns>The Task associated with this asynchronous operation</returns>
    //    public async Task RunAsync(CancellationToken token, params object[] args)
    //    {
    //        await RunAsync(token, TaskCreationOptions.None, args);
    //    }
    //    /// <summary>
    //    /// Calls the delegate with the specified arguments.  These arguments must be accurate in number and in type to the arguments expected by the delegate.
    //    /// </summary>
    //    /// <param name="token">A CancellationToken which can be used to interrupt and stop execution of the delegate</param>
    //    /// <param name="options">Task creation options to use when creating the Task</param>
    //    /// <param name="args">An array of arguments to be passed into the delegate</param>
    //    /// <returns>The Task associated with this asynchronous operation</returns>
    //    public async Task RunAsync(CancellationToken token, TaskCreationOptions options, params object[] args)
    //    {
    //        lock (_lock)
    //        {
    //            if (_mreIsRunning.IsSet)
    //                return;
    //            _mreIsRunning.Set();
    //            _mreComplete.Reset();
    //        };
    //        Exception ex = null;
    //        await Task.Factory.StartNew(() =>
    //        {
    //            try
    //            {
    //                if (args?.Length > 0)
    //                {
    //                    //_delegate.BeginInvoke(args, ar => _delegate.EndInvoke(ar), null);
    //                    _delegate?.Invoke(args);
    //                }
    //                else
    //                {
    //                    //_delegate.BeginInvoke(null, ar => _delegate.EndInvoke(ar), null);
    //                    _delegate?.Invoke();
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                ex = e;
    //                Exception = ex;
    //            }
    //        }, token, options, TaskScheduler.Current).ContinueWith(task =>
    //        {
    //            Exception = Exception ?? task.Exception;
    //            _mreComplete.Set();
    //            _mreIsRunning.Reset();
    //        });
    //        //if (ex != null) //should this be in the ContinueWith?
    //        //    throw new Exception("The action raised an unhandled exception", ex);
    //    }

    //    public void Dispose()
    //    {
    //        _mreComplete.Dispose();
    //        _mreIsRunning.Dispose();
    //        Exception = null;
    //        _delegate = null;
    //        _lock = null;
    //    }
    //    /// <summary>
    //    /// Constructs a new ThreadMethod
    //    /// </summary>
    //    /// <param name="action">An action to be called instead of an ThreadMethodDelegate</param>
    //    public ThreadMethod(Action action) : this((args => action.Invoke())) { }
    //    /// <summary>
    //    /// Constructs a new ThreadMethod
    //    /// </summary>
    //    /// <param name="delegate">The ThreadMethodDelgate to be called</param>
    //    public ThreadMethod(ThreadMethodDelegate @delegate)
    //    {
    //        if (@delegate == null)
    //            throw new ArgumentNullException(nameof(@delegate));
    //        _delegate = @delegate;
    //    }
    //    ///// <summary>
    //    ///// Destructs the ThreadMethod
    //    ///// </summary>
    //    //~ThreadMethod()
    //    //{
    //    //}
    //}
    //public partial class ThreadMethod //static members
    //{
    //    /// <summary>
    //    /// Creates a new ThreadMethod, immediately calls Run and returns the ThreadMethod instance
    //    /// </summary>
    //    /// <param name="action">The Action to be called</param>
    //    /// <returns>The ThreadMethod instanced created</returns>
    //    public static ThreadMethod Run(Action action)
    //    {
    //        var result = new ThreadMethod(action);
    //        result.Run();
    //        return result;
    //    }
    //    /// <summary>
    //    /// Creates a new ThreadMethod, immediately calls Run and returns the ThreadMethod instance
    //    /// </summary>
    //    /// <param name="delegate">The ActionMethodDelegate to be called</param>
    //    /// <returns>The ThreadMethod instanced created</returns>
    //    public static ThreadMethod Run(ThreadMethodDelegate @delegate)
    //    {
    //        var result = new ThreadMethod(@delegate);
    //        result.Run();
    //        return result;
    //    }
    //    /// <summary>
    //    /// Creates a new ThreadMethod, immediately calls Run and returns the ThreadMethod instance
    //    /// </summary>
    //    /// <param name="delegate">The ActionMethodDelegate to be called</param>
    //    /// <param name="args">Arguments to be passed into the ThreadMethodDelegate when called</param>
    //    /// <returns>The ThreadMethod instanced created</returns>
    //    public static ThreadMethod Run(ThreadMethodDelegate @delegate, params object[] args)
    //    {
    //        var result = new ThreadMethod(@delegate);
    //        result.Run(args);
    //        return result;
    //    }
    //    /// <summary>
    //    /// Creates a new ThreadMethod, immediately calls RunAsync and returns the ThreadMethod instance
    //    /// </summary>
    //    /// <param name="action">The Action to be called</param>
    //    /// <returns>The ThreadMethod instanced created</returns>
    //    public static async Task<ThreadMethod> RunAsync(Action action)
    //    {
    //        var result = new ThreadMethod(action);
    //        await result.RunAsync();
    //        return result;
    //    }
    //    /// <summary>
    //    /// Creates a new ThreadMethod, immediately calls RunAsync and returns the ThreadMethod instance
    //    /// </summary>
    //    /// <param name="delegate">The ActionMethodDelegate to be called</param>
    //    /// <returns>The ThreadMethod instanced created</returns>
    //    public static async Task<ThreadMethod> RunAsync(ThreadMethodDelegate @delegate)
    //    {
    //        var result = new ThreadMethod(@delegate);
    //        await result.RunAsync();
    //        return result;
    //    }
    //    /// <summary>
    //    /// Creates a new ThreadMethod, immediately calls RunAsync and returns the ThreadMethod instance
    //    /// </summary>
    //    /// <param name="delegate">The ActionMethodDelegate to be called</param>
    //    /// <param name="args">Arguments to be passed into the ThreadMethodDelegate when called</param>
    //    /// <returns>The ThreadMethod instanced created</returns>
    //    public static async Task<ThreadMethod> RunAsync(ThreadMethodDelegate @delegate, params object[] args)
    //    {
    //        var result = new ThreadMethod(@delegate);
    //        await result.RunAsync(args);
    //        return result;
    //    }
    //}

    //public class ThreadMethodSlim //: IDisposable
    //{
    //    //private CancellationTokenSource _cts;
    //    private ManualResetEventSlim _mreRun;
    //    private ManualResetEventSlim _mreComplete;
    //    private Task _runTask;
    //    private Task _cleanupTask;

    //    public bool IsComplete => _mreComplete.IsSet;

    //    public void Run()
    //    {
    //        _mreRun.Set();
    //    }
    //    public async Task RunAsync()
    //    {
    //        _mreRun.Set();
    //        await _runTask;
    //    }
    //    public void Wait()
    //    {
    //        _mreComplete.Wait();
    //    }
    //    public bool Wait(int msTimeout)
    //    {
    //        return _mreComplete.Wait(msTimeout);
    //    }
    //    public bool Wait(CancellationToken token)
    //    {
    //        _mreComplete.Wait(token);
    //        return IsComplete;
    //    }
    //    //public void Dispose()
    //    //{
    //    //    try
    //    //    {
    //    //        //if (!_cts?.IsCancellationRequested ?? false)
    //    //        //{
    //    //            _cts?.Cancel();
    //    //            _mreTerminated.Wait();
    //    //        //}
    //    //    }
    //    //    catch (Exception e)
    //    //    {
    //    //        System.Diagnostics.Debugger.Break();
    //    //        System.Diagnostics.Debug.WriteLine(e.ToString());
    //    //    }
    //    //}
    //    internal ThreadMethodSlim(Task runTask, Task cleanupTask, ManualResetEventSlim mreRun, ManualResetEventSlim mreComplete)//, CancellationTokenSource cts)
    //    {
    //        _runTask = runTask;
    //        _cleanupTask = cleanupTask;
    //        _mreRun = mreRun;
    //        _mreComplete = mreComplete;
    //        //_cts = cts;
    //    }
    //    ~ThreadMethodSlim()
    //    {
    //        //this has to be run synchronously to work
    //        _cleanupTask.RunSynchronously();
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="action"></param>
    //    /// <returns></returns>
    //    public static ThreadMethodSlim Create(Action action)
    //    {
    //        return action.AsThreadMethodSlim();
    //    }
    //}

    /// <summary>
    /// Adds multi-threading and additional functionality to an Action or ThreadMethodDelegate
    /// </summary>
    public partial class ThreadMethod : IDisposable
    {
        private object _lockDispose = new object();
        private AnyMethodDelegate _delegate = null;
        private CancellationTokenSource _ctsFinalizer = new CancellationTokenSource();
        private ManualResetEventSlim _mreRun = new ManualResetEventSlim();
        private ManualResetEventSlim _mreComplete = new ManualResetEventSlim();
        private Task _runTask;
        private object[] _args;

        /// <summary>
        /// A user-defined name for this object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if the method is running or has completed
        /// </summary>
        public bool IsStarted => IsRunning || IsComplete;
        /// <summary>
        /// True if the method is currently running and not complete, otherwise False
        /// </summary>
        public bool IsRunning => _mreRun.IsSet && !IsComplete;
        /// <summary>
        /// True if the method has completed, otherwise False
        /// </summary>
        public bool IsComplete => _mreComplete.IsSet;

        private void WaitToRun()
        {
            _mreRun.Wait(_ctsFinalizer.Token);
        }
        private void RunTheMethod(Task task)
        {
            var ok = !task.IsFaulted && !task.IsCanceled;
            ok = (ok && !_ctsFinalizer.Token.IsCancellationRequested);
            try
            {
                if (ok)
                {
                    _delegate.Invoke(_args);
                }
            }
#if NET45
                catch (System.Threading.ThreadAbortException)
                {
                    System.Threading.Thread.ResetAbort();
                }
#endif
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            finally
            {
                _mreComplete.Set();//allow waiters to continue
            }
        }

        /// <summary>
        /// Initializes variables and creates the task associated with this ThreadMethod
        /// </summary>
        protected void Initialize()
        {
            _mreRun.Reset();
            _mreComplete.Reset();
            _runTask = Task.Factory.StartNew(WaitToRun, TaskCreationOptions.LongRunning)
                           .ContinueWith(RunTheMethod, TaskContinuationOptions.LongRunning);
        }

        /// <summary>
        /// Synchronously runs the method
        /// </summary>
        /// <param name="args">Arguments to pass into the thread method</param>
        public void RunSynchronously(params object[] args)
        {
            //Start(args).Wait();
            if (IsRunning)
                throw new InvalidOperationException("The ThreadMethod is already running");
            if (!IsStarted || IsComplete)
                Initialize();

            _args = args;
            _mreRun.Set();
            _runTask.Wait();
        }
        /// <summary>
        /// Asynchronously runs the method
        /// </summary>
        /// <param name="args">Arguments to pass into the thread method</param>
        /// <returns>The Task associated with the thread</returns>
        public void Start(params object[] args)
        {
            if (IsRunning)
                throw new InvalidOperationException("The ThreadMethod is already running");
            if (!IsStarted || IsComplete)
                Initialize();

            _args = args;
            _mreRun.Set();
            _runTask.ConfigureAwait(false);
        }
        /// <summary>
        /// Asynchronously runs the method
        /// </summary>
        /// <param name="args">Arguments to pass into the thread method</param>
        /// <returns>The Task associated with the thread</returns>
        public async Task StartAsync(params object[] args)
        {
            if (IsRunning)
                throw new InvalidOperationException("The ThreadMethod is already running");
            if (!IsStarted || IsComplete)
                Initialize();

            _args = args;
            _mreRun.Set();
            await _runTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Waits for the thread method to complete
        /// </summary>
        public void Wait()
        {
            if (IsStarted)
                _mreComplete.Wait();
        }
        /// <summary>
        /// Waits for the thread method to complete
        /// </summary>
        /// <param name="msTimeout">The maximum number of milliseconds to wait for the thread to method complete</param>
        /// <returns>True if the thread method completes within the timeout period, otherwise False</returns>
        public bool Wait(int msTimeout)
        {
            return _mreComplete.Wait(msTimeout);
        }
        /// <summary>
        /// Waits for the thread method to complete
        /// </summary>
        /// <param name="token">The CancellationToken to observe while waiting</param>
        /// <returns>True if the thread method completes before the CancellationToken is cancelled, otherwise False</returns>
        public bool Wait(CancellationToken token)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            _mreComplete.Wait(token);
#if NETSTANDARD1_0 || WINDOWS_UWP || WINDOWS_PORTABLE
            System.Diagnostics.Debug.WriteLine("ThreadMethod.Wait: Elapsed = {0}ms", sw.ElapsedMilliseconds);
#else
            Console.WriteLine("ThreadMethod.Wait: Elapsed = {0}ms", sw.ElapsedMilliseconds);
#endif
            return IsComplete;
        }

        /// <summary>
        /// Disposes the ThreadMethod and releases resources
        /// </summary>
        public void Dispose()
        {
            lock (_lockDispose)
            {
                if (_runTask == null)
                    return;
                if (!_mreRun.IsSet) //still waiting?  Then cancel.
                    _ctsFinalizer.Cancel();
                _mreRun.Dispose();
                _ctsFinalizer.Dispose();
                if (!_mreComplete.IsSet)
                    _mreComplete.Set(); //unlock any waiters
                _mreComplete.Dispose();
#if NET45
                _runTask.Wait();
                _runTask.Dispose();
#endif
                _runTask = null;
                System.Diagnostics.Debug.WriteLine("ThreadMethod Destroyed");
            }
        }
        /// <summary>
        /// Construct a new ThreadMethod
        /// </summary>
        /// <param name="delegate">The ThreadMethodDelegate to call on a thread</param>
        public ThreadMethod(AnyMethodDelegate @delegate)
        {
            _delegate = @delegate;
            //Initialize();
        }
        /// <summary>
        /// Destructs the ThreadMethod and releases resources
        /// </summary>
        ~ThreadMethod()
        {
            Dispose();
        }
    }
    public partial class ThreadMethod //static members
    {
        /// <summary>
        /// Creates a new ThreadMethod from the ThreadMethodDelegate
        /// </summary>
        /// <param name="delegate">The ThreadMethodDelegate to wrap with an ThreadMethod</param>
        /// <returns>A new instance of an ThreadMethod</returns>
        public static ThreadMethod Create(AnyMethodDelegate @delegate)
        {
            return new ThreadMethod(@delegate);
        }
        /// <summary>
        /// Creates a new ThreadMethod from the Action
        /// </summary>
        /// <param name="action">The Action to wrap with an ThreadMethod</param>
        /// <returns>A new instance of an ThreadMethod</returns>
        public static ThreadMethod Create(Action action)
        {
            return new ThreadMethod(args => action.Invoke());
        }
        /// <summary>
        /// Creates a new ThreadMethod from the Action
        /// </summary>
        /// <param name="action">The Action to wrap with an ThreadMethod</param>
        /// <returns>A new instance of an ThreadMethod</returns>
        public static ThreadMethod Create<T>(Action<T> action)
        {
            return new ThreadMethod(args => action.Invoke((T)args[0]));
        }
        /// <summary>
        /// Creates a new ThreadMethod from the Action
        /// </summary>
        /// <param name="action">The Action to wrap with an ThreadMethod</param>
        /// <returns>A new instance of an ThreadMethod</returns>
        public static ThreadMethod Create<T1, T2>(Action<T1, T2> action)
        {
            return new ThreadMethod(args => action.Invoke((T1)args[0], (T2)args[1]));
        }
        /// <summary>
        /// Creates a new ThreadMethod from the Action
        /// </summary>
        /// <param name="action">The Action to wrap with an ThreadMethod</param>
        /// <returns>A new instance of an ThreadMethod</returns>
        public static ThreadMethod Create<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return new ThreadMethod(args => action.Invoke((T1)args[0], (T2)args[1], (T3)args[2]));
        }
        /// <summary>
        /// Creates a new ThreadMethod from the Action
        /// </summary>
        /// <param name="action">The Action to wrap with an ThreadMethod</param>
        /// <returns>A new instance of an ThreadMethod</returns>
        public static ThreadMethod Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return new ThreadMethod(args => action.Invoke((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]));
        }
        /// <summary>
        /// Creates a new ThreadMethod from the Action
        /// </summary>
        /// <param name="action">The Action to wrap with an ThreadMethod</param>
        /// <returns>A new instance of an ThreadMethod</returns>
        public static ThreadMethod Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return new ThreadMethod(args => action.Invoke((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]));
        }
    }
}
