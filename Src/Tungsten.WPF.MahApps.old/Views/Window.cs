using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using mah = MahApps.Metro;

namespace W.WPF.MahApps.Views
{
    //for whatever reason, this class cannot inherit WindowBase<TWindow> and extend it.  It won't compile.
    /// <summary>
    /// A WPF window based on MahApps
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class Window<TModel> : mah.Controls.MetroWindow where TModel :  class, new()
    {
        /// <summary>
        /// Gets a handle to the ViewModel
        /// </summary>
        public TModel ViewModel { get; private set; }

        private void HandleLoaded(object sender, RoutedEventArgs args)
        {
            //IsBusy = false;
            OnLoaded(sender, args);
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
        }
        /// <summary>
        /// Gets a handle to the TModel singleton, creating it if necessary
        /// </summary>
        /// <returns>A handle to the model Instance</returns>
        /// <remarks>If the model does not have a static property called Instance, this method will return null</remarks>
        protected static TModel GetViewModelInstance()
        {
            //try
            //{
                var p = typeof(TModel).GetProperty("Instance", BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
                if (p != null)
                    return (TModel)p.GetGetMethod().Invoke(null, null);
            //}
            //catch (Exception e)
            //{
            //    System.Diagnostics.Debug.WriteLine(e.ToString());
            //}
            return default(TModel);
        }
        /// <summary>
        /// Initializes the PageFramework before anything else
        /// </summary>
        static Window()
        {
            PageFramework.Initialize();
        }
        /// <summary>
        /// Create a new window
        /// </summary>
        public Window() : this (false) { }
        /// <summary>
        /// Create a new window
        /// </summary>
        /// <param name="useSingletonVM"></param>
        public Window(bool useSingletonVM)
        {
            //for some reason, reflection isn't able to cast the base type to the real type (MainWindowModel)
            //IsBusy = true;
            if (useSingletonVM)
                ViewModel = GetViewModelInstance();
            else
                ViewModel = Activator.CreateInstance<TModel>();

            var model = ViewModel as IModel;
            if (model != null)
                model.Owner.Value = this;

            DataContext = ViewModel;
            Loaded += HandleLoaded;
        }
    }

}
