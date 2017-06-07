using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using W.Logging;
using W.WPF.Commands;
using W.WPF.Core;

namespace W.WPF.Models
{
    public class WindowModel : ModelBase//<TModel> : ModelBase where TModel : new()
    {
        //#region IPageHost
        //public static readonly DependencyProperty ActivePageProperty = DependencyProperty.Register("ActivePage", typeof(PageWrapper), typeof(WindowModel<TModel>), new PropertyMetadata(null, HandleActivePageChanged));
        //public PageWrapper ActivePage
        //{
        //    get { return (PageWrapper)GetValue(ActivePageProperty); }
        //    set { Dispatcher.InvokeEx(() => SetValue(ActivePageProperty, value)); }
        //}
        //private static void HandleActivePageChanged(object sender, DependencyPropertyChangedEventArgs args)
        //{
        //    var ctl = sender as WindowModel<TModel>;
        //    if (ctl != null)
        //    {
        //        var oldValue = args.OldValue.As<PageWrapper>();
        //        var newValue = args.NewValue.As<PageWrapper>();

        //        ctl.Dispatcher.InvokeEx(() => ctl.OnActivePageChanged(ctl, oldValue, newValue));
        //    }
        //}
        //#endregion

        //public virtual void OnActivePageChanged(/*WindowModel<TModel> ctl, */PageWrapper oldValue, PageWrapper newValue) // FrameworkElement oldValue, FrameworkElement newValue)
        //{
        //    Log.i(this.GetType().Name + ":OnActivePageChanged");
        //}
        public WindowModel() : base()
        {
        }
    }
}
