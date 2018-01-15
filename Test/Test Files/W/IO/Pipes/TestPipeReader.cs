using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W;
using W.AsExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using System.Collections;
#if NET45
using System.Security.Principal;
#endif
using System.Runtime.InteropServices;
using W.DelegateExtensions;
using System.ComponentModel;

namespace W.Tests
{
//    internal enum PipeMessageTypeEnum : int
//    {
//        None,
//        Post,
//        Request,
//        Response
//    }
//    //internal class PipeHeader
//    //{
//    //    public PipeMessageTypeEnum MessageType { get; set; }
//    //    public int Length { get; set; }

//    //    //public byte[] AsBytes()
//    //    //{
//    //    //    return GetBytes(MessageType, Length);
//    //    //}
//    //    //public override string ToString()
//    //    //{
//    //    //    return string.Format("Type: {0}, Length: {1}", MessageType, Length);
//    //    //}
//    //    public static PipeHeader FromBytes(byte[] bytes)
//    //    {
//    //        var result = new PipeHeader();
//    //        result.MessageType = (PipeMessageTypeEnum)BitConverter.ToInt32(bytes, 0);
//    //        result.Length = BitConverter.ToInt32(bytes, 4);
//    //        return result;
//    //    }
//    //    //public static PipeHeader Create(PipeMessageTypeEnum type, int length)
//    //    //{
//    //    //    return new PipeHeader() { MessageType = type, Length = length };
//    //    //}
//    //    //public static byte[] GetBytes(PipeMessageTypeEnum type, int length)
//    //    //{
//    //    //    var result = new byte[8];
//    //    //    System.Buffer.BlockCopy(BitConverter.GetBytes((int)type), 0, result, 0, 4);
//    //    //    System.Buffer.BlockCopy(BitConverter.GetBytes(length), 0, result, 4, 4);
//    //    //    return result;
//    //    //}
//    //}
//    //internal class PipeMessage
//    //{
//    //    public PipeHeader Header { get; set; }
//    //    public byte[] Data { get; set; }
//    //    public override string ToString()
//    //    {
//    //        return string.Format("{0}, Data: {1}", Header, Data.AsString());
//    //    }
//    //}
//    public enum MessageReaderResultEnum
//    {
//        None,
//        MessageReady,
//        NoMessage,
//        Disconnected
//    }
//    internal class PipeReader
//    {
//        private PipeStream _stream;

//        public int BytesRead { get; private set; }
//        public int TotalBytesRead { get; private set; }
//        public byte[] Buffer { get; private set; }
//        public MessageReaderResultEnum Result { get; private set; }

//        private void SetResult(bool timedOut, int bytesRead)
//        {
//            //try
//            //{
//                if (timedOut)
//                {
//                    BytesRead = 0;
//                    Result = MessageReaderResultEnum.NoMessage;
//                }
//                else
//                {
//                    BytesRead = bytesRead;
//                    TotalBytesRead += BytesRead;
//                    if (TotalBytesRead == Buffer.Length)
//                        Result = MessageReaderResultEnum.MessageReady;
//                }
//            //}
//            //finally
//            //{
//            //    //_locker.Release();
//            //}
//        }
//        public async Task ReadAsync(int msTimeout)
//        {
//            if (Result == MessageReaderResultEnum.MessageReady || Result == MessageReaderResultEnum.Disconnected)
//                return;
//            Result = MessageReaderResultEnum.None;
//            try
//            {
//#if NET45
//                var _task = Task.Factory.FromAsync(_stream.BeginRead, _stream.EndRead, Buffer, TotalBytesRead, Buffer.Length - TotalBytesRead, _stream);
//                await _task;
//                await _task.ContinueWith(t =>
//                {
//                    SetResult(_task.Result == 0, _task.Result);
//                });
//#else
//                var _task = _stream.ReadAsync(Buffer, TotalBytesRead, Buffer.Length - TotalBytesRead, new CancellationTokenSource(msTimeout).Token);
//                await _task;
//                SetResult(_task.IsCanceled, _task.Result);
//#endif
//            }
//            catch (TimeoutException) { Result = MessageReaderResultEnum.NoMessage; }
//            catch (OperationCanceledException) { Result = MessageReaderResultEnum.NoMessage; }
//            catch (System.IO.IOException) { Result = MessageReaderResultEnum.Disconnected; }
//            catch (ObjectDisposedException) { Result = MessageReaderResultEnum.Disconnected; }
//        }

//        public PipeReader(PipeStream stream, int numberOfBytesToRead)
//        {
//            _stream = stream;
//            Buffer = new byte[numberOfBytesToRead];
//        }
//    }
//    internal class MessageReader
//    {
//        private PipeStream _stream;

//        public PipeReader HeaderInfo { get; private set; }
//        public PipeReader Message { get; private set; }

//        public PipeMessageTypeEnum MessageType { get; set; }
//        public int Length { get; set; }

//        public async Task<MessageReaderResultEnum> ReadMessageAsync()
//        {
//            if (Message == null) //need to read/finish reading the header
//            {
//                await HeaderInfo.ReadAsync(10);
//                if (HeaderInfo.Result == MessageReaderResultEnum.MessageReady)
//                {
//                    MessageType = (PipeMessageTypeEnum)BitConverter.ToInt32(HeaderInfo.Buffer, 0);
//                    Length = BitConverter.ToInt32(HeaderInfo.Buffer, 4);
//                    Message = new PipeReader(_stream, Length);
//                }
//                return HeaderInfo.Result;
//            }

//            if (Message.Result == MessageReaderResultEnum.MessageReady || Message.Result == MessageReaderResultEnum.Disconnected)
//                return Message.Result;
//            await Message.ReadAsync(10);
//            return Message.Result;
//        }
//        public void Reset()
//        {
//            HeaderInfo = new PipeReader(_stream, 8);
//            MessageType = PipeMessageTypeEnum.None;
//            Length = 0;
//            Message = null;
//        }
//        public MessageReader(PipeStream stream)
//        {
//            _stream = stream;
//            Reset();
//        }
//    }
//    public sealed class BackgroundPipeReader : IDisposable
//    {
//        private PipeStream _stream;
//        private W.Threading.ThreadMethod _thread;
//        private CancellationTokenSource _cts = new CancellationTokenSource();
//        private ManualResetEventSlim _readOk = new ManualResetEventSlim(false);
//        private ManualResetEventSlim _doneReading = new ManualResetEventSlim(false);
//        //private byte[] _data = null;
//        //private int _dataLength = 0;
//        //private byte[] _buffer = null;
//        //private PipeHeader _header;
//        private bool _started = false;
//        private bool _disconnected = false;
//        private MessageReader _reader;

//        public event Action<BackgroundPipeReader> Disconnected;
//        public event Action<BackgroundPipeReader, byte[]> MessageReceived;

//        private void RaiseDisconnected()
//        {
//            Disconnected?.Invoke(this);
//        }
//        private void RaiseMessageReceived(byte[] data)
//        {
//            MessageReceived?.Invoke(this, data);
//        }

//        //private async Task BlanketReadBytesAsync(CancellationToken token)
//        //{
//        //    _bytesRead.Value = await _stream.ReadAsync(_buffer, 0, _buffer.Length, token);
//        //}
//        //private async void BlanketReadBytesBase()
//        //{
//        //    //this method cannot be async, but requires an async call for the timeout to work
//        //    try
//        //    {
//        //        //Task.Run(async () =>
//        //        //{
//        //            await BlanketReadBytesAsync(new CancellationTokenSource(100).Token);
//        //        //}).Wait(100);
//        //        //bytesRead = await Task.Run(async () =>
//        //        //{
//        //        //    return await stream.ReadAsync(buffer, 0, numberOfBytes);
//        //        //}, new CancellationTokenSource(msTimeout).Token);

//        //        //stream.ReadAsync(buffer, 0, numberOfBytes).ContinueWith(task =>
//        //        //{
//        //        //    bytesRead = task.Result;
//        //        //    Console.WriteLine("Read {0} bytes", bytesRead);
//        //        //}).Wait(msTimeout);
//        //    }
//        //    catch (OperationCanceledException)
//        //    {
//        //    }
//        //}
//        //private void Read()
//        //{
//        //    var buffer = new byte[PipeHelpers.GetInBufferSize(_stream)];
//        //    try
//        //    {
//        //        _stream.ReadAsync(buffer, 0, buffer.Length).ContinueWith(task =>
//        //        {
//        //            lock (_dataLock)
//        //            {
//        //                if (!task.IsCanceled && !task.IsFaulted && task.Result > 0)
//        //                {
//        //                    Console.WriteLine("{0}: Read {1} bytes", this.GetType().Name, task.Result);
//        //                    var curlen = _data.Length;
//        //                    //adjust array size
//        //                    Array.Resize(ref _data, _data.Length + task.Result);
//        //                    //copy the data read from the pipe
//        //                    System.Buffer.BlockCopy(buffer, 0, _data, curlen, task.Result);

//        //                    AnalyzeData();
//        //                }
//        //            }
//        //        }).Wait(10);
//        //    }
//        //    catch (OperationCanceledException) { } //ignore
//        //    catch (ObjectDisposedException) //disconnected
//        //    {
//        //        _disconnected = true;
//        //        Disconnected?.Invoke(this);
//        //    }
//        //}
//        //private void Read2()
//        //{
//        //    //var buffer = new byte[PipeHelpers.GetInBufferSize(_stream)];
//        //    try
//        //    {
//        //        lock (_buffer)
//        //        {
//        //            _stream.ReadAsync(_buffer, 0, _buffer.Length).ContinueWith(task =>
//        //            {
//        //                lock (_dataLock)
//        //                {
//        //                    if (!task.IsCanceled && !task.IsFaulted && task.Result > 0)
//        //                    {
//        //                        Console.WriteLine("{0}: Read {1} bytes", this.GetType().Name, task.Result);

//        //                        var bytesRead = task.Result;

//        //                        if (_header == null && bytesRead >= 8)
//        //                        {
//        //                            var headerBuffer = ArrayMethods.PeekStart(_buffer, 8);
//        //                            _header = PipeHeader.FromBytes(headerBuffer);
//        //                            _data = new byte[_header.Length];
//        //                            bytesRead -= 8;
//        //                            if (bytesRead > 0)
//        //                            {
//        //                                System.Buffer.BlockCopy(_buffer, 8, _data, _dataLength, bytesRead);
//        //                                _dataLength += bytesRead;
//        //                            }
//        //                        }
//        //                        else if (bytesRead > 0)
//        //                        {
//        //                            System.Buffer.BlockCopy(_buffer, 0, _data, _dataLength, bytesRead);
//        //                            _dataLength += bytesRead;
//        //                        }
//        //                        if (_dataLength >= _header.Length)
//        //                        {
//        //                            var message = ArrayMethods.TakeFromStart(ref _data, _header.Length);
//        //                            MessageReceived?.Invoke(this, new PipeMessage() { Header = _header, Data = message });
//        //                            _header = null;
//        //                        }
//        //                    }
//        //                }
//        //            }).Wait(10);
//        //        }
//        //    }
//        //    catch (OperationCanceledException) { } //ignore
//        //    catch (ObjectDisposedException) //disconnected
//        //    {
//        //        _disconnected = true;
//        //        Disconnected?.Invoke(this);
//        //    }
//        //    finally
//        //    {
//        //    }
//        //}
//        //private void AnalyzeData2()
//        //{
//        //    if (_dataLength > 8)
//        //    {
//        //        _nextMessage = new PipeMessage() { Header = PipeHeader.FromBytes(_data) };
//        //        _nextMessage.Data = new byte[_nextMessage.Header.Length];
//        //    }

//        //    //if a header is available, check for full message
//        //    while (_data.Length > 8)
//        //    {
//        //        var messageLength = BitConverter.ToInt32(_data, 4); //peek the header's data length
//        //        if (_data.Length < messageLength + 8)
//        //            break;

//        //        // we have at least one full message
//        //        //take the message header and message
//        //        var data = ArrayMethods.TakeFromStart(ref _data, messageLength + 8);
//        //        var headerBuffer = ArrayMethods.TakeFromStart(ref data, 8);
//        //        var header = PipeHeader.FromBytes(headerBuffer);
//        //        MessageReceived?.Invoke(this, new PipeMessage() { Header = header, Data = data });
//        //        _dataLength -= messageLength + 8;
//        //    }
//        //}
//        //private async Task Read3()
//        //{
//        //    //var buffer = new byte[PipeHelpers.GetInBufferSize(_stream)];
//        //    try
//        //    {
//        //        await _stream.ReadAsync(_buffer, 0, _buffer.Length).ContinueWith(task =>
//        //        {
//        //            lock (_dataLock)
//        //            {
//        //                if (!task.IsCanceled && !task.IsFaulted && task.Result > 0)
//        //                {
//        //                    Console.WriteLine("{0}: Read {1} bytes", this.GetType().Name, task.Result);
//        //                    var bytesRead = task.Result;
//        //                    //resize _data and append the bytes read
//        //                    Array.Resize(ref _data, _data.Length + bytesRead);
//        //                    System.Buffer.BlockCopy(_buffer, 0, _data, _dataLength, bytesRead);
//        //                    _dataLength += bytesRead;
//        //                }
//        //            }
//        //        });

//        //    }
//        //    catch (OperationCanceledException) { } //ignore
//        //    catch (ObjectDisposedException) //disconnected
//        //    {
//        //        _disconnected = true;
//        //        Disconnected?.Invoke(this);
//        //    }
//        //    finally
//        //    {
//        //    }
//        //}
//        //private void BlanketReadBytes()
//        //{
//        //    BlanketReadBytesBase();
//        //    if (_bytesRead.Value > 0)
//        //    {
//        //        //lock (_dataLock)
//        //        {
//        //            var curlen = _data.Length;
//        //            //adjust array size
//        //            Array.Resize(ref _data, _data.Length + _bytesRead.Value);
//        //            //copy the data read from the pipe
//        //            System.Buffer.BlockCopy(_buffer, 0, _data, curlen, _bytesRead.Value);
//        //        }
//        //    }
//        //}
//        //private void AnalyzeData()
//        //{
//        //    //lock (_dataLock)
//        //    {
//        //        if (_data.Length > 8 && _header == null)
//        //        {
//        //            var headerBuffer = ArrayMethods.TakeFromStart(ref _data, 8);
//        //            _header = PipeHeader.FromBytes(headerBuffer);
//        //        }
//        //        if (_header == null)
//        //            return;
//        //        if (_data.Length >= _header.Length)
//        //        {
//        //            var message = ArrayMethods.TakeFromStart(ref _data, _header.Length);
//        //            MessageReceived?.Invoke(this, new PipeMessage() { Header = _header, Data = message });
//        //            _header = null;
//        //        }
//        //    }
//        //}
//        //private async Task Read4()
//        //{
//        //    if (_nextMessage == null)
//        //        await ReadHeader();
//        //    else
//        //        await ReadMessage();
//        //}
//        //private async Task<MessageReader> ReadAsync(byte[] buffer, int offset, int bytesToRead)
//        //{
//        //    var reader = new MessageReader(_stream);
//        //    await reader.ReadAsync(buffer, offset, bytesToRead, 10);
//        //    return reader;
//        //    //return await _stream.ReadAsync(buffer, offset, bytesToRead, new CancellationTokenSource(10).Token);//.ConfigureAwait(false);
//        //}
//        //private int Read(byte[] buffer, int offset, int bytesToRead)
//        //{
//        //    var bytesRead = 0;
//        //    try
//        //    {
//        //        var task = ReadAsync(buffer, offset, bytesToRead);
//        //        task.Wait(10);
//        //        bytesRead = task.Result;
//        //        //if (bytesRead > 0)
//        //        //    System.Diagnostics.Debug.WriteLine("Read {0} bytes", bytesRead);
//        //    }
//        //    catch (OperationCanceledException) { } //ignore
//        //    catch (ObjectDisposedException) //disconnected
//        //    {
//        //        _disconnected = true;
//        //        RaiseDisconnected();
//        //    }
//        //    finally
//        //    {
//        //    }
//        //    return bytesRead;
//        //}
//        //private async Task ReadHeader()
//        //{
//        //    var buffer = new byte[8];
//        //    try
//        //    {
//        //        //only read the bytes we need
//        //        var reader = await ReadAsync(buffer, 0, buffer.Length - _bytesRead);
//        //        //var bytesRead = ReadAsync(buffer, _bytesRead, buffer.Length - _bytesRead).Result;
//        //        if (reader.BytesRead > 0)
//        //        {
//        //            System.Buffer.BlockCopy(buffer, 0, _headerBuffer, _bytesRead, reader.BytesRead);
//        //            //System.Diagnostics.Debug.WriteLine("{0}: Read {1} bytes", this.GetType().Name, bytesRead);
//        //            _bytesRead += reader.BytesRead;
//        //            if (_bytesRead == 8)
//        //            {
//        //                _nextMessage = new PipeMessage() { Header = PipeHeader.FromBytes(_headerBuffer) };
//        //                _nextMessage.Data = new byte[_nextMessage.Header.Length];
//        //                _bytesRead = 0;
//        //            }
//        //        }
//        //        //await _stream.ReadAsync(buffer, _bytesRead, _headerBuffer.Length - _bytesRead, new CancellationTokenSource(10).Token).ContinueWith(task =>
//        //        //{
//        //        //    //lock (_dataLock)
//        //        //    {
//        //        //        if (!task.IsCanceled && !task.IsFaulted && task.Result > 0)
//        //        //        {
//        //        //            System.Buffer.BlockCopy(buffer, 0, _headerBuffer, _bytesRead, task.Result);
//        //        //            Console.WriteLine("{0}: Read {1} bytes", this.GetType().Name, task.Result);
//        //        //            _bytesRead += task.Result;
//        //        //            if (_bytesRead == 8)
//        //        //            {
//        //        //                _nextMessage = new PipeMessage() { Header = PipeHeader.FromBytes(_headerBuffer) };
//        //        //                _nextMessage.Data = new byte[_nextMessage.Header.Length];
//        //        //                _bytesRead = 0;
//        //        //            }
//        //        //        }
//        //        //    }
//        //        //});
//        //    }
//        //    catch (OperationCanceledException) { } //ignore
//        //    catch (ObjectDisposedException) //disconnected
//        //    {
//        //        _disconnected = true;
//        //        RaiseDisconnected();
//        //    }
//        //    finally
//        //    {
//        //    }
//        //}
//        //private async Task ReadMessage()
//        //{
//        //    var reader = await ReadAsync(_nextMessage.Data, _bytesRead, _nextMessage.Header.Length - _bytesRead);
//        //    if (!reader.Timedout)
//        //        _bytesRead += reader.BytesRead;
//        //    //var reader = new Reader(_stream);
//        //    //await reader.ReadAsync(_nextMessage.Data, _bytesRead, _nextMessage.Header.Length - _bytesRead, 10).ContinueWith(task =>
//        //    //{
//        //    //    if (!task.IsCanceled && !task.IsFaulted && reader.BytesRead > 0)
//        //    //    {
//        //    //        _bytesRead += reader.BytesRead;
//        //    //    }
//        //    //});

//        //    //try
//        //    //{
//        //    //    //if (_disconnected)
//        //    //    //    return;
//        //    //    //only read the bytes we need
//        //    //    await ReadAsync(_nextMessage.Data, _bytesRead, _nextMessage.Header.Length - _bytesRead).ContinueWith(task =>
//        //    //    //await _stream.ReadAsync(_nextMessage.Data, _bytesRead, _nextMessage.Header.Length - _bytesRead, new CancellationTokenSource(10).Token).ContinueWith(task =>
//        //    //    {
//        //    //        //lock (_dataLock)
//        //    //        {
//        //    //            if (!task.IsCanceled && !task.IsFaulted && task.Result > 0)
//        //    //            {
//        //    //                System.Diagnostics.Debug.WriteLine("{0}: Read {1} bytes", this.GetType().Name, task.Result);
//        //    //                _bytesRead += task.Result;
//        //    //            }
//        //    //        }
//        //    //    });
//        //    //}
//        //    //catch (OperationCanceledException) { } //ignore
//        //    //catch (ObjectDisposedException) //disconnected
//        //    //{
//        //    //    _disconnected = true;
//        //    //    RaiseDisconnected();
//        //    //}
//        //    //finally
//        //    //{
//        //    //}
//        //}
//        private async void ThreadProc(params object[] args)
//        {
//            try
//            {
//                while (_readOk.Wait(-1) && !_cts.IsCancellationRequested)
//                {
//                    if (!_stream.IsConnected)
//                    {
//                        Trace.WriteLine("ThreadProc exiting due to disconnect");
//                        break;
//                    }
//                    _doneReading.Reset();
//                    if (_reader.Message?.Result != MessageReaderResultEnum.MessageReady)
//                        await _reader.ReadMessageAsync();
//                    if (_reader.Message?.Result == MessageReaderResultEnum.MessageReady)
//                    {
//                        RaiseMessageReceived(_reader.Message.Buffer);
//                        _reader.Reset();
//                    }
//                    _doneReading.Set();
//                }
//            }
//            //catch (Exception e)
//            //{
//            //    RaiseDisconnected();
//            //}
//#if NET45
//            catch (ThreadAbortException) { System.Threading.Thread.ResetAbort(); } //ignore
//#endif
//            catch (ObjectDisposedException) { } //ignore
//            finally
//            {
//                _disconnected = true;
//                _doneReading.Set();
//                RaiseDisconnected();
//            }
//        }

//        public async Task Start()
//        {
//            if (_started)
//                throw new InvalidOperationException("Cannot be restarted.  Use Pause and Resume.");
//            _started = true;
//            _thread.Start();
//            await Resume();
//            _doneReading.Set();
//        }
//        public async Task Pause()
//        {
//            if (!_started)
//                return;
//            await Task.Run(() =>
//            {
//                _readOk.Reset();
//                _doneReading.Wait();
//            });
//        }
//        public async Task Resume()
//        {
//            if (!_started)
//                return;
//            await Task.Run(() =>
//            {
//                _readOk.Set();
//            });
//        }
//        public async void Dispose()
//        {
//            _cts.Cancel();
//            await Resume(); //allow the threadproc to exit
//            _thread.Wait();
//            _cts.Dispose();
//            _readOk.Dispose();
//            _doneReading.Dispose();
//        }
//        public BackgroundPipeReader(PipeStream stream)
//        {
//            _stream = stream;
//            _reader = new MessageReader(_stream);
//            _thread = new Threading.ThreadMethod(ThreadProc);
//        }
//    }
//    //wraps a PipeReader
//    public class PipeBase : IDisposable
//    {
//        private BackgroundPipeReader _reader;
//        private bool _disconnectedRaised = false;
//        private LockableSlim<bool> _disposed = new LockableSlim<bool>(false);

//        public event Action<PipeBase> Connected;
//        public event Action<PipeBase> Disconnected;
//        public event Action<PipeBase, byte[]> MessageReceived;
//        public PipeStream Stream { get; set; }

//        protected void RaiseConnected()
//        {
//            Connected?.Invoke(this);
//        }
//        protected void RaiseDisconnected()
//        {
//            if (_disconnectedRaised)
//                return;
//            _disconnectedRaised = true;
//            //if (_disconnectedRaised.Value)
//            //    return;
//            //_disconnectedRaised.InWriteLock(v2 =>
//            //{
//            Disconnected?.Invoke(this);
//            //    return true;
//            //});
//        }
//        protected void Start()
//        {
//            if (_disconnectedRaised)//.Value)
//                throw new InvalidOperationException("Cannot be restarted");
//            if (Stream.As<NamedPipeServerStream>() != null)
//                Trace.WriteLine("Server Started");
//            else
//                Trace.WriteLine("Client Started");
//            _reader = new BackgroundPipeReader(Stream);
//            _reader.Disconnected += reader =>
//            {
//                RaiseDisconnected();
//            };
//            _reader.MessageReceived += (reader, message) =>
//            {
//                MessageReceived?.Invoke(this, message);
//            };
//            _reader.Start();
//        }

//        public virtual void Dispose()
//        {
//            _disposed.InWriteLock(value =>
//            {
//                if (value)
//                    return value;

//                _reader?.Dispose(); //reader can be null if Start was never called
//                return true;
//            });
//        }

//        private async Task WriteAsync(byte[] bytes)
//        {
//            await Task.Run(() =>
//            {
//                try
//                {
//                    _reader.Pause();
//                    PipeHelpers.WriteMessageChunks(Stream, bytes);
//                    _reader.Resume();
//                }
//                catch (ObjectDisposedException) //disconnected
//                {
//                    RaiseDisconnected();
//                }
//            }).ConfigureAwait(false);
//        }
//        private byte[] MakeMessage(PipeMessageTypeEnum type, byte[] bytes)
//        {
//            var typeBytes = BitConverter.GetBytes((int)type);
//            var lengthBytes = BitConverter.GetBytes(bytes.Length);
//            byte[] buffer = new byte[8 + bytes.Length];

//            System.Buffer.BlockCopy(typeBytes, 0, buffer, 0, 4);
//            System.Buffer.BlockCopy(lengthBytes, 0, buffer, 4, 4);
//            System.Buffer.BlockCopy(bytes, 0, buffer, 8, bytes.Length);
//            return buffer;
//        }
//        //protected async Task WriteAsync(PipeMessage message)
//        //{
//        //    var headerBytes = message.Header.AsBytes();
//        //    byte[] buffer = new byte[headerBytes.Length + message.Data.Length];
//        //    System.Buffer.BlockCopy(headerBytes, 0, buffer, 0, headerBytes.Length);
//        //    System.Buffer.BlockCopy(message.Data, 0, buffer, headerBytes.Length, message.Data.Length);
//        //    await PipeHelpers.WriteMessageChunksAsync(Stream, buffer);
//        //}
//        public async Task PostAsync(byte[] bytes)
//        {
//            //await WriteAsync(new PipeMessage() { Header = PipeHeader.Create(PipeMessageTypeEnum.Post, bytes.Length), Data = bytes });
//            var message = MakeMessage(PipeMessageTypeEnum.Post, bytes);
//            await WriteAsync(message);
//        }
//        public async Task RequestAsync(byte[] bytes)
//        {
//            //await WriteAsync(new PipeMessage() { Header = PipeHeader.Create(PipeMessageTypeEnum.Request, bytes.Length), Data = bytes });
//            var message = MakeMessage(PipeMessageTypeEnum.Request, bytes);
//            await WriteAsync(message);
//        }
//        public async Task RespondAsync(byte[] bytes)
//        {
//            //await WriteAsync(new PipeMessage() { Header = PipeHeader.Create(PipeMessageTypeEnum.Response, bytes.Length), Data = bytes });
//            var message = MakeMessage(PipeMessageTypeEnum.Response, bytes);
//            await WriteAsync(message);
//        }
//    } //PipeBase
//    public class Client : PipeBase
//    {
//        private LockableSlim<bool> _disposed = new LockableSlim<bool>(false);

//        public void Connect(string pipeName) { Connect(".", pipeName); }
//        public void Connect(string serverName, string pipeName)
//        {
//            Stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
//            Stream.As<NamedPipeClientStream>().Connect();
//            if (Stream.IsConnected)
//            {
//                Stream.ReadMode = PipeTransmissionMode.Byte;
//                Start();
//                RaiseConnected();
//            }
//        }
//        public void Connect(string pipeName, int msTimeout) { Connect(".", pipeName, msTimeout); }
//        public void Connect(string serverName, string pipeName, int msTimeout)
//        {
//            Stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
//            Stream.As<NamedPipeClientStream>().Connect(msTimeout);
//            if (Stream.IsConnected)
//            {
//                Stream.ReadMode = PipeTransmissionMode.Byte;
//                Start();
//                RaiseConnected();
//            }
//        }
//        //public async Task ConnectAsync(string pipeName) { await ConnectAsync(".", pipeName); }
//        //public async Task ConnectAsync(string serverName, string pipeName)
//        //{
//        //    Stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
//        //    await Stream.As<NamedPipeClientStream>().ConnectAsync().ContinueWith(task =>
//        //    {
//        //        if (Stream.IsConnected)
//        //        {
//        //            Stream.ReadMode = PipeTransmissionMode.Byte;
//        //            Start();
//        //            RaiseConnected();
//        //        }
//        //    });
//        //}
//        //public async Task ConnectAsync(string pipeName, int msTimeout) { await ConnectAsync(".", pipeName, msTimeout); }
//        //public async Task ConnectAsync(string serverName, string pipeName, int msTimeout)
//        //{
//        //    Stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
//        //    await Stream.As<NamedPipeClientStream>().ConnectAsync(msTimeout).ContinueWith(task =>
//        //    {
//        //        if (Stream.IsConnected)
//        //        {
//        //            Stream.ReadMode = PipeTransmissionMode.Byte;
//        //            Start();
//        //            RaiseConnected();
//        //        }
//        //    });
//        //}
//        //public async Task ConnectAsync(string pipeName, CancellationToken token) { await ConnectAsync(".", pipeName, token); }
//        //public async Task ConnectAsync(string serverName, string pipeName, CancellationToken token)
//        //{
//        //    Stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
//        //    await Stream.As<NamedPipeClientStream>().ConnectAsync(token).ContinueWith(task =>
//        //    {
//        //        if (Stream.IsConnected)
//        //        {
//        //            Stream.ReadMode = PipeTransmissionMode.Byte;
//        //            Start();
//        //            RaiseConnected();
//        //        }
//        //    });
//        //}
//        //public async Task ConnectAsync(string pipeName, int msTimeout, CancellationToken token) { await ConnectAsync(".", pipeName, msTimeout, token); }
//        //public async Task ConnectAsync(string serverName, string pipeName, int msTimeout, CancellationToken token)
//        //{
//        //    Stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
//        //    await Stream.As<NamedPipeClientStream>().ConnectAsync(msTimeout, token).ContinueWith(task =>
//        //    {
//        //        if (Stream.IsConnected)
//        //        {
//        //            Stream.ReadMode = PipeTransmissionMode.Byte;
//        //            Start();
//        //            RaiseConnected();
//        //        }
//        //    });
//        //}
//        public override void Dispose()
//        {
//            _disposed.InWriteLock(value =>
//            {
//                if (value)
//                    return value;
//                base.Dispose();
//                Stream.Dispose();
//                RaiseDisconnected();
//                return true;
//            });
//        }
//    }
//    public class Server : PipeBase
//    {
//        private static int _instanceCounter = 0;
//        private LockableSlim<bool> _disposed = new LockableSlim<bool>(false);

//        public int Id { get; set; }
//        internal void Dispose(bool quiet)
//        {
//            _disposed.InWriteLock(value =>
//            {
//                if (value)
//                    return value;

//                base.Dispose();
//#if NET45

//                if (Stream.IsConnected)
//                    Stream.As<NamedPipeServerStream>()?.Disconnect();
//                Stream?.Close();
//#endif
//                Stream.Dispose();
//                if (!quiet)
//                    RaiseDisconnected();
//                return true;
//            });
//        }
//        public override void Dispose()
//        {
//            Dispose(false);
//        }
//#if NET45
//        public void Start(string pipeName, int maxConnections, int inBufferSize = 4096, int outBufferSize = 4096)
//        {
//            Id = Interlocked.Increment(ref _instanceCounter);
//            var pipeSecurity = new PipeSecurity();
//            pipeSecurity.AddAccessRule(new PipeAccessRule(WindowsIdentity.GetCurrent().User, PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
//            pipeSecurity.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
//            Stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, inBufferSize, outBufferSize, pipeSecurity);
//            Stream.As<NamedPipeServerStream>().BeginWaitForConnection(ar =>
//            {
//                var server = ar.AsyncState.As<NamedPipeServerStream>();
//                try
//                {
//                    server.EndWaitForConnection(ar);
//                    Start();
//                    RaiseConnected();
//                }
//                catch (System.Threading.ThreadAbortException)
//                {
//                    System.Threading.Thread.ResetAbort();
//                }
//                catch
//                {
//                    RaiseDisconnected();
//                }
//            }, Stream);
//        }
//#else
//        public void Start(string pipeName, int maxConnections)
//        {
//            Id = Interlocked.Increment(ref _instanceCounter);
//            Stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
//            Stream.As<NamedPipeServerStream>().BeginWaitForConnection(ar =>
//            {
//                var server = ar.AsyncState.As<NamedPipeServerStream>();
//                try
//                {
//                    server.EndWaitForConnection(ar);
//                    Start();
//                    RaiseConnected();
//                }
//                catch
//                {
//                    RaiseDisconnected();
//                }
//            }, Stream);
//        }
//#endif
//    }
//    public class Host : IDisposable
//    {
//        private object _serversLock = new object();
//        private List<Server> _servers = new List<Server>();
//        private string _pipeName = "";
//        private int _maxConnections = 0;
//        private bool _isDisposing = false;
//        private object _disposeLock = new object();

//        public event Action<Server, byte[]> MessageReceived;

//        private Server CreateServer(int id)
//        {
//            var server = new Server() { Id = id };
//            server.Disconnected += reader =>
//            {
//                lock (_disposeLock)
//                {
//                    var existingId = reader.As<Server>().Id;
//                    try
//                    {
//                        reader.As<Server>().Dispose();
//                    }
//                    catch (Exception e)
//                    {
//                        System.Diagnostics.Debugger.Break();
//                        System.Diagnostics.Debug.WriteLine(e.ToString());
//                    }
//                    finally
//                    {
//                        if (!_isDisposing)
//                        {
//                            lock (_serversLock)
//                                _servers[existingId] = CreateServer(existingId);
//                        }
//                    }
//                }
//            };
//            server.MessageReceived += (writablePipeReader, m) =>
//            {
//                MessageReceived?.Invoke((Server)writablePipeReader, m);
//            };
//            server.Start(_pipeName, _maxConnections);
//            return server;
//        }
//        public void Start(string pipeName, int maxConnections)
//        {
//            _pipeName = pipeName;
//            _maxConnections = maxConnections;
//            for (int t = 0; t < _maxConnections; t++)
//            {
//                lock (_serversLock)
//                    _servers.Add(CreateServer(t));
//            }
//        }
//        public void Stop()
//        {
//            lock (_disposeLock)
//            {
//                foreach (var server in _servers)
//                {
//                    try
//                    {
//                        server.Dispose(true);
//                    }
//                    finally { }
//                }
//                lock (_serversLock)
//                    _servers.Clear();
//            }
//        }
//        public void Dispose()
//        {
//            _isDisposing = true;
//            Stop();
//        }
//    }

    [TestClass]
    public class PipeReader_Tests
    {
        private W.IO.Pipes.Server _server;
        private W.IO.Pipes.Client _client;

        [TestInitialize]
        public void Initialize()
        {
            var pipeName = "PipeReader_Tests";
            Trace.WriteLine("Pipe Name = " + pipeName);
            _server = new W.IO.Pipes.Server();
            _server.Connected += server =>
            {
                Trace.WriteLine("Server Connected");
            };
            _server.Disconnected += server =>
            {
                Trace.WriteLine("Server Disconnected");
            };
            _server.Start(pipeName, 20);

            _client = new W.IO.Pipes.Client();
            _client.Connected += client =>
            {
                Trace.WriteLine("Client Connected");
            };
            _client.Disconnected += client =>
            {
                Trace.WriteLine("Client Disconnected");
            };
            _client.Connect(pipeName);
        }
        [TestCleanup]
        public void Cleanup()
        {
            W.Threading.Thread.Sleep(10);
            _client.Dispose();
            Trace.WriteLine("Client Disposed");
            _server.Dispose();
            Trace.WriteLine("Server Disposed");
        }

        [TestMethod]
        public void IsConnected()
        {
            Assert.IsTrue(_client.Stream.IsConnected);
        }
        [TestMethod]
        public async Task ClientPost()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var received = false;
            _server.MessageReceived += (server, type, message) =>
            {
                received = true;
                Trace.WriteLine($"Server received: {message.AsString()}");
                mreContinue.Set();
            };
            _client.Disconnected += c =>
            {
                Trace.WriteLine($"Client Disconnected");
                mreContinue.Set();
            };
            _client.Request("Hello Mr. Server".AsBytes());
            //_client.Request("Hello Mr. Server".AsBytes());
            mreContinue.Wait();
            Assert.IsTrue(received);

            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public async Task RequestResponse()
        {
            var mreContinue = new ManualResetEventSlim(false);
            _server.MessageReceived += async (server, type, message) =>
            {
                Trace.WriteLine("Server received: {0}", message.AsString());
                server.Respond("Hello Mr. Client".AsBytes());
            };
            _client.MessageReceived += (client, type, message) =>
            {
                Trace.WriteLine($"Client received: {message.AsString()}");
                mreContinue.Set();
            };
            _client.Request("Hello Mr. Server".AsBytes());
            mreContinue.Wait();

            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public async Task ClientRespondToServerPost()
        {
            var mreContinue = new ManualResetEventSlim(false);
            _server.MessageReceived += (server, type, message) =>
            {
                Trace.WriteLine($"Server received: {message.AsString()}");
                mreContinue.Set();
            };
            _client.MessageReceived += async (client, type, message) =>
            {
                Trace.WriteLine($"Client received: {message.AsString()}");
                client.Respond("Ok to shutdown".AsBytes());
            };
            _server.Post("Server shutting down".AsBytes());
            mreContinue.Wait();

            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public async Task ClientPost_10Times_NoReponse()
        {
            var mreContinue = new ManualResetEventSlim(false);
            _server.MessageReceived += (server, type, message) =>
            {
                Trace.WriteLine($"Server received: {message.AsString()}");
                mreContinue.Set();
            };
            _client.Disconnected += c =>
            {
                Trace.WriteLine($"Disconnected");
                mreContinue.Set();
            };
            for (int t = 0; t < 10; t++)
            {
                mreContinue.Reset();
                _client.Post(string.Format("Test Data {0}", t).AsBytes());
                mreContinue.Wait();
            }
            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public async Task RequestResponse_Latin()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var latin = PipeHelpers.GetLatinText();
            var latinBytes = latin.AsBytes();
            _server.MessageReceived += async (server, type, message) =>
            {
                Trace.WriteLine($"Server received {message.Length} bytes");
                for (int t = 0; t < latinBytes.Length; t++)
                {
                    if (message[t] != latinBytes[t])
                        System.Diagnostics.Debugger.Break();
                }
                server.Respond(message);
            };
            _client.MessageReceived += (client, type, message) =>
            {
                Trace.WriteLine($"Client received {message.Length} bytes");
                for (int t = 0; t < latinBytes.Length; t++)
                {
                    if (message[t] != latinBytes[t])
                        System.Diagnostics.Debugger.Break();
                }
                mreContinue.Set();
            };
            _client.Request(latin.AsBytes());
            mreContinue.Wait();
            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public async Task RequestResponse_1k()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var bigData = new byte[1000]; //.2mb
                                          //var bigData = new byte[2000000]; //2mb
                                          //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);

            _server.MessageReceived += async (server, type, message) =>
            {
                Trace.WriteLine($"Server received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (message[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                server.Respond(message);
            };
            _client.MessageReceived += (client, type, message) =>
            {
                Trace.WriteLine($"Client received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (message[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                mreContinue.Set();
            };
            _client.Request(bigData);
            mreContinue.Wait();
            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public void RequestResponse_200k()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var bigData = new byte[200000]; //.2mb
                                            //var bigData = new byte[2000000]; //2mb
                                            //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);

            _server.MessageReceived += (server, type, message) =>
            {
                Trace.WriteLine($"Server received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (message[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                server.Respond(message);//.Wait();
            };
            _client.MessageReceived += (client, type, message) =>
            {
                Trace.WriteLine($"Client received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (message[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                mreContinue.Set();
            };
            _client.Request(bigData);//.Wait();
            mreContinue.Wait();
            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public void RequestResponse_2MB()
        {
            var mreContinue = new ManualResetEventSlim(false);
            //var bigData = new byte[200000]; //.2mb
            var bigData = new byte[2000000]; //2mb
                                             //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);

            _server.MessageReceived += (server, type, message) =>
            {
                Trace.WriteLine($"Server received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (message[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                server.Respond(message);//.Wait();
            };
            _client.MessageReceived += (client, type, message) =>
            {
                Trace.WriteLine($"Client received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (message[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                mreContinue.Set();
            };
            _client.Request(bigData);//.Wait();
            mreContinue.Wait();
            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public void RequestResponse_20MB()
        {
            var mreContinue = new ManualResetEventSlim(false);
            //var bigData = new byte[200000]; //.2mb
            //var bigData = new byte[2000000]; //2mb
            var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);
            byte[] response = null;

            _server.MessageReceived += (server, type, message) =>
            {
                Trace.WriteLine($"Server received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                {
                    if (message[t] != bigData[t])
                        System.Diagnostics.Debugger.Break();
                }
                server.Respond(message);//.Wait();
            };
            _client.MessageReceived += (client, type, message) =>
            {
                Trace.WriteLine($"Client received {message.Length} bytes");
                if (message.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                response = message;
                for (int t = 0; t < bigData.Length; t++)
                {
                    if (message[t] != bigData[t])
                        System.Diagnostics.Debugger.Break();
                }
                mreContinue.Set();
            };
            _client.Request(bigData);//.Wait();
            mreContinue.Wait();
            for (int t = 0; t < bigData.Length; t++)
                Assert.IsTrue(response[t] == bigData[t], "t == " + t.ToString());
            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public async Task ClientPostManyTimes()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var numberOfRequests = 100L;
            var counter = 0L;
            _server.MessageReceived += async (server, type, message) =>
            {
                Interlocked.Increment(ref counter);
                Trace.WriteLine($"Server received: {message.AsString()}");
                if (Interlocked.Read(ref counter) == numberOfRequests)
                    mreContinue.Set();
            };
            for (var t = 0L; t < numberOfRequests; t++)
                _client.Request(string.Format("Hello {0}", t).AsBytes());
            mreContinue.Wait();

            Trace.WriteLine("Complete");
        }
        [TestMethod]
        public async Task ServerPostManyTimes()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var mreComplete = new ManualResetEventSlim(false);
            var numberOfRequests = 100L;
            var counter = 0L;
            _client.MessageReceived += (server, type, message) =>
            {
                Interlocked.Increment(ref counter);
                Trace.WriteLine($"Client received: {message.AsString()}");
                if (Interlocked.Read(ref counter) == numberOfRequests)
                    mreComplete.Set();
                mreContinue.Set();
            };
            for (var t = 0L; t < numberOfRequests; t++)
            {
                mreContinue.Reset();
                _server.Post($"Hello {t}".AsBytes());
                mreContinue.Wait();
            }
            mreComplete.Wait(25000);

            Trace.WriteLine("Complete");
        }
    }
    [TestClass]
    public class PipeHost_Tests
    {
        [TestMethod]
        public void Host_Create()
        {
            var pipeName = nameof(Host_Create);
            var mreContinue = new ManualResetEventSlim(false);
            var clients = new List<W.IO.Pipes.Client>();
            var numberOfClients = 100;

            using (var host = new W.IO.Pipes.Host())
            {
                host.Start(pipeName, numberOfClients);

                W.Threading.Thread.Sleep(100);
            }
        }
        [TestMethod]
        public async Task Concurrent_RequestResponse_PipeHost()
        {
            var pipeName = nameof(Concurrent_RequestResponse_PipeHost);
            var mreContinue = new ManualResetEventSlim(false);
            var clients = new List<W.IO.Pipes.Client>();
            var numberOfClients = 20;
            var numberOfResponses = 0L;

            using (var host = new W.IO.Pipes.Host())
            {
                host.MessageReceived += async (server, type, message) =>
                {
                    Trace.WriteLine($"Server {server.Id} received: {message.AsString()}");
                    server.Respond($"{server.Id} says Hello".AsBytes());
                };
                host.Start(pipeName, numberOfClients);

                //create and connect clients
                for (int t = 0; t < numberOfClients; t++) //concurrent connections
                {
                    var newClient = new W.IO.Pipes.Client();
                    newClient.MessageReceived += (client, type, message) =>
                    {
                        Trace.WriteLine($"Client received: {message.AsString()}");
                        Interlocked.Increment(ref numberOfResponses);
                        if (Interlocked.Read(ref numberOfResponses) == numberOfClients)
                            mreContinue.Set();
                    };
                    newClient.Connect(pipeName, 5000);
                    Assert.IsTrue(newClient.Stream.IsConnected);
                    clients.Add(newClient);
                }
                //make requests
                for (int t = 0; t < numberOfClients; t++) //concurrent connections
                {
                    clients[t].Request($"{t + 1}: Hello Mr. Server".AsBytes());
                }
                //wait for all servers to respond
                mreContinue.Wait();

                //disconnect clients
                for (int t = 0; t < numberOfClients; t++) //concurrent connections
                {
                    clients[t].Dispose();
                }
            }

            Trace.WriteLine("Complete");
        }
    }
    [TestClass]
    public class PipeHost_Tests2
    {
        [TestMethod]
        public void Connect1()
        {
            using (var host = new W.IO.Pipes.Host())
            {
                host.MessageReceived += async (s, t, m) =>
                {
                    s.Respond(m);
                };
                host.Start(nameof(Connect1), 20);
                using (var client = new W.IO.Pipes.Client())
                {
                    client.Connect(nameof(Connect1));
                    Assert.IsTrue(client.Stream.IsConnected);
                }
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinUntil, 1000);
            }
        }
        [TestMethod]
        public void Connect10()
        {
            using (var host = new W.IO.Pipes.Host())
            {
                host.MessageReceived += async (s, t, m) =>
                {
                    s.Respond(m);
                };
                host.Start(nameof(Connect10), 20);
                for (int t = 0; t < 10; t++)
                {
                    using (var client = new W.IO.Pipes.Client())
                    {
                        client.Connect(nameof(Connect10));
                        Assert.IsTrue(client.Stream.IsConnected);
                    }
                }
            }
        }
        [TestMethod]
        public void ContinuousConnections()
        {
            using (var host = new W.IO.Pipes.Host())
            {
                host.MessageReceived += async (s, t, m) =>
                {
                    s.Respond(m);
                };
                host.Start(nameof(ContinuousConnections), 20);
                for (int t = 0; t < 40; t++)
                {
                    using (var client = new W.IO.Pipes.Client())
                    {
                        client.Connect(nameof(ContinuousConnections));
                        Assert.IsTrue(client.Stream.IsConnected);
                    }
                }
            }
        }
        [TestMethod]
        public void ContinuousConnectionsWithRequestRespond()
        {
            var mreContinue = new ManualResetEventSlim(false);
            using (var host = new W.IO.Pipes.Host())
            {
                host.MessageReceived += async (s, t, m) =>
                {
                    s.Respond(m);
                };
                host.Start(nameof(ContinuousConnectionsWithRequestRespond), 20);
                for (int t = 0; t < 100; t++)
                {
                    using (var client = new W.IO.Pipes.Client())
                    {
                        var msg = $"{t}: This is a test message";
                        client.MessageReceived += (c, type, m) =>
                        {
                            Assert.IsTrue(m.AsString() == msg);
                            Trace.WriteLine("Client Received: " + m.AsString());
                            mreContinue.Set();
                        };
                        client.Connect(nameof(ContinuousConnectionsWithRequestRespond));
                        Assert.IsTrue(client.Stream.IsConnected);

                        mreContinue.Reset();
                        client.Request(msg.AsBytes());//.Wait();
                        mreContinue.Wait();
                    }
                }
            }
        }
        [TestMethod]
        public void AbundantRequestResponses()
        {
            var mreContinue = new ManualResetEventSlim(false);
            using (var host = new W.IO.Pipes.Host())
            {
                host.MessageReceived += async (s, type, m) =>
                {
                    s.Respond(m);
                };
                host.Start(nameof(AbundantRequestResponses), 1);
                using (var client = new W.IO.Pipes.Client())
                {
                    string msg = string.Empty;
                    client.MessageReceived += (c, type, m) =>
                    {
                        Assert.IsTrue(m.AsString() == msg);
                        Trace.WriteLine("Client Received: " + m.AsString());
                        mreContinue.Set();
                    };
                    client.Connect(nameof(AbundantRequestResponses));
                    Assert.IsTrue(client.Stream.IsConnected);

                    for (int t = 0; t < 100000; t++)
                    {
                        msg = "This is test message #" + t.ToString();
                        mreContinue.Reset();
                        client.Request(msg.AsBytes());//.Wait();
                        mreContinue.Wait();
                    }
                }
            }
        }
    }
}
