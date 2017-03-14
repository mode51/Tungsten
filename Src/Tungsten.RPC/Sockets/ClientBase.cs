using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Logging;

namespace W.RPC
{
    internal abstract class ClientBase : ISocketClient, INamed
    {
        //public delegate void ConnectedDelegate(ClientBase client, IPAddress remoteAddress);
        public event Delegates.ConnectedDelegate Connected;
        ////public delegate void ConnectionTimeoutDelegate(ClientBase client, IPAddress remoteAddress);
        //public event Delegates.ConnectionTimeoutDelegate ConnectionTimeout;
        //public delegate void DisconnectedDelegate(ClientBase client, IPAddress remoteAddress, Exception e);
        public event Delegates.DisconnectedDelegate Disconnected;

        private readonly Lockable<bool> _isConnected = new Lockable<bool>(false);

        public bool IsConnected => _isConnected.Value;

        protected delegate void MessageArrivedDelegate(ClientBase client, string message);
        protected event MessageArrivedDelegate MessageArrived;
        protected double MessageReceivedCount { get; set; }

        protected System.Net.Sockets.Socket _socket = null;
        protected bool _keepAlive = false;
        private CancellationTokenSource _close = new CancellationTokenSource();
        private readonly System.Collections.Concurrent.ConcurrentQueue<string> _outgoing = new ConcurrentQueue<string>();
        private readonly System.Collections.Concurrent.ConcurrentQueue<string> _incoming = new ConcurrentQueue<string>();
        private IPAddress _remoteAddress;
        private int blocksize;

        private void RaiseConnected()
        {
            var evt = Connected;
            if (evt != null)
            {
                _remoteAddress = (_socket?.RemoteEndPoint as IPEndPoint)?.Address;
                try
                {
                    evt.Invoke(this, _remoteAddress);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }
        }
        private void RaiseDisconnected(Exception e = null)
        {
            var evt = Disconnected;
            if (evt != null)
            {
                try
                {
                    evt.Invoke(this, e);
                }
                catch (Exception ex)
                {
                    Log.e(ex);
                }
            }
        }
        private dynamic Send(string item, Action<int, int> progress = null)
        {
            var encoded = Convert.ToBase64String(item.AsBytes()) + '\0';
            var data = encoded.AsBytes();
            int length = data.Length; // convert length byte order from host to network order
            int offset = 0;
            int sent = 0;

            int blocksize = _socket.SendBufferSize; // (int)_socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);

            // send the Message data in blocks
            while (offset < length)
            {
                blocksize = Math.Min(length - offset, blocksize);
                try
                {
                    sent = _socket?.Send(data, offset, blocksize, SocketFlags.None) ?? 0;
                }
                catch (NullReferenceException e)
                {
                    Log.e(e);
                    return new { BytesSent = offset, Result = false };
                }
                catch (ObjectDisposedException e)
                {
                    Log.e(e);
                    return new { BytesSent = offset, Result = false };
                }
                catch (SocketException e)
                {
                    Log.e(e);
                    Disconnect(e); //12.7.2016 - added, but probably not the best solution since it's too broad
                    return new { BytesSent = offset, Result = false };
                }
                catch (Exception e)
                {
                    Log.e(e);
                    return new { BytesSent = offset, Result = false };
                }
                offset += sent;
                progress?.Invoke(offset, length);
            }
            return new { BytesSent = length, Result = true };
        }

        private class SimpleReceiveResult : CallResult
        {
            public bool RemoteDisconnected { get; set; } = false;
        }
        private SimpleReceiveResult SimpleReceive(ref byte[] buffer, ref int count, SocketFlags flags, int msTimeout)
        {
            SimpleReceiveResult result = new SimpleReceiveResult();

            try
            {
                System.Threading.Thread.Sleep(0);
                count = _socket.Receive(buffer, 0, count, flags); //remove the complete message and allow for more
                result.Success = true;// = new { Result = true };
            }
            catch (NullReferenceException e)
            {
                result.Exception = e; //= new { Exception = e, Result = false };
                Log.e(e);
            }
            catch (ObjectDisposedException e)
            {
                result.Exception = e;// = new { Exception = e, Result = false };
                Log.e(e);
            }
            catch (SocketException e)
            {
                result.Exception = e;// = new { Exception = e, Result = false };
                Array.Clear(buffer, 0, buffer.Length);
                count = 0;
            }
            return result;
        }

        private string Receive(int msTimeout = 250)
        {
            StringBuilder message = new StringBuilder();
            // read until we get all the data (we can only read the number of bytes == socketbuffer_receive_size at a time
            var buffer = new byte[blocksize];
            var count = 0;
            var fragment = string.Empty;

            while (!_close.IsCancellationRequested)
            {
                System.Threading.Thread.Sleep(0);
                count = blocksize;
                var result = SimpleReceive(ref buffer, ref count, SocketFlags.Peek, msTimeout);
                if (result.Success == false)
                {
                    var se = result.Exception as SocketException;
                    if (se != null && se.SocketErrorCode == SocketError.ConnectionReset || se.SocketErrorCode == SocketError.Interrupted) //the remote connection closed
                    {
                        Disconnect(se);
                        break;
                    }
                    continue;
                }
                fragment = (buffer.Take(count).ToArray()).AsString();

                int pos = fragment.IndexOf('\0');
                if (pos >= 0) //add tail end of message
                {

                    message.Append(fragment.Substring(0, pos));
                    result = SimpleReceive(ref buffer, ref count, SocketFlags.None, msTimeout);
                    break;
                }
                else //add fragment of message
                {
                    message.Append(fragment);
                    try
                    {
                        result = SimpleReceive(ref buffer, ref count, SocketFlags.None, msTimeout);
                        //count = _socket.Receive(buffer, 0, count, SocketFlags.None); //remove the complete message and allow for more
                    }
                    catch (SocketException e)
                    {
                        //just ignore this one
                        //12.8.2016 - why ignore this one?  Why not disconnect due to exception?
                        Log.w("Ignored SocketException: {0}", Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode));
                        Log.e(e);
                    }
                    catch (System.Exception e)
                    {
                        Log.e(e);
                    }
                }
            }
            var encoded = Convert.FromBase64String(message.ToString().Trim('\n'));
            return encoded.AsString();
        }

        private void StartReceiveThread()
        {
            //Listen thread
            W.Threading.Thread.Create((cts) =>
            {
                while (!_close.IsCancellationRequested)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1);
                        if (!_isConnected.Value)
                                //if (!_socket.Connected)
                                continue;
                        var msg = Receive();
                        if (!string.IsNullOrEmpty(msg))
                        {
                            OnMessageReceived(ref msg);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.e(e);
                    }
                }
            });
        }
        private void StartSendThread()
        {
            //Send thread
            W.Threading.Thread.Create(cts =>
            {
                while (!_close.IsCancellationRequested)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(0);
                        if (!_socket.Connected)
                            continue;
                        while (_outgoing.Count > 0)
                        {
                            System.Threading.Thread.Sleep(0);
                            string msg = string.Empty;
                            if (!_outgoing.TryDequeue(out msg)) continue;
                            var result = Send(msg);
                            if (!result.Result)
                            {
                                Log.e("Failed to send message");
                                System.Diagnostics.Debugger.Break();
                                continue;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.e(e);
                    }
                }
            });
        }
        private void StartMessageArrivedThread()
        {
            //Raise event thread
            W.Threading.Thread.Create(cts =>
            {
                while (!_close.IsCancellationRequested)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(0);
                        if (!_socket.Connected)
                            continue;
                        string msg = string.Empty;
                        if (!_incoming.TryDequeue(out msg)) continue;

                        try
                        {
                            var evt = MessageArrived;
                            evt?.Invoke(this, msg);
                        }
                        catch (Exception e)
                        {
                            Log.e(e);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.e(e);
                    }
                }
            });
        }
        private void SetKeepAlive(System.Net.Sockets.Socket socket, bool on, uint keepAliveTime, uint keepAliveInterval)
        {
            int size = Marshal.SizeOf(new uint());

            var inOptionValues = new byte[size * 3];

            BitConverter.GetBytes((uint)(on ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)keepAliveTime).CopyTo(inOptionValues, size);
            BitConverter.GetBytes((uint)keepAliveInterval).CopyTo(inOptionValues, size * 2);

            socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }
        protected void Post(string message)
        {
            OnMessageSend(ref message);
        }

        public void Disconnect()
        {
            Disconnect(null);
        }
        internal void Disconnect(Exception e) //used to disconnect when not actually connected
        {
            System.Diagnostics.Debug.WriteLine("Disconnecting From: {0}", _remoteAddress);
            Log.i("Disconnecting From: {0}", _remoteAddress);

            _close?.Cancel();
            System.Threading.Thread.Sleep(200); //let the other threads think on it

            try
            {
                _socket?.Close();
            }
            catch (PlatformNotSupportedException ex)
            {
                Log.e(ex);
            }
            catch (ObjectDisposedException)
            {
                //Log.e(ex);
            }
            catch (SocketException ex)
            {
                Log.e(ex);
            }

            try
            {
                _isConnected.Value = false;
                RaiseDisconnected(e);
            }
            catch (Exception ex)
            {
                Log.e(ex);
            }
            System.Diagnostics.Debug.WriteLine("Disconnected From: {0}", _remoteAddress);
            Log.i("Disconnected From: {0}", _remoteAddress);
        }

        protected virtual void OnMessageReceived(ref string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _incoming.Enqueue(message);
                MessageReceivedCount += 1;
            }
        }
        protected virtual void OnMessageSend(ref string message)
        {
            if (!string.IsNullOrEmpty(message))
                _outgoing.Enqueue(message);
        }

        private bool  Activate(System.Net.Sockets.Socket socket)
        {
            var result = new CallResult(false);
            try
            {
                if (socket == null)
                    return result.Success;

                _close = new CancellationTokenSource();
                _socket = socket;
                blocksize = socket.ReceiveBufferSize;

                //StartHeartbeat();
                StartReceiveThread();
                StartSendThread();
                StartMessageArrivedThread();
                _isConnected.Value = true;
                result.Success = true;
                RaiseConnected();
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result.Success;
        }

        public bool Connect(System.Net.Sockets.Socket socket) //called by Server
        {
            return Activate(socket);
        }
        public bool Connect(System.Net.Sockets.TcpClient client) //called by Server
        {
            return Connect(client.Client);
        }

        public bool KeepAlive
        {
            get { return _keepAlive; }
            set
            {
                _keepAlive = value;
                SetKeepAlive(_socket, value, 0, 3000);
            }
        }

        //public CallResult Connect(string remoteAddress, int remotePort)
        //{
        //    CallResult result = new CallResult(true);
        //    try
        //    {
        //        //synchronous connect to remote
        //        var client = new TcpClient(remoteAddress, remotePort);
        //        result = Connect(client);
        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        result.Success = false;
        //        result.Exception = e;
        //    }
        //    catch (ArgumentOutOfRangeException e)
        //    {
        //        result.Success = false;
        //        result.Exception = e;
        //    }
        //    catch (SocketException e)
        //    {
        //        result.Success = false;
        //        result.Exception = e;
        //    }
        //    return result;
        //}
        //public CallResult Connect(IPAddress remoteAddress, int remotePort)
        //{
        //    return Connect(remoteAddress.ToString(), remotePort);
        //}

        public bool Connect(string remoteAddress, int remotePort, int msTimeout = 10000, Action<ISocketClient, IPAddress> onConnection = null, Action<Exception> onException = null)
        {
            var client = new TcpClient();

            try
            {
                var result = client.BeginConnect(remoteAddress, remotePort, null, null);
                var success = result.AsyncWaitHandle.WaitOne(msTimeout);
                if (success)
                {
                    try
                    {
                        client.EndConnect(result);
                        if (onConnection  != null)
                            Task.Factory.FromAsync((asyncCallback, @object) => onConnection.BeginInvoke(this, IPAddress.Parse(remoteAddress),  asyncCallback, @object), onConnection.EndInvoke, null);

                        return Connect(client?.Client); //complete connection setup
                    }
                    catch (SocketException e)
                    {
                        Disconnect(e);
                        if (onException != null)
                            Task.Factory.FromAsync((asyncCallback, @object) => onException.BeginInvoke(e, asyncCallback, @object), onException.EndInvoke, null);
                        Log.e(e);
                    }
                    catch (TargetInvocationException e)
                    {
                        Disconnect(e);
                        if (onException != null)
                            Task.Factory.FromAsync((asyncCallback, @object) => onException.BeginInvoke(e, asyncCallback, @object), onException.EndInvoke, null);
                        Log.e(e);
                    }
                    catch (Exception e)
                    {
                        Disconnect(e);
                        if (onException != null)
                            Task.Factory.FromAsync((asyncCallback, @object) => onException.BeginInvoke(e, asyncCallback, @object), onException.EndInvoke, null);
                        Log.e(e);
                    }
                }
                else
                    Disconnect(); //to let the consumer know we didn't connect
            }
            catch (SocketException e)
            {
                Disconnect(e);
                Log.e(e);
            }
            catch (Exception e)
            {
                Disconnect(e);
                Log.e(e);
            }

            return false;
            //client.BeginConnect(remoteAddress, remotePort, ar =>
            //{
            //    var c = ar.AsyncState as TcpClient;
            //    if (c != null)
            //        Activate(c.Client);
            //    else
            //        RaiseDisconnected();
            //}, client);
        }
        public bool Connect(IPAddress remoteAddress, int remotePort, int msTimeout = 10000, Action<ISocketClient, IPAddress> onConnection = null, Action<Exception> onException = null)
        {
            return Connect(remoteAddress.ToString(), remotePort, msTimeout, onConnection, onException);
        }

        public async Task ConnectAsync(string remoteAddress, int remotePort, int msTimeout = 10000)
        {
            await Task.Run(() =>
            {
                Connect(remoteAddress, remotePort, msTimeout);
            });
        }
        public async Task ConnectAsync(IPAddress remoteAddress, int remotePort, int msTimeout = 10000)
        {
            await Task.Run(() =>
            {
                Connect(remoteAddress, remotePort, msTimeout);
            });
        }

        //public ClientBase() { }
        public string Name { get; set; }
    }
}
