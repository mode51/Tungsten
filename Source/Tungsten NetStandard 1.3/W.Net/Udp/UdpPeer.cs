using System;
using System.Net;
using System.Threading.Tasks;
using W.Threading;
using W.Threading.Lockers;

namespace W.Net
{
    public static partial class Udp
    {
        /// <summary>
        /// A Udp peer
        /// </summary>
        public class UdpPeer : IDisposable
        {
            private System.Net.Sockets.UdpClient _client;
            private ThreadMethod _thread;
            private Disposer _disposer = new Disposer();
            private bool _useCompression;

            private void InitializeThread()
            {
                _thread = new ThreadMethod(args =>
                {
                    try
                    {
                        while (true)
                        {
                            var result = _client.ReceiveAsync().Result;
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
            }

            /// <summary>
            /// Raised when bytes have been received from a client
            /// </summary>
            public event Action<IPEndPoint, byte[]> BytesReceived;

            /// <summary>
            /// Returns the internal UdpClient
            /// </summary>
            public System.Net.Sockets.UdpClient Client => _client;

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
                    await _client.SendAsync(bytes, bytes.Length, remoteEndPoint);
                });
            }
            /// <summary>
            /// Disposes the UdpServer and release resources
            /// </summary>
            public void Dispose()
            {
                _disposer.Dispose(() =>
                {
#if NET45 || NETSTANDARD2_0
                    _client.Close();
#elif NETSTANDARD1_3
                    _client.Dispose(); //will raise an exception in the thread method, thus terminating the thread
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
                InitializeThread();
                _client = new System.Net.Sockets.UdpClient(localEndPoint);
                _thread.Start();
            }
            /// <summary>
            /// Constructs a new UdpServer
            /// </summary>
            /// <param name="port">The port on which to listen for data</param>
            /// <param name="useCompression">If True, data is compressed before sending and decompressed when received</param>
            public UdpPeer(int port, bool useCompression)
            {
                _useCompression = useCompression;
                InitializeThread();
                _client = new System.Net.Sockets.UdpClient(port);
                _thread.Start();
            }
            /// <summary>
            /// Constructs a new UdpServer
            /// </summary>
            /// <param name="udpClient">An existing UdpClient on which to listen for data</param>
            /// <param name="useCompression">If True, data is compressed before sending and decompressed when received</param>
            public UdpPeer(System.Net.Sockets.UdpClient udpClient, bool useCompression)
            {
                _useCompression = useCompression;
                InitializeThread();
                _client = udpClient;
                _thread.Start();
            }
        }
    }
}
