using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using W;

namespace W.Tests.Tungsten.IO.Pipes
{
    [TestFixture]
    public class TestPipes
    {
        private const string PIPENAME = "test-pipe";
        [Test]
        public void CreatePipeServer()
        {
            var server = new W.IO.Pipes.PipeServer<W.IO.Pipes.PipeClient>(PIPENAME);
            server.Start();
            server.Started += s => { s.Stop(); };
            server.Dispose();
        }
        [Test]
        public void CreatePipeClient()
        {
            var client = new W.IO.Pipes.PipeClient();
            client.Dispose();
        }
        [Test]
        public void PipeTransmit()
        {
            var mreMessageReceived = new System.Threading.ManualResetEvent(false);
            var mreServerReady = new System.Threading.ManualResetEvent(false);
            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            using (var server = new W.IO.Pipes.PipeServer<W.IO.Pipes.PipeClient>(PIPENAME))
            {
                server.ClientConnected += c =>
                {
                    c.As<W.IO.Pipes.PipeClient>().MessageReceived += (o, message) =>
                    {
                        Assert.IsTrue(o == c);
                        Assert.IsTrue(message.AsString() == "Test");
                        Console.WriteLine("{0} Server Received Message", sw.Elapsed);
                        mreMessageReceived.Set();
                    };
                };
                server.Started += s =>
                {
                    mreServerReady.Set();
                };
                server.Start();
                Assert.IsTrue(mreServerReady.WaitOne(1000));

                using (var client = new W.IO.Pipes.PipeClient())
                {
                    Assert.IsTrue(client.Connect(PIPENAME));
                    client.Write("Test".AsBytes());
                    Assert.IsTrue(mreMessageReceived.WaitOne(5000));
                    Console.WriteLine("{0} Client Disposing", sw.Elapsed);
                }
                Console.WriteLine("{0} Client Disposed", sw.Elapsed);
            }
            Console.WriteLine("{0} Server Disposed", sw.Elapsed);
            sw.Stop();
        }

        [Test]
        public void TestNamedPipeLoggingWithHost()
        {
            var mreQuit = new System.Threading.ManualResetEvent(false);
            int count = 0;

            W.IO.Pipes.PipeClient.AddNamedPipeLogger("ConsoleLogger");
            using (var server = new W.IO.Pipes.PipeServer<W.IO.Pipes.PipeClient>("ConsoleLogger"))
            {
                server.ClientConnected += (r) =>
                {
                    r.MessageReceived += (o, m) =>
                    {
                        count += 1;
                        Console.WriteLine(m.AsString());
                        if (count == 10)
                            mreQuit.Set();
                        //o.As<W.IO.Pipes.PipeClient>().Write("".AsBytes());
                    };
                };
                server.Started += s =>
                {
                    Console.WriteLine("ConsoleLogger Started");

                    for (int t = 0; t < 10; t++)
                        W.Logging.Log.i("Test Log Message {0}", t);
                    //mreQuit.Set();
                };
                server.Start();

                Assert.IsTrue(mreQuit.WaitOne(10000));
                Console.WriteLine("ConsoleLogger Complete");
            }
            //optionally, you can manually Stop the logging Pipe
            W.IO.Pipes.PipeClient.RemoveNamedPipeLogger();
        }

        [Test]
        public void TestNamedPipeLogging()
        {
            W.IO.Pipes.PipeClient.AddNamedPipeLogger("ConsoleLogger");

            for (int t = 0; t < 10; t++)
                W.Logging.Log.i("Test Log Message {0}", t);

            Console.WriteLine("ConsoleLogger Complete");

            //optionally, you can manually Stop the logging Pipe
            W.IO.Pipes.PipeClient.RemoveNamedPipeLogger();
        }

    }
}
