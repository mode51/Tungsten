using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using W.Logging;

namespace W.Net.RPC
{
    /// <summary>
    /// Used to store and call RPC methods on a Tungsten.Net.RPC Server
    /// </summary>
    public class MethodDictionary : Dictionary<string, MethodInfo>
    {
        private void ExamineAssembly(Assembly asm)
        {
            foreach (var t in asm.GetExportedTypes())
            {
                foreach (var a in t.GetTypeInfo().GetCustomAttributes())
                {
                    if (a is RPCClassAttribute)
                    {
                        foreach (var mi in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
                        {
                            foreach (var a2 in mi.GetCustomAttributes())
                            {
                                if (a2 is RPCMethodAttribute)
                                {
                                    var key = t.Namespace + "." + t.Name + "." + mi.Name;
                                    Add(key, mi);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void FindAllRPCMethods()
        {
            //4.12.2017 - The use of GetEntryAssembly requires netstandard1.5.  I can't see any way to use an earlier version of netstandard.
            //scan the assemblies for RPC attributes
            //var assemblies2 = AppDomain.CurrentDomain.GetAssemblies();
            //var assemblies = Microsoft.Framework.Runtime.LibraryManager.DependencyContext.Default.RuntimeLibraries;
            //var srcPath2 = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            var assemblies = System.Reflection.Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
            var srcPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            Clear();
            ExamineAssembly(System.Reflection.Assembly.GetEntryAssembly());
            foreach (var asmName in assemblies)
            {
                //ignore System.X assemblies
                if (asmName.Name.StartsWith("System."))
                    continue;
                //ignore Microsoft.X assemblies
                if (asmName.Name.StartsWith("Microsoft."))
                    continue;
                //ignore nunitassemblies
                if (asmName.Name.StartsWith("nunit.framework"))
                    continue;
                var asm = System.Reflection.Assembly.Load(asmName);
                ExamineAssembly(asm);
            }
            Console.WriteLine($"Found {Count} RPC Methods");
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
            return (TResult)Convert.ChangeType(Call(method, args).Result, typeof(TResult));
        }
        /// <summary>
        /// Call a method on the Tungsten.Net.RPC Server.  This method s
        /// </summary>
        /// <param name="method">The namespace, class name and method name of the method to call (ie: MyNamespace.MyClass.Method1)</param>
        /// <param name="args">Arguments, if any, to be passed into the remote method</param>
        /// <returns>A CallResult object describing the result of the call.  If the remote method does not have a return value, the value of CallResult.Result will be null.</returns>
        public CallResult<object> Call(string method, params object[] args)
        {
            var result = new CallResult<object>(true, null);
            MethodInfo mi = null;
            if (ContainsKey(method))
                mi = this[method];

            try
            {
                if (args != null)
                {
                    var parameters = mi?.GetParameters();
                    if (parameters != null)
                    {
                        if (parameters.Length != args.Length)
                            throw new Exception("Wrong number of arguments");
                        for (int t = 0; t < args.Length; t++)
                        {
                            if (args[t] is Newtonsoft.Json.Linq.JToken)
                                args[t] = ((Newtonsoft.Json.Linq.JToken)args[t]).ToObject(parameters[t].ParameterType);
                            //args[t] = Newtonsoft.Json.JsonConvert.DeserializeObject((string)args[t], parameters[t].ParameterType);
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
                        result.Result = mi?.Invoke(null, args);
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }

        /// <summary>
        /// Scans the server process for RPC methods (static methods with the RPCMethod attribute in classes with the RPCClass attribute)
        /// </summary>
        /// <remarks>Any methods previously added manually will have to be re-added</remarks>
        public void Refresh()
        {
            Clear();
            FindAllRPCMethods();
        }
    }
}
