using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace W
{
    public class PropertyHost
    {
       public bool IsDirty => PropertyHostMethods.IsDirty(this);

        public void MarkAsClean()
        {
            PropertyHostMethods.MarkAsClean(this);
        }

        //protected bool SetValue<TValue>(object lockObj, ref TValue property, TValue value, Action<TValue, TValue, string> onSetProperty, [CallerMemberName] string propertyName = null)
        //{
        //    var result = PropertyMethods.SetValue(lockObj, ref property, value, onSetProperty, propertyName);
        //    if (result)
        //        _isDirty.Value = true;
        //    return result;
        //}

        //protected bool LoadValue<TValue>(object lockObj, ref TValue property, TValue value)
        //{
        //    return PropertyMethods.LoadValue(lockObj, ref property, value);
        //}
        public PropertyHost()
        {
            PropertyHostMethods.InitializeProperties(this);
        }
    }
}
