using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// Hosts multiple PipeServers to support concurrent client connections
    /// </summary>
    public class PipeHost : IDisposable
    {
        private bool _raiseEvents = true;
        private object _serversLock = new object();
        private List<PipeServer> _servers = new List<PipeServer>();
        private LockableSlim<bool> _okToListen = new LockableSlim<bool>(true);
        private string _pipeName = "";
        private int _maxConnections = 0;
        private bool _useCompression = false;

        /// <summary>
        /// Raised when a PipeClient has connected to a PipeServer
        /// </summary>
        public event Action<PipeServer> Connected;
        /// <summary>
        /// Raised when a PipeClient has disconnected from it's PipeServer
        /// </summary>
        public event Action<PipeServer> Disconnected;
        /// <summary>
        /// Raised when a PipeServer has received a message from a PipeClient
        /// </summary>
        public event Action<PipeServer, byte[]> BytesReceived;

        private PipeServer AddAServer(string pipeName, int maxConnections, bool useCompression)
        {
            var result = PipeServer.CreateServer(pipeName, maxConnections, useCompression, s => { if (_raiseEvents) Connected?.Invoke(s); });
            lock (_serversLock)
                _servers.Add(result.Result);
            //Connected?.Invoke(result.Result);
            result.Result.BytesReceived += (s, bytes) =>
            {
                if (_raiseEvents)
                    BytesReceived?.Invoke(s, bytes);
            };
            result.Result.Disconnected += Handle_Disconnected;
            return result.Result;
        }
        private void Handle_Disconnected(PipeClient server)
        {
            RemoveServer((PipeServer)server);
            if (_okToListen.Value)
                AddAServer(_pipeName, _maxConnections, _useCompression);
        }
        private void RemoveServer(PipeServer server)
        {
            try
            {
                server.Disconnected -= Handle_Disconnected; //to disable a recursive call to Dispose
                server.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            lock (_serversLock)
                _servers.Remove(server);
            //Disconnected?.Invoke(server);
        }

        /// <summary>
        /// Starts the specified number of servers
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="maxConnections">The maximum number of concurrent client connections</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public void Start(string pipeName, int maxConnections, bool useCompression)
        {
            _raiseEvents = true;
            _pipeName = pipeName;
            _maxConnections = maxConnections;
            _useCompression = useCompression;
            //add the servers and have each one start listening
            for (int t = 0; t < maxConnections; t++)
            {
                AddAServer(pipeName, maxConnections, useCompression);
            }
        }
        /// <summary>
        /// Asynchronously starts the specified number of servers
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="maxConnections">The maximum number of concurrent client connections</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public async Task StartAsync(string pipeName, int maxConnections, bool useCompression)
        {
            await Task.Run(() =>
            {
                Start(pipeName, maxConnections, _useCompression);
            });
        }
        /// <summary>
        /// Disconnects all clients and stops all the servers
        /// </summary>
        public void Stop()
        {
            _okToListen.Value = false;
            _raiseEvents = false;
            lock (_serversLock)
            {
                //Close each server and remove it
                for (int t = _servers.Count - 1; t >= 0; t--)
                {
                    //connect and immediately disconnect a client to each waiting server stream (disconnecting will automatically remove it)
                    using (var npcs = new System.IO.Pipes.NamedPipeClientStream(_pipeName))
                    {
                        npcs.Connect();
                    }
                    //RemoveServer(_servers[t]);
                }
            }
            _okToListen.Value = true;
        }
        /// <summary>
        /// Disposes the PipeHost and releases resources
        /// </summary>
        public void Dispose()
        {
            Stop();
            _okToListen.Value = false;
        }
    }

    /// <summary>
    /// Hosts multiple PipeServers to support concurrent client connections
    /// </summary>
    public class PipeHost<TType> : IDisposable where TType : class
    {
        private bool _raiseEvents = true;
        private object _serversLock = new object();
        private List<PipeServer<TType>> _servers = new List<PipeServer<TType>>();
        private LockableSlim<bool> _okToListen = new LockableSlim<bool>(true);
        private string _pipeName = "";
        private int _maxConnections = 0;
        private bool _useCompression = false;

        /// <summary>
        /// Raised when a PipeClient has connected to a PipeServer
        /// </summary>
        public event Action<PipeServer<TType>> Connected;
        /// <summary>
        /// Raised when a PipeClient has disconnected from it's PipeServer
        /// </summary>
        public event Action<PipeServer<TType>> Disconnected;
        /// <summary>
        /// Raised when a PipeServer has received a message from a PipeClient
        /// </summary>
        public event Action<PipeServer<TType>, TType> MessageReceived;

        private void Handle_Disconnected(PipeClient server)
        {
            RemoveServer((PipeServer<TType>)server);
            if (_okToListen.Value)
                AddAServer(_pipeName, _maxConnections, _useCompression);
        }
        private PipeServer<TType> AddAServer(string pipeName, int maxConnections, bool useCompression)
        {
            var result = PipeServer<TType>.CreateServer(pipeName, maxConnections, useCompression, s => { if (_raiseEvents) Connected?.Invoke(s); });
            lock (_serversLock)
                _servers.Add(result.Result);
            //Connected?.Invoke(result.Result);
            result.Result.MessageReceived += (s, message) =>
            {
                MessageReceived?.Invoke(s, message);
            };
            result.Result.Disconnected += Handle_Disconnected;
            return result.Result;
        }
        private void RemoveServer(PipeServer<TType> server)
        {
            try
            {
                server.Disconnected -= Handle_Disconnected;
                server.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            lock (_serversLock)
                _servers.Remove(server);
            if (_raiseEvents)
                Disconnected?.Invoke(server);
        }

        /// <summary>
        /// Starts the specified number of servers
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="maxConnections">The maximum number of concurrent client connections</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public void Start(string pipeName, int maxConnections, bool useCompression)
        {
            _raiseEvents = true;
            _useCompression = useCompression;
            //add the servers and have each one start listening
            for (int t = 0; t < maxConnections; t++)
            {
                AddAServer(pipeName, maxConnections, useCompression);
            }
        }
        /// <summary>
        /// Asynchronously starts the specified number of servers
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="maxConnections">The maximum number of concurrent client connections</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public async Task StartAsync(string pipeName, int maxConnections, bool useCompression)
        {
            await Task.Run(() =>
            {
                Start(pipeName, maxConnections, _useCompression);
            });
        }
        /// <summary>
        /// Disconnects all clients and stops all the servers
        /// </summary>
        public void Stop()
        {
            _okToListen.Value = false;
            _raiseEvents = false;
            lock (_serversLock)
            {
                //Close each server and remove it
                for (int t = _servers.Count - 1; t >= 0; t--)
                {
                    RemoveServer(_servers[t]);
                }
            }
            _okToListen.Value = true;
        }
        /// <summary>
        /// Disposes the PipeHost and releases resources
        /// </summary>
        public void Dispose()
        {
            Stop();
            _okToListen.Value = false;
        }
    }
}
