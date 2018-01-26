using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W;
using W.AsExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using W.Logging;
using W.IO.Pipes;

namespace W.Tests
{
    [TestClass]
    public class PipeClientLoggerTests
    {
        private ManualResetEventSlim _mreContinue = new ManualResetEventSlim(false);
        protected string PipeName = "PipeClientLogger_Tests_" + new Random().Next(int.MaxValue).ToString();// Guid.NewGuid().ToString();
        private W.IO.Pipes.PipeHost Host { get; set; }
        private W.IO.Pipes.PipeLogger Client { get; set; }

        private void LogMessages(int count)
        {
            _mreContinue.Reset();
            var r = new Random();
            for (int t = 1; t <= count; t++)
            {
                var msg = "Test Log Message: " + t.ToString();
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
                _mreContinue.Wait();
            }
        }

        [TestInitialize]
        public virtual void Initialize()
        {
            Console.WriteLine("PipeName = {0}", PipeName);
            Host = new W.IO.Pipes.PipeHost();
            Host.BytesReceived += (s, bytes) =>
            {
                //System.Diagnostics.Trace.WriteLine(bytes.AsString());
                Console.WriteLine(bytes.AsString());
                _mreContinue.Set();
                //don't return anything
            };
            Assert.IsNotNull(Host);
            Console.WriteLine("Host Created");
            Host.Start(PipeName, 20, false);
            Console.WriteLine("Host Started");

            Client = new W.IO.Pipes.PipeLogger(PipeName, true);
            Console.WriteLine("Logging Client Created");
        }
        [TestCleanup]
        public virtual void Cleanup()
        {
            Host?.Dispose();
            Host = null;
            Console.WriteLine("Host Disposed");

            Client.Dispose();
            Console.WriteLine("Logging Client Disposed");
        }

        [TestMethod]
        public void Pipe_LogOnce()
        {
            LogMessages(1);
            //Client.Pipe.Stream.WaitForPipeDrain();
        }
        [TestMethod]
        public void Pipe_Log10()
        {
            LogMessages(10);
            //Client.Pipe.Stream.WaitForPipeDrain();
        }
        [TestMethod]
        public void Pipe_Log100()
        {
            LogMessages(100);
            //Client.Pipe.Stream.WaitForPipeDrain();
        }
    }
}