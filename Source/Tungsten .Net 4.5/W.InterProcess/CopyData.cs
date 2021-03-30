using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace W.InterProcess
{
    internal static class Extensions
    {
        /// <summary>
        /// Converts a string to an encoded byte array
        /// </summary>
        /// <param name="this">The string to convert to an encoded byte array</param>
        /// <returns>A byte array encoding of the specified string</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static byte[] AsBytes(string @this)
        {
            return System.Text.Encoding.UTF8.GetBytes(@this);
        }
        /// <summary>
        /// Uses binary serialization to serialize an object to an array of bytes
        /// </summary>
        /// <typeparam name="T">The object Type</typeparam>
        /// <param name="item">The object to serialize</param>
        /// <returns>An array of bytes containing the serialized object</returns>
        public static byte[] AsBytes<T>(T item)
        {
            var stream = new System.IO.MemoryStream();
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            formatter.Serialize(stream, item);
            return stream.GetBuffer();
        }
        /// <summary>
        /// Uses binary serialization to deserialize an array of bytes into an object
        /// </summary>
        /// <typeparam name="T">The object Type</typeparam>
        /// <param name="bytes">The bytes containing a serialized object</param>
        /// <returns>The deserialized object</returns>
        public static T FromBytes<T>(byte[] bytes)
        {
            var stream = new System.IO.MemoryStream(bytes);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            return (T)formatter.Deserialize(stream);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }

    internal partial class Win32
    {
        public const int WM_COPYDATA = 0x4A;
        public const uint WM_COPYGLOBALDATA = 0x49;
        [StructLayout(LayoutKind.Sequential)]
        public struct MessageStruct
        {
            [MarshalAs(UnmanagedType.BStr)]
            public string Message;
        }

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage 
        /// function calls the window procedure for the specified window and does 
        /// not return until the window procedure has processed the message. 
        /// </summary>
        /// <param name="hWnd">
        /// Handle to the window whose window procedure will receive the message.
        /// </param>
        /// <param name="Msg">Specifies the message to be sent.</param>
        /// <param name="wParam">
        /// Specifies additional message-specific information.
        /// </param>
        /// <param name="lParam">
        /// Specifies additional message-specific information.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll")]
        public static extern bool SetWindowText(IntPtr hWnd, string text);
    }
    //Enable WM_COPYDATA on Windows Vista or newer
    internal partial class Win32
    {
        private enum MessageFilterInfo : uint
        {
            None,
            AlreadyAllowed,
            AlreadyDisAllowed,
            AllowedHigher
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CHANGEFILTERSTRUCT
        {
            public uint cbSize;
            public MessageFilterInfo ExtStatus;
        }

        /// <summary>
        /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/dd388202(v=vs.85).aspx"/>
        /// </summary>
        private enum ChangeWindowMessageFilterExAction : uint
        {
            Reset = 0,
            Allow = 1,
            Disallow = 2
        }

        private enum ChangeWindowMessageFilterFlags : uint
        {
            Add = 1,
            Remove = 2
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, IntPtr ptr);// ref CHANGEFILTERSTRUCT changeInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilter(uint msg, ChangeWindowMessageFilterFlags flags);

        private static readonly bool IsVistaOrHigher = System.Environment.OSVersion.Version.Major >= 6;
        private static readonly bool Is7OrHigher = (System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor >= 1) || System.Environment.OSVersion.Version.Major > 6;

        public static System.ComponentModel.Win32Exception EnableWM_COPYDATA(IntPtr hWnd)
        {
            bool result = false;//, result2 = false;
            if (Is7OrHigher)
            {
                var changeStruct = new CHANGEFILTERSTRUCT();
                changeStruct.cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT)));
                //result = ChangeWindowMessageFilterEx(hWnd, WM_COPYDATA, ChangeWindowMessageFilterExAction.Reset, ref changeStruct);
                result = ChangeWindowMessageFilterEx(hWnd, WM_COPYDATA, ChangeWindowMessageFilterExAction.Allow, IntPtr.Zero);// ref changeStruct);
                //result2 = ChangeWindowMessageFilterEx(hWnd, WM_COPYGLOBALDATA, ChangeWindowMessageFilterExAction.Allow, ref changeStruct);
            }
            else if (IsVistaOrHigher)
            {
                result = ChangeWindowMessageFilter(WM_COPYDATA, ChangeWindowMessageFilterFlags.Add);
                //result2 = ChangeWindowMessageFilter(WM_COPYGLOBALDATA, ChangeWindowMessageFilterFlags.Add);
            }
            return result ? null : new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
        }
    }
    /// <summary>
    /// Sends and receives data via WM_COPYDATA
    /// </summary>
    public class CopyData : NativeWindow
    {
        /// <summary>
        /// Helper class which converts byte arrays and COPYDATASTRUCTs
        /// </summary>
        protected class CopyDataStruct : IDisposable
        {
            private COPYDATASTRUCT _copyDataStruct;

            /// <summary>
            /// Get the COPYDATASTRUCT representing the byte array
            /// </summary>
            /// <returns>The COPYDATASTRUCT representing the byte array</returns>
            public COPYDATASTRUCT GetCopyDataStruct() => _copyDataStruct;

            /// <summary>
            /// Converts the contents of the COPYDATASTRUCT in the lParam of the specified message m to a byte array
            /// </summary>
            /// <param name="m">The windows message</param>
            /// <returns>A contents of the COPYDATASTRUCT</returns>
            public static byte[] AsBytes(Message m)
            {
                return AsBytes((COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT)));
            }
            /// <summary>
            /// Converts the contents of the COPYDATASTRUCT to a byte array
            /// </summary>
            /// <param name="cds">The COPYDATASTRUCT containing a byte array</param>
            /// <returns>A contents of the COPYDATASTRUCT</returns>
            public static byte[] AsBytes(COPYDATASTRUCT cds)
            {
                if (cds.cbData > 0)
                {
                    var itemBytes = new byte[cds.cbData];
                    Marshal.Copy(cds.lpData, itemBytes, 0, cds.cbData);
                    return itemBytes;
                }
                return null;
            }
            /// <summary>
            /// Disposes the CopyDataStruct and releases resource
            /// </summary>
            public void Dispose()
            {
                if (_copyDataStruct.lpData != IntPtr.Zero)
                    Marshal.FreeHGlobal(_copyDataStruct.lpData);
            }
            /// <summary>
            /// Constructs a new CopyDataStruct from the specified byte array
            /// </summary>
            /// <param name="itemBytes">The byte array used to create the COPYDATASTRUCT</param>
            public CopyDataStruct(byte[] itemBytes)
            {
                _copyDataStruct = new COPYDATASTRUCT()
                {
                    cbData = itemBytes.Length,
                    dwData = IntPtr.Zero,
                    lpData = Marshal.AllocCoTaskMem(itemBytes.Length)
                };
                Marshal.Copy(itemBytes, 0, GetCopyDataStruct().lpData, itemBytes.Length);
            }
        }
        private Predicate<string> _filter;
        private bool _findAll;

        /// <summary>
        /// The windows which should receive messages
        /// </summary>
        protected IntPtr[] TargetWindows { get; private set; }

        /// <summary>
        /// Raised when a message has been received
        /// </summary>
        public event Action<byte[]> BytesReceived;
        /// <summary>
        /// Raised when an error occurs while sending or receiving messages
        /// </summary>
        public event Action<System.ComponentModel.Win32Exception> Error;

        /// <summary>
        /// Raises the BytesReceived event
        /// </summary>
        /// <param name="e">The exception if one occurred</param>
        /// <param name="bytes">The bytes received</param>
        protected void RaiseBytesReceived(byte[] bytes)
        {
            BytesReceived?.Invoke(bytes);
        }
        /// <summary>
        /// Raises the Error event
        /// </summary>
        /// <param name="e">The exception</param>
        protected void RaiseError(System.ComponentModel.Win32Exception e)
        {
            Error?.Invoke(e);
        }
        /// <summary>
        /// Called when the window receives a WM_COPYDATA message
        /// </summary>
        /// <param name="cds">The COPYDATASTRUCT associated with the WM_COPYDATA windows message</param>
        protected virtual void OnWM_COPYDATA(COPYDATASTRUCT cds)
        {
            var bytes = CopyDataStruct.AsBytes(cds);
            RaiseBytesReceived(bytes);
        }
        /// <summary>
        /// The window procedure
        /// </summary>
        /// <param name="m">The message received</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32.WM_COPYDATA)
            {
                var cds = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                OnWM_COPYDATA(cds);
            }
            base.WndProc(ref m);
        }
        /// <summary>
        /// Sends a message via WM_COPYDATA
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Send(byte[] message)
        {
            if (TargetWindows == null)
                return;
            foreach (var hTargetWnd in TargetWindows)
                Send(Handle, message, hTargetWnd, e => RaiseError(e));
        }
        /// <summary>
        /// Refreshes the list of target windows
        /// </summary>
        public void RefreshTargets()
        {
            TargetWindows = Windows.FindWindow.Find(_filter, _findAll).ToArray();
        }

        /// <summary>
        /// Constructs a new CopyData instance which can only listen for messages
        /// </summary>
        /// <param name="windowText">The Window Text for the underlying NativeWindow</param>
        public CopyData(string windowText) : this(IntPtr.Zero, null, false)
        {
            Win32.SetWindowText(Handle, windowText);
        }
        /// <summary>
        /// Constructs a new CopyData instance
        /// </summary>
        /// <param name="filter">The predicate used to filter target windows by Window Text</param>
        /// <param name="findAll">If True, multiple windows can be targeted, otherwise only the first window found will be targeted</param>
        public CopyData(Predicate<string> filter, bool findAll) : this(IntPtr.Zero, filter, findAll)
        {
        }
        /// <summary>
        /// Constructs a new CopyData instance
        /// </summary>
        /// <param name="parent">The Form which will send and receive messages</param>
        /// <param name="filter">The predicate used to filter target windows by Window Text</param>
        /// <param name="findAll">If True, multiple windows can be targeted, otherwise only the first window found will be targeted</param>
        public CopyData(Form parent, Predicate<string> filter, bool findAll) : this(parent.Handle, filter, findAll)
        {
            parent.HandleCreated += (s, e) => { AssignHandle(((Form)s).Handle); Win32.EnableWM_COPYDATA(((Form)s).Handle); };
            parent.HandleDestroyed += (s, e) => { ReleaseHandle(); };
        }
        /// <summary>
        /// Constructs a new CopyData instance
        /// </summary>
        /// <param name="hSourceWnd">The window handle which will send and receive messages</param>
        /// <param name="filter">The predicate used to filter target windows by Window Text</param>
        /// <param name="findAll">If True, multiple windows can be targeted, otherwise only the first window found will be targeted</param>
        public CopyData(IntPtr hSourceWnd, Predicate<string> filter, bool findAll)
        {
            _filter = filter;
            _findAll = findAll;
            if (hSourceWnd != IntPtr.Zero && !Handle.Equals(hSourceWnd))
                AssignHandle(hSourceWnd);
            Enable(Handle);
            if (_filter != null)
                RefreshTargets();
        }
        
        /// <summary>
        /// Enables a window to receive WM_COPYDATA messages
        /// </summary>
        /// <param name="hWnd">The handle of the window to enable</param>
        /// <returns>Null upon success, otherwise a Win32Exception containing exception information</returns>
        public static System.ComponentModel.Win32Exception Enable(IntPtr hWnd)
        {
            return Win32.EnableWM_COPYDATA(hWnd);
        }
        /// <summary>
        /// Sends a byte array from the source window to windows matching the filter predicate
        /// </summary>
        /// <param name="hSourceWnd">The window which is sending the message</param>
        /// <param name="message">The array of bytes to send</param>
        /// <param name="filter">Used to target one or more windows based on Window Text</param>
        /// <param name="onError">Called if an error occurs</param>
        public static void Send(IntPtr hSourceWnd, byte[] message, Predicate<string> filter, Action<System.ComponentModel.Win32Exception> onError = null)
        {
            Enable(hSourceWnd);
            //find receivers
            foreach (var hTargetWnd in Windows.FindWindow.Find(filter, false))
                Send(hSourceWnd, message, hTargetWnd, onError);
        }
        /// <summary>
        /// Sends a byte array from the source window to windows matching the filter predicate
        /// </summary>
        /// <param name="hSourceWnd">The window which is sending the message</param>
        /// <param name="message">The array of bytes to send</param>
        /// <param name="hTargetWnd">The receiving window</param>
        /// <param name="onError">Called if an error occurs</param>
        public static void Send(IntPtr hSourceWnd, byte[] message, IntPtr hTargetWnd, Action<System.ComponentModel.Win32Exception> onError = null)
        {
            Enable(hSourceWnd);
            var result = Send(hSourceWnd, message, hTargetWnd);
            if (result != null)
                onError?.Invoke(result);
        }
        /// <summary>
        /// Sends a byte array from the source window to windows matching the filter predicate
        /// </summary>
        /// <param name="hSourceWnd">The window which is sending the message</param>
        /// <param name="message">The array of bytes to send</param>
        /// <param name="hTargetWnd">The receiving window</param>
        /// <returns>An exception if one occurs while sending the message</returns>
        public static System.ComponentModel.Win32Exception Send(IntPtr hSourceWnd, byte[] message, IntPtr hTargetWnd)
        {
            System.ComponentModel.Win32Exception result = null;
            if (hTargetWnd == IntPtr.Zero)
                return result;

            Enable(hSourceWnd);
            using (var wrapper = new CopyDataStruct(message))
            {
                var cds = wrapper.GetCopyDataStruct();
                Win32.SendMessage(hTargetWnd, Win32.WM_COPYDATA, hSourceWnd, ref cds);
            }
            return result;
        }
    }
    /// <summary>
    /// Sends and receives Generics via WM_COPYDATA
    /// </summary>
    public sealed class CopyData<TMessage> : CopyData
    {
        /// <summary>
        /// Raised when a message has been received
        /// </summary>
        public event Action<TMessage> MessageReceived;

        /// <summary>
        /// Called when the window receives a WM_COPYDATA message
        /// </summary>
        /// <param name="cds">The COPYDATASTRUCT associated with the WM_COPYDATA windows message</param>
        protected override void OnWM_COPYDATA(COPYDATASTRUCT cds)
        {
            base.OnWM_COPYDATA(cds); //allow BytesReceived to be raised (unnecessarily, but it also shouldn't hurt)
            var item = Extensions.FromBytes<TMessage>(CopyDataStruct.AsBytes(cds));
            MessageReceived?.Invoke(item);
        }

        /// <summary>
        /// Sends a message via WM_COPYDATA
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Send(TMessage message)
        {
            base.Send(Extensions.AsBytes(message));
        }
        /// <summary>
        /// Constructs a new CopyData instance which can only listen for messages
        /// </summary>
        /// <param name="windowText">The Window Text for the underlying NativeWindow</param>
        public CopyData(string windowText) : base(windowText) { }
        /// <summary>
        /// Constructs a new CopyData instance
        /// </summary>
        /// <param name="filter">The predicate used to filter target windows by Window Text</param>
        /// <param name="findAll">If True, multiple windows can be targeted, otherwise only the first window found will be targeted</param>
        public CopyData(Predicate<string> filter, bool findAll) : base(filter, findAll) { }
        /// <summary>
        /// Constructs a new CopyData instance
        /// </summary>
        /// <param name="parent">The form which will send and receive messages</param>
        /// <param name="filter">The predicate used to filter target windows by Window Text</param>
        /// <param name="findAll">If True, multiple windows can be targeted, otherwise only the first window found will be targeted</param>
        public CopyData(Form parent, Predicate<string> filter, bool findAll) : base(parent, filter, findAll) { }
        /// <summary>
        /// Constructs a new CopyData instance
        /// </summary>
        /// <param name="hSourceWnd">The window handle which will send and receive messages</param>
        /// <param name="filter">The predicate used to filter target windows by Window Text</param>
        /// <param name="findAll">If True, multiple windows can be targeted, otherwise only the first window found will be targeted</param>
        public CopyData(IntPtr hSourceWnd, Predicate<string> filter, bool findAll) : base(hSourceWnd, filter, findAll) { }
    }

    /// <summary>
    /// Logs messages to the specified window via WM_COPYDATA messages
    /// </summary>
    /// <example>
    ///     Log.LogTheMessage += (category, message) => W.InterProcess.CopyDataLogger.LogTheMessage("ConsoleLogger", true, category, message);
    /// </example>
    public static class CopyDataLogger
    {
        //private static bool _exception = false;
        private static CopyData _cd = null;
        private static CopyData GetCopyDataInstance(string windowCaption)
        {
            try
            {
                if (_cd == null)// && !_exception)
                    _cd = new CopyData(caption => caption == windowCaption, true);
            }
            catch 
            {
                //_exception = true;
            }
            finally { }
            return _cd;
        }

        /// <summary>
        /// Log a message to the specified window via WM_COPYDATA messaging
        /// </summary>
        /// <param name="windowCaption"></param>
        /// <param name="addTimestamp"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        public static void LogTheMessage(string windowCaption, string message)
        {
            var cd = GetCopyDataInstance(windowCaption);
            if (cd != null)
            {
                cd?.Send(Extensions.AsBytes(message));
            }
        }
    }
}
