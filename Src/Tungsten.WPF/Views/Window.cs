using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using W.Logging;

namespace W.WPF.Views
{
    /// <summary>
    /// A WPF window which supports PageFramework
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class Window<TModel> : WindowBase, IPageHost where TModel : W.WPF.Models.WindowModel, new()
    {
        /// <summary>
        /// Gets a handle to the View Model
        /// </summary>
        public TModel ViewModel { get; private set; }

        public static readonly DependencyProperty PageFrameworkProperty = DependencyProperty.Register("PageFrmework", typeof(PageFramework), typeof(Window<TModel>));
        public PageFramework PageFramework
        {
            get { return (PageFramework)GetValue(PageFrameworkProperty); }
            set { SetValue(PageFrameworkProperty, value); }
        }

        #region IPageHost
        public static readonly DependencyProperty ActivePageProperty = DependencyProperty.Register("ActivePage", typeof(PageWrapper), typeof(Window<TModel>), new PropertyMetadata(null, HandleActivePageChanged));
        public PageWrapper ActivePage
        {
            get { return (PageWrapper)GetValue(ActivePageProperty); }
            set { SetValue(ActivePageProperty, value); }
        }
        private static void HandleActivePageChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var ctl = sender as Window;// W.WPF.Models.WindowModel<TModel>;
            if (ctl != null)
            {
                var oldValue = args.OldValue.As<PageWrapper>();
                var newValue = args.NewValue.As<PageWrapper>();
                //var vm = ctl.ViewModel.As<W.WPF.Models.WindowModel<TModel>>();
                //vm?.OnActivePageChanged(vm, oldValue, newValue);
                //ctl.OnActivePageChanged(oldValue, newValue);
            }
        }
        // IPageHost.Dispatcher - MetroWindow already exposes a Dispatcher
        #endregion

        /// <summary>
        /// Constructs a new WPF Window which supports PageFramework
        /// </summary>
        /// <remarks>A default constructor is necessary for generics in XAML</remarks>
        public Window() : this(default(TModel)) { }
        /// <summary>
        /// Constructs a new WPF Window which supports PageFramework
        /// </summary>
        /// <param name="viewModel">An existing viewmodel to use</param>
        public Window(TModel viewModel) : base()
        {
            PageFramework = new PageFramework(this);
            ViewModel = viewModel ?? new TModel();
            DataContext = ViewModel;
        }
    }

    /// <summary>
    /// A WPF window which supports PageFramework
    /// </summary>
    public class Window : WindowBase, IPageHost
    {
        /// <summary>
        /// Gets a handle to the View Model
        /// </summary>
        public object ViewModel { get; set; }

        public static readonly DependencyProperty PageFrameworkProperty = DependencyProperty.Register("PageFrmework", typeof(PageFramework), typeof(Window));
        public PageFramework PageFramework
        {
            get { return (PageFramework)GetValue(PageFrameworkProperty); }
            set { SetValue(PageFrameworkProperty, value); }
        }

        #region IPageHost
        public static readonly DependencyProperty ActivePageProperty = DependencyProperty.Register("ActivePage", typeof(PageWrapper), typeof(Window), new PropertyMetadata(null, HandleActivePageChanged));
        public PageWrapper ActivePage
        {
            get { return (PageWrapper)GetValue(ActivePageProperty); }
            set { SetValue(ActivePageProperty, value); }
        }
        private static void HandleActivePageChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var ctl = sender as Window;// W.WPF.Models.WindowModel<TModel>;
            if (ctl != null)
            {
                var oldValue = args.OldValue.As<PageWrapper>();
                var newValue = args.NewValue.As<PageWrapper>();
                //var vm = ctl.ViewModel.As<W.WPF.Models.WindowModel<TModel>>();
                //vm?.OnActivePageChanged(vm, oldValue, newValue);
                //ctl.OnActivePageChanged(oldValue, newValue);
            }
        }
        // IPageHost.Dispatcher - MetroWindow already exposes a Dispatcher
        #endregion

        private void HandleLoaded(object sender, RoutedEventArgs args)
        {
            //IsBusy = false;
            OnLoaded(sender, args);
        }
        //protected static TModel GetViewModelInstance<TModel>()
        //{
        //    try
        //    {
        //        var p = typeof(TModel).GetProperty("Instance", BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
        //        if (p != null)
        //            return (TModel)p.GetGetMethod().Invoke(null, null);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.e(e);
        //    }
        //    return default(TModel);
        //}
        //protected virtual void OnActivePageChanged(PageWrapper oldValue, PageWrapper newValue)
        //{
        //    ViewModel.OnActivePageChanged(oldValue, newValue);
        //}

        /// <summary>
        /// Constructs a new WPF Window which supports PageFramework
        /// </summary>
        /// <remarks>A default constructor is necessary for XAML</remarks>
        public Window() : this(null) { }
        /// <summary>
        /// Constructs a new WPF Window which supports PageFramework
        /// </summary>
        /// <param name="viewModel">An existing viewmodel to use</param>
        public Window(object viewModel)
        {
            PageFramework = new PageFramework(this);
            ViewModel = viewModel ?? this;
            DataContext = ViewModel;
        }
    }
    /// <summary>
    /// A WPF window
    /// </summary>
    /// <remarks>This used to contain more shared code</remarks>
    public abstract class WindowBase : System.Windows.Window
    {
        protected virtual void OnLoaded(object sender, RoutedEventArgs args) { }

        //protected static TModel GetViewModelInstance<TModel>()
        //{
        //    try
        //    {
        //        var p = typeof(TModel).GetProperty("Instance", BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
        //        if (p != null)
        //            return (TModel)p.GetGetMethod().Invoke(null, null);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.e(e);
        //    }
        //    return default(TModel);
        //}
        //protected virtual void OnActivePageChanged(PageWrapper oldValue, PageWrapper newValue)
        //{
        //    ViewModel.OnActivePageChanged(oldValue, newValue);
        //}

        static WindowBase() { }
        public WindowBase()
        {
            Loaded += OnLoaded;
        }
    }
}
