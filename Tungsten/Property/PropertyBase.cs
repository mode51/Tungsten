using System.Threading;

namespace W
{
    public abstract class PropertyBase<TOwner, TValue> : PropertyChangedNotifier, IProperty<TValue> where TOwner : class
    {
        //These delegates/events require the base class to support TOwner (otherwise, the owner parameter would have to be of object type)
        public delegate void PropertyValueChangingDelegate(TOwner owner, TValue oldValue, TValue newValue, ref bool cancel);
        public delegate void PropertyValueChangedDelegate(TOwner sender, TValue oldValue, TValue newValue);
        public delegate void OnValueChangedDelegate(TOwner owner, TValue oldValue, TValue newValue);

        /// <summary>
        /// Raised after Value has changed
        /// </summary>
        public event PropertyValueChangedDelegate ValueChanged;
        /// <summary>
        /// Raised before Value has changed.  To prevent Value from changing set cancel to true.
        /// </summary>
        public event PropertyValueChangingDelegate ValueChanging;
        protected OnValueChangedDelegate OnValueChanged; //Callback for use in the constructor (non-event-based callback)

        private readonly Lockable<bool> _isDirty = new Lockable<bool>();
        private readonly ManualResetEvent _mre = new ManualResetEvent(false);

        #region IProperty
        public bool IsDirty
        {
            get { return _isDirty.Value; }
            set { _isDirty.Value = value; }
        }
        #endregion

        #region Owner
        private readonly Lockable<TOwner> _owner = new Lockable<TOwner>(default(TOwner));
        public TOwner Owner
        {
            get { return _owner.Value; }
            set { _owner.Value = value; }
        }
        #endregion

        #region Default Value Support
        public TValue DefaultValue { get; set; } = default(TValue);
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

        protected override object GetValue()
        {
            return _value.Value;
        }
        protected override void SetValue(object value, string callerMemberName = "")
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
        protected override void OnPropertyChanged(string propertyName = null)
        {
            //override so that we can pass in the correct Owner
            RaiseOnPropertyChanged(Owner, propertyName);
        }

        protected virtual void ExecuteOnValueChanged(TValue oldValue, TValue newValue)
        {
            OnValueChanged?.Invoke(Owner, oldValue, newValue);
        }
        protected virtual void RaisePropertyValueChanging(TValue oldValue, TValue newValue, ref bool cancel)
        {
            ValueChanging?.Invoke(Owner, oldValue, newValue, ref cancel);
        }
        protected virtual void RaisePropertyValueChanged(TValue oldValue, TValue newValue)
        {
            ValueChanged?.Invoke(Owner, oldValue, newValue);
        }
    }
}