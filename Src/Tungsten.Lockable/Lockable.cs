using System;
using System.Threading;
using System.Threading.Tasks;

namespace W
{
    /// <summary>
    /// Raised when the value has changed
    /// </summary>
    /// <param name="sender">The object which raised the event</param>
    /// <param name="oldValue">The old value</param>
    /// <param name="newValue">The new value</param>
    public delegate void ValueChangedDelegate<TValue>(object sender, TValue oldValue, TValue newValue);

    /// <summary>
    /// <para>
    /// Extends LockableSlim with ValueChangedDelegate notification
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The data Type to be used</typeparam>
    public partial class Lockable<TValue> : LockableSlim<TValue>, IDisposable
    {
        private LockableSlim<bool> _isDisposed = new LockableSlim<bool>(false);

        #region ValueChanged
        private ManualResetEventSlim _mreChanged = new ManualResetEventSlim(false);
        
        /// <summary>
        /// Raised when the value has changed
        /// </summary>
        public event ValueChangedDelegate<TValue> ValueChanged;

        /// <summary>
        /// Informs those who are waiting on WaitForChanged that the value has changed
        /// </summary>
        protected virtual void InformWaiters()
        {
            _mreChanged.Set();
            _mreChanged.Reset();
        }
        /// <summary>
        /// Raises the ValueChanged event
        /// </summary>
        /// <param name="oldValue">The previous value</param>
        /// <param name="newValue">The current value</param>
        protected void RaiseValueChanged(object sender, TValue oldValue, TValue newValue)
        {
            ValueChanged?.Invoke(sender, oldValue, newValue);
        }
        /// <summary>
        /// Calls RaiseValueChanged to raise the ValueChanged event
        /// </summary>
        /// <param name="sender">The object initiating the change</param>
        /// <param name="oldValue">The previous value</param>
        /// <param name="newValue">The current value</param>
        protected virtual void OnValueChanged(object sender, TValue oldValue, TValue newValue)
        {
            _onValueChanged?.Invoke(sender, oldValue, newValue);
            RaiseValueChanged(sender, oldValue, newValue);
            InformWaiters();
        }
        
        /// <summary>
        /// Allows the caller to block until Value changes
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the value to change</param>
        /// <returns>True if the value changed within the specified timeout period, otherwise False</returns>
        public bool WaitForValueChanged(int msTimeout = -1)
        {
            return _mreChanged.Wait(msTimeout);
        }
        #endregion

        /// <summary>
        /// Sets the value and raises the ValueChanged event
        /// </summary>
        /// <param name="newValue">The new value</param>
        protected override void SetValue(TValue newValue)
        {
            InLock(Threading.Lockers.LockTypeEnum.Write, oldValue => 
            {
                State = newValue; //manually set this here, as a call to SetState would use InLock recursively
                OnValueChanged(this, oldValue, newValue);
            });
        }
        /// <summary>
        /// Sets the value without raising the ValueChanged event or notifying waiters
        /// </summary>
        /// <param name="newValue">The new value</param>
        public virtual void LoadValue(TValue newValue)
        {
            InLock(Threading.Lockers.LockTypeEnum.Write, oldValue =>
            {
                var shouldSet = !System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(oldValue, newValue);
                if (shouldSet)
                    State = newValue;
            });
        }

        /// <summary>
        /// Disposes the Lockable and releases resources
        /// </summary>
        public new void Dispose()
        {
            _isDisposed.InLock(Threading.Lockers.LockTypeEnum.Write, oldValue =>
            {
                //if (!oldValue)
                _mreChanged.Dispose();
                base.Dispose();
                return true;
            });
        }
        /// <summary>
        /// Constructs a new Lockable&lt;TValue&gt;
        /// </summary>
        public Lockable() : this(default(TValue)) { }
        /// <summary>
        /// Constructs a new Lockable&lt;TValue&gt;
        /// </summary>
        /// <param name="initialValue">The initial value</param>
        public Lockable(TValue initialValue) : base(initialValue) { }
    }

    //add initialValue and Action<object, TValue, TValue> onValueChanged (called in OnValueChanged) overloads
    public partial class Lockable<TValue>
    {
        private Action<object, TValue, TValue> _onValueChanged = null;
        /// <summary>
        /// Constructs a new Lockable&lt;TValue&gt;
        /// </summary>
        /// <param name="onValueChanged">The Action to call when the value has changed</param>
        public Lockable(Action<object, TValue, TValue> onValueChanged) : this(default(TValue), onValueChanged) { }
        /// <summary>
        /// Constructs a new Lockable&lt;TValue&gt;
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="onValueChanged"></param>
        public Lockable(TValue initialValue, Action<object, TValue, TValue> onValueChanged) : base(initialValue)
        {
            _onValueChanged = onValueChanged;
        }
    }
}
