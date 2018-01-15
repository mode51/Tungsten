using System;
using System.Collections.Generic;
using System.Text;

namespace W.Net.Sockets
{
    /// <summary>
    /// Minimum implementation of W.Net.XClient
    /// </summary>
    public interface IDataSocket
    {
        /// <summary>
        /// Called when a connection has been established
        /// </summary>
        Action<IDataSocket, System.Net.IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the connection has been terminated
        /// </summary>
        Action<IDataSocket, System.Net.IPEndPoint, Exception> Disconnected { get; set; }
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        Action<IDataSocket, byte[]> RawDataReceived { get; set; }
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        Action<IDataSocket, byte[]> DataReceived { get; set; }
        ///// <summary>
        ///// Called after data has been formatted and sent
        ///// </summary>
        //Action<IDataSocket, byte[]> DataSent { get; set; }
        ///// <summary>
        ///// True if the socket is in a connected state, otherwise False.
        ///// </summary>
        //bool IsConnected { get; }

        /// <summary>
        /// The underlying Tungsten Socket
        /// </summary>
        W.Net.Sockets.Socket Socket { get; }

        /// <summary>
        /// Sends data to the remote
        /// </summary>
        /// <param name="bytes">The data to send to the remote</param>
        ulong Send(byte[] bytes);
    }
}
