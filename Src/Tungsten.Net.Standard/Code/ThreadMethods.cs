using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.Net
{
    /// <summary>
    /// Helper methods for multi-threading
    /// </summary>
    internal static class ThreadMethods
    {
//        /// <summary>
//        /// If True, ThreadMethods.Sleep method will use System.Threading.Thread.Sleep or Task.Delay to cause the thread to sleep.  This will make the code execute faster, but at the cost of CPU overhead.  If False, System.Threading.SpinWait.SpinUntil will be used instead.  This will make the code execute slower, but will consume less CPU.
//        /// </summary>
//        /// <remarks>Set this value to True if you expect your code to run on a single processor machine; Sleep is more efficient than a SpinWait when there is only one CPU/Core.</remarks>
//        public static bool UseSleepInsteadOfSpinWait { get; set; }

//        /// <summary>
//        /// Causes the active Thread or Task to release the CPU for the given number of milliseconds
//        /// </summary>
//        /// <param name="ms"></param>
//        public static void Sleep(int ms)
//        {
//            if (UseSleepInsteadOfSpinWait)
//#if NETSTANDARD1_3
//                W.Threading.Thread.Sleep(ms);
//#else
//                System.Threading.Thread.Yield();
//#endif
//            else
//#if NETSTANDARD1_3
//                System.Threading.SpinWait.SpinUntil(() => { return false; }, ms);
//#else
//                System.Threading.SpinWait.SpinUntil(() => { return false; }, ms); //this is slowest, but uses the least CPU
//                                                                                  ///W.Threading.Thread.Sleep(ms); //this is slower, but uses less CPU
//            //System.Threading.Thread.Yield(); //this is actually faster, but pegs the CPU
//#endif
//        }

//        /// <summary>
//        /// Attempts to free the CPU for other processes, based on the desired level.  Consequences will vary depending on your hardware architecture.  The more processors/cores you have, the better performance you will have by selecting LowCPU.  Likewise, on a single-core processor, you may wish to select HighCPU.
//        /// </summary>
//        /// <param name="level">The desired level of CPU usage</param>
//        /// <remarks>Note results may vary.  LowCPU will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture.  This may not always be the case.</remarks>
//        public static void Sleep(CPUProfileEnum level)
//        {
//            switch (level)
//            {
//                case CPUProfileEnum.HighCPU:
//#if NETSTANDARD1_3
//                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 0);
//#else
//                    System.Threading.Thread.Yield(); //seems fastest method on .Net Framework (pegs CPU)
//                    //W.Threading.Thread.Sleep(1); //this is definitely faster than SpinWait on .Net Framework 4.5 (and less CPU)
//#endif
//                    break;
//                case CPUProfileEnum.NormalCPU:
//                    //System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
//                    W.Threading.Thread.Sleep(1);
//                    break;
//                case CPUProfileEnum.LowCPU:
//                    System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
//                    break;
////                case CPUProfileEnum.Yield:
////#if NETSTANDARD1_3
////                    W.Threading.Thread.Sleep(1);
////                    //System.Threading.SpinWait.SpinUntil(() => { return false; }, 1);
////#else
////                    System.Threading.Thread.Yield();
////#endif
////                    break;
//            }
//        }
    }
}
