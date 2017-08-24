using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using W.Logging;

namespace W.Demo
{
    class TestSecureClient2
    {
        public static void Run()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);

            var server = new W.Net.SecureEchoServer<W.Net.SecureClient2>();
            server.Start(localEndPoint);

            for (int t = 0; t < 5; t++)
            {
                var mre = new ManualResetEventSlim(false);
                var client = new W.Net.SecureClient2();
                client.DataReceived += (c, data) =>
                {
                    var response = data.AsString();
                    Console.WriteLine("Received {0}", response);
                    mre.Set();
                };
                bool connected = client.Socket.ConnectAsync(localEndPoint.Address, localEndPoint.Port).Result;
                if (connected)
                {
                    var request = string.Format("Message: {0}", t);
                    client.Send(request.AsBytes());
                    mre.Wait();
                    //if (!mre.Wait(30000))
                    //    Console.WriteLine("Waiting for server timed out.");
                    client.Socket.Disconnect();
                }
                client.Dispose();
                mre.Dispose();
            }

            server.Stop();
            server.Dispose();
        }
    }
}