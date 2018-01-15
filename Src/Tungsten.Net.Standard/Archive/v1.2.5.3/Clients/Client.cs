﻿using System;
using System.Net;
using System.Net.Sockets;
using W.Net.Sockets;
using W.Logging;

namespace W.Net
{
    /// <summary>
    /// A TCP client to be used with W.Net.Server
    /// </summary>
    public class Client : IDisposable, IDataSocket
    {
        private W.Net.Sockets.Socket _socket = null;

        /// <summary>
        /// A ManualResetEventSlim which is used to signal when a connection has been established
        /// </summary>
        protected System.Threading.ManualResetEventSlim IsConnectedResetEvent { get; private set; } = new System.Threading.ManualResetEventSlim(false);
        /// <summary>
        /// True if the client was created with an existing connection
        /// </summary>
        protected bool IsServerSide { get; private set; }

        /// <summary>
        /// Serialization settings for Newtonsoft.Json.  Can be modified as needed.
        /// </summary>
        public Newtonsoft.Json.JsonSerializerSettings SerializationSettings { get; private set; } = new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented };// ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Error };

        ///// <summary>
        ///// Blocks the current thread until either a connection is established or the specified time elapses
        ///// </summary>
        ///// <param name="msTimeout"></param>
        ///// <returns></returns>
        //public bool WaitForConnected(int msTimeout = -1)
        //{
        //    return IsConnectedResetEvent.Wait(msTimeout);
        //}

        #region Implicit ISocket
        /// <summary>
        /// True if a connection has been established
        /// </summary>
        public bool IsConnected => IsConnectedResetEvent.IsSet;
        /// <summary>
        /// Exposes the Socket used to send and receive data
        /// </summary>
        public W.Net.Sockets.Socket Socket => _socket;

        /// <summary>
        /// Called when a connection has been established
        /// </summary>
        public Action<IDataSocket, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the connection has been terminated
        /// </summary>
        public Action<IDataSocket, IPEndPoint, Exception> Disconnected { get; set; }
        /// <summary>
        /// Called when data has been received
        /// </summary>
        public Action<IDataSocket, byte[]> RawDataReceived { get; set; }
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        public Action<IDataSocket, byte[]> DataReceived { get; set; }
        /// <summary>
        /// Called after data has been formatted and sent
        /// </summary>
        public Action<IDataSocket, SocketData> DataSent { get; set; }

        /// <summary>
        /// Sends data to the remote
        /// </summary>
        /// <param name="bytes">The data to send</param>
        public virtual ulong Send(byte[] bytes)
        {
            if (!IsConnected)
                throw new InvalidOperationException("A connection has not been established");
            //var bytes = FormatMessageToSend(message);
            //can't log over Tungsten.Net here, otherwise it'll be a recursive stack overflow
            //System.Diagnostics.Debug.WriteLine((IsServerSide ? "Server" : "Client") + " sending " + bytes.Length + " bytes");
            var data = OnSendingData(bytes);
            return _socket.Send(data);
        }
        #endregion

        /// <summary>
        /// Constructs a new StringClient
        /// </summary>
        public Client()
        {
            _socket = new W.Net.Sockets.Socket();
            ConfigureDelegates();
        }
        /// <summary>
        /// Constructs a new StringClient
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        public Client(TcpClient client)
        {
            IsServerSide = true;
            _socket = new W.Net.Sockets.Socket(client);
            ConfigureDelegates(); //8.15.2017 - moved this up a line
            //because this might be a SecureClient, we must call OnConnected instead of handling it here
            OnConnected(client.Client.RemoteEndPoint.As<IPEndPoint>());
            //IsConnectedResetEvent.Set(); //have to set this because we're already connected
        }
        /// <summary>
        /// Destructs the Client and calls Dispose
        /// </summary>
        ~Client()
        {
            Dispose();
        }
        /// <summary>
        /// Disposes the Client and releases resources
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// Calls the Connected multi-cast delegate when a connection has been established
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        protected virtual void OnConnected(IPEndPoint remoteEndPoint)
        {
            IsConnectedResetEvent.Set();
            Connected?.Invoke(this, remoteEndPoint);
        }
        /// <summary>
        /// Calls the Disconnected multi-cast delegate when the connection is terminated
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        /// <param name="e">The Exception object if an exception occurred</param>
        protected virtual void OnDisconnected(IPEndPoint remoteEndPoint, Exception e)
        {
            IsConnectedResetEvent.Reset();
            Disconnected?.Invoke(this, remoteEndPoint, e);
        }
        /// <summary>
        /// Calls the RawDataRecieved multi-cast delegate with potentially compressed data
        /// </summary>
        /// <param name="bytes">The received data</param>
        /// <returns>The raw data, potentially compressed, received from the remote machine</returns>
        /// <remarks>If UseCompression is True, this data will be in a compressed state</remarks>
        protected virtual byte[] OnRawDataReceived(byte[] bytes)
        {
            RawDataReceived?.Invoke(this, bytes);
            return bytes;
        }
        /// <summary>
        /// Calls the DataRecieved multi-cast delegate when data is received from the remote machine
        /// </summary>
        /// <param name="bytes">The data received from the remote machine</param>
        /// <returns>The data received from the remote machine</returns>
        /// <remarks>If UseCompression is True, this data will be in a decompressed state</remarks>
        protected virtual byte[] OnDataReceived(byte[] bytes)
        {
            DataReceived?.Invoke(this, bytes);// message);
            return bytes;
        }
        /// <summary>
        /// May be overridden to format the data as desired before transmission
        /// </summary>
        /// <param name="bytes">The data to send</param>
        /// <returns>The altered data to send</returns>
        protected virtual byte[] OnSendingData(byte[] bytes)
        {
            return bytes;
        }
        /// <summary>
        /// Calls the DataSent multi-cast delegate
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnDataSent(SocketData data)
        {
            DataSent?.Invoke(this, data);
        }
        /// <summary>
        /// Disposes the Client and releases resources
        /// </summary>
        protected virtual void OnDispose()
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
                GC.SuppressFinalize(this);
            }
        }

        private void ConfigureDelegates()
        {
            _socket.Connected += (client, remoteEndPoint) =>
            {
                OnConnected(remoteEndPoint);
            };
            _socket.Disconnected += (socket, remoteEndPoint, e) =>
            {
                OnDisconnected(remoteEndPoint, e);
            };
            _socket.RawDataReceived += (c, bytes) =>
            {
                OnRawDataReceived(bytes);
            };
            _socket.DataReceived += (c, bytes) =>
            {
                OnDataReceived(bytes);
            };
            _socket.DataSent += (c, message) =>
            {
                OnDataSent(message);
            };
        }
    }

    /// <summary>
    /// A generic TCP client to be used with W.Net.Server
    /// </summary>
    public class Client<TMessageType> : Client, IMessageSocket<Client<TMessageType>, TMessageType>// where TClientType: ClientBase<TClientType, TDataType>
    {
        /// <summary>
        /// Called when a connection has been established
        /// </summary>
        public new Action<Client<TMessageType>, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the connection has been terminated
        /// </summary>
        public new Action<Client<TMessageType>, IPEndPoint, Exception> Disconnected { get; set; }
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        public Action<Client<TMessageType>, TMessageType> MessageReceived { get; set; }
        /// <summary>
        /// Called after data has been formatted and sent
        /// </summary>
        public Action<Client<TMessageType>, TMessageType> MessageSent { get; set; }

        /// <summary>
        /// Calls the Connected multi-cast delegate when a connection has been established
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        protected override void OnConnected(IPEndPoint remoteEndPoint)
        {
            base.OnConnected(remoteEndPoint);
            Connected?.Invoke(this, remoteEndPoint);
        }
        /// <summary>
        /// Calls the Disconnected multi-cast delegate when the connection is terminated
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        /// <param name="e">The Exception object if an exception occurred</param>
        protected override void OnDisconnected(IPEndPoint remoteEndPoint, Exception e)
        {
            base.OnDisconnected(remoteEndPoint, e);
            Disconnected?.Invoke(this, remoteEndPoint, e);
        }
        /// <summary>
        /// Calls the MessageRecieved multi-cast delegate when a message is received from the remote machine
        /// </summary>
        /// <param name="bytes">The data received from the remote machine</param>
        /// <returns>The data received from the remote machine</returns>
        /// <remarks>If UseCompression is True, this data will be decompressed</remarks>
        protected override byte[] OnDataReceived(byte[] bytes)
        {
            var result = base.OnDataReceived(bytes);
            var msg = StreamHelpers.FormatReceivedMessage<TMessageType>(result, SerializationSettings);
            MessageReceived?.Invoke(this, msg);
            return result;
        }
        /// <summary>
        /// Calls the MessageSent multi-cast delegate when a message has been sent
        /// </summary>
        /// <param name="data"></param>
        protected override void OnDataSent(SocketData data)
        {
            base.OnDataSent(data);
            var msg = StreamHelpers.FormatReceivedMessage<TMessageType>(data.Data, SerializationSettings);
            MessageSent?.Invoke(this, msg);
        }

        /// <summary>
        /// Sends a message to the remote machine
        /// </summary>
        /// <param name="message">The message to send</param>
        public virtual ulong Send(TMessageType message)
        {
            var bytes = StreamHelpers.FormatMessageToSend<TMessageType>(message, SerializationSettings);
            return base.Send(bytes);
        }

        /// <summary>
        /// Constructs a new StringClient
        /// </summary>
        public Client() : base()
        {
        }
        /// <summary>
        /// Constructs a new StringClient
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        public Client(TcpClient client) : base(client)
        {
        }
    }
}
