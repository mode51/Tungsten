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
                    Assert.IsTrue(mreMessageReceived.WaitOne(10000));
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
            Console.WriteLine("If this test fails, try it again.  It just might work.");
            var mreQuit = new System.Threading.ManualResetEvent(false);
            int count = 0;
            using (var server = new W.IO.Pipes.PipeServer<W.IO.Pipes.PipeClient>(PIPENAME))
            {
                server.ClientConnected += (r) =>
                {
                    Console.WriteLine("Server accepted client");
                    r.MessageReceived += (o, m) =>
                    {
                        count += 1;
                        Console.WriteLine(m.AsString());
                        if (count == 10)
                            mreQuit.Set();
                    };
                };
                server.Started += s =>
                {
                    Console.WriteLine("Started");

                    using (var client = new W.IO.Pipes.PipeClient())
                    {
                        if (client.Connect(PIPENAME, System.IO.Pipes.PipeDirection.InOut))
                        {
                            for (int t = 0; t < 10; t++)
                                client.Write(string.Format("Test Message {0}", t).AsBytes());
                            Assert.IsTrue(mreQuit.WaitOne(20000));
                        }
                        else
                            Console.WriteLine("Failed to connect to the server");
                    }
                };
                server.Start();

                Assert.IsTrue(mreQuit.WaitOne(20000));
                Console.WriteLine("Complete");
            }
        }

        [Test]
        public void TestNamedPipeLogging()
        {
            //To verify, an external server must be running
            using (var pipeLogger = new W.IO.Pipes.PipeClientLogger("ConsoleLogger"))
            {
                for (int t = 0; t < 10; t++)
                    W.Logging.Log.i("Test Log Message {0}", t);

                Console.WriteLine("Complete");
            }
        }
    }
}
