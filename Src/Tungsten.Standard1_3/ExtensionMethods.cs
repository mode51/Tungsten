using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace W
{
    /// <summary>
    /// Tungsten extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Performs the given action in a lock statement using the provided object for the lock
        /// </summary>
        /// <typeparam name="TItemType">The type of object on which the lock is made</typeparam>
        /// <param name="this">The object to use in the lock</param>
        /// <param name="action">The action to perform in a lock statement</param>
        /// <code>
        /// someValue.Lock(value => { value += 10; }); //lock on someValue and pass it into the action
        /// </code>
        public static void Lock<TItemType>(this TItemType @this, Action<TItemType> action)
        {
            lock (@this)
            {
                action?.Invoke(@this);
            }

        }
        /// <summary>
        /// Initiates a Task which will wait for the given variable to have the specified value
        /// </summary>
        /// <param name="this">The value being inspected</param>
        /// <param name="desiredValue">The value to wait for</param>
        /// <param name="msTimeout">The task will time out within the specified number of milliseconds.  Use -1 to wait indefinitely.</param>
        /// <returns>True if the value was acquired within the specified timeout, otherwise False</returns>
        public static async Task<bool> WaitForValue(this object @this, object desiredValue, int msTimeout = -1)
        {
            System.Threading.CancellationTokenSource cts;
            if (msTimeout > 0)
                cts = new System.Threading.CancellationTokenSource(msTimeout);
            else
                cts = new System.Threading.CancellationTokenSource();
            var result = await Task.Run(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    if (@this == desiredValue)//.Equals(desiredValue))
                    {
                        return true;
                    }
                    W.Threading.Thread.Sleep(1);
                }
                return false;
            }, cts.Token).ContinueWith(task =>
            {
                return (!task.IsCanceled);
            });
            return result;
        }
        /// <summary>
        /// Initiates a Task which will wait for the specified condition to be met
        /// </summary>
        /// <param name="this">The value being inspected</param>
        /// <param name="predicate">The condition to be met</param>
        /// <param name="msTimeout">The task will time out within the specified number of milliseconds.  Use -1 to wait indefinitely.</param>
        /// <returns>True if the condition was met within the specified timeout, otherwise False</returns>
        public static async Task<bool> WaitForValue<TItemType>(this TItemType @this, Predicate<TItemType> predicate, int msTimeout = -1)
        {
            System.Threading.CancellationTokenSource cts;
            if (msTimeout > 0)
                cts = new System.Threading.CancellationTokenSource(msTimeout);
            else
                cts = new System.Threading.CancellationTokenSource();
            var result = await Task.Run(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    if (predicate.Invoke(@this))
                    {
                        return true;
                    }
                    W.Threading.Thread.Sleep(1);
                }
                return false;
            }, cts.Token).ContinueWith(task =>
            {
                return (!task.IsCanceled);
            });
            return result;
        }
    }
}
