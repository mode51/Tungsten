using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Core
{
    /// <summary>
    /// Extension method for Dispatcher
    /// </summary>
    public static class InvokeExtensions
    {
        /// <summary>
        /// Invokes the action on the dispatcher, avoiding the cross-thread exception
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void InvokeEx(this System.Windows.Threading.Dispatcher @this, Action action)
        {
            //usage:  this.InvokeEx(f => pbProgress.Value = 0);
            if (!@this.CheckAccess())
            {
#if DEBUG
                try
                {
#endif
                @this.Invoke(action);//, new object[] { @this });
#if DEBUG
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //System.Diagnostics.Debugger.Break();
                }
#endif
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
