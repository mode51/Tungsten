using System;
using System.Threading;
using System.Threading.Tasks;
using W.LockExtensions;

namespace W.Threading.Lockers
{
    /// <summary>
    /// Used by ReaderWriterLocker to specify the type of lock to obtain
    /// </summary>
    public enum LockTypeEnum
    {
        /// <summary>
        /// Obtain a read-lock
        /// </summary>
        Read,
        /// <summary>
        /// Obtain a write-lock
        /// </summary>
        Write
    }

    /// <summary>
    /// Uses ReaderWriterLockSlim to provide thread-safety
    /// </summary>
    /// <remarks>Can be overridden to provide additional functionality</remarks>
    public partial class ReaderWriterLocker : IDisposable
    {
        private Disposer _disposer = new Disposer();

        ///// <summary>
        ///// Lock the resource
        ///// </summary>
        ///// <param name="lockType">The type of lock to use</param>
        //protected void Lock(LockTypeEnum lockType)
        //{
        //    if (lockType == LockTypeEnum.Read)
        //        Locker.EnterReadLock();
        //    else
        //        Locker.EnterWriteLock();
        //}
        ///// <summary>
        ///// Unlock the resource
        ///// </summary>
        ///// <param name="lockType">The type of lock to use</param>
        //protected void Unlock(LockTypeEnum lockType)
        //{
        //    if (lockType == LockTypeEnum.Read)
        //        Locker.ExitReadLock();
        //    else
        //        Locker.ExitWriteLock();
        //}

        /// <summary>
        /// Executes an action from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="action">The action to run</param>
        public void InLock(LockTypeEnum lockType, Action action)
        {
            Locker.InLock(lockType, action);
        }
        /// <summary>
        /// Executes a function from within a ReaderWriterLockSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public TValue InLock<TValue>(LockTypeEnum lockType, Func<TValue> func)
        {
            return Locker.InLock(lockType, func);
        }

        /// <summary>
        /// Executes an action from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="action">The action to run</param>
        public async Task InLockAsync(LockTypeEnum lockType, Action action)
        {
            await Locker.InLockAsync(lockType, action);
        }
        /// <summary>
        /// Executes a function from within a ReaderWriterLockSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public async Task<TValue> InLockAsync<TValue>(LockTypeEnum lockType, Func<TValue> func)
        {
            return await Locker.InLockAsync(lockType, func);
        }

        /// <summary>
        /// Disposes the instance and releases resources
        /// </summary>
        public void Dispose()
        {
            _disposer.Cleanup(this, () =>
            {
                Locker?.Dispose();
                Locker = null;
            });
        }
        /// <summary>
        /// Constructs a new ReaderWriterLocker with a LockRecursionPolicy of NoRecursion
        /// </summary>
        public ReaderWriterLocker() : this(LockRecursionPolicy.NoRecursion) { }
        /// <summary>
        /// Constructs a new ReaderWriterLocker using the specified LockRecursionPolicy
        /// </summary>
        /// <param name="lockPolicy">The lock recusion policy to use</param>
        public ReaderWriterLocker(System.Threading.LockRecursionPolicy lockPolicy)
        {
            Locker = new ReaderWriterLockSlim(lockPolicy);
        }
    }
    //implement ILocker<ReaderWriterLockerSlim>
    public partial class ReaderWriterLocker : ILocker<ReaderWriterLockSlim>
    {
        /// <summary>
        /// The ReaderWriterLockSlim used to perform locks
        /// </summary>
        public ReaderWriterLockSlim Locker { get; private set; }
        /// <summary>
        /// Performs the action in a read lock
        /// </summary>
        /// <param name="action">The action to perform</param>
        public void InLock(Action action)
        {
            InLock(LockTypeEnum.Read, action);
        }
        /// <summary>
        /// Performs the function in a read lock
        /// </summary>
        /// <param name="func">The function to perform</param>
        public TResult InLock<TResult>(Func<TResult> func)
        {
            return InLock(LockTypeEnum.Read, func);
        }
        /// <summary>
        /// Asynchronously performs the action in a read lock
        /// </summary>
        /// <param name="action">The action to perform</param>
        public async Task InLockAsync(Action action)
        {
            await InLockAsync(LockTypeEnum.Read, action);
        }
        /// <summary>
        /// Asynchronously performs the function in a read lock
        /// </summary>
        /// <param name="func">The action to perform</param>
        public async Task<TResult> InLockAsync<TResult>(Func<TResult> func)
        {
            return await InLockAsync(LockTypeEnum.Read, func);
        }
    }
    /// <summary>
    /// Extends ReaderWriterLocker with an internal state variable
    /// </summary>
    /// <typeparam name="TState">The state Type</typeparam>
    public class ReaderWriterLocker<TState> : StateLocker<ReaderWriterLocker, TState>, IDisposable
    {
        /// <summary>
        /// Disposes this object and releases resources
        /// </summary>
        public void Dispose()
        {
            Locker.Dispose();
        }

        /// <summary>
        /// Performs the action in a lock, passing in the current state
        /// </summary>
        /// <param name="action">The action to perform</param>
        public void InLock(LockTypeEnum lockType, Action<TState> action)
        {
            Locker.InLock(lockType, () => action.Invoke(State));
        }
        /// <summary>
        /// Performs the function in a lock, passing in the current state
        /// </summary>
        /// <param name="func">The action to perform</param>
        public TState InLock(LockTypeEnum lockType, Func<TState, TState> func)
        {
            return Locker.InLock(lockType, () => func.Invoke(State));
        }
        /// <summary>
        /// Asynchronously performs the action in a lock, passing in the current state
        /// </summary>
        /// <param name="action">The action to perform</param>
        public async Task InLockAsync(LockTypeEnum lockType, Action<TState> action)
        {
            await Locker.InLockAsync(lockType, () => action.Invoke(State));
        }
        /// <summary>
        /// Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result
        /// </summary>
        /// <param name="func">The action to perform</param>
        public async Task<TState> InLockAsync(LockTypeEnum lockType, Func<TState, TState> func)
        {
            return await Locker.InLockAsync(lockType, () => func.Invoke(State));
        }
    }
}
