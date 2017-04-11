using System;

namespace W.Net.RPC
{
    /// <summary>
    /// Add this attribute to a static method and Tungsten.Net.RPC.Server can automatically add the method to it's dictionary of callable methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RPCMethodAttribute : Attribute
    {
    }
}