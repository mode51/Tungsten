using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Net.Alpha
{
    public partial class SecureClient<TMessage> : SecureSocketBase<SecureClient<TMessage>, Client<TMessage>, TMessage> where TMessage : class, new()
    {
        //this method is called by the programmer
        public bool Connect(IPEndPoint endPoint, int msTimeout = 5000)
        {
            Socket = new Client<TMessage>();
            var result = Socket.Connect(endPoint, msTimeout);
            if (result)
                InitializeConnection();
            return result;
        }
        public SecureClient() : base(2048) { }
        public SecureClient(int keySize) : base(keySize) { }
    }

    public partial class SecureServer<TMessage> : SecureSocketBase<SecureServer<TMessage>, Server<TMessage>, TMessage> where TMessage : class, new()
    {
        //this method is called by SecureHost.InitializeServer when the socket is created
        internal void InitializeServer(Socket socket)
        {
            Socket = new Server<TMessage>();
            Socket.InitializeConnection(socket); //clients call this upon Connect, servers need to assign it here
            InitializeConnection();
        }
        public SecureServer() : base(2048) { }
        public SecureServer(int keySize) : base(keySize) { }
    }
}
