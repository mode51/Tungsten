using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// <para>
    /// A Gated thread.  Execution of the Action will proceed when the Run method is called.
    /// </para>
    /// </summary>
    public class Gate : W.Threading.Thread
    {
        private AutoResetEvent _are = new AutoResetEvent(false);
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private ManualResetEventSlim _mreComplete = new ManualResetEventSlim(false);

        /// <summary>
        /// <para>
        /// Used to wrap the call to InvokeAction with try/catch handlers.  This method should call InvokeAction.
        /// </para>
        /// </summary>
        /// <returns>An Exception if on occurs, otherwise null</returns>
        protected override Exception CallInvokeAction()
        {
            Exception ex = null;
            try
            {
                while ((!Cts?.IsCancellationRequested ?? false))// && (!_cts?.IsCancellationRequested ?? false))
                {
                    if (_are?.WaitOne(10) ?? false) //just release the thread periodically to check our CancellationTokenSources
                    {
                        try
                        {
                            InvokeAction(_cts);
                            //Console.WriteLine("Action Complete");
                        }
                        //catch (AggregateException e)
                        //{
                        //    //ignore 
                        //}
                        //catch (TaskCanceledException e)
                        //{
                        //    //ignore - this particular run was cancelled
                        //}
                        //catch (OperationCanceledException e)
                        //{
                        //    //ignore - this particular run was cancelled via CancellationTokenSource
                        //}
                        catch (Exception e)
                        {
                            //ignore
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                        finally
                        {
                            _mreComplete?.Set();
                        }
                    }
                }
            }
            catch (TaskSchedulerException e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
            }
            catch (TaskCanceledException e)
            {
                ex = e;
            }
            catch (Exception e)
            {
                ex = e;
            }
            return ex;
        }
        /// <summary>
        /// Allows the Action to be called
        /// </summary>
        public void Run()
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            _mreComplete.Reset();
            _are.Set();
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates
        /// </summary>
        public override void Join()
        {
            _mreComplete.Wait();
        }

        /// <summary>
        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
        public override bool Join(int msTimeout)
        {
            return _mreComplete.Wait(msTimeout);
        }

        /// <summary>
        /// Signals the task to cancel
        /// </summary>
        public new void Cancel()
        {
            _cts?.Cancel();
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose()
        {
            base.Cancel(); //to cancel Cts
            base.Dispose();
            Cancel();
            _are?.Dispose();
            _are = null;
            _mreComplete?.Dispose();
            _mreComplete = null;
            _cts?.Dispose();
            _cts = null;
        }

        /// <summary>
        /// Construct a Gate
        /// </summary>
        /// <param name="action">The code to execute in a background task, when signaled</param>
        /// <param name="onExit">Called after the </param>
        public Gate(Action<CancellationTokenSource> action, Action<bool, Exception> onExit = null) : base(action, onExit)
        {
        }
    }

    /// <summary>
    /// <para>
    /// A Gated thread.  Execution of the Action will proceed when the Run method is called.
    /// </para>
    /// </summary>
    public class Gate<T> : W.Threading.Thread<T>
    {
        private AutoResetEvent Event { get; } = new AutoResetEvent(false);
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private ManualResetEvent _mreComplete = new ManualResetEvent(false);

        /// <summary>
        /// <para>
        /// Used to wrap the call to InvokeAction with try/catch handlers.  This method should call InvokeAction.
        /// </para>
        /// </summary>
        /// <returns>An Exception if on occurs, otherwise null</returns>
        protected override Exception CallInvokeAction()
        {
            Exception ex = null;
            try
            {
                while (!Cts?.IsCancellationRequested ?? false)
                {
                    if (Event?.WaitOne(1000) ?? false) //just release the thread periodically
                    {
                        try
                        {
                            InvokeAction(_cts);
                        }
                        catch (Exception e)
                        {
                            //ignore
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                        finally
                        {
                            _mreComplete.Set();
                        }
                    }
                }
            }
            catch (TaskSchedulerException e)
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine("ThreadExtensions.ThreadProc TaskSchedulerException:  " + e.Message);
            }
            catch (TaskCanceledException e)
            {
                ex = e;
            }
            catch (Exception e)
            {
                ex = e;
            }
            return ex;
        }
        /// <summary>
        /// Allows the Action to be called
        /// </summary>
        public void Run()
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            _mreComplete.Reset();
            Event.Set();
        }
        /// <summary>
        /// Blocks the calling thread until the thread terminates
        /// </summary>
        public override void Join()
        {
            _mreComplete?.WaitOne();
        }

        /// <summary>
        /// Blocks the calling thread until either the thread terminates or the specified milliseconds elapse
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the thread to terminate</param>
        /// <returns>True if the thread terminates within the timeout specified, otherwise false</returns>
        public override bool Join(int msTimeout)
        {
            return _mreComplete?.WaitOne(msTimeout) ?? true;
        }
        /// <summary>
        /// Signals the task to cancel
        /// </summary>
        public new void Cancel()
        {
            _cts?.Cancel(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose()
        {
            Cancel(); //to cancel _cts
            _mreComplete?.Dispose();
            _mreComplete = null;
            _cts?.Dispose();
            _cts = null;
            base.Cancel(); //to cancel Cts
            base.Dispose();
        }

        /// <summary>
        /// Construct a Gate
        /// </summary>
        /// <param name="action">The action to execute in a background task</param>
        /// <param name="onExit">Called when the task completes</param>
        /// <param name="args"></param>
        public Gate(Action<T, CancellationTokenSource> action, Action<bool, Exception> onExit = null, T args = default(T)) 
            : base(action, onExit, args)
        {
        }
    }
}