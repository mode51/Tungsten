using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using W.Logging;

namespace W.Domains
{
    /// <summary>
    /// This class is loaded in the new domain, but used by the calling domain
    /// </summary>
    [Serializable]
    internal class DomainHelper : IDisposable, IDomainHelper
    {
        protected static IDomainHelper _Instance = null;

        public DomainHelper()
        {
            Log.v("Domain({0}): DomainHelper Created, Path={1}", AppDomain.CurrentDomain.Id, AppDomain.CurrentDomain.BaseDirectory);
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            OnDispose();
        }

        protected void OnDispose()
        {
            Log.v("W.Domains.DomainHelper Disposed");
        }
        protected Assembly GetAssembly(string assemblyName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.Equals(assemblyName))
                    return assembly;
            }
            return null;
        }

        public object[] GetAllItemsOfType<T>()
        {
            var result = new List<object>();
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var searchType = typeof(T);

            foreach (var asm in asms)
            {
                if (asm.FullName != typeof(T).Assembly.FullName)
                {
                    foreach (var t in asm.GetExportedTypes())
                    {
                        //if (t.FullName == searchType.FullName)
                        //{
                        //the type, or interface, are somehow of a different dll and therefore are not equal (searchItem is always null)
                        var item = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(asm.FullName, t.FullName);
                        result.Add(item);
                        //}
                        System.Threading.Thread.Sleep(0); //yield cpu
                    }
                }
                System.Threading.Thread.Sleep(0); //yield cpu
            }
            return result.ToArray();
        }
        public object[] GetAllItemsWithInterface<T>()
        {
            var result = new List<object>();
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var searchType = typeof(T);

            foreach (var asm in asms)
            {
                if (asm.FullName != typeof(T).Assembly.FullName)
                {
                    foreach (var t in asm.GetExportedTypes())
                    {
                        var i = t.GetInterface(searchType.FullName);
                        if (i != null && !t.IsAbstract)
                        {
                            var item = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(asm.FullName, t.FullName);
                            //var searchItem = item as ItemType; //this is always null, even when it's the right type (a dll versioning thing?)
                            //if (searchItem != null)
                            result.Add(item);
                        }
                        //foreach (var i in t.GetInterfaces())
                        //{
                        //    if (i.FullName == searchType.FullName)
                        //    {
                        //        //var item = (ItemType)asm.CreateInstance(t.FullName);
                        //        //var item = CreateInstanceAndUnwrap<ItemType>(asm.FullName, t.FullName);

                        //        //the type, or interface, are somehow of a different dll and therefore are not equal (searchItem is always null)
                        //        var item = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(asm.FullName, t.FullName);
                        //        var searchItem = item as ItemType;
                        //        if (searchItem != null)
                        //            result.Add(searchItem);
                        //    }
                        //}
                        System.Threading.Thread.Sleep(0); //yield cpu
                    }
                }
                System.Threading.Thread.Sleep(0); //yield cpu
            }
            return result.ToArray();
        }

        public object CreateInstanceAndUnwrap(string assemblyName, string typeName)
        {
            var result = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
            return result;
        }
        public T CreateInstanceAndUnwrap<T>(string assemblyName, string typeName)
        {
            var result = (T)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
            return result;
        }
        public int ExecuteAssemblyByName(string assemblyName, params string[] args)
        {
            return AppDomain.CurrentDomain.ExecuteAssemblyByName(assemblyName, args);
        }
        public object ExecuteStaticMethod(string assemblyName, string typeName, string methodName, params object[] args)
        {
            var asm = GetAssembly(assemblyName);
            if (asm != null)
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.FullName.Equals(typeName))
                    {
                        return type.InvokeMember(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, args);
                    }
                }
            }
            return null;
        }
        public object ExecuteStaticMethod(string typeName, string methodName, params object[] args)
        {
            //search each assembly
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.FullName.Equals(typeName))
                    {
                        return type.InvokeMember(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, args);
                    }
                }
            }
            return null;
        }
        public static object ExecuteStaticMethodEx(string typeName, string methodName, params object[] args)
        {
            try
            {
                //search each assembly
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.FullName.Equals(typeName))
                        {
                            return type.InvokeMember(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, args);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return null;
        }
        public Assembly Load(string assemblyName)
        {
            if (!System.IO.File.Exists(assemblyName))
                return null;
            var asmName = new AssemblyName() { CodeBase = assemblyName };
            var asm = AppDomain.CurrentDomain.Load(asmName);
            if (asm != null)
            {
                OnAssemblyLoaded(asm);
            }
            return asm;
        }
        public int Load(params string[] assemblyNames)
        {
            int count = 0;
            foreach (var assemblyName in assemblyNames)
            {
                var asm = Load(assemblyName);
                if (asm != null)
                    count++;
            }
            return count;
        }
        //public bool KeepAlive()
        //{
        //    return true;
        //}

        protected virtual void OnAssemblyLoaded(Assembly asm)
        {

        }

        public static T GetHelper<T>(AppDomain domain, string typeName = "W.Domains.DomainHelper") where T : IDomainHelper
        {
            //System.Diagnostics.Debugger.Break();//make sure this function works (are the string values accurate?)
            if (_Instance == null)
            {
                _Instance = (T)domain.CreateInstanceAndUnwrap("Tungsten.Domains", typeName);
            }
            return (T)_Instance;
        }
    }
}
