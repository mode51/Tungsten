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
    public class ModelBase : DisposableDependencyObject, IBusy /*, IModel , ISaveCommander, IRefreshCommander*/
    {
        //protected double DelayStartWaitTime { get; set; } = 500;

        //public Property<FrameworkElement> UIHandle { get; } = new Property<FrameworkElement>();
        //public new System.Windows.Threading.Dispatcher Dispatcher => (Owner.Value?.Dispatcher ?? base.Dispatcher);

        #region IBusy
        public Property<ModelBase, bool> IsBusy { get; } = new Property<ModelBase, bool>((m, oldValue, newValue) => { m.BusyVisibility.Value = newValue ? Visibility.Visible : Visibility.Hidden; });
        public Property<string> BusyTitle { get; } = new Property<string>("Loading");
        public Property<string> BusyMessage { get; } = new Property<string>("");
        public Property<System.Windows.Visibility> BusyVisibility { get; } = new Property<System.Windows.Visibility>(Visibility.Hidden);
        #endregion

        public Property<string> Title { get; } = new Property<string>("");


        public Commander RefreshCommander { get; private set; }
        public Commander SaveCommander { get; private set; }
        public Commander AddCommander { get; private set; }
        public Commander DeleteCommander { get; private set; }

        protected virtual void OnRefreshStarting(CancellationToken cancellationToken) { IsBusy.Value = true; RefreshCommander.CanExecute.Value = false; /*AsRefreshable.Value.CanRefresh.Value = false;*/ }
        protected virtual void OnRefresh(CancellationToken cancellationToken) { }
        protected virtual void OnRefreshComplete(CancellationToken cancellationToken) { IsBusy.Value = false; RefreshCommander.CanExecute.Value = true; /*AsRefreshable.Value.CanRefresh.Value = true;*/ }

        protected virtual void OnSaveStarting(CancellationToken cancellationToken) { IsBusy.Value = true; RefreshCommander.CanExecute.Value = false; /*AsRefreshable.Value.CanRefresh.Value = false;*/ }
        protected virtual void OnSave(CancellationToken cancellationToken) { }
        protected virtual void OnSaveComplete(CancellationToken cancellationToken) { IsBusy.Value = false; RefreshCommander.CanExecute.Value = true; /*AsRefreshable.Value.CanRefresh.Value = true;*/ }

        protected virtual void OnAddStarting(CancellationToken cancellationToken) { }
        protected virtual void OnAdd(CancellationToken cancellationToken) { }
        protected virtual void OnAddComplete(CancellationToken cancellationToken) { }

        protected virtual void OnDeleteStarting(CancellationToken cancellationToken) { }
        protected virtual void OnDelete(CancellationToken cancellationToken) { }
        protected virtual void OnDeleteComplete(CancellationToken cancellationToken) { }

        //protected virtual void OnDelayedStart() { }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            RefreshCommander = new Commander(OnRefreshStarting, OnRefresh, OnRefreshComplete);
            SaveCommander = new Commander(OnSaveStarting, OnSave, OnSaveComplete);
            AddCommander = new Commander(OnAddStarting, OnAdd, OnAddComplete);
            DeleteCommander = new Commander(OnDeleteStarting, OnDelete, OnDeleteComplete);
        }

        protected override void OnCreateInDesignMode()
        {
            base.OnCreateInDesignMode();
        }
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        public ModelBase() : base()
        {
            Log.i("{0}.InitializeProperties()", this.GetType().Name);
            this.InitializeProperties();
        }
        //public ModelBase(TModel owner) : this()
        //{
        //    UIHandle = owner;
        //    //OnCreate is called before this code executes
        //}
    }
}
