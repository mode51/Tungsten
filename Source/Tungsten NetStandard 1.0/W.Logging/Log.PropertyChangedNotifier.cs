using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace W.Logging
{
    public static partial class Log
    {
        /// <summary>
        /// Provides 
        /// </summary>
        public class PropertyHost : INotifyPropertyChanged
        {
            /// <summary>
            /// Raised when the value of a property has changed
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;
            public delegate void SetValueDelegate();
            protected virtual void SetValue(object owner, SetValueDelegate assignValue, [CallerMemberName] string callerMemberName = "")
            {
                assignValue.Invoke();
                RaiseOnPropertyChanged(owner, callerMemberName);
            }
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
        }
    }
}
