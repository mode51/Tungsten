using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace W
{
    public abstract partial class PropertyBase<TOwner, TValue> : PropertySlim<TValue>, IProperty<TValue>, IDisposable
    {
        #region IsDirty
        private LockableSlim<bool> _isDirty = new LockableSlim<bool>();
        /// <summary>
        /// True if Value has changed since initialization or since the last call to MarkAsClean
        /// </summary>
        public bool IsDirty { get { return _isDirty.Value; } set { _isDirty.Value = value; } }
        //public void MarkAsClean() { IsDirty = false; }
        #endregion

        #region Owner
        private readonly LockableSlim<TOwner> _owner = new LockableSlim<TOwner>(default(TOwner));
        /// <summary>
        /// The property owner
        /// </summary>
        public TOwner Owner
        {
            get { return _owner.Value; }
            protected set { _owner.Value = value; }
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
            InLock(Threading.Lockers.LockTypeEnum.Write, oldValue =>
            {
                var shouldSet = !EqualityComparer<TValue>.Default.Equals(oldValue, DefaultValue);
                if (shouldSet)
                {
                    if (raise)
                        OnPropertyChanging("Value");
                    State = DefaultValue;
                    IsDirty = false;
                    if (raise)
                    {
                        OnValueChanged(Owner, oldValue, State);
                        OnPropertyChanged("Value");
                    }
                }
            });
        }
        #endregion

        /// <summary>
        /// <para>
        /// Calls RaisePropertyChanging to raise the PropertyChanging event
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the caller (the property which changed)</param>
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            //base.OnPropertyChanging(propertyName);
            //overridden so that we can specify the new Owner
            RaiseOnPropertyChanging(Owner, propertyName);
        }
        /// <summary>
        /// <para>
        /// Calls RaisePropertyChanged to raise the PropertyChanged event
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the caller (the property which changed)</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //base.OnPropertyChanged(propertyName);
            //overridden so that we can specify the new Owner
            RaiseOnPropertyChanged(Owner, propertyName);
            IsDirty = true;
            var ph = Owner as PropertyHost;
            if (ph != null)
                ph.IsDirtyFlag.Value = true;
        }
        /// <summary>
        /// Calls RaiseValueChanged to raise the ValueChanged event
        /// </summary>
        /// <param name="sender">The property owner</param>
        /// <param name="oldValue">The previous value</param>
        /// <param name="newValue">The current value</param>
        protected override void OnValueChanged(object sender, TValue oldValue, TValue newValue)
        {
            base.OnValueChanged(Owner, oldValue, newValue);
        }

        /// <summary>
        /// Raises the PropertyChanged event regardless of whether the value has changed or not
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        public override void ForcePropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaiseOnPropertyChanged(Owner, propertyName);
        }

        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase() : this(default(TOwner), default(TValue), null) { }
        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase(TOwner owner) : this(owner, default(TValue), null) { }
        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase(TValue defaultValue) : this(default(TOwner), default(TValue), null) { }
        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase(TOwner owner, TValue defaultValue) : this(owner, defaultValue, null) { }
    }
    //add initialValue and Action<object, TValue, TValue> onValueChanged (called in OnValueChanged) overloads
    public partial class PropertyBase<TOwner, TValue>
    {
        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase(Action<object, TValue, TValue> onValueChanged) : this(default(TOwner), default(TValue), onValueChanged) { }
        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase(TOwner owner, Action<object, TValue, TValue> onValueChanged) : this(owner, default(TValue), onValueChanged) { }
        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase(TValue defaultValue, Action<object, TValue, TValue> onValueChanged) : this(default(TOwner), default(TValue), onValueChanged) { }
        /// <summary>
        /// Construct a new PropertyBase
        /// </summary>
        protected PropertyBase(TOwner owner, TValue defaultValue, Action<object, TValue, TValue> onValueChanged) : base(defaultValue, onValueChanged)
        {
            Owner = owner;
            DefaultValue = defaultValue;
            LoadValue(defaultValue);
        }
    }
}