using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace W.Threading.Lockers
{
    /// <summary>
    /// The required implementation for a locking object
    /// </summary>
    public interface ILocker
    {
        /// <summary>
        /// Perform some action in a lock
        /// </summary>
        /// <param name="action">The action to perform</param>
        void InLock(Action action);
        /// <summary>
        /// Perform some function in a lock
        /// </summary>
        /// <typeparam name="TResult">The result Type</typeparam>
        /// <param name="func">The function to perform</param>
        /// <returns>The result of the function</returns>
        TResult InLock<TResult>(Func<TResult> func);
        /// <summary>
        /// Asynchronously perform some action in a lock
        /// <summary>
        /// <param name="action">The action to perform</param>
        Task InLockAsync(Action action);
        /// <summary>
        /// Asyncrhonously perform some function in a lock
        /// </summary>
        /// <typeparam name="TResult">The result Type</typeparam>
        /// <param name="func">The function to perform</param>
        /// <returns>The result of the function</returns>
        Task<TResult> InLockAsync<TResult>(Func<TResult> func);
    }
    /// <summary>
    /// The required implementation for a locking object
    /// </summary>
    /// <typeparam name="TLocker">The type of locker to use (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock)</typeparam>
    public interface ILocker<TLocker> : ILocker
    {
        /// <summary>
        /// The object used for locking
        /// </summary>
        TLocker Locker { get; }
    }
}
