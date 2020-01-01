using System;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// Read/Write functionality for Pipe
    /// </summary>
    public static class PipeReadWriteExtensions
    {
        //#region Read/Write
        ///// <summary>
        ///// Write an array of bytes to the pipe
        ///// </summary>
        ///// <param name="pipe">The pipe on which to write data</param>
        ///// <param name="bytes">The bytes to write</param>
        ///// <returns>True if the bytes were sent successfully, otherwise false</returns>
        //public static bool Write(this Pipe pipe, byte[] bytes)
        //{
        //    var result = pipe.Stream.Write(bytes, out Exception e);
        //    if (e != null)
        //        pipe.HandleDisconnection(e);
        //    return result;
        //}
        ///// <summary>
        ///// Write a subset of an array of bytes to the pipe
        ///// </summary>
        ///// <param name="pipe">The pipe on which to write data</param>
        ///// <param name="bytes">The array containing the bytes to write</param>
        ///// <param name="offset">The index of the first byte to write</param>
        ///// <param name="count">The number of bytes to write</param>
        ///// <returns>True if the bytes were sent successfully, otherwise false</returns>
        //public static bool Write(this Pipe pipe, byte[] bytes, int offset, int count)
        //{
        //    var result = pipe.Stream.Write(bytes, offset, count, out Exception e);
        //    if (e != null)
        //        pipe.HandleDisconnection(e);
        //    return result;
        //}
        ///// <summary>
        ///// Asynchronously write an array of bytes to the pipe
        ///// </summary>
        ///// <param name="pipe">The pipe on which to write data</param>
        ///// <param name="bytes">the bytes to write</param>
        ///// <returns>True if the bytes were sent successfully, otherwise false</returns>
        //public static async Task<bool> WriteAsync(this Pipe pipe, byte[] bytes)
        //{
        //    var result = await pipe.Stream.WriteAsync(bytes, e =>
        //    {
        //        if (e != null)
        //            pipe.HandleDisconnection(e);
        //    });
        //    return result;
        //}
        ///// <summary>
        ///// Asynchronously write a subset of an array of bytes to the pipe
        ///// </summary>
        ///// <param name="pipe">The pipe on which to write data</param>
        ///// <param name="bytes">The array containing the bytes to write</param>
        ///// <param name="offset">The index of the first byte to write</param>
        ///// <param name="count">The number of bytes to write</param>
        ///// <returns>True if the bytes were sent successfully, otherwise false</returns>
        //public static async Task<bool> WriteAsync(this Pipe pipe, byte[] bytes, int offset, int count)
        //{
        //    var result = await pipe.Stream.WriteAsync(bytes, offset, count, e =>
        //    {
        //        if (e != null)
        //            pipe.HandleDisconnection(e);
        //    });
        //    return result;
        //}
        ///// <summary>
        ///// Wait for data to be read from the pipe
        ///// </summary>
        ///// <param name="pipe">The pipe from which to read data</param>
        ///// <returns>An array of bytes read from the pipe, or null if the read failed (the pipe was closed)</returns>
        //public static byte[] Read(this Pipe pipe)
        //{
        //    var bytes = pipe.Stream.Read(out Exception e);
        //    if (bytes != null)
        //        pipe.HandleBytesReceived(bytes);
        //    if (e != null)
        //        pipe.HandleDisconnection(e);
        //    return bytes;
        //}
        ///// <summary>
        ///// Asynchronously wait for data to be read from the pipe
        ///// </summary>
        ///// <param name="pipe">The pipe from which to read data</param>
        ///// <returns>An array of bytes read from the pipe, or null if the read failed (the pipe was closed)</returns>
        //public static async Task<byte[]> ReadAsync(this Pipe pipe)
        //{
        //    Exception exception = null;
        //    var bytes = await pipe.Stream.ReadAsync(e => exception = e);
        //    if (bytes != null)
        //        pipe.HandleBytesReceived(bytes);
        //    if (exception != null)
        //        pipe.HandleDisconnection(exception);
        //    return bytes;
        //}
        //#endregion

        //#region Read/Write Generics

        ///// <summary>
        ///// Asynchronously waits for a message to be read from the pipe
        ///// </summary>
        ///// <param name="pipe">The pipe from which to read data</param>
        ///// <returns>The message received or null if the read failed (the pipe was closed)</returns>
        //public static async Task<TMessage> ReadAsync<TMessage>(this Pipe pipe) where TMessage : new()
        //{
        //    TMessage result = default(TMessage);
        //    var bytes = await Helpers.ReadAsync(pipe.Stream);
        //    if (bytes?.Length > 0)
        //        pipe.HandleBytesReceived(bytes);
        //    return result;
        //}
        //#endregion

        private class PipeBuffer<TMessage>
        {
            private W.Threading.Lockers.SpinLocker _locker = new Threading.Lockers.SpinLocker();
            private System.Collections.Generic.List<byte[]> _messageBuffer = new System.Collections.Generic.List<byte[]>();
            private int _totalLength = 0;
            private int _messageLength;

            public int MessageLength
            {
                get
                {
                    return _locker.InLock(() => { return _messageLength; });
                }
                set
                {
                    _locker.InLock(() => { _messageLength = value; });
                }
            }

            ///// <summary>
            ///// Raised when a full message has been received
            ///// </summary>
            //public event Action<PipeBuffer<TMessage>, byte[]> BytesReceived;
            ///// <summary>
            ///// Raises the BytesReceived event
            ///// </summary>
            ///// <param name="bytes">The full message</param>
            //protected void RaiseBytesReceived(PipeBuffer<TMessage> pipeBuffer, byte[] bytes)
            //{
            //    BytesReceived?.Invoke(pipeBuffer, bytes);
            //}
            /// <summary>
            /// Raised when a full message has been received
            /// </summary>
            public event Action<PipeBuffer<TMessage>, TMessage> MessageReceived;
            /// <summary>
            /// Raises the MessageReceived event
            /// </summary>
            /// <param name="message">The full message</param>
            protected void RaiseMessageReceived(PipeBuffer<TMessage> pipeBuffer, TMessage message)
            {
                MessageReceived?.Invoke(pipeBuffer, message);
            }
            /// <summary>
            /// Adds a PipeBufferPart to the full message
            /// </summary>
            /// <param name="bufferPart">The PipeBufferPart recieved from the pipe</param>
            public void Add(byte[] bytes)
            {
                _locker.InLock(() =>
                {
                    if (_messageLength == 0)
                        throw new InvalidOperationException("Message length is 0.  Cannot add bytes to a message of 0 length");
                    if (_totalLength + bytes.Length > _messageLength)
                        throw new InvalidOperationException("The length of additional bytes makes the number of total bytes greater than the specified message length");

                    _messageBuffer.Add(bytes);
                    _totalLength += bytes.Length;
                    if (_totalLength == _messageLength)
                    {
                        var fullMessageBytes = new byte[_messageLength];
                        var offset = 0;
                        for (int t = 0; t < _messageBuffer.Count; t++)
                        {
                            Buffer.BlockCopy(_messageBuffer[t], 0, fullMessageBytes, offset, _messageBuffer[t].Length);
                            offset += _messageBuffer[t].Length;
                        }
                        _messageBuffer.Clear();
                        _messageLength = 0;
                        _totalLength = 0;

                        //RaiseBytesReceived(this, fullMessageBytes);

                        TMessage message = default(TMessage);
                        if (typeof(TMessage) == typeof(byte[]))
                            message = (TMessage)Convert.ChangeType(fullMessageBytes, typeof(TMessage));
                        else
                        {
                            var json = fullMessageBytes.AsString();
                            message = Activator.CreateInstance<TMessage>();
                            Newtonsoft.Json.JsonConvert.PopulateObject(json, message);
                        }
                        RaiseMessageReceived(this, message);
                    }
                });
            }
        }


        #region Chunked Pipe Messages

        /// <summary>
        /// Waits for a message to be read from the pipe
        /// </summary>
        /// <param name="pipe">The pipe from which to read data</param>
        /// <returns>The message received or null if the read failed (the pipe was closed)</returns>
        public static TMessage Read<TMessage>(this Pipe<TMessage> pipe)
        {
            return Read<TMessage>((Pipe)pipe);
        }
        /// <summary>
        /// Waits for a message to be read from the pipe
        /// </summary>
        /// <param name="pipe">The pipe from which to read data</param>
        /// <returns>The message received or null if the read failed (the pipe was closed)</returns>
        public static async Task<TMessage> ReadAsync<TMessage>(this Pipe<TMessage> pipe)
        {
            return await ReadAsync<TMessage>((Pipe)pipe);
        }

        /// <summary>
        /// Write a message to the pipe
        /// </summary>
        /// <param name="pipe">The pipe on which to write data</param>
        /// <param name="message">The message to write</param>
        /// <returns>True if the message was sent successfully, otherwise false</returns>
        public static bool Write<TMessage>(this Pipe<TMessage> pipe, TMessage message)
        {
            if (typeof(TMessage) == typeof(byte[]))
                return Write(pipe, (byte[])Convert.ChangeType(message, typeof(byte[])));

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            return Write(pipe, json.AsBytes());
        }
        /// <summary>
        /// Asynchronously write a message to the pipe
        /// </summary>
        /// <param name="pipe">The pipe on which to write data</param>
        /// <param name="message">The message to write</param>
        /// <returns>True if the message was sent successfully, otherwise false</returns>
        public static async Task<bool> WriteAsync<TMessage>(this Pipe<TMessage> pipe, TMessage message)
        {
            if (typeof(TMessage) == typeof(byte[]))
            {
                var bytes = (byte[])Convert.ChangeType(message, typeof(byte[]));
                var result = await Helpers.WriteAsync(pipe.Stream, BitConverter.GetBytes(bytes.Length));
                if (result)
                    result = await Helpers.WriteAsync(pipe.Stream, bytes);
                return result;
            }
            else
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                return await WriteAsync(pipe, json.AsBytes());
            }
        }

        #endregion

        private static TMessage Read<TMessage>(this Pipe pipe)
        {
            TMessage result = default(TMessage);
            try
            {
                var bytes = Helpers.Read(pipe.Stream);
                if (bytes?.Length == 4) //only accept a proper header
                {
                    var exit = false;
                    var buffer = new PipeBuffer<TMessage>();
                    buffer.MessageLength = BitConverter.ToInt32(bytes, 0);
                    buffer.MessageReceived += (b, m) =>
                    {
                        //pipe.HandleBytesReceived(m);
                        if (typeof(TMessage) == typeof(byte[]))
                            result = (TMessage)Convert.ChangeType(m, typeof(TMessage));
                        else
                        {
                            result = m;
                            //result = (TMessage)Activator.CreateInstance(typeof(TMessage));
                            //Newtonsoft.Json.JsonConvert.PopulateObject(m.AsString(), result);
                        }
                        exit = true;
                    };

                    while (!exit)
                    {
                        bytes = Helpers.Read(pipe.Stream);
                        if (bytes != null)
                            buffer.Add(bytes);
                    };
                }
                return result;
            }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (System.IO.IOException) { }
            catch (ObjectDisposedException) { }
#if NET45
            catch (System.Threading.ThreadAbortException) { System.Threading.Thread.ResetAbort(); }
#endif
            catch (InvalidOperationException) { } //pipe is in a disconnected state
            catch (Exception) { System.Diagnostics.Debugger.Break(); }
            return result;
        }
        private static async Task<TMessage> ReadAsync<TMessage>(this Pipe pipe)
        {
            TMessage result = default(TMessage);
            try
            {
                var bytes = await Helpers.ReadAsync(pipe.Stream);
                if (bytes?.Length == 4) //only accept a proper header
                {
                    var exit = false;
                    var buffer = new PipeBuffer<TMessage>();
                    buffer.MessageLength = BitConverter.ToInt32(bytes, 0);
                    buffer.MessageReceived += (b, m) =>
                    {
                        //pipe.HandleBytesReceived(m);
                        if (typeof(TMessage) == typeof(byte[]))
                            result = (TMessage)Convert.ChangeType(m, typeof(byte[]));
                        else
                            result = m;
                        //result = (TMessage)Activator.CreateInstance(typeof(TMessage));
                        //Newtonsoft.Json.JsonConvert.PopulateObject(m.AsString(), result);
                        exit = true;
                    };

                    while (!exit)
                    {
                        bytes = await Helpers.ReadAsync(pipe.Stream);
                        if (bytes != null)
                            buffer.Add(bytes);
                    };
                }
            }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (System.IO.IOException) { }
            catch (ObjectDisposedException) { }
#if NET45
            catch (System.Threading.ThreadAbortException) { System.Threading.Thread.ResetAbort(); }
#endif
            catch (InvalidOperationException) { } //pipe is in a disconnected state
            catch (Exception) { System.Diagnostics.Debugger.Break(); }
            return result;
        }

        private static bool Write(this Pipe pipe, byte[] bytes)
        {
            if (Helpers.Write(pipe.Stream, BitConverter.GetBytes(bytes.Length)))
                return Helpers.Write(pipe.Stream, bytes);
            return false;
        }
        private static async Task<bool> WriteAsync(this Pipe pipe, byte[] bytes)
        {
            var result = await Helpers.WriteAsync(pipe.Stream, BitConverter.GetBytes(bytes.Length));
            if (result)
                result = await Helpers.WriteAsync(pipe.Stream, bytes);
            return result;
        }
    }
}