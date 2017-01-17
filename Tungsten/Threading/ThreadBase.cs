using System;
using System.Threading;

namespace W.Threading
{
    /// <summary>
    /// A base class for Thread which should work for all compiler Target types
    /// </summary>
    public abstract class ThreadBase
    {
        protected Action<CancellationTokenSource> Action { get; set; }
        protected Action<bool, Exception> OnComplete { get; set; }
        protected CancellationTokenSource Cts { get; set; }
        protected Lockable<bool> IsBusy { get; set; } = new Lockable<bool>();
        protected Lockable<bool> Success { get; set; } = new Lockable<bool>();

        protected virtual void InvokeAction()
        {
            Action?.Invoke(Cts);
        }
        protected virtual void InvokeOnComplete(Exception e)
        {
            OnComplete?.Invoke(Success.Value, e);
        }

        protected abstract Exception CallInvokeAction();

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
        protected void ThreadProc()
        {
            Exception ex = CallInvokeAction();
            CallInvokeOnComplete(ex);
        }

        /// <summary>
        /// <para>
        /// Cancels the thread by calling Cancel on the CancellationTokenSource.  The value should be checked in the code in the specified Action parameter.
        /// </para>
        /// </summary>
        public void Cancel()
        {
            Cts.Cancel();
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
        /// <returns></returns>
        protected ThreadBase(Action<CancellationTokenSource> action, Action<bool, Exception> onComplete = null)
        {
            Cts = new CancellationTokenSource();
            Action = action;
            OnComplete = onComplete;
            Cts = new CancellationTokenSource();
            IsBusy.Value = true;
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