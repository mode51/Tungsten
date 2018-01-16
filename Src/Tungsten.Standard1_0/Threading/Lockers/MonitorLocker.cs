using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.LockExtensions;

namespace W.Threading.Lockers
{
    /// <summary>
    /// Uses Monitor to provide resource locking
    /// </summary>
    /// <remarks>Can be overridden to provide additional functionality</remarks>
    public class MonitorLocker : ILocker<object>
    {
        /// <summary>
        /// The object used to perform locks
        /// </summary>
        public object Locker { get; } = new object();

        ///// <summary>
        ///// Locks the resource
        ///// </summary>
        ///// <returns>Returns True</returns>
        //protected bool Lock()
        //{
        //    Monitor.Enter(Locker);
        //    return true;
        //}
        ///// <summary>
        ///// Unlock the resource
        ///// </summary>
        //protected void Unlock()
        //{
        //    Monitor.Exit(Locker);
        //}

        /// <summary>
        /// Executes an action from within a Monitor
        /// </summary>
        /// <param name="action">The action to run</param>
        public void InLock(Action action)
        {
            Locker.InLock(action);
        }
        /// <summary>
        /// Executes a function from within a Monitor
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public TValue InLock<TValue>(Func<TValue> func)
        {
            return Locker.InLock(func);
        }

        /// <summary>
        /// Executes an action from within a Monitor
        /// </summary>
        /// <param name="action">The action to run</param>
        public async Task InLockAsync(Action action)
        {
            await Locker.InLockAsync(action);
        }
        /// <summary>
        /// Executes a function from within a Monitor
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public async Task<TValue> InLockAsync<TValue>(Func<TValue> func)
        {
            return await Locker.InLockAsync(func);
        }
    }
    /// <summary>
    /// Extends MonitorLocker with an internal state variable
    /// </summary>
    /// <typeparam name="TState">The state Type</typeparam>
    /// <remarks>Same as StateLocker&lt;MonitorLocker;, TState&gt;</remarks>
    public class MonitorLocker<TState> : StateLocker<MonitorLocker, TState> { }

    ///// <summary>
    ///// Extends MonitorLocker with an internal state value
    ///// </summary>
    ///// <typeparam name="TState">The Type of the internal state value</typeparam>
    //public class MonitorLocker<TState> : MonitorLocker
    //{
    //    private TState _state;

    //    /// <summary>
    //    /// Executes an action from within a Monitor lock, passing in an internal state value
    //    /// </summary>
    //    /// <param name="action">The action to run</param>
    //    public void InLock(Action<TState> action)
    //    {
    //        base.InLock(() => { action.Invoke(_state); });
    //    }
    //    /// <summary>
    //    /// Executes an action from within a Monitor lock, passing in an internal state value
    //    /// </summary>
    //    /// <param name="action">The action to run</param>
    //    public async Task InLockAsync(Action<TState> action)
    //    {
    //        await base.InLockAsync(() => { action.Invoke(_state); });
    //    }
    //    /// <summary>
    //    /// Executes a function from within a Monitor lock, assigning the state value with the result
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    public TState InLock(StateAssignmentDelegate<TState> func)
    //    {
    //        return base.InLock(() => func.Invoke(_state));
    //    }
    //    /// <summary>
    //    /// Executes a function from within a Monitor lock, assigning the state value with the result
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    public async Task<TState> InLockAsync(StateAssignmentDelegate<TState> func)
    //    {
    //        return await base.InLockAsync(() => func.Invoke(_state));
    //    }
    //}
}
