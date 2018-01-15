using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Threading;

namespace W.Threading.ThreadExtensions
{
    /// <summary>
    /// Extension methods related to threading
    /// </summary>
    public static class ThreadExtensions
    {
        //1.3.2018 - Use System.Threading.SpinWait instead of WaitForValue[Async]

        ///// <summary>
        ///// Creates a new Thread instance
        ///// </summary>
        ///// <typeparam name="TType">The Type of the item which will be passed into the threaded action</typeparam>
        ///// <param name="this">The item to be passed into the threaded action</param>
        ///// <param name="action">The action to be run on a new thread</param>
        ///// <returns>A handle to the created Thread object</returns>
        //public static Thread CreateThread<TType>(this TType @this, Action<CancellationToken, TType> action)
        //{
        //    var result = new Thread(token => action.Invoke(token, @this));
        //    return result;
        //}
        ///// <summary>
        ///// Creates a new ThreadSlim&lt;TType&gt; instance
        ///// </summary>
        ///// <typeparam name="TType">The Type of the item which will be passed into the threaded action</typeparam>
        ///// <param name="this">The item to be passed into the threaded action</param>
        ///// <param name="action">The action to be run on a new thread</param>
        ///// <returns>A handle to the created ThreadSlim object</returns>
        //public static ThreadSlim CreateThreadSlim<TType>(this TType @this, Action<CancellationToken, TType> action)
        //{
        //    var result = new ThreadSlim(token => action.Invoke(token, @this));
        //    return result;
        //}

        ///// <summary>
        ///// Initiates a Task which will wait for the given variable to have the specified value
        ///// </summary>
        ///// <param name="this">The value being inspected</param>
        ///// <param name="desiredValue">The value to wait for</param>
        ///// <param name="msTimeout">The task will time out within the specified number of milliseconds.  Use -1 to wait indefinitely.</param>
        ///// <returns>True if the value was acquired within the specified timeout, otherwise False</returns>
        //public static async Task<bool> WaitForValueAsync(this object @this, object desiredValue, int msTimeout = -1)
        //{
        //    System.Threading.CancellationTokenSource cts;
        //    if (msTimeout >= 0)
        //        cts = new System.Threading.CancellationTokenSource(msTimeout);
        //    else
        //        cts = new System.Threading.CancellationTokenSource();
        //    var result = await Task.Run(() =>
        //    {
        //        while (!cts.IsCancellationRequested)
        //        {
        //            if (@this == desiredValue)//.Equals(desiredValue))
        //            {
        //                return true;
        //            }
        //            W.Threading.Thread.Sleep(1);
        //        }
        //        return false;
        //    }, cts.Token).ContinueWith(task =>
        //    {
        //        //return (!task.IsCanceled);
        //        return (task.Result);
        //    });
        //    return result;
        //}
        ///// <summary>
        ///// Initiates a Task which will wait for the specified condition to be met
        ///// </summary>
        ///// <typeparam name="TItemType">The object Type of the item being extended</typeparam>
        ///// <param name="this">The value being inspected</param>
        ///// <param name="predicate">The condition to be met</param>
        ///// <param name="msTimeout">The task will time out within the specified number of milliseconds.  Use -1 to wait indefinitely.</param>
        ///// <returns>True if the condition was met within the specified timeout, otherwise False</returns>
        //public static async Task<bool> WaitForValueAsync<TItemType>(this TItemType @this, Predicate<TItemType> predicate, int msTimeout = -1)
        //{
        //    System.Threading.CancellationTokenSource cts;
        //    if (msTimeout >= 0)
        //        cts = new System.Threading.CancellationTokenSource(msTimeout);
        //    else
        //        cts = new System.Threading.CancellationTokenSource();
        //    var result = await Task.Run(() =>
        //    {
        //        while (!cts.IsCancellationRequested)
        //        {
        //            if (predicate.Invoke(@this))
        //            {
        //                return true;
        //            }
        //            W.Threading.Thread.Sleep(1);
        //        }
        //        return false;
        //    }, cts.Token).ContinueWith(task =>
        //    {
        //        //return (!task.IsCanceled);
        //        return (task.Result);
        //    });
        //    return result;
        //}
        ///// <summary>
        ///// Initiates a Task which will wait for the specified condition to be met
        ///// </summary>
        ///// <typeparam name="TItemType">The object Type of the item being extended</typeparam>
        ///// <param name="this">The value being inspected</param>
        ///// <param name="predicate">The condition to be met</param>
        ///// <param name="msTimeout">The task will time out within the specified number of milliseconds.  Use -1 to wait indefinitely.</param>
        ///// <returns>True if the condition was met within the specified timeout, otherwise False</returns>
        //public static bool WaitForValue<TItemType>(this TItemType @this, Predicate<TItemType> predicate, int msTimeout = -1)
        //{
        //    var result = false;
        //    var thread = new W.Threading.ThreadSlim(token =>
        //    {
        //        while (!token.IsCancellationRequested)
        //        {
        //            if (predicate.Invoke(@this))
        //            {
        //                result = true;
        //                break;
        //            }
        //            W.Threading.Thread.Sleep(1);
        //        }
        //    });
        //    thread.Start();
        //    thread.Wait(msTimeout);
        //    return result;
        //}

        //public static Task RunWhenSet(this ManualResetEventSlim mre, Action action)
        //{
        //    return Task.Factory.StartNew(() =>
        //    {
        //        mre.Wait();
        //        action.Invoke();
        //    }, TaskCreationOptions.LongRunning);
        //}
        //public static Task RunWhenSet(this ManualResetEventSlim mre, Action action, int msTimeout)
        //{
        //    return Task.Factory.StartNew(() =>
        //    {
        //        if (mre.Wait(msTimeout))
        //            action.Invoke();
        //    }, TaskCreationOptions.LongRunning);
        //}
        //public static Task RunWhenSet(this ManualResetEventSlim mre, Action action, CancellationToken token)
        //{
        //    return Task.Factory.StartNew(() =>
        //    {
        //        mre.Wait(token);
        //    }, TaskCreationOptions.LongRunning).ContinueWith(task =>
        //    {
        //        if (!token.IsCancellationRequested)
        //            action.Invoke();
        //    });
        //}

        //public static ThreadMethodSlim AsThreadMethodSlim(this Action action)
        //{
        //    var cts = new CancellationTokenSource();
        //    var mreRun = new ManualResetEventSlim(false);
        //    var mreComplete = new ManualResetEventSlim(false);

        //    var runTask = Task.Factory.StartNew(() =>
        //    {
        //        mreRun.Wait(cts.Token);
        //    }).ContinueWith(task =>
        //    {
        //        var ok = (!task.IsFaulted && !task.IsCanceled && !cts.Token.IsCancellationRequested);
        //        try
        //        {
        //            if (ok)
        //            {
        //                action.Invoke();
        //                mreComplete.Set(); //keep this here so it's not set if there was an unhandled exception
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            System.Diagnostics.Debugger.Break();
        //            System.Diagnostics.Debug.WriteLine(e.ToString());
        //        }
        //        finally
        //        {
        //        }
        //    });
        //    var cleanupTask = new Task(() =>
        //    {
        //        if (!mreRun.IsSet)
        //            cts.Cancel();
        //        mreRun.Dispose();
        //        cts.Dispose();
        //        mreComplete.Dispose();
        //        System.Diagnostics.Debug.WriteLine("ActionTask Destroyed");
        //    });
        //    //var task = Task.Factory.StartNew(() =>
        //    //{
        //    //    mreRun.Wait(cts.Token);
        //    //}, TaskCreationOptions.LongRunning).ContinueWith(thisTask =>
        //    //{
        //    //    try
        //    //    {
        //    //        if (!cts.Token.IsCancellationRequested)
        //    //        {
        //    //            action.Invoke();
        //    //            mreComplete.Set();
        //    //        }
        //    //    }
        //    //    catch (Exception e)
        //    //    {
        //    //        System.Diagnostics.Debugger.Break();
        //    //        System.Diagnostics.Debug.WriteLine(e.ToString());
        //    //    }
        //    //}).ContinueWith(task2 =>
        //    //{
        //    //    mreRun.Dispose();
        //    //    cts.Dispose();
        //    //    mreComplete.Dispose();
        //    //    mreTerminated.Set();
        //    //    mreTerminated.Dispose();
        //    //    //cts = null;
        //    //});
        //    return new ThreadMethodSlim(runTask, cleanupTask, mreRun, mreComplete);//, cts);
        //}
    }
}
