using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// A base class for Thread which should work for all compiler Target types
    /// </summary>
    public abstract class ThreadBase : IDisposable
    {
        /// <summary>
        /// The Action to execute on the thread
        /// </summary>
        protected Action<CancellationToken> Action { get; set; }
        /// <summary>
        /// The Action to execute when the thread completes
        /// </summary>
        protected Action<bool, Exception> OnExit { get; set; }
        /// <summary>
        /// The CancellationTokenSource which can be used to cancel the thread
        /// </summary>
        protected CancellationTokenSource Cts { get; set; }
        /// <summary>
        /// Value is True if the thread is currently running, otherwise False
        /// </summary>
        protected Lockable<bool> IsBusy { get; set; } = new Lockable<bool>();
        /// <summary>
        /// The Value to send to the onExit Action.  True if the thread returns successfully, otherwise False.
        /// </summary>
        protected Lockable<bool> Success { get; set; } = new Lockable<bool>();

        /// <summary>
        /// Invokes the Action. Virtual for customization.
        /// </summary>
        /// <param name="token">The CancellationToken to pass into the thread procedure</param>
        protected virtual void InvokeAction(CancellationToken token)
        {
            try
            {
                IsBusy.Value = true;
                Action?.Invoke(token);
            }
#if NET45 || NETSTANDARD1_3 || NETCOREAPP1_0 || WINDOWS_UWP
            catch (System.MissingMethodException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
#endif
            finally
            {
                IsBusy.Value = false;
            }
        }
        /// <summary>
        /// Invokes the onExit action.  Virtual for customization.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void InvokeOnComplete(Exception e)
        {
            OnExit?.Invoke(Success.Value, e);
        }

        /// <summary>
        /// Must be overridden to provide exception handling
        /// </summary>
        /// <returns>An Exception object, if an exception occured</returns>
        protected virtual Exception CallInvokeAction()
        {
            Exception result = null;
            try
            {
                InvokeAction(Cts.Token);
                Success.Value = true;
            }
#if NET45
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
#endif
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
        protected virtual void CallInvokeOnComplete(Exception e)
        {
            try
            {
                InvokeOnComplete(e);
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
        /// The host thread procedure.  This method calls the Action and subsequent onExit.
        /// </summary>
        protected void ThreadProc()
        {
            //this method is protected so that it can be called directly by inheriters
            Exception ex = CallInvokeAction();
            CallInvokeOnComplete(ex);
        }

        /// <summary>
        /// <para>
        /// Cancels the thread by calling Cancel on the CancellationTokenSource.  The value should be checked in the code in the specified Action parameter.
        /// </para>
        /// </summary>
        public virtual void Cancel()
        {
            Cts.Cancel();
        }
        ///// <summary>
        ///// <para>
        ///// Cancels the thread by calling Cancel on the CancellationTokenSource.  The value should be checked in the code in the specified Action parameter.
        ///// </para>
        ///// </summary>
        ///// <param name="msForceAbortDelay">Abort the thread if it doesn't terminate before the specified number of milliseconds elapse</param>
        //public virtual void Cancel(int msForceAbortDelay)
        //{
        //    Cancel();

        //    if (!Join(msForceAbortDelay)) //give the thread 5 seconds to close down, otherwise force it
        //    {
        //        Cts?.Cancel(true);
        //    }
        //}

        /// <summary>
        /// True if the thread is running, otherwise false
        /// </summary>
        public bool IsRunning => IsBusy.Value;

        /// <summary>
        /// Blocks the calling thread until the thread terminates
        /// </summary>
        public abstract void Join();

        /// <summary>
        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
        public abstract bool Join(int msTimeout);

        /// <summary>
        /// Starts a new thread
        /// </summary>
        /// <param name="action">Action to call on a thread</param>
        /// <param name="onExit">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="token">A CancellationTokenSource to use instead of creating a new one</param>
        /// <returns></returns>
        protected ThreadBase(Action<CancellationToken> action, Action<bool, Exception> onExit = null, CancellationTokenSource token = null)
        {
            Cts = token ?? new CancellationTokenSource();
            Action = action;
            OnExit = onExit;
            //IsBusy.Value = true;
        }

        /// <summary>
        /// Destructs the ThreadBase object.  Calls Dispose.
        /// </summary>
        //~ThreadBase()
        //{
        //    Dispose();
        //}

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public virtual void Dispose()
        {
            Cancel();
            Cts.Dispose();
            //Cts = null;
        }

        //moved to each version of Thread (so that references would be Thread instead of ThreadBase
        ///// <summary>
        ///// Starts a new thread
        ///// </summary>
        ///// <param name="action">Action to call on a thread</param>
        ///// <param name="onExit">Action to call upon comletion.  Executes on the same thread as Action.</param>
        ///// <returns></returns>
        //public static Thread Create(Action<CancellationTokenSource> action, Action<bool, Exception> onExit = null)
        //{
        //    var result = new Thread(action, onExit);
        //    return result;
        //}
    }
}