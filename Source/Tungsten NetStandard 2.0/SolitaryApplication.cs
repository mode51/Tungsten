using System;
using System.Collections.Generic;
using System.Text;

namespace W
{
    /// <summary>
    /// A helper class which makes it easy to disallow concurrent instances of an application
    /// </summary>
    /// <example>
    /// A sample Program.Main method.  Note that any action may, or need not, be performed in the Activate event.
    /// <code>
    /// [STAThread]
    /// static void Main()
    /// {
    ///     var instance = SolitaryApplication.RegisterInstance("MyApplicationName", true);
    ///     if (instance != null)
    ///     {
    ///         using (instance)
    ///         {
    ///             Application.EnableVisualStyles();
    ///             Application.SetCompatibleTextRenderingDefault(false);
    ///             var mainform = new frmMain();
    ///             instance.Activate += (o, e) => { mainform.Show(); mainform.Activate(); };
    ///             Application.Run(mainform);
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public class SolitaryApplication : IDisposable
    {
        private System.Threading.Mutex _mutex;
        private System.Threading.Thread _thread = null;
        private System.Threading.EventWaitHandle _activate = null;
        private long _exitWhenZero = 1;

        /// <summary>
        /// Delegate for the Activate event
        /// </summary>
        public delegate void ActivateDelegate(object sender, EventArgs e);
        /// <summary>
        /// Raised when a user tries to open another instance of your application
        /// </summary>
        public event ActivateDelegate Activate;
        /// <summary>
        /// Allows the programmer to test for an existing solitary application instance
        /// </summary>
        /// <param name="applicationName">The name of the solitary application</param>
        /// <returns></returns>
        public static bool IsRunning(string applicationName)
        {
            var result = false;
            using (var _singleInstance = new System.Threading.Mutex(false, applicationName))
            {
                try
                {
                    result = _singleInstance.WaitOne(TimeSpan.Zero);
                    if (result)
                        _singleInstance.ReleaseMutex();
                }
                catch (System.Threading.AbandonedMutexException)
                {
                    result = true;
                }
            }
            return !result;
        }
        /// <summary>
        /// Notifies the solitary application that it should activate
        /// </summary>
        /// <param name="applicationName"></param>
        public static void ActivateApplication(string applicationName)
        {
            using (var activate = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, "activate_" + applicationName))
            {
                activate.Set();
            }
        }
        /// <summary>
        /// Attempt to register an application as the first and only allowed instance
        /// </summary>
        /// <param name="applicationName">The name of the desired solitary application</param>
        /// <param name="activateExistingInstance">If true, any existing solitary application will automatically be notified to activate.</param>
        /// <returns></returns>
        public static SolitaryApplication RegisterInstance(string applicationName, bool activateExistingInstance = true)
        {
            var singleInstance = new System.Threading.Mutex(false, applicationName);
            try
            {
                var result = singleInstance.WaitOne(TimeSpan.Zero);
                if (result)
                    return new SolitaryApplication(singleInstance, applicationName);
                else
                {
#if NETSTANDARD2_0 || NET45
                    singleInstance.Dispose();
#else
                    singleInstance.Close();
#endif
                    //tell the other instance to activate
                    if (activateExistingInstance)
                    {
                        ActivateApplication(applicationName);
                    }
                }
            }
            catch (System.Threading.AbandonedMutexException)
            {
#if NETSTANDARD2_0 || NET45
                singleInstance.Dispose();
#else
                singleInstance.Close();
#endif
            }
            return null;
        }
        /// <summary>
        /// Disposes the SolitaryApplication and releases resources
        /// </summary>
        public void Dispose()
        {
            var value = System.Threading.Interlocked.Read(ref _exitWhenZero);
            if (value > 0)
                System.Threading.Interlocked.Decrement(ref _exitWhenZero);
            _activate.Set(); //unblock the thread
            _mutex.ReleaseMutex();
            _thread.Join(10);
        }
        /// <summary>
        /// Constructs a new SolitaryApplication with the given parameters
        /// </summary>
        /// <param name="mutex">The mutex associated with this solitary application</param>
        /// <param name="applicationName">The name of the solitary application</param>
        protected SolitaryApplication(System.Threading.Mutex mutex, string applicationName)
        {
            _mutex = mutex;
            _activate = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, "activate_" + applicationName);
            _thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                while (System.Threading.Interlocked.Read(ref _exitWhenZero) > 0)
                {
                    _activate.WaitOne();
                    if (System.Threading.Interlocked.Read(ref _exitWhenZero) > 0)
                    {
                        var evt = Activate;
                        evt?.Invoke(this, new EventArgs());
                        _activate.Reset();
                    }
                }
            }));
            _thread.Start();
        }
    }
}