using System;
using System.Collections.Generic;
using System.Text;

namespace W.Net.RPC
{
    /// <summary>
    /// Encapsulates information related to making the RPC call and the return value
    /// </summary>
    public class RPCResponse
    {
        /// <summary>
        /// The name of the method called
        /// </summary>
        public string Method { get; set; } = "";
        /// <summary>
        /// True if the call was successful, otherwise False
        /// </summary>
        /// <remarks>Note that this is different than the return value from the method, which can be of any value or type</remarks>
        public bool Success { get; set; } = false;
        /// <summary>
        /// The return value from the method
        /// </summary>
        public object Response { get; set; } = null;
        /// <summary>
        /// May contain exception information if there was an exception making or as a result of the call
        /// </summary>
        public string Exception { get; set; } = "";

        /// <summary>
        /// Useful for debugging or displaying information quickly.
        /// </summary>
        /// <returns>Returns a string representation of class members and their values</returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, "Name = " + Method, "Success = " + Success.ToString(), "Exception = " + Exception, "Response = " + Response?.ToString());
        }
    }
    /// <summary>
    /// Encapsulates information related to making the RPC call and the return value
    /// </summary>
    /// <typeparam name="TResponseType">The Type expected as a return value from the method call</typeparam>
    public class RPCResponse<TResponseType> : RPCResponse
    {
        /// <summary>
        /// The return value from the method
        /// </summary>
        public new TResponseType Response
        {
            get
            {
                return (TResponseType)base.Response;
            }
            set
            {
                base.Response = value;
            }
        }
    }
}
