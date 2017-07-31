using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#endif

namespace W
{
    //1.12.2017 - I can't remember where I got the original code/inspiration for these
    //There seem to be a lot of variants now too, but here's a good reference link:
    //http://stackoverflow.com/questions/711408/best-way-to-invoke-any-cross-threaded-code
    /// <summary>
    /// Extension methods to provide code shortcuts to evaluate InvokeRequired and run code appropriately
    /// </summary>
    public static class InvokeExtensions
    {
#if WINDOWS_UWP
        /// <summary>
        /// Runs the provided Action on the UI thread
        /// </summary>
        /// <param name="this">The form or control which supports Dispatcher</param>
        /// <param name="action">The code to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        public static void InvokeEx<T>(this T @this, Action action)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke()).GetResults();
        }
        /// <summary>
        /// Asynchronously runs the provided Action on the UI thread
        /// </summary>
        /// <param name="this">The form or control which supports Dispatcher</param>
        /// <param name="action">The code to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        public static async Task InvokeAsync<T>(this T @this, Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke());
        }
#elif (WINDOWS_PORTABLE || NETCOREAPP1_0 || NETCOREAPP1_1)
        /// <summary>
        /// Runs the provided Action on the UI thread
        /// </summary>
        /// <param name="context">The form or control which supports ISynchronizeInvoke</param>
        /// <param name="action">The code to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        public static void InvokeEx<T>(this SynchronizationContext context, Action action)
        {
            //1.14.2017 - from http://stackoverflow.com/questions/11258164/portable-class-library-equivalent-of-dispatcher-invoke-or-dispatcher-runasync
            if (SynchronizationContext.Current == context)
            {
                action?.Invoke();
            }
            else
            {
                context.Send(state => action?.Invoke(), null); // send = synchronously
                // context.Post(action)  // - post is asynchronous
            }
        }
        /// <summary>
        /// Asynchronously runs the provided Action on the UI thread
        /// </summary>
        /// <param name="context">The form or control which supports ISynchronizeInvoke</param>
        /// <param name="action">The code to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        public static void InvokeExAsync<T>(this SynchronizationContext context, Action action)
        {
            //1.14.2017 - from http://stackoverflow.com/questions/11258164/portable-class-library-equivalent-of-dispatcher-invoke-or-dispatcher-runasync
            if (SynchronizationContext.Current == context)
            {
                action?.Invoke();
            }
            else
            {
                //context.Send(state => action?.Invoke(), null);
                // send = synchronously
                context.Post(state => action?.Invoke(), null); // - post is asynchronous
            }
        }
        /// <summary>
        /// Creates a Task to run the provided Action on the UI thread.  Can be awaited.
        /// </summary>
        /// <param name="context">The form or control which supports ISynchronizeInvoke</param>
        /// <param name="action">The code to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        public static Task InvokeAsync<T>(this SynchronizationContext context, Action action)
        {
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();

            // Run the action asyncronously. The Send method can be used to run syncronously.
            context.Post((obj) =>
            {
                try
                {
                    action();
                    taskCompletionSource.SetResult(null);
                }
                catch (Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            },
            null);
            return taskCompletionSource.Task;
        }
#else
        /// <summary>
        /// Runs the provided Action on the UI thread
        /// </summary>
        /// <param name="this">The form or control which supports ISynchronizeInvoke</param>
        /// <param name="action">The code to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        public static void InvokeEx<T>(this T @this, Action<T> action) where T : ISynchronizeInvoke
        {
            //use:  this.InvokeEx(f => pbProgress.Value = 0);
            if (@this.InvokeRequired)
            {
                @this.Invoke(action, new object[] { @this });
            }
            else
            {
                action(@this);
            }
        }
        /// <summary>
        /// Runs the provided Function on the UI thread. Avoids the cross-threaded exceptions.
        /// </summary>
        /// <param name="this">The form or control which supports ISynchronizationInvoke</param>
        /// <param name="f">The function to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        /// <typeparam name="U">The type of return value</typeparam>
        /// <returns>The function should return an object of type U</returns>
        public static U InvokeEx<T, U>(this T @this, Func<T, U> f) where T : ISynchronizeInvoke
        {
            //use:  this.InvokeEx((o) => lstProjects.SelectionItem as string);
            if (@this.InvokeRequired)
            {
                return (U)@this.Invoke(f, new object[] { @this });
            }
            else
            {
                return (U)f(@this);
            }
        }
        /// <summary>
        /// Runs the provided Function on the UI thread. Avoids the cross-threaded exceptions.
        /// </summary>
        /// <param name="this">The form or control which supports ISynchronizationInvoke</param>
        /// <param name="f">The function to be executed on the UI thread</param>
        /// <typeparam name="T">The form or control who's thread will execute the code</typeparam>
        /// <returns>The function should return an object</returns>
        public static object InvokeEx<T>(this T @this, Func<T, object> f) where T : ISynchronizeInvoke
        {
            //use:  this.InvokeEx((o) => lstProjects.SelectionItem as string);
            if (@this.InvokeRequired)
            {
                return @this.Invoke(f, new object[] { @this });
            }
            else
            {
                return f(@this);
            }
        }
#endif
    }
}
