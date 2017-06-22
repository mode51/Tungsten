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
        /// <summary>
        /// The Task/Thread which was created
        /// </summary>
        private Task _task;// { get; private set; }

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

        /// <summary>
        /// Blocks the calling thread until the thread terminates
        /// </summary>
        public override void Join()
        {
            _task.Wait();
        }

        /// <summary>
        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
        public override bool Join(int msTimeout)
        {
            return _task.Wait(msTimeout);
        }

        /// <summary>
        /// Constructs the Thread object
        /// </summary>
        /// <param name="action">The Action to be called on a new thread</param>
        /// <param name="onExit">The Action to be called when the thread completes</param>
        /// <param name="cts">A CancellationTokenSource to use instead of the default</param>
        public Thread(Action<CancellationTokenSource> action, Action<bool, Exception> onExit = null, CancellationTokenSource cts = null) : base(action, onExit, cts)
        {
            _task = Task.Factory.StartNew(ThreadProc, Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
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
            var result = false;
#if !(WINDOWS_PORTABLE || WINDOWS_UWP)
            try
            {
                System.Threading.Thread.Sleep(msDelay);
                result = true;
            }
            catch (System.MissingMethodException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
#endif
            if (!result)
                System.Threading.Tasks.Task.Delay(msDelay);
        }
    }
}
