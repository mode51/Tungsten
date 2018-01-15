using System;
using System.Threading.Tasks;
using W;
using W.DelegateExtensions;
using W.Threading.Lockers;

namespace W
{
    /// <summary>
    /// Uses ReaderWriterLock to provide thread-safe access to an underlying value
    /// </summary>
    /// <typeparam name="TValue">The Type of value</typeparam>
    /// <remarks>Can be overridden to provide additional functionality</remarks>
    public class LockableSlim<TValue> : ReaderWriterLocker<TValue>
    {
        /// <summary>
        /// Gets the underlying value
        /// </summary>
        protected virtual TValue GetValue()
        {
            return InLock(() => State);
        }
        /// <summary>
        /// Sets the underlying value
        /// </summary>
        /// <param name="value">The new value</param>
        protected virtual void SetValue(TValue value)
        {
            InLock(() => State = value);
        }

        /// <summary>
        /// Get or Set the value
        /// </summary>
        /// <returns>The current value</returns>
        public TValue Value
        {
            get
            {
                return GetValue();
            }
            set
            {
                SetValue(value);
            }
        }

        /// <summary>
        /// Constructs a new LockableSlim with a default initial value
        /// </summary>
        public LockableSlim() : this(default(TValue)) { }
        /// <summary>
        /// Constructs a new LockableSlim assigning an initial value
        /// </summary>
        /// <param name="initialValue">The initial value to assign</param>
        public LockableSlim(TValue initialValue) { State = initialValue; }
    }

    ///// <summary>
    ///// Uses ReaderWriterLock to provide thread-safe access to an underlying value
    ///// </summary>
    ///// <typeparam name="TValue">The Type of value to wrap</typeparam>
    ///// <remarks>Can be overridden to provide additional functionality</remarks>
    //public class LockableSlim<TValue> : ReaderWriterLocker
    //{
    //    private TValue _value;

    //    /// <summary>
    //    /// Gets the underlying value without locking.  To be used in conjunction with the Lock/Unlock and InLock/InLockAsync.
    //    /// </summary>
    //    /// <returns>The underlying value</returns>
    //    /// <remarks>Use with caution.  Do not call this function without a prior lock.</remarks>
    //    protected TValue GetUnlockedValue()
    //    {
    //        return _value;
    //    }
    //    /// <summary>
    //    /// Sets the underlying value without locking.  To be used in conjunction with the Lock/Unlock and InLock/InLockAsync.
    //    /// </summary>
    //    /// <remarks>Use with caution.  Do not call this function without a prior lock.</remarks>
    //    protected void SetUnlockedValue(TValue value)
    //    {
    //        _value = value;
    //    }

    //    /// <summary>
    //    /// Gets the underlying value
    //    /// </summary>
    //    protected virtual TValue GetValue()
    //    {
    //        return InLock(() => { return GetUnlockedValue(); });
    //    }
    //    /// <summary>
    //    /// Sets the underlying value
    //    /// </summary>
    //    /// <param name="value">The new value</param>
    //    protected virtual void SetValue(TValue value)
    //    {
    //        SetValueInLock(v => { return value; });
    //    }

    //    /// <summary>
    //    /// Get or Set the value
    //    /// </summary>
    //    /// <returns>The current value</returns>
    //    public TValue Value
    //    {
    //        get
    //        {
    //            return GetValue();
    //        }
    //        set
    //        {
    //            SetValue(value);
    //        }
    //    }
    //    /// <summary>
    //    /// Executes a function from within a Monitor lock, assigning the underlying value to the result
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    /// <returns>The result of the function call (a value of type TValue)</returns>
    //    public void SetValueInLock(Func<TValue, TValue> func)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Write);
    //            SetUnlockedValue(func.Invoke(GetUnlockedValue()));
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Write);
    //        }
    //    }
    //    /// <summary>
    //    /// Executes a function from within a Monitor lock, assigning the underlying value to the result
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    /// <returns>The result of the function call (a value of type TValue)</returns>
    //    public async Task SetValueInLockAsync(Func<TValue, TValue> func)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Write);
    //            await Task.Run(() => SetUnlockedValue(func.Invoke(GetUnlockedValue())));
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Write);
    //        }
    //    }

    //    /// <summary>
    //    /// Executes an action from within a Monitor lock, passing in the underlying value
    //    /// </summary>
    //    /// <param name="action">The action to run</param>
    //    public void InLock(Action action)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Read);
    //            action.Invoke();
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Read);
    //        }
    //    }
    //    /// <summary>
    //    /// Executes a function from within a Monitor lock, passing in the underlying value
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    /// <returns>The result of the function call (a value of type TValue)</returns>
    //    public TValue InLock(Func<TValue> func)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Read);
    //            return func.Invoke();
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Read);
    //        }
    //    }

    //    /// <summary>
    //    /// Executes an action from within a Monitor lock, passing in the underlying value
    //    /// </summary>
    //    /// <param name="action">The action to run</param>
    //    public void InLock(Action<TValue> action)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Read);
    //            action.Invoke(GetUnlockedValue());
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Read);
    //        }
    //    }
    //    /// <summary>
    //    /// Executes a function from within a Monitor lock, passing in the underlying value
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    /// <returns>The result of the function call (a value of type TValue)</returns>
    //    public TValue InLock(Func<TValue, TValue> func)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Read);
    //            return func.Invoke(GetUnlockedValue());
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Read);
    //        }
    //    }
    //    /// <summary>
    //    /// Executes an action from within a Monitor lock, passing in the underlying value
    //    /// </summary>
    //    /// <param name="action">The action to run</param>
    //    public async Task InLockAsync(Action<TValue> action)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Read);
    //            await Task.Run(() => action(GetUnlockedValue()));
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Read);
    //        }
    //    }
    //    /// <summary>
    //    /// Executes a function from within a Monitor lock, passing in the underlying value
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    /// <returns>The result of the function call (a value of type TValue)</returns>
    //    public async Task<TValue> InLockAsync(Func<TValue, TValue> func)
    //    {
    //        try
    //        {
    //            Lock(LockTypeEnum.Read);
    //            return await Task.Run(() => func.Invoke(GetUnlockedValue()));
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            Unlock(LockTypeEnum.Read);
    //        }
    //    }

    //    /// <summary>
    //    /// Constructs a new LockableSlim with a default initial value
    //    /// </summary>
    //    public LockableSlim() : this(default(TValue)) { }
    //    /// <summary>
    //    /// Constructs a new LockableSlim assigning an initial value
    //    /// </summary>
    //    /// <param name="initialValue">The initial value to assign</param>
    //    public LockableSlim(TValue initialValue) { SetUnlockedValue(initialValue); }
    //}

    //    /// <summary>
    //    /// <para>
    //    /// Provides thread safety via ReaderWriterLockSlim locking.  This is more efficient than Lockable&lt;TValue&gt;.
    //    /// </para>
    //    /// </summary>
    //    /// <typeparam name="TValue">The Type of value to wrap</typeparam>
    //    public partial class LockableSlim<TValue> : ThreadSafeValue<TValue>
    //    {
    //        /// <summary>
    //        /// The ReaderWriterLockSlim which is used to obtain read/write locks on the value
    //        /// </summary>
    //        protected System.Threading.ReaderWriterLockSlim ValueLock { get; private set; }

    //        /// <summary>
    //        /// Gets the value
    //        /// </summary>
    //        /// <param name="withLock">If True, a read lock will be acquired before retrieving the value</param>
    //        /// <returns>The current value</returns>
    //        protected virtual TValue GetValue(bool withLock)
    //        {
    //            try
    //            {
    //                if (withLock)
    //                    ValueLock.EnterReadLock();
    //                return GetValue();
    //            }
    //            catch
    //            {
    //                throw;
    //            }
    //            finally
    //            {
    //                if (withLock)
    //                    ValueLock.ExitReadLock();
    //            }
    //        }
    //        /// <summary>
    //        /// Sets the value
    //        /// </summary>
    //        /// <param name="withLock">If True, a write lock will be acquired before setting the value</param>
    //        /// <param name="newValue">The new value</param>
    //        protected virtual void SetValue(TValue newValue, bool withLock)
    //        {
    //            try
    //            {
    //                if (withLock)
    //                    ValueLock.EnterWriteLock();
    //                SetValue(newValue);
    //            }
    //            catch
    //            {
    //                throw;
    //            }
    //            finally
    //            {
    //                if (withLock)
    //                    ValueLock.ExitWriteLock();
    //            }
    //        }

    //        /// <summary>
    //        /// Constructor which initializes Value with the default of TValue
    //        /// </summary>
    //        public LockableSlim() : this(default(TValue)) { }
    //        /// <summary>
    //        /// Constructor which initializes Value with the specified value
    //        /// </summary>
    //        /// <param name="initialValue">The initial value for Value</param>
    //        public LockableSlim(TValue initialValue) : this(initialValue, System.Threading.LockRecursionPolicy.NoRecursion) { }
    //        /// <summary>
    //        /// Constructor which initializes Value with the specified value
    //        /// </summary>
    //        /// <param name="initialValue">The initial value for Value</param>
    //        /// <param name="lockingPolicy">The locking policy to enforce</param>
    //        public LockableSlim(TValue initialValue, System.Threading.LockRecursionPolicy lockingPolicy)
    //        {
    //            ValueLock = new System.Threading.ReaderWriterLockSlim(lockingPolicy);
    //            SetValue(initialValue);
    //        }
    //        /// <summary>
    //        /// Destructs the LockableSlim object and releases resources
    //        /// </summary>
    //        ~LockableSlim()
    //        {
    //            ValueLock.Dispose();
    //        }
    //    }
    //    public partial class LockableSlim<TValue>
    //    {
    //        /// <summary>
    //        /// Executes the action in a read lock
    //        /// </summary>
    //        /// <param name="action">The action to execute</param>
    //        public void InReadLock(Action<TValue> action)
    //        {
    //            ValueLock.EnterReadLock();
    //            try
    //            {
    //                action.Invoke(GetValue(false));
    //            }
    //            catch
    //            {
    //                throw;
    //            }
    //            finally
    //            {
    //                ValueLock.ExitReadLock();
    //            }
    //        }
    //        /// <summary>
    //        /// Executes the action in a read lock
    //        /// </summary>
    //        /// <param name="action">The action to execute</param>
    //        public async Task InReadLockAsync(Action<TValue> action)
    //        {
    //            await Task.Run(() => InReadLock(action));
    //        }

    //        /// <summary>
    //        /// Executes the function in a read lock and returns the result
    //        /// </summary>
    //        /// <param name="func">The function to execute</param>
    //        public TValue InReadLock(Func<TValue, TValue> func)
    //        {
    //            ValueLock.EnterReadLock();
    //            try
    //            {
    //                var result = func.Invoke(GetValue(false));
    //                return result;
    //            }
    //            catch
    //            {
    //                throw;
    //            }
    //            finally
    //            {
    //                ValueLock.ExitReadLock();
    //            }
    //        }
    //        /// <summary>
    //        /// Asynchronously executes the function in a read lock and returns the result
    //        /// </summary>
    //        /// <param name="func">The function to execute</param>
    //        public async Task<TValue> InReadLockAsync(Func<TValue, TValue> func)
    //        {
    //            return await Task.Run(() => InReadLock(func));
    //        }

    //        /// <summary>
    //        /// Executes the action in a write lock
    //        /// </summary>
    //        /// <param name="action">The function to execute</param>
    //        public void InWriteLock(Action<TValue> action)
    //        {
    //            ValueLock.EnterWriteLock();
    //            try
    //            {
    //                action.Invoke(GetValue(false));
    //            }
    //            catch
    //            {
    //                throw;
    //            }
    //            finally
    //            {
    //                ValueLock.ExitWriteLock();
    //            }
    //        }
    //        /// <summary>
    //        /// Asynchronsly executes the action in a write lock
    //        /// </summary>
    //        /// <param name="action">The function to execute</param>
    //        public async Task InWriteLockAsync(Action<TValue> action)
    //        {
    //            await Task.Run(() => InWriteLock(action));
    //        }

    //        /// <summary>
    //        /// Executes the function in a write lock, and sets Value to the return value
    //        /// </summary>
    //        /// <param name="func">The function to execute</param>
    //        /// <remarks>The return value will be assigned to Value</remarks>
    //        public void InWriteLock(Func<TValue, TValue> func)
    //        {
    //            ValueLock.EnterWriteLock();
    //            try
    //            {
    //                var result = func.Invoke(GetValue(false));
    //                SetValue(result, false);
    //            }
    //            catch
    //            {
    //                throw;
    //            }
    //            finally
    //            {
    //                ValueLock.ExitWriteLock();
    //            }
    //        }
    //        /// <summary>
    //        /// Asynchronously executes the function in a write lock, and sets Value to the return value
    //        /// </summary>
    //        /// <param name="func">The function to execute</param>
    //        /// <remarks>The return value will be assigned to Value</remarks>
    //        public async Task InWriteLockAsync(Func<TValue, TValue> func)
    //        {
    //            await Task.Run(() => InWriteLock(func));
    //        }
    //    }
}
