using System;
using System.Net;
using W;

namespace W.Net.Discovery
{
    public class DiscoveryClient : DiscoveryBase, IDisposable
    {
        private System.Threading.CancellationTokenSource Cts { get; set; } = null;
        private bool ExitSendDiscoveryRequestsTask
        {
            get { return Cts?.IsCancellationRequested ?? true; }
        }
        private System.Threading.Tasks.Task DiscoveryTask { get; set; } = null;
        private int DiscoveryPort { get; set; } = 0;
        private int SendDiscoveryFrequency { get; set; } = 1000;

        private string _applicationId = "";
        public delegate void LastDiscoveryRequestDelegate(DiscoveryClient peer, DateTime timestamp);
        public event LastDiscoveryRequestDelegate LastDiscoveryRequest;
        public delegate void HostFoundDelegate(DiscoveryClient peer, IPEndPoint hostEndPoint, DiscoveryInformation discoveryInformation);
        public event HostFoundDelegate HostFound;

        protected override void OnUnconnectedEvent(IPEndPoint remoteEndPoint, LiteNetLib.NetPacketReader reader, LiteNetLib.UnconnectedMessageType messageType)
        {
            Console.WriteLine($"{DateTime.Now} DiscoveryClient: UnconnectedEvent - {remoteEndPoint}, {messageType}");
            if (messageType == LiteNetLib.UnconnectedMessageType.DiscoveryResponse && reader.AvailableBytes > 0)
            {
                var bytes = reader.GetRemainingBytes();
                var json = bytes.AsString();
                var di = DiscoveryInformation.FromJson(json);
                Console.WriteLine($"{DateTime.Now} DiscoveryClient: Received - {di?.ApplicationId}, {di?.Data}");
                if (di?.ApplicationId == _applicationId)
                {
                    HostFound?.Invoke(this, remoteEndPoint, di);
                }
            }
        }
        public void Start(IPAddress localIPAddress, int serverDiscoveryPort, string applicationId, int msSendDiscoveryFrequency = 3000, int msPollPeriod = 10)
        {
            _applicationId = applicationId;
            DiscoveryPort = serverDiscoveryPort;
            SendDiscoveryFrequency = msSendDiscoveryFrequency;
            Console.WriteLine($"{DateTime.Now} DiscoveryClient: Starting");
            base.Start();

            Manager.Start(localIPAddress.ToString(), "", 0);
            Console.WriteLine($"{DateTime.Now} DiscoveryClient: Manager started");
            StartPollingForEvents(msPollPeriod);
            Cts = new System.Threading.CancellationTokenSource();
            DiscoveryTask.Start();
            Console.WriteLine($"{DateTime.Now} DiscoveryClient: Polling thread started");
        }
        public new void Stop()
        {
            Console.WriteLine($"{DateTime.Now} DiscoveryClient: Stopping");
            Cts?.Cancel();
            DiscoveryTask?.Wait();
            Cts?.Dispose();
            Cts = null;
            base.Stop();
            Console.WriteLine($"{DateTime.Now} DiscoveryClient: Stopped");
        }
        public DiscoveryClient()
        {
            DiscoveryTask = new System.Threading.Tasks.Task(() =>
            {
                Console.WriteLine($"{DateTime.Now} DiscoveryClient: Entering discovery thread");
                var di = new DiscoveryInformation() { ApplicationId = _applicationId };
                while (!ExitSendDiscoveryRequestsTask)
                {
                    var json = di.AsJson();
                    var bytes = json.AsBytes();
                    var dt = DateTime.Now;
                    Console.WriteLine($"{DateTime.Now} DiscoveryClient: Manager sending discovery request");
                    Manager.SendDiscoveryRequest(bytes, DiscoveryPort);
                    Console.WriteLine($"{DateTime.Now} DiscoveryClient: Manager sent discovery request");
                    LastDiscoveryRequest?.Invoke(this, dt);
                    System.Threading.SpinWait.SpinUntil(() => ExitSendDiscoveryRequestsTask, SendDiscoveryFrequency);
                }
                Console.WriteLine($"{DateTime.Now} DiscoveryClient: Exiting discovery thread");
            });
        }
        public new void Dispose()
        {
            Stop();
            DiscoveryTask?.Dispose();
            DiscoveryTask = null;
            base.Dispose();
        }
    }
}
