using System;
using System.Net;
using System.Threading;
using W.AsExtensions;
using W.FromExtensions;

namespace W.Demo
{
    public class TestAlphaClientServer
    {
        internal class ClientConnection
        {
            public W.Net.Alpha.Client Client { get; private set; }
            public LockableSlim<string> Request { get; } = new LockableSlim<string>();
            public LockableSlim<int> Requests { get; } = new LockableSlim<int>();
            public LockableSlim<int> Responses { get; } = new LockableSlim<int>();
            public LockableSlim<Exception> Exception { get; } = new LockableSlim<System.Exception>();
            private LockableSlim<bool> _stop = new LockableSlim<bool>();

            private void SendNextMessage(W.Net.Alpha.Client client)
            {
                Requests.Value += 1;
                Request.Value = string.Format("Message: {0}", Requests);
                client.Write(Request.Value.AsBytes());
            }
            public string Status
            {
                get
                {
                    string result;
                    result = string.Format("{0}: {1}/{2} - {3}{4}", Client.Socket.LocalEndPoint, Requests.Value, Responses.Value, Client.Socket.Connected ? "Connected" : "Disconnected", Exception.Value != null ? ": " + Exception.Value : "");
                    return result;
                }
            }
            public void Start(IPEndPoint remoteEndPoint)
            {
                if (Client != null)
                    return;
                Client = new W.Net.Alpha.Client();
                Client.BytesReceived += (c, bytes) =>
                {
                    if (bytes == null)
                        System.Diagnostics.Debugger.Break();

                    var message = bytes.AsString();
                    if (message == Request.Value)// Request.Value.ToUpper())
                        Responses.Value += 1;
                    else
                        Exception.Value = new System.Exception("Message Lost");
#if NETCOREAPP1_1
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
#else
                    W.Threading.Thread.Sleep(1);
#endif
                    if (!_stop.Value) //automatically send a new message
                        SendNextMessage(c);
                };
                if (Client.Connect(remoteEndPoint))//.ContinueWith(task =>
                {
                    SendNextMessage(Client); //start the send/receive cycle
                }
            }
            public void Stop()
            {
                _stop.Value = true;
                //if (_thread != null)
                //{
                //    Mre.Set();
                //    _thread?.Stop();
                //    _thread = null;
                //}
                Client?.Disconnect();
                Client = null;
            }

            ~ClientConnection()
            {
                if (!_stop.Value)
                    Stop();
                //_stop.Dispose();
            }
        }
        public class Customer
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime Created { get; set; }
            public bool IsDeleted { get; set; }
        }
        public static void Run()
        {
            var mre = new ManualResetEvent(false);

            using (var server = new W.Net.Alpha.Host<Customer>())
            {
                server.BytesReceived += (h, s, bytes) =>
                {
                    //echo
                    s.Write(bytes);
                };
                server.Start("127.0.0.1", 5150);

                using (var client = new W.Net.Alpha.Client<Customer>())
                {
                    client.MessageReceived += (o, customer) =>
                    {
                        Console.WriteLine("Server Echoed: {0}, Age = {1}", customer.Name, customer.Age);
                        Console.WriteLine("Send <Return To Exit>: ");
                        mre.Set();
                    };
                    if (client.Connect("127.0.0.1", 5150))
                    {

                        Console.WriteLine("Send <Return To Exit>: ");
                        while (client.IsConnected)
                        {
                            var name = Console.ReadLine();
                            if (string.IsNullOrEmpty(name))
                                break;
                            mre.Reset();
                            var customer = new Customer() { Name = name, Age = new Random().Next(1, 75) };
                            Console.WriteLine("Sending: {0}, Age = {1}", customer.Name, customer.Age);
                            client.Write(customer);
                            mre.WaitOne(5000);
                        }
                    }
                }
            }
        }

        public static void Run_Sequentially()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var request = string.Empty;
            var mre = new ManualResetEventSlim(false);
            int t = 0;
            int connectionCount = 0;
            int failedConnectionCount = 0;
            int sentCount = 0;
            int timeoutCount = 0;
            Console.CursorVisible = false;
            using (var host = new W.Net.Alpha.Host())
            {
                host.BytesReceived += (h, s, bytes) =>
                {
                    //echo
                    s.Write(bytes);
                };
                host.Start(localEndPoint);

                var client = new W.Net.Alpha.Client();
                client.BytesReceived += (c, bytes) =>
                {
                    try
                    {
                        "client.MessageReady".WriteFullConsoleLine();
                        if (bytes == null)
                            System.Diagnostics.Debugger.Break();

                        var message = bytes.AsString();
                        string.Format("Received {0}", message).WriteFullConsoleLine();

                        if (message != request)
                            System.Diagnostics.Debugger.Break();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debugger.Break();
                        e.Message.WriteFullConsoleLine();
                    }
                    finally
                    {
                        mre.Set();
                    }
                };
                var sw = System.Diagnostics.Stopwatch.StartNew();
                while (!Console.KeyAvailable)
                {
                    Console.SetCursorPosition(0, 0);
                    sw.Restart();

                    //connect
                    //client.Connect(localEndPoint);
                    if (client.Connect(localEndPoint))
                    {
                        connectionCount += 1;
                        //send message
                        request = string.Format("Message: {0}", t);
                        var bytes = request.AsBytes();
                        mre.Reset();
                        client.Write(bytes);
                        sentCount += 1;

                        //wait for response
                        if (!mre.Wait(4000)) 
                        {
                            timeoutCount += 1;
                            "Server timed out".WriteFullConsoleLine();
                        }

                        //disconnect
                        client.Disconnect();
                        string.Format("Elapsed Connection Time (ms): " + sw.ElapsedMilliseconds).WriteFullConsoleLine();
                    }
                    else
                    {
                        failedConnectionCount += 1;
                    }
                    string.Format("Total Elapsed Time (ms): " + sw.ElapsedMilliseconds).WriteFullConsoleLine();
                    string.Format("Connections={0}, Failed Connections={1}, Sent={2}, Timeouts={3}", connectionCount, failedConnectionCount, sentCount, timeoutCount).WriteFullConsoleLine();

                    Console.WriteLine("Press Any Key To Return");
                    t += 1;
                }
                client.Dispose();
            }

            //Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
            Console.CursorVisible = true;
        }

        public static void Run_Concurrently()
        {
            Console.CursorVisible = false;
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var maxClients = 10;
            var clients = new ClientConnection[maxClients];

            using (var host = new W.Net.Alpha.EchoHost())
            {
                host.Start(localEndPoint, maxClients);

                //allocate and configure the clients
                System.Threading.Tasks.Parallel.For(0, maxClients, t =>
                {
                    clients[t] = new ClientConnection();
                    clients[t].Start(localEndPoint);
                });

                //display status and wait for exit
                while (!Console.KeyAvailable)
                {

                    //System.Threading.Tasks.Parallel.For(0, maxClients, t =>
                    //{
                    //    clients[t].Status.WriteFullConsoleLine(t);
                    //});
                    for (int t = 0; t < maxClients; t++)
                    {
                        clients[t].Status.WriteFullConsoleLine(t);
                    }
                    string.Format("Press Any Key To Return").WriteFullConsoleLine(maxClients + 1);
#if NETCOREAPP1_1
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
#else
                    W.Threading.Thread.Sleep(1);
#endif
                }
                Console.ReadKey(true);

                //cleanup

                System.Threading.Tasks.Parallel.For(0, maxClients, t =>
                {
                    clients[t].Stop();
                });
            }
            Console.CursorVisible = true;
        }
    }
}