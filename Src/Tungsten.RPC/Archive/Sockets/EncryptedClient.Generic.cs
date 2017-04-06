using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Logging;

namespace W.RPC
{
    internal class EncryptedClient<TMessageType> : EncryptedClient where TMessageType : EncryptedMessageBase
    {
        public new delegate void MessageArrivedDelegate(EncryptedClient<TMessageType> clientClient, TMessageType message);
        public new event MessageArrivedDelegate MessageArrived;

        public delegate void MessageArrivedCallback(EncryptedClient<TMessageType> clientClient, TMessageType response, bool expired);

        private readonly List<KeyValuePair<TMessageType, MessageArrivedCallback>> _waiters = new List<KeyValuePair<TMessageType, MessageArrivedCallback>>();

        private void RaiseMessageArrived(TMessageType message)
        {
            var evt = this.MessageArrived;
            Task.Run(() =>
            {
                try
                {
                    evt?.Invoke(this, message);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            });
        }
        private void HandleMessageArrived(TMessageType msg)
        {
            for (int t = _waiters.Count - 1; t >= 0; t--)
            {
                try
                {
                    var waiter = _waiters[t];
                    bool isExpired = waiter.Key.ExpireDateTime < DateTime.Now;
                    if (waiter.Key.Id == msg.Id)
                    {
                        // invoke on separate "thread" so we can continue
                        Task.Run(() =>
                        {
                            try
                            {
                                waiter.Value?.Invoke(this, msg, isExpired);
                            }
                            catch (Exception e)
                            {
                                Log.e(e);
                            }
                        });
                        _waiters.RemoveAt(t);
                    }
                    else if (isExpired)//remove any that have timed out
                    {
                        // invoke on separate "thread" so we can continue
                        Task.Run(() =>
                        {
                            try
                            {
                                waiter.Value?.Invoke(this, null, true);
                            }
                            catch (Exception e)
                            {
                                Log.e(e);
                            }
                        });
                        _waiters.RemoveAt(t);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected virtual TMessageType FormatReceivedMessage(string message)
        {
            TMessageType result = Activator.CreateInstance<TMessageType>();
            Json.Populate(message, result);
            //var result = Json.FromJson<TMessageType>(message);
            //var result = JsonConvert.DeserializeObject<TMessageType>(message);
            return result;
        }
        protected virtual string FormatOutgoingMessage(TMessageType message)
        {
            var result = Json.ToJson(message);
            return result;
        }

        public void Post(TMessageType message)
        {
            var msg = FormatOutgoingMessage(message);
            base.Post(msg);
        }
        public void Post(TMessageType message, MessageArrivedCallback onResponse, double msTimeout)
        {
            if (message.Id == Guid.Empty)
                message.Id = Guid.NewGuid();
            message.ExpireDateTime = DateTime.Now.AddMilliseconds(msTimeout);
            _waiters.Add(new KeyValuePair<TMessageType, MessageArrivedCallback>(message, onResponse));
            Post(message);
        }

        public EncryptedClient()
        {
            base.MessageArrived += (sender, message) =>
            {
                TMessageType msg = FormatReceivedMessage(message);
                if (msg == null) //ignore null messages (this allows us to trap the encryption key)
                            return;
                HandleMessageArrived(msg);
                RaiseMessageArrived(msg);
            };
        }
        ~EncryptedClient()
        {
            //release any waiting callbacks
            while (_waiters.Count > 0)
            {
                try
                {
                    _waiters[0].Value?.Invoke(this, null, true); //so the waiter can continue
                }
                catch
                {
                }
                _waiters.RemoveAt(0);
            }
        }
    }
}
