using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// Extension methods for PipeClient to provide additional functionality (in this case, also the base functionality)
    /// </summary>
    internal static class PipeClientExtensions
    {
        internal static async Task PostAsync(this PipeClient pipe, byte[] bytes, bool useCompression, bool useEvent)
        {
            ManualResetEventSlim mre = null;
            if (useEvent)
                mre = new ManualResetEventSlim(false);
            try
            {
                if (useCompression)
                    bytes = bytes.AsCompressed();
                await pipe.Stream.WriteAsync(bytes, 0, bytes.Length);//.ConfigureAwait(false);
                await pipe.Stream.FlushAsync();//.ConfigureAwait(false);
                mre?.Set();
            }
            catch (ObjectDisposedException) //disconnected
            {
                pipe.Disconnect();
                mre?.Set();
            }
            catch
            {
                mre?.Set();
                throw;
            }
            finally
            {
                mre?.Wait();
                mre?.Dispose();
            }
        }
        internal static async Task<byte[]> WaitForMessageAsync(this PipeClient pipe, bool useCompression, int msTimeout, bool useEvent)
        {
            byte[] result = null;
            ManualResetEventSlim mre = null;
            if (useEvent)
                mre = new ManualResetEventSlim(false);
            try
            {
                Task<PipeResult> waitResult = null;
                waitResult = Helpers.WaitForMessageAsync(pipe.Stream, msTimeout);
                await waitResult;
                if (waitResult.Result.Success)
                    result = useCompression ? waitResult.Result.Result.FromCompressed() : waitResult.Result.Result;
                mre?.Set();
            }
#if NET45
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
                System.Diagnostics.Debugger.Break();
                mre?.Set();
            }
#endif
            catch (ObjectDisposedException) //disconnected
            {
                pipe.Disconnect();
                mre?.Set();
            }
            catch
            {
                mre?.Set();
                throw;
            }
            finally
            {
                mre?.Wait();
                mre?.Dispose();
            }
            return result;
        }
        internal static async Task<byte[]> RequestAsync(this PipeClient pipe, byte[] bytes, bool useCompression, int msTimeout, bool useEvent)
        {
            byte[] result = null;
            ManualResetEventSlim mre = null;
            if (useEvent)
                mre = new ManualResetEventSlim(false);
            try
            {
                await pipe.PostAsync(bytes, useCompression, useEvent);
                result = await pipe.WaitForMessageAsync(useCompression, msTimeout, useEvent);
                mre?.Set();
            }
            catch (ObjectDisposedException) //disconnected
            {
                pipe.Disconnect();
                mre?.Set();
            }
            catch
            {
                mre?.Set();
                throw;
            }
            finally
            {
                mre?.Wait();
                mre?.Dispose();
            }
            return result;
        }

        internal static async Task PostAsync<TType>(this PipeClient pipe, TType item, bool useCompression, bool useEvent)
        {
            ManualResetEventSlim mre = null;
            if (useEvent)
                mre = new ManualResetEventSlim(false);
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                var bytes = json.AsBytes();
                await pipe.PostAsync(bytes, useCompression, useEvent);//.ConfigureAwait(false);
                mre?.Set();
            }
            catch (ObjectDisposedException) //disconnected
            {
                pipe.Disconnect();
                mre?.Set();
            }
            catch
            {
                mre?.Set();
                throw;
            }
            finally
            {
                mre?.Wait();
                mre?.Dispose();
            }
        }
        internal static async Task<TType> WaitForMessageAsync<TType>(this PipeClient pipe, bool useCompression, int msTimeout, bool useEvent)
        {
            var result = default(TType);
            ManualResetEventSlim mre = null;
            if (useEvent)
                mre = new ManualResetEventSlim(false);
            try
            {
                var bytes = await pipe.WaitForMessageAsync(useCompression, msTimeout, useEvent);
                if (bytes != null)
                {
                    var json = bytes.AsString();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<TType>(json);
                    mre?.Set();
                }
            }
            catch (ObjectDisposedException) //disconnected
            {
                pipe.Disconnect();
                mre?.Set();
            }
            catch
            {
                mre?.Set();
                throw;
            }
            finally
            {
                mre?.Wait();
                mre?.Dispose();
            }
            return result;
        }
        internal static async Task<TType> RequestAsync<TType>(this PipeClient pipe, TType item, bool useCompression, int msTimeout, bool useEvent)
        {
            var result = default(TType);
            ManualResetEventSlim mre = null;
            if (useEvent)
                mre = new ManualResetEventSlim(false);
            try
            {
                await pipe.PostAsync(item, useCompression, useEvent);
                result = await pipe.WaitForMessageAsync<TType>(useCompression, msTimeout, useEvent);
                mre?.Set();
            }
            catch (ObjectDisposedException) //disconnected
            {
                pipe.Disconnect();
                mre?.Set();
            }
            catch
            {
                mre?.Set();
                throw;
            }
            finally
            {
                mre?.Wait();
                mre?.Dispose();
            }
            return result;
        }
    }
}
