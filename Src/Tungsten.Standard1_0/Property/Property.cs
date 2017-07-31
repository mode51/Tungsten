namespace W
{
    /// <summary>
    /// A generic Property with an owner
    /// </summary>
    /// <typeparam name="TOwner">The type of owner</typeparam>
    /// <typeparam name="TValue">The type of the property value</typeparam>
    public class Property<TOwner, TValue> : PropertyBase<TOwner, TValue>, IOwnedProperty where TOwner : class
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
        /// <param name="defaultValue">The default and initial value of the property</param>
        public Property(TValue defaultValue) : this(default(TOwner), defaultValue, null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="onValueChanged">A callback for when the property value changes</param>
        public Property(OnValueChangedDelegate onValueChanged = null) : this(default(TOwner), default(TValue), onValueChanged) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="defaultValue">The default and initial value of the property</param>
        /// <param name="onValueChanged">A callback for when the property value changes</param>
        public Property(TValue defaultValue, OnValueChangedDelegate onValueChanged = null) : this(default(TOwner), defaultValue, onValueChanged) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="owner">The owner of the property</param>
        public Property(TOwner owner) : this(owner, default(TValue), null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="owner">The owner of the property</param>
        /// <param name="defaultValue">The default and initial value of the property</param>
        public Property(TOwner owner, TValue defaultValue) : this(owner, defaultValue, null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="owner">The owner of the property</param>
        /// <param name="onValueChanged">A callback for when the property value changes</param>
        public Property(TOwner owner, OnValueChangedDelegate onValueChanged = null) : this(owner, default(TValue), onValueChanged) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="owner">The owner of the property</param>
        /// <param name="defaultValue">The default and initial value of the property</param>
        /// <param name="onValueChanged">A callback for when the property value changes</param>
        public Property(TOwner owner, TValue defaultValue, OnValueChangedDelegate onValueChanged)
        {
            Owner = owner;
            DefaultValue = defaultValue;
            LoadValue(defaultValue);
            OnValueChanged = onValueChanged;
        }
    }

    /// <summary>
    /// A generic Property with no owner (self-owned)
    /// </summary>
    /// <typeparam name="TValue">The type of the property value</typeparam>
    public class Property<TValue> : PropertyBase<Property<TValue>, TValue>
    {
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        public Property() : this(default(TValue), null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="defaultValue">The default and initial value of the property value</param>
        public Property(TValue defaultValue) : this(defaultValue, null) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="onValueChanged">A callback for when the property value changes</param>
        public Property(OnValueChangedDelegate onValueChanged) : this(default(TValue), onValueChanged) { }
        /// <summary>
        /// Constructs a new Property
        /// </summary>
        /// <param name="defaultValue">The default and initial value of the property value</param>
        /// <param name="onValueChanged">A callback for when the property value changes</param>
        public Property(TValue defaultValue, OnValueChangedDelegate onValueChanged)
        {
            Owner = this;
            DefaultValue = defaultValue;
            LoadValue(DefaultValue);
            OnValueChanged = onValueChanged;
        }
    }
}