using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using W.Logging;
using W.WPF;
using W.WPF.Commands;
using W.WPF.Core;

namespace W.WPF
{
    /// <summary>
    /// A framework for using Pages in WPF
    /// </summary>
    public class PageFramework : DependencyObject // DependencyObject for ActivePage property (so the UI knows to update page changes)
    {
        #region Static Members
        private static void DiscoverPages<IPageType>(Assembly asm = null)
        {
            try
            {
                if (asm == null)
                    asm = Assembly.GetEntryAssembly();
                foreach (var t in asm.GetTypes())
                {
                    if (!t.IsAbstract)
                    {
                        var iface = t.GetInterfaces().FirstOrDefault(i => i == typeof(IPageType));// i.FullName == "W.WPF.Framework.IPage");
                        if (iface != null)
                        {
                            //var page = Activator.CreateInstance(t) as IPage;
                            var pw = new PageWrapper()
                            {
                                //Name = t.Name,
                                Type = t,
                                //PageXaml = t.FullName,
                                //Page = new Lazy<IPage>(() => Activator.CreateInstance(t) as IPage)
                                FrameworkElement = (FrameworkElement)Activator.CreateInstance(t)
                            };
                            AllPages.Add(pw);
                        }
                    }
                }
                foreach (AssemblyName assemblyName in asm.GetReferencedAssemblies())
                {
                    try
                    {
                        if (assemblyName.CodeBase?.Trim('/', '\\') == AppDomain.CurrentDomain.BaseDirectory.Trim('/', '\\'))
                        //if (!assemblyName.Name.StartsWith("System") && !(assemblyName.Name.StartsWith("mscorlib")))
                        {
                            Log.i("Loading {0}", assemblyName);
                            var assembly = Assembly.Load(assemblyName.Name);
                            if (assembly != null)
                                DiscoverPages<IPageType>(assembly);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.e(e);
                    }
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }
        /// <summary>
        /// Returns a list of all the available Pages that were found.
        /// </summary>
        public static List<PageWrapper> AllPages = new List<PageWrapper>();
        #endregion

        private Stack<PageWrapper> _history = new Stack<PageWrapper>();
        public IPageHost Host { get; private set; }

        private Stack<T> TrimHistory<T>(Stack<T> @this, int maxSize)
        {
            if (@this.Count <= maxSize || @this.Count < 2)
                return @this;
            var result = new Stack<T>();
            var items = @this.ToArray();
            @this.Clear();
            result.Push(items[items.Length - 1]); //re-add the last page, which is the Home page
            maxSize -= 1; //because we just re-added the Home page
            int endIndex = items.Length - 2;//skip the last one because we've already added it
            int startIndex = 1 + (endIndex - maxSize);
            for (var t = endIndex; t >= startIndex; t--)
            {
                result.Push(items[t]);
            }
            return result;
        }
        private PageWrapper NavigateTo(PageWrapper page, int maxHistory, bool push = true, params object[] args)
        {
            try
            {
                if (ActivePage == page)
                    return ActivePage;
                Host.Dispatcher.InvokeEx(() =>
                {
                    if (Host != null)
                    {
                        var previousPage = ActivePage?.AsPage;
                        previousPage?.OnNavigateFrom(page?.AsPage);
                        _history = TrimHistory(_history, maxHistory); //just picked a number to limit the size of the history
                        if (push)
                            _history.Push(page);
                        ActivePage = page;
                        page?.AsPage?.OnNavigateTo(previousPage, args);
                        CanNavigateBack.Value = _history.Count > 1;
                        //ActivePage = page;
                    }
                });
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return page;
        }

        public static readonly DependencyProperty ActivePageProperty = DependencyProperty.Register("ActivePage", typeof(PageWrapper), typeof(PageFramework));
        public PageWrapper ActivePage
        {
            get { return (PageWrapper)GetValue(ActivePageProperty); }
            set { SetValue(ActivePageProperty, value); }
        }
        /// <summary>
        /// Can be used to detect the currently active page
        /// </summary>
        //public Property<PageWrapper> ActivePage { get; }/* private set; }*/ = new Property<PageWrapper>();

        /// <summary>
        /// Navigate to the page of the given Type
        /// </summary>
        /// <param name="type">The type of page to navigate to</param>
        /// <param name="args">Any arguments to pass into the page upon navigation</param>
        /// <returns></returns>
        public PageWrapper NavigateTo(Type type, params object[] args)
        {
            var page = PageFramework.AllPages.FirstOrDefault(p => p.Type == type);
            if (page != null)
                return NavigateTo(page, MaximumNumberOfPreviousPages.Value, true, args);
            return null;
        }
        /// <summary>
        /// Navigate to the page of the given Type name
        /// </summary>
        /// <param name="typeName">The name of the page type</param>
        /// <param name="args">Any arguments to pass into the page upon navigation</param>
        /// <returns></returns>
        public PageWrapper NavigateTo(string typeName, params object[] args)
        {
            var page = PageFramework.AllPages.FirstOrDefault(p => p.Type.Name == typeName);
            if (page != null)
                return NavigateTo(page, MaximumNumberOfPreviousPages.Value, true, args);
            return null;
        }
        /// <summary>
        /// Navigate to the previous page
        /// </summary>
        /// <returns></returns>
        public PageWrapper NavigateBack()
        {
            if (_history.Count < 2)
                return null;
            _history.Pop();//remove current page
            var page = _history.Peek();
            return NavigateTo(page, MaximumNumberOfPreviousPages.Value, false);
        }
        /// <summary>
        /// True if there is at least one previous page in the history
        /// </summary>
        public Property<bool> CanNavigateBack { get; } = new Property<bool>(false);
        /// <summary>
        /// Always True
        /// </summary>
        public Property<bool> CanNavigateHome { get; } = new Property<bool>(true);

        /// <summary>
        /// The number of previous pages to remember
        /// </summary>
        public Property<int> MaximumNumberOfPreviousPages { get; } = new Property<int>(99);

        #region Commands
        /// <summary>
        /// Provides WPF Commands for navigation
        /// </summary>
        public Commands.NavigationCommands Navigation { get; private set; }
        #endregion

        /// <summary>
        /// Sets the Page Host
        /// </summary>
        /// <param name="host"></param>
        public void SetPageHost(IPageHost host)
        {
            Host = host;
        }
        /// <summary>
        /// Destructs the PageFramework instance and frees resources
        /// </summary>
        ~PageFramework()
        {
            _history.Clear();
            _history = null;
            Host = null;
            CanNavigateBack.Dispose();
            CanNavigateHome.Dispose();
        }

        static PageFramework()
        {
            DiscoverPages<IPage>();
        }
        /// <summary>
        /// Constructs a new LocalPageFramework
        /// </summary>
        public PageFramework(IPageHost host)
        {
            SetPageHost(host);
            Navigation = new NavigationCommands(this);
        }
    }

    ///// <summary>
    ///// A framework for using Pages in WPF
    ///// </summary>
    //public class PageFramework
    //{
    //    private static Stack<PageWrapper> _history = new Stack<PageWrapper>();
    //    private static PageWrapper _previousPage = new PageWrapper();
    //    /// <summary>
    //    /// Returns a list of all the available Pages that were found.
    //    /// </summary>
    //    public static List<PageWrapper> AllPages = new List<PageWrapper>();

    //    private static Stack<T> TrimHistory<T>(Stack<T> @this, uint maxSize)
    //    {
    //        if (@this.Count <= maxSize || @this.Count < 2)
    //            return @this;
    //        var result = new Stack<T>();
    //        var items = @this.ToArray();
    //        @this.Clear();
    //        result.Push(items[items.Length - 1]); //re-add the last page, which is the Home page
    //        maxSize -= 1; //because we just re-added the Home page
    //        int endIndex = items.Length - 2;//skip the last one because we've already added it
    //        int startIndex = 1 + (endIndex - (int)maxSize);
    //        for (var t = endIndex; t >= startIndex; t--)
    //        {
    //            result.Push(items[t]);
    //        }
    //        return result;
    //    }
    //    private static void DiscoverPages<IPageType>(Assembly asm = null)
    //    {
    //        try
    //        {
    //            if (asm == null)
    //                asm = Assembly.GetEntryAssembly();
    //            foreach (var t in asm.GetTypes())
    //            {
    //                if (!t.IsAbstract)
    //                {
    //                    var iface = t.GetInterfaces().FirstOrDefault(i => i == typeof(IPageType));// i.FullName == "W.WPF.Framework.IPage");
    //                    if (iface != null)
    //                    {
    //                        //var page = Activator.CreateInstance(t) as IPage;
    //                        var pw = new PageWrapper()
    //                        {
    //                            //Name = t.Name,
    //                            Type = t,
    //                            //PageXaml = t.FullName,
    //                            //Page = new Lazy<IPage>(() => Activator.CreateInstance(t) as IPage)
    //                            FrameworkElement = (FrameworkElement)Activator.CreateInstance(t)
    //                        };
    //                        AllPages.Add(pw);
    //                    }
    //                }
    //            }
    //            foreach (AssemblyName assemblyName in asm.GetReferencedAssemblies())
    //            {
    //                try
    //                {
    //                    if (assemblyName.CodeBase?.Trim('/', '\\') == AppDomain.CurrentDomain.BaseDirectory.Trim('/', '\\'))
    //                    //if (!assemblyName.Name.StartsWith("System") && !(assemblyName.Name.StartsWith("mscorlib")))
    //                    {
    //                        Log.i("Loading {0}", assemblyName);
    //                        var assembly = Assembly.Load(assemblyName.Name);
    //                        if (assembly != null)
    //                            DiscoverPages<IPageType>(assembly);
    //                    }
    //                }
    //                catch (Exception e)
    //                {
    //                    Log.e(e);
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Log.e(e);
    //        }
    //    }
    //    private static PageWrapper NavigateTo(PageWrapper page, bool push = true, params object[] args)
    //    {
    //        try
    //        {
    //            HostDispatcher.InvokeEx(() =>
    //            {
    //                if (Host != null)
    //                {
    //                    _history = TrimHistory(_history, 5); //just picked a number to limit the size of the history
    //                    if (push)
    //                        _history.Push(page);
    //                    Host.ActivePage = page;
    //                    page.AsPage?.OnNavigateTo(args);
    //                    CanNavigateBack.Value = _history.Count > 1;
    //                }
    //            });
    //        }
    //        catch (Exception e)
    //        {
    //            Log.e(e);
    //        }
    //        return page;
    //    }

    //    public static IPageHost Host => Application.Current.MainWindow?.DataContext as IPageHost;
    //    public static Dispatcher HostDispatcher => Application.Current.MainWindow?.Dispatcher ?? System.Windows.Threading.Dispatcher.CurrentDispatcher;

    //    public static PageWrapper NavigateTo(Type type, params object[] args)
    //    {
    //        var page = AllPages.FirstOrDefault(p => p.Type == type);
    //        if (page != null)
    //            return NavigateTo(page, true, args);
    //        return null;
    //    }
    //    public static PageWrapper NavigateTo(string name, params object[] args)
    //    {
    //        var page = AllPages.FirstOrDefault(p => p.Type.Name == name);
    //        if (page != null)
    //            return NavigateTo(page, true, args);
    //        return null;
    //    }
    //    public static PageWrapper NavigateBack()
    //    {
    //        if (_history.Count < 2)
    //            return null;
    //        _history.Pop();//remove current page
    //        var page = _history.Peek();
    //        return NavigateTo(page, false);
    //    }

    //    public static Property<bool> CanNavigateBack { get; } = new Property<bool>(false);
    //    public static Property<bool> CanNavigateHome { get; } = new Property<bool>(false);

    //    public static void Initialize()
    //    {
    //        Log.i("PageFramework Initialized");
    //    }

    //    /// <summary>
    //    /// Uses reflection to locate all instances of classes supporting IPage
    //    /// </summary>
    //    static PageFramework()
    //    {
    //        DiscoverPages<IPage>();
    //    }
    //    /// <summary>
    //    /// Constructs a new PageFramework
    //    /// </summary>
    //    public PageFramework()
    //    {
    //    }
    //}

}
