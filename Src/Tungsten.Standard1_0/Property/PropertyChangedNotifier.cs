using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace W
{
    /// <summary>
    /// <para>
    /// This is a base class for supporting INotifyPropertyChanged
    /// </para>
    /// </summary>
    public abstract class PropertyChangedNotifier : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// <para>
        /// Override this method to provide Get functionality
        /// </para>
        /// </summary>
        /// <returns>Unless overridden, this function will always return null</returns>
        protected virtual object GetValue()
        {
            return null;
        }
        /// <summary>
        /// <para>
        /// Calls OnPropertyChanged.  This method does not make assignments.  Override this method to make assignments.
        /// </para>
        /// </summary>
        /// <param name="value">The new value</param>
        /// <param name="propertyName">The name of the caller (the property being set)</param>
        protected virtual void SetValue(object value, [CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(propertyName);
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
        /// <summary>
        /// <para>
        /// Raises the PropertyChanged event
        /// </para>
        /// </summary>
        /// <param name="sender">The sender is the owner of the property</param>
        /// <param name="propertyName">The name of the caller (the property which changed)</param>
        protected virtual void RaiseOnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}