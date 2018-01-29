using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Threading.ThreadExtensions;
using W.AsExtensions;

namespace W.Threading
{
    /// <summary>
    /// The delegate Type used by ThreadMethod
    /// </summary>
    /// <param name="args"></param>
    public delegate void AnyMethodDelegate(params object[] args);

    /// <summary>
    /// Adds multi-threading and additional functionality to an Action or ThreadMethodDelegate
    /// </summary>
    public partial class ThreadMethod : IDisposable
    {
        private object _lockDispose = new object();
        private AnyMethodDelegate _delegate = null;
        private CancellationTokenSource _ctsFinalizer = new CancellationTokenSource();
        private ManualResetEventSlim _mreIsRunning = new ManualResetEventSlim();
        private ManualResetEventSlim _mreIsComplete = new ManualResetEventSlim();
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
        public bool IsRunning => _mreIsRunning.IsSet && !IsComplete;
        /// <summary>
        /// True if the method has completed, otherwise False
        /// </summary>
        public bool IsComplete => _mreIsComplete.IsSet;

        private void WaitToRun()
        {
            _mreIsRunning.Wait(_ctsFinalizer.Token);
        }
        private void RunTheMethod(Task task)
        {
            try
            {
                //_mreIsRunning.Wait(_ctsFinalizer.Token);
                if (_ctsFinalizer.Token.IsCancellationRequested)
                    return;
                //var ok = !_ctsFinalizer.IsCancellationRequested;// !task.IsFaulted && !task.IsCanceled;
                //ok = (ok && !_ctsFinalizer.Token.IsCancellationRequested);
                //if (ok)
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
                _mreIsComplete.Set();//allow waiters to continue
            }
        }

        /// <summary>
        /// Initializes variables and creates the task associated with this ThreadMethod
        /// </summary>
        protected void Run()
        {
            _mreIsRunning.Reset();
            _mreIsComplete.Reset();

            _runTask = Task.Run(() => Task.Factory.StartNew(WaitToRun, TaskCreationOptions.LongRunning)
                           .ContinueWith(RunTheMethod, TaskContinuationOptions.LongRunning)
            , _ctsFinalizer.Token).ContinueWith(task =>
            {
                _mreIsRunning.Reset();
            });
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

            _args = args;
            Run();

            _mreIsRunning.Set();
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

            _args = args;
            Run();

            _mreIsRunning.Set();
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

            _args = args;
            Run();

            _mreIsRunning.Set();
            await _runTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Waits for the thread method to complete
        /// </summary>
        public void Wait()
        {
            _mreIsComplete.Wait();
        }
        /// <summary>
        /// Waits for the thread method to complete
        /// </summary>
        /// <param name="msTimeout">The maximum number of milliseconds to wait for the thread to method complete</param>
        /// <returns>True if the thread method completes within the timeout period, otherwise False</returns>
        public bool Wait(int msTimeout)
        {
            return _mreIsComplete.Wait(msTimeout);
        }
        /// <summary>
        /// Waits for the thread method to complete
        /// </summary>
        /// <param name="token">The CancellationToken to observe while waiting</param>
        /// <returns>True if the thread method completes before the CancellationToken is cancelled, otherwise False</returns>
        public bool Wait(CancellationToken token)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            _mreIsComplete.Wait(token);
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
                _ctsFinalizer.Cancel();
                _mreIsComplete.Set();
                //_mreIsComplete.Wait(); //wait for it to complete
                //#if NET45
                //_runTask.Wait(1000);
                //_runTask.Dispose();
                //#endif
                _ctsFinalizer.Dispose();
                //_mreIsComplete.Set(); //unlock any waiters
                _mreIsComplete.Dispose();
                _mreIsRunning.Dispose();
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
        ///// <summary>
        ///// Destructs the ThreadMethod and releases resources
        ///// </summary>
        //~ThreadMethod()
        //{
        //    Dispose();
        //}
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
