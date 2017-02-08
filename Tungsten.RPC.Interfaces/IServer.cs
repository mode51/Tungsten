using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace W.RPC.Interfaces
{
    public interface IServer
    {
        void Start(IPAddress address, int port);
        void Stop();
        void Dispose();
    }
}
