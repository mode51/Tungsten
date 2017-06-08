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
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnRefreshStarting(CancellationToken cancellationToken) { BusyIndication.IsBusy.Value = true; RefreshCommander.CanExecute.Value = false; /*AsRefreshable.Value.CanRefresh.Value = false;*/ }
        /// <summary>
        /// Place code to refresh the data here
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnRefresh(CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after the data has been refreshed
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnRefreshComplete(CancellationToken cancellationToken) { BusyIndication.IsBusy.Value = false; RefreshCommander.CanExecute.Value = true; /*AsRefreshable.Value.CanRefresh.Value = true;*/ }

        /// <summary>
        /// Called before OnSave
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnSaveStarting(CancellationToken cancellationToken) { BusyIndication.IsBusy.Value = true; RefreshCommander.CanExecute.Value = false; /*AsRefreshable.Value.CanRefresh.Value = false;*/ }
        /// <summary>
        /// Place code to save data here
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnSave(CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after OnSave
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnSaveComplete(CancellationToken cancellationToken) { BusyIndication.IsBusy.Value = false; RefreshCommander.CanExecute.Value = true; /*AsRefreshable.Value.CanRefresh.Value = true;*/ }

        /// <summary>
        /// Called before OnAdd
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnAddStarting(CancellationToken cancellationToken) { }
        /// <summary>
        /// Place code to add data here
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnAdd(CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after OnAdd
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnAddComplete(CancellationToken cancellationToken) { }

        /// <summary>
        /// Called before OnDelete
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnDeleteStarting(CancellationToken cancellationToken) { }
        /// <summary>
        /// Place code to delete data here
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnDelete(CancellationToken cancellationToken) { }
        /// <summary>
        /// Called after OnDelete
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken which can be used to monitor cancellation</param>
        protected virtual void OnDeleteComplete(CancellationToken cancellationToken) { }
        
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
        public CRUDViewModel() : base() { }
    }
}
