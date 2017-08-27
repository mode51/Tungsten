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
        ///<summary>High CPU usage, but fastest execution</summary>
        HighCPU,
        ///<summary>Medium CPU usage, normal execution</summary>
        NormalCPU,
        ///<summary>Low CPU usage, but also slowest execution</summary>
        LowCPU
        /////<summary>Only available for .Net Framework; uses Thread.Yield instead of Thread.Sleep</summary>
        //Yield    
    }
}
