using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    public static class InvokeExtensions
    {
#if WINDOWS_PORTABLE
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
#elif WINDOWS_UWP
        public static void InvokeEx<T>(this T @this, Action action)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke()).GetResults();
        }
        public static async Task InvokeAsync<T>(this T @this, Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke());
        }
#else
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
