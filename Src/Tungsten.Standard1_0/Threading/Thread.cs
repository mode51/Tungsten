using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// A thread wrapper which makes multi-threading easier
    /// </summary>
    public class Thread : ThreadBase
    {
#if NET45
        private readonly System.Threading.Thread _thread = null;
#else
        /// <summary>
        /// The Task/Thread which was created
        /// </summary>
        private Task _task;// { get; private set; }
#endif

        /// <summary>
        /// Called by the host thread procedure, this method calls the Action
        /// </summary>
        /// <returns>An Exception object, if an exception occured</returns>
        //[DebuggerStepThrough]
        protected override Exception CallInvokeAction()
        {
            Exception result = null;
            try
            {
                InvokeAction(Cts);
                Success.Value = true;
            }
            catch (AggregateException e)
            {
                System.Diagnostics.Debug.WriteLine("W.Threading.Thread.CallInvokeAction.AggregateException: " + e.Message);
            }
            catch (TaskSchedulerException e)
            {
                result = e;
                System.Diagnostics.Debug.WriteLine("W.Threading.Thread.CallInvokeAction.TaskSchedulerException: " + e.Message);
            }
            catch (TaskCanceledException e)
            {
                result = e;
                //System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc Exception:  " + e.Message);
            }
            catch (Exception e)
            {
                result = e;
                System.Diagnostics.Debug.WriteLine("W.Threading.Thread.CallInvokeAction.Exception: " + e.Message);
            }
            return result;
        }

        /// <summary>
        /// Calls the onExit Action when the thread returns
        /// </summary>
        /// <param name="e">An Exception object, if an exception occured</param>
        protected override void CallInvokeOnComplete(Exception e)
        {
            try
            {
                base.CallInvokeOnComplete(e);
            }
            catch (AggregateException ex)
            {
                System.Diagnostics.Debug.WriteLine("W.Threading.Thread.CallInvokeOnComplete.AggregateException: " + ex.Message);
            }
            catch (TaskSchedulerException ex)
            {
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + ex.Message);
            }
            catch (TaskCanceledException)
            {
                //System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc Exception:  " + e.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc General Exception:  " + ex.Message);
            }
        }
        /// <summary>
        /// Cancels the thread.  This defaults to a 5 second allowance for the thread to quit on it's own.
        /// </summary>
        public override void Cancel()
        {
            base.Cancel();
#if NET45
            if (!Join(5000)) //give the thread 5 seconds to close down, otherwise force it
            {
                _thread?.Abort();
            }
#endif
        }
        /// <summary>
        /// Cancels the thread.  If the thread is still active after the specified number of milliseconds, it is forcefully aborted
        /// </summary>
        /// <param name="msForceAbortDelay"></param>
        public override void Cancel(int msForceAbortDelay)
        {
            base.Cancel(msForceAbortDelay);
#if NET45
            if (!Join(msForceAbortDelay)) //give the thread 5 seconds to close down, otherwise force it
            {
                _thread?.Abort();
            }
#endif
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates
        /// </summary>
        public override void Join()
        {
#if NET45
            _thread?.Join();
#else
            _task.Wait();
#endif
        }

        /// <summary>
        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
        public override bool Join(int msTimeout)
        {
#if NET45
            return _thread?.Join(msTimeout) ?? true;
#else
            return _task.Wait(msTimeout);
#endif
        }

        /// <summary>
        /// Disposes the Thread and releases resources
        /// </summary>
        public override void Dispose()
        {
#if NET45
            Cancel(3000);
#endif
            base.Dispose();
        }
        /// <summary>
        /// Constructs the Thread object
        /// </summary>
        /// <param name="action">The Action to be called on a new thread</param>
        /// <param name="onExit">The Action to be called when the thread completes</param>
        /// <param name="cts">A CancellationTokenSource to use instead of the default</param>
        public Thread(Action<CancellationTokenSource> action, Action<bool, Exception> onExit = null, CancellationTokenSource cts = null) : base(action, onExit, cts)
        {
#if NET45
            _thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadProc));
            _thread.Start();
#else
            _task = Task.Factory.StartNew(ThreadProc, Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
#endif
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onExit">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="cts">A CancellationTokenSource to use instead of the default</param>
        /// <returns>A new Thread</returns>
        public static Thread Create(Action<CancellationTokenSource> action, Action<bool, Exception> onExit = null, CancellationTokenSource cts = null)
        {
            var result = new Thread(action, onExit, cts);
            return result;
        }

        /// <summary>
        /// Blocks the calling thread for the specified time
        /// </summary>
        /// <param name="msDelay">The number of milliseconds to block the thread</param>
        /// <param name="useSpinWait">If True, a SpinWait.SpinUntil will be used instead of a call to Thread.Sleep (or Task.Delay).  Note that SpinWait should only be used on multi-core/cpu machines.</param>
        public static void Sleep(int msDelay, bool useSpinWait = false)
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
                case CPUProfileEnum.HighCPU:
#if NET45
                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
#else
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 0);
#endif
                    break;
                case CPUProfileEnum.NormalCPU:
                    W.Threading.Thread.Sleep(1);
                    break;
                case CPUProfileEnum.LowCPU:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
                    break;
            }
        }
    }
}
