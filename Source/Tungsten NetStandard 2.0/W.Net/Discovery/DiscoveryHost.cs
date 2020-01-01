using System;
using System.Net;
using W;

namespace W.Net.Discovery
{
    public class DiscoveryHost : DiscoveryBase
    {
        private string _applicationId = string.Empty;
        private string _response = string.Empty;
        protected override void OnUnconnectedEvent(IPEndPoint remoteEndPoint, LiteNetLib.NetPacketReader reader, LiteNetLib.UnconnectedMessageType messageType)
        {
            if (messageType == LiteNetLib.UnconnectedMessageType.DiscoveryRequest && reader.AvailableBytes > 0)
            {
                var bytes = reader.GetRemainingBytes();
                var json = bytes.AsString();
                var di = DiscoveryInformation.FromJson(json);
                if (di?.ApplicationId == _applicationId)
                {
                    di.Data = _response;
                    di.Timestamp = DateTime.Now;
                    Console.WriteLine($"{DateTime.Now} DiscoveryHost: Sending \'{di.Data}\'");
                    Manager.SendDiscoveryResponse(di.AsJson().AsBytes(), remoteEndPoint);
                    Console.WriteLine($"{DateTime.Now} DiscoveryHost: Sent discovery response");
                }
            }
        }
        public void Start(IPAddress discoveryAddress, int discoveryPort, string applicationId, string response)
        {
            _applicationId = applicationId;
            _response = response;
            Console.WriteLine($"{DateTime.Now} DiscoveryHost: Starting");
            base.Start();
            Manager.Start(discoveryAddress.ToString(), "", discoveryPort);
            Console.WriteLine($"{DateTime.Now} DiscoveryHost: Manager started");
            StartPollingForEvents(15);
            Console.WriteLine($"{DateTime.Now} DiscoveryHost: Polling thread started");
        }
    }
}
