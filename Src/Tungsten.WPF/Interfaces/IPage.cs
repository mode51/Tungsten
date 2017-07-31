using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF
{
    /// <summary>
    /// Minimum requirement for a Tungsten.WPF Page
    /// </summary>
    public interface IPage
    {
        //enforce using PageFramework instead
        //void Select();

        /// <summary>
        /// Called when the Page has been nagivated to
        /// </summary>
        /// <param name="args"></param>
        void OnNavigateTo(IPage previousPage, params object[] args);
        /// <summary>
        /// Called when the Page has been nagivated away from
        /// </summary>
        /// <param name="args"></param>
        void OnNavigateFrom(IPage nextPage);
    }
}
