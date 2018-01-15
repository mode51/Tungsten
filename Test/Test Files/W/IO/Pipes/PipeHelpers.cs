using System;
using System.Collections.Generic;
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

namespace W.Tests
{
    internal class PipeHelpers
    {
        public struct PipeDatagram //(256 - 32) / (4096 - 32) / (8192 - 32) / etc
        {
            public Guid DatagramID; //16 bytes (16)
            public int TotalDataSize; // 4 bytes (20)
            public int NumberOfDatagrams; // 4 bytes (24)
            public int Index; // 4 bytes (28)
            public int DataLength; // 4 bytes (32)
            public byte[] Data;

            public byte[] AsBytes()
            {
                return PipeDatagramManager.FromDatagram(this, DataLength + SizeofMembers());
            }
            public void FromBytes(byte[] bytes, int datagramSize)
            {
                var datagram = PipeDatagramManager.AsDatagram(bytes, datagramSize);
                DatagramID = datagram.DatagramID;
                TotalDataSize = datagram.TotalDataSize;
                NumberOfDatagrams = datagram.NumberOfDatagrams;
                Index = datagram.Index;
                DataLength = datagram.DataLength;
                Data = datagram.Data;
            }

            public static int SizeofMembers()
            {
                var sample = new PipeDatagram();
                var size = Marshal.SizeOf(sample.DatagramID)
                       + Marshal.SizeOf(sample.TotalDataSize)
                       + Marshal.SizeOf(sample.NumberOfDatagrams)
                       + Marshal.SizeOf(sample.Index)
                       + Marshal.SizeOf(sample.DataLength);
                return size;
            }
            public static PipeDatagram Create(int datagramSize)
            {
                var result = new PipeDatagram();
                result.DatagramID = Guid.NewGuid();
                result.TotalDataSize = 0;
                result.NumberOfDatagrams = 0;
                result.Index = 0;
                result.DataLength = datagramSize - SizeofMembers();
                result.Data = new byte[result.DataLength];
                return result;
            }
        }
        public class PipeMessage
        {
            public Guid Id;
            public int TotalDataLength;
            public int ReceivedDataLength;
            public byte[] Data;

            public bool IsComplete
            {
                get
                {
                    return TotalDataLength == ReceivedDataLength;
                }
            }
            public static PipeMessage Empty
            {
                get
                {
                    return new PipeMessage() { Id = Guid.Empty, TotalDataLength = 0, ReceivedDataLength = 0, Data = null };
                }
            }
        }
        public class PipeDatagramManager
        {
            public static bool IsCompleteMessage(IEnumerable<PipeDatagram> datagrams)
            {
                var realTotalBytes = datagrams.Sum(d => d.TotalDataSize);
                var totalBytes = datagrams.ElementAt(0).TotalDataSize;
                return totalBytes == realTotalBytes;
            }
            public static int NumberOfDatagrams(int totalNumberOfBytes, int datagramSize)
            {
                return (int)(totalNumberOfBytes / datagramSize) + (totalNumberOfBytes % datagramSize > 0 ? 1 : 0);
            }
            public static List<PipeDatagram> AsDatagrams(Guid id, byte[] data, int datagramSize = 256)
            {
                var result = new List<PipeDatagram>();
                var datagramLength = data.Length;
                var remaining = data.Length;
                var offset = 0;
                datagramSize = datagramSize - 32;
                if (datagramSize < 33)
                    throw new ArgumentOutOfRangeException(nameof(datagramSize), "The datagram size must be larger than 32 bytes");

                var numberOfDatagrams = NumberOfDatagrams(data.Length, datagramSize);
                for (int t = 0; t < numberOfDatagrams; t++)
                {
                    offset = t * datagramSize;
                    var dg = new PipeDatagram();
                    dg.DatagramID = id;
                    dg.TotalDataSize = data.Length;
                    dg.NumberOfDatagrams = numberOfDatagrams;
                    dg.Index = t;
                    dg.DataLength = Math.Min(datagramSize, data.Length - offset);
                    dg.Data = new byte[data.Length];
                    Array.Copy(data, offset, dg.Data, 0, dg.DataLength);

                    result.Add(dg);
                }
                return result;
            }
            public static byte[] FromDatagrams(PipeDatagram[] datagrams, int datagramSize = 256)
            {
                var result = new byte[datagrams[0].TotalDataSize];
                int offset = 0;
                foreach (var d in datagrams)
                {
                    System.Buffer.BlockCopy(d.Data, 0, result, offset, d.DataLength);
                    offset += d.DataLength;
                }
                if (offset != result.Length)
                    throw new ArgumentOutOfRangeException(nameof(datagrams), "The total number of datagram bytes does not match the datagrams' specified total.");

                //if (datagrams?.Length == 0)
                //    return null;
                //var ordered = datagrams.OrderBy(d => d.Index);
                //var totalNumberOfBytes = ordered.ElementAt(0).TotalDataSize;
                //var numberOfDatagrams = NumberOfDatagrams(totalNumberOfBytes, datagramSize);
                //if (numberOfDatagrams != datagrams.Length)
                //    throw new IndexOutOfRangeException(string.Format("PipeDatagram count mismatch.  Found {0}/{1}.", datagrams.Length.ToString(), numberOfDatagrams));
                //byte[] result = new byte[totalNumberOfBytes];
                //var read = 0;
                //var index = 0;
                //foreach(var datagram in ordered)
                //{
                //    if (index != datagram.Index)
                //        throw new IndexOutOfRangeException("Missing PipeDatagram #" + index.ToString() + "/" );
                //    Array.Copy(datagram.Data, 0, result, read, datagram.DataLength);
                //    read += datagram.DataLength;
                //    index += 1;
                //}
                return result;
            }
            public static byte[] FromDatagram(PipeDatagram datagram, int datagramSize)
            {
                var result = new byte[datagramSize];
                //var ptr = Marshal.AllocHGlobal(datagramSize);
                //Marshal.StructureToPtr(datagram, ptr, false);
                //Marshal.Copy(ptr, result, 0, datagramSize);
                //Marshal.FreeHGlobal(ptr);
                Buffer.BlockCopy(datagram.DatagramID.ToByteArray(), 0, result, 0, 16);
                Buffer.BlockCopy(BitConverter.GetBytes(datagram.TotalDataSize), 0, result, 16, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(datagram.NumberOfDatagrams), 0, result, 20, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(datagram.Index), 0, result, 24, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(datagram.DataLength), 0, result, 28, 4);
                Buffer.BlockCopy(datagram.Data, 0, result, 32, datagram.DataLength);

                return result;
            }
            public static PipeDatagram AsDatagram(byte[] data, int datagramSize)
            {
                var result = new PipeDatagram();
                //var ptr = Marshal.AllocHGlobal(datagramSize);
                //Marshal.Copy(data, 0, ptr, datagramSize);
                //result = (PipeDatagram)Marshal.PtrToStructure(ptr, typeof(PipeDatagram));
                //Marshal.FreeHGlobal(ptr);

                result.DatagramID = new Guid(ArrayMethods.Peek(data, 0, 16));
                result.TotalDataSize = BitConverter.ToInt32(ArrayMethods.Peek(data, 16, 4), 0);
                result.NumberOfDatagrams = BitConverter.ToInt32(ArrayMethods.Peek(data, 20, 4), 0);
                result.Index = BitConverter.ToInt32(ArrayMethods.Peek(data, 24, 4), 0);
                result.DataLength = BitConverter.ToInt32(ArrayMethods.Peek(data, 28, 4), 0);
                result.Data = ArrayMethods.Peek(data, 32, result.DataLength);
                return result;
            }
        }
        public enum PipeReadResultEnum
        {
            None,
            Ok,
            Disconnected,
            Timeout
        }
        public static async Task WriteMessageSizeAsync(PipeStream stream, int length)
        {
            try
            {
                var lengthBuffer = BitConverter.GetBytes(length); //4 bytes
                await stream.WriteAsync(lengthBuffer, 0, lengthBuffer.Length, new CancellationTokenSource(1000).Token);
                await stream.FlushAsync();
                //stream.WaitForPipeDrain();
            }
            catch (OperationCanceledException) //timed out
            { }
            catch (Exception e) //remove this when I've handled all exceptions
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        public static async Task WriteMessageChunksAsync(PipeStream stream, byte[] bytes)
        {
            //send the bytes in as many messages as necessary
            int numberOfBytesSent = 0;
            int length = bytes.Length;
            int outBufferSize = PipeHelpers.GetOutBufferSize(stream);
            while (numberOfBytesSent < length)
            {
                var bytesToSend = 0;
                if (numberOfBytesSent < length)
                    bytesToSend = Math.Min(outBufferSize, length - numberOfBytesSent);
                await stream.WriteAsync(bytes, numberOfBytesSent, bytesToSend, CancellationToken.None);
                await stream.FlushAsync();
                //stream.WaitForPipeDrain();
                numberOfBytesSent += bytesToSend;
            }
            //await stream.FlushAsync();
            //stream.WaitForPipeDrain();
        }
        public static void WriteMessageChunks(PipeStream stream, byte[] bytes)
        {
            //send the bytes in as many messages as necessary
            int numberOfBytesSent = 0;
            int length = bytes.Length;
            int outBufferSize = PipeHelpers.GetOutBufferSize(stream);
            while (numberOfBytesSent < length)
            {
                var bytesToSend = 0;
                if (numberOfBytesSent < length)
                    bytesToSend = Math.Min(outBufferSize, length - numberOfBytesSent);
                stream.Write(bytes, numberOfBytesSent, bytesToSend);
                stream.Flush();
                //stream.WaitForPipeDrain();
                numberOfBytesSent += bytesToSend;
            }
            //await stream.FlushAsync();
            //stream.WaitForPipeDrain();
        }
        public static async Task WriteCompleteMessageAsync(PipeStream stream, byte[] bytes)
        {
            //send the size
            await WriteMessageSizeAsync(stream, bytes.Length);
            //send the message bytes
            await WriteMessageChunksAsync(stream, bytes);
        }
        private static object _bufferLock = new object();
        //private static async Task<byte[]> ReadBytesBaseAsync(PipeStream stream, byte[] buffer, int numberOfBytes, int msTimeout)
        private static byte[] ReadBytesBaseAsync(PipeStream stream, byte[] buffer, int numberOfBytes, int msTimeout)
        {
            //this method cannot be async, but requires an async call for the timeout to work
            byte[] bytes = null;
            var bytesRead = 0;
            try
            {
                //Array.Clear(buffer, 0, buffer.Length);
                stream.ReadAsync(buffer, 0, numberOfBytes).ContinueWith(task =>
                {
                    bytesRead = task.Result;
                //}, new CancellationTokenSource(msTimeout).Token);//.Wait(msTimeout);
            }).Wait(msTimeout);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            if (bytesRead > 0)
                bytes = ArrayMethods.PeekStart(buffer, bytesRead);
            //Array.Clear(_globalBytes, 0, bytesRead);
            return bytes;
        }
        //public static async Task<Tuple<PipeReadResultEnum, int, byte[]>> ReadBytesAsync(PipeStream stream, ref byte[] buffer, int numberOfBytes, int msTimeout)
        public static async Task<Tuple<PipeReadResultEnum, int, byte[]>> ReadBytesAsync(PipeStream stream, byte[] buffer, int numberOfBytes, int msTimeout)
        {
            Tuple<PipeReadResultEnum, int, byte[]> result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Ok, 0, null);
            byte[] bytes = null;// new byte[numberOfBytes];
                                //int bytesRead = 0;

            try
            {
                ////Task.Run(() =>
                ////{
                //stream.ReadAsync(bytes, 0, numberOfBytes, new CancellationTokenSource(msTimeout).Token).ContinueWith(task =>
                //{
                //    bytesRead = task.Result;
                //    result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Ok, task.Result, ArrayMethods.TrimEnd(ref bytes, bytes.Length - bytesRead));
                ////}).Wait();
                ////}, new CancellationTokenSource(msTimeout).Token).Wait();

                //Task.Run(() =>
                //{
                //    bytes = ReadBytesBaseAsync(stream, numberOfBytes, msTimeout);
                //}, new CancellationTokenSource(msTimeout).Token).ContinueWith(task =>
                //{
                //    if (!task.IsCanceled && !task.IsFaulted && bytes?.Length > 0)
                //        result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Ok, bytes.Length, bytes);
                //    else
                //    {
                //        //when the task cancels, we seem to get here instead of in the outer OperationCanceledException handler
                //        //System.Diagnostics.Debugger.Break(); //we shouldn't actually ever get here
                //        result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Timeout, 0, null);
                //    }
                //}).Wait();
                bytes = ReadBytesBaseAsync(stream, buffer, numberOfBytes, msTimeout);
                if (bytes?.Length > 0)
                    result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Ok, bytes.Length, bytes);
                else
                    result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Timeout, 0, null);
            }
            catch (OperationCanceledException) //task was cancelled/read timed out
            {
                result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Timeout, 0, null);
            }
            catch (ObjectDisposedException) //pipe was closed
            {
                result = new Tuple<PipeReadResultEnum, int, byte[]>(PipeReadResultEnum.Disconnected, 0, null);
            }
            return result;
        }

        public static string GetLatinText()
        {
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            return latin;
        }
        public static int GetOutBufferSize(PipeStream stream)
        {
            try
            {
                return stream?.OutBufferSize > 0 ? stream.OutBufferSize : 4096;
            }
            catch
            {
                return 256;
            }
        }
        public static int GetInBufferSize(PipeStream stream)
        {
            try
            {
                return stream?.InBufferSize > 0 ? stream.InBufferSize : 4096;
            }
            catch
            {
                return 256;
            }
        }
        public static int GetInOutBufferSize(PipeStream stream)
        {
            return Math.Min(GetOutBufferSize(stream), GetInBufferSize(stream));
        }
    }
}
