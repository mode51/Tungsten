using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using W.Net;

namespace W.Tests
{
    [TestClass]
    public class UdpTests
    {
        [TestMethod]
        public void Peer_CreateAndDispose()
        {
            var ep = new IPEndPoint(IPAddress.Loopback, 2110);
            using (var peer = new Udp.UdpPeer(ep, true))
            {
            }
        }
        [TestMethod]
        public void Peer_Receive()
        {
            var epServer = new IPEndPoint(IPAddress.Loopback, 2110);
            var mreContinue = new ManualResetEventSlim(false);
            using (var server = new Udp.UdpPeer(epServer, false))
            {
                server.BytesReceived += (epRemote, bytes) =>
                {
                    Console.WriteLine($"Server received: {bytes.AsString()} from {epRemote.ToString()}");
                    mreContinue.Set();
                };
                Udp.SendAsync(epServer, "This is a test".AsBytes());
                Assert.IsTrue(mreContinue.Wait(3000));
            }
        }
        [TestMethod]
        public void Peer_SendReceive()
        {
            var epServer = new IPEndPoint(IPAddress.Loopback, 2110);
            var epClient = new IPEndPoint(IPAddress.Loopback, 2111);
            var mreContinue = new ManualResetEventSlim(false);
            using (var server = new Udp.UdpPeer(epServer, false))
            {
                server.BytesReceived += (epRemote, bytes) =>
                {
                    Console.WriteLine($"Server received: {bytes.AsString()} from {epRemote.ToString()}");
                    server.SendAsync(bytes, epRemote); //simple echo
                };
                using (var client = new Udp.UdpPeer(epClient, false))
                {
                    client.BytesReceived += (epRemote, bytes) =>
                    {
                        Console.WriteLine($"Client received: {bytes.AsString()} from {epRemote.ToString()}");
                        mreContinue.Set();
                    };
                    client.SendAsync("This is a round-trip test".AsBytes(), epServer);
                    Assert.IsTrue(mreContinue.Wait(3000));
                }
            }
        }
        [TestMethod]
        public void Peer_ReceiveMany()
        {
            var epServer = new IPEndPoint(IPAddress.Loopback, 2110);
            var mreContinue = new ManualResetEventSlim(false);
            using (var server = new Udp.UdpPeer(epServer, false))
            {
                server.BytesReceived += (epRemote, bytes) =>
                {
                    Console.WriteLine($"Server received: {bytes.AsString()} from {epRemote.ToString()}");
                    mreContinue.Set();
                };
                for (int t = 0; t < 50; t++)
                {
                    Udp.SendAsync(epServer, $"{t}. This is a test".AsBytes());
                    Assert.IsTrue(mreContinue.Wait(3000));
                    mreContinue.Reset();
                }
            }
        }
        [TestMethod]
        public void Peer_SendReceiveFromMany()
        {
            var epServer = new IPEndPoint(IPAddress.Loopback, 2110);
            var mreContinue = new ManualResetEventSlim(false);
            var clients = new List<Udp.UdpPeer>();

            using (var server = new Udp.UdpPeer(epServer, false))
            {
                server.BytesReceived += (epRemote, bytes) =>
                {
                    Console.WriteLine($"Server received: {bytes.AsString()} from {epRemote.ToString()}");
                    server.SendAsync(bytes, epRemote); //simple echo
                };

                //create 45 peers
                for (int t = 0; t < 45; t++)
                {
                    var client = new Udp.UdpPeer(new IPEndPoint(IPAddress.Loopback, 2111 + t), false);
                    client.BytesReceived += (ep, bytes) =>
                    {
                        Console.WriteLine($"Client received: {bytes.AsString()} from {ep.ToString()}");
                        mreContinue.Set();
                    };
                    clients.Add(client);
                }

                //send a message from each peer
                foreach (var client in clients)
                {
                    client.SendAsync("This is a test".AsBytes(), epServer);
                    Assert.IsTrue(mreContinue.Wait(3000));
                    mreContinue.Reset();
                }

                //dispose clients
                foreach (var client in clients)
                    client.Dispose();
            }
        }
    }
}