using System;
using System.Collections.Generic;
using System.Text;

namespace W
{
    /// <summary>
    /// Provides the Disposable pattern as a base class
    /// </summary>
    public class Disposable : IDisposable
    {
        private object _disposeLock = new object();

        /// <summary>
        /// If True, the object has been disposed
        /// </summary>
        protected bool IsDisposed = false; // To detect redundant calls
        /// <summary>
        /// If True, the object is in the process of disposing
        /// </summary>
        protected bool IsDisposing = false;

        /// <summary>
        /// Override to release unmanaged objects
        /// </summary>
        protected virtual void OnDisposeUnmanaged()
        {

        }
        /// <summary>
        /// Overload to dispose managed objects
        /// </summary>
        protected virtual void OnDispose()
        {

        }
        private void Dispose(bool disposing)
        {
            lock (_disposeLock)
            {
                IsDisposing = disposing;
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                        OnDispose();
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    OnDisposeUnmanaged();
                    // TODO: set large fields to null.
                    IsDisposed = true;
                }
            }
        }

        /// <summary>
        /// Override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        /// </summary>
        ~Disposable()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        /// <summary>
        /// This code added to correctly implement the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
    }
}
