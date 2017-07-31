using System;

namespace W.Net.Sockets
{
    /// <summary>
    /// Minimum implementation of a generic W.Net.XClient
    /// </summary>
    public interface IMessageSocket<TClientType, TMessageType> //dont' inherit IDataSocket so that the implementations are separate //: IDataSocket
    {
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        Action<TClientType, TMessageType> MessageReceived { get; set; }

        //we can't decrypt the sent data because we don't have the private key (so we need another way to track sent data)
        ///// <summary>
        ///// Called after data has been formatted and sent
        ///// </summary>
        //Action<TClientType, TMessageType> MessageSent { get; set; }
    }
}