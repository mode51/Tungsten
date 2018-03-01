using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//using W.LockExtensions;
namespace W.Threading.Lockers
{
    /// <summary>
    /// Uses SemaphoreSlim to provide resource locking
    /// </summary>
    public class SemaphoreSlimLocker : ILocker<SemaphoreSlim>, IDisposable
    {
        /// <summary>
        /// The SemaphoreSlim used to perform locks
        /// </summary>
        public SemaphoreSlim Locker { get; private set; } = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Locks the resource
        /// </summary>
        public void Lock()
        {
            Locker.Wait();
        }
        /// <summary>
        /// Unlocks the resource
        /// </summary>
        public void Unlock()
        {
            Locker.Release();
        }
        /// <summary>
        /// Executes an action from within a SemaphoreSlim
        /// </summary>
        /// <param name="action">The action to run</param>
        public void InLock(Action action)
        {
            Locker.InLock(action);
        }
        /// <summary>
        /// Executes a function from within a SemaphoreSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public TValue InLock<TValue>(Func<TValue> func)
        {
            return Locker.InLock(func);
        }

        /// <summary>
        /// Executes an action from within a SemaphoreSlim
        /// </summary>
        /// <param name="action">The action to run</param>
        public async Task InLockAsync(Action action)
        {
            await Locker.InLockAsync(action);
        }
        /// <summary>
        /// Executes a function from within a SemaphoreSlim
        /// </summary>
        /// <typeparam name="TValue">The type of return value</typeparam>
        /// <param name="func">The function to run</param>
        /// <returns>The result of the function call (a value of type TValue)</returns>
        public async Task<TValue> InLockAsync<TValue>(Func<TValue> func)
        {
            return await Locker.InLockAsync(func);
        }

        /// <summary>
        /// Disposes the SemaphoreSlimLocker and releases resources
        /// </summary>
        public void Dispose()
        {
            Locker?.Dispose();
            Locker = null;
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Constructs a new SemaphoreSlimLocker with an initial request count of 1 and maximum request count of 1
        /// </summary>
        public SemaphoreSlimLocker() : this(1, 1) { }
        /// <summary>
        /// Constructs a new SemaphoreSlimLocker
        /// </summary>
        /// <param name="initialCount">The initial number of requests that the semaphore can grant concurrently</param>
        public SemaphoreSlimLocker(int initialCount)
        {
            Locker = new SemaphoreSlim(initialCount);
        }
        /// <summary>
        /// Constructs a new SemaphoreSlimLocker
        /// </summary>
        /// <param name="initialCount">The initial number of requests that the semaphore can grant concurrently</param>
        /// <param name="maxCount">The maximum number of requests that can be granted concurrently</param>
        public SemaphoreSlimLocker(int initialCount, int maxCount)
        {
            Locker = new SemaphoreSlim(initialCount, maxCount);
        }
    }
    /// <summary>
    /// Extends SemaphoreSlimLocker with an internal state variable
    /// </summary>
    /// <typeparam name="TState">The state Type</typeparam>
    /// <remarks>Same as StateLocker&lt;SemaphoreSlimLocker;, TState&gt;</remarks>
    public class SemaphoreSlimLocker<TState> : StateLocker<SemaphoreSlimLocker, TState> { }
}
