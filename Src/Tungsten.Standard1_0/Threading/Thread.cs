using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// A thread class
    /// </summary>
    public partial class Thread : ThreadSlim
    {
        private new void Wait() { }
        private new bool Wait(int msTimeout) { return false; }
        private new bool Wait(CancellationToken token) { return false; }

        public void Join() { base.Wait(); }
        public bool Join(int msTimeout) { return base.Wait(msTimeout); }
        public bool Join(CancellationToken token) { return base.Wait(token); }

        /// <summary>
        /// Disposes the Thread and releases resources
        /// </summary>
        public new void Dispose()
        {
            Stop();
            base.Dispose();
        }

        /// <summary>
        /// Constructs a new ThreadSlim using an Action&lt;CancellationToken&gt; as the thread method
        /// </summary>
        /// <param name="action">The action to run on a separate thread</param>
        public Thread(Action<CancellationToken> action) : base(action) { }
        /// <summary>
        /// Constructs a new ThreadSlim using a ThreadDelegate as the thread method
        /// </summary>
        /// <param name="delegate">The ThreadDelegate to run on a separate thread</param>
        public Thread(ThreadDelegate @delegate) : base(@delegate) { }
    }
    public partial class Thread //static methods
    {
        public static Thread Create(Action<CancellationToken> action)
        {
            var result = new Thread(action);
            return result;
        }
        public static Thread<TType> Create<TType>(Action<CancellationToken, TType> action)
        {
            var result = new Thread<TType>(action);
            return result;
        }
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
            {
                System.Threading.Tasks.Task.Delay(msDelay);
            }
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
                case CPUProfileEnum.SpinUntil:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
                    break;
            }
        }

        /// <summary>
        /// Attempts to free the CPU for other processes, based on the desired level.  Consequences will vary depending on your hardware architecture.  The more processors/cores you have, the better performance you will have by selecting SpinWait1.  Likewise, on a single-core processor, you may wish to select SpinWait0.
        /// </summary>
        /// <param name="level">The desired level of CPU usage</param>
        /// <param name="msTimeout">Optional value for CPUProfileEnum.Sleep and CPUProfileEnum.SpinUntil. Ignored by other profiles.</param>
        /// <remarks>Note results may vary.  SpinWait1 will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture.  This may not always be the case.</remarks>
        public static void Sleep(CPUProfileEnum level, int msTimeout = 1)
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
                    W.Threading.Thread.Sleep(msTimeout);
                    break;
                case CPUProfileEnum.SpinWait1:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
                    break;
                case CPUProfileEnum.SpinUntil:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, msTimeout);
                    break;
            }
        }
    }

    /// <summary>
    /// A thread class which can pass a typed parameter into the thread method
    /// </summary>
    /// <typeparam name="TType">The argument Type to be passed into the thread method</typeparam>
    public class Thread<TType> : Thread
    {
        /// <summary>
        /// Starts the thread and returns the Task associated with it 
        /// </summary>
        /// <param name="arg">The argument to pass into the thread procedure</param>
        /// <returns></returns>
        public async Task StartAsync(TType arg)
        {
            await base.StartAsync(args: arg);
        }
        /// <summary>
        /// Constructs a new Thread which can accept a single, typed, paramter
        /// </summary>
        /// <param name="action">The action to run on a separate thread</param>
        public Thread(Action<CancellationToken, TType> action) : base((token, args) => action.Invoke(token, (TType)args[0])) { }
    }


    //    /// <summary>
    //    /// A thread class which can pass a typed parameter into the thread action
    //    /// </summary>
    //    /// <typeparam name="TParameterType">The argument Type to be passed into the thread action</typeparam>
    //    public class Thread<TParameterType> : Thread
    //    {
    //        private Action<TParameterType, CancellationToken> _action;
    //        private TParameterType _defaultArg;
    //        private TParameterType _arg;

    //        /// <summary>
    //        /// Calls the action encapsulated by this thread.  This method can be overridden to provide more specific functionality.
    //        /// </summary>
    //        /// <param name="token">The CancellationToken, passed into the action, which can be used to cancel the thread Action</param>
    //        protected override void CallAction(CancellationToken token)
    //        {
    //            _action.Invoke(_arg, token);
    //            //base.CallAction(token);
    //        }

    //        /// <summary>
    //        /// Starts the thread if it's not already running
    //        /// </summary>
    //        public override void Start()
    //        {
    //            Start(_defaultArg);
    //        }
    //        /// <summary>
    //        /// Starts the thread if it's not already running
    //        /// </summary>
    //        /// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
    //        public override void Start(int msLifetime)
    //        {
    //            Start(_defaultArg, msLifetime);
    //        }
    //        /// <summary>
    //        /// Starts the thread if it's not already running, passing in the specified argument
    //        /// </summary>
    //        /// <param name="arg">The argument to pass into the threaded Action</param>
    //        public void Start(TParameterType arg)
    //        {
    //            Start(arg, -1);
    //        }
    //        /// <summary>
    //        /// Starts the thread if it's not already running
    //        /// </summary>
    //        /// <param name="arg">The argument to pass into the threaded Action</param>
    //        /// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
    //        public void Start(TParameterType arg, int msLifetime)
    //        {
    //            _arg = arg;
    //            base.Start(msLifetime);
    //        }
    //        /// <summary>
    //        /// Constructs a new Thread which can accept a parameter
    //        /// </summary>
    //        /// <param name="action">The CancellationToken which can be used to cancel the thread</param>
    //        /// <remarks>Calling this constructor will automatically start the thread</remarks>
    //        public Thread(Action<TParameterType, CancellationToken> action) : this(action, default(TParameterType), true)
    //        {
    //        }
    //        /// <summary>
    //        /// Constructs a new Thread which can accept a parameter
    //        /// </summary>
    //        /// <param name="action">The CancellationToken which can be used to cancel the thread</param>
    //        /// <param name="autoStart">If True, the Thread will immediately start, otherwise the Start method will have to be called manually</param>
    //        public Thread(Action<TParameterType, CancellationToken> action, bool autoStart) : this(action, default(TParameterType), autoStart)
    //        {
    //        }
    //        /// <summary>
    //        /// Constructs a new Thread which can accept a parameter
    //        /// </summary>
    //        /// <param name="action">The CancellationToken which can be used to cancel the thread</param>
    //        /// <param name="defaultArg">The default argument to pass into the thread procedure</param>
    //        /// <remarks>Calling this constructor will automatically start the thread</remarks>
    //        public Thread(Action<TParameterType, CancellationToken> action, TParameterType defaultArg) : this(action, defaultArg, true)
    //        {
    //            _action = action;
    //            _defaultArg = defaultArg;
    //        }
    //        /// <summary>
    //        /// Constructs a new Thread which can accept a parameter
    //        /// </summary>
    //        /// <param name="action">The CancellationToken which can be used to cancel the thread</param>
    //        /// <param name="defaultArg">The default argument to pass into the thread procedure</param>
    //        /// <param name="autoStart">If True, the Thread will immediately start, otherwise the Start method will have to be called manually</param>
    //        public Thread(Action<TParameterType, CancellationToken> action, TParameterType defaultArg, bool autoStart) : base(null, autoStart)
    //        {
    //            _action = action;
    //            _defaultArg = defaultArg;
    //        }
    //    }
    //    /// <summary>
    //    /// A thread class
    //    /// </summary>
    //    public partial class Thread : IDisposable
    //    {
    //        private ThreadSlim _threadProc;
    //        private Action<System.Threading.CancellationToken> _action;
    //        private System.Threading.CancellationTokenSource cts;

    //        private void ThreadProc(CancellationToken token)
    //        {
    //            try
    //            {
    //                CallAction(cts.Token);
    //            }
    //            catch (Exception e)
    //            {
    //                IsFaulted = true;
    //                Exception = e;
    //            }
    //            finally
    //            {
    //            }
    //        }

    //        /// <summary>
    //        /// Calls the action encapsulated by this thread.  This method can be overridden to provide more specific functionality.
    //        /// </summary>
    //        /// <param name="token">The CancellationToken, passed into the action, which can be used to cancel the thread Action</param>
    //        /// <param name="args">Values to pass into the thread method</param>
    //        protected virtual void CallAction(CancellationToken token)
    //        {
    //            _action.Invoke(token);
    //        }
    //        /// <summary>
    //        /// True if the thread has been started, otherwise False
    //        /// </summary>
    //        public bool IsStarted => _threadProc?.IsRunning ?? false;
    //        /// <summary>
    //        /// True if the thread has completed, otherwise False
    //        /// </summary>
    //        /// <remarks>A True value does not indicate that the thread completed successfully</remarks>
    //        public bool IsComplete => _threadProc?.IsComplete ?? false;
    //        /// <summary>
    //        /// True if the thread raised an exception, otherwise False
    //        /// </summary>
    //        public bool IsFaulted { get; private set; }
    //        /// <summary>
    //        /// The exception, if one was caught
    //        /// </summary>
    //        public Exception Exception { get; private set; }
    //        /// <summary>
    //        /// Not available until after Start has been called
    //        /// </summary>
    //        public CancellationToken Token { get { return cts.Token; } }
    //        /// <summary>
    //        /// Block the calling thread until this thread object has completed
    //        /// </summary>
    //        public void Join()
    //        {
    //            _threadProc.Wait();
    //        }
    //        /// <summary>
    //        /// Block the calling thread until this thread object has completed or until the timeout has occurred
    //        /// </summary>
    //        /// <param name="msTimeout">The number of milliseconds to wait, for the thread Action to complete, before timing out</param>
    //        /// <returns>True if the thread Action completed within the specified timeout, otherwise False</returns>
    //        public bool Join(int msTimeout)
    //        {
    //            return _threadProc.Wait(msTimeout);
    //        }
    //        /// <summary>
    //        /// Start the thread
    //        /// </summary>
    //        public virtual void Start()
    //        {
    //            Start(msLifetime: -1);
    //        }
    //        /// <summary>
    //        /// Start the thread with a CancellationToken which will timeout in the specified milliseconds
    //        /// </summary>
    //        /// <param name="msLifetime">The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.</param>
    //        public virtual void Start(int msLifetime)
    //        {
    //            if (msLifetime >= 0)
    //                cts = new System.Threading.CancellationTokenSource(msLifetime);
    //            else
    //                cts = new System.Threading.CancellationTokenSource();
    //            _threadProc = new ThreadSlim(ThreadProc);
    //            _threadProc.Start();
    //        }
    //        /// <summary>
    //        /// Stop the thread
    //        /// </summary>
    //        public void Stop()
    //        {
    //            _threadProc?.Stop();
    //        }
    //        /// <summary>
    //        /// Dispose the thread and free resources
    //        /// </summary>
    //        public void Dispose()
    //        {
    //            Stop();
    //        }
    //        /// <summary>
    //        /// Constructs a new Thread
    //        /// </summary>
    //        /// <param name="action">The action to call on a thread</param>
    //        public Thread(Action<CancellationToken> action) : this(action, true)
    //        {
    //        }
    //        /// <summary>
    //        /// Constructs a new Thread
    //        /// </summary>
    //        /// <param name="action">The action to call on a thread</param>
    //        /// <param name="autoStart">If True, the thread will immediately start.  Otherwise Start will have to be called.</param>
    //        public Thread(Action<CancellationToken> action, bool autoStart)
    //        {
    //            _action = action;
    //            if (autoStart)
    //                Start();
    //        }
    //    }
    //    public partial class Thread //backwards compatability
    //    {
    //        /// <summary>
    //        /// Cancel the thread
    //        /// </summary>
    //        [Obsolete("Call Stop or Dispose instead.")]
    //        public void Cancel()
    //        {
    //            cts.Cancel();
    //            _threadProc.Wait();
    //        }
    //        /// <summary>
    //        /// True if the thread is currently running, otherwise False
    //        /// </summary>
    //        [Obsolete("Use IsStarted instead")]
    //        public bool IsRunning => IsStarted && !IsComplete;
    //    }

    //    public partial class Thread //static and factory methods
    //    {
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
    //        /// Starts a new thread
    //        /// </summary>
    //        /// <param name="action">Action to call on a thread</param>
    //        /// <returns>A new Thread</returns>
    //        public static Thread Create(Action<CancellationToken> action)
    //        {
    //            var result = new Thread(action);
    //            return result;
    //        }
    //        /// <summary>
    //        /// Starts a new thread
    //        /// </summary>
    //        /// <param name="action">Action to call on a thread</param>
    //        /// <param name="autoStart">If True, the thread immediately starts</param>
    //        /// <returns>A new Thread</returns>
    //        public static Thread Create(Action<CancellationToken> action, bool autoStart)
    //        {
    //            var result = new Thread(action, autoStart);
    //            return result;
    //        }
    //        /// <summary>
    //        /// Creates and starts a new thread
    //        /// </summary>
    //        /// <typeparam name="TParameterType">The argument Type to be passed into the thread action</typeparam>
    //        /// <param name="action">Action to call on a thread</param>
    //        /// <returns>A new Thread</returns>
    //        public static Thread<TParameterType> Create<TParameterType>(Action<TParameterType, CancellationToken> action)
    //        {
    //            var result = new Thread<TParameterType>(action, true);
    //            return result;
    //        }
    //        /// <summary>
    //        /// Creates a new thread
    //        /// </summary>
    //        /// <typeparam name="TParameterType">The argument Type to be passed into the thread action</typeparam>
    //        /// <param name="action">Action to call on a thread</param>
    //        /// <param name="autoStart">If True, the thread is immediately started</param>
    //        /// <returns>A new Thread</returns>
    //        public static Thread<TParameterType> Create<TParameterType>(Action<TParameterType, CancellationToken> action, bool autoStart)
    //        {
    //            var result = new Thread<TParameterType>(action, autoStart);
    //            return result;
    //        }
    //    }
}
