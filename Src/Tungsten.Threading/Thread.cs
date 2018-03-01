using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading
{
    /// <summary>
    /// Functionality related to multi-threading
    /// </summary>
    public class Thread //static methods
    {
        /// <summary>
        /// Blocks the calling thread for the specified time
        /// </summary>
        /// <param name="msDelay">The number of milliseconds to block the thread</param>
        public static void Sleep(int msDelay)
        {
            Sleep(msDelay, false);
        }
        /// <summary>
        /// Blocks the calling thread for the specified time
        /// </summary>
        /// <param name="msDelay">The number of milliseconds to block the thread</param>
        /// <param name="useSpinWait">If True, a SpinWait.SpinUntil will be used instead of a call to Thread.Sleep (or Task.Delay).  Note that SpinWait should only be used on multi-core/cpu machines.</param>
        public static void Sleep(int msDelay, bool useSpinWait)
        {
#if NET45
            try
            {
                if (useSpinWait)
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, msDelay);
                else
                    System.Threading.Thread.Sleep(msDelay);
            }
            catch (System.MissingMethodException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
#else
            if (useSpinWait)
                System.Threading.SpinWait.SpinUntil(() => { return false; }, msDelay);
            else
            {
                System.Threading.Tasks.Task.Delay(msDelay);
            }
#endif
        }
        /// <summary>
        /// Attempts to free the CPU for other processes, based on the desired level.  Consequences will vary depending on your hardware architecture.  The more processors/cores you have, the better performance you will have by selecting LowCPU.  Likewise, on a single-core processor, you may wish to select HighCPU.
        /// </summary>
        /// <param name="level">The desired level of CPU usage</param>
        /// <remarks>Note results may vary.  LowCPU will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture.  This may not always be the case.</remarks>
        public static void Sleep(CPUProfileEnum level)
        {
            switch (level)
            {
#if NET45
                case CPUProfileEnum.Yield:
                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
                    break;
#endif
                case CPUProfileEnum.SpinWait0:
                    //#if NET45
                    //                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
                    //#else
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 0);
                    //#endif
                    break;
                case CPUProfileEnum.Sleep:
                    Thread.Sleep(1);
                    break;
                case CPUProfileEnum.SpinWait1:
                case CPUProfileEnum.SpinUntil:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
                    break;
            }
        }

        /// <summary>
        /// Attempts to free the CPU for other processes, based on the desired level.  Consequences will vary depending on your hardware architecture.  The more processors/cores you have, the better performance you will have by selecting SpinWait1.  Likewise, on a single-core processor, you may wish to select SpinWait0.
        /// </summary>
        /// <param name="level">The desired level of CPU usage</param>
        /// <param name="msTimeout">Optional value for CPUProfileEnum.Sleep and CPUProfileEnum.SpinUntil. Ignored by other profiles.</param>
        /// <remarks>Note results may vary.  SpinWait1 will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture.  This may not always be the case.</remarks>
        public static void Sleep(CPUProfileEnum level, int msTimeout = 1)
        {
            switch (level)
            {
#if NET45
                case CPUProfileEnum.Yield:
                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
                    break;
#endif
                case CPUProfileEnum.SpinWait0:
                    //#if NET45
                    //                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
                    //#else
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 0);
                    //#endif
                    break;
                case CPUProfileEnum.Sleep:
                    Thread.Sleep(msTimeout);
                    break;
                case CPUProfileEnum.SpinWait1:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
                    break;
                case CPUProfileEnum.SpinUntil:
                    System.Threading.SpinWait.SpinUntil(() => { return false; }, msTimeout);
                    break;
            }
        }
    }
}
