using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W
{
    ///// <summary>
    ///// The template delegate
    ///// </summary>
    ///// <param name="sender">The object which raised this event</param>
    ///// <param name="callerMemberName">The name of the method which raised the event</param>
    ///// <param name="args">An array of untyped arguments</param>
    //public delegate void EventTemplateDelegate(object sender, string callerMemberName, params object[] args);
    ///// <summary>
    ///// The template delegate
    ///// </summary>
    ///// <param name="sender">The object which raised this event</param>
    ///// <param name="arg">The detailed event argument</param>
    ///// <param name="callerMemberName">The name of the method which raised the event</param>
    //public delegate void EventTemplateDelegate<TEventArg>(object sender, TEventArg arg, string callerMemberName);
    ///// <summary>
    ///// The template delegate
    ///// </summary>
    ///// <param name="sender">The object which raised this event</param>
    ///// <param name="arg1">The first detailed event argument</param>
    ///// <param name="arg2">The second detailed event argument</param>
    ///// <param name="callerMemberName">The name of the method which raised the event</param>
    //public delegate void EventTemplateDelegate<TEventArg1, TEventArg2>(object sender, TEventArg1 arg1, TEventArg2 arg2, string callerMemberName);
    ///// <summary>
    ///// The template delegate
    ///// </summary>
    ///// <param name="sender">The object which raised this event</param>
    ///// <param name="arg1">The first detailed event argument</param>
    ///// <param name="arg2">The second detailed event argument</param>
    ///// <param name="arg3">The third detailed event argument</param>
    ///// <param name="callerMemberName">The name of the method which raised the event</param>
    //public delegate void EventTemplateDelegate<TEventArg1, TEventArg2, TEventArg3>(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, string callerMemberName);
    ///// <summary>
    ///// The template delegate
    ///// </summary>
    ///// <param name="sender">The object which raised this event</param>
    ///// <param name="arg1">The first detailed event argument</param>
    ///// <param name="arg2">The second detailed event argument</param>
    ///// <param name="arg3">The third detailed event argument</param>
    ///// <param name="arg4">The fourth detailed event argument</param>
    ///// <param name="callerMemberName">The name of the method which raised the event</param>
    //public delegate void EventTemplateDelegate<TEventArg1, TEventArg2, TEventArg3, TEventArg4>(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, string callerMemberName);
    ///// <summary>
    ///// The template delegate
    ///// </summary>
    ///// <param name="sender">The object which raised this event</param>
    ///// <param name="arg1">The first detailed event argument</param>
    ///// <param name="arg2">The second detailed event argument</param>
    ///// <param name="arg3">The third detailed event argument</param>
    ///// <param name="arg4">The fourth detailed event argument</param>
    ///// <param name="arg5">The fifth detailed event argument</param>
    ///// <param name="callerMemberName">The name of the method which raised the event</param>
    //public delegate void EventTemplateDelegate<TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, TEventArg5 arg5, string callerMemberName);

    ///// <summary>
    ///// Wraps the functionality of delegate, event and RaiseXXX into a single class
    ///// </summary>
    //public class EventTemplate
    //{
    //    /// <summary>
    //    /// The template event
    //    /// </summary>
    //    public event EventTemplateDelegate OnRaised;
    //    /// <summary>
    //    /// Raises the template event
    //    /// </summary>
    //    /// <param name="sender">The object which raised this event</param>
    //    /// <param name="callerMemberName">The name of the method which raised the event</param>
    //    /// <param name="args">An array of untyped arguments</param>
    //    public void Raise(object sender, string callerMemberName = "", params object[] args)
    //    {
    //        var evt = OnRaised;
    //        evt?.Invoke(sender, callerMemberName, args);
    //    }
    //}
    ///// <summary>
    ///// Wraps the functionality of delegate, event and RaiseXXX into a single class
    ///// </summary>
    ///// <typeparam name="TEventArg">The detailed event argument</typeparam>
    //public class EventTemplate<TEventArg>
    //{
    //    /// <summary>
    //    /// The template event
    //    /// </summary>
    //    public event EventTemplateDelegate<TEventArg> OnRaised;
    //    /// <summary>
    //    /// Raises the template event
    //    /// </summary>
    //    /// <param name="sender">The object which raised this event</param>
    //    /// <param name="arg">The detailed event argument</param>
    //    /// <param name="callerMemberName">The name of the method which raised the event</param>
    //    public void Raise(object sender, TEventArg arg, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName="")
    //    {
    //        var evt = OnRaised;
    //        evt?.Invoke(sender, arg, callerMemberName);
    //    }
    //}
    ///// <summary>
    ///// Wraps the functionality of delegate, event and RaiseXXX into a single class
    ///// </summary>
    ///// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    ///// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    //public class EventTemplate<TEventArg1, TEventArg2>
    //{
    //    /// <summary>
    //    /// The template event
    //    /// </summary>
    //    public event EventTemplateDelegate<TEventArg1, TEventArg2> OnRaised;
    //    /// <summary>
    //    /// Raises the template event
    //    /// </summary>
    //    /// <param name="sender">The object which raised this event</param>
    //    /// <param name="arg1">The first detailed event argument</param>
    //    /// <param name="arg2">The second detailed event argument</param>
    //    /// <param name="callerMemberName">The name of the method which raised the event</param>
    //    public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
    //    {
    //        var evt = OnRaised;
    //        evt?.Invoke(sender, arg1, arg2, callerMemberName);
    //    }
    //}
    ///// <summary>
    ///// Wraps the functionality of delegate, event and RaiseXXX into a single class
    ///// </summary>
    ///// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    ///// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    ///// <typeparam name="TEventArg3">The third detailed event argument</typeparam>
    //public class EventTemplate<TEventArg1, TEventArg2, TEventArg3>
    //{
    //    /// <summary>
    //    /// The template event
    //    /// </summary>
    //    public event EventTemplateDelegate<TEventArg1, TEventArg2, TEventArg3> OnRaised;
    //    /// <summary>
    //    /// Raises the template event
    //    /// </summary>
    //    /// <param name="sender">The object which raised this event</param>
    //    /// <param name="arg1">The first detailed event argument</param>
    //    /// <param name="arg2">The second detailed event argument</param>
    //    /// <param name="arg3">The third detailed event argument</param>
    //    /// <param name="callerMemberName">The name of the method which raised the event</param>
    //    public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
    //    {
    //        var evt = OnRaised;
    //        evt?.Invoke(sender, arg1, arg2, arg3, callerMemberName);
    //    }
    //}
    ///// <summary>
    ///// Wraps the functionality of delegate, event and RaiseXXX into a single class
    ///// </summary>
    ///// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    ///// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    ///// <typeparam name="TEventArg3">The third detailed event argument</typeparam>
    ///// <typeparam name="TEventArg4">The fourth detailed event argument</typeparam>
    //public class EventTemplate<TEventArg1, TEventArg2, TEventArg3, TEventArg4>
    //{
    //    /// <summary>
    //    /// The template event
    //    /// </summary>
    //    public event EventTemplateDelegate<TEventArg1, TEventArg2, TEventArg3, TEventArg4> OnRaised;
    //    /// <summary>
    //    /// Raises the template event
    //    /// </summary>
    //    /// <param name="sender">The object which raised this event</param>
    //    /// <param name="arg1">The first detailed event argument</param>
    //    /// <param name="arg2">The second detailed event argument</param>
    //    /// <param name="arg3">The third detailed event argument</param>
    //    /// <param name="arg4">The fourth detailed event argument</param>
    //    /// <param name="callerMemberName">The name of the method which raised the event</param>
    //    public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
    //    {
    //        var evt = OnRaised;
    //        evt?.Invoke(sender, arg1, arg2, arg3, arg4, callerMemberName);
    //    }
    //}
    ///// <summary>
    ///// Wraps the functionality of delegate, event and RaiseXXX into a single class
    ///// </summary>
    ///// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    ///// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    ///// <typeparam name="TEventArg3">The third detailed event argument</typeparam>
    ///// <typeparam name="TEventArg4">The fourth detailed event argument</typeparam>
    ///// <typeparam name="TEventArg5">The fifth detailed event argument</typeparam>
    //public class EventTemplate<TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>
    //{
    //    /// <summary>
    //    /// The template event
    //    /// </summary>
    //    public event EventTemplateDelegate<TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5> OnRaised;
    //    /// <summary>
    //    /// Raises the template event
    //    /// </summary>
    //    /// <param name="sender">The object which raised this event</param>
    //    /// <param name="arg1">The first detailed event argument</param>
    //    /// <param name="arg2">The second detailed event argument</param>
    //    /// <param name="arg3">The third detailed event argument</param>
    //    /// <param name="arg4">The fourth detailed event argument</param>
    //    /// <param name="arg5">The fifth detailed event argument</param>
    //    /// <param name="callerMemberName">The name of the method which raised the event</param>
    //    public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, TEventArg5 arg5, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
    //    {
    //        var evt = OnRaised;
    //        evt?.Invoke(sender, arg1, arg2, arg3, arg4, arg5, callerMemberName);
    //    }
    //}
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TSender">The object raising the event </typeparam>
    public class EventTemplate<TSender>
    {
        /// <summary>
        /// The template event
        /// </summary>
        public event Action<TSender> OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        public void Raise(TSender sender)
        {
            OnRaised?.Invoke(sender);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TSender">The object raising the event </typeparam>
    /// <typeparam name="Arg1">The first argument</typeparam>
    public class EventTemplate<TSender, Arg1>
    {
        /// <summary>
        /// The template event
        /// </summary>
        public event Action<TSender, Arg1> OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first argument</param>
        public void Raise(TSender sender, Arg1 arg1)
        {
            OnRaised?.Invoke(sender, arg1);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TSender">The object raising the event </typeparam>
    /// <typeparam name="Arg1">The first argument</typeparam>
    /// <typeparam name="Arg2">The second argument</typeparam>
    public class EventTemplate<TSender, Arg1, Arg2>
    {
        /// <summary>
        /// The template event
        /// </summary>
        public event Action<TSender, Arg1, Arg2> OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first argument</param>
        /// <param name="arg2">The second argument</param>
        public void Raise(TSender sender, Arg1 arg1, Arg2 arg2)
        {
            OnRaised?.Invoke(sender, arg1, arg2);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TSender">The object raising the event </typeparam>
    /// <typeparam name="Arg1">The first argument</typeparam>
    /// <typeparam name="Arg2">The second argument</typeparam>
    /// <typeparam name="Arg3">The third argument</typeparam>
    public class EventTemplate<TSender, Arg1, Arg2, Arg3>
    {
        /// <summary>
        /// The template event
        /// </summary>
        public event Action<TSender, Arg1, Arg2, Arg3> OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first argument</param>
        /// <param name="arg2">The second argument</param>
        /// <param name="arg3">The third argument</param>
        public void Raise(TSender sender, Arg1 arg1, Arg2 arg2, Arg3 arg3)
        {
            OnRaised?.Invoke(sender, arg1, arg2, arg3);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TSender">The object raising the event </typeparam>
    /// <typeparam name="Arg1">The first argument</typeparam>
    /// <typeparam name="Arg2">The second argument</typeparam>
    /// <typeparam name="Arg3">The third argument</typeparam>
    /// <typeparam name="Arg4">The fourth argument</typeparam>
    public class EventTemplate<TSender, Arg1, Arg2, Arg3, Arg4>
    {
        /// <summary>
        /// The template event
        /// </summary>
        public event Action<TSender, Arg1, Arg2, Arg3, Arg4> OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first argument</param>
        /// <param name="arg2">The second argument</param>
        /// <param name="arg3">The third argument</param>
        /// <param name="arg4">The fourth argument</param>
        public void Raise(TSender sender, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4)
        {
            OnRaised?.Invoke(sender, arg1, arg2, arg3, arg4);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TSender">The object raising the event </typeparam>
    /// <typeparam name="Arg1">The first argument</typeparam>
    /// <typeparam name="Arg2">The second argument</typeparam>
    /// <typeparam name="Arg3">The third argument</typeparam>
    /// <typeparam name="Arg4">The fourth argument</typeparam>
    /// <typeparam name="Arg5">The fifth argument</typeparam>
    public class EventTemplate<TSender, Arg1, Arg2, Arg3, Arg4, Arg5>
    {
        /// <summary>
        /// The template event
        /// </summary>
        public event Action<TSender, Arg1, Arg2, Arg3, Arg4, Arg5> OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first argument</param>
        /// <param name="arg2">The second argument</param>
        /// <param name="arg3">The third argument</param>
        /// <param name="arg4">The fourth argument</param>
        /// <param name="arg5">The fifth argument</param>
        public void Raise(TSender sender, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4, Arg5 arg5)
        {
            OnRaised?.Invoke(sender, arg1, arg2, arg3, arg4, arg5);
        }
    }
}
