namespace W
{
    /// <summary>
    /// The base interface which Property must support
    /// </summary>
    /// <typeparam name="TValue">The type of value for the property</typeparam>
    public interface IProperty<TValue> : IProperty
    {
        /// <summary>
        /// The value of the property
        /// </summary>
        TValue Value { get; set; }
    }

    /// <summary>
    /// The base interface which Property must support
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// True if the property's value has changed since initialization or since the last call to MarkAsClean
        /// </summary>
        bool IsDirty { get; set; }

        /// <summary>
        /// Forces the property to raise the PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        void ForcePropertyChanged(string propertyName);
    }
}
