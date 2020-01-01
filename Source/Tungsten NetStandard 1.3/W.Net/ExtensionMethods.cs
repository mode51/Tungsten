using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Net
{
    /// <summary>
    /// Extension methods for W.Net
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Determines the broadcast address from an ip address and subnet mask
        /// </summary>
        /// <param name="address">The IP address</param>
        /// <param name="subnetMask">The subnet mask</param>
        /// <returns>The broadcast IP address associated with the given IP address and subnet mask</returns>
        /// <remarks>Taken from: <see href="https://blogs.msdn.microsoft.com/knom/2008/12/31/ip-address-calculations-with-c-subnetmasks-networks/"/></remarks>
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        /// <summary>
        /// Sends a request to the server and waits for a response.  Can be used along with, or in lieu of, the regular method of calling Write and handling the BytesReceived event.
        /// </summary>
        /// <param name="client">The W.Net.Tcp.TcpClient instance</param>
        /// <param name="request">The request to be made to the server</param>
        /// <param name="msTimeout">The time to wait for a response</param>
        /// <returns>Null if a timeout occurs, otherwise the server's response</returns>
        public static async Task<byte[]> RequestAsync(this W.Net.Tcp.TcpClient client, byte[] request, int msTimeout)
        {
            return await Task.Run(() =>
            {
                byte[] response = null;
                void BytesReceivedHandler(W.Net.Tcp.IClient c, byte[] b)
                {
                    response = b;
                }
                using (var mre = new ManualResetEventSlim(false))
                {
                    try
                    {
                        client.BytesReceived += BytesReceivedHandler;
                        client.Write(request);
                        mre.Wait(msTimeout);
                    }
                    finally
                    {
                        client.BytesReceived -= BytesReceivedHandler;
                    }
                }
                return response;
            });
        }
        /// <summary>
        /// Sends a request to the server and waits for a response.  Can be used along with, or in lieu of, the regular method of calling Write and handling the BytesReceived event.
        /// </summary>
        /// <typeparam name="TMessage">The message Type</typeparam>
        /// <param name="client">The W.Net.Tcp.Generic.TcpClient&lt;TMessage&gt; instance</param>
        /// <param name="request">The request to be made to the server</param>
        /// <param name="msTimeout">The time to wait for a response</param>
        /// <returns>default(TMessage) if a timeout occurs, otherwise the server's response</returns>
        public static async Task<TMessage> RequestAsync<TMessage>(this W.Net.Tcp.Generic.TcpClient<TMessage> client, TMessage request, int msTimeout)
        {
            return await Task.Run(() =>
            {
                TMessage response = default(TMessage);
                void MessageReceivedHandler(W.Net.Tcp.IClient c, TMessage b)
                {
                    response = b;
                }
                using (var mre = new ManualResetEventSlim(false))
                {
                    try
                    {
                        client.MessageReceived += MessageReceivedHandler;
                        client.Write(request);
                        mre.Wait(msTimeout);
                    }
                    finally
                    {
                        client.MessageReceived -= MessageReceivedHandler;
                    }
                }
                return response;
            });
        }
        /// <summary>
        /// Sends a request to the server and waits for a response.  Can be used along with, or in lieu of, the regular method of calling Write and handling the BytesReceived event.
        /// </summary>
        /// <param name="client">The W.Net.Tcp.SecureTcpClient instance</param>
        /// <param name="request">The request to be made to the server</param>
        /// <param name="msTimeout">The time to wait for a response</param>
        /// <returns>Null if a timeout occurs, otherwise the server's response</returns>
        public static async Task<byte[]> RequestAsync(this W.Net.Tcp.SecureTcpClient client, byte[] request, int msTimeout)
        {
            return await Task.Run(() =>
            {
                byte[] response = null;
                void BytesReceivedHandler(W.Net.Tcp.IClient c, byte[] b)
                {
                    response = b;
                }
                using (var mre = new ManualResetEventSlim(false))
                {
                    try
                    {
                        client.BytesReceived += BytesReceivedHandler;
                        client.Write(request);
                        mre.Wait(msTimeout);
                    }
                    finally
                    {
                        client.BytesReceived -= BytesReceivedHandler;
                    }
                }
                return response;
            });
        }
        /// <summary>
        /// Sends a request to the server and waits for a response.  Can be used along with, or in lieu of, the regular method of calling Write and handling the BytesReceived event.
        /// </summary>
        /// <typeparam name="TMessage">The message Type</typeparam>
        /// <param name="client">The W.Net.Tcp.Generic.SecureTcpClient&lt;TMessage&gt; instance</param>
        /// <param name="request">The request to be made to the server</param>
        /// <param name="msTimeout">The time to wait for a response</param>
        /// <returns>default(TMessage) if a timeout occurs, otherwise the server's response</returns>
        public static async Task<TMessage> RequestAsync<TMessage>(this W.Net.Tcp.Generic.SecureTcpClient<TMessage> client, TMessage request, int msTimeout)
        {
            return await Task.Run(() =>
            {
                TMessage response = default(TMessage);
                void MessageReceivedHandler(W.Net.Tcp.IClient c, TMessage b)
                {
                    response = b;
                }
                using (var mre = new ManualResetEventSlim(false))
                {
                    try
                    {
                        client.MessageReceived += MessageReceivedHandler;
                        client.Write(request);
                        mre.Wait(msTimeout);
                    }
                    finally
                    {
                        client.MessageReceived -= MessageReceivedHandler;
                    }
                }
                return response;
            });
        }

        /// <summary>
        /// Sends a request to a Udp peer and waits for a response.  Can be used along with, or in lieu of, the regular method of calling Write and handling the BytesReceived event.
        /// </summary>
        /// <param name="peer">The W.Net.Udp.UdpPeer instance</param>
        /// <param name="request">The request to be made to the server</param>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote Udp peer</param>
        /// <param name="msTimeout">The time to wait for a response</param>
        /// <returns>Null if a timeout occurs, otherwise the server's response</returns>
        public static async Task<byte[]> RequestAsync(this W.Net.Udp.UdpPeer peer, byte[] request, IPEndPoint remoteEndPoint, int msTimeout)
        {
            return await Task.Run(async () =>
            {
                byte[] response = null;
                void BytesReceivedHandler(IPEndPoint ep, byte[] b)
                {
                    response = b;
                }
                using (var mre = new ManualResetEventSlim(false))
                {
                    try
                    {
                        peer.BytesReceived += BytesReceivedHandler;
                        await peer.SendAsync(request, remoteEndPoint);
                        mre.Wait(msTimeout);
                    }
                    finally
                    {
                        peer.BytesReceived -= BytesReceivedHandler;
                    }
                }
                return response;
            });
        }
        /// <summary>
        /// Sends a request to a generic Udp peer and waits for a response.  Can be used along with, or in lieu of, the regular method of calling Write and handling the BytesReceived event.
        /// </summary>
        /// <param name="peer">The W.Net.Udp.Generic.UdpPeer&lt;TMessage&gt; instance</param>
        /// <param name="request">The request to be made to the server</param>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote Udp peer</param>
        /// <param name="msTimeout">The time to wait for a response</param>
        /// <returns>default(TMessage) if a timeout occurs, otherwise the server's response</returns>
        public static async Task<TMessage> RequestAsync<TMessage>(this W.Net.Udp.Generic.UdpPeer<TMessage> peer, TMessage request, IPEndPoint remoteEndPoint, int msTimeout)
        {
            return await Task.Run(async () =>
            {
                TMessage response = default(TMessage);
                void MessageReceivedHandler(IPEndPoint ep, TMessage m)
                {
                    response = m;
                }
                using (var mre = new ManualResetEventSlim(false))
                {
                    try
                    {
                        peer.MessageReceived += MessageReceivedHandler;
                        await peer.SendAsync(request, remoteEndPoint);
                        mre.Wait(msTimeout);
                    }
                    finally
                    {
                        peer.MessageReceived -= MessageReceivedHandler;
                    }
                }
                return response;
            });
        }
    }
}
