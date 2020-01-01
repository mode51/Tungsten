using System;
using System.Collections;
using System.IO.Pipes;
using System.Linq;
using System.Text;

namespace W.IO.Pipes
{
    /// <summary>
    /// The untyped base Pipe class
    /// </summary>
    public abstract class Pipe : IDisposable
    {
        private PipeStream _stream;

        private static int? _inBufferSize = null;
        private static int? _outBufferSize = null;
        public static int? InBufferSize
        {
            get
            {
                if (_inBufferSize == null)
                    _inBufferSize = Helpers.GetInBufferSize();
                return _inBufferSize;
            }
            internal set
            {
                _inBufferSize = value;
            }
        }
        public static int? OutBufferSize
        {
            get
            {
                if (_outBufferSize == null)
                    _outBufferSize = Helpers.GetOutBufferSize();
                return _outBufferSize;
            }
            internal set
            {
                _outBufferSize = value;
            }
        }

        private W.Threading.Lockers.Disposer _disposer = new Threading.Lockers.Disposer();
        /// <summary>
        /// The PipeStream on which to send and receive data
        /// </summary>
        public PipeStream Stream { get { return _stream; } protected set { _stream = value; if (InBufferSize == null) InBufferSize = Helpers.GetInBufferSize(_stream); if (OutBufferSize == null) OutBufferSize = Helpers.GetOutBufferSize(_stream); } }
        /// <summary>
        /// A unique id for this Pipe
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Raised when the pipe has disconnected
        /// </summary>
        public event Action<Pipe, Exception> Disconnected;

        /// <summary>
        /// Raises the Disconnected event.  Pass in an exception if desired.
        /// </summary>
        /// <param name="sender">A reference to the caller</param>
        /// <param name="e">An exception if one was captured</param>
        protected void RaiseDisconnection(Pipe sender, Exception e)
        {
            Disconnected?.Invoke(sender, e);
        }
        ///// <summary>
        ///// Raises the BytesReceived event
        ///// </summary>
        ///// <param name="bytes">The bytes recieved from the pipe</param>
        //protected virtual void OnBytesReceived(byte[] bytes) { }
        ///// <summary>
        ///// Continuously waits for data from the pipe.  The BytesReceived event is raised when data arrives, then immediately waits for more data.
        ///// </summary>
        //protected virtual void OnListen() { }
        ///// <summary>
        ///// After the next bytes received, stops waiting for data
        ///// </summary>
        //protected virtual void OnStopListening() { }

        #region Listen
        protected volatile bool _shouldListen = false;
        /// <summary>
        /// Continuously waits for data from the pipe.  The BytesReceived event is raised when data arrives, then immediately waits for more data.
        /// </summary>
        protected virtual void OnListen()
        {
            //if (_shouldListen)
            //    return; //disallow re-entrance
            //_shouldListen = true;
            ////Task.Run is faster than Task.StartNew when Task.StartNew specified LongRunning
            //System.Threading.Tasks.Task.Run(() =>
            //{
            //    Exception exception = null;
            //    while (_shouldListen)
            //    {
            //        //the next line exits when a read succeeds and when the stream is closed (via exception)
            //        var bytes = Stream.ReadAsync(e => { exception = e; }).Result;
            //        if (bytes != null)
            //        {
            //            OnBytesReceived(bytes);
            //            //System.Threading.Thread.Sleep(0);
            //        }
            //        if (bytes == null || exception != null)
            //            break;
            //    }
            //    RaiseDisconnection(this, null);
            //    _shouldListen = false;
            //}).ConfigureAwait(false);
        }
        /// <summary>
        /// After the next bytes received, stops waiting for data
        /// </summary>
        protected virtual void OnStopListening()
        {
            _shouldListen = false;
        }
        #endregion

        /// <summary>
        /// Continuously waits for data from the pipe.  The BytesReceived event is raised when data arrives, then immediately waits for more data.
        /// </summary>
        public void Listen() { OnListen(); }
        /// <summary>
        /// After the next bytes received, stops waiting for data
        /// </summary>
        public void StopListening() { OnStopListening(); }

        /// <summary>
        /// Disconnects and disposes the pipe
        /// </summary>
        protected virtual void OnDispose()
        {
            StopListening();
            if (Stream != null)
            {
                Helpers.DisconnectAndDispose(Stream);
                Stream = null;
            }
        }

        #region Internal Members
        internal void HandleDisconnection(Exception e)
        {
            RaiseDisconnection(this, e);
        }
        //internal void HandleBytesReceived(byte[] bytes)
        //{
        //    OnBytesReceived(bytes);
        //}
        #endregion

        /// <summary>
        /// Disconnects and disposes the pipe
        /// </summary>
        public void Dispose()
        {
            _disposer.Dispose(OnDispose);
        }
    }

    /// <summary>
    /// The base generic Pipe class
    /// </summary>
    public abstract class Pipe<TMessage> : Pipe
    {
        //private PipeBuffer<TMessage> _buffer = new PipeBuffer<TMessage>();

        /// <summary>
        /// Raised when a message has been received
        /// </summary>
        /// <remarks>This event will only be raised if TMessage != byte[]</remarks>
        public event Action<Pipe<TMessage>, TMessage> MessageReceived;
        /// <summary>
        /// Called by the PipeExtensions extension methods to raise the MessageReceived event
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="message"></param>
        protected void RaiseMessageReceived(Pipe<TMessage> pipe, TMessage message)
        {
            MessageReceived?.Invoke(pipe, message);
        }
        ///// <summary>
        ///// Raises the BytesReceived event
        ///// </summary>
        ///// <param name="bytes">The bytes recieved from the pipe</param>
        //protected override void OnBytesReceived(byte[] bytes)
        //{
        //    //if (bytes == null)
        //    //    return;
        //    ////if (_buffer.MessageLength == 0)
        //    ////    _buffer.MessageLength = BitConverter.ToInt32(bytes, 0);
        //    ////else
        //    ////    _buffer.Add(bytes);


        //    ////if (typeof(TMessage) == typeof(byte[]))
        //    ////    RaiseMessageReceived(this, (TMessage)Convert.ChangeType(bytes, typeof(TMessage)));
        //    ////else
        //    ////{
        //    //try
        //    //{
        //    //    var json = bytes.AsString();
        //    //    var message = (TMessage)Activator.CreateInstance(typeof(TMessage));
        //    //    Newtonsoft.Json.JsonConvert.PopulateObject(json, message);
        //    //    RaiseMessageReceived(this, message);
        //    //}
        //    //catch (Exception e) //TODO: decide what to do about exceptions here
        //    //{
        //    //    System.Diagnostics.Debug.WriteLine(e.Message);
        //    //    System.Diagnostics.Debugger.Break();
        //    //}
        //    //}
        //}
        /// <summary>
        /// Continuously waits for data from the pipe.  The BytesReceived event is raised when data arrives, then immediately waits for more data.
        /// </summary>
        protected override void OnListen()
        {
            if (_shouldListen)
                return; //disallow re-entrance
            _shouldListen = true;
            //Task.Run is faster than Task.StartNew when Task.StartNew specified LongRunning
            System.Threading.Tasks.Task.Run(() =>
            {
                while (_shouldListen)
                {
                    //the next line exits when a read succeeds and when the stream is closed (via exception)
                    var result = this.Read<TMessage>();
                    if (result == null)
                        break;
                    RaiseMessageReceived(this, result);
                }
                RaiseDisconnection(this, null);
                _shouldListen = false;
            }).ConfigureAwait(false);
        }

        public Pipe()
        {
            //_buffer.MessageReceived += (b, m) => { RaiseMessageReceived(this, m); };
        }
    }
}
