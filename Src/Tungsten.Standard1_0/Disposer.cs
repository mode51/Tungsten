using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using W.LockExtensions;

namespace W
{
    ///// <summary>
    ///// <para>Aids in implementing a clean Dispose method.  Supports re-entrancy but only calls the cleanup Action once.
    ///// </para>
    ///// </summary>
    //public class Disposer
    //{
    //    private SpinLock _locker = new SpinLock(); //not Disposable, which is perfect
    //    private bool _isDisposed = false;

    //    /// <summary>
    //    /// True if Cleanup has been called and completed, otherwise False
    //    /// </summary>
    //    public bool IsDisposed { get { return InLock(() => _isDisposed); } }

    //    /// <summary>
    //    /// Executes a function from within a SpinLock
    //    /// </summary>
    //    /// <param name="func">The function to run</param>
    //    /// <returns>The result of the function</returns>
    //    /// <remarks>Used externally to obtain the value of _isDisposed and internally to lock the cleanup code</remarks>
    //    private TValue InLock<TValue>(Func<TValue> func)
    //    {
    //        return _locker.InLock(func);
    //    }

    //    /// <summary>
    //    /// Calls the action (should contain cleanup code)
    //    /// </summary>
    //    /// <param name="cleanupAction">The action to call</param>
    //    public void Cleanup(Action cleanupAction)
    //    {
    //        InLock<bool>(() =>
    //        {
    //            if (!_isDisposed)
    //            {
    //                cleanupAction.Invoke();
    //                _isDisposed = true;
    //            }
    //            return true;
    //        });
    //    }
    //    /// <summary>
    //    /// Calls the action (should contain cleanup code)
    //    /// </summary>
    //    /// <param name="this">The object on which to suppress the finalizer call (usually the one currently being disposed)</param>
    //    /// <param name="cleanupAction">The action to call</param>
    //    public void Cleanup(object @this, Action cleanupAction)
    //    {
    //        InLock<bool>(() =>
    //        {
    //            if (!_isDisposed)
    //            {
    //                cleanupAction.Invoke();
    //                _isDisposed = true;
    //                GC.SuppressFinalize(@this);
    //            }
    //            return true;
    //        });
    //    }
    //}

    /// <summary>
    /// <para>Aids in implementing a clean Dispose method.  Supports re-entrancy but only calls the cleanup Action once.
    /// </para>
    /// </summary>
    public class Disposer
    {
        private W.Threading.Lockers.SpinLocker<bool> _locker = new Threading.Lockers.SpinLocker<bool>();

        /// <summary>
        /// True if Cleanup has been called and completed, otherwise False
        /// </summary>
        public bool IsDisposed { get { return _locker.InLock(isDisposed => { return isDisposed; }); } }
        /// <summary>
        /// Calls the action (should contain cleanup code)
        /// </summary>
        /// <param name="cleanupAction">The action to call</param>
        public void Cleanup(Action cleanupAction)
        {
            _locker.InLock(isDisposed => { if (!isDisposed) cleanupAction.Invoke(); });
        }
        /// <summary>
        /// Calls the action (should contain cleanup code)
        /// </summary>
        /// <param name="objToSupressFinalize">The object on which to suppress the finalizer call (usually the one currently being disposed)</param>
        /// <param name="cleanupAction">The action to call</param>
        public void Cleanup(object objToSupressFinalize, Action cleanupAction)
        {
            _locker.InLock(isDisposed =>
            {
                if (!isDisposed)
                {
                    cleanupAction.Invoke();
                    GC.SuppressFinalize(objToSupressFinalize);
                }
                return true;
            });
        }
    }
}
