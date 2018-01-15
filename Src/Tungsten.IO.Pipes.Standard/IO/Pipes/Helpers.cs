using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.AsExtensions;

namespace W.IO.Pipes
{
    /// <summary>
    /// A few methods to facilitate pipe functionality
    /// </summary>
    public static partial class Helpers
    {
        /// <summary>
        /// Retrieve the OutBufferSize for the given PipeStream
        /// </summary>
        /// <param name="stream">The PipeStream on which to determine the value</param>
        /// <param name="defaultValue">The value to use if the platform returns 0</param>
        /// <returns>If supported, the stream's OutBufferSize, defaultValue if the OutBufferSize reports 0, and 256 if an exception occurs while trying to obtain the value.</returns>
        public static int GetOutBufferSize(PipeStream stream = null, int defaultValue = 4096)
        {
            try
            {
                return stream?.OutBufferSize > 0 ? stream.OutBufferSize : defaultValue;
            }
            catch
            {
                return 256;
            }
        }
        /// <summary>
        /// Retrieve the InBufferSize for the given PipeStream
        /// </summary>
        /// <param name="stream">The PipeStream on which to determine the value</param>
        /// <param name="defaultValue">The value to use if the platform returns 0</param>
        /// <returns>If supported, the stream's InBufferSize, defaultValue if the InBufferSize reports 0, and 256 if an exception occurs while trying to obtain the value.</returns>
        public static int GetInBufferSize(PipeStream stream = null, int defaultValue = 4096)
        {
            try
            {
                return stream?.InBufferSize > 0 ? stream.InBufferSize : defaultValue;
            }
            catch
            {
                return 256;
            }
        }
        /// <summary>
        /// Creates a new NamedPipeServerStream
        /// </summary>
        /// <param name="pipeName">The name of the pipe to create</param>
        /// <param name="maxConnections">The maximum number of concurrent connections.  This value can be limited by your platform.</param>
        /// <param name="onCreated">Called when the NamedPipeServerStream has been created and right before it starts listening for a client connection</param>
        /// <param name="onClientConnected">Called when a client has connected to the server</param>
        public static void CreateServer(string pipeName, int maxConnections, Action<NamedPipeServerStream> onCreated, Action<NamedPipeServerStream> onClientConnected)
        {
#if NET45
            var pipeSecurity = new PipeSecurity();
            pipeSecurity.AddAccessRule(new PipeAccessRule(System.Security.Principal.WindowsIdentity.GetCurrent().User, PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            pipeSecurity.AddAccessRule(new PipeAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.AuthenticatedUserSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
            var Stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Message, PipeOptions.Asynchronous, GetInBufferSize(), GetOutBufferSize(), pipeSecurity);
            onCreated?.Invoke(Stream);
            //try
            //{
                Stream?.BeginWaitForConnection(ar =>
                {
                    try
                    {
                        Stream.EndWaitForConnection(ar);
                        if (Stream.IsConnected)
                        {
                            onClientConnected?.Invoke(Stream);
                        }
                    }
                    catch(System.Threading.ThreadAbortException) //can happen when the calling thread is aborted
                    {
                        System.Threading.Thread.ResetAbort();
                        Stream.Dispose();
                    }
                    catch (ObjectDisposedException) //pipe was closed
                    {
                        Stream.Dispose();
                    }
                }, Stream);
            //}
            //catch (Exception e)
            //{
            //    Stream.Dispose();
            //    Stream = null;
            //    System.Diagnostics.Debugger.Break();//what's causing this to break;?
            //    System.Diagnostics.Debug.WriteLine(e.ToString());
            //}
#elif NETSTANDARD1_4
            var Stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            onCreated?.Invoke(Stream);
            Task.Factory.StartNew(async () =>
            {
                //try
                {
                    await Stream.WaitForConnectionAsync().ContinueWith(task =>
                    {
                        if (!task.IsCanceled && !task.IsFaulted && Stream.IsConnected)
                        {
                            onClientConnected?.Invoke(Stream);
                        }
                    });//.ConfigureAwait(false);
                }
                //catch (ObjectDisposedException) //pipe closed
                //{
                //    Stream.Dispose();
                //    Stream = null;
                //}
                //catch (Exception e)
                //{
                //    System.Diagnostics.Debugger.Break();
                //    System.Diagnostics.Debug.WriteLine(e.ToString());
                //    Stream.Dispose();
                //    Stream = null;
                //}
            }, TaskCreationOptions.LongRunning).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// Disconnects the PipeStream and disposes it
        /// </summary>
        /// <param name="stream">The PipeStream to disconnect and dispose</param>
        public static void DisconnectAndDispose(PipeStream stream)
        {
            var client = stream.As<NamedPipeClientStream>();
            if (client != null)
                DisconnectAndDispose(client);
            var server = stream.As<NamedPipeServerStream>();
            if (server != null)
                DisconnectAndDispose(server);
        }
        /// <summary>
        /// Disconnects and disposes a NamedPipeClientStream
        /// </summary>
        /// <param name="stream">The NamedPipeClientStream to disconnect and dispose</param>
        public static void DisconnectAndDispose(NamedPipeClientStream stream)
        {
            try
            {
#if NET45
                stream?.Close();
#endif
                stream?.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// Disconnects and disposes a NamedPipeServerStream
        /// </summary>
        /// <param name="stream">The NamedPipeServerStream to disconnect and dispose</param>
        public static void DisconnectAndDispose(NamedPipeServerStream stream)
        {
            try
            {
#if NET45
                if (stream?.IsConnected ?? false)
                    stream?.Disconnect();
                stream?.Close();
#endif
                stream?.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// Connects and returns a NamedPipeClientStream to a remote NamedPipeServerStream
        /// </summary>
        /// <param name="serverName">The name of the machine hosting the NamedPipeServerStream</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="tokenImpersonationLevel">The impersonation level to use</param>
        /// <param name="msTimeout">The number of milliseconds to wait for the server to connected</param>
        /// <returns>A PipeResult containing the success of the call, exception information if available, and actual NamedPipeClientStream on success</returns>
        public static PipeResult<NamedPipeClientStream> Connect(string serverName, string pipeName, System.Security.Principal.TokenImpersonationLevel tokenImpersonationLevel, int msTimeout)
        {
            var result = new PipeResult<NamedPipeClientStream>() { Success = false };
            var stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous, tokenImpersonationLevel);
            try
            {
                stream.Connect(msTimeout);
            }
            catch (TimeoutException e)
            {
                result.Exception = e;
                return result;
            }
            catch
            {
                stream.Dispose();
            }
            if (stream.IsConnected)
            {
                stream.ReadMode = PipeTransmissionMode.Message; //not available until after connected
                result.Success = true;
                result.Status = PipeStatusEnum.Connected;
                result.Result = stream;
            }
            else
                stream.Dispose();
            return result;
        }
        //public static PipeResult WaitForMessage(PipeStream stream, int msTimeout)
        //{
        //    var result = new PipeResult();
        //    var cache = new byte[GetInBufferSize(stream)];
        //    var bytesRead = 0;
        //    byte[] receivedMessage = null;

        //    try
        //    {
        //        do
        //        {
        //            //11.2.2017 - https://stackoverflow.com/a/12431633
        //            //It turns out that the CancellationToken is completely ignored.  You have to cancel an outside task.
        //            var read = stream.ReadAsync(cache, 0, cache.Length, new CancellationTokenSource(msTimeout).Token).Result;//.ConfigureAwait(false);
        //            //var read = await _stream.ReadAsync(cache, 0, cache.Length).ConfigureAwait(false);
        //            if (read > 0)
        //            {
        //                //System.Diagnostics.Debug.WriteLine(nameof(WaitForMessage) + " received {0} bytes", read);
        //                Array.Resize(ref receivedMessage, bytesRead + read);
        //                Array.Copy(cache, 0, receivedMessage, bytesRead, read);
        //                bytesRead += read;
        //            }
        //        } while (!stream.IsMessageComplete);
        //        if (receivedMessage != null)
        //        {
        //            result.Result = receivedMessage;
        //            result.Status = PipeStatusEnum.Connected;
        //            result.Success = true;
        //        }
        //    }
        //    catch (AggregateException e) //task cancelled
        //    {
        //        result.Exception = e;
        //        result.Status = PipeStatusEnum.Connected;
        //    }
        //    catch (TaskCanceledException e)
        //    {
        //        result.Exception = e;
        //        result.Status = PipeStatusEnum.Connected;
        //    }
        //    catch (ObjectDisposedException e)
        //    {
        //        result.Exception = e;
        //        result.Status = PipeStatusEnum.Disconnected;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debugger.Break();
        //        System.Diagnostics.Debug.WriteLine(e.ToString());
        //        result.Exception = e;
        //        result.Status = PipeStatusEnum.Disconnected;
        //    }
        //    return result;
        //}
        //public static PipeResult RequestAResponse(PipeStream stream, byte[] request, int msTimeout)
        //{
        //    stream.Write(request, 0, request.Length);
        //    stream.Flush();
        //    //stream.WaitForPipeDrain();
        //    var response = WaitForMessage(stream, msTimeout);
        //    return response;
        //}

        /// <summary>
        /// Asynchronously connects a NamedPipeClientStream to a listening NamedPipeServerStream
        /// </summary>
        /// <param name="serverName">The name of the machine hosting the NamePipeServerStream</param>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="msTimeout">The number of milliseconds to wait for the server to connect</param>
        /// <returns></returns>
        public static async Task<PipeResult<NamedPipeClientStream>> ConnectAsync(string serverName, string pipeName, int msTimeout)
        {
            var result = new PipeResult<NamedPipeClientStream>() { Success = false };
            var stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
#if NET45
            await Task.Run(() =>
            {
                stream.Connect(msTimeout);//.ConfigureAwait(false);
            }).ContinueWith(task =>
            {
                result.Exception = task.Exception;
                if (!task.IsCanceled && !task.IsFaulted && stream.IsConnected)
                {
                    stream.ReadMode = PipeTransmissionMode.Message; //not available until after connected
                    result.Success = true;
                    result.Result = result.Success ? stream : null;
                }
                else
                    stream.Dispose();
            });
#elif NETSTANDARD1_4
            //stream.Connect(msTimeout);
            //if (stream.IsConnected)
            await stream.ConnectAsync(msTimeout).ContinueWith(task =>
            {
                result.Exception = task.Exception;
                if (!task.IsCanceled && !task.IsFaulted && stream.IsConnected)
                {
                    stream.ReadMode = PipeTransmissionMode.Message; //not available until after connected
                    result.Success = true;
                    result.Status = PipeStatusEnum.Connected;
                    result.Result = stream;
                }
                else
                {
                    stream.Dispose();
                }
            });
#endif
            return result;
        }
        /// <summary>
        /// Asynchronously waits for data from the PipeStream
        /// </summary>
        /// <param name="stream">The PipeStream on which to wait for data</param>
        /// <param name="msTimeout">The number of milliseconds to wait for the data</param>
        /// <returns>The data received, or null if a timeout occurred</returns>
        public static async Task<PipeResult> WaitForMessageAsync(PipeStream stream, int msTimeout)
        {
            var result = new PipeResult();
            var cache = new byte[GetInBufferSize(stream)];
            var bytesRead = 0;
            byte[] receivedMessage = null;
            int numberOfReads = 0;
            try
            {
                do
                {
                    //11.2.2017 - https://stackoverflow.com/a/12431633
                    //It turns out that the CancellationToken is completely ignored.  You have to cancel an outside task.
                    var read = await stream.ReadAsync(cache, 0, cache.Length, new CancellationTokenSource(msTimeout).Token);//.ConfigureAwait(false);
                                                                                                                            //var read = await _stream.ReadAsync(cache, 0, cache.Length).ConfigureAwait(false);
                    numberOfReads += 1;
                    if (read > 0)
                    {
                        //System.Diagnostics.Debug.WriteLine(nameof(WaitForMessageAsync) + " received {0} bytes", read);
                        Array.Resize(ref receivedMessage, bytesRead + read);
                        Array.Copy(cache, 0, receivedMessage, bytesRead, read);
                        bytesRead += read;
                    }
                } while (!stream.IsMessageComplete);
                if (receivedMessage != null)
                {
                    result.Result = receivedMessage;
                    result.Status = PipeStatusEnum.Connected;
                    result.Success = true;
                }
            }
            catch (TaskCanceledException e)
            {
                result.Exception = e;
                result.Status = PipeStatusEnum.Disconnected;
            }
            catch (ObjectDisposedException e)
            {
                result.Exception = e;
                result.Status = PipeStatusEnum.Disconnected;
            }
            catch (InvalidOperationException e) //pipe is in a disconnected state
            {
                result.Exception = e;
                result.Status = PipeStatusEnum.Disconnected;
            }
#if NET45
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
                System.Diagnostics.Debugger.Break();
            }
#endif
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
                result.Exception = e;
                result.Status = PipeStatusEnum.Disconnected;
            }
            return result;
        }
        //public static async Task<PipeResult> RequestAResponseAsync(PipeStream stream, byte[] request, int msTimeout)
        //{
        //    stream.Write(request, 0, request.Length);
        //    stream.Flush();
        //    //stream.WaitForPipeDrain();
        //    var response = await WaitForMessageAsync(stream, msTimeout);
        //    return response;
        //}
        //public static async Task<PipeResult<bool>> IsDataAvailableAsync(PipeStream stream, int msTimeout)
        //{
        //    //duplicate of IsDataAvailableAsync (with a CancellationToken), except that this token is created locally
        //    //the token is created in the Task call to ensure a timeout period as accurate as possible
        //    //TODO: Make an IsDataAvailable for NetStandard
        //    var result = new PipeResult<bool>();
        //    var read = 0;
        //    var cache = new byte[4];
        //    try
        //    {
        //        //this works in .Net, but not .NetStandard
        //        await Task.Run(async () =>
        //        {
        //            read = await stream.ReadAsync(cache, 0, 0, CancellationToken.None);
        //        }, new CancellationTokenSource(msTimeout).Token).ContinueWith(task =>
        //        {
        //            result.Exception = task.Exception;
        //            result.Result = !stream.IsMessageComplete;
        //            result.Status = stream.IsConnected ? PipeStatusEnum.Connected : PipeStatusEnum.Disconnected;
        //            result.Success = true;
        //        });
        //    }
        //    catch (TaskCanceledException)
        //    {
        //        //System.Diagnostics.Debugger.Break();
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        //System.Diagnostics.Debugger.Break();
        //    }
        //    catch (ObjectDisposedException) //pipe closed
        //    {
        //        result.Status = PipeStatusEnum.Disconnected;
        //    }
        //    return result;
        //}
        //public static async Task<PipeResult<bool>> IsDataAvailableAsync(PipeStream stream, CancellationToken token)
        //{
        //    //TODO: Make an IsDataAvailable for NetStandard
        //    var result = new PipeResult<bool>();
        //    var read = 0;
        //    var cache = new byte[4];
        //    try
        //    {
        //        //this works in .Net, but not .NetStandard
        //        await Task.Run(async () =>
        //        {
        //            read = await stream.ReadAsync(cache, 0, 0, CancellationToken.None);
        //        }, token).ContinueWith(task =>
        //        {
        //            result.Exception = task.Exception;
        //            result.Result = !stream.IsMessageComplete;
        //            result.Status = stream.IsConnected ? PipeStatusEnum.Connected : PipeStatusEnum.Disconnected;
        //            result.Success = true;
        //        });

        //    }
        //    catch (TaskCanceledException)
        //    {
        //        //System.Diagnostics.Debugger.Break();
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        //System.Diagnostics.Debugger.Break();
        //    }
        //    catch (ObjectDisposedException) //pipe closed
        //    {
        //        result.Status = PipeStatusEnum.Disconnected;
        //    }
        //    return result;
        //}

        //public static async Task<PipeStatusEnum> WaitForMessagesAsync(PipeStream stream, Action<byte[]> messageReceived, int msTimeout)
        //{
        //    var cts = new CancellationTokenSource(msTimeout);
        //    return await WaitForMessagesAsync(stream, messageReceived, cts.Token);
        //}
        //public static async Task<PipeStatusEnum> WaitForMessagesAsync(PipeStream stream, Action<byte[]> messageReceived, CancellationToken token)
        //{
        //    var result = PipeStatusEnum.Connected;
        //    while (!token.IsCancellationRequested)
        //    {
        //        await WaitForMessageAsync(stream, token).ContinueWith(task =>
        //        {
        //            if (task.Result.Status == PipeStatusEnum.Disconnected) //disconnected
        //            {
        //                result = PipeStatusEnum.Disconnected;
        //                return;
        //            }
        //            messageReceived.Invoke(task.Result.Result);
        //        });
        //    }
        //    return result;
        //}
        //public static PipeResult RequestAResponse(PipeStream stream, byte[] request, int msTimeout)
        //{
        //    var cts = new CancellationTokenSource(msTimeout);
        //    return RequestAResponse(stream, request, cts.Token);
        //}
        //private static async Task WriteMessageSizeAsync(PipeStream stream, int length)
        //{
        //    //send the size in it's own message
        //    var sizeBuffer = BitConverter.GetBytes(length); //4 bytes
        //    await stream.WriteAsync(sizeBuffer, 0, 4);
        //    await stream.FlushAsync();
        //    stream.WaitForPipeDrain();
        //}
        //private static async Task WriteMessageChunksAsync(PipeStream stream, byte[] bytes)
        //{
        //    //send the bytes in as many messages as necessary
        //    int numberOfBytesSent = 0;
        //    int length = bytes.Length;
        //    while (numberOfBytesSent < length)
        //    {
        //        var bytesToSend = 0;
        //        if (numberOfBytesSent < length)
        //            bytesToSend = Math.Min(PipeMethods.GetOutBufferSize(stream), length - numberOfBytesSent);
        //        await stream.WriteAsync(bytes, numberOfBytesSent, bytesToSend);
        //        //await stream.FlushAsync();
        //        //stream.WaitForPipeDrain();
        //        numberOfBytesSent += bytesToSend;
        //    }
        //    await stream.FlushAsync();
        //    stream.WaitForPipeDrain();
        //}
        //public static async Task WriteCompleteMessageAsync(PipeStream stream, byte[] bytes)
        //{
        //    //send the size
        //    await WriteMessageSizeAsync(stream, bytes.Length);
        //    //send the message bytes
        //    await WriteMessageChunksAsync(stream, bytes);
        //}
    }
}
