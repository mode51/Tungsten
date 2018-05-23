using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace W.IO.Pipes
{
    /// <summary>
    /// Hosts a number of PipeServers.  This class sends and receives byte arrays.
    /// </summary>
    public class PipeHost : PipeHost<byte[]> { }

    /// <summary>
    /// The generic version of PipeHost.  This class expects all messages to be of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The message type to send and receive</typeparam>
    public class PipeHost<TMessage> : IDisposable //where TServer : PipeServer<TMessage>, new()
    {
        private class PipeContainer
        {
            public PipeServer<TMessage> Server { get; set; }
            public Exception e { get; set; }
        }
        private W.Lockable<List<PipeContainer>> _servers = new W.Lockable<List<PipeContainer>>(new List<PipeContainer>());
        private volatile bool _isStarted = false;
        private string _pipeName = "";
        private int _maxConnections = 0;

        /// <summary>
        /// Raised when a pipe server has received data from a client
        /// </summary>
        public event Action<PipeHost<TMessage>, Pipe<TMessage>, TMessage> MessageReceived;
        /// <summary>
        /// Creates the NamedPipeServerStream and intiates waiting for a client connection
        /// </summary>
        /// <returns></returns>
        private bool AddServer()
        {
            var newServer = new PipeServer<TMessage>();
            newServer.MessageReceived += (p, m) => { MessageReceived?.Invoke(this, p, m); };
            newServer.Disconnected += Server_Disconnected;
            newServer.Connected += s =>
            {
                s.Listen();
                //_servers.InLock(W.Threading.Lockers.LockTypeEnum.Read, async list =>
                //{
                //var container = list.FirstOrDefault(pc => pc.Server == s);
                //if (container != null)
                //{
                //    container.Server.Listen();
                //}
                //});
            };
            _servers.InLock(W.Threading.Lockers.LockTypeEnum.Write, list =>
            {
                list.Add(new PipeContainer() { Server = newServer });
            });
            newServer.StartException += (s, e) =>
            {
                _servers.InLock(W.Threading.Lockers.LockTypeEnum.Write, list =>
                {
                    var container = list.FirstOrDefault(pc => pc.Server == newServer);
                    if (container != null) //remove the container in the StartAsync task continuation below
                        container.e = e;
                });
            };

            //if successfull, StartAsync calls the newServer.Connected event before returning
            //if unsuccessfull, find the container and remove it, raise the exception if one was captured
            if (newServer.WaitForConnection(_pipeName, -1 /*_maxConnections*/ ) == false)
            {
                newServer.Dispose();
                _servers.InLock(W.Threading.Lockers.LockTypeEnum.Write, list =>
                {
                    list.RemoveAll(c => c.Server == newServer);
                });
                return false;
            }
            return true;
        }
        private void Server_Disconnected(Pipe server, Exception e)
        {
            if (server == null)
                return;
            _servers.InLock(W.Threading.Lockers.LockTypeEnum.Write, list =>
            {
                var container = list.FirstOrDefault(pc => pc.Server == server);
                if (container != null)
                {
                    list.Remove(container);
                    container.Server.Dispose();
                }
            });
            if (_isStarted)
                AddServer();
        }
        /// <summary>
        /// Creates the specified number of pipe servers and starts listening for clients
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="maxConnections">The maximum number of pipe servers to create.</param>
        /// <returns></returns>
        /// <remarks>Because PipeHost creates a PipeServer for each possible connection, this value cannot be negative.  This breaks the standard paradigm for pipes.  If this does not work for you, use PipeServer instead.</remarks>
        public uint Start(string pipeName, int maxConnections)
        {
            if (_isStarted)
                return (uint)_servers.Value.Count;
            _pipeName = pipeName;
            _maxConnections = maxConnections;
            _isStarted = true;
            while (_servers.Value.Count < maxConnections && AddServer()) ;
            return (uint)_servers.Value.Count;
        }
        /// <summary>
        /// Disconnects and disposes all of the pipe servers
        /// </summary>
        /// <param name="waitForCleanup"></param>
        public void Stop()
        {
            _isStarted = false;
            _servers.InLock(W.Threading.Lockers.LockTypeEnum.Write, list =>
            {
                var tasks = new List<Task>();
                foreach (var item in list)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        item.Server.StopListening();
                        item.Server.Dispose();
                    }));
                }
                Task.WaitAll(tasks.ToArray());
                list.Clear();
            });
        }
        /// <summary>
        /// Stops the host and releases resources
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
