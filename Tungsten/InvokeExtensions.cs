using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
#if WINDOWS_UWP
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
