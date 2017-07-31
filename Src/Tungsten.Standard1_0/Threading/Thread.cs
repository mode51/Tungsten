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
            Exception ex = null;
            try
            {
                InvokeAction(Cts);
                Success.Value = true;
            }
            catch (TaskSchedulerException e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
            }
            catch (TaskCanceledException e)
            {
                ex = e;
                //System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc Exception:  " + e.Message);
            }
            catch (Exception e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc General Exception:  " + e.Message);
            }
            return ex;
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
            catch (TaskSchedulerException)
            {
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
            }
            catch (TaskCanceledException)
            {
                //System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc Exception:  " + e.Message);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc General Exception:  " + e.Message);
            }
        }

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
        public static void Sleep(int msDelay)
        {
#if NET45
            try
            {
                System.Threading.Thread.Sleep(msDelay);
            }
            catch (System.MissingMethodException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
#else
            System.Threading.Tasks.Task.Delay(msDelay);
#endif
        }
    }
}
