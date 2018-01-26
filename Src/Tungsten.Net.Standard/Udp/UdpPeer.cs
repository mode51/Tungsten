using System;
using System.Net;
using System.Threading.Tasks;
using W.AsExtensions;
using W.FromExtensions;

namespace W.Net
{
    public static partial class Udp
    {
        /// <summary>
        /// A Udp peer
        /// </summary>
        public class UdpPeer : IDisposable
        {
            private System.Net.Sockets.UdpClient _server;
            private W.Threading.ThreadMethod _thread;
            private Disposer _disposer = new Disposer();
            private bool _useCompression;

            /// <summary>
            /// Raised when bytes have been received from a client
            /// </summary>
            public event Action<IPEndPoint, byte[]> BytesReceived;

            /// <summary>
            /// Sends bytes to the specified remote client
            /// </summary>
            /// <param name="bytes">The data to send</param>
            /// <param name="remoteEndPoint">The remote client which is listening for data</param>
            /// <returns></returns>
            public async Task SendAsync(byte[] bytes, IPEndPoint remoteEndPoint)
            {
                await Task.Run(async () =>
                {
                    if (_useCompression)
                        bytes = bytes.AsCompressed();
                    await _server.SendAsync(bytes, bytes.Length, remoteEndPoint);
                });
            }
            /// <summary>
            /// Disposes the UdpServer and release resources
            /// </summary>
            public void Dispose()
            {
                _disposer.Cleanup(() =>
                {
#if NET45
                    _server.Close();
#elif NETSTANDARD1_3
                    _server.Dispose(); //will raise an exception in the thread method, thus terminating the thread
#endif
                    _thread.Dispose();
                });
            }
            /// <summary>
            /// Constructs a new UdpServer
            /// </summary>
            /// <param name="localEndPoint">The local IPEndPoint on which to listen for data</param>
            /// <param name="useCompression">If True, data is compressed before sending and decompressed when received</param>
            public UdpPeer(IPEndPoint localEndPoint, bool useCompression)
            {
                _useCompression = useCompression;
                _thread = new W.Threading.ThreadMethod(args =>
                {
                    try
                    {
                        while (true)
                        {
                            var result = _server.ReceiveAsync().Result;
                            if (result != null && result.Buffer?.Length > 0)
                            {
                                var bytes = result.Buffer;
                                if (_useCompression)
                                    bytes = bytes.FromCompressed();
                                BytesReceived?.Invoke(result.RemoteEndPoint, bytes);
                            }
                        }
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                    }
                    catch (ObjectDisposedException) //disconnected
                    {
                    }
                });
                _server = new System.Net.Sockets.UdpClient(localEndPoint);
                _thread.Start();
            }
        }
    }
}
