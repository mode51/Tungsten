using System;
using System.Threading;

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
        protected Action<CancellationTokenSource> Action { get; set; }
        /// <summary>
        /// The Action to execute when the thread completes
        /// </summary>
        protected Action<bool, Exception> OnComplete { get; set; }
        /// <summary>
        /// The CancellationTokenSource which can be used to cancel the thread
        /// </summary>
        protected CancellationTokenSource Cts { get; set; }
        /// <summary>
        /// Value is True if the thread is currently running, otherwise False
        /// </summary>
        protected Lockable<bool> IsBusy { get; set; } = new Lockable<bool>();
        /// <summary>
        /// The Value to send to the OnComplete Action.  True if the thread returns successfully, otherwise False.
        /// </summary>
        protected Lockable<bool> Success { get; set; } = new Lockable<bool>();

        /// <summary>
        /// Invokes the Action. Virtual for customization.
        /// </summary>
        protected virtual void InvokeAction()
        {
            Action?.Invoke(Cts);
        }
        /// <summary>
        /// Invokes the OnComplete action.  Virtual for customization.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void InvokeOnComplete(Exception e)
        {
            OnComplete?.Invoke(Success.Value, e);
        }

        /// <summary>
        /// Must be overridden to provide exception handling
        /// </summary>
        /// <returns>An Exception object, if an exception ocurred</returns>
        protected abstract Exception CallInvokeAction();

        /// <summary>
        /// Calls the OnComplete Action when the thread returns
        /// </summary>
        /// <param name="e">An Exception object, if an exception ocurred</param>
        protected virtual void CallInvokeOnComplete(Exception e)
        {
            try
            {
                IsBusy.Value = false;
                InvokeOnComplete(e);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc.OnComplete Exception:  " + ex.Message);
            }
        }
        /// <summary>
        /// The host thread procedure.  This method calls the Action and subsequent OnComplete.
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
            Cts?.Cancel();
            Cts = null;
        }

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
        /// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        /// <param name="cts">A CancellationTokenSource which can be used to Cancel the thread</param>
        /// <returns></returns>
        protected ThreadBase(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null, CancellationTokenSource cts = null)
        {
            Cts = cts ?? new CancellationTokenSource();
            Action = action;
            OnComplete = onComplete;
            IsBusy.Value = true;
        }

        /// <summary>
        /// Destructs the ThreadBase object.  Calls Dispose.
        /// </summary>
        ~ThreadBase()
        {
            Dispose();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public virtual void Dispose()
        {
            Cancel();
        }

        //moved to each version of Thread (so that references would be Thread instead of ThreadBase
        ///// <summary>
        ///// Starts a new thread
        ///// </summary>
        ///// <param name="action">Action to call on a thread</param>
        ///// <param name="onComplete">Action to call upon comletion.  Executes on the same thread as Action.</param>
        ///// <returns></returns>
        //public static Thread Create(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null)
        //{
        //    var result = new Thread(action, onComplete);
        //    return result;
        //}
    }
}