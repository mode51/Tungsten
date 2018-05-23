using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using W.Threading.Lockers;

namespace W.IO.Pipes
{
    /// <summary>
    /// A named pipe client class to make IPC easier
    /// </summary>
    public partial class PipeClient : IDisposable
    {
        private LockableSlim<bool> _connected = new LockableSlim<bool>(true);
        private SemaphoreSlimLocker StreamLock = new SemaphoreSlimLocker(1, 1);
        private Disposer _disposer = new Disposer();

        /// <summary>
        /// Raised when the client has been disconnected
        /// </summary>
        public event Action<PipeClient> Disconnected;

        /// <summary>
        /// The underlying NamedPipeClientStream
        /// </summary>
        public PipeStream Stream { get; private set; }

        /// <summary>
        /// Raises the Disconnected event
        /// </summary>
        protected virtual void RaiseDisconnected()
        {
            if (_connected.Value)
            {
                _connected.Value = false;
                Disconnected?.Invoke(this);
            }
        }

        /// <summary>
        /// Used by PipeClientExtensions to handle disconnections
        /// </summary>
        internal void Disconnect(bool supressDisconnectEvent) { if (!supressDisconnectEvent) RaiseDisconnected(); }

        /// <summary>
        /// Asynchronosly posts data to the PipeServer
        /// </summary>
        /// <param name="bytes">The data to send to the server</param>
        /// <param name="useCompression">If True, the data will be compressed before sending</param>
        /// <returns>The associated Task</returns>
        public async Task PostAsync(byte[] bytes, bool useCompression)
        {
            await StreamLock.InLockAsync(async () =>
            {
                await this.PostAsync(bytes, useCompression, false);
            });
            //using (var mre = new ManualResetEventSlim(false))
            //{
            //    try
            //    {
            //        if (useCompression)
            //            bytes = bytes.AsCompressed();
            //        await _streamSemaphore.LockAsync(async () =>
            //        {
            //            await Stream.WriteAsync(bytes, 0, bytes.Length);//.ConfigureAwait(false);
            //            await Stream.FlushAsync();//.ConfigureAwait(false);
            //            mre.Set();
            //        });
            //    }
            //    catch (ObjectDisposedException) //disconnected
            //    {
            //        RaiseDisconnected();
            //        mre.Set();
            //    }
            //    catch
            //    {
            //        mre.Set();
            //        throw;
            //    }
            //    //mre.Wait();
            //}
        }
        /// <summary>
        /// Asynchronosly posts an object to the PipeServer
        /// </summary>
        /// <param name="item">The object to send to the server</param>
        /// <param name="useCompression">If True, the data will be compressed before sending</param>
        /// <returns>The associated Task</returns>
        public async Task PostAsync<TType>(TType item, bool useCompression) where TType : class
        {
            await StreamLock.InLockAsync(async () =>
            {
                await this.PostAsync(item, useCompression, false);
            });
            ////using (var mre = new ManualResetEventSlim(false))
            ////{
            //try
            //{
            //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            //    var bytes = json.AsBytes();
            //    await PostAsync(bytes, useCompression, true, false);//.ConfigureAwait(false);
            //                                                        //mre.Set();
            //}
            //catch (ObjectDisposedException) //disconnected
            //{
            //    RaiseDisconnected();
            //}
            //catch
            //{
            //    //mre.Set();
            //    throw;
            //}
            ////mre.Wait();
            ////}
        }

        /// <summary>
        /// Asynchronously waits for a message to be received.  A timeout is required, but subsequent calls are expected.
        /// </summary>
        /// <param name="useCompression">If True, the data will be decompressed after reception</param>
        /// <param name="msTimeout">The number of milliseconds to wait for a message before returning</param>
        /// <returns>Data received from the server.  If a timeout occurrs, a null value is returned.</returns>
        public async Task<byte[]> WaitForMessageAsync(bool useCompression, int msTimeout)
        {
            var result = await StreamLock.InLockAsync(async () =>
            {
                return await this.WaitForMessageAsync(useCompression, msTimeout, false);
            });
            await result;
            return result.Result;
            //            byte[] result = null;
            //            using (var mre = new ManualResetEventSlim(false))
            //            {
            //                try
            //                {
            //                    var waitResult = await _streamSemaphore.LockAsync(async () =>
            //                    {
            //                        return await PipeMethods.WaitForMessageAsync(Stream, msTimeout);
            //                    });
            //                    await waitResult;
            //                    if (waitResult.Result.Success)
            //                        result = useCompression ? waitResult.Result.Result.FromCompressed() : waitResult.Result.Result;
            //                    mre.Set();
            //                }
            //#if NET45
            //                catch (System.Threading.ThreadAbortException)
            //                {
            //                    System.Threading.Thread.ResetAbort();
            //                    System.Diagnostics.Debugger.Break();
            //                }
            //#endif
            //                catch (ObjectDisposedException) //disconnected
            //                {
            //                    RaiseDisconnected();
            //                    mre.Set();
            //                }
            //                catch
            //                {
            //                    mre.Set();
            //                    throw;
            //                }
            //                //mre.Wait();
            //            }
            //            return result;
        }
        /// <summary>
        /// Asynchronously waits for a message to be received.  A timeout is required, but subsequent calls are expected.
        /// </summary>
        /// <param name="useCompression">If True, the item data will be decompressed after reception</param>
        /// <param name="msTimeout">The number of milliseconds to wait for a message before returning</param>
        /// <returns>A deserialized item of type TType.  If a timeout occurrs, a null value is returned.</returns>
        public async Task<TType> WaitForMessageAsync<TType>(bool useCompression, int msTimeout) where TType : class
        {
            var result = await StreamLock.InLockAsync(async () =>
            {
                return await this.WaitForMessageAsync<TType>(useCompression, msTimeout, false);
            });
            await result;
            return result.Result;
            //var result = default(TType);
            ////using (var mre = new ManualResetEventSlim(false))
            //{
            //    try
            //    {
            //        var bytes = await WaitForMessageAsync(useCompression, msTimeout, true, false);
            //        if (bytes != null)
            //        {
            //            var json = bytes.AsString();
            //            result = Newtonsoft.Json.JsonConvert.DeserializeObject<TType>(json);
            //            //mre.Set();
            //        }
            //    }
            //    catch (ObjectDisposedException) //disconnected
            //    {
            //        RaiseDisconnected();
            //        //mre.Set();
            //    }
            //    catch
            //    {
            //        //mre.Set();
            //        throw;
            //    }
            //    //mre.Wait();
            //}
            //return result;
        }

        /// <summary>
        /// Asynchronously posts data to the server and waits the specified number of milliseconds for a response
        /// </summary>
        /// <param name="bytes">The data to send to the server</param>
        /// <param name="useCompression">If True, the data will be compressed before sending and decompressed upon reception</param>
        /// <param name="msTimeout">The number of milliseconds to wait for a response</param>
        /// <returns>Data received from the server.  If a timeout occurrs, a null value is returned.</returns>
        public async Task<byte[]> RequestAsync(byte[] bytes, bool useCompression, int msTimeout)
        {
            var result = await StreamLock.InLockAsync(async () =>
            {
                return await this.RequestAsync(bytes, useCompression, msTimeout, false);
            });
            await result;
            return result.Result;
            //byte[] result = null;
            ////using (var mre = new ManualResetEventSlim(false))
            //{
            //    try
            //    {
            //        await StreamLock.LockAsync(async () =>
            //        {
            //            await PostAsync(bytes, useCompression, false, false);
            //            result = await WaitForMessageAsync(useCompression, msTimeout, false, false);
            //            //mre.Set();
            //        });
            //    }
            //    catch (ObjectDisposedException) //disconnected
            //    {
            //        RaiseDisconnected();
            //        //mre.Set();
            //    }
            //    catch
            //    {
            //        //mre.Set();
            //        throw;
            //    }
            //    //mre.Wait();
            //}
            //return result;
        }
        /// <summary>
        /// Asynchronously posts a message to the server and waits the specified number of milliseconds for a response
        /// </summary>
        /// <param name="item">The object send to the server</param>
        /// <param name="useCompression">If True, the data will be compressed before sending and decompressed upon reception</param>
        /// <param name="msTimeout">The number of milliseconds to wait for a response</param>
        /// <returns>The deserialized response from the server.  If a timeout occurrs, a null value is returned.</returns>
        public async Task<TType> RequestAsync<TType>(TType item, bool useCompression, int msTimeout) where TType : class
        {
            var result = await StreamLock.InLockAsync(async () =>
            {
                return await this.RequestAsync<TType>(item, useCompression, msTimeout, false);
            });
            await result;
            return result.Result;
        }
        //public async Task<TType> RequestAsync<TType>(TType item, bool useCompression, int msTimeout)
        //{
        //    var result = default(TType);
        //    //using (var mre = new ManualResetEventSlim(false))
        //    {
        //        try
        //        {
        //            await StreamSemaphore.LockAsync(async () =>
        //            {
        //                await PostAsync(item, useCompression, false, false);
        //                result = await WaitForMessageAsync<TType>(useCompression, msTimeout, false, false);
        //                //mre.Set();
        //            });
        //        }
        //        catch (ObjectDisposedException) //disconnected
        //        {
        //            RaiseDisconnected();
        //            //mre.Set();
        //        }
        //        catch
        //        {
        //            //mre.Set();
        //            throw;
        //        }
        //        //mre.Wait();
        //    }
        //    return result;
        //}

        //public async Task<byte[]> RequestAndWaitAsync(byte[] bytes, bool useCompression, int msTimeout)
        //{
        //    byte[] result = null;
        //    using (var mre = new ManualResetEventSlim(false))
        //    {
        //        try
        //        {
        //            if (useCompression)
        //                bytes = bytes.AsCompressed();
        //            await Stream.WriteAsync(bytes, 0, bytes.Length);//.ConfigureAwait(false);
        //            await Stream.FlushAsync();//.ConfigureAwait(false);

        //            var waitResult = await PipeMethods.WaitForMessageAsync(Stream, msTimeout);
        //            if (waitResult.Success)
        //                result = useCompression ? waitResult.Result.FromCompressed() : waitResult.Result;
        //            mre.Set();

        //        }
        //        catch (ObjectDisposedException) //disconnected
        //        {
        //            RaiseDisconnected();
        //            mre.Set();
        //        }
        //        catch
        //        {
        //            mre.Set();
        //            throw;
        //        }
        //        mre.Wait();
        //    }
        //    return result;
        //}
        //public async Task<TType> RequestAndWaitAsync<TType>(TType item, bool useCompression, int msTimeout)
        //{
        //    TType result = default(TType);
        //    using (var mre = new ManualResetEventSlim(false))
        //    {
        //        try
        //        {
        //            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
        //            var bytes = json.AsBytes();
        //            if (useCompression)
        //                bytes = bytes.AsCompressed();
        //            await Stream.WriteAsync(bytes, 0, bytes.Length);//.ConfigureAwait(false);
        //            await Stream.FlushAsync();//.ConfigureAwait(false);

        //            var waitResult = await PipeMethods.WaitForMessageAsync(Stream, msTimeout);
        //            if (waitResult.Success)
        //            {
        //                bytes = useCompression ? waitResult.Result.FromCompressed() : waitResult.Result;
        //                json = bytes.AsString();
        //                result = Newtonsoft.Json.JsonConvert.DeserializeObject<TType>(json);
        //                mre.Set();
        //            }

        //        }
        //        catch (ObjectDisposedException) //disconnected
        //        {
        //            RaiseDisconnected();
        //            mre.Set();
        //        }
        //        catch
        //        {
        //            mre.Set();
        //            throw;
        //        }
        //        mre.Wait();
        //    }
        //    return result;
        //}

        /// <summary>
        /// Disposes the PipeClient and releases resources
        /// </summary>
        public void Dispose()
        {
            _disposer.Dispose(() =>
            {
                StreamLock?.InLock(() =>
                {
                    Helpers.DisconnectAndDispose(Stream);
                });
                //4.4.2018 - the key is to never raise an event from Dispose()
                //Disconnect(false); //causes a recursive call to Dispose
                StreamLock?.Dispose();
                StreamLock = null;
            });
        }

        /// <summary>
        /// Constructs a new PipeClient
        /// </summary>
        /// <param name="stream">The NamedPipeClientStream associated with this PipeClient</param>
        public PipeClient(PipeStream stream)
        {
            Stream = stream;
        }

        /// <summary>
        /// Creates a new PipeServer instance, initializing the NamedPipeServerStream instance with the specified parameters
        /// </summary>
        /// <param name="serverName">The name of the machine hosting the PipeServer</param>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="msTimeout">The number of milliseconds to wait before failing</param>
        /// <returns></returns>
        /// <returns>A CallResult containing success or failure of the call, an exception if one occurred and the resulting PipeClient.  Note that if the call fails, the Result member will be null.</returns>
        public static CallResult<PipeClient> CreateClient(string serverName, string pipeName, int msTimeout)
        {
            var result = new PipeResult<PipeClient>();
            var connectionResult = Helpers.Connect(serverName, pipeName, System.Security.Principal.TokenImpersonationLevel.None, msTimeout);
            result.Exception = connectionResult.Exception;
            result.Success = connectionResult.Success;
            if (connectionResult.Success)
                result.Result = new PipeClient(connectionResult.Result);
            return result;
        }
    }
    public partial class PipeClient
    {
        /// <summary>
        /// Asynchronously posts a messages to the remote pipe in a single call
        /// </summary>
        /// <param name="bytes">The data to send to the server</param>
        /// <param name="useCompression">If True, the data will be compressed before sending and decompressed after reception</param>
        /// <param name="serverName">The name of the machine hosting the server pipe</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="msTimeout">The number of milliseconds to wait to establish a connection</param>
        /// <returns>The Task associated with this action</returns>
        public static async Task PostAsync(string serverName, string pipeName, byte[] bytes, bool useCompression, int msTimeout)
        {
            var connectionResult = CreateClient(serverName, pipeName, msTimeout);
            if (connectionResult.Result != null)
            {
                using (var client = connectionResult.Result)
                {
                    await client.PostAsync(bytes, useCompression);
                }
            }
        }
        /// <summary>
        /// Asynchronously posts a messages to, and waits for a response from, a remote pipe in a single method call
        /// </summary>
        /// <param name="bytes">The data to send to the server</param>
        /// <param name="useCompression">If True, the data will be compressed before sending and decompressed after reception</param>
        /// <param name="serverName">The name of the machine hosting the server pipe</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="msTimeout">The number of milliseconds to wait to establish a connection</param>
        /// <returns>The Task associated with this action</returns>
        public static async Task<byte[]> RequestAsync(string serverName, string pipeName, byte[] bytes, bool useCompression, int msTimeout)
        {
            var connectionResult = CreateClient(serverName, pipeName, msTimeout);
            if (connectionResult.Result != null)
            {
                using (var client = connectionResult.Result)
                {
                    return await client.RequestAsync(bytes, useCompression, msTimeout);
                }
            }
            return null;
        }
        /// <summary>
        /// Asynchronously posts a messages to, and waits for a response from, a remote pipe in a single method call
        /// </summary>
        /// <param name="item">The item to send to the server</param>
        /// <param name="useCompression">If True, the data will be compressed before sending and decompressed after reception</param>
        /// <param name="serverName">The name of the machine hosting the server pipe</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="msTimeout">The number of milliseconds to wait to establish a connection</param>
        /// <returns>The Task associated with this action</returns>
        public static async Task<TType> RequestAsync<TType>(string serverName, string pipeName, TType item, bool useCompression, int msTimeout) where TType : class
        {
            var connectionResult = CreateClient(serverName, pipeName, msTimeout);
            if (connectionResult.Result == null)
                return default(TType);
            using (var client = connectionResult.Result)
            {
                return await client.RequestAsync<TType>(item, useCompression, msTimeout);
            }
        }
    }
}
