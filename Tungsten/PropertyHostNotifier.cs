namespace W
{
    /// <summary>
    /// <para>
    /// Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality
    /// Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support
    /// </para>
    /// </summary>
    public class PropertyHostNotifier : PropertyChangedNotifier
    {
        /// <summary>
        /// Finds all Properties and checks their IsDirty flag
        /// </summary>
        /// <returns>True if any Property's IsDirty flag is true. Otherwise false.</returns>
        public bool IsDirty { get { return PropertyHostMethods.IsDirty(this); } }

        /// <summary>
        /// Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean())
        /// </summary>
        public void MarkAsClean()
        {
            PropertyHostMethods.MarkAsClean(this);
        }

        /// <summary>
        /// Calls PropertyHostMethods.InitializeProperties so you don't have to
        /// </summary>
        public PropertyHostNotifier()
        {
            PropertyHostMethods.InitializeProperties(this);
        }
    }
}