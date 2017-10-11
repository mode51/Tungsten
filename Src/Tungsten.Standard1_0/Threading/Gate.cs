using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// A thread Gate which supports passing in a typed parameter
    /// </summary>
    /// <typeparam name="TParameterType">The type of parameter that will be passed to the gated thread procedure</typeparam>
    public class Gate<TParameterType> : Gate
    {
        private Action<TParameterType, CancellationToken> _action;
        private TParameterType _arg;
        private TParameterType _defaultArg;

        /// <summary>
        /// Invokes the gated Action
        /// </summary>
        /// <param name="token">The CancellationToken which is passed into the Action and should be used to monitor whether the Gate has been cancelled</param>
        protected override void CallAction(CancellationToken token)
        {
            //override base functionality to call our own _action
            _action.Invoke(_arg, token);
        }

        /// <summary>
        /// Opens the gate (allows the gated Action to be called), passing in the default value which was specified in the constructor
        /// </summary>
        public override void Run()
        {
            Run(_defaultArg);
        }
        /// <summary>
        /// Opens the gate (allows the gated Action to be called), passing in the specified typed value
        /// </summary>
        /// <param name="arg">The argument to pass into the gated Action</param>
        public void Run(TParameterType arg)
        {
            this._arg = arg;
            base.Run();
        }

        /// <summary>
        /// Constructs a new Gate
        /// </summary>
        /// <param name="action">The Action to call when the gate is opened</param>
        public Gate(Action<TParameterType, CancellationToken> action) : this(action, default(TParameterType))
        {
        }
        /// <summary>
        /// Constructs a new Gate
        /// </summary>
        /// <param name="action">The Action to call when the gate is opened</param>
        /// <param name="defaultArg">A default value which will be passed to the gated Action, if not otherwise specified</param>
        public Gate(Action<TParameterType, CancellationToken> action, TParameterType defaultArg) : base(null)
        {
            _action = action;
            _defaultArg = defaultArg;
        }
    }
    /// <summary>
    /// A thread Gate is a background thread which is initially closed.  When a Gate is opened, the Action runs until completion.  The Gate can be opened (Run) any number of times.
    /// </summary>
    public class Gate : IDisposable
    {
        private W.Threading.Thread _thread;
        private Action<CancellationToken> _action;
        private ManualResetEventSlim _mreGateOkToRun = new ManualResetEventSlim(false);
        private ManualResetEventSlim _mreIsRunning = new ManualResetEventSlim(false);
        private ManualResetEventSlim _mreGateComplete = new ManualResetEventSlim(false);
        private CancellationTokenSource _gateCts;// = new CancellationTokenSource(); //initialize so to guarantee a safe call to dispose in ThreadProc
        private object _ctsLock = new object();

        private void ThreadProc(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _mreGateOkToRun.Wait(token);
                //now check to see if the Gate is being disposed
                if (token.IsCancellationRequested)
                    break;
                try
                {
                    lock (_ctsLock)
                    {
                        _gateCts = new CancellationTokenSource();
                    }
                    _mreIsRunning.Set();
                    CallAction(_gateCts.Token);
                }
                finally
                {
                    _mreGateComplete.Set();
                    _mreIsRunning.Reset();
                    lock (_ctsLock) //lock so that a call to Cancel can't access a disposed object before it's set to null
                    {
                        _gateCts.Dispose();
                        _gateCts = null;
                    }
                    _mreGateOkToRun.Reset();
                }
            }
        }

        /// <summary>
        /// Invokes the gated Action
        /// </summary>
        /// <param name="token">The CancellationToken which is passed into the Action and should be used to monitor whether the Gate has been cancelled</param>
        protected virtual void CallAction(CancellationToken token)
        {
            _action.Invoke(token);
        }

        /// <summary>
        /// True if the Gate is currently open (running), otherwise False
        /// </summary>
        public bool IsRunning => _mreIsRunning.IsSet;
        /// <summary>
        /// True if the gated Action has completed, otherwise False
        /// </summary>
        public bool IsComplete => _mreGateComplete.IsSet;
        /// <summary>
        /// Signals the thread to open the gate (allows the gated Action to be called).
        /// </summary>
        public virtual void Run()
        {
            if (!IsRunning)
            {
                _mreGateComplete.Reset();
                _mreGateOkToRun.Set();
                //_mreIsRunning.Wait();
            }
        }
        /// <summary>
        /// Blocks the calling thread until the gated Action is complete
        /// </summary>
        public void Join()
        {
            _mreGateComplete.Wait();
        }
        /// <summary>
        /// Blocks the calling thread until the gated Action is complete, or until the specified number of milliseconds has elapsed
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the gate to complete before timing out and returning False</param>
        /// <returns>True if the gate completed within the specified timeout, otherwise False</returns>
        public bool Join(int msTimeout)
        {
            //if (IsRunning && !IsComplete)
            return _mreGateComplete.Wait(msTimeout);//, _gateCts.Token);
            //return IsComplete;
        }
        /// <summary>
        /// Singals the gated Action that a Cancel has been requested
        /// </summary>
        public void Cancel()
        {
            lock (_ctsLock)
            {
                _gateCts?.Cancel();
            }
        }
        /// <summary>
        /// Cancels the gated Action, disposes the Gate and releases resources
        /// </summary>
        public void Dispose()
        {
            Cancel();
            _thread.Stop();
            _mreGateOkToRun.Set();//let the thread exit gracefully
            _thread.Join();
            _thread.Dispose();

            _mreGateOkToRun.Dispose();
            _mreGateComplete.Dispose();
        }
        /// <summary>
        /// Constructs a new Gate
        /// </summary>
        /// <param name="action">The Action to call when the gate is opened</param>
        public Gate(Action<CancellationToken> action)
        {
            _action = action;
            _thread = new W.Threading.Thread(ThreadProc, true);
        }
    }

    //10.10.2017 - GateSlim is untested, but I think it should work just as well as Gate
    ///// <summary>
    ///// A simplified Gate
    ///// </summary>
    //public class GateSlim : IDisposable
    //{
    //    private ManualResetEventSlim _mreRun = new ManualResetEventSlim(false);
    //    private ManualResetEventSlim _mreGateComplete = new ManualResetEventSlim(false);
    //    private CancellationTokenSource _cts = new CancellationTokenSource();
    //    private W.Threading.Thread _thread;
    //    private CancellationToken _token;

    //    /// <summary>
    //    /// Delegate to handle an unhandled exception raised by the Action
    //    /// </summary>
    //    /// <param name="sender">The GateSlim which caught the exception</param>
    //    /// <param name="e">The unhandled exception</param>
    //    public delegate void ExceptionDelegate(object sender, Exception e);
    //    /// <summary>
    //    /// Raised when an unhandled exception is raised in the Action
    //    /// </summary>
    //    public event ExceptionDelegate Exception;

    //    /// <summary>
    //    /// The Action to call when the gate is opened (when Run is called)
    //    /// </summary>
    //    public Action<CancellationToken> Action { get; set; }
    //    /// <summary>
    //    /// True if the gate has been opened and the Action is running, otherwise False
    //    /// </summary>
    //    public bool IsRunning => _mreRun.IsSet;
    //    /// <summary>
    //    /// True if the Action has run at least once and has completed, otherwise False
    //    /// </summary>
    //    public bool IsComplete => _mreGateComplete.IsSet;

    //    /// <summary>
    //    /// Open the Gate (invoke the Action).  Can only be cancelled by disposing the GateSlim.
    //    /// </summary>
    //    public void Run() { Run(CancellationToken.None); }
    //    /// <summary>
    //    /// Open the Gate (invoke the Action), passing in a CancellationToken to support cancellation
    //    /// </summary>
    //    /// <param name="token">A CancellationToken which the Action can check</param>
    //    public void Run(CancellationToken token)
    //    {
    //        _token = (token == CancellationToken.None) ? _cts.Token : token;
    //        _mreGateComplete.Reset();
    //        _mreRun.Set();
    //    }
    //    /// <summary>
    //    /// Blocks the calling thread until the gated Action is complete
    //    /// </summary>
    //    public void Join()
    //    {
    //        _mreGateComplete.Wait();
    //    }
    //    /// <summary>
    //    /// Blocks the calling thread until the gated Action is complete, or until the specified number of milliseconds has elapsed
    //    /// </summary>
    //    /// <param name="msTimeout">The number of milliseconds to wait for the gate to complete before timing out and returning False</param>
    //    /// <returns>True if the gate completed within the specified timeout, otherwise False</returns>
    //    public bool Join(int msTimeout)
    //    {
    //        return _mreGateComplete.Wait(msTimeout);
    //    }
    //    /// <summary>
    //    /// Blocks the calling thread until the gated Action is complete, or until the specified CancellationToken has been cancelled
    //    /// </summary>
    //    /// <param name="token">The CancellationToken used to join the Action</param>
    //    /// <returns>True if the gate completed before the CancellationToken was cancelled, otherwise False</returns>
    //    public bool Join(CancellationToken token)
    //    {
    //        _mreGateComplete.Wait(token);
    //        return !token.IsCancellationRequested;
    //    }

    //    /// <summary>
    //    /// Cancels the Action if it's running, Disposes the GateSlim object and releases resources
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        _cts.Cancel(); //first cancel the token
    //        _mreRun.Set(); //if it's currently blocking, allow it to run (it'll check _cts.Token first and immediately exit)
    //        _thread.Join(); //and wait for the thread to exit
    //    }
    //    /// <summary>
    //    /// Constructs a new GateSlim
    //    /// </summary>
    //    public GateSlim()
    //    {
    //        _thread = new Thread(token =>
    //        {
    //            while (true)
    //            {
    //                _mreRun.Wait(token); //wait indefinitely, or until the token is cancelled
    //                if (token.IsCancellationRequested || _cts.IsCancellationRequested)
    //                    break;
    //                try
    //                {
    //                    Action.Invoke(_token);
    //                }
    //                catch (Exception e)
    //                {
    //                    var evt = Exception;
    //                    evt?.Invoke(this, e);
    //                }
    //                _mreGateComplete?.Set();
    //                if (token.IsCancellationRequested || _cts.IsCancellationRequested)
    //                    break;
    //                _mreRun?.Reset();
    //            }
    //        });
    //    }
    //}
}


//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace W.Threading
//{
//    /// <summary>
//    /// <para>
//    /// A Gated thread.  Execution of the Action will proceed when the Run method is called.
//    /// </para>
//    /// </summary>
//    public class Gate : W.Threading.Thread
//    {
//        private AutoResetEvent _are = new AutoResetEvent(false);
//        private CancellationTokenSource _cts = new CancellationTokenSource();
//        private ManualResetEventSlim _mreComplete = new ManualResetEventSlim(false);

//        /// <summary>
//        /// <para>
//        /// Used to wrap the call to InvokeAction with try/catch handlers.  This method should call InvokeAction.
//        /// </para>
//        /// </summary>
//        /// <returns>An Exception if on occurs, otherwise null</returns>
//        protected override Exception CallInvokeAction()
//        {
//            Exception ex = null;
//            try
//            {
//                while ((!Cts?.IsCancellationRequested ?? false))// && (!_cts?.IsCancellationRequested ?? false))
//                {
//                    if (_are?.WaitOne(10) ?? false) //just release the thread periodically to check our CancellationTokenSources
//                    {
//                        try
//                        {
//                            InvokeAction(_cts.Token);
//                            //Console.WriteLine("Action Complete");
//                        }
//                        //catch (AggregateException e)
//                        //{
//                        //    //ignore 
//                        //}
//                        //catch (TaskCanceledException e)
//                        //{
//                        //    //ignore - this particular run was cancelled
//                        //}
//                        //catch (OperationCanceledException e)
//                        //{
//                        //    //ignore - this particular run was cancelled via CancellationTokenSource
//                        //}
//                        catch (Exception e)
//                        {
//                            //ignore
//                            System.Diagnostics.Debug.WriteLine(e.Message);
//                        }
//                        finally
//                        {
//                            _mreComplete?.Set();
//                        }
//                    }
//                }
//            }
//            catch (TaskSchedulerException e)
//            {
//                ex = e;
//                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
//            }
//            catch (TaskCanceledException e)
//            {
//                ex = e;
//            }
//            catch (Exception e)
//            {
//                ex = e;
//            }
//            return ex;
//        }
//        /// <summary>
//        /// Allows the Action to be called
//        /// </summary>
//        public void Run()
//        {
//            _cts?.Dispose();
//            _cts = new CancellationTokenSource();
//            _mreComplete.Reset();
//            _are.Set();
//        }

//        /// <summary>
//        /// Blocks the calling thread until the thread terminates
//        /// </summary>
//        public override void Join()
//        {
//            _mreComplete.Wait();
//        }

//        /// <summary>
//        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
//        /// </summary>
//        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
//        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
//        public override bool Join(int msTimeout)
//        {
//            return _mreComplete.Wait(msTimeout);
//        }

//        /// <summary>
//        /// Signals the task to cancel
//        /// </summary>
//        public new void Cancel()
//        {
//            _cts?.Cancel();
//        }
//        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//        public override void Dispose()
//        {
//            base.Cancel(); //to cancel Cts
//            base.Dispose();
//            Cancel();
//            _are?.Dispose();
//            _are = null;
//            _mreComplete?.Dispose();
//            _mreComplete = null;
//            _cts?.Dispose();
//            _cts = null;
//        }

//        /// <summary>
//        /// Construct a Gate
//        /// </summary>
//        /// <param name="action">The code to execute in a background task, when signaled</param>
//        /// <param name="onExit">Called after the </param>
//        public Gate(Action<CancellationToken> action, Action<bool, Exception> onExit = null) : base(action, onExit)
//        {
//        }
//    }

//    /// <summary>
//    /// <para>
//    /// A Gated thread.  Execution of the Action will proceed when the Run method is called.
//    /// </para>
//    /// </summary>
//    public class Gate<T> : W.Threading.Thread<T>
//    {
//        private AutoResetEvent Event { get; } = new AutoResetEvent(false);
//        private CancellationTokenSource _cts = new CancellationTokenSource();
//        private ManualResetEvent _mreComplete = new ManualResetEvent(false);

//        /// <summary>
//        /// <para>
//        /// Used to wrap the call to InvokeAction with try/catch handlers.  This method should call InvokeAction.
//        /// </para>
//        /// </summary>
//        /// <returns>An Exception if on occurs, otherwise null</returns>
//        protected override Exception CallInvokeAction()
//        {
//            Exception ex = null;
//            try
//            {
//                while (!Cts?.IsCancellationRequested ?? false)
//                {
//                    if (Event?.WaitOne(1000) ?? false) //just release the thread periodically
//                    {
//                        try
//                        {
//                            InvokeAction(_cts.Token);
//                        }
//                        catch (Exception e)
//                        {
//                            //ignore
//                            System.Diagnostics.Debug.WriteLine(e.Message);
//                        }
//                        finally
//                        {
//                            _mreComplete.Set();
//                        }
//                    }
//                }
//            }
//            catch (TaskSchedulerException e)
//            {
//                ex = e;
//                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
//            }
//            catch (TaskCanceledException e)
//            {
//                ex = e;
//            }
//            catch (Exception e)
//            {
//                ex = e;
//            }
//            return ex;
//        }
//        /// <summary>
//        /// Allows the Action to be called
//        /// </summary>
//        public void Run()
//        {
//            _cts?.Dispose();
//            _cts = new CancellationTokenSource();
//            _mreComplete.Reset();
//            Event.Set();
//        }
//        /// <summary>
//        /// Blocks the calling thread until the thread terminates
//        /// </summary>
//        public override void Join()
//        {
//            _mreComplete?.WaitOne();
//        }

//        /// <summary>
//        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
//        /// </summary>
//        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
//        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
//        public override bool Join(int msTimeout)
//        {
//            return _mreComplete?.WaitOne(msTimeout) ?? true;
//        }
//        /// <summary>
//        /// Signals the task to cancel
//        /// </summary>
//        public new void Cancel()
//        {
//            _cts?.Cancel(false);
//        }

//        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//        public override void Dispose()
//        {
//            Cancel(); //to cancel _cts
//            _mreComplete?.Dispose();
//            _mreComplete = null;
//            _cts?.Dispose();
//            _cts = null;
//            base.Cancel(); //to cancel Cts
//            base.Dispose();
//        }

//        /// <summary>
//        /// Construct a Gate
//        /// </summary>
//        /// <param name="action">The action to execute in a background task</param>
//        /// <param name="onExit">Called when the task completes</param>
//        /// <param name="args"></param>
//        public Gate(Action<T, CancellationToken> action, Action<bool, Exception> onExit = null, T args = default(T))
//            : base(action, onExit, args)
//        {
//        }
//    }
//}