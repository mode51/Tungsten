using System;

#if !NETSTANDARD1_3 // NET45 || NETSTANDARD2_0
namespace W.Net.RPC
{
    /// <summary>
    /// Add this attribute to a class if it contains static methods with the RPCMethod attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RPCClassAttribute : Attribute
    {
    }
}
#endif
