using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace W.Net
{
    /// <summary>
    /// Network extension methods
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
    }
}
