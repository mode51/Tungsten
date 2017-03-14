using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using W.Logging;

namespace W.Domains
{
    /// <summary><para>
    /// Allows developers to create child AppDomains while contain all or specified dlls and allows for shadow copying.  Shadow copying allows the domain to be unloaded at runtime and reloaded if desired.
    /// </para></summary>
    /// <remarks>Objects loaded in the domain must be accessed via an interface and the objects must inherit MarshalByRefObject (and be serializable)</remarks>
    public class CustomAppDomain : IDisposable
    {
        protected AppDomain Domain { get; private set; }

        //public System.Reflection.Assembly[] Assemblies => Domain.GetAssemblies();
        public string Name { get; private set; }

        public CustomAppDomain(string personalPath, bool shadowCopyFiles, bool autoloadAllDlls = true) : this(Guid.NewGuid().ToString(), personalPath, shadowCopyFiles, autoloadAllDlls) { }
        public CustomAppDomain(string name, string personalPath, bool shadowCopyFiles, bool autoloadAllDlls = true) : this(name, personalPath, shadowCopyFiles, null)
        {
            try
            {
                if (autoloadAllDlls)
                {
                    using (var helper = DomainHelper.GetHelper<IDomainHelper>(Domain))
                    {
                        foreach (var filename in System.IO.Directory.GetFiles(personalPath))
                        {
                            if (System.IO.Path.GetExtension(filename).ToLower().Equals(".dll"))
                            {
                                helper.Load(filename);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }
        /// <summary>
        /// Creates a new AppDomain using a Guid to name it.  ShadowCopying is disabled.
        /// </summary>
        /// <param name="assemblyNames">An array of strings specifying the assemblies to load</param>
        public CustomAppDomain(params string[] assemblyNames) : this(Guid.NewGuid().ToString(), "", false, assemblyNames) { }
        /// <summary>
        /// Creates a new AppDomain with the specified name, personal path and allows shadow copying of files.
        /// </summary>
        /// <param name="name">The name for the AppDomain</param>
        /// <param name="personalPath">The relative path to the subfolder containing files for the AppDomain</param>
        /// <param name="shadowCopyFiles">True to use shadow copying.  This allows the domain to be reloaded at runtime to handle file changes.</param>
        /// <param name="assemblyNames"></param>
        public CustomAppDomain(string personalPath, string name = "", bool shadowCopyFiles = false, params string[] assemblyNames)
        {
            if (string.IsNullOrEmpty(name))
                name = Guid.NewGuid().ToString();
            //if (!System.IO.Path.IsPathRooted(personalPath))
            //    personalPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, personalPath);
            AppDomainSetup setup = null;
            Name = name;
            if (shadowCopyFiles)
            {
                var rootPath = System.IO.Path.GetDirectoryName(personalPath);
                var probingPath = System.IO.Path.GetFileName(personalPath.Trim('\\'));
                var cachePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, probingPath + "_shadowcopy");
                setup = CreateDomainSetup(Name, rootPath, personalPath, cachePath, probingPath);
                Domain = AppDomain.CreateDomain(Name, null, setup);
            }
            else
            {
                var probingPath = System.IO.Path.GetFileName(personalPath.Trim('\\'));
                setup = CreateDomainSetup(Name, AppDomain.CurrentDomain.BaseDirectory, personalPath, "", probingPath);
                Domain = AppDomain.CreateDomain(Name, null, AppDomain.CurrentDomain.BaseDirectory, personalPath, false);
            }
            if (assemblyNames != null)
                LoadAssemblies(assemblyNames);
        }
        ~CustomAppDomain()
        {
            Dispose();
        }

        protected void OnDispose()
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            helper?.Dispose();
            if (Domain != null)
            {
                try
                {
                    AppDomain.Unload(Domain);
                }
                catch (CannotUnloadAppDomainException e)
                {
                    Log.e(e);
                }
            }
            Domain = null;
        }
        protected void LoadAssemblies(params string[] assemblyNames)
        {
            if (assemblyNames?.Length > 0)
            {
                var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
                foreach (var assembly in assemblyNames)
                {
                    helper.Load(assembly);
                }
            }
        }

        private AppDomainSetup CreateDomainSetup(string name, string applicationBase, string privatePath, string cachePath, string probingPath)
        {
            var setup = new AppDomainSetup
            {
                ApplicationName = name,
                ApplicationBase = applicationBase,
                PrivateBinPath = probingPath,
                CachePath = cachePath,
                ShadowCopyFiles = string.IsNullOrEmpty(cachePath) ? "false" : "true",
                ShadowCopyDirectories = privatePath
            };
            return setup;
        }

        public void Dispose()
        {
            OnDispose();
        }

        public FileVersionInfo GetAssemblyVersion(string path)
        {
            var asm = Domain.GetAssemblies().FirstOrDefault(a => a.Location == path);
            if (asm != null)
            {
                var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return fvi;
            }
            return null;
        }
        public object CreateInstanceAndUnwrap(string assemblyName, string typeName)
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            var result = helper.CreateInstanceAndUnwrap(assemblyName, typeName);
            return result;
        }
        public T CreateInstanceAndUnwrap<T>(string assemblyName, string typeName)
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            var result = helper.CreateInstanceAndUnwrap<T>(assemblyName, typeName);
            return result;
        }
        public int ExecuteAssemblyByName<T>(string assemblyName, params string[] args)
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            return helper.ExecuteAssemblyByName(assemblyName, args);
        }
        public object[] GetAllItemsOfType<T>() where T : class
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            var items = helper.GetAllItemsOfType<T>();
            return items;
        }
        public object[] GetAllItemsWithInterface<T>() where T : class
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            var items = helper.GetAllItemsWithInterface<T>();
            return items;
        }

        public T ExecuteStaticMethod<T>(string typeName, string methodName, params object[] args)
        {
            return (T)ExecuteStaticMethod(typeName, methodName, args);
        }
        public T ExecuteStaticMethod<T>(string assemblyName, string typeName, string methodName, params object[] args)
        {
            return (T)ExecuteStaticMethod(assemblyName, typeName, methodName, args);
        }
        public object ExecuteStaticMethod(string typeName, string methodName, params object[] args)
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            return helper.ExecuteStaticMethod(typeName, methodName, args);
        }
        public object ExecuteStaticMethod(string assemblyName, string typeName, string methodName, params object[] args)
        {
            var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
            return helper.ExecuteStaticMethod(assemblyName, typeName, methodName, args);
        }
        //public bool KeepAlive() //this member is to be called periodically, to keep the remoting session open (hopefully it'll work)
        //{
        //    var helper = DomainHelper.GetHelper<IDomainHelper>(Domain);// GetDomainHelper();
        //    return helper.KeepAlive();
        //}

        //protected IDomainHelper GetDomainHelper(string typeName = "TheGeneral.Common.Domains.DomainHelper")
        //{
        //    var helper = (IDomainHelper)DomainHelper.ExecuteStaticMethodEx(typeName, "GetInstance", Domain);
        //    return helper;
        //}
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    }
}
