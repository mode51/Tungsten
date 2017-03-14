using System;

namespace W.SampleAPI.Interface
{
    public interface ISampleResult
    {
        bool Success { get; set; }
        object Result { get; set; }
        Exception Exception { get; set; }
    }
}