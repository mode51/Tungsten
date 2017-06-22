using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF
{
    /// <summary>
    /// Minimum functionality required to indicate if an object is busy
    /// </summary>
    public interface IBusy
    {
        /// <summary>
        /// Get or set the IsBusy flag
        /// </summary>
        Property<Busy, bool> IsBusy { get; }
        /// <summary>
        /// Get or set a title string which can be displayed while busy
        /// </summary>
        Property<string> Title { get; }
        /// <summary>
        /// Get or set a message to display while busy
        /// </summary>
        Property<string> Message { get; }
        /// <summary>
        /// Visible if IsBusy.Value is True, otherwise Hidden
        /// </summary>
        Property<System.Windows.Visibility> Visibility { get; }
    }
}
