using System;
using System.Threading.Tasks;
using W.Logging;
using System.Threading;

namespace W.Net.RPC
{
    public partial class Client : IDisposable
    {
        private class Waiter : IDisposable
        {
            private W.Threading.Thread _thread = null;
            private Client _client = null;
            private Client<Message> _genericClient;
            //private CancellationTokenSource _cts = null;
            private Lockable<bool> _handling { get; } = new Lockable<bool>(false);
            private Lockable<bool> _disposing { get; } = new Lockable<bool>(false);
            private ManualResetEventSlim _mreCompleted = new ManualResetEventSlim(false);

            public Message Message { get; set; }
            public MessageArrivedCallback Callback { get; set; }
            public Action<Waiter, bool> Completed { get; set; } // to be used by Client to know when to release the Waiter

            private void ExecuteCallback(Message message)
            {
                if (_handling.Value)
                    return;
                _handling.Value = true;

                if (message != null)
                    Cancel(); //stop the timeout task
                try
                {
                    if (message == null)
                        Callback.Invoke(_client, null, true);
                    else
                    {
                        var isExpired = message.ExpireDateTime < DateTime.Now;
                        Callback.Invoke(_client, message, isExpired);
                    }
                }
                catch (Exception e)
                {
                    Log.e(e);
                    throw;
                }
                finally
                {
                    _mreCompleted.Set();
                    Completed?.Invoke(this, message != null);
                }
            }
            private void OnMessageReceived(object client, Message message)
            {
                if (message?.Id == Message.Id)
                {
                    ExecuteCallback(message);
                }
            }
            public void Cancel()
            {
                try //a hack
                {
                    _thread?.Cancel();
                }
                catch { }
                //if (!_cts?.IsCancellationRequested ?? false)
                //    _cts?.Cancel();
            }
            public void WaitOne()
            {
                if (!_disposing.Value)
                    _mreCompleted?.Wait();
            }
            public bool WaitOne(int msTimeout)
            {
                if (!_disposing.Value)
                    return _mreCompleted?.Wait(msTimeout) ?? true;
                return true;
            }
            public Waiter(Client client, Client<Message> genericClient, int msTimeout)
            {
                _client = client;
                genericClient.MessageReceived += OnMessageReceived;
                //_cts = new CancellationTokenSource(msTimeout);
                _thread = W.Threading.Thread.Create(cts =>
                {
                    bool isExpired = false;
                    try
                    {
                        //cts is Cancelled before the response is sent
                        while (!cts?.IsCancellationRequested ?? false)
                        {
                            isExpired = DateTime.Now > Message.ExpireDateTime;
                            if (isExpired)
                                break;
                            System.Threading.Thread.Sleep(25);
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (isExpired)
                            ExecuteCallback(null);
                    }
                }, null);//, _cts);
            }
            ~Waiter()
            {
                Dispose();
            }
            public void Dispose()
            {
                if (_disposing.Value)
                    return;
                _disposing.Value = true;

                _thread.Dispose();
                Cancel();
                //_cts.Dispose();
                _mreCompleted?.Set();
                _mreCompleted?.Dispose();
                _mreCompleted = null;
                if (_genericClient != null)
                    _genericClient.MessageReceived -= OnMessageReceived;
                _genericClient = null;
                GC.SuppressFinalize(this);
            }
        }
    }
}
