using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace W
{
    //Add INotifyPropertyChanging (shell only when not supported by the platform)
    public abstract partial class PropertySlim<TValue>
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
    public abstract partial class PropertySlim<TValue> : INotifyPropertyChanged
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

    //root implementation
    public abstract partial class PropertySlim<TValue> : Lockable<TValue>
    {
        /// <summary>
        /// <para>
        /// Calls OnPropertyChanged on assignment
        /// </para>
        /// </summary>
        /// <param name="value">The new value</param>
        /// <param name="propertyName">The name of the caller (the property being set)</param>
        protected override void SetValue(TValue value)
        {
            InLock(oldValue =>
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

    //add initialValue and Action<object, TValue, TValue> onValueChanged (called in OnValueChanged) overloads
    public abstract partial class PropertySlim<TValue>
    {
        public PropertySlim() : this(null) { }
        public PropertySlim(TValue initialValue) : this(initialValue, null) { }
        public PropertySlim(Action<object, TValue, TValue> onValueChanged) : this(default(TValue), onValueChanged) { }
        public PropertySlim(TValue initialValue, Action<object, TValue, TValue> onValueChanged) : base(initialValue, onValueChanged) { }
    }
}
