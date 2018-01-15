namespace W
{
    /// <summary>
    /// <para>
    /// Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality
    /// Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)
    /// </para>
    /// </summary>
    public class PropertyHost
    {
        /// <summary>
        /// Finds all Properties and checks their IsDirty flag
        /// </summary>
        /// <returns>True if any Property's IsDirty flag is true. Otherwise false.</returns>
        public bool IsDirty { get { return PropertyHostExtensions.PropertyHostExtensions.IsDirty(this); } }

        /// <summary>
        /// Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean())
        /// </summary>
        public void MarkAsClean()
        {
            PropertyHostExtensions.PropertyHostExtensions.MarkAsClean(this);
        }

        /// <summary>
        /// Calls PropertyHostExtensions.InitializeProperties so you don't have to
        /// </summary>
        public PropertyHost()
        {
            PropertyHostExtensions.PropertyHostExtensions.InitializeProperties(this);
        }
    }
}
