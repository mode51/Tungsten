using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Extension methods for delegates
/// </summary>
namespace W.DelegateExtensions
{
    /// <summary>
    /// Extension methods for delegates
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// Calls the delegate, passing in any arguments.  Provides error handling to allow all subscribers to handle the delegate.
        /// </summary>
        /// <param name="del">The delegate to call</param>
        /// <param name="args">Parameters to pass into the delegate</param>
        /// <returns>Exceptions if any are thrown</returns>
        public static List<Exception> Raise(this Delegate del, params object[] args)
        {
            var result = new List<Exception>();
            var evt = del; //not sure if this is necessary, but it can't hurt
                           //try
                           //{
            foreach (var handler in evt?.GetInvocationList())
            {
                Exception ex = null;
                try
                {
                    if (args != null)
                        handler.DynamicInvoke(args);
                    else
                        handler.DynamicInvoke();
                }
                catch (Exception e)
                {
                    ex = e;
                }
                if (ex != null)
                {
                    ex = new Exception("Exception in " + handler.Target?.GetType().Name, ex);
                    //Debug.e(ex);
                    System.Diagnostics.Debugger.Break();
                    result.Add(ex);
                }
            }
            //            }
            //#if NET45
            //            catch (System.Threading.ThreadAbortException e)
            //            {
            //                System.Threading.Thread.ResetAbort();
            //            }
            //#else
            //            catch (ObjectDisposedException)
            //            {
            //                //ignore it, the task is shutting down forcefully
            //            }
            //            catch (AggregateException)
            //            {
            //                //ignore it, the task might be shutting down forcefully
            //            }
            //#endif
            return result;
        }
    }
}
