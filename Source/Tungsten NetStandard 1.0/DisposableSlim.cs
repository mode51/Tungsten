using System;
using System.Diagnostics;

namespace W
{
    public class DisposableSlim : IDisposable
    {
        private bool _isDisposed = false;
        private readonly object _disposeLock = new object();

        ~DisposableSlim()
        {
            Dispose();// (false);
        }

        protected virtual void OnDispose()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + " Disposed");
            //Log.i(this.GetType().Name + " Disposed");
#endif
        }
        #region IDisposable Members
        //private void Dispose(bool disposing)
        //{
        //    IsDisposing = disposing;
        //    if (!IsDisposed && disposing)
        //        OnDispose();
        //    IsDisposed = true;
        //    IsDisposing = false;
        //}
        public void Dispose()
        {
            if (_isDisposed) return;
            lock (_disposeLock)
            {
                OnDispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
