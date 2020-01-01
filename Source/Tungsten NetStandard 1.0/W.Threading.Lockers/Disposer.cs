using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace W.Threading.Lockers
{
    /// <summary>
    /// <para>Aids in implementing a clean Dispose method.  Supports re-entrancy but only calls the cleanup Action once.
    /// </para>
    /// </summary>
    public class Disposer
    {
        private SpinLocker<bool> _locker = new SpinLocker<bool>();
        private volatile bool _isDisposing = false;

        /// <summary>
        /// True if the Disposer is in the process of disposing, otherwise False
        /// </summary>
        public bool IsDisposing { get { return _isDisposing; } }

        /// <summary>
        /// True if Cleanup has been called and completed, otherwise False
        /// </summary>
        public bool IsDisposed { get { return _locker.InLock(isDisposed => { return isDisposed; }); } }
        /// <summary>
        /// Calls the action (should contain cleanup code)
        /// </summary>
        /// <param name="cleanupAction">The action to call</param>
        public void Dispose(Action cleanupAction)
        {
            _locker.InLock(isDisposed => { if (!isDisposed) cleanupAction.Invoke(); });
        }
        /// <summary>
        /// Calls the action (should contain cleanup code)
        /// </summary>
        /// <param name="objToSupressFinalize">The object on which to suppress the finalizer call (usually the one currently being disposed)</param>
        /// <param name="cleanupAction">The action to call</param>
        public void Dispose(object objToSupressFinalize, Action cleanupAction)
        {
            _locker.InLock(isDisposed =>
            {
                if (!isDisposed && !_isDisposing)
                {
                    _isDisposing = true;
                    cleanupAction.Invoke();
                    GC.SuppressFinalize(objToSupressFinalize);
                    _isDisposing = false;
                }
                return true;
            });
        }
    }
}
