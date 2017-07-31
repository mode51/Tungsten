namespace W.WPF
{
    /// <summary>
    /// Contains information related to a busy flag
    /// </summary>
    public class Busy : IBusy
    {
        /// <summary>
        /// Get or set the IsBusy flag
        /// </summary>
        public Property<Busy, bool> IsBusy { get; } = new Property<Busy, bool>((m, oldValue, newValue) => { m.Visibility.Value = newValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden; m.IsNotBusy.Value = !newValue; });
        /// <summary>
        /// Simply the reverse of IsBusy
        /// </summary>
        public Property<bool> IsNotBusy { get; } = new Property<bool>(true);
        /// <summary>
        /// Get or set a title string which can be displayed while busy
        /// </summary>
        public Property<string> Title { get; } = new Property<string>("Loading");
        /// <summary>
        /// Get or set a message to display while busy
        /// </summary>
        public Property<string> Message { get; } = new Property<string>("");
        /// <summary>
        /// Visible if IsBusy.Value is True, otherwise Hidden
        /// </summary>
        public Property<System.Windows.Visibility> Visibility { get; } = new Property<System.Windows.Visibility>(System.Windows.Visibility.Hidden);

        /// <summary>
        /// Constructs a BusyIndicator
        /// </summary>
        public Busy()
        {
            this.InitializeProperties();
        }
    }
}
