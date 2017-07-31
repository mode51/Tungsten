using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace W.WPF
{
    /// <summary>
    /// Minimum requirement to host Tungsten.WPF Pages
    /// </summary>
    public interface IPageHost
    {
        ///// <summary>
        ///// Get or set the currently displayed Page
        ///// </summary>
        //PageWrapper ActivePage { get; set; }

        /// <summary>
        /// A handle to the IPageHost's dispatcher
        /// </summary>
        System.Windows.Threading.Dispatcher Dispatcher { get; }
    }
}
