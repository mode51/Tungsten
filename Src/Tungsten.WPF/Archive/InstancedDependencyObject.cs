using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using W.Logging;

namespace W.WPF.Core
{
    /// <summary>
    /// Base class for a dependency object singleton
    /// </summary>
    /// <typeparam name="TSingletonType"></typeparam>
    public class InstancedDependencyObject<TSingletonType> : Singleton<DependencyObjectBase> where TSingletonType : class, new()
    {
        //private static TSingletonType _instance = default(TSingletonType);
        ///// <summary>
        ///// The singleton
        ///// </summary>
        //public static TSingletonType Instance
        //{
        //    get
        //    {
        //        return _instance;
        //    }
        //}
        /// <summary>
        /// Constructs the new dependency object
        /// </summary>
        public InstancedDependencyObject()
        {
            if (Instance == null)
            {
                OnCreate();
            }
        }
        /// <summary>
        /// Creates the singleton
        /// </summary>
        static InstancedDependencyObject()
        {
            //_instance = new TSingletonType();
        }
    }
}
