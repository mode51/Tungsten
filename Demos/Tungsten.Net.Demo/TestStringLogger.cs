using System;
using System.Net;
using System.Threading;

namespace W.Demo
{
    public class TestStringLogger
    {
        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("1.  Test Self Hosted");
                Console.WriteLine("2.  Test With External Host");
                Console.WriteLine("Press <Escape> To Exit");
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        SelfHost();
                        break;
                    case ConsoleKey.D2:
                        ExternalHost();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
                Console.Clear();
            }
        }
        private static void SelfHost()
        {
            ManualResetEventSlim mre = new ManualResetEventSlim(true);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2212);
            using (var server = new W.Net.Server<W.Net.Client<string>>())
            {
                server.ClientConnected += client =>
                {
                    client.As<W.Net.Client<string>>().MessageReceived += (o, message) =>
                    {
                        Console.WriteLine(message);
                        mre.Set();
                    };
                };
                server.Start(ipEndPoint.Address, ipEndPoint.Port);

                //To verify this method, an external server must be listening
                using (var logger = new W.Net.StringClientLogger(ipEndPoint))
                {
                    var r = new Random();
                    while (true)
                    {
                        mre.Wait();
                        Console.Write("Send <Return to Exit>:");
                        var msg = Console.ReadLine().Trim();
                        if (string.IsNullOrEmpty(msg))
                            break;
                        mre.Reset();

                        switch (r.Next(0, 4))
                        {
                            case 0:
                                W.Logging.Log.e(msg);
                                break;
                            case 1:
                                W.Logging.Log.w(msg);
                                break;
                            case 2:
                                W.Logging.Log.i(msg);
                                break;
                            case 3:
                                W.Logging.Log.v(msg);
                                break;
                        }
                    }
                    Console.WriteLine("Complete");
                }
            }
        }
        private static void ExternalHost()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.12"), 2112);
            //To verify this method, an external server must be listening
            using (var logger = new W.Net.StringClientLogger(ipEndPoint))
            {
                var r = new Random();
                while (true)
                {
                    Console.Write("Send <Return to Exit>:");
                    var msg = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(msg))
                        break;

                    switch (r.Next(0, 4))
                    {
                        case 0:
                            W.Logging.Log.e(msg);
                            break;
                        case 1:
                            W.Logging.Log.w(msg);
                            break;
                        case 2:
                            W.Logging.Log.i(msg);
                            break;
                        case 3:
                            W.Logging.Log.v(msg);
                            break;
                    }
                }
                Console.WriteLine("Complete");
            }
        }
    }
}