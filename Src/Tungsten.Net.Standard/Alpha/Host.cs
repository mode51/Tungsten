using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using W.AsExtensions;
using W.DelegateExtensions;
#if NET45
using System.Security.Principal;
#endif

namespace W.Net.Alpha
{
    /// <summary>
    /// A 
    /// </summary>
    public class EchoHost : Host
    {
        /// <summary>
        /// Constructs a new EchoHost
        /// </summary>
        public EchoHost()
        {
            BytesReceived += (h, s, bytes) =>
            {
                //echo
                s.Write(bytes);
            };
        }
    }
    /// <summary>
    /// A host for byte-array network servers
    /// </summary>
    public class Host : HostBase<Server> { }

    /// <summary>
    /// A host for network servers for serializable objects
    /// </summary>
    /// <typeparam name="TMessage">The Type of object to send/receive over the network</typeparam>
    public class Host<TMessage> : HostBase<Server<TMessage>> where TMessage : class, new()
    {
        /// <summary>
        /// Raised when a message is received from a client
        /// </summary>
        public event Action<Host<TMessage>, Server<TMessage>, TMessage> MessageReceived;// { get; set; }

        /// <summary>
        /// Constructs a new Host
        /// </summary>
        public Host()
        {
            BytesReceived += (o, server, bytes) =>
            {
                var json = bytes.AsString();
                var message = Newtonsoft.Json.JsonConvert.DeserializeObject<TMessage>(json);
                //MessageReceived?.BeginInvoke(this, server, message, ar => MessageReceived.EndInvoke(ar), null);
                MessageReceived?.Raise(this, server, message);
            };
        }
    }
}
