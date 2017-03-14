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
        public Task Task { get; private set; }

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
            Task.Wait();
        }

        /// <summary>
        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
        public override bool Join(int msTimeout)
        {
            return Task.Wait(msTimeout);
        }

        /// <summary>
        /// Constructs the Thread object
        /// </summary>
        /// <param name="action">The Action to be called on a new thread</param>
        /// <param name="onExit">The Action to be called when the thread completes</param>
        /// <param name="cts">A CancellationTokenSource to use instead of the default</param>
        public Thread(Action<CancellationTokenSource> action, Action<bool, Exception> onExit = null, CancellationTokenSource cts = null) : base(action, onExit, cts)
        {
            Task = Task.Factory.StartNew(ThreadProc, Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
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
    }

    /// <summary>
    /// A thread wrapper which makes multi-threading easier
    /// </summary>
    /// <typeparam name="T">The type of custom data to pass to the thread Action</typeparam>
    public class Thread<T> : Thread
    {
        /// <summary>
        /// The custom data to pass into the Action
        /// </summary>
        protected T CustomData { get; set; }
        /// <summary>
        /// The Action to be run on a new thread
        /// </summary>
        protected new Action<T, CancellationTokenSource> Action { get; set; }

        /// <summary>
        /// Invokes the Action
        /// </summary>
        protected override void InvokeAction(CancellationTokenSource cts)
        {
            Action?.Invoke(CustomData, cts);
        }

        /// <summary>
        /// Constructs a new Thread object
        /// </summary>
        /// <param name="action">The Action to be called in the thread</param>
        /// <param name="onExit">The Action to be called when the thread completes</param>
        /// <param name="customData">The custom data to be passed into the thread</param>
        public Thread(Action<T, CancellationTokenSource> action, Action<bool, Exception> onExit = null, T customData = default(T), CancellationTokenSource cts = null) : base(null, onExit, cts)
        {
            Action = action;
            CustomData = customData;
        }

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onExit">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="customData">Custom data to be passed into the thread</param>
        /// <returns>A new long-running Thread object</returns>
        public static Thread<T> Create<T>(Action<T, CancellationTokenSource> action, Action<bool, Exception> onExit = null, T customData = default(T), CancellationTokenSource cts = null)
        {
            var result = new Thread<T>(action, onExit, customData, cts);
            return result;
        }
    }
}
