namespace W
{
    /// <summary><para>
    /// Used by PropertyHostMethods.InitializeProperties to find properties on which to set the owner.  This interface is not used by self-owned properties.
    /// </para></summary>
    public interface IOwnedProperty
    {
        /// <summary>
        /// Sets the property owner to the specified value
        /// </summary>
        /// <param name="owner">The new property owner</param>
        void SetOwner(object owner);
    }
}