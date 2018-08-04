using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace W
{
    //Add INotifyPropertyChanging (shell only when not supported by the platform)
    /// <summary>
    /// PropertySlim extends W.Lockable by adding support for INotifyPropertyChanged
    /// </summary>
    /// <typeparam name="TValue">The Type of Value</typeparam>
    public partial class PropertySlim<TValue>
#if !WINDOWS_PORTABLE && !NETCOREAPP1_0 && !NETSTANDARD1_0 && !NETSTANDARD1_1 && !NETSTANDARD1_2
        : INotifyPropertyChanging
#endif
    {
#if !WINDOWS_PORTABLE && !NETCOREAPP1_0 && !NETSTANDARD1_0 && !NETSTANDARD1_1 && !NETSTANDARD1_2
        public event PropertyChangingEventHandler PropertyChanging;
#endif
        /// <summary>
        /// <para>
        /// Raises the PropertyChanging event
        /// </para>
        /// </summary>
        /// <param name="sender">The sender is the owner of the property</param>
        /// <param name="propertyName">The name of the caller (the property which changing)</param>
        protected void RaiseOnPropertyChanging(object sender, string propertyName)
        {
#if !WINDOWS_PORTABLE && !NETCOREAPP1_0 && !NETSTANDARD1_0 && !NETSTANDARD1_1 && !NETSTANDARD1_2
            PropertyChanging?.Invoke(sender, new PropertyChangingEventArgs(propertyName));
#endif
        }
        /// <summary>
        /// <para>
        /// Calls RaisePropertyChanging to raise the PropertyChanging event
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the caller (the property which changed)</param>
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
#if !WINDOWS_PORTABLE && !NETCOREAPP1_0 && !NETSTANDARD1_0 && !NETSTANDARD1_1 && !NETSTANDARD1_2
            RaiseOnPropertyChanging(this, propertyName);
#endif
        }
    }

    //add INotifyPropertyChanged support
    public partial class PropertySlim<TValue> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// <para>
        /// Raises the PropertyChanged event
        /// </para>
        /// </summary>
        /// <param name="sender">The sender is the owner of the property</param>
        /// <param name="propertyName">The name of the caller (the property which changed)</param>
        protected void RaiseOnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// <para>
        /// Calls RaisePropertyChanged to raise the PropertyChanged event
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the caller (the property which changed)</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaiseOnPropertyChanged(this, propertyName);
        }
    }

    //add initialValue and Action<object, TValue, TValue> onValueChanged (called in OnValueChanged) overloads
    public partial class PropertySlim<TValue>
    {
        /// <summary>
        /// Construct a new PropertySlim with a default initial value
        /// </summary>
        public PropertySlim() : this(null) { }
        /// <summary>
        /// Concstruct a new PropertySlim
        /// </summary>
        /// <param name="initialValue">The initial value</param>
        public PropertySlim(TValue initialValue) : this(initialValue, null) { }
        /// <summary>
        /// Concstruct a new PropertySlim
        /// </summary>
        /// <param name="onValueChanged">Called when the value is changed</param>
        public PropertySlim(Action<object, TValue, TValue> onValueChanged) : this(default(TValue), onValueChanged) { }
        /// <summary>
        /// Concstruct a new PropertySlim
        /// </summary>
        /// <param name="initialValue">The initial value</param>
        /// <param name="onValueChanged">Called when the value is changed</param>
        public PropertySlim(TValue initialValue, Action<object, TValue, TValue> onValueChanged) : base(initialValue, onValueChanged) { }
    }

    // implicit conversions
    public partial class PropertySlim<TValue>
    {
        /// <summary>
        /// Implicit conversion from PropertySlim&lt;TValue&gt; to TValue
        /// </summary>
        /// <param name="property">The PropertySlim&lt;TValue&gt; from which to obtain the value</param>
        public static implicit operator TValue(PropertySlim<TValue> property)
        {
            return property.Value;
        }
        /// <summary>
        /// Implicit conversion from TValue to PropertySlim&lt;TValue&gt;
        /// </summary>
        /// <param name="value">The value from which to create a new PropertySlim&lt;TValue&gt;</param>
        public static implicit operator PropertySlim<TValue>(TValue value)
        {
            return new PropertySlim<TValue>(value);
        }
    }

    //root implementation
    public partial class PropertySlim<TValue> : Lockable<TValue>
    {
        /// <summary>
        /// <para>
        /// Calls OnPropertyChanged on assignment.  This is perfomred in a write lock.
        /// </para>
        /// </summary>
        /// <param name="value">The new value</param>
        protected override void SetValue(TValue value)
        {
            InLock(Threading.Lockers.LockTypeEnum.Write, oldValue =>
            {
                var shouldSet = !EqualityComparer<TValue>.Default.Equals(oldValue, value);
                if (shouldSet)
                {
                    OnPropertyChanging("Value");
                    State = value;
                    OnValueChanged(this, oldValue, value);
                    OnPropertyChanged("Value");
                }
            });
        }
    }
}
