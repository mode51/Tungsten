using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace W
{
    //These delegates/events require the base class to support TOwner (otherwise, the owner parameter would have to be of object type)
    /// <summary>
    /// Raised prior to the value of the property changing.  Allows the programmer to cancel the change.
    /// </summary>
    /// <param name="owner">The owner of the property</param>
    /// <param name="oldValue">The old value</param>
    /// <param name="newValue">The expected new value</param>
    /// <param name="cancel">Set to True to prevent the property value from changing</param>
    public delegate void PropertyValueChangingDelegate(TOwner owner, TValue oldValue, TValue newValue, ref bool cancel);
    /// <summary>
    /// Raised when the value of the property changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public delegate void PropertyValueChangedDelegate(TOwner sender, TValue oldValue, TValue newValue);
    /// <summary>
    /// Used by the constructor to handle the property change via a callback rather than the events
    /// </summary>
    /// <param name="owner">The property owner</param>
    /// <param name="oldValue">The previous value</param>
    /// <param name="newValue">The new value</param>
    public delegate void OnValueChangedDelegate(TOwner owner, TValue oldValue, TValue newValue);

    /// <summary>
    /// Provides the functionality for the Property classes
    /// </summary>
    /// <typeparam name="TOwner">The type of the property owner</typeparam>
    /// <typeparam name="TValue">The type of the property value</typeparam>
    public abstract class PropertyBase<TOwner, TValue> : PropertyChangedNotifier, IDisposable, IProperty<TValue> where TOwner : class
    {
        /// <summary>
        /// Raised after Value has changed
        /// </summary>
        public event PropertyValueChangedDelegate ValueChanged;
        /// <summary>
        /// Raised before Value has changed.  To prevent Value from changing set cancel to true.
        /// </summary>
        public event PropertyValueChangingDelegate ValueChanging;
        /// <summary>
        /// Callback type for use in the constructor (if one wants to avoid using the event)
        /// </summary>
        protected OnValueChangedDelegate OnValueChanged;

        private readonly Lockable<bool> _isDirty = new Lockable<bool>();
        private ManualResetEvent _mre = new ManualResetEvent(false);

        #region IProperty
        /// <summary>
        /// True if Value has changed since initialization or since the last call to MarkAsClean
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty.Value; }
            set { _isDirty.Value = value; }
        }
        #endregion

        #region Owner
        private readonly Lockable<TOwner> _owner = new Lockable<TOwner>(default(TOwner));
        /// <summary>
        /// The property owner
        /// </summary>
        public TOwner Owner
        {
            get { return _owner.Value; }
            set { _owner.Value = value; }
        }
        #endregion

        #region Default Value Support
        /// <summary><para>
        /// Allows the programmer to assign a default value which can be reset via the ResetToDefaultValue method. This value does not have to be the initial value.
        /// </para></summary>
        public TValue DefaultValue { get; set; } = default(TValue);
        /// <summary>
        /// Resets the Value to the value provided by DefaultValue
        /// </summary>
        /// <param name="raise">If True, raise OnPropertyChanged event and call the OnValueChanged callback</param>
        public void ResetToDefaultValue(bool raise)
        {
            var oldValue = Value;
            PropertyMethods.LoadValue(_value, DefaultValue);
            if (raise)
                RaiseOnPropertyChanged(Owner, "Value");
            if (raise)
                ExecuteOnValueChanged(oldValue, Value);
            IsDirty = false;
        }
        #endregion

        #region Value
        private readonly Lockable<TValue> _value = new Lockable<TValue>(default(TValue));
        /// <summary>
        /// Get/Set the actual value of the Property
        /// </summary>
        public TValue Value
        {
            get
            {
                return (TValue)GetValue();
            }
            set
            {
                SetValue(value);
            }
        }
        #endregion

        /// <summary>
        /// Allows the caller to suspend it's thread until Value changes
        /// </summary>
        /// <param name="msTimeout"></param>
        /// <returns></returns>
        public bool WaitForChanged(int msTimeout = 0)
        {
            bool result = false;
            if (msTimeout > 0)
                result = _mre.WaitOne(msTimeout);
            else
                result = _mre.WaitOne();
            return result;
        }

        /// <summary>
        /// Loads Value without raising events or calling the OnValueChanged callback
        /// </summary>
        /// <remarks>Calling LoadValue sets IsDirty to false</remarks>
        /// <param name="value">The new value</param>
        public void LoadValue(TValue value)
        {
            PropertyMethods.LoadValue(_value, value);
            IsDirty = false;
        }

        /// <summary>
        /// Gets the property value
        /// </summary>
        /// <returns>The property value</returns>
        protected override object GetValue()
        {
            return _value.Value;
        }
        /// <summary>
        /// Sets the property value
        /// </summary>
        /// <param name="value">The new property value</param>
        /// <param name="callerMemberName">For logging purposes; it's not necessary to set</param>
        protected override void SetValue(object value, [CallerMemberName] string callerMemberName = "")
        {
            bool cancel = false;
            RaisePropertyValueChanging(Value, (TValue)value, ref cancel);
            if (!cancel)
            {
                var oldValue = _value.Value;
                PropertyMethods.SetValue<TValue>(_value, (TValue)value, null);
                IsDirty = true;
                RaisePropertyValueChanged(oldValue, (TValue)value);
                //call the base implementation to raise the OnPropertyChanged
                base.SetValue(value, callerMemberName);
                //now call our callback
                ExecuteOnValueChanged(oldValue, (TValue)value);
                _mre.Set();
                _mre.Reset();
            }
        }
        /// <summary>
        /// Raises the OnPropertyChanged event
        /// </summary>
        /// <param name="propertyName">The name of the property which changed</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //override so that we can pass in the correct Owner
            RaiseOnPropertyChanged(Owner, propertyName);
        }
        /// <summary>
        /// Calls the OnValueChanged callback
        /// </summary>
        /// <param name="oldValue">The old property value</param>
        /// <param name="newValue">The new property value</param>
        protected virtual void ExecuteOnValueChanged(TValue oldValue, TValue newValue)
        {
            OnValueChanged?.Invoke(Owner, oldValue, newValue);
        }
        /// <summary>
        /// Raises the ValueChanging event
        /// </summary>
        /// <param name="oldValue">The old property value</param>
        /// <param name="newValue">The expected new property value</param>
        /// <param name="cancel">Set to True to cancel the property change</param>
        protected virtual void RaisePropertyValueChanging(TValue oldValue, TValue newValue, ref bool cancel)
        {
            ValueChanging?.Invoke(Owner, oldValue, newValue, ref cancel);
        }
        /// <summary>
        /// Raises the PropertyValueChanged event
        /// </summary>
        /// <param name="oldValue">The old property value</param>
        /// <param name="newValue">The new property value</param>
        protected virtual void RaisePropertyValueChanged(TValue oldValue, TValue newValue)
        {
            ValueChanged?.Invoke(Owner, oldValue, newValue);
        }
        /// <summary>
        /// Disposes the object and releases resources
        /// </summary>
        public void Dispose()
        {
            _mre?.Dispose();
            _mre = null;
        }
        /// <summary>
        /// Disposes the PropertyBase
        /// </summary>
        ~PropertyBase()
        {
            Dispose();
        }
    }
}