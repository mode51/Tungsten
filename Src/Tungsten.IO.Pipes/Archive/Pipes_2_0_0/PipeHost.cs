using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace W.IO.Pipes
{
    //internal static class LockExtensions
    //{
    //    public static void InLock(this System.Threading.ReaderWriterLockSlim @this, bool writeMode, Action action)
    //    {
    //        if (writeMode)
    //            @this.EnterWriteLock();
    //        else
    //            @this.EnterReadLock();
    //        try
    //        {
    //            action.Invoke();
    //        }
    //        catch { throw; }
    //        finally
    //        {
    //            if (writeMode)
    //                @this.ExitWriteLock();
    //            else
    //                @this.ExitReadLock();
    //        }

    //    }
    //}
    /// <summary>
    /// Hosts multiple PipeServers to support concurrent client connections
    /// </summary>
    public class PipeHost : IDisposable
    {
        private bool _raiseEvents = true;
        private W.Threading.Lockers.ReaderWriterLocker _serversLock = new Threading.Lockers.ReaderWriterLocker(System.Threading.LockRecursionPolicy.SupportsRecursion);
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
        //public event Action<PipeServer> Disconnected;
        /// <summary>
        /// Raised when a PipeServer has received a message from a PipeClient
        /// </summary>
        public event Action<PipeServer, byte[]> BytesReceived;

        private PipeServer AddAServer(string pipeName, int maxConnections, bool useCompression)
        {
            var result = PipeServer.CreateServer(pipeName, maxConnections, useCompression, s => { if (_raiseEvents) Connected?.Invoke(s); });
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
            //Disconnected?.Invoke((PipeServer)server);
            _serversLock.InLock(W.Threading.Lockers.LockTypeEnum.Write, () =>
            {
                RemoveServer((PipeServer)server);
                if (_okToListen.Value)
                    AddAServer(_pipeName, _maxConnections, _useCompression);
            });
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

            _serversLock.InLock(W.Threading.Lockers.LockTypeEnum.Write, () =>
            {
                for (int t = 0; t < maxConnections; t++)
                {
                    AddAServer(pipeName, maxConnections, useCompression);
                }
            });
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
            _serversLock.InLock(W.Threading.Lockers.LockTypeEnum.Write, () =>
            {
                //Close each server and remove it
                for (int t = _servers.Count - 1; t >= 0; t--)
                {
                    if (!_servers[t]?.Stream?.IsConnected ?? false)
                    {
                        //connect and immediately disconnect a client to each waiting server stream (disconnecting will automatically remove it)
                        using (var npcs = new System.IO.Pipes.NamedPipeClientStream(_pipeName))
                        {
                            npcs.Connect();
                        }//dispose causes disconnect which will in-turn cause a RemoveServer
                    }
                    _servers[t].Dispose();
                    RemoveServer(_servers[t]);
                }
            });
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
    public class PipeHostServer : IDisposable
    {
        protected LockableSlim<bool> AddServerOk { get; } = new LockableSlim<bool>(true);

        public static PipeServer[] Servers = new PipeServer[0];
        public string PipeName { get; private set; }
        public int MaxNumberOfServers { get; private set; }
        public int ServerCount { get => Servers.Length; }
        public bool UseCompression { get; private set; }

        /// <summary>
        /// Raised when a PipeServer has received a message from a PipeClient
        /// </summary>
        public event Action<PipeServer, byte[]> BytesReceived;

        private bool AddServer(int serverIndex)
        {
            var result = false;
            AddServerOk.InLock(Threading.Lockers.LockTypeEnum.Read, addServerOk =>
            {
                if (!addServerOk)
                {
                    //Servers[serverIndex]?.Dispose();
                    //Servers[serverIndex] = null;
                }
                else
                {
                    try
                    {
                        Helpers.CreateServer(PipeName, MaxNumberOfServers, p =>
                        {
                            var server = new PipeServer(p, UseCompression);
                            server.ServerIndex = serverIndex;
                            server.BytesReceived += (s, b) => { BytesReceived?.Invoke(s, b); };
                            server.Disconnected += s => AddServer(((PipeServer)s).ServerIndex);

                            if (Servers.Length == 0 || serverIndex >= Servers.Length)
                                //insert the new server
                                ArrayMethods.Append(ref Servers, new PipeServer[] { server });
                            else
                            {
                                //replace the old server
                                Servers[serverIndex]?.Dispose();
                                Servers[serverIndex] = server;
                            }
                        }, null);
                        result = true;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        System.Diagnostics.Debugger.Break();
                    }
                }
            });
            return result;
        }

        public void Start(string pipeName, int numberOfServers = -1, bool useCompression = false)
        {
            Stop();
            AddServerOk.InLock(Threading.Lockers.LockTypeEnum.Write, addServerOk =>
            {
                AddServerOk.Value = true;
                MaxNumberOfServers = numberOfServers;
                PipeName = pipeName;
                UseCompression = useCompression;
                while (ServerCount < numberOfServers)
                {
                    var result = AddServer(ServerCount);
                    if (!result)
                        break;
                }
                Console.WriteLine($"Added {ServerCount} servers");
            });
        }
        public async Task StartAsync(string pipeName, int numberOfServers = -1, bool useCompression = false)
        {
            await Task.Run(() =>
            {
                Start(pipeName, numberOfServers, useCompression);
            });
        }
        public void Stop()
        {
            AddServerOk.InLock(Threading.Lockers.LockTypeEnum.Write, addServerOk =>
            {
                AddServerOk.Value = false;
                var count = 0;
                foreach (var server in Servers)
                {
                    try
                    {
                        server.Disconnect(true);
                        server.Dispose();
                    }
                    finally
                    {
                        count += 1;
                    }
                }
                Array.Resize(ref Servers, 0);
                //AddServerOk.Value = true;
                Console.WriteLine($"PipeHostServer Stopped: {count} servers removed.");
            });
        }

        public void Dispose()
        {
            Stop();
        }
    }


    /// <summary>
    /// Hosts multiple PipeServers to support concurrent client connections
    /// </summary>
    public class PipeHost<TType> : IDisposable where TType : class
    {
        private bool _raiseEvents = true;
        private W.Threading.Lockers.ReaderWriterLocker _serversLock = new Threading.Lockers.ReaderWriterLocker(System.Threading.LockRecursionPolicy.SupportsRecursion);
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
            _serversLock.InLock(W.Threading.Lockers.LockTypeEnum.Write, () => _servers.Add(result.Result));
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
            _serversLock.InLock(W.Threading.Lockers.LockTypeEnum.Write, () => _servers.Remove(server));
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
            _serversLock.InLock(W.Threading.Lockers.LockTypeEnum.Write, () =>
            {
                //Close each server and remove it
                for (int t = _servers.Count - 1; t >= 0; t--)
                {
                    RemoveServer(_servers[t]);
                }
            });
            _okToListen.Value = true;
        }
        /// <summary>
        /// Disposes the PipeHost and releases resources
        /// </summary>
        public void Dispose()
        {
            Stop();
            _okToListen.Value = false;
            _serversLock.Dispose();
        }
    }
}
