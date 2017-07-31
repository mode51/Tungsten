using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Core
{
    /// <summary>
    /// Base class for a disposable dependency object
    /// </summary>
    public class DisposableDependencyObject : DependencyObjectBase, IDisposable //where T : class, new()
    {
        #region IDisposable
        //[Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// True if the object has been disposed, otherwise False
        /// </summary>
        public bool IsDisposed { get; private set; }
        //[Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// True if the object is being disposed, otherwise False
        /// </summary>
        protected bool IsDisposing { get; private set; }

        /// <summary>
        /// Deconstructs the object and calls Dispose
        /// </summary>
        ~DisposableDependencyObject()
        {
            Dispose();// (false);
        }

        /// <summary>
        /// Override to provide custom dispose functionality
        /// </summary>
        protected virtual void OnDispose()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + " Disposed");
#endif
        }
        #region IDisposable Members
        /// <summary>
        /// Disposes the object and frees resources
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed && !IsDisposing)
            {
                IsDisposing = true;
                OnDispose();
                IsDisposed = true;
                IsDisposing = false;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
        #endregion
    }
}
