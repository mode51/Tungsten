using System;

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
