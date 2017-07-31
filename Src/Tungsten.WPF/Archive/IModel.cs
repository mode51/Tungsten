using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace W.WPF
{
    /// <summary>
    /// Minimum requirements for a Tungsten.WPF model
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// The UI owner of the model
        /// </summary>
        Property<FrameworkElement> UIHandle { get; }
        //void SetControl(object control);
    }
}
