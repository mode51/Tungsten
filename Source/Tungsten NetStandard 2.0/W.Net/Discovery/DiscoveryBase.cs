using System;
using System.Net;

namespace W.Net.Discovery
{
    public abstract class DiscoveryBase : IDisposable
    {
        private System.Threading.CancellationTokenSource Cts { get; set; } = null;
        private bool ExitSendDiscoveryResponseTask
        {
            get { return Cts?.IsCancellationRequested ?? true; }
        }
        private System.Threading.Tasks.Task PollTask { get; set; } = null;
        private int? PollPeriod { get; set; } = null;
        protected LiteNetLib.EventBasedNetListener Listener { get; set; } = null;
        protected LiteNetLib.NetManager Manager { get; set; } = null;

        //public class PeerMessage : EventArgs { public string Data { get; set; } }
        //public delegate void MessageReceivedDelegate(Peer peer, PeerMessage message);
        //public event MessageReceivedDelegate MessageReceived;

        protected virtual void OnUnconnectedEvent(IPEndPoint remoteEndPoint, LiteNetLib.NetPacketReader reader, LiteNetLib.UnconnectedMessageType messageType) { }
        protected virtual void OnNetworkError(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError) { }
        protected virtual void OnPeerDisconnected(LiteNetLib.NetPeer peer, LiteNetLib.DisconnectInfo disconnectInfo) { }
        protected virtual void OnPeerConnected(LiteNetLib.NetPeer peer) { }
        protected virtual void OnReceive(LiteNetLib.NetPeer peer, LiteNetLib.NetPacketReader reader, LiteNetLib.DeliveryMethod deliveryMethod) { }

        protected virtual void Start()
        {
            Stop();
            Listener = new LiteNetLib.EventBasedNetListener();
            Listener.NetworkReceiveUnconnectedEvent += OnUnconnectedEvent;
            Listener.NetworkReceiveEvent += OnReceive;
            Listener.PeerConnectedEvent += OnPeerConnected;
            Listener.PeerDisconnectedEvent += OnPeerDisconnected;
            Listener.NetworkErrorEvent += OnNetworkError;

            Manager = new LiteNetLib.NetManager(Listener);
            Manager.DiscoveryEnabled = true;
            Manager.ReuseAddress = true;
            Manager.UnconnectedMessagesEnabled = true;
        }
        protected void StartPollingForEvents(int msPollPeriod)
        {
            PollPeriod = msPollPeriod;
            Cts = new System.Threading.CancellationTokenSource();
            PollTask.Start();
        }
        protected void StopPollingForEvents()
        {
            if (Cts != null)
            {
                Cts.Cancel();
                PollTask.Wait();
            }
        }

        public virtual void Stop()
        {
            StopPollingForEvents();
            Manager?.Stop();
            Manager = null;
            Listener = null;
        }
        public DiscoveryBase()
        {
            PollTask = new System.Threading.Tasks.Task(() =>
            {
                while (!ExitSendDiscoveryResponseTask)
                {
                    Manager.PollEvents();
                    System.Threading.SpinWait.SpinUntil(() => ExitSendDiscoveryResponseTask, (int)PollPeriod);
                }
            });
        }
        public void Dispose()
        {
            Stop();
            Cts?.Dispose();
            PollTask.Wait();
            Cts = null;
            PollTask.Dispose();
            PollTask = null;
        }
    }
}
