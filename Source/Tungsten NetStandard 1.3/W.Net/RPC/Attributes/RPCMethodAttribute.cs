using System;

namespace W.Net.RPC
{
    /// <summary>
    /// Add this attribute to a static method and Tungsten.Net.RPC.Server can automatically add the method to it's dictionary of callable methods.
    /// </summary>
    /// <remarks>Note: Due to the way Newtonsoft.Json deserializes integers, do NOT use int (Int32) in your RPC methods as parameters or return types; use longs instead.</remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class RPCMethodAttribute : Attribute
    {
    }
}
