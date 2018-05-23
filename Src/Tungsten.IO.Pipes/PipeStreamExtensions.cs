//using System;
//using System.IO.Pipes;
//using System.Threading.Tasks;

//namespace W.IO.Pipes
//{
//    //used internally by PipeReadWriteExtensions
//    internal static class PipeStreamExtensions
//    {
//        public static bool Write(this PipeStream stream, byte[] bytes, out Exception exception)
//        {
//            exception = null;
//            try
//            {
//                if (Helpers.Write(stream, BitConverter.GetBytes(bytes.Length)))
//                    return Helpers.Write(stream, bytes);
//            }
//            catch (Exception e)
//            {
//                exception = e;
//            }
//            return false;
//        }
//        public static bool Write(this PipeStream stream, byte[] bytes, int offset, int count, out Exception exception)
//        {
//            exception = null;
//            try
//            {
//                if (Helpers.Write(stream, BitConverter.GetBytes(count)))
//                    return Helpers.Write(stream, bytes, offset, count);
//            }
//            catch (Exception e)
//            {
//                exception = e;
//            }
//            return false;
//        }
//        public static async Task<bool> WriteAsync(this PipeStream stream, byte[] bytes, Action<Exception> onException = null)
//        {
//            try
//            {
//                if (await Helpers.WriteAsync(stream, BitConverter.GetBytes(bytes.Length)))
//                    return await Helpers.WriteAsync(stream, bytes);
//            }
//            catch (Exception e)
//            {
//                onException?.Invoke(e);
//            }
//            return false;
//        }
//        public static async Task<bool> WriteAsync(this PipeStream stream, byte[] bytes, int offset, int count, Action<Exception> onException)
//        {
//            try
//            {
//                if (await Helpers.WriteAsync(stream, BitConverter.GetBytes(count)))
//                    return await Helpers.WriteAsync(stream, bytes, offset, count);
//            }
//            catch (Exception e)
//            {
//                onException?.Invoke(e);
//            }
//            return false;
//        }

//        public static byte[] Read(this PipeStream stream, out Exception exception)
//        {
//            var result = Execute(() =>
//            {
//                var bytes = Helpers.Read(stream);
//                return bytes;
//            }, out Exception e);
//            exception = e;
//            return result;
//        }
//        public static async Task<byte[]> ReadAsync(this PipeStream stream, Action<Exception> onException)
//        {
//            var result = await Execute(async () =>
//            {
//                var bytes = await Helpers.ReadAsync(stream);
//                return bytes;
//            }, out Exception e);
//            if (e != null)
//                onException?.Invoke(e);
//            return result;
//        }

//        internal static T Execute<T>(Func<T> function, out Exception exception)
//        {
//            exception = null;
//            try
//            {
//                return function.Invoke();
//            }
//            catch (TaskCanceledException e)
//            {
//                exception = e;
//                //System.Diagnostics.Debugger.Break();
//                System.Diagnostics.Debug.WriteLine("PipeStreamExtensions.Execute: TaskCanceledException");
//            }
//            catch (OperationCanceledException e)
//            {
//                exception = e;
//                //System.Diagnostics.Debugger.Break();
//                System.Diagnostics.Debug.WriteLine("PipeStreamExtensions.Execute: OperationCanceledException");
//            }
//            catch (System.ObjectDisposedException e)
//            {
//                exception = e;
//                //System.Diagnostics.Debugger.Break();
//                System.Diagnostics.Debug.WriteLine("PipeStreamExtensions.Execute: ObjectDisposedException");
//            }
//            catch (InvalidOperationException e) //pipe is in a disconnected state
//            {
//                exception = e;
//                System.Diagnostics.Debugger.Break();
//                System.Diagnostics.Debug.WriteLine("PipeStreamExtensions.Execute: InvalidOperationException");
//            }
//#if NET45
//            catch (System.Threading.ThreadAbortException e)
//            {
//                exception = e;
//                System.Threading.Thread.ResetAbort();
//                System.Diagnostics.Debugger.Break();
//                System.Diagnostics.Debug.WriteLine("PipeStreamExtensions.Execute: ThreadAbortException");
//            }
//#endif
//            catch (Exception e)
//            {
//                exception = e;
//                System.Diagnostics.Debugger.Break();
//                System.Diagnostics.Debug.WriteLine(e.ToString());
//                System.Diagnostics.Debug.WriteLine("PipeStreamExtensions.Execute: General Exception");
//            }
//            return default(T);
//        }
//    }
//}
