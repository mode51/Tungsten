using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace W.WPF.Commands
{
    ///<summary>
    /// Simplifies creating a WPF Command
    ///<para>RelayCommand _saveCommand;
    ///public ICommand SaveCommand
    ///{
    ///    get
    ///    {
    ///        if (_saveCommand == null)
    ///        {
    ///            _saveCommand = new RelayCommand(param => this.Save(), param => this.CanSave);
    ///        }
    ///        return _saveCommand;
    ///    }
    ///}
    /// </para>
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        /// <summary>
        /// Constructs a new RelayCommand
        /// </summary>
        /// <param name="execute">The action to execute</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {

        }
        /// <summary>
        /// Constructs a new RelayCommand
        /// </summary>
        /// <param name="execute">The action to execute</param>
        /// <param name="canExecute">Predicate to determine if the command can execute</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = execute;
            _canExecute = canExecute;
        }
        
        /// <summary>
        /// Determines whether the command can execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        /// <summary>
        /// Called when the value of CanExecute changes
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        //use:
        //RelayCommand _saveCommand;
        //public ICommand SaveCommand
        //{
        //    get
        //    {
        //        if (_saveCommand == null)
        //        {
        //            _saveCommand = new RelayCommand(param => this.Save(), param => this.CanSave);
        //        }
        //        return _saveCommand;
        //    }
        //}
    }
}