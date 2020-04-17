using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using W;
using W.Logging;

namespace W.Net.RPC
{
    /// <summary>
    /// Used to store and call RPC methods on a Tungsten.Net.RPC Server
    /// </summary>
    public class MethodDictionary //: Dictionary<string, MethodInfo>
    {
        /// <summary>
        /// All of the RPC methods found.  The name of each method is the complete namespace.class.methodname hierarchy (recursive classes are allowed).
        /// </summary>
        public Dictionary<string, MethodInfo> Methods { get; } = new Dictionary<string, MethodInfo>();

        private void FindAllRPCMethods(Type type)
        {
            foreach (var a in type.GetTypeInfo().GetCustomAttributes())
            {
                if (a is RPCClassAttribute)
                {
                    foreach (var mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
                    {
                        foreach (var a2 in mi.GetCustomAttributes())
                        {
                            if (a2 is RPCMethodAttribute)
                            {
                                var key = type.FullName.Replace("+", ".") + "." + mi.Name;// type.Namespace + "." + type.DeclaringType.Name + "." + type.Name + "." + mi.Name;
                                if (!Methods.ContainsKey(key))
                                    Methods.Add(key, mi);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Scans an assembly for all the classes which have the RPCClass attribute and then find all of their methods with the RPCMethod attribute
        /// </summary>
        /// <param name="asm"></param>
        private void FindAllRPCMethods(Assembly asm)
        {
            Type type = null;
            try
            {
                foreach (var t in asm.GetTypes().Where(a => !a.IsNotPublic))
                {
                    type = t;
                    Log.v($"Scanning Type: {t.FullName}");
                    FindAllRPCMethods(t);
                }
            }
            catch (Exception e)
            {
                Log.e($"Type={type?.FullName}: {e}");
            }
        }
        private void FindAssemblies(Assembly rootAssembly, bool recurse, string[] excludedAssemblies)
        {
            FindAllRPCMethods(rootAssembly);
            if (!recurse)
                return;
            var assemblies = rootAssembly.GetReferencedAssemblies();
            foreach (var asmName in assemblies)
            {
                var assemblyName = asmName.Name.ToLower();
                if (assemblyName == "mscorlib")
                    continue;
                if (assemblyName == "system")
                    continue;
                //ignore System.X assemblies
                if (assemblyName.StartsWith("system."))
                    continue;
                //ignore Microsoft.X assemblies
                if (assemblyName.StartsWith("microsoft."))
                    continue;
                //ignore Microsoft.X assemblies
                if (assemblyName.StartsWith("windows."))
                    continue;
                if (assemblyName.StartsWith("netstandard."))
                    continue;
                //ignore nunitassemblies
                if (assemblyName.StartsWith("nunit.framework"))
                    continue;
                if (excludedAssemblies?.FirstOrDefault(a => a.ToLower() == assemblyName) != null)
                    continue;
                try
                {
                    Log.v($"Scanning Assembly: {assemblyName}");
                    //var asm = System.Reflection.Assembly.Load(asmName);
                    var asm = Assembly.ReflectionOnlyLoad(asmName.FullName);
                    Log.v($"Loaded Assembly: {assemblyName}");
                    FindAssemblies(asm, recurse, excludedAssemblies);
                }
                finally //ignore assembly load failures
                {
                }
            }
        }

        /// <summary>
        /// Call a method on the Tungsten.Net.RPC Server.
        /// </summary>
        /// <typeparam name="TResult">The expected return type of the call</typeparam>
        /// <param name="method">The namespace, class name and method name of the method to call (ie: MyNamespace.MyClass.Method1)</param>
        /// <param name="args">Arguments, if any, to be passed into the remote method</param>
        /// <returns>A result of type TResult</returns>
        /// <remarks>If TResult does not match the return type of the method on the server, a return value cannot be expected and the call may time out.</remarks>
        public TResult Call<TResult>(string method, params object[] args)
        {
            var success = Call(out object result, out Exception e, method, args);
            if (result == null) //7.19.2017
                return default(TResult);
            var convertible = result as IConvertible;
            if (convertible == null)
                return (TResult)result;
            return (TResult)Convert.ChangeType(result, typeof(TResult));
        }
        /// <summary>
        /// Call a method on the Tungsten.Net.RPC Server.  This method s
        /// </summary>
        /// <param name="result">The value returned from the called method</param>
        /// <param name="exception">The exception if one occurred</param>
        /// <param name="method">The namespace, class name and method name of the method to call (ie: MyNamespace.MyClass.Method1)</param>
        /// <param name="args">Arguments, if any, to be passed into the remote method</param>
        /// <returns>A CallResult object describing the result of the call.  If the remote method does not have a return value, the value of CallResult.Result will be null.</returns>
        public bool Call(out object result, out Exception exception, string method, params object[] args)
        {
            Log.v($"Request to invoke method: {method}");
            var success = false;
            result = null;
            exception = null;
            MethodInfo mi = null;
            if (Methods.ContainsKey(method))
                mi = Methods[method];
            try
            {
                if (mi != null)
                {
                    if (args != null)
                    {
                        var parameters = mi?.GetParameters();
                        if (parameters != null)
                        {
                            //format each argument
                            for (int t = 0; t < args.Length; t++)
                            {
                                if (args[t] is Newtonsoft.Json.Linq.JToken)
                                    args[t] = ((Newtonsoft.Json.Linq.JToken)args[t]).ToObject(parameters[t].ParameterType);
                                //args[t] = Newtonsoft.Json.JsonConvert.DeserializeObject((string)args[t], parameters[t].ParameterType);
                            }
                            //verify args to parameters
                            if (parameters.Length != args.Length)
                            {
                                //if the last parameter is an array and the arguments outnumber the parameters, then convert the extra arguments into an array
                                if (args.Length > parameters.Length && parameters.Length > 0 && parameters[parameters.Length - 1].ParameterType.IsArray)
                                {
                                    var paramsArgs = new List<object>();
                                    //move the extra parameters into a new parameter array
                                    var lastParameterIndex = parameters.Length - 1;
                                    for (int t = lastParameterIndex; t < args.Length; t++)
                                        paramsArgs.Add(args[t]);

                                    var newArgs = new List<object>();
                                    for (int t = 0; t < lastParameterIndex; t++) //add args prior to the last one (because the last one is the params array)
                                        newArgs.Add(args[t]);
                                    newArgs.Add(paramsArgs.ToArray());
                                    args = newArgs.ToArray();
                                }
                                else
                                    throw new ArgumentException("Wrong number of arguments");
                            }
                        }
                    }

                    if (mi != null)
                    {
                        if (mi.ReturnType.FullName == "System.Void")
                            mi?.Invoke(null, args);
                        else
                            //TODO: 12/4/2016
                            //invoking this method isn't working because the actual User/Address/whatever class isn't being deserialized from JSON
                            //instead it's leaving it as JSON (as JArray or JObject or the like)
                            //figure out how to deserialize the objects as objects rather than json strings
                            result = mi?.Invoke(null, args);
                        success = true;
                    }
                }
                else
                    exception = new NotImplementedException("Method not found: " + method);
            }
            catch (ArgumentException e)
            {
                if (e.Message.Contains("'System.Int64' cannot be converted to type 'System.Int32'"))
                {
                    exception = new ArgumentException("Invalid parameter or return type 'int'.  Use 'long' instead.");
                }
                else
                {
                    exception = new ArgumentException(e.Message);
                }
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
                //Log.e(e);
            }
            catch (Exception e)
            {
                exception = new Exception(e.Message);
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
                //Log.e(e);
            }
            Log.v($"Call Method={method}, Success={success}, Exception={exception}, Result={result}");
            return success;
        }

        ///// <summary>
        ///// Scans an assembly, which contains the specified CLR Type, for RPC methods (static methods with the RPCMethod attribute in classes with the RPCClass attribute)
        ///// </summary>
        ///// <remarks>Any methods previously added manually will have to be re-added</remarks>
        ///// <typeparam name="T">The CLR Type</typeparam>
        ///// <param name="recurse">If True, referenced assemblies will also be scanned</param>
        //public void Refresh<T>(bool recurse)
        //{
        //    var rootAssembly = Assembly.GetAssembly(typeof(T));
        //    if (rootAssembly == null)
        //    {
        //        Log.v($"Root assembly was null");
        //        throw new ArgumentNullException(nameof(rootAssembly), "The supplied assembly cannot be null");
        //    }
        //    Methods.Clear();
        //    FindAssemblies(rootAssembly, recurse);
        //    Log.v($"Found {Methods.Count} RPC Methods");
        //}

        /// <summary>
        /// Scans an assembly for RPC methods (static methods with the RPCMethod attribute in classes with the RPCClass attribute)
        /// </summary>
        /// <remarks>Any methods previously added manually will have to be re-added</remarks>
        /// <param name="recurse">If True, referenced assemblies will also be scanned</param>
        public void Refresh(bool recurse)
        {
            Refresh(recurse, null, new Assembly[] { });
        }
        /// <summary>
        /// Scans an assembly for RPC methods (static methods with the RPCMethod attribute in classes with the RPCClass attribute)
        /// </summary>
        /// <remarks>Any methods previously added manually will have to be re-added</remarks>
        /// <param name="recurse">If True, referenced assemblies will also be scanned</param>
        /// <param name="types">The CLR Types used to determine which Assemblies to scan</param>
        public void Refresh(bool recurse, string[] excludedAssemblies, params Type[] types)
        {
            if (types != null)
            {
                Methods.Clear();
                foreach (var type in types)
                {
                    var rootAssembly = Assembly.GetAssembly(type);
                    if (rootAssembly == null)
                    {
                        Log.v($"Root assembly was null");
                        throw new ArgumentNullException(nameof(rootAssembly), "The supplied assembly cannot be null");
                    }
                    FindAssemblies(rootAssembly, recurse, excludedAssemblies);
                }
            }
            Log.v($"Found {Methods.Count} RPC Methods");
        }
        /// <summary>
        /// Scans an assembly for RPC methods (static methods with the RPCMethod attribute in classes with the RPCClass attribute)
        /// </summary>
        /// <remarks>Any methods previously added manually will have to be re-added</remarks>
        /// <param name="rootAssembly">The Assembly to scan for RPC methods</param>
        /// <param name="recurse">If True, referenced assemblies will also be scanned</param>
        public void Refresh(bool recurse, string[] excludedAssemblies, params Assembly[] assemblies)
        {
            if (assemblies?.Length == 0)
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            Methods.Clear();
            foreach (var asm in assemblies)
            {
                FindAssemblies(asm, recurse, excludedAssemblies);
            }
            Log.v($"Found {Methods.Count} RPC Methods");
        }
    }

    //This would appear to be a simpler, more flexible, implementation.  But I haven't tested it at all.
    //    public class MethodDictionaryAlternative
    //    {
    //        /// <summary>
    //        /// Delegate type used by MethodDictionary to footprint any method
    //        /// </summary>
    //        /// <param name="args">Zero or more arguments to pass into the thread method</param>
    //        public delegate object AnyMethodDelegate(params object[] args);

    //        private Dictionary<string, AnyMethodDelegate> Methods { get; } = new Dictionary<string, AnyMethodDelegate>();
    //        private void FindAllRPCMethods(Type type)
    //        {
    //            foreach (var a in type.GetTypeInfo().GetCustomAttributes())
    //            {
    //                if (a is RPCClassAttribute)
    //                {
    //                    foreach (var mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
    //                    {
    //                        foreach (var a2 in mi.GetCustomAttributes())
    //                        {
    //                            if (a2 is RPCMethodAttribute)
    //                            {
    //                                var key = type.Namespace + "." + type.Name + "." + mi.Name;
    //                                if (!Methods.ContainsKey(key))
    //                                    Methods.Add(key, args => mi.Invoke(null, args));
    //                            }
    //                        }
    //                    }
    //                }
    //            }

    //        }
    //        private void FindAllRPCMethods(Assembly asm)
    //        {
    //            foreach (var t in asm.GetExportedTypes())
    //            {
    //                FindAllRPCMethods(t);
    //            }
    //        }
    //        private void FindAllRPCMethods()
    //        {
    //            //4.12.2017 - The use of GetEntryAssembly requires netstandard1.5.  I can't see any way to use an earlier version of netstandard.
    //            //scan the assemblies for RPC attributes
    //            //var assemblies2 = AppDomain.CurrentDomain.GetAssemblies();
    //            //var assemblies = Microsoft.Framework.Runtime.LibraryManager.DependencyContext.Default.RuntimeLibraries;
    //            //var srcPath2 = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
    //#if NETSTANDARD1_3 || NETSTANDARD1_4
    //            var assemblies = new Assembly[] { };
    //            Assembly entryAsm = null;
    //#elif NETSTANDARD1_5
    //            var assemblies = System.Reflection.Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
    //            //var srcPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
    //            Assembly entryAsm = System.Reflection.Assembly.GetEntryAssembly();
    //#else
    //            var assemblies = System.Reflection.Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
    //            Assembly entryAsm = System.Reflection.Assembly.GetEntryAssembly();
    //#endif
    //            Methods.Clear();
    //            //this only gets W.Net.RPC (not the entry asm) - FindAllRPCMethods(typeof(RPCClassAttribute).GetTypeInfo().Assembly);
    //            if (entryAsm != null)
    //                FindAllRPCMethods(entryAsm);// System.Reflection.Assembly.GetEntryAssembly());
    //            foreach (var asmName in assemblies)
    //            {
    //#if NETSTANDARD1_3 || NETSTANDARD1_4
    //                var assemblyName = asmName.GetName();
    //#else
    //                var assemblyName = asmName.Name;
    //                //ignore System.X assemblies
    //                if (assemblyName.StartsWith("System."))
    //                    continue;
    //                //ignore Microsoft.X assemblies
    //                if (assemblyName.StartsWith("Microsoft."))
    //                    continue;
    //                //ignore Microsoft.X assemblies
    //                if (assemblyName.StartsWith("Windows."))
    //                    continue;
    //                //ignore nunitassemblies
    //                if (assemblyName.StartsWith("nunit.framework"))
    //                    continue;
    //#endif
    //                try
    //                {
    //#if NETSTANDARD1_3 || NETSTANDARD1_4
    //                    var asm = System.Reflection.Assembly.Load(assemblyName);
    //#else
    //                    var asm = System.Reflection.Assembly.Load(asmName);
    //#endif
    //                    FindAllRPCMethods(asm);
    //                }
    //                finally //ignore assembly load failures
    //                {
    //                }
    //            }
    //            Console.WriteLine($"Found {Methods.Count} RPC Methods");
    //        }

    //        /// <summary>
    //        /// Manually adds a specific RPC method
    //        /// </summary>
    //        /// <param name="name">The name of the method</param>
    //        /// <param name="mi">The MethodInfo for the method</param>
    //        private void AddMethod(string name, MethodInfo mi)
    //        {
    //            Methods.Add(name, args => mi.Invoke(null, args));
    //        }
    //        /// <summary>
    //        /// Uses reflection to find all RPC methods provided by the given type
    //        /// </summary>
    //        /// <param name="type">The Type which exposes RPC methods</param>
    //        public void AddMethods(Type type)
    //        {
    //            FindAllRPCMethods(type);
    //        }
    //        /// <summary>
    //        /// Uses reflection to find all RPC methods provided by the given type
    //        /// </summary>
    //        /// <param name="obj">The object which exposes RPC methods</param>
    //        public void AddMethods(object obj)
    //        {
    //            FindAllRPCMethods(obj.GetType());
    //        }
    //        /// <summary>
    //        /// Manually add an action
    //        /// </summary>
    //        /// <param name="name">The name of the action</param>
    //        /// <param name="action">The action to call</param>
    //        public void AddMethod(string name, Func<object> f)
    //        {
    //            Methods.Add(name, (args) => { return f.Invoke(); });
    //        }
    //        /// <summary>
    //        /// Manually add any method
    //        /// </summary>
    //        /// <param name="name">The name of the method</param>
    //        /// <param name="del">The AnyMethodDelegate to be called</param>
    //        public void AddMethod(string name, AnyMethodDelegate del)
    //        {
    //            Methods.Add(name, (args) => del(args));
    //        }

    //        /// <summary>
    //        /// Call a method on the Tungsten.Net.RPC Server.
    //        /// </summary>
    //        /// <typeparam name="TResult">The expected return type of the call</typeparam>
    //        /// <param name="method">The namespace, class name and method name of the method to call (ie: MyNamespace.MyClass.Method1)</param>
    //        /// <param name="exception">The exception if one occurred</param>
    //        /// <param name="args">Arguments, if any, to be passed into the remote method</param>
    //        /// <returns>A result of type TResult</returns>
    //        /// <remarks>If TResult does not match the return type of the method on the server, a return value cannot be expected and the call may time out.</remarks>
    //        public TResult Call<TResult>(string method, out Exception exception, params object[] args)
    //        {
    //            var success = Call(out object result, out exception, method, args);

    //            if (result == null) //7.19.2017
    //                return default(TResult);
    //            var convertible = result as IConvertible;
    //            if (convertible == null)
    //                return (TResult)result;
    //            return (TResult)Convert.ChangeType(result, typeof(TResult));
    //        }
    //        /// <summary>
    //        /// Call a method on the Tungsten.Net.RPC Server.  This method s
    //        /// </summary>
    //        /// <param name="result">The value returned from the called method</param>
    //        /// <param name="exception">The exception if one occurred</param>
    //        /// <param name="method">The namespace, class name and method name of the method to call (ie: MyNamespace.MyClass.Method1)</param>
    //        /// <param name="args">Arguments, if any, to be passed into the remote method</param>
    //        /// <returns>A CallResult object describing the result of the call.  If the remote method does not have a return value, the value of CallResult.Result will be null.</returns>
    //        public bool Call(out object result, out Exception exception, string method, params object[] args)
    //        {
    //            result = null;
    //            exception = null;
    //            if (!Methods.ContainsKey(method))
    //            {
    //                exception = new MissingMethodException($"Method \"{Methods}\" not found");
    //                return false;
    //            }
    //            var m = Methods[method];
    //            try
    //            {
    //                result = m.Invoke(args);
    //            }
    //            catch (Exception e)
    //            {
    //                exception = e;
    //                return false;
    //            }
    //            return true;
    //        }

    //        /// <summary>
    //        /// Scans the server process for RPC methods (static methods with the RPCMethod attribute in classes with the RPCClass attribute)
    //        /// </summary>
    //        /// <remarks>Any methods previously added manually will have to be re-added</remarks>
    //        public void Refresh()
    //        {
    //            Methods.Clear();
    //            FindAllRPCMethods();
    //        }
    //    }
}
