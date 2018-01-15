using System;
using System.Net;
using System.Security.Cryptography;
using W;
using W.AsExtensions;
using System.Threading.Tasks;

namespace W.Net
{
    //public class CommandClient : Client<CommandClient.CommandMessage>
    //{
    //    //Commands should not overlap, therefore they shouldn't be used like flags
    //    public enum CommandEnum
    //    {
    //        None = 0,
    //        KeepAlive = 1,
    //        Disconnect = 2,
    //        Configuration = 3,
    //        Login = 4,
    //        Message = 5
    //    }
    //    [Flags] //I imagine one could specify multiple statuses at once
    //    public enum StatusEnum
    //    {
    //        None = 0,
    //        Ok = 1,
    //        Yes = 2,
    //        No = 4,
    //        Cancel = 8
    //    }
    //    public class CommandMessage
    //    {
    //        public CommandEnum Command { get; set; }
    //        public StatusEnum Status { get; set; }
    //        public string Data { get; set; } = "";//using byte[] here would be somewhat redundant (just leave it as json)
    //        public string Exception { get; set; } = "";
    //        public string Tag { get; set; } = "";
    //    }

    //    public Action<object, CommandMessage> CommandReceived { get; set; }

    //    protected void SendDisconnect(string exception = "")
    //    {
    //        var command = new CommandMessage();
    //        command.Command = CommandEnum.Disconnect;
    //        command.Status = StatusEnum.None;
    //        command.Exception = exception;
    //        Send(command);
    //    }

    //    protected override void OnMessageReceived(ref CommandMessage command)
    //    {
    //        base.OnMessageReceived(ref command); //ok to do this because I'm the one overriding and the base class doesn't modify the command
    //        OnCommandReceived(ref command);
    //    }
    //    protected virtual void OnCommandReceived(ref CommandMessage command)
    //    {
    //        //do we need to consume this command or should we pass it on?
    //        //consume it for now
    //        if (command.Command == CommandEnum.Disconnect)
    //        {
    //            if (!string.IsNullOrEmpty(command.Exception))
    //                Disconnect(new Exception(command.Exception));
    //            else
    //                Disconnect();
    //        }
    //        else
    //            RaiseCommandReceived(this, command);
    //    }
    //    protected void RaiseCommandReceived(object sender, CommandMessage command)
    //    {
    //        var evt = CommandReceived;
    //        if (evt != null)
    //        {
    //            Debug.i("Raising SecureClient.CommandReceived");
    //            DelegateExtensions.Raise(evt, sender, command);
    //            Debug.i("Raised SecureClient.CommandReceived");
    //        }
    //    }

    //    public override void Disconnect(Exception exception)
    //    {
    //        SendDisconnect(exception?.Message ?? "");
    //        base.Disconnect(exception);
    //    }
    //    public void Send(CommandEnum command, StatusEnum status, string data = "", string exception = "", string tag = "")
    //    {
    //        var cmd = new CommandMessage() { Command = command, Status = status, Data = data, Exception = exception, Tag = tag };
    //        Send(cmd);
    //    }
    //    public void Send(CommandMessage command)
    //    {
    //        System.Diagnostics.Debug.WriteLine("Sending Command: {0}, {1}", command.Command, command.Status);
    //        base.Send(ref command);
    //    }
    //    //public void Send(CommandMessage command)
    //    //{
    //    //    var json = SerializationMethods.Serialize(command);
    //    //    var bytes = json.AsBytes();
    //    //    Send(ref bytes);
    //    //}

    //    public CommandClient()
    //    {
    //    }
    //}

    /// <summary>
    /// The generic version of W.Net.Client
    /// </summary>
    /// <typeparam name="TMessageType">The message Type</typeparam>
    public class SecureClient<TMessageType> : SecureClient
    {
        /// <summary>
        /// Called when a message has been received
        /// </summary>
        public Action<object, TMessageType> MessageReceived { get; set; }
        /// <summary>
        /// Calls MessageReceived multi-cast delegate when a message has been received
        /// </summary>
        /// <param name="message">The received message</param>
        protected virtual void OnMessageReceived(ref TMessageType message)
        {
            RaiseMessageReceived(this, ref message);
        }
        /// <summary>
        /// Overridden to deserialize the data into a TMessageType message
        /// </summary>
        /// <param name="bytes">The data received from remote machine</param>
        protected override void OnDataReceived(ref byte[] bytes)
        {
            TMessageType message = SerializationMethods.Deserialize<TMessageType>(ref bytes);
            if (message != null)
            {
                base.OnDataReceived(ref bytes);
                OnMessageReceived(ref message);
            }
        }
        /// <summary>
        /// Calls the MessageReceived multi-cast delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        protected void RaiseMessageReceived(object sender, ref TMessageType message)
        {
            //try
            //{
                Debug.i("Raising Client.MessageReceived");
                MessageReceived?.Invoke(sender, message);
                Debug.i("Raised Client.MessageReceived");
//            }
//#if NET45
//            catch (System.Threading.ThreadAbortException e)
//            {
//                System.Threading.Thread.ResetAbort();
//            }
//#else
//            catch (ObjectDisposedException)
//            {
//                //ignore it, the task is shutting down forcefully
//            }
//            catch (AggregateException)
//            {
//                //ignore it, the task might be shutting down forcefully
//            }
//#endif
//            catch (Exception e)
//            {
//                System.Diagnostics.Debug.WriteLine(e.ToString());
//                System.Diagnostics.Debugger.Break();
//            }
        }
        /// <summary>
        /// Overload to send messages of TMessageType
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Send(ref TMessageType message)
        {
            byte[] bytes = SerializationMethods.Serialize(message).AsBytes();
            base.Send(bytes);
        }
        /// <summary>
        /// Overload to send messages of type TMessageType via serialization
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="response">The response from the remote machine</param>
        /// <param name="msTimeout">A timeout, in milliseconds, if desired</param>
        public bool SendAndWaitForResponse(ref TMessageType message, out TMessageType response, int msTimeout = -1)
        {
            byte[] bytes = SerializationMethods.Serialize(message).AsBytes();
            var lengthBuffer = new byte[4];
            if (SendAndWaitForResponse(bytes, out byte[] responseBytes, msTimeout))
            {
                response = SerializationMethods.Deserialize<TMessageType>(ref responseBytes);
                return true;
            }
            response = default(TMessageType);
            return false;
        }
    }

    /// <summary>
    /// A secure version of W.Net.Client.  Uses assymetric encryption and designed to be used with W.Net.Server&lt;W.Net.SecureClient&gt;.
    /// </summary>
    public class SecureClient : Client
    {
        private W.Encryption.PublicPrivateKeyPair _keys = W.Encryption.RSAMethods.CreateKeyPair(2048);
        private System.Security.Cryptography.RSAParameters? _remotePublicKey = null;
        private System.Threading.ManualResetEventSlim _mreIsSecure = new System.Threading.ManualResetEventSlim(false);

        /// <summary>
        /// True if a connection has been made and is secure, otherwise False.
        /// </summary>
        public new bool IsConnected { get => base.IsConnected && _mreIsSecure.IsSet; protected set => base.IsConnected = value; }

        private bool ExchangeKeys()
        {
            byte[] response;
            var bytes = SerializationMethods.Serialize(_keys.PublicKey).AsBytes();
            var lengthBuffer = new byte[4];
            if (SendRawAndWaitForResponse(bytes, out response, 15000))
            {
                var json = response.AsString();
                _remotePublicKey = SerializationMethods.Deserialize<System.Security.Cryptography.RSAParameters>(json);
                _mreIsSecure.Set();
                return true;
            }
            return false;
        }
        private bool Encrypt(ref byte[] bytes)
        {
            var result = false;
            if (_mreIsSecure.IsSet)
            {
                try
                {
                    var message = W.Encryption.RSAMethods.Encrypt(bytes.AsString(), (System.Security.Cryptography.RSAParameters)_remotePublicKey); //msg should be base64 encoded (by a previous Encrypt call) going into _rsa.Decrypt
                    bytes = message.AsBytes();
                    result = true;
                }
                catch (Exception)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            return result;
        }
        private bool Decrypt(ref byte[] bytes)
        {
            var result = false;
            if (_mreIsSecure.IsSet)
            {
                try
                {
                    var message = W.Encryption.RSAMethods.Decrypt(bytes.AsString(), _keys.PrivateKey); //msg should be base64 encoded (by a previous Encrypt call) going into _rsa.Decrypt
                    bytes = message.AsBytes();
                    result = true;
                }
                catch (Exception)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            return result;
        }

        /// <summary>
        /// Calls the Connected multi-cast delegate
        /// </summary>
        /// <param name="remoteEndPoint">The ip endpoint of the remote machine</param>
        protected override void OnConnected(IPEndPoint remoteEndPoint)
        {
            if (!_mreIsSecure.IsSet)
            {
                if (ExchangeKeys())
                {
                    base.OnConnected(remoteEndPoint);
                }
            }
        }
        /// <summary>
        /// Overridden 
        /// </summary>
        /// <param name="bytes">The array of bytes to decrypt</param>
        protected override void FormatDataReceived(ref byte[] bytes)
        {
            //decompress if so configured
            base.FormatDataReceived(ref bytes);
            //then decrypt
            Decrypt(ref bytes);
        }
        /// <summary>
        /// Overridden to encrypt the data.
        /// </summary>
        /// <param name="bytes">The array of bytes to encrypt</param>
        protected override void FormatDataToSend(ref byte[] bytes)
        {
            //encrypt, then compress if so configured
            Encrypt(ref bytes);
            base.FormatDataToSend(ref bytes);
        }
        /// <summary>
        /// Disconnects from the remote server and cleans up resources
        /// </summary>
        /// <param name="exception">The Exception which caused the disconnection, otherwise null</param>
        protected override void OnDisconnect(Exception exception)
        {
            _mreIsSecure.Reset();
            _remotePublicKey = null;
            base.OnDisconnect(exception);
        }
    }
}