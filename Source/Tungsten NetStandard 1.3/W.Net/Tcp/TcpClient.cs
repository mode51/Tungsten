using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using W.Threading;
using W.Logging;
using W;

namespace W.Net
{
    public static partial class Tcp
    {
        public class TcpClient : IInitialize, IClient
        {
            private ThreadMethod _thread;
            private volatile bool _exitNow = false;
            private object _keepAliveLock = new object();
            private System.Diagnostics.Stopwatch _swKeepAlive;
            private System.Diagnostics.Stopwatch _swDisconnect;
            private W.Threading.Lockers.MonitorLocker _socketLocker = new W.Threading.Lockers.MonitorLocker();

            public Socket Socket { get; private set; }
            public bool IsServerSide { get; set; } = false;

            public event Action<IClient, byte[]> BytesReceived;
            public event Action<IClient> Disconnected;
            public event Action<IClient> Connected;
            //public EventTemplate<IClient, byte[]> BytesReceived { get; private set; } = new EventTemplate<IClient, byte[]>();
            //public EventTemplate<IClient> Disconnected { get; private set; } = new EventTemplate<IClient>();
            //public EventTemplate<IClient> Connected { get; private set; } = new EventTemplate<IClient>();

            private void ThreadProc(CancellationToken token, params object[] args)
            {
                try
                {
                    while (!token.IsCancellationRequested && !_exitNow)
                    {
                        _socketLocker.Lock();
                        try
                        {
                            if (Socket == null || !Socket.IsConnected())
                            {
                                _exitNow = true;
                            }
                            else if (Socket?.Connected ?? false)
                            {
                                //if (Socket.Available > 0)
                                //    Log.i(IsServerSide ? "Server" : "Client" + $"{Socket.Available} bytes available");
                                if (Socket.Available >= 8)
                                {
                                    //Socket.GetResponse(out byte[] bytes);
                                    //OnReceived(ref bytes);
                                    if (Socket.GetResponseWithHeader(out HeaderTypeEnum type, out byte[] bytes))
                                    {
                                        _swDisconnect.Restart();
                                        switch (type)
                                        {
                                            case HeaderTypeEnum.Disconnect:
                                                _exitNow = true;
                                                break;
                                            case HeaderTypeEnum.KeepAlive:
                                                _swDisconnect.Restart();
                                                break;
                                            case HeaderTypeEnum.Data:
                                                if (bytes != null)
                                                    OnReceived(ref bytes);
                                                break;
                                        }
                                    }
                                }
                                //lock (_keepAliveLock)
                                //{
                                //    if (_swKeepAlive.Elapsed.Seconds == 5)
                                //    {
                                //        //System.Diagnostics.Debugger.Break();
                                //        Socket.SendKeepAlive();
                                //        _swKeepAlive.Restart();
                                //    }
                                //}
                                //if (_swDisconnect.Elapsed.Seconds > 60)
                                //{
                                //    //System.Diagnostics.Debugger.Break();
                                //    notifyRemote = true;
                                //    _exitNow = true;
                                //}
                            }
                        }//);
                        catch { throw; }
                        finally { _socketLocker.Unlock(); }
                        //2.1.2018 - this is the best sleep option
                        W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                    } // while
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
                //don't disconnect here (clients will call Dispose/Disconnect and servers will only call Dispose)
            }
            private void RaiseDisconnected()
            {
                Disconnected?.Invoke(this);
            }

            protected virtual void OnSend(ref byte[] bytes)
            {
                lock (_keepAliveLock)
                {
                    if (_swKeepAlive == null)
                        _swKeepAlive = System.Diagnostics.Stopwatch.StartNew();
                    else
                        _swKeepAlive.Restart();
                }
                if (_socketLocker == null)
                    return;
                _socketLocker?.Lock();
                try
                {
                    Socket?.SendMessageWithHeader(HeaderTypeEnum.Data, ref bytes);
                }
                finally
                {
                    _socketLocker?.Unlock();
                }

                lock (_keepAliveLock)
                    _swKeepAlive.Restart();
                //Socket.SendMessage(ref bytes);
            }
            protected virtual void OnReceived(ref byte[] bytes)
            {
                BytesReceived?.Invoke(this, bytes);
            }
            protected virtual void OnConnect(params object[] args)
            {
                if (Socket != null && Socket.Connected)
                    throw new InvalidOperationException("The socket is already connected");
                Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                Socket.Connect((IPEndPoint)args[0]);
                Socket.LingerState.Enabled = true;

                args[0] = Socket; //replace the IPEndPoint, but keep any additional args
                this.As<IInitialize>().Initialize(args);
                //Log.i(IsServerSide ? "Server" : "Client" + " Connected");
                Connected?.Invoke(this);
            }
            protected virtual bool OnInitialize(params object[] args)
            {
                Socket = (Socket)args[0];
                return true;
            }
            protected void Disconnect(bool notifyRemote, bool waitForThreadToExit)
            {
                try
                {
                    //_socketLocker should only be null if already disposed
                    _socketLocker.InLock(() =>
                    {
                        //Socket might be null if it was never connected
                        if (Socket?.Connected ?? false)
                        {
                            _exitNow = true; //tell the thread to exit
                                             //if (waitForThreadToExit)
                            _thread?.Cancel(); //wait for the thread to exit
                            _thread?.Dispose();
                            if (notifyRemote)
                                Socket?.SendDisconnect();
                            _swKeepAlive?.Stop();
                            _swDisconnect?.Stop();
                            Socket?.ShutdownAndDispose();
                            if (Socket != null)
                                RaiseDisconnected();
                            Socket = null;
                            //Log.i(IsServerSide ? "Server" : "Client" + " Disconnected");
                        }
                    });
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    System.Diagnostics.Debugger.Break();
                }
            }

            public void Write(byte[] bytes) { OnSend(ref bytes); }

            //private class CRequest : IDisposable
            //{
            //    private ManualResetEventSlim _mre = new ManualResetEventSlim(false);
            //    private object _locker = new object();
            //    private TcpClient _client;
            //    private byte[] _response = null;

            //    public bool TimedOut { get; private set; } = false;

            //    public async Task<byte[]> RequestAsync(byte[] bytes, int msTimeout)
            //    {
            //        return await Task.Run(() =>
            //        {
            //            try
            //            {
            //                _client.BytesReceived += _client_BytesReceived;
            //                _client.Write(bytes);
            //                TimedOut = !_mre.Wait(msTimeout);
            //            }
            //            catch
            //            {
            //            }
            //            finally
            //            {
            //                _client.BytesReceived -= _client_BytesReceived;
            //            }
            //            return _response;
            //        });
            //    }

            //    private void _client_BytesReceived(IClient client, byte[] response)
            //    {
            //        lock (_locker)
            //        {
            //            _response = response;
            //            _mre?.Set();
            //        }
            //    }
            //    public void Dispose()
            //    {
            //        lock (_locker)
            //        {
            //            _mre?.Dispose();
            //            _mre = null;
            //        }
            //    }
            //    public CRequest(TcpClient client)
            //    {
            //        _client = client;
            //    }
            //}
            public void Connect(IPEndPoint ep)
            {
                OnConnect(ep);
            }
            public void Disconnect()
            {
                Disconnect(true, true);
            }
            public virtual void Dispose()
            {
                Disconnect(true, true); //may already be disconnected, but make sure
                                        //Log.i(IsServerSide ? "Server" : "Client" + " Disposed");
                                        //_socketLocker = null;
            }

            bool IInitialize.Initialize(params object[] args)
            {
                var result = OnInitialize(args);
                if (result)
                {
                    _swKeepAlive = System.Diagnostics.Stopwatch.StartNew();
                    _swDisconnect = System.Diagnostics.Stopwatch.StartNew();
                    _thread = ThreadMethod.Create(ThreadProc);
                    _thread.Start();
                }
                else
                    Log.w("Socket Initialization Failed");
                return result;
            }
        }
    }
}