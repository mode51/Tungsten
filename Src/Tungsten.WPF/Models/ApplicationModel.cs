using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using W.Logging;
using W.WPF.Core;

namespace W.WPF.Models
{
    /// <summary>
    /// Base class for a WPF application model
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ApplicationModel<TModel> : ViewModel where TModel : ApplicationModel<TModel>, new()
    {
        private static volatile TModel _instance;
        private static object _lock = new object();

        /// <summary>
        /// Gets the Instance singleton
        /// </summary>
        public static TModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TModel();
                        }
                    }
                }
                return _instance;
            }
        }

        ///// <summary>
        ///// Get or set the application Title
        ///// </summary>
        //public Property<ApplicationModel<TModel>, string> ApplicationTitle { get; } = new Property<ApplicationModel<TModel>, string>("");
        /// <summary>
        /// Gets whether the application is running in debug mode
        /// </summary>
        public Property<ApplicationModel<TModel>, bool> IsDebugMode { get; } = new Property<ApplicationModel<TModel>, bool>(false);
        /// <summary>
        /// Gets whether the application is running by an administrator
        /// </summary>
        public Property<ApplicationModel<TModel>, bool> IsAdministrator { get; } = new Property<ApplicationModel<TModel>, bool>(false);
        ///// <summary>
        ///// Get or set the MainFrame which is used for displaying pages
        ///// </summary>
        //public Property<ApplicationModel<TModel>, Frame> MainFrame { get; } = new Property<ApplicationModel<TModel>, Frame>((owner, oldValue, newValue) =>
        //{
        //    newValue.Navigating += (sender, args) => { Log.i("MainFrame.Navigating"); };
        //    newValue.Navigated += (sender, args) => { Log.i("MainFrame.Navigated"); /*owner.CurrentPage.Value = (System.Windows.Controls.Page)args.Content;*/ };
        //    newValue.ContentRendered += (sender, args) => { Log.i("MainFrame.ContentRendered"); };
        //    newValue.FragmentNavigation += (sender, args) => { Log.i("MainFrame.FragmentNavigation"); };
        //    newValue.LoadCompleted += (sender, args) => { Log.i("MainFrame.LoadCompleted"); };
        //    newValue.NavigationFailed += (sender, args) => { Log.i("MainFrame.NavigationFailed"); };
        //    newValue.NavigationProgress += (sender, args) => { Log.i("MainFrame.NavigationProgress"); };
        //    newValue.NavigationStopped += (sender, args) => { Log.i("MainFrame.NavigationStopped"); };
        //});
        ///// <summary>
        ///// Get or set the currently displayed Page
        ///// </summary>
        //public Property<ApplicationModel<TModel>, System.Windows.Controls.Page> CurrentPage { get; } = new Property<ApplicationModel<TModel>, System.Windows.Controls.Page>((owner, oldPage, newPage) =>
        //{
        //    if (oldPage != newPage)
        //        owner.MainFrame?.Value?.Navigate(newPage);
        //});
        private string GetApplicationTitle()
        {
            var result = string.Empty;
            var asm = System.Reflection.Assembly.GetEntryAssembly();
            var info = asm.FullName.Split(',');
            var title = info[0];
            var version = info[1].Replace("Version=", "v");
            string deploymentVersion = "";
            result = title;

            try
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed && System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion != null)
                {
                    deploymentVersion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                    result += " v" + deploymentVersion;
                }
                else
                {
                    if (!string.IsNullOrEmpty(version))
                        result += " " + version;
                }
            }
            //catch
            //{
            //}
            finally
            {
                Log.i("Version={0}, Deployment Version={1}", version, deploymentVersion);
            }
            return result;
        }
        private void ParseCommandLine()
        {
            var commands = Environment.CommandLine.Split(new char[] { '/', '-' });
            foreach (string command in commands)
            {
                switch (command.Trim()) //this value must be trimmed to eliminate trailing spaces
                {
                    case "debug":
                        IsDebugMode.Value = true;
                        break;
                    case "advanced":
                        IsAdministrator.Value = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Can be overridden to provide custom creation code
        /// </summary>
        /// <remarks>Called after OnInitialize. Not called when in design mode.</remarks>
        protected override void OnCreate()
        {
            base.OnCreate();

            Title.Value = GetApplicationTitle();

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            var myPrincipal = System.Security.Principal.WindowsPrincipal.Current; //OR System.Threading.Thread.CurrentPrincipal

#if DEBUG
            IsDebugMode.Value = true;
            IsAdministrator.Value = true;// System.Security.Principal.WindowsPrincipal.Current.IsInRole("SERVER\\Domain Admins");
#else
            IsDebugMode.Value = false;
            IsAdministrator.Value = myPrincipal.IsInRole("BUILTIN\\Administrators");

            //IsAdministrator.Value = true;// System.Security.Principal.WindowsPrincipal.Current.IsInRole("GAMMA\\Administrators");
#endif
            if (IsDebugMode.Value)
                this.Title.Value = this.Title.Value + "@";
            if (IsAdministrator.Value)
                this.Title.Value = "@" + this.Title.Value;
        }
        
        /// <summary>
        /// Constructs a new ApplicationModel object
        /// </summary>
        public ApplicationModel() : base()
        {
            this.InitializeProperties();
        }
    }
}
