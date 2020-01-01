using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    internal static class Helpers
    {
        public static int GetOutBufferSize(PipeStream stream = null, int defaultValue = 4096)
        {
            try
            {
                return stream?.OutBufferSize > 0 ? stream.OutBufferSize : defaultValue;
            }
            catch (ObjectDisposedException)
            {
                return 256;
            }
            catch
            {
                return 256;
            }
        }
        public static int GetInBufferSize(PipeStream stream = null, int defaultValue = 4096)
        {
            try
            {
                return stream?.InBufferSize > 0 ? stream.InBufferSize : defaultValue;
            }
            catch (ObjectDisposedException)
            {
                return 256;
            }
            catch
            {
                return 256;
            }
        }
        public static int GetMaxConnections()
        {
#if NET45
            return NamedPipeServerStream.MaxAllowedServerInstances;
#else
            return 254;
#endif
        }

        public static void DisconnectAndDispose(PipeStream stream)
        {
            var client = stream as NamedPipeClientStream;
            if (client != null)
                DisconnectAndDispose(client);
            var server = stream as NamedPipeServerStream;
            if (server != null)
                DisconnectAndDispose(server);
        }
        internal static void DisconnectAndDispose(NamedPipeClientStream stream)
        {
            try
            {
#if NET45
                stream?.Close();
#endif
                stream?.Dispose();
            }
            catch (ObjectDisposedException) { System.Diagnostics.Debugger.Break(); }
            catch { }
        }
        internal static void DisconnectAndDispose(NamedPipeServerStream stream)
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
            catch (ObjectDisposedException) { System.Diagnostics.Debugger.Break(); }
            catch { }
        }

        public static bool Connect(NamedPipeClientStream stream, int msTimeout)
        {
            try
            {
                stream.Connect(msTimeout);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }
        public static NamedPipeClientStream CreateClientAndConnect(string serverName, string pipeName, System.Security.Principal.TokenImpersonationLevel tokenImpersonationLevel, int msTimeout, out Exception e)
        {
            NamedPipeClientStream stream = null;
            e = null;
            try
            {
                stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous, tokenImpersonationLevel);
            }
            catch (Exception ex)
            {
                e = ex;
                return null;
            }

            try
            {
                stream.Connect(msTimeout);
                stream.ReadMode = PipeTransmissionMode.Message; //not available until after connected
            }
            catch (TimeoutException ex)
            {
                stream.Dispose();
                e = ex;
                stream = null;
            }
            catch (Exception ex)
            {
                e = ex;
                stream?.Dispose();
                stream = null;
            }
            return stream;
        }
        public static NamedPipeServerStream CreateServer(string pipeName, int maxConnections, out Exception e)
        {
            NamedPipeServerStream stream = null;
            e = null;
            try
            {
#if NET45
                var pipeSecurity = new PipeSecurity();
                pipeSecurity.AddAccessRule(new PipeAccessRule(System.Security.Principal.WindowsIdentity.GetCurrent().User, PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
                pipeSecurity.AddAccessRule(new PipeAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.AuthenticatedUserSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
                stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Message, PipeOptions.Asynchronous, Helpers.GetInBufferSize(), Helpers.GetOutBufferSize(), pipeSecurity);
#else
                stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
#endif
            }
            catch (Exception ex)
            {
                e = ex;
            }
            return stream;
        }
        public static async Task WaitForClientToConnectAsync(NamedPipeServerStream server, CancellationToken token, Action onConnect)
        {
#if NET45
            await Task.Run(() =>
            {
                var result = false;
                server?.BeginWaitForConnection(ar =>
                {
                    var stream = ar.AsyncState as NamedPipeServerStream;
                    try
                    {
                        stream.EndWaitForConnection(ar);
                        result = stream.IsConnected;
                        if (stream.IsConnected)
                            onConnect?.Invoke();
                    }
                    catch (ObjectDisposedException) { stream.Close(); stream.Dispose(); } //pipe was closed
                    catch (System.Threading.ThreadAbortException) //can happen when the calling thread is aborted
                    {
                        System.Threading.Thread.ResetAbort();
                        stream.Dispose();
                    }
                }, server);
            });
#else
            await server.WaitForConnectionAsync(token).ContinueWith(task =>
            {
                if (server.IsConnected)
                    onConnect?.Invoke();
                //return server.IsConnected;
            }).ConfigureAwait(false);
#endif
        }

        public static bool Write(PipeStream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            return true;
        }
        public static bool Write(PipeStream stream, byte[] bytes, int offset, int count)
        {
            stream.Write(bytes, offset, count);
            stream.Flush();
            return true;
        }
        public static async Task<bool> WriteAsync(PipeStream stream, byte[] bytes)
        {
            if (!stream?.IsConnected ?? true)
                throw new InvalidOperationException("The pipe is not connected");
            return await WriteAsync(stream, bytes, 0, bytes.Length);
        }
        public static async Task<bool> WriteAsync(PipeStream stream, byte[] bytes, int offset, int count)
        {
            if (!stream?.IsConnected ?? true)
                throw new InvalidOperationException("The pipe is not connected");
            await stream.WriteAsync(bytes, offset, count);
            await stream.FlushAsync();
            return true;
        }

        public static byte[] Read(PipeStream stream, int msTimeout = -1)
        {
            return ReadAsync(stream, msTimeout).Result;
            //byte[] bytes = null;
            //using (var thread = new W.Threading.ThreadMethod((token, args) =>
            //{
            //    while (!token.IsCancellationRequested)
            //    {
            //        bytes = ReadAsync(stream, msTimeout).Result;
            //    }
            //}))
            //{
            //    thread.Wait(msTimeout);
            //}
            //return bytes;
        }
        public static async Task<byte[]> ReadAsync(PipeStream stream, int msTimeout = -1)
        {
            byte[] receivedMessage = null;
            var cache = new byte[GetInBufferSize(stream)];
            var bytesRead = 0;
            int numberOfReads = 0;

            try
            {
                if (!stream?.IsConnected ?? true)
                    throw new InvalidOperationException("The pipe is not connected");
                do
                {
                    //11.2.2017 - https://stackoverflow.com/a/12431633
                    //It turns out that the CancellationToken is completely ignored.  You have to cancel an outside task.
                    //var read = await _stream.ReadAsync(cache, 0, cache.Length).ConfigureAwait(false);
                    var read = await stream.ReadAsync(cache, 0, cache.Length, msTimeout == -1 ? CancellationToken.None : new CancellationTokenSource(msTimeout).Token);//.ConfigureAwait(false);

                    numberOfReads += 1;
                    if (read > 0)
                    {
                        //System.Diagnostics.Debug.WriteLine(nameof(WaitForMessageAsync) + " received {0} bytes", read);
                        Array.Resize(ref receivedMessage, bytesRead + read);
                        Array.Copy(cache, 0, receivedMessage, bytesRead, read);
                        bytesRead += read;
                    }
                } while (!stream.IsMessageComplete);
            }
            catch (ObjectDisposedException)
            {
                //System.Diagnostics.Debugger.Break();
                receivedMessage = null;
            }
            catch (InvalidOperationException) //stream is in an invalid state to check IsMessageComplete
            {
                //System.Diagnostics.Debugger.Break();
                receivedMessage = null;
            }
            return receivedMessage;
        }
        public static async Task<byte[]> ReadAsync(PipeStream stream, CancellationToken token)
        {
            byte[] receivedMessage = null;
            var cache = new byte[GetInBufferSize(stream)];
            var bytesRead = 0;
            int numberOfReads = 0;

            try
            {
                if (!stream?.IsConnected ?? true)
                    throw new InvalidOperationException("The pipe is not connected");
                do
                {
                    //11.2.2017 - https://stackoverflow.com/a/12431633
                    //It turns out that the CancellationToken is completely ignored.  You have to cancel an outside task.
                    //var read = await _stream.ReadAsync(cache, 0, cache.Length).ConfigureAwait(false);
                    var read = await stream.ReadAsync(cache, 0, cache.Length, token);//.ConfigureAwait(false);

                    numberOfReads += 1;
                    if (read > 0)
                    {
                        //System.Diagnostics.Debug.WriteLine(nameof(WaitForMessageAsync) + " received {0} bytes", read);
                        Array.Resize(ref receivedMessage, bytesRead + read);
                        Array.Copy(cache, 0, receivedMessage, bytesRead, read);
                        bytesRead += read;
                    }
                } while (!stream.IsMessageComplete);
            }
            catch (ObjectDisposedException)
            {
                //System.Diagnostics.Debugger.Break();
                receivedMessage = null;
            }
            catch (InvalidOperationException) //stream is in an invalid state to check IsMessageComplete
            {
                //System.Diagnostics.Debugger.Break();
                receivedMessage = null;
            }
            return receivedMessage;
        }

        public static void CreateListeningServerStream(string pipeName, int maxConnections, CancellationToken cancelToken, Action<Exception> onCreateException, Action<PipeStream> onConnect)
        {
            PipeStream Stream;
            Stream = CreateServer(pipeName, maxConnections, out Exception e);
            if (e != null)
                onCreateException?.Invoke(e);
            else
            {
                WaitForClientToConnectAsync((NamedPipeServerStream)Stream, cancelToken, () =>
                {
                    onConnect?.Invoke(Stream);
                }).Wait();
            }
        }
    }
}
