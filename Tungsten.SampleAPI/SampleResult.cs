using System;
using W.SampleAPI.Interface;

namespace W.SampleAPI
{
    public class SampleResult : ISampleResult
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
}