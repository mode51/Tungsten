//using System;
//using System.Collections.Generic;

//namespace W.IO.Pipes
//{
//    internal class PipeBuffer<TMessage>
//    {
//        private W.Threading.Lockers.SpinLocker _locker = new Threading.Lockers.SpinLocker();
//        private List<byte[]> _messageBuffer = new List<byte[]>();
//        private int _totalLength = 0;
//        private int _messageLength;

//        public int MessageLength
//        {
//            get
//            {
//                return _locker.InLock(() => { return _messageLength; });
//            }
//            set
//            {
//                _locker.InLock(() => { _messageLength = value; });
//            }
//        }

//        ///// <summary>
//        ///// Raised when a full message has been received
//        ///// </summary>
//        //public event Action<PipeBuffer<TMessage>, byte[]> BytesReceived;
//        ///// <summary>
//        ///// Raises the BytesReceived event
//        ///// </summary>
//        ///// <param name="bytes">The full message</param>
//        //protected void RaiseBytesReceived(PipeBuffer<TMessage> pipeBuffer, byte[] bytes)
//        //{
//        //    BytesReceived?.Invoke(pipeBuffer, bytes);
//        //}
//        /// <summary>
//        /// Raised when a full message has been received
//        /// </summary>
//        public event Action<PipeBuffer<TMessage>, TMessage> MessageReceived;
//        /// <summary>
//        /// Raises the MessageReceived event
//        /// </summary>
//        /// <param name="message">The full message</param>
//        protected void RaiseMessageReceived(PipeBuffer<TMessage> pipeBuffer, TMessage message)
//        {
//            MessageReceived?.Invoke(pipeBuffer, message);
//        }
//        /// <summary>
//        /// Adds a PipeBufferPart to the full message
//        /// </summary>
//        /// <param name="bufferPart">The PipeBufferPart recieved from the pipe</param>
//        public void Add(byte[] bytes)
//        {
//            _locker.InLock(() =>
//            {
//                if (_messageLength == 0)
//                    throw new InvalidOperationException("Message length is 0.  Cannot add bytes to a message of 0 length");
//                if (_totalLength + bytes.Length > _messageLength)
//                    throw new InvalidOperationException("The length of additional bytes makes the number of total bytes greater than the specified message length");

//                _messageBuffer.Add(bytes);
//                _totalLength += bytes.Length;
//                if (_totalLength == _messageLength)
//                {
//                    var fullMessageBytes = new byte[_messageLength];
//                    var offset = 0;
//                    for (int t = 0; t < _messageBuffer.Count; t++)
//                    {
//                        Buffer.BlockCopy(_messageBuffer[t], 0, fullMessageBytes, offset, _messageBuffer[t].Length);
//                        offset += _messageBuffer[t].Length;
//                    }
//                    _messageBuffer.Clear();
//                    _messageLength = 0;
//                    _totalLength = 0;

//                    //RaiseBytesReceived(this, fullMessageBytes);

//                    TMessage message = default(TMessage);
//                    if (typeof(TMessage) == typeof(byte[]))
//                        message = (TMessage)Convert.ChangeType(fullMessageBytes, typeof(TMessage));
//                    else
//                    {
//                        var json = fullMessageBytes.AsString();
//                        message = Activator.CreateInstance<TMessage>();
//                        Newtonsoft.Json.JsonConvert.PopulateObject(json, message);
//                    }
//                    RaiseMessageReceived(this, message);
//                }
//            });
//        }
//    }
//}
