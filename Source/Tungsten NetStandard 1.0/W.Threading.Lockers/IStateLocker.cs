using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace W.Threading.Lockers
{
    /// <summary>
    /// Delegate which can be used to assign a new value to the internal state
    /// </summary>
    /// <param name="state">The current state</param>
    /// <returns>The new value for the internal state</returns>
    public delegate TState StateAssignmentDelegate<TState>(TState state);

    /// <summary>
    /// The required implementation for a stateful locking object
    /// </summary>
    public interface IStateLocker<TLocker, TState> : ILocker<TLocker>
    {
        /// <summary>
        /// Perform some action in a lock
        /// </summary>
        /// <param name="action">The action to perform</param>
        void InLock(Action<TState> action);
        /// <summary>
        /// Perform some function in a lock
        /// </summary>
        /// <param name="func">The function to perform</param>
        /// <returns>The result of the function</returns>
        TState InLock(Func<TState, TState> func);
        /// <summary>
        /// Asynchronously perform some action in a lock
        /// <summary>
        /// <param name="action">The action to perform</param>
        Task InLockAsync(Action<TState> action);
        /// <summary>
        /// Asyncrhonously perform some function in a lock
        /// </summary>
        /// <typeparam name="TResult">The result Type</typeparam>
        /// <param name="func">The function to perform</param>
        /// <returns>The result of the function</returns>
        Task<TState> InLockAsync(Func<TState, TState> func);
    }
}
