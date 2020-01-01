using System;
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
        /// Gets the underlying value.  This is performed in a read lock.
        /// </summary>
        protected virtual TValue GetValue()
        {
            return GetState();
            //return InLock(LockTypeEnum.Read, state => { return state; });
        }
        /// <summary>
        /// Sets the underlying value.  This is performed in a write lock.
        /// </summary>
        /// <param name="value">The new value</param>
        protected virtual void SetValue(TValue value)
        {
            base.SetState(value);
            //InLock(() => { return value; });
        }

        /// <summary>
        /// Get or Set the value.  This is performed in a read lock or write lock (respectively).
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
        public LockableSlim(TValue initialValue) : base(System.Threading.LockRecursionPolicy.SupportsRecursion) { State = initialValue; }
        ///// <summary>
        ///// Constructs a new LockableSlim assigning an initial value
        ///// </summary>
        ///// <param name="initialValue">The initial value to assign</param>
        ///// <param name="supportsRecursion">The desired LockRecusionPolicy</param>
        //public LockableSlim(TValue initialValue, System.Threading.LockRecursionPolicy supportsRecursion) : base(supportsRecursion) { _value = initialValue; }
    }
}
