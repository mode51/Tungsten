using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace W
{
    public class Property<TOwner, TValue> : PropertyBase<TOwner, TValue> where TOwner : class
    {
        public Property() : this(default(TOwner), default(TValue), null) { }
        public Property(TValue defaultValue) : this(default(TOwner), defaultValue, null) { }
        public Property(OnValueChangedDelegate onValueChanged = null) : this(default(TOwner), default(TValue), onValueChanged) { }

        public Property(TOwner owner) : this(owner, default(TValue), null) { }
        public Property(TOwner owner, TValue defaultValue) : this(owner, defaultValue, null) { }
        public Property(TOwner owner, OnValueChangedDelegate onValueChanged = null) : this(owner, default(TValue), onValueChanged) { }
        public Property(TOwner owner, TValue defaultValue, OnValueChangedDelegate onValueChanged)
        {
            Owner = owner;
            DefaultValue = defaultValue;
            LoadValue(defaultValue);
            OnValueChanged = onValueChanged;
        }
    }

    public class Property<TValue> : PropertyBase<Property<TValue>, TValue>
    {
        public Property() : this(default(TValue), null) { }
        public Property(TValue defaultValue) : this(defaultValue, null) { }
        public Property(OnValueChangedDelegate onValueChanged) : this(default(TValue), onValueChanged) { }
        public Property(TValue defaultValue, OnValueChangedDelegate onValueChanged)
        {
            Owner = this;
            DefaultValue = defaultValue;
            LoadValue(DefaultValue);
            OnValueChanged = onValueChanged;
        }
    }
}