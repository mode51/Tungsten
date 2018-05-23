using System;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// A Pipe server.  This class sends and receives only byte arrays.
    /// </summary>
    public class PipeServer : PipeServer<byte[]> { }

    /// <summary>
    /// The generic version of PipeServer.  This class expects all messages to be of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The message type to send and receive</typeparam>
    public class PipeServer<TMessage> : Pipe<TMessage>
    {
        private static volatile int _serverCount = 0;
        private volatile bool _waitingForConnection = false;
        private volatile bool _isDisposed = false;
        private volatile bool _isDisposing = false;
        private System.Threading.CancellationTokenSource _ctsDispose = new System.Threading.CancellationTokenSource();
        private W.Threading.Lockers.MonitorLocker _locker = new W.Threading.Lockers.MonitorLocker();
        private string _pipeName = string.Empty;
        private bool _connectedToClient = false;

        /// <summary>
        /// Raised if an exception occurs while creating the NamedPipeServerStream
        /// </summary>
        public event Action<Pipe, Exception> StartException;
        /// <summary>
        /// Raised when a client has connected to the server
        /// </summary>
        public event Action<Pipe> Connected;

        private static void UpdateServerCount(int change)
        {
            _serverCount += change;
            System.Diagnostics.Debug.WriteLine($"Server Count = {_serverCount}");
        }

        /// <summary>
        /// Creats a new NamedPipeServerStream and Waits for a client to connect
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="maxConnections">The maximum number of pipes with this name</param>
        /// <returns>True if the server was created and is waiting, otherwise False</returns>
#if NET45
        public bool WaitForConnection(string pipeName, int maxConnections = NamedPipeServerStream.MaxAllowedServerInstances)
#elif NETSTANDARD1_4
        public bool WaitForConnection(string pipeName, int maxConnections = 254)
#endif
        {
            _pipeName = pipeName;
            //return await Task.Run(() =>
            //{
                return _locker.InLock(() =>
                {
                    if (_isDisposing)
                        return false;
                    try
                    {
                        Stream = Helpers.CreateServer(pipeName, maxConnections, out Exception e);
                        if (e != null)
                            StartException?.Invoke(this, e);
                        else
                        {
                            if (_isDisposing)
                                return false;
                            try
                            {
                                _waitingForConnection = true;
                                Helpers.WaitForClientToConnectAsync((NamedPipeServerStream)Stream, _ctsDispose.Token, () => 
                                {
                                    Connected?.Invoke(this);
                                    _waitingForConnection = false;
                                    _connectedToClient = true;
                                    UpdateServerCount(1);
                                }).ConfigureAwait(false);
                                return true;
                            }
                            catch
                            {
                                System.Diagnostics.Debugger.Break(); //what exception is possible?
                                return false;
                            }
                            finally
                            {
                            }
                        }
                    }
                    catch (NullReferenceException ex) //_ctsDispose can be null (if the server is disposed before the wait completes?)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                    return false;
                });
            //});
        }
        /// <summary>
        /// Disposes the PipeServer and release resources
        /// </summary>
        protected override void OnDispose()
        {
            try
            {
                _locker.InLock(() =>
                {
                    _isDisposing = true;
                    _ctsDispose?.Cancel();
                    _ctsDispose?.Dispose();
                    _ctsDispose = null;
                    base.OnDispose();
                    if (!_isDisposed)
                        RaiseDisconnection(null, null);
                    _isDisposed = true;
                });
                _locker.As<IDisposable>()?.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
            if (_connectedToClient)
                UpdateServerCount(-1);
            _connectedToClient = false;
        }
    }
}
