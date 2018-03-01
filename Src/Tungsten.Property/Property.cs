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
}