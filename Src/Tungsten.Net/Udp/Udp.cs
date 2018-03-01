using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace W.Net
{
    /// <summary>
    /// Provides simple UDP reading and writing
    /// </summary>
    public static partial class Udp
    {
        /// <summary>
        /// Sends data to a remote via UDP
        /// </summary>
        /// <param name="ipEndPoint">The remote machine's endpoint</param>
        /// <param name="format">The string format</param>
        /// <param name="args">String formatting arguments</param>
        /// <returns>The Task associated with this action</returns>
        public static async Task SendAsync(IPEndPoint ipEndPoint, string format, params object[] args)
        {
            byte[] data;
            if (args != null && args.Length > 0)
                data = string.Format(format, args).AsBytes();
            else
                data = format.AsBytes();
            await SendAsync(ipEndPoint, data);
        }
        /// <summary>
        /// Sends data to a remote via UDP
        /// </summary>
        /// <param name="ipEndPoint">The remote machine's endpoint</param>
        /// <param name="bytes">The data to send</param>
        /// <returns>The Task associated with this action</returns>
        public static async Task SendAsync(IPEndPoint ipEndPoint, byte[] bytes)
        {
            using (var client = new System.Net.Sockets.UdpClient())
            {
                await client.SendAsync(bytes, bytes.Length, ipEndPoint);
            }
        }
        /// <summary>
        /// Sends a message to a remote via UDP
        /// </summary>
        /// <param name="ipEndPoint">The remote machine's endpoint</param>
        /// <param name="message">The message to send</param>
        /// <returns>The Task associated with this action</returns>
        public static async Task SendAsync<TType>(IPEndPoint ipEndPoint, TType message)
        {
            await SendAsync(ipEndPoint, SerializationMethods.Serialize(message).AsBytes());
        }
    }
}
