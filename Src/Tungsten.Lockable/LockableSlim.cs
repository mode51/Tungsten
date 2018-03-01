using System;
using System.Threading.Tasks;
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
}
