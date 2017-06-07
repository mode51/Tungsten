using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using W.Logging;
using W.WPF.Core;

namespace W.WPF.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class Commander //<TModel> where TModel : class, IBusy, new()
    {
        private CancellationTokenSource _executeCTS;
        private Action<CancellationToken> _onExecuteStarting;
        private Action<CancellationToken> _onExecute;
        private Action<CancellationToken> _onExecuteComplete;

        //private ControlModelBase<TModel> _parent;
        private ICommand _command;

        private async Task ExecuteAsync(Action<CancellationToken> onBeforeExecute, Action<CancellationToken> onExecute, Action<CancellationToken> onAfterExecute, Action<bool> onComplete = null, CancellationTokenSource cts = null)
        {
            if (cts == null)
                cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            await Task.Run(() =>
            {
                bool success = false;
                try
                {
                    onBeforeExecute?.Invoke(cancellationToken);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                if (cancellationToken.IsCancellationRequested)
                    return;
                try
                {
                    onExecute?.Invoke(cancellationToken);
                    success = true;
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                if (cancellationToken.IsCancellationRequested)
                    return;

                try
                {
                    onAfterExecute?.Invoke(cancellationToken);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                if (cancellationToken.IsCancellationRequested)
                    return;
                try
                {
                    onComplete?.Invoke(success);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }, cancellationToken);
        }
        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="parameter">Ignored heere.  May be used by overridden implementations.</param>
        protected virtual async void OnCommand(object parameter)
        {
            await Execute();
        }
        /// <summary>
        /// Returns True
        /// </summary>
        /// <param name="parameter">Ignored here.  May be used by overridden implementations.</param>
        /// <returns></returns>
        protected virtual bool OnCanExecuteCommand(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Notifies the command that it should cancel
        /// </summary>
        public void Cancel()
        {
            _executeCTS?.Cancel();
        }
        /// <summary>
        /// Starts the process of executing the command.  Call order is: onExecuteStarting, onExecute, onExecuteComplate, onComplete
        /// </summary>
        /// <param name="onComplete">An action to execute when the command has completed.</param>
        /// <returns>The Task related to this command</returns>
        public async Task Execute(Action<bool> onComplete = null)
        {
            _executeCTS = new CancellationTokenSource();
            await ExecuteAsync(
                cts =>
                {
                    //_parent.Dispatcher.InvokeEx(() => { _parent.IsBusy.Value = true; });
                    _onExecuteStarting(cts);
                },
                cts => { _onExecute(cts); },
                cts =>
                {
                    //_parent.Dispatcher.InvokeEx(() => { _parent.IsBusy.Value = false; });
                    _onExecuteComplete(cts);
                },
                onComplete, _executeCTS);
        }
        //TODO: 6.3.2017 - not sure CanExecute will work right (should the owner be the page or model instead?)
        /// <summary>
        /// If True, the command can be executed
        /// </summary>
        public Property<Commander, bool> CanExecute { get; } = new Property<Commander, bool>(false);
        /// <summary>
        /// A handle to this Commander as an ICommand
        /// </summary>
        public ICommand Command
        {
            get
            {
                if (_command == null)
                {
                    _command = new RelayCommand(OnCommand, OnCanExecuteCommand);
                }
                return _command;
            }
        }
        /// <summary>
        /// Constructs a new Commander object
        /// </summary>
        /// <param name="onExecuteStarting">Code to execute before the command executes</param>
        /// <param name="onExecute">Code to execute for this command</param>
        /// <param name="onExecuteComplete">Code to execute after the command executes </param>
        public Commander(Action<CancellationToken> onExecuteStarting, Action<CancellationToken> onExecute, Action<CancellationToken> onExecuteComplete)
        {
            this.InitializeProperties();
            _onExecuteStarting = onExecuteStarting;
            _onExecute = onExecute;
            _onExecuteComplete = onExecuteComplete;
        }
    }
}
