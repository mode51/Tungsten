using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace W.WPF
{
    /// <summary>
    /// Conveniency wrapper for a Page
    /// </summary>
    public class PageWrapper
    {
        //public string Name { get; set; } //this should be unique (it's NOT the FullName)
        /// <summary>
        /// The class Type of this Page
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// The UI FrameworkElement of this Page
        /// </summary>
        public FrameworkElement FrameworkElement { get; set; }
        /// <summary>
        /// The UI FrameworkElement cast as IPage
        /// </summary>
        public IPage AsPage => (FrameworkElement as IPage);
        //public Lazy<IPage> Page { get; set; }
    }
}
