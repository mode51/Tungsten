using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.RPC
{
    internal interface IClient
    {
        //CallResult Connect(System.Net.Sockets.Socket socket);
        CallResult Connect(System.Net.Sockets.TcpClient client);
        CallResult Connect(string remoteAddress, int remotePort, int msTimeout = 10000);
        CallResult Connect(IPAddress remoteAddress, int remotePort, int msTimeout = 10000);

        //void BeginConnect(string remoteAddress, int remotePort);
        //void BeginConnect(IPAddress remoteAddress, int remotePort);
    }
}
