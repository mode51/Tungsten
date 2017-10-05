using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W
{
    public class EventTemplate<TArg>
    {
        public delegate void EventDelegate(object sender, TArg args, string callerMemberName);
        public event EventDelegate OnRaised;

        public void Raise(object sender, TArg args, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName="")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, args, callerMemberName);
        }
    }
    public class EventTemplate<TArg1, TArg2>
    {
        public delegate void EventDelegate(object sender, TArg1 arg1, TArg2 arg2, string callerMemberName);
        public event EventDelegate OnRaised;

        public void Raise(object sender, TArg1 arg1, TArg2 arg2, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, callerMemberName);
        }
    }
    public class EventTemplate<TArg1, TArg2, TArg3>
    {
        public delegate void EventDelegate(object sender, TArg1 arg1, TArg2 arg2, TArg3 arg3, string callerMemberName);
        public event EventDelegate OnRaised;

        public void Raise(object sender, TArg1 arg1, TArg2 arg2, TArg3 arg3, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, arg3, callerMemberName);
        }
    }
    public class EventTemplate<TArg1, TArg2, TArg3, TArg4>
    {
        public delegate void EventDelegate(object sender, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, string callerMemberName);
        public event EventDelegate OnRaised;

        public void Raise(object sender, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, arg3, arg4, callerMemberName);
        }
    }
    public class EventTemplate<TArg1, TArg2, TArg3, TArg4, TArg5>
    {
        public delegate void EventDelegate(object sender, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, string callerMemberName);
        public event EventDelegate OnRaised;

        public void Raise(object sender, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, arg3, arg4, arg5, callerMemberName);
        }
    }
}
