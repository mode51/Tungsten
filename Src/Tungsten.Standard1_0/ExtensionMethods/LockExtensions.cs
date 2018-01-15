using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Threading.Lockers;

namespace W.LockExtensions
{
    /// <summary>
    /// Extensions to simplify locking with SemaphoreSlim
    /// </summary>
    public static class SemaphoreSlimExtensions
    {
        /// <summary>
        /// Performs the action in a lock
        /// </summary>
        /// <param name="this">The SemaphoreSlim to provide resource locking</param>
        /// <param name="action">The action to perform</param>
        public static void InLock(this SemaphoreSlim @this, Action action)
        {
            @this.Wait();
            try
            {
                action.Invoke();
            }
            catch { throw; }
            finally { @this.Release(); }
        }
        /// <summary>
        /// Asynchronously performs the action in a lock
        /// </summary>
        /// <param name="this">The SemaphoreSlim to provide resource locking</param>
        /// <param name="action">The action to perform</param>
        public static async Task InLockAsync(this SemaphoreSlim @this, Action action)
        {
            @this.Wait();
            try
            {
                await Task.Run(action);
            }
            catch { throw; }
            finally { @this.Release(); }
        }
        /// <summary>
        /// Performs the action in a lock
        /// </summary>
        /// <param name="this">The SemaphoreSlim to provide resource locking</param>
        /// <param name="func">The function to perform</param>
        public static TType InLock<TType>(this SemaphoreSlim @this, Func<TType> func)
        {
            @this.Wait();
            try
            {
                return func.Invoke();
            }
            catch { throw; }
            finally { @this.Release(); }
        }
        /// <summary>
        /// Asynchronously performs the function in a lock
        /// </summary>
        /// <param name="this">The SemaphoreSlim to provide resource locking</param>
        /// <param name="func">The function to perform</param>
        public static async Task<TType> InLockAsync<TType>(this SemaphoreSlim @this, Func<TType> func)
        {
            return await Task.Run(() =>
            {
                @this.Wait();
                try
                {
                    return func();
                }
                catch { throw; }
                finally { @this.Release(); }
            });
        }
    }
    /// <summary>
    /// Extensions to simplify locking with SpinLock
    /// </summary>
    public static class SpinLockExtensions
    {
        /// <summary>
        /// Performs the action in a lock
        /// </summary>
        /// <param name="this">The SpinLock to provide resource locking</param>
        /// <param name="action">The action to perform</param>
        public static void InLock(this SpinLock @this, Action action)
        {
            var lockTaken = false;
            try
            {
                @this.Enter(ref lockTaken);
                action.Invoke();
            }
            catch { throw; }
            finally { if (lockTaken) @this.Exit(false); }
        }
        /// <summary>
        /// Asynchronously performs the action in a lock
        /// </summary>
        /// <param name="this">The SpinLock to provide resource locking</param>
        /// <param name="action">The action to perform</param>
        public static async Task InLockAsync(this SpinLock @this, Action action)
        {
            var lockTaken = false;
            try
            {
                @this.Enter(ref lockTaken);
                await Task.Run(action);
            }
            catch { throw; }
            finally { if (lockTaken) @this.Exit(false); }
        }
        /// <summary>
        /// Performs the function in a lock
        /// </summary>
        /// <param name="this">The SpinLock to provide resource locking</param>
        /// <param name="func">The function to perform</param>
        public static TType InLock<TType>(this SpinLock @this, Func<TType> func)
        {
            var lockTaken = false;
            try
            {
                @this.Enter(ref lockTaken);
                return func.Invoke();
            }
            catch { throw; }
            finally { if (lockTaken) @this.Exit(false); }
        }
        /// <summary>
        /// Asynchronously performs the function in a lock
        /// </summary>
        /// <param name="this">The SpinLock to provide resource locking</param>
        /// <param name="func">The function to perform</param>
        public static async Task<TType> InLockAsync<TType>(this SpinLock @this, Func<TType> func)
        {
            return await Task.Run(() =>
            {
                var lockTaken = false;
                try
                {
                    @this.Enter(ref lockTaken);
                    return func();
                }
                catch { throw; }
                finally { if (lockTaken) @this.Exit(false); }
            });
        }
    }
    /// <summary>
    /// Extensions to object to simplify locking with Monitor
    /// </summary>
    public static class MonitorExtensions
    {
        /// <summary>
        /// Performs the action in a Monitor lock
        /// </summary>
        /// <param name="this">The object to provide resource locking</param>
        /// <param name="action">The action to perform</param>
        public static void InLock(this object @this, Action action)
        {
            Monitor.Enter(@this);
            try
            {
                action.Invoke();
            }
            catch { throw; }
            finally { Monitor.Exit(@this); }
        }
        /// <summary>
        /// Asynchronously performs the action in a Monitor lock
        /// </summary>
        /// <param name="this">The object to provide resource locking</param>
        /// <param name="action">The action to perform</param>
        public static async Task InLockAsync(this object @this, Action action)
        {
            Monitor.Enter(@this);
            try
            {
                await Task.Run(action);
            }
            catch { throw; }
            finally { Monitor.Exit(@this); }
        }
        /// <summary>
        /// Performs the function in a Monitor lock
        /// </summary>
        /// <param name="this">The object to provide resource locking</param>
        /// <param name="func">The function to perform</param>
        public static TType InLock<TType>(this object @this, Func<TType> func)
        {
            Monitor.Enter(@this);
            try
            {
                return func.Invoke();
            }
            catch { throw; }
            finally { Monitor.Exit(@this); }
        }
        /// <summary>
        /// Asynchronously performs the action in a Monitor lock
        /// </summary>
        /// <param name="this">The object to provide resource locking</param>
        /// <param name="func">The function to perform</param>
        public static async Task<TType> InLockAsync<TType>(this object @this, Func<TType> func)
        {
            return await Task.Run(() =>
            {
                Monitor.Enter(@this);
                try
                {
                    return func();
                }
                catch { throw; }
                finally { Monitor.Exit(@this); }
            });
        }
    }
    /// <summary>
    /// Extensions to simplify locking with ReaderWriterLockSlim
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {
        /// <summary>
        /// Enters a read or write lock on the ReaderWriterLockSlim
        /// </summary>
        /// <param name="this">The object to provide resource locking</param>
        /// <param name="lockType">The type of lock to enter</param>
        public static void Lock(this ReaderWriterLockSlim @this, LockTypeEnum lockType)
        {
            if (lockType == LockTypeEnum.Read)
                @this.EnterReadLock();
            else
                @this.EnterWriteLock();
        }
        /// <summary>
        /// Exits a read or write lock on the ReaderWriterLockSlim
        /// </summary>
        /// <param name="this">The object to provide resource locking</param>
        /// <param name="lockType">The type of lock to exit</param>
        public static void Unlock(this ReaderWriterLockSlim @this, LockTypeEnum lockType)
        {
            if (lockType == LockTypeEnum.Read)
                @this.ExitReadLock();
            else
                @this.ExitWriteLock();
        }

        /// <summary>
        /// Performs the action in a lock
        /// </summary>
        /// <param name="this">The ReaderWriterLockSlim to provide resource locking</param>
        /// <param name="lockType">The type of lock to obtain</param>
        /// <param name="action">The action to perform</param>
        public static void InLock(this ReaderWriterLockSlim @this, LockTypeEnum lockType, Action action)
        {
            @this.Lock(lockType);
            try
            {
                action.Invoke();
            }
            catch { throw; }
            finally { @this.Unlock(lockType); }
        }
        /// <summary>
        /// Asynchronously performs the action in a lock
        /// </summary>
        /// <param name="this">The ReaderWriterLockSlim to provide resource locking</param>
        /// <param name="lockType">The type of lock to obtain</param>
        /// <param name="action">The action to perform</param>
        public static async Task InLockAsync(this ReaderWriterLockSlim @this, LockTypeEnum lockType, Action action)
        {
            @this.Lock(lockType);
            try
            {
                await Task.Run(action);
            }
            catch { throw; }
            finally { @this.Unlock(lockType); }
        }
        /// <summary>
        /// Performs the function in a lock
        /// </summary>
        /// <param name="this">The ReaderWriterLockSlim to provide resource locking</param>
        /// <param name="lockType">The type of lock to obtain</param>
        /// <param name="func">The function to perform</param>
        public static TType InLock<TType>(this ReaderWriterLockSlim @this, LockTypeEnum lockType, Func<TType> func)
        {
            @this.Lock(lockType);
            try
            {
                return func.Invoke();
            }
            catch { throw; }
            finally { @this.Unlock(lockType); }
        }
        /// <summary>
        /// Asynchronously performs the function in a lock
        /// </summary>
        /// <param name="this">The ReaderWriterLockSlim to provide resource locking</param>
        /// <param name="lockType">The type of lock to obtain</param>
        /// <param name="func">The function to perform</param>
        public static async Task<TType> InLockAsync<TType>(this ReaderWriterLockSlim @this, LockTypeEnum lockType, Func<TType> func)
        {
            return await Task.Run(() =>
            {
                @this.Lock(lockType);
                try
                {
                    return func();
                }
                catch { throw; }
                finally { @this.Unlock(lockType); }
            });
        }
    }
}
