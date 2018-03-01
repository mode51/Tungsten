using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using W.Threading.Lockers;

namespace W.Net
{
    public static partial class Udp
    {
        /// <summary>
        /// Contains the generic implementation of W.Net.UdpPeer
        /// </summary>
        public static partial class Generic
        {
            /// <summary>
            /// A generic Udp peer
            /// </summary>
            public class UdpPeer<TType> : IDisposable
            {
                private UdpPeer _peer;
                private Disposer _disposer = new Disposer();

                /// <summary>
                /// Raised when a message has been received from a client
                /// </summary>
                public event Action<IPEndPoint, TType> MessageReceived;

                /// <summary>
                /// Sends a message to the specified remote client
                /// </summary>
                /// <param name="message">The message to send</param>
                /// <param name="remoteEndPoint">The remote client which is listening for messages</param>
                /// <returns></returns>
                public async Task SendAsync(TType message, IPEndPoint remoteEndPoint)
                {
                    await _peer.SendAsync(SerializationMethods.Serialize(message).AsBytes(), remoteEndPoint);
                }

                /// <summary>
                /// Disposes the UdpServer and release resources
                /// </summary>
                public void Dispose()
                {
                    _disposer.Dispose(() =>
                    {
                        _peer.Dispose(); //will raise an exception in the thread method, thus terminating the thread
                });
                }
                /// <summary>
                /// Constructs a new UdpServer
                /// </summary>
                /// <param name="localEndPoint">The local IPEndPoint on which to listen for data</param>
                /// <param name="useCompression">If True, messages will be compressed before sending and decompressed when received</param>
                public UdpPeer(IPEndPoint localEndPoint, bool useCompression)
                {
                    _peer = new UdpPeer(localEndPoint, useCompression);
                    _peer.BytesReceived += (ep, bytes) =>
                    {
                        MessageReceived?.Invoke(ep, SerializationMethods.Deserialize<TType>(ref bytes));
                    };
                }
            }
        }
    }
}
