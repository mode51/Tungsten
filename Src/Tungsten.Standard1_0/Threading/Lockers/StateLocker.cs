using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using W.LockExtensions;

namespace W.Threading.Lockers
{
    /// <summary>
    /// Extends a locker (SpinLocker, MonitorLocker, ReaderWriterLocker, SemaphoreSlimLocker) with an internal state value
    /// </summary>
    /// <typeparam name="TLocker">The Type of Locker to extend</typeparam>
    /// <typeparam name="TState">The Type of the internal state value</typeparam>
    /// <remarks>This class adds the state functionality by wrapping the TLocker and re-implementing the ILocker interface</remarks>
    public class StateLocker<TLocker, TState> : IStateLocker<TLocker, TState> where TLocker : ILocker, new()
    {
        /// <summary>
        /// The internal state
        /// </summary>
        protected TState State;// { get; set; }

        /// <summary>
        /// The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock)
        /// </summary>
        public TLocker Locker { get; } = new TLocker();

        #region ILocker
        /// <summary>
        /// Performs an action from within a lock
        /// </summary>
        /// <param name="action">The action to run</param>
        public void InLock(Action action)
        {
            Locker.InLock(action);
        }
        /// <summary>
        /// Performs a function from within a lock
        /// </summary>
        /// <param name="func">The function to run</param>
        public TResult InLock<TResult>(Func<TResult> func)
        {
            return Locker.InLock(func);
        }
        /// <summary>
        /// Asynchronously performs an action from within a lock
        /// </summary>
        /// <param name="action">The action to run</param>
        public async Task InLockAsync(Action action)
        {
            await Locker.InLockAsync(action);
        }
        /// <summary>
        /// Asynchronously performs a function from within a lock
        /// </summary>
        /// <param name="func">The function to run</param>
        public async Task<TResult> InLockAsync<TResult>(Func<TResult> func)
        {
            return await Locker.InLockAsync(func);
        }
        #endregion

        #region IStateLocker
        /// <summary>
        /// Performs an action from within a lock, passing in the current state
        /// </summary>
        /// <param name="action">The action to run</param>
        public void InLock(Action<TState> action)
        {
            Locker.InLock(() => action.Invoke(State));
        }
        /// <summary>
        /// Performs a function from within a lock, passing in the current state and assigning the state to the function result
        /// </summary>
        /// <param name="func">The function to run</param>
        public TState InLock(Func<TState, TState> func)
        {
            return Locker.InLock(() => { State = func.Invoke(State); return State; });
        }
        /// <summary>
        /// Asynchronously performs an action from within a lock, passing in the current state
        /// </summary>
        /// <param name="action">The action to run</param>
        public async Task InLockAsync(Action<TState> action)
        {
            await Locker.InLockAsync(() => action.Invoke(State));
        }
        /// <summary>
        /// Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result
        /// </summary>
        /// <param name="func">The function to run</param>
        public async Task<TState> InLockAsync(Func<TState, TState> func)
        {

            return await Locker.InLockAsync(() => { State = func.Invoke(State); return State; });
        }
        #endregion
    }
}
