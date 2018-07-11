using System;

namespace W
{
    /// <summary>
    /// A Property with no owner (self-owned)
    /// </summary>
    /// <typeparam name="TValue">The type of the property value</typeparam>
    public partial class Property<TValue> : PropertyBase<Property<TValue>, TValue>
    {
        public Property() : this(default(TValue)) { }
        public Property(TValue defaultValue) : base(null, defaultValue)
        {
            Owner = this;
        }

    }
    //add initialValue and Action<object, TValue, TValue> onValueChanged (called in OnValueChanged) overloads
    public partial class Property<TValue>
    {
        public Property(Action<object, TValue, TValue> onValueChanged) : base(null, default(TValue), onValueChanged) { }
        public Property(TValue defaultValue, Action<object, TValue, TValue> onValueChanged) : base(null, defaultValue, onValueChanged) { }
    }
    // implicit conversions
    public partial class Property<TValue>
    {
        /// <summary>
        /// Implicit conversion from Property&lt;TValue&gt; to TValue
        /// </summary>
        /// <param name="property">The Property&lt;TValue&gt; from which to obtain the value</param>
        public static implicit operator TValue(Property<TValue> property)
        {
            return property.Value;
        }
        /// <summary>
        /// Implicit conversion from TValue to Property&lt;TValue&gt;
        /// </summary>
        /// <param name="value">The value from which to create a new Property&lt;TValue&gt;</param>
        public static implicit operator Property<TValue>(TValue value)
        {
            return new Property<TValue>(value);
        }
    }

    /// <summary>
    /// A generic Property with an owner
    /// </summary>
    /// <typeparam name="TOwner">The type of owner</typeparam>
    /// <typeparam name="TValue">The type of the property value</typeparam>
    public partial class Property<TOwner, TValue> : PropertyBase<TOwner, TValue>, IOwnedProperty where TOwner : class
    {
#region IOwnedProperty
        void IOwnedProperty.SetOwner(object owner)
        {
            Owner = owner as TOwner;
        }
#endregion

        /// <summary>
        /// Constructs a new Property
        /// </summary>
        public Property() : this(default(TOwner), default(TValue), null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="owner">The owner of the property</param>
        public Property(TOwner owner) : this(owner, default(TValue), null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="defaultValue">The default and initial value of the property</param>
        public Property(TValue defaultValue) : this(default(TOwner), defaultValue, null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="owner">The owner of the property</param>
        /// <param name="defaultValue">The default and initial value of the property</param>
        public Property(TOwner owner, TValue defaultValue) : this(owner, defaultValue, null) { }
    }
    //add initialValue and Action<object, TValue, TValue> onValueChanged (called in OnValueChanged) overloads
    public partial class Property<TOwner, TValue>
    {
        public Property(Action<object, TValue, TValue> onValueChanged) : this(default(TOwner), default(TValue), onValueChanged) { }
        public Property(TOwner owner, Action<object, TValue, TValue> onValueChanged) : this(owner, default(TValue), onValueChanged) { }
        public Property(TValue defaultValue, Action<object, TValue, TValue> onValueChanged) : this(default(TOwner), default(TValue), onValueChanged) { }
        public Property(TOwner owner, TValue defaultValue, Action<object, TValue, TValue> onValueChanged) : base(owner, defaultValue, onValueChanged) { }
    }
    // implicit conversions
    public partial class Property<TOwner, TValue>
    {
        /// <summary>
        /// Implicit conversion from Property&lt;TOwner, TValue&gt; to TValue
        /// </summary>
        /// <param name="property">The Property&lt;TOwner, TValue&gt; from which to obtain the value</param>
        public static implicit operator TValue(Property<TOwner, TValue> property)
        {
            return property.Value;
        }
        /// <summary>
        /// Implicit conversion from TValue to Property&lt;TOwner, TValue&gt;
        /// </summary>
        /// <param name="value">The value from which to create a new Property&lt;TOwner, TValue&gt;</param>
        public static implicit operator Property<TOwner, TValue>(TValue value)
        {
            return new Property<TOwner, TValue>(value);
        }
    }
}