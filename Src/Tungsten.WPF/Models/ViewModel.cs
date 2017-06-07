using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using W;
using W.Logging;
using W.WPF.Core;
using W.WPF.Commands;

namespace W.WPF.Models
{
    /// <summary>
    /// The base class for W.WPF PageFramework view models
    /// </summary>
    public class ViewModel : DisposableDependencyObject /*, IModel , ISaveCommander, IRefreshCommander*/
    {
        //private System.Threading.ManualResetEventSlim _delayStart = new ManualResetEventSlim(false);

        /// <summary>
        /// Information related to this model being busy
        /// </summary>
        public BusyIndication BusyIndication { get; } = new BusyIndication();
        //#region IBusy
        ///// <summary>
        ///// Get or set the IsBusy flag
        ///// </summary>
        //public Property<ModelBase, bool> IsBusy { get; } = new Property<ModelBase, bool>((m, oldValue, newValue) => { m.BusyVisibility.Value = newValue ? Visibility.Visible : Visibility.Hidden; });
        ///// <summary>
        ///// Get or set a title string which can be displayed while busy
        ///// </summary>
        //public Property<string> BusyTitle { get; } = new Property<string>("Loading");
        ///// <summary>
        ///// Get or set a message to display while busy
        ///// </summary>
        //public Property<string> BusyMessage { get; } = new Property<string>("");
        ///// <summary>
        ///// Visible if IsBusy.Value is True, otherwise Hidden
        ///// </summary>
        //public Property<System.Windows.Visibility> BusyVisibility { get; } = new Property<System.Windows.Visibility>(Visibility.Hidden);
        //#endregion

        /// <summary>
        /// Everything needs a title
        /// </summary>
        public Property<string> Title { get; } = new Property<string>("");

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

        //protected virtual void OnDelayedStart()
        //{

        //}

        /// <summary>
        /// Called immediately upon object creation.  Place initialization code here.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            RefreshCommander = new Commander(OnRefreshStarting, OnRefresh, OnRefreshComplete);
            SaveCommander = new Commander(OnSaveStarting, OnSave, OnSaveComplete);
            AddCommander = new Commander(OnAddStarting, OnAdd, OnAddComplete);
            DeleteCommander = new Commander(OnDeleteStarting, OnDelete, OnDeleteComplete);
        }
        /// <summary>
        /// Called instead of OnCreate if the code is running in DesignMode
        /// </summary>
        protected override void OnCreateInDesignMode()
        {
            base.OnCreateInDesignMode();
        }
        /// <summary>
        /// Called immediately after OnInitialize (when no in DesignMode)
        /// </summary>
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        /// <summary>
        /// Constructs a new ViewModel
        /// </summary>
        public ViewModel() : base()
        {
            Log.i("{0}.InitializeProperties()", this.GetType().Name);
            this.InitializeProperties();
            //Task.Run(() => 
            //{
            //    _delayStart.Wait();
            //    _delayStart.Dispose();
            //    OnDelayedStart();
            //});
            //_delayStart.Set();
        }
        //public ModelBase(TModel owner) : this()
        //{
        //    UIHandle = owner;
        //    //OnCreate is called before this code executes
        //}
    }
}
