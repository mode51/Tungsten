using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Threading.Lockers
{
    /// <summary>
    /// Uses SpinLock to provide resource locking
    /// </summary>
    /// <remarks>Can be overridden to provide additional functionality</remarks>
    public class SpinLocker : ILocker<SpinLock>
    {
        /// <summary>
        /// The SpinLock used to perform locks
        /// </summary>
        public SpinLock Locker { get; } = new SpinLock();

        /// <summary>
        /// Locks the resource
        /// </summary>
        public void Lock(ref bool lockTaken)
        {
            Locker.TryEnter(ref lockTaken);
        }
        /// <summary>
        /// Unlocks the resource
        /// </summary>
        public void Unlock(bool useMemberBarrier = false)
        {
            Locker.Exit(useMemberBarrier);
        }

        /// <summary>
        /// Performs an action from within a SpinLock
        /// </summary>
        /// <param name="action">The action to run</param>
        public void InLock(Action action)
        {
            Locker.InLock(action);
        }
        /// <summary>
        /// Performs a function from within a SpinLock
        /// </summary>
        /// <typeparam name="TResult">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public TResult InLock<TResult>(Func<TResult> func)
        {
            return Locker.InLock(func);
        }

        /// <summary>
        /// Performs an action from within a SpinLock
        /// </summary>
        /// <param name="action">The action to run</param>
        public async Task InLockAsync(Action action)
        {
            await Locker.InLockAsync(action);
        }
        /// <summary>
        /// Performs a function from within a SpinLock
        /// </summary>
        /// <typeparam name="TResult">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public async Task<TResult> InLockAsync<TResult>(Func<TResult> func)
        {
            return await Locker.InLockAsync(func);
        }
    }
    /// <summary>
    /// Extends SpinLocker with an internal state variable
    /// </summary>
    /// <typeparam name="TState">The state Type</typeparam>
    /// <remarks>Same as StateLocker&lt;SpinLocker;, TState&gt;</remarks>
    public class SpinLocker<TState> : StateLocker<SpinLocker, TState> { }

    ///// <summary>
    ///// Extends SpinLocker with an internal state value
    ///// </summary>
    ///// <typeparam name="TState">The Type of the internal state value</typeparam>
    //public class SpinLocker<TState> : SpinLocker
    //{
    //    private TState _state;

    //    /// <summary>
    //    /// Executes an action from within a SpinLock, passing in an internal state value
    //    /// </summary>
    //    /// <param name="action">The action to run</param>
    //    public void InLock(Action<TState> action)
    //    {
    //        base.InLock(() => { action.Invoke(_state); });
    //    }
    //    /// <summary>
    //    /// Executes an action from within a SpinLock, passing in an internal state value
    //    /// </summary>
    //    /// <param name="action">The action to run</param>
    //    public async Task InLockAsync(Action<TState> action)
    //    {
    //        await base.InLockAsync(() => { action.Invoke(_state); });
    //    }
    //    /// <summary>
    //    /// Executes a function from within a SpinLock, assigning the state value with the result
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    public TState InLock(StateAssignmentDelegate<TState> func)
    //    {
    //        return base.InLock(() => func.Invoke(_state));
    //    }
    //    /// <summary>
    //    /// Executes a function from within a SpinLock, assigning the state value with the result
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    public async Task<TState> InLockAsync(StateAssignmentDelegate<TState> func)
    //    {
    //        return await base.InLockAsync(() => func.Invoke(_state));
    //    }
    //}
}
