using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace W.Net.Alpha
{
    /// <summary>
    /// Methods to broadcast a magic packet to wake up a machine with the given MAC address
    /// </summary>
    public class WakeOnLan
    {
        /// <summary>
        /// Broadcasts a magic packet to wake up the machine with the given MAC address
        /// </summary>
        /// <param name="macAddress">The MAC address of the machine to wake up</param>
        /// <param name="broadcastAddress">The port on which to send the magic packet (typically 0, 7 or 9)</param>
        /// <returns>True if the magic packet was successfully sent, otherwise False</returns>
        /// <remarks>Adapted from: <see href="https://www.codeproject.com/Articles/5315/Wake-On-Lan-sample-for-C"/></remarks>
        public static bool WakeUp_via_Socket(string macAddress, string broadcastAddress = "192.168.1.255", int port = 9)
        {
            macAddress = macAddress.Replace(":", "");
            if (string.IsNullOrEmpty(macAddress))
                throw new ArgumentOutOfRangeException(nameof(macAddress), "\"" + macAddress + "\" is not a valid MAC address");
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Connect(IPAddress.Parse(broadcastAddress), port);
            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);

            int counter = 0;
            var magicPacket = new byte[102];

            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                magicPacket[counter++] = 0xFF;
            //now repeat MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    magicPacket[counter++] = byte.Parse(macAddress.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            var result = client.Send(magicPacket);
            return result == magicPacket.Length;
        }
        /// <summary>
        /// Broadcasts a magic packet to wake up the machine with the given MaC address
        /// </summary>
        /// <param name="macAddress">The MAC address of the machine to wake up</param>
        /// <param name="ipBroadcastAddress">The broadcast address.  This should be determined by the machine's IP address and the desired network mask.</param>
        /// <param name="port">The socket port</param>
        /// <returns>True if the magic packet was successfully sent, otherwise False</returns>
        /// <remarks>Adapted from: <see href="https://www.codeproject.com/Articles/5315/Wake-On-Lan-sample-for-C"/></remarks>
        public static bool WakeUp(string macAddress, string ipBroadcastAddress = "192.168.1.255", int port = 9)
        {
            macAddress = macAddress.Replace(":", "");
            macAddress = macAddress.Replace("-", "");
            if (string.IsNullOrEmpty(macAddress))
                throw new ArgumentOutOfRangeException(nameof(macAddress), "\"" + macAddress + "\" is not a valid MAC address");
            var client = new UdpClient();

            int counter = 0;
            var magicPacket = new byte[102];
            for (int i = 0; i <= 5; i++)
                magicPacket[i] = 0xff;

            //now repeat MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    magicPacket[counter++] = byte.Parse(macAddress.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            var result = client.SendAsync(magicPacket, magicPacket.Length, ipBroadcastAddress, port).Result;
            return result == magicPacket.Length;
        }
        /// <summary>
        /// Broadcasts a magic packet to wake up the machine with the given MAC address
        /// </summary>
        /// <param name="macAddress">The MAC address of the machine to wake up</param>
        /// <param name="ipAddress">The IP address of the machine to wake</param>
        /// <param name="subnetMask">The subnet mask to determine the broadcast IP address</param>
        /// <returns>True if the magic packet was successfully sent, otherwise False</returns>
        /// <remarks>Taken from: <see href="https://blogs.msdn.microsoft.com/knom/2008/12/31/wake-on-lan-client-with-c/"/></remarks>
        public static bool WakeUp(string macAddress, string ipAddress, string subnetMask)
        {
            UdpClient client = new UdpClient();

            Byte[] datagram = new byte[102];

            for (int i = 0; i <= 5; i++)
            {
                datagram[i] = 0xff;
            }

            string[] macDigits = null;
            if (macAddress.Contains("-"))
            {
                macDigits = macAddress.Split('-');
            }
            else
            {
                macDigits = macAddress.Split(':');
            }

            if (macDigits.Length != 6)
            {
                throw new ArgumentException("Incorrect MAC address supplied!");
            }

            int start = 6;
            for (int i = 0; i < 16; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                }
            }

            IPAddress address = IPAddress.Parse(ipAddress);
            IPAddress mask = IPAddress.Parse(subnetMask);
            IPAddress broadcastAddress = address.GetBroadcastAddress(mask);

            var result = client.SendAsync(datagram, datagram.Length, broadcastAddress.ToString(), 3).Result;
            return result == datagram.Length;
        }
    }
}
