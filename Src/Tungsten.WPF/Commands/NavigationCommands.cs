using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Commands
{
    /// <summary>
    /// Contains two commands, one to NavigateTo and one to NavigateBack
    /// </summary>
    public class NavigationCommands
    {
        private PageFramework _pf;
        private W.WPF.Commands.RelayCommand<string> _navigateTo;
        private W.WPF.Commands.RelayCommand _navigateBack;
        /// <summary>
        /// A NavigateTo Command
        /// </summary>
        public System.Windows.Input.ICommand NavigateTo
        {
            get
            {
                if (_navigateTo == null)
                {
                    _navigateTo = new Commands.RelayCommand<string>(page => { _pf.NavigateTo(page); });
                }
                return _navigateTo;
            }
        }
        /// <summary>
        /// A NavigateBack Command
        /// </summary>
        public System.Windows.Input.ICommand NavigateBack
        {
            get
            {
                if (_navigateBack == null)
                {
                    _navigateBack = new Commands.RelayCommand(page => { _pf.NavigateBack(); });
                }
                return _navigateBack;
            }
        }
        /// <summary>
        /// Constructs a new NavigationCommands instance
        /// </summary>
        /// <param name="pageFramework"></param>
        public NavigationCommands(PageFramework pageFramework)
        {
            if (pageFramework == null)
                throw new ArgumentNullException("pageFramework cannot be null");
            _pf = pageFramework;
        }
    }
}
