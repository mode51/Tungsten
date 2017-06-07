using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W
{
    /// <summary>
    /// Thread-safe Singleton implementation
    /// </summary>
    /// <typeparam name="TSingletonType">The singleton Type</typeparam>
    /// <remarks>Adapted from MSDN article "Implementing Singleton in C#"</remarks>
    /// <see ref="https://msdn.microsoft.com/en-us/library/ff650316.aspx">MSDN Article: Implementing Singleton in C#</see>
    public class Singleton<TSingletonType> where TSingletonType : class, new()
    {
        private static volatile TSingletonType _instance;
        private static object _lock = new object();

        /// <summary>
        /// Returns the singleton
        /// </summary>
        public static TSingletonType Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(_lock)
                    {
                        if (_instance == null)
                            _instance = Activator.CreateInstance<TSingletonType>();
                    }
                }
                return (TSingletonType)_instance;
            }
        }
    }
}
