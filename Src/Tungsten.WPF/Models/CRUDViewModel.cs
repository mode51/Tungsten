using System.Threading;
using W.WPF.Commands;

namespace W.WPF.Models
{
    /// <summary>
    /// A ViewModel with some initial functionality
    /// </summary>
    public class CRUDViewModel : ViewModel
    {
        /// <summary>
        /// A Refresh Command
        /// </summary>
        public Commander RefreshCommander { get; private set; }
        /// <summary>
        /// A Save Command
        /// </summary>
        public Commander SaveCommander { get; private set; }
        /// <summary>
        /// An Add Command
        /// </summary>
        public Commander AddCommander { get; private set; }
        /// <summary>
        /// A Delete Command
        /// </summary>
        public Commander DeleteCommander { get; private set; }

        /// <summary>
        /// Called before OnRefresh
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnRefreshStarting(Commander commander, CancellationToken cancellationToken) { Busy.IsBusy.Value = true; RefreshCommander.CanExecute.Value = false; /*AsRefreshable.Value.CanRefresh.Value = false;*/ }
        /// <summary>
        /// Place code to refresh the data here
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnRefresh(Commander commander, CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after the data has been refreshed
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnRefreshComplete(Commander commander, CancellationToken cancellationToken) { Busy.IsBusy.Value = false; RefreshCommander.CanExecute.Value = true; /*AsRefreshable.Value.CanRefresh.Value = true;*/ }

        /// <summary>
        /// Called before OnSave
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnSaveStarting(Commander commander, CancellationToken cancellationToken) { Busy.IsBusy.Value = true; RefreshCommander.CanExecute.Value = false; /*AsRefreshable.Value.CanRefresh.Value = false;*/ }
        /// <summary>
        /// Place code to save data here
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnSave(Commander commander, CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after OnSave
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnSaveComplete(Commander commander, CancellationToken cancellationToken) { Busy.IsBusy.Value = false; RefreshCommander.CanExecute.Value = true; /*AsRefreshable.Value.CanRefresh.Value = true;*/ }

        /// <summary>
        /// Called before OnAdd
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnAddStarting(Commander commander, CancellationToken cancellationToken) { }
        /// <summary>
        /// Place code to add data here
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnAdd(Commander commander, CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after OnAdd
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnAddComplete(Commander commander, CancellationToken cancellationToken) { }

        /// <summary>
        /// Called before OnDelete
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnDeleteStarting(Commander commander, CancellationToken cancellationToken) { }
        /// <summary>
        /// Place code to delete data here
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnDelete(Commander commander, CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after OnDelete
        /// </summary>
        /// <param name="commander">The Commander which is being executed</param>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnDeleteComplete(Commander commander, CancellationToken cancellationToken) { }
        
        /// <summary>
        /// Called immediately by the constructor.  Initializes the CRUD commands.
        /// </summary>
        protected override void OnInitialize()
        {
            RefreshCommander = new Commander(OnRefreshStarting, OnRefresh, OnRefreshComplete);
            SaveCommander = new Commander(OnSaveStarting, OnSave, OnSaveComplete);
            AddCommander = new Commander(OnAddStarting, OnAdd, OnAddComplete);
            DeleteCommander = new Commander(OnDeleteStarting, OnDelete, OnDeleteComplete);
            base.OnInitialize();
        }
        /// <summary>
        /// Construct a new CRUDViewModel
        /// </summary>
        public CRUDViewModel(IPageHost host) : base(host) { }
    }
}
