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
        private W.Threading.Lockers.MonitorLocker _locker = new Threading.Lockers.MonitorLocker();
        /// <summary>
        /// Finds all Properties and checks their IsDirty flag
        /// </summary>
        /// <returns>True if any Property's IsDirty flag is true. Otherwise false.</returns>
        public bool IsDirty { get { return PropertyHostExtensions.IsDirty(this); } }

        /// <summary>
        /// Set by child Property members when they become dirty
        /// </summary>
        /// <returns>True if any Property's IsDirty flag is true. Otherwise false.</returns>
        public PropertySlim<bool> IsDirtyFlag { get; } = new PropertySlim<bool>();

        /// <summary>
        /// Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean())
        /// </summary>
        public void MarkAsClean()
        {
            _locker.InLock(() =>
            {
                PropertyHostExtensions.MarkAsClean(this);
                IsDirtyFlag.Value = false;
            });
        }
        /// <summary>
        /// Raises the PropertyChanged event, for each Property, regardless of whether the value has changed or not
        /// </summary>
        public void ForcePropertyChanged()
        {
            PropertyHostExtensions.ForcePropertyChanged(this);
        }

        /// <summary>
        /// Calls PropertyHostExtensions.InitializeProperties so you don't have to
        /// </summary>
        public PropertyHost()
        {
            PropertyHostExtensions.InitializeProperties(this);
        }
    }
}
