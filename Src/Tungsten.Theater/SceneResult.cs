using System;
using System.Collections.Generic;
using System.Threading;

namespace W.Threater
{
    public sealed class SceneResult<TTheater>
    {
        public Scene<TTheater> Scene { get; internal set; }
        public bool Success { get; internal set; }
        public List<object> Results { get; } = new List<object>();
        public T Result<T>(int index) { return (T)Results[index]; }
        public Exception Exception { get; internal set; }
        public ManualResetEventSlim IsComplete { get; } = new ManualResetEventSlim(false);
        ~SceneResult()
        {
            IsComplete.Dispose();
        }
    }
}
