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
        Property<W.WPF.Models.ModelBase, bool> IsBusy { get; }
        /// <summary>
        /// Get or set a title string which can be displayed while busy
        /// </summary>
        Property<string> BusyTitle { get; }
        /// <summary>
        /// Get or set a message to display while busy
        /// </summary>
        Property<string> BusyMessage { get; }

        Property<System.Windows.Visibility> BusyVisibility { get; }
    }
}
