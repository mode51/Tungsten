using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using W;
using W.Logging;
using W.IO.Pipes;

namespace W.Tests
{
    [TestClass]
    public class PipeClientLoggerTests
    {
        //private W.IO.Pipes.PipeHost Host { get; set; }
        //private W.IO.Pipes.PipeLogger Client { get; set; }
        //private ManualResetEventSlim _mreContinue = new ManualResetEventSlim(false);
        private Random _r = new Random();
        protected string PipeName => "PipeClientLogger_Tests_" + new Random().Next(int.MaxValue).ToString();// Guid.NewGuid().ToString();

        private void LogAMessage(int number)
        {
            var msg = $"Test Log Message: {number}";
            switch (_r.Next(0, 4))
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
        //private void LogMessages(int count)
        //{
        //    for (int t = 1; t <= count; t++)
        //    {
        //        _mreContinue.Reset();
        //        LogAMessage(t);
        //        Assert.IsTrue(_mreContinue.Wait(2000), $"Failed on {t}");
        //    }
        //}

        //[TestInitialize]
        //public virtual void Initialize()
        //{
        //    Console.WriteLine("PipeName = {0}", PipeName);
        //    Host = new W.IO.Pipes.PipeHost();
        //    Host.BytesReceived += (s, bytes) =>
        //    {
        //        //System.Diagnostics.Trace.WriteLine(bytes.AsString());
        //        Console.WriteLine(bytes.AsString());
        //        _mreContinue.Set();
        //        //don't return anything
        //    };
        //    Assert.IsNotNull(Host);
        //    Console.WriteLine("Host Created");
        //    Host.Start(PipeName, 20, false);
        //    Console.WriteLine("Host Started");

        //    Client = new W.IO.Pipes.PipeLogger(PipeName, true);
        //    Console.WriteLine("Logging Client Created");
        //}
        //[TestCleanup]
        //public virtual void Cleanup()
        //{
        //    Host?.Dispose();
        //    Host = null;
        //    Console.WriteLine("Host Disposed");

        //    Client.Dispose();
        //    Console.WriteLine("Logging Client Disposed");
        //}
        //private static string _pipeLoggerName;
        //private void ReceiveBytes(PipeServer server, byte[] bytes)
        //{
        //    Console.WriteLine("Received bytes: " + bytes.AsString());
        //    _mreContinue.Set();
        //}
        //private void LogTheMessage(string message)
        //{
        //    W.IO.Pipes.PipeLogger.LogTheMessage(".", _pipeLoggerName, message);
        //}
        [TestMethod]
        public void Pipe_Log1()
        {
            Log.LogTheMessage += message => { System.Diagnostics.Debug.WriteLine(message); };
            var mreContinue = new ManualResetEventSlim(false);
            var pipeLoggerName = PipeName;
            void MessageReceived(PipeHost<byte[]> host, Pipe server, byte[] bytes)
            {
                Console.WriteLine("Received bytes: " + bytes.AsString());
                mreContinue.Set();
            }
            void LogTheMessage(string message)
            {
                W.IO.Pipes.PipeLogger.LogTheMessage(".", pipeLoggerName, message);
            }
            void LogMessages(int count)
            {
                for (int t = 1; t <= count; t++)
                {
                    mreContinue.Reset();
                    LogAMessage(t);
                    Assert.IsTrue(mreContinue.Wait(2000), $"Failed on {t}");
                }
            }
            pipeLoggerName = PipeName;
            using (var host = new PipeHost())
            {
                host.MessageReceived += MessageReceived;

                Assert.IsNotNull(host);
                Console.WriteLine("Host Created");
                host.Start(pipeLoggerName, 20);
                Console.WriteLine("Host Started");

                W.Logging.Log.LogTheMessage += LogTheMessage;
                LogMessages(1);
                W.Logging.Log.LogTheMessage -= LogTheMessage;
            }
            Console.WriteLine("Host Disposed");
            mreContinue.Dispose();
        }
        [TestMethod]
        public void Pipe_Log10()
        {
            Log.LogTheMessage += message => { System.Diagnostics.Debug.WriteLine(message); };
            var mreContinue = new ManualResetEventSlim(false);
            var pipeLoggerName = PipeName;
            void ReceiveBytes(PipeHost<byte[]> host, Pipe server, byte[] bytes)
            {
                Console.WriteLine("Received bytes: " + bytes.AsString());
                mreContinue.Set();
            }
            void LogTheMessage(string message)
            {
                W.IO.Pipes.PipeLogger.LogTheMessage(".", pipeLoggerName, message);
            }
            void LogMessages(int count)
            {
                for (int t = 1; t <= count; t++)
                {
                    mreContinue.Reset();
                    LogAMessage(t);
                    Assert.IsTrue(mreContinue.Wait(2000), $"Failed on {t}");
                }
            }
            pipeLoggerName = PipeName;
            using (var host = new PipeHost())
            {
                host.MessageReceived += ReceiveBytes;
                Assert.IsNotNull(host);
                Console.WriteLine("Host Created");
                host.Start(pipeLoggerName, 20);
                Console.WriteLine("Host Started");

                W.Logging.Log.LogTheMessage += LogTheMessage;
                LogMessages(10);
                W.Logging.Log.LogTheMessage -= LogTheMessage;
            }
            Console.WriteLine("Host Disposed");
            mreContinue.Dispose();
        }
        [TestMethod]
        public void Pipe_Log100()
        {
            Log.LogTheMessage += message => { System.Diagnostics.Debug.WriteLine(message); };
            var mreContinue = new ManualResetEventSlim(false);
            var pipeLoggerName = PipeName + "A";
            void ReceiveBytes(PipeHost<byte[]> host, Pipe server, byte[] bytes)
            {
                Console.WriteLine("Received bytes: " + bytes.AsString());
                mreContinue.Set();
            }
            void LogTheMessage(string message)
            {
                W.IO.Pipes.PipeLogger.LogTheMessage(".", pipeLoggerName, message);
            }
            void LogMessages(int count)
            {
                for (int t = 1; t <= count; t++)
                {
                    mreContinue.Reset();
                    LogAMessage(t);
                    var success = mreContinue.Wait(2000);
                    if (!success)
                    {
                        //Assert.IsNotNull(W.IO.Pipes.PipeLogger.PipeClient, "The Pipe is null");
                        //Assert.IsTrue(W.IO.Pipes.PipeLogger.PipeClient.Stream.IsConnected, "The Pipe is not connected");
                    }
                    Assert.IsTrue(success, $"Failed on {t}");
                }
            }
            pipeLoggerName = PipeName + "A";
            using (var host = new PipeHost())
            {
                host.MessageReceived += ReceiveBytes;
                Assert.IsNotNull(host);
                Console.WriteLine("Host Created");
                host.Start(pipeLoggerName, 20);
                Console.WriteLine("Host Started");

                W.Logging.Log.LogTheMessage += LogTheMessage;
                LogMessages(100);
                W.Logging.Log.LogTheMessage -= LogTheMessage;
            }
            Console.WriteLine("Host Disposed");
            mreContinue.Dispose();
        }
    }
}