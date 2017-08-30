using System;
using System.Collections.Generic;
using System.Text;

namespace W.Threading
{
    /// <summary>
    /// The preferred level of CPU usage
    /// </summary>
    public enum CPUProfileEnum
    {
        /////<summary>High CPU usage, but fastest execution</summary>
        //HighCPU,
        /////<summary>Medium CPU usage, normal execution</summary>
        //NormalCPU,
        /////<summary>Low CPU usage, but also slowest execution</summary>
        //LowCPU

        ///<summary>High CPU usage, but fastest execution.  May be faster on single-core/cpu machines.  May be slower on multi-core/cpu machines.</summary>
        SpinWait0,
            ///<summary>Medium CPU usage.  Uses Thread.Sleep or Task.Delay to block the current thread.</summary>
        Sleep,
        ///<summary>Low CPU usage.  Should be faster on multi-core/cpu machines as the load will be divided among cores/cpus.  Slowest on single-core/cpu machines.</summary>
        SpinWait1

#if NET45
        ,
        ///<summary>Only available for .Net Framework; uses Thread.Yield instead of Thread.Sleep.</summary>
        Yield
#endif
    }
}
