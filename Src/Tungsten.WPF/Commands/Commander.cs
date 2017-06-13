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
    ///// <summary>
    ///// Usefully wraps the functionality of a WPF Command
    ///// </summary>
    //public class Commander<TOwner> : Commander, IOwnedProperty
    //{
    //    private TOwner _owner = default(TOwner);

    //    public void SetOwner(object owner)
    //    {
    //        _owner = (TOwner)owner;
    //    }
    //    /// <summary>
    //    /// Constructs a new Commander object
    //    /// </summary>
    //    /// <param name="onExecuteStarting">Code to execute before the command executes</param>
    //    /// <param name="onExecute">Code to execute for this command</param>
    //    /// <param name="onExecuteComplete">Code to execute after the command executes </param>
    //    public Commander(Action<TOwner, CancellationToken> onExecuteStarting, Action<TOwner, CancellationToken> onExecute, Action<TOwner, CancellationToken> onExecuteComplete) : base(onExecuteStarting, onExecute, onExecuteComplete)
    //    {
    //        this.InitializeProperties();
    //    }
    //    /// <summary>
    //    /// Constructs a new Commander object
    //    /// </summary>
    //    /// <param name="onExecute">Code to execute for this command</param>
    //    public Commander(Action<TOwner, CancellationToken> onExecute) : base(onExecute)
    //    {
    //        this.InitializeProperties();
    //    }
    //}


    /// <summary>
    /// Usefully wraps the functionality of a WPF Command
    /// </summary>
    public class Commander : Commander<Commander>
    {
        /// <summary>
        /// Constructs a new Commander object
        /// </summary>
        /// <param name="onExecute">Code to execute for this command</param>
        public Commander(Action<Commander, CancellationToken> onExecute) : this(null, onExecute, null)
        {
        }
        /// <summary>
        /// Constructs a new Commander object
        /// </summary>
        /// <param name="onExecuteStarting">Code to execute before the command executes</param>
        /// <param name="onExecute">Code to execute for this command</param>
        /// <param name="onExecuteComplete">Code to execute after the command executes </param>
        public Commander(Action<Commander, CancellationToken> onExecuteStarting, Action<Commander, CancellationToken> onExecute, Action<Commander, CancellationToken> onExecuteComplete) : base(onExecuteStarting, onExecute, onExecuteComplete)
        {
        }
    }
    /// <summary>
    /// Usefully wraps the functionality of a WPF Command
    /// </summary>
    public class Commander<TOwner> : IOwnedProperty //<TModel> where TModel : class, IBusy, new()
    {
        private TOwner _owner = default(TOwner);

        private CancellationTokenSource _executeCTS;
        private ICommand _command;
        private Action<TOwner, CancellationToken> _onExecuteStarting = null;
        private Action<TOwner, CancellationToken> _onExecute = null;
        private Action<TOwner, CancellationToken> _onExecuteComplete = null;

        private async Task ExecuteAsync(Action<TOwner, CancellationToken> onBeforeExecute, Action<TOwner, CancellationToken> onExecute, Action<TOwner, CancellationToken> onAfterExecute, Action<TOwner, bool> onComplete = null, CancellationTokenSource cts = null)
        {
            if (cts == null)
                cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            await Task.Run(() =>
            {
                bool success = false;
                try
                {
                    onBeforeExecute?.Invoke(_owner, cancellationToken);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                if (cancellationToken.IsCancellationRequested)
                    return;
                try
                {
                    onExecute?.Invoke(_owner, cancellationToken);
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
                    onAfterExecute?.Invoke(_owner, cancellationToken);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                if (cancellationToken.IsCancellationRequested)
                    return;
                try
                {
                    onComplete?.Invoke(_owner, success);
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
        /// Sets the Owner of this property (the default value is itself)
        /// </summary>
        /// <param name="owner">The new owning object</param>
        public void SetOwner(object owner)
        {
            _owner = (TOwner)owner;
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
        public async Task Execute(Action<TOwner, bool> onComplete = null)
        {
            _executeCTS = new CancellationTokenSource();
            await ExecuteAsync(
                (o, ct) =>
                {
                    //_parent.Dispatcher.InvokeEx(() => { _parent.IsBusy.Value = true; });
                    _onExecuteStarting(o, ct);
                },
                (o, ct) => { _onExecute(o, ct); },
                (o, ct) =>
                {
                    //_parent.Dispatcher.InvokeEx(() => { _parent.IsBusy.Value = false; });
                    _onExecuteComplete(o, ct);
                },
                onComplete, _executeCTS);
        }
        /// <summary>
        /// If True, the command can be executed
        /// </summary>
        public Property<bool> CanExecute { get; } = new Property<bool>(false);
        
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
        /// <param name="onExecute">Code to execute for this command</param>
        public Commander(Action<TOwner, CancellationToken> onExecute) : this(null, onExecute, null)
        {
        }
        /// <summary>
        /// Constructs a new Commander object
        /// </summary>
        /// <param name="onExecuteStarting">Code to execute before the command executes</param>
        /// <param name="onExecute">Code to execute for this command</param>
        /// <param name="onExecuteComplete">Code to execute after the command executes </param>
        public Commander(Action<TOwner, CancellationToken> onExecuteStarting, Action<TOwner, CancellationToken> onExecute, Action<TOwner, CancellationToken> onExecuteComplete)
        {
            this.InitializeProperties();
            _onExecuteStarting = onExecuteStarting;
            _onExecute = onExecute;
            _onExecuteComplete = onExecuteComplete;
        }
    }
}
