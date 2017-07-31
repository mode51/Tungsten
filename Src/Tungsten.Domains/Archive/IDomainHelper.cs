using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace W.Domains
{
    internal interface IDomainHelper : IDisposable
    {
        object CreateInstanceAndUnwrap(string assemblyName, string typeName);
        T      CreateInstanceAndUnwrap<T>(string assemblyName, string typeName);
        
        int    ExecuteAssemblyByName(string assemblyName, params string[] args);
        
        object ExecuteStaticMethod(string typeName, string methodName, params object[] args);
        object ExecuteStaticMethod(string assemblyName, string typeName, string methodName, params object[] args);

        object[] GetAllItemsOfType<T>();
        object[] GetAllItemsWithInterface<T>();
        //bool KeepAlive();
        
        int    Load(params string[] assemblyNames);
    }
}
