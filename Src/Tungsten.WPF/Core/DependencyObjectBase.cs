using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using W.Logging;

namespace W.WPF.Core
{
    /// <summary>
    /// A base class for DependencyObjects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DependencyObjectBase : DependencyObject //where T : class, new()
    {
        /// <summary>
        /// Returns the Dispatcher for this dependency object
        /// </summary>
        public new System.Windows.Threading.Dispatcher Dispatcher
        {
            get
            {
                return (base.Dispatcher ?? System.Windows.Threading.Dispatcher.CurrentDispatcher);
            }
        }
        /// <summary>
        /// Called immediately when the object is created
        /// </summary>
        protected virtual void OnInitialize()
        {
            Log.i(this.GetType().Name + ".OnInitialize");
        }
        /// <summary>
        /// Called after OnInitialize if IsInDesignMode == False
        /// </summary>
        protected virtual void OnCreate()
        {
            Log.i(this.GetType().Name + ".OnCreate");
        }
        /// <summary>
        /// Called after OnInitialize if IsInDesignMode == True
        /// </summary>
        protected virtual void OnCreateInDesignMode()
        {
            Log.i(this.GetType().Name + ".OnCreateInDesignMode");
        }
        /// <summary>
        /// True if the code is running in design mode, otherwise False
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
            }
        }
        /// <summary>
        /// Constructs a new DependencyObjectBase
        /// </summary>
        public DependencyObjectBase()
        {
            OnInitialize();
            if (IsInDesignMode)
            {
                OnCreateInDesignMode();
            }
            else
            {
                OnCreate();
            }
        }
    }
}