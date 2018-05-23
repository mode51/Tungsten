using System;
using System.Collections.Generic;
using System.Text;

namespace W.IO.Pipes
{
    /// <summary>
    /// A CallResult tailored for W.IO.Pipes
    /// </summary>
    public class PipeResult : PipeResult<byte[]>
    {
    }
    /// <summary>
    /// A CallResult tailored for W.IO.Pipes with additional Pipe information
    /// </summary>
    public class PipeResult<T> : CallResult<T>
    {
        public PipeStatusEnum Status { get; internal set; }
    }
}
