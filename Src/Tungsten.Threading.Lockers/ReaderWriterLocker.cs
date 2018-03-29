using System;
using System.Threading;
using System.Threading.Tasks;

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
    /// Uses ReaderWriterLockSlim to provide resource locking
    /// </summary>
    /// <remarks>Can be overridden to provide additional functionality</remarks>
    public partial class ReaderWriterLocker : IDisposable
    {
        /// <summary>
        /// Enters a read or write lock on the ReaderWriterLockSlim
        /// </summary>
        /// <param name="lockType">The type of lock to enter</param>
        public void Lock(LockTypeEnum lockType)
        {
            if (lockType == LockTypeEnum.Read)
                Locker.EnterReadLock();
            else
                Locker.EnterWriteLock();
        }
        /// <summary>
        /// Exits a read or write lock on the ReaderWriterLockSlim
        /// </summary>
        /// <param name="lockType">The type of lock to exit</param>
        public void Unlock(LockTypeEnum lockType)
        {
            if (lockType == LockTypeEnum.Read)
                Locker.ExitReadLock();
            else
                Locker.ExitWriteLock();
        }

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
            Locker?.Dispose();
            Locker = null;
            GC.SuppressFinalize(this);
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
    ///// <summary>
    ///// Extends ReaderWriterLocker with an internal state variable
    ///// </summary>
    ///// <typeparam name="TState">The state Type</typeparam>
    //public class ReaderWriterLocker<TState> : StateLocker<ReaderWriterLocker, TState>, IDisposable
    //{
    //    /// <summary>
    //    /// Disposes this object and releases resources
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        Locker.Dispose();
    //    }

    //    /// <summary>
    //    /// Performs the action in a lock, passing in the current state
    //    /// </summary>
    //    /// <param name="action">The action to perform</param>
    //    public void InLock(LockTypeEnum lockType, Action<TState> action)
    //    {
    //        Locker.InLock(lockType, () => action.Invoke(State));
    //    }
    //    /// <summary>
    //    /// Performs the function in a lock, passing in the current state
    //    /// </summary>
    //    /// <param name="func">The action to perform</param>
    //    public TState InLock(LockTypeEnum lockType, Func<TState, TState> func)
    //    {
    //        return Locker.InLock(lockType, () => func.Invoke(State));
    //    }
    //    /// <summary>
    //    /// Asynchronously performs the action in a lock, passing in the current state
    //    /// </summary>
    //    /// <param name="action">The action to perform</param>
    //    public async Task InLockAsync(LockTypeEnum lockType, Action<TState> action)
    //    {
    //        await Locker.InLockAsync(lockType, () => action.Invoke(State));
    //    }
    //    /// <summary>
    //    /// Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result
    //    /// </summary>
    //    /// <param name="func">The action to perform</param>
    //    public async Task<TState> InLockAsync(LockTypeEnum lockType, Func<TState, TState> func)
    //    {
    //        return await Locker.InLockAsync(lockType, () => func.Invoke(State));
    //    }
    //}

    /// <summary>
    /// Interface definition for a ReaderWriterLocker with a State variable
    /// </summary>
    /// <typeparam name="TState">The state Type</typeparam>
    public interface IReaderWriterStateLocker<TState>
    {
        /// <summary>
        /// Executes an action from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="action">The action to run</param>
        void InLock(LockTypeEnum lockType, Action<TState> action);
        /// <summary>
        /// Executes a function from within a ReaderWriterLockSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        TValue InLock<TValue>(LockTypeEnum lockType, Func<TState, TValue> func);
        /// <summary>
        /// Executes an action from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="action">The action to run</param>
        Task InLockAsync(LockTypeEnum lockType, Action<TState> action);
        /// <summary>
        /// Executes a function from within a ReaderWriterLockSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        Task<TValue> InLockAsync<TValue>(LockTypeEnum lockType, Func<TState, TValue> func);

        /// <summary>
        /// Sets the internal state from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="newState">The new value</param>
        void SetState(TState newState);
        /// <summary>
        /// Retrieves the internal state from within a ReaderWriterLockSlim
        /// </summary>
        /// <returns>The current state</returns>
        TState GetState();
    }
    /// <summary>
    /// Extends ReaderWriterLocker with an internal state variable
    /// </summary>
    /// <typeparam name="TState">The state Type</typeparam>
    public class ReaderWriterLocker<TState> : IReaderWriterStateLocker<TState>, IDisposable
    {
        /// <summary>
        /// The ReaderWriterLocker used to access the State
        /// </summary>
        protected ReaderWriterLocker Locker;

        /// <summary>
        /// The internal state
        /// </summary>
        protected TState State;// { get; set; }

        /// <summary>
        /// Executes an action from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="action">The action to run</param>
        public void InLock(LockTypeEnum lockType, Action<TState> action)
        {
            Locker.InLock(lockType, () => action.Invoke(State));
        }
        /// <summary>
        /// Executes a function from within a ReaderWriterLockSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public TValue InLock<TValue>(LockTypeEnum lockType, Func<TState, TValue> func)
        {
            return Locker.InLock(lockType, () => { return func.Invoke(State); });
        }
        /// <summary>
        /// Executes an action from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="action">The action to run</param>
        public async Task InLockAsync(LockTypeEnum lockType, Action<TState> action)
        {
            await Locker.InLockAsync(lockType, () => action.Invoke(State));
        }
        /// <summary>
        /// Executes a function from within a ReaderWriterLockSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="lockType">Specifies whether to use a Read or Write lock</param>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public async Task<TValue> InLockAsync<TValue>(LockTypeEnum lockType, Func<TState, TValue> func)
        {
            return await Locker.InLockAsync(lockType, () => { return func.Invoke(State); });
        }

        /// <summary>
        /// Sets the internal state from within a ReaderWriterLockSlim
        /// </summary>
        /// <param name="newState">The new value</param>
        public void SetState(TState newState)
        {
            Locker.InLock(LockTypeEnum.Write, () => { State = newState; });
        }
        /// <summary>
        /// Retrieves the internal state from within a ReaderWriterLockSlim
        /// </summary>
        /// <returns>The current state</returns>
        public TState GetState()
        {
            return Locker.InLock(LockTypeEnum.Read, () => { return State; });
        }

        /// <summary>
        /// Disposes the ReaderWriterLocker and releases resources
        /// </summary>
        public void Dispose()
        {
            Locker.Dispose();
            Locker = null;
        }
        /// <summary>
        /// Constructs a new ReaderWriterLocker
        /// </summary>
        /// <param name="lockRecursionPolicy">The lock recursion policy to use</param>
        public ReaderWriterLocker(LockRecursionPolicy lockRecursionPolicy)
        {
            Locker = new ReaderWriterLocker(lockRecursionPolicy);
        }
    }
}
