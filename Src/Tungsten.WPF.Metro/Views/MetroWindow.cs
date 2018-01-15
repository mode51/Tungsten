using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using mah = MahApps.Metro;
using W.AsExtensions;

namespace W.WPF.Views
{
    //These two windows should only differ by the dependency properties: PageFramework and ActivePage
    /// <summary>
    /// A WPF window based on MahApps.Metro.Controls.MetroWindow which supports PageFramework
    /// </summary>
    /// <typeparam name="TModel">The type of viewmodel to use. One will be created if one is not passed to the constructor.</typeparam>
    /// <remarks>This code behind is nearly duplicate of W.WPF.Views.Window</remarks>
    public class MetroWindow<TModel> : MetroWindowBase, IPageHost where TModel : class, new()
    {
        /// <summary>
        /// Gets a handle to the ViewModel
        /// </summary>
        public TModel ViewModel { get; set; }

        ///// <summary>
        ///// A local PageFramework which can be used to navigate pages in the current window
        ///// </summary>
        //public PageFramework PageFramework { get; private set; }

        //public static readonly DependencyProperty PageFrameworkProperty = DependencyProperty.Register("PageFramework", typeof(PageFramework), typeof(MetroWindow<TModel>));
        //public PageFramework PageFramework
        //{
        //    get { return (PageFramework)GetValue(PageFrameworkProperty); }
        //    set { SetValue(PageFrameworkProperty, value); }
        //}

        //#region IPageHost
        //public static readonly DependencyProperty ActivePageProperty = DependencyProperty.Register("ActivePage", typeof(PageWrapper), typeof(MetroWindow<TModel>), new PropertyMetadata(null, HandleActivePageChanged));
        //public PageWrapper ActivePage
        //{
        //    get { return (PageWrapper)GetValue(ActivePageProperty); }
        //    set { SetValue(ActivePageProperty, value); }
        //}
        //private static void HandleActivePageChanged(object sender, DependencyPropertyChangedEventArgs args)
        //{
        //    var ctl = sender as MetroWindowBase;// W.WPF.Models.WindowModel<TModel>;
        //    if (ctl != null)
        //    {
        //        var oldValue = args.OldValue.As<PageWrapper>();
        //        var newValue = args.NewValue.As<PageWrapper>();
        //        //var vm = ctl.ViewModel.As<W.WPF.Models.WindowModel<TModel>>();
        //        //vm?.OnActivePageChanged(vm, oldValue, newValue);
        //        //ctl.OnActivePageChanged(oldValue, newValue);
        //    }
        //}
        //// IPageHost.Dispatcher - MetroWindow already exposes a Dispatcher
        //#endregion

        /// <summary>
        /// Create a new MetroWindow
        /// </summary>
        /// <remarks>A default constructor is necessary for generics in XAML</remarks>
        public MetroWindow() : this(default(TModel)) { }
        /// <summary>
        /// Create a new MetroWindow
        /// </summary>
        /// <param name="viewModel">An existing viewmodel to use</param>
        public MetroWindow(TModel viewModel) : base()
        {
            //PageFramework = new PageFramework(this);
            ViewModel = viewModel ?? new TModel();
            ViewModel.As<W.WPF.Models.ViewModel>()?.SetPageHost(this);
            DataContext = ViewModel;
        }
    }
    /// <summary>
    /// A WPF window based on MahApps.Metro.Controls.MetroWindow which supports PageFramework
    /// </summary>
    /// <remarks>This code behind is nearly duplicate of W.WPF.Views.Window</remarks>
    public class MetroWindow : MetroWindowBase, IPageHost
    {
        /// <summary>
        /// Gets a handle to the ViewModel
        /// </summary>
        public object ViewModel { get; set; }

        ///// <summary>
        ///// A local PageFramework which can be used to navigate pages in the current window
        ///// </summary>
        //public PageFramework PageFramework { get; private set; }

        //public static readonly DependencyProperty PageFrameworkProperty = DependencyProperty.Register("PageFrmework", typeof(PageFramework), typeof(MetroWindowBase));
        //public PageFramework PageFramework
        //{
        //    get { return (PageFramework)GetValue(PageFrameworkProperty); }
        //    set { SetValue(PageFrameworkProperty, value); }
        //}

        //#region IPageHost
        //public static readonly DependencyProperty ActivePageProperty = DependencyProperty.Register("ActivePage", typeof(PageWrapper), typeof(MetroWindowBase), new PropertyMetadata(null, HandleActivePageChanged));
        //public PageWrapper ActivePage
        //{
        //    get { return (PageWrapper)GetValue(ActivePageProperty); }
        //    set { SetValue(ActivePageProperty, value); }
        //}
        //private static void HandleActivePageChanged(object sender, DependencyPropertyChangedEventArgs args)
        //{
        //    var ctl = sender as MetroWindowBase;// W.WPF.Models.WindowModel<TModel>;
        //    if (ctl != null)
        //    {
        //        var oldValue = args.OldValue.As<PageWrapper>();
        //        var newValue = args.NewValue.As<PageWrapper>();
        //        //var vm = ctl.ViewModel.As<W.WPF.Models.WindowModel<TModel>>();
        //        //vm?.OnActivePageChanged(vm, oldValue, newValue);
        //        //ctl.OnActivePageChanged(oldValue, newValue);
        //    }
        //}
        //// IPageHost.Dispatcher - MetroWindow already exposes a Dispatcher
        //#endregion

        /// <summary>
        /// Create a new MetroWindow
        /// </summary>
        /// <remarks>A default constructor is necessary for XAML</remarks>
        public MetroWindow() : this(null) { }
        /// <summary>
        /// Create a new MetroWindow
        /// </summary>
        /// <param name="viewModel">The viewmodel to use</param>
        public MetroWindow(object viewModel) : base()
        {
            //PageFramework = new PageFramework(this);
            ViewModel = viewModel;
            ViewModel.As<W.WPF.Models.ViewModel>()?.SetPageHost(this);
            DataContext = ViewModel;
        }
    }

    /// <summary>
    /// A WPF window based on MahApps.Metro.Controls.MetroWindow
    /// </summary>
    /// <remarks>This used to have more shared code</remarks>
    public abstract class MetroWindowBase : mah.Controls.MetroWindow
    {
        /// <summary>
        /// Called by the constructor
        /// </summary>
        /// <param name="sender">The metro window</param>
        /// <param name="args"></param>
        protected virtual void OnLoaded(object sender, RoutedEventArgs args) { }

        /// <summary>
        /// Gets a handle to the TModel singleton, creating it if necessary
        /// </summary>
        /// <returns>A handle to the model Instance</returns>
        /// <remarks>If the model does not have a static property called Instance, this method will return null</remarks>
        //protected static TModel GetViewModelInstance<TModel>()
        //{
        //    //try
        //    //{
        //    var p = typeof(TModel).GetProperty("Instance", BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
        //    if (p != null)
        //        return (TModel)p.GetGetMethod().Invoke(null, null);
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    System.Diagnostics.Debug.WriteLine(e.ToString());
        //    //}
        //    return default(TModel);
        //}
        //protected virtual void OnActivePageChanged(PageWrapper oldValue, PageWrapper newValue)
        //{
        //    ViewModel.OnActivePageChanged(oldValue, newValue);
        //}

        /// <summary>
        /// Initializes the PageFramework before anything else
        /// </summary>
        static MetroWindowBase() { }
        /// <summary>
        /// Create a new MahApps window
        /// </summary>
        public MetroWindowBase()
        {
            Loaded += OnLoaded;
        }
    }

}
