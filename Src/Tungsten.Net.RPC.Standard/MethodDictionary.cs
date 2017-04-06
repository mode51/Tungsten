using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using W.Logging;

namespace W.Net.RPC
{
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
            //scan the assemblies for RPC attributes
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //var assemblies = Microsoft.Framework.Runtime.LibraryManager.DependencyContext.Default.RuntimeLibraries;
            var assemblies = System.Reflection.Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
            //var srcPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
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

        public T Call<T>(string method, params object[] args)
        {
            return (T)Convert.ChangeType(Call(method, args).Result, typeof(T));
        }
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

        public void Refresh()
        {
            Clear();
            FindAllRPCMethods();
        }
    }
}
