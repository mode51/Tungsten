using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using W.AsExtensions;

namespace W.Demo
{
    public class TestClientConcurrency
    {
        internal class ClientConnection
        {
            //private object _lock = new object();
            private W.Threading.Thread _thread;
            public W.Net.Client Client { get; private set; }
            public ManualResetEventSlim Mre { get; set; } = new ManualResetEventSlim();
            public LockableSlim<string> Request { get; } = new LockableSlim<string>();
            public LockableSlim<int> Requests { get; } = new LockableSlim<int>();
            public LockableSlim<int> Responses { get; } = new LockableSlim<int>();
            public LockableSlim<Exception> Exception { get; } = new LockableSlim<System.Exception>();

            public string Status
            {
                get
                {
                    string result;
                    result = string.Format("{0}: {1}/{2} - {3}{4}", Client?.Name, Requests.Value, Responses.Value, Client.IsConnected ? "Connected" : "Disconnected", Exception.Value != null ? ": " + Exception.Value : "");
                    //if (Client.IsConnected)
                    //    result = string.Format("Client({0}): {1}/{2} - Connected", Client?.Name, Requests.Value, Responses.Value);
                    //else
                    //    result = string.Format("Client({0}): {1}/{2} - Disconnected: {3}", Client?.Name, Requests.Value, Responses.Value, Exception.Value.Message);
                    return result;
                }
            }
            public void Start(IPEndPoint remoteEndPoint)
            {
                if (Client != null)
                    return;
                Client = new W.Net.Client();
                Client.Disconnected += (c, ep, e) =>
                {
                    Exception.Value = e;
                };
                Client.DataReceived += (c, bytes) =>
                {
                    if (bytes == null)
                        System.Diagnostics.Debugger.Break();

                    var message = bytes.AsString();
                    if (message == Request.Value)// Request.Value.ToUpper())
                        Responses.Value += 1;
                    else
                        Exception.Value = new System.Exception("Message Lost");
                    Mre.Set();
                };
                if (Client.Connect(remoteEndPoint))//.ContinueWith(task =>
                {
                    Mre.Reset();
                    _thread = new W.Threading.Thread(ct =>
                    {
                        try
                        {
                            while (!ct.IsCancellationRequested)
                            {
                                if (Client.IsConnected)
                                {
                                    Request.Value = string.Format("Message: {0}", Requests);
                                    var bytes = Request.Value.AsBytes();
                                    Requests.Value += 1;
                                    Mre.Reset();
                                    Client.Send(bytes);
                                    Mre.Wait();
                                }
                                //W.Threading.Thread.Sleep(1);
                            }
                        }
#if NET45
                        catch (System.Threading.ThreadAbortException)
                        {
                            System.Threading.Thread.ResetAbort();
                        }
#endif
                        catch (OperationCanceledException)
                        {
                            //ignore...this thread was canceled
                        }
                        catch (Exception)
                        {

                        }
                        if (Client?.IsConnected ?? false)
                            Client?.Disconnect();
                    });
                    _thread.Start();
                }//);
            }
            public void Stop()
            {
                if (_thread != null)
                {
                    Mre.Set();
                    _thread?.Stop();
                    _thread = null;
                }
                Client?.Disconnect();
                Client = null;
            }

            ~ClientConnection()
            {
                Stop();
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
            using (var server = new W.Net.EchoServer<W.Net.Client>())
            {
                //server.ClientConnected += (sc) =>
                //{
                //    sc.DataReceived += (c, bytes) =>
                //    {
                //        var echo = bytes.AsString().ToUpper().AsBytes();
                //        c.As<W.Net.Client>().Send(echo);
                //    };
                //};
                //W.Threading.Thread.Sleep(10);
                server.Start(localEndPoint);

                var client = new W.Net.Client();
                client.DataReceived += (c, bytes) =>
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
                    client.Connect(localEndPoint);
                    //client.ConnectAsync(localEndPoint).Wait();
                    if (client.IsConnected)
                    {
                        connectionCount += 1;
                        //send message
                        request = string.Format("Message: {0}", t);
                        var bytes = request.AsBytes();
                        mre.Reset();
                        client.Send(bytes);
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
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var maxClients = 10;
            var clients = new ClientConnection[maxClients];

            using (var server = new W.Net.Server<W.Net.Client>(20))
            {
                server.ClientConnected += (sc) =>
                {
                    //sc.UseCompression = useCompression;
                    sc.DataReceived += (c, bytes) =>
                    {
                        //var echo = bytes.AsString().ToUpper().AsBytes();
                        c.As<W.Net.Client>().Send(bytes);
                    };
                };
                //W.Threading.Thread.Sleep(10);
                server.Start(localEndPoint);

                //allocate and configure the clients
                for (int t = 0; t < maxClients; t++)
                {
                    clients[t] = new ClientConnection();
                    clients[t].Start(localEndPoint);
                }

                //display status and wait for exit
                Console.CursorVisible = false;
                while (!Console.KeyAvailable)
                {
                    Console.SetCursorPosition(0, 0);

                    for (int t = 0; t < maxClients; t++)
                    {
                        clients[t].Status.WriteFullConsoleLine();
                    }
                    string.Format("Press Any Key To Return").WriteFullConsoleLine();
                    W.Threading.Thread.Sleep(1);
                }
                Console.ReadKey(true);
                Console.CursorVisible = true;

                //cleanup
                for (int t = 0; t < maxClients; t++)
                {
                    clients[t].Stop();
                }
            }
        }
    }
}
