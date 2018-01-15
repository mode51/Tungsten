using System;
using System.Threading;
using System.Threading.Tasks;
using W.DelegateExtensions;

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
            ValueChanged?.Raise(sender, oldValue, newValue);
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
            InLock(oldValue => 
            {
                State = newValue;
                OnValueChanged(this, oldValue, newValue);
            });
        }

        /// <summary>
        /// Disposes the Lockable and releases resources
        /// </summary>
        public void Dispose()
        {
            _isDisposed.InLock(oldValue =>
            {
                if (!oldValue)
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

    //    /// <summary>
    //    /// <para>
    //    /// Provides thread safety via locking
    //    /// </para>
    //    /// </summary>
    //    /// <typeparam name="TValue">The data Type to be used</typeparam>
    //    public class Lockable<TValue> : ExtravertedLockable<TValue>
    //    {
    //        ///// <summary>
    //        ///// Executes an action within a lock of the LockObject
    //        ///// </summary>
    //        ///// <param name="action">The action to call within a lock</param>
    //        //public void Lock(Action<TValue> action)
    //        //{
    //        //    //LockAsync(action, CancellationToken.None).Wait();
    //        //}
    //        ///// <summary>
    //        ///// Executes an action within a write lock
    //        ///// </summary>
    //        ///// <param name="function">The function to call</param>
    //        //public void Lock(Func<TValue, TValue> function)
    //        //{
    //        //    LockAsync(function, CancellationToken.None).Wait();
    //        //}

    //        ///// <summary>
    //        ///// Asynchronously executes an action within a write lock
    //        ///// </summary>
    //        ///// <param name="action">The action to call</param>
    //        //public async Task LockAsync(Action<TValue> action)
    //        //{
    //        //    await LockAsync(action, CancellationToken.None);
    //        //}
    //        ///// <summary>
    //        ///// Asynchronously executes an action within a write lock
    //        ///// </summary>
    //        ///// <param name="action">The action to call</param>
    //        ///// <param name="token">A CancellationToken token for the task to observe</param>
    //        //public async Task LockAsync(Action<TValue> action, CancellationToken token)
    //        //{
    //        //    if (action == null)
    //        //        throw new ArgumentNullException(nameof(action));
    //        //    await Task.Run(() =>
    //        //    {
    //        //        var currentValue = GetValue(false);
    //        //        ValueLock.EnterWriteLock();

    //        //        try
    //        //        {
    //        //            action?.Invoke(currentValue);
    //        //        }
    //        //        catch (Exception e)
    //        //        {
    //        //            throw e;
    //        //        }
    //        //        finally
    //        //        {
    //        //            ValueLock.ExitWriteLock();
    //        //        }
    //        //    }, token);
    //        //}
    //        ///// <summary>
    //        ///// Asynchronously executes a function within a write lock
    //        ///// </summary>
    //        ///// <param name="function">The function to call</param>
    //        //public async Task LockAsync(Func<TValue, TValue> function)
    //        //{
    //        //    await LockAsync(function, CancellationToken.None);
    //        //}
    //        ///// <summary>
    //        ///// Asynchronously executes a function within a write lock
    //        ///// </summary>
    //        ///// <param name="function">The function to call</param>
    //        ///// <param name="token">A CancellationToken token for the task to observe</param>
    //        //public async Task LockAsync(Func<TValue, TValue> function, CancellationToken token)
    //        //{
    //        //    if (function == null)
    //        //        throw new ArgumentNullException(nameof(function));
    //        //    await Task.Run(() =>
    //        //    {
    //        //        var currentValue = GetValue(false);
    //        //        ValueLock.EnterWriteLock();
    //        //        try
    //        //        {
    //        //            var newValue = function.Invoke(currentValue);
    //        //            SetValue(newValue, false);
    //        //        }
    //        //        catch (Exception e)
    //        //        {
    //        //            throw e;
    //        //        }
    //        //        finally
    //        //        {
    //        //            ValueLock.ExitWriteLock();
    //        //        }
    //        //    }, token);
    //        //}

    //        /// <summary>
    //        /// Constructor which initializes Value with the default of TValue
    //        /// </summary>
    //        public Lockable() : this(default(TValue))
    //        {
    //        }
    //        /// <summary>
    //        /// Constructor which initializes Value with the specified value
    //        /// </summary>
    //        /// <param name="initialValue">The initial value for Value</param>
    //        public Lockable(TValue initialValue)
    //        {
    //            SetValue(initialValue);
    //        }
    //        /// <summary>
    //        /// Destructs the Lockable object and releases resources
    //        /// </summary>
    //        ~Lockable()
    //        {
    //            //ValueLock.Dispose();
    //        }
    //    }

    //    ///// <summary>
    //    ///// <para>
    //    ///// Provides thread safety via locking
    //    ///// </para>
    //    ///// </summary>
    //    ///// <typeparam name="TValue">The Type of the lockable value</typeparam>
    //    //public class Lockable<TValue>
    //    //{
    //    //    private readonly object _lockObject = new object();
    //    //    private TValue _value;
    //    //    /// <summary>
    //    //    /// The object used internally for lock statements
    //    //    /// </summary>
    //    //    public object LockObject => _lockObject;


    //    //    /// <summary>
    //    //    /// <para>
    //    //    /// Provides automatic locking during read/writes
    //    //    /// </para>
    //    //    /// </summary>
    //    //    public TValue Value
    //    //    {
    //    //        get
    //    //        {
    //    //            lock (_lockObject)
    //    //            {
    //    //                return _value;
    //    //            }
    //    //        }
    //    //        set
    //    //        {
    //    //            lock (_lockObject)
    //    //            {
    //    //                _value = value;
    //    //            }
    //    //        }
    //    //    }

    //    //    /// <summary>
    //    //    /// <para>
    //    //    /// To be used by caller, with LockObject, to batch read/writes under one lock)
    //    //    /// </para>
    //    //    /// </summary>
    //    //    public TValue UnlockedValue
    //    //    {
    //    //        get { return _value; }
    //    //        set { _value = value; }
    //    //    }

    //    //    /// <summary>
    //    //    /// Executes an action within a lock of the LockObject
    //    //    /// </summary>
    //    //    /// <param name="action">The action to call within a lock</param>
    //    //    public void ExecuteInLock(Action<TValue> action)
    //    //    {
    //    //        if (action == null)
    //    //            throw new ArgumentNullException(nameof(action));
    //    //        lock (_lockObject)
    //    //        {
    //    //            action?.Invoke(Value);
    //    //        }
    //    //    }
    //    //    /// <summary>
    //    //    /// Executes an action within a lock of the LockObject
    //    //    /// </summary>
    //    //    /// <param name="function">The function to call within a lock</param>
    //    //    public void ExecuteInLock(Func<TValue, TValue> function)
    //    //    {
    //    //        if (function == null)
    //    //            throw new ArgumentNullException(nameof(function));

    //    //        lock (_lockObject)
    //    //        {
    //    //            Value = function.Invoke(Value);
    //    //        }
    //    //    }
    //    //    /// <summary>
    //    //    /// Executes a task within a lock of the LockObject
    //    //    /// </summary>
    //    //    /// <param name="action">The action to call within a lock</param>
    //    //    public async void ExecuteInLockAsync(Action<TValue> action)
    //    //    {
    //    //        if (action == null)
    //    //            throw new ArgumentNullException(nameof(action));
    //    //        await Task.Run(() =>
    //    //        {
    //    //            lock (_lockObject)
    //    //            {
    //    //                action?.Invoke(Value);
    //    //            }
    //    //        });
    //    //    }
    //    //    /// <summary>
    //    //    /// Executes a task within a lock of the LockObject
    //    //    /// </summary>
    //    //    /// <param name="function">The function to call within a lock</param>
    //    //    public async void ExecuteInLockAsync(Func<TValue, TValue> function)
    //    //    {
    //    //        if (function == null)
    //    //            throw new ArgumentNullException(nameof(function));
    //    //        await Task.Run(() =>
    //    //        {
    //    //            lock (_lockObject)
    //    //            {
    //    //                Value = function.Invoke(Value);
    //    //            }
    //    //        });
    //    //    }

    //    //    /// <summary>
    //    //    /// <para>
    //    //    /// Constructor which initializes Value with the default of TValue
    //    //    /// </para>
    //    //    /// </summary>
    //    //    public Lockable() : this(default(TValue))
    //    //    {
    //    //    }
    //    //    /// <summary>
    //    //    /// Constructor which initializes Value with the specified value
    //    //    /// </summary>
    //    //    /// <param name="value">The initial value for Value</param>
    //    //    public Lockable(TValue value)
    //    //    {
    //    //        Value = value;
    //    //    }
    //    //}
}
