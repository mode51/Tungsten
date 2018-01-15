using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace W
{
    //internal static class PropertyMethods
    //{
    //    /// <summary>
    //    /// Provides thread-safe assignment of a variable and invoking a handler when the change occurs
    //    /// </summary>
    //    /// <returns>True if the assignment was made, otherwise false</returns>
    //    internal static bool SetValue<TValue>(object lockObj, ref TValue property, TValue value, Action<TValue, TValue, string> onSetProperty, [CallerMemberName] string propertyName = null)
    //    {
    //        if (EqualityComparer<TValue>.Default.Equals(property, value)) return false;
    //        lock (lockObj)
    //        {
    //            var oldValue = property;
    //            property = value;
    //            onSetProperty?.Invoke(oldValue, value, propertyName);
    //            return true;
    //        }
    //    }
    //    /// <summary>
    //    /// Provides thread-safe assignment of a variable
    //    /// </summary>
    //    /// <returns>True if the assignment was made, otherwise false</returns>
    //    internal static bool LoadValue<TValue>(object lockObj, ref TValue property, TValue value)
    //    {
    //        if (EqualityComparer<TValue>.Default.Equals(property, value)) return false;
    //        lock (lockObj)
    //        {
    //            property = value;
    //            return true;
    //        }
    //    }

    //    /// <summary>
    //    /// Provides thread-safe assignment of a Lockable object and invoking a handler when the change occurs
    //    /// </summary>
    //    /// <returns>True if the assignment was made, otherwise false</returns>
    //    internal static bool SetValue<TValue>(LockableSlim<TValue> property, TValue newValue, Action<TValue, TValue, string> onSetProperty, [CallerMemberName] string propertyName = null)
    //    {
    //        if (EqualityComparer<TValue>.Default.Equals(property.Value, newValue)) return false;
    //        property.InLock(v =>
    //        {
    //            property.Value = newValue;
    //            onSetProperty?.Invoke(v, newValue, propertyName);
    //            return newValue;
    //        });
    //        return true;
    //        //lock (property.LockObject)
    //        //{
    //        //    var oldValue = property.UnlockedValue;
    //        //    property.UnlockedValue = value;
    //        //    onSetProperty?.Invoke(oldValue, value, propertyName);
    //        //    return true;
    //        //}
    //    }
    //    /// <summary>
    //    /// Provides thread-safe assignment of a Lockable object
    //    /// </summary>
    //    /// <returns>True if the assignment was made, otherwise false</returns>
    //    internal static bool LoadValue<TValue>(LockableSlim<TValue> property, TValue newValue)
    //    {
    //        if (EqualityComparer<TValue>.Default.Equals(property.Value, newValue)) return false;
    //        property.Value = newValue;
    //        return true;
    //    }
    //}
}