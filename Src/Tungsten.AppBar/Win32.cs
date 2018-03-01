using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace W
{
    public partial class AppBar
    {
        public enum AppBarEdge : int
        {
            Left = 0,
            Top,
            Right,
            Bottom,
            None
        }
        internal sealed class Win32
        {
            //1.4.2014 - taken from http://www.codeproject.com/Articles/6741/AppBar-using-C
            #region Win32 for AppBar
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
                public int Width { get { return right - left; } }
                public int Height { get { return bottom - top; } }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct APPBARDATA
            {
                public int cbSize;
                public IntPtr hWnd;
                public uint uCallbackMessage;
                public int uEdge;
                public RECT rc;
                public IntPtr lParam;

                public static APPBARDATA Create(System.Windows.Forms.Form form = null)
                {
                    var result = new APPBARDATA();
                    result.cbSize = Marshal.SizeOf(result);
                    if (form != null)
                        result.hWnd = form.Handle;
                    return result;
                }
                public static APPBARDATA Create(IntPtr hwnd)
                {
                    var result = new APPBARDATA();
                    result.cbSize = Marshal.SizeOf(result);
                    result.hWnd = hwnd;
                    return result;
                }
            }

            public enum AppBarMessage : int
            {
                ABM_NEW = 0,
                ABM_REMOVE = 1,
                ABM_QUERYPOS = 2,
                ABM_SETPOS = 3,
                ABM_GETSTATE = 4,
                ABM_GETTASKBARPOS = 5,
                ABM_ACTIVATE = 6,
                ABM_GETAUTOHIDEBAR = 7,
                ABM_SETAUTOHIDEBAR = 8,
                ABM_WINDOWPOSCHANGED = 9,
                ABM_SETSTATE = 10
            }

            public enum AppBarNotify : int
            {
                ABN_STATECHANGE = 0,
                ABN_POSCHANGED,
                ABN_FULLSCREENAPP,
                ABN_WINDOWARRANGE
            }

            #region SetWindowPosFlags
            [Flags()]
            public enum SetWindowPosFlags : uint
            {
                /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
                /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
                /// blocking its execution while Other threads process the request.</summary>
                /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
                AsynchronousWindowPosition = 0x4000,
                /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
                /// <remarks>SWP_DEFERERASE</remarks>
                DeferErase = 0x2000,
                /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
                /// <remarks>SWP_DRAWFRAME</remarks>
                DrawFrame = 0x0020,
                /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
                /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
                /// is sent only when the window's size is being changed.</summary>
                /// <remarks>SWP_FRAMECHANGED</remarks>
                FrameChanged = 0x0020,
                /// <summary>Hides the window.</summary>
                /// <remarks>SWP_HIDEWINDOW</remarks>
                HideWindow = 0x0080,
                /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
                /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
                /// parameter).</summary>
                /// <remarks>SWP_NOACTIVATE</remarks>
                DoNotActivate = 0x0010,
                /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
                /// contents of the client area are saved and copied back into the client area after the window is sized or 
                /// repositioned.</summary>
                /// <remarks>SWP_NOCOPYBITS</remarks>
                DoNotCopyBits = 0x0100,
                /// <summary>Retains the current position (ignores X and Y parameters).</summary>
                /// <remarks>SWP_NOMOVE</remarks>
                IgnoreMove = 0x0002,
                /// <summary>Does not change the owner window's position in the Z order.</summary>
                /// <remarks>SWP_NOOWNERZORDER</remarks>
                DoNotChangeOwnerZOrder = 0x0200,
                /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
                /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
                /// window uncovered as a result of the window being moved. When this flag is set, the application must 
                /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
                /// <remarks>SWP_NOREDRAW</remarks>
                DoNotRedraw = 0x0008,
                /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
                /// <remarks>SWP_NOREPOSITION</remarks>
                DoNotReposition = 0x0200,
                /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
                /// <remarks>SWP_NOSENDCHANGING</remarks>
                DoNotSendChangingEvent = 0x0400,
                /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
                /// <remarks>SWP_NOSIZE</remarks>
                IgnoreResize = 0x0001,
                /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
                /// <remarks>SWP_NOZORDER</remarks>
                IgnoreZOrder = 0x0004,
                /// <summary>Displays the window.</summary>
                /// <remarks>SWP_SHOWWINDOW</remarks>
                ShowWindow = 0x0040,
            }
            #endregion
            #region ShowWindowCommands
            public enum ShowWindowCommands
            {
                /// <summary>
                /// Hides the window and activates another window.
                /// </summary>
                Hide = 0,
                /// <summary>
                /// Activates and displays a window. If the window is minimized or 
                /// maximized, the system restores it to its original size and position.
                /// An application should specify this flag when displaying the window 
                /// for the first time.
                /// </summary>
                Normal = 1,
                /// <summary>
                /// Activates the window and displays it as a minimized window.
                /// </summary>
                ShowMinimized = 2,
                /// <summary>
                /// Maximizes the specified window.
                /// </summary>
                Maximize = 3, // is this the right value?
                /// <summary>
                /// Activates the window and displays it as a maximized window.
                /// </summary>       
                ShowMaximized = 3,
                /// <summary>
                /// Displays a window in its most recent size and position. This value 
                /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
                /// the window is not activated.
                /// </summary>
                ShowNoActivate = 4,
                /// <summary>
                /// Activates the window and displays it in its current size and position. 
                /// </summary>
                Show = 5,
                /// <summary>
                /// Minimizes the specified window and activates the next top-level 
                /// window in the Z order.
                /// </summary>
                Minimize = 6,
                /// <summary>
                /// Displays the window as a minimized window. This value is similar to
                /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
                /// window is not activated.
                /// </summary>
                ShowMinNoActive = 7,
                /// <summary>
                /// Displays the window in its current size and position. This value is 
                /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
                /// window is not activated.
                /// </summary>
                ShowNA = 8,
                /// <summary>
                /// Activates and displays the window. If the window is minimized or 
                /// maximized, the system restores it to its original size and position. 
                /// An application should specify this flag when restoring a minimized window.
                /// </summary>
                Restore = 9,
                /// <summary>
                /// Sets the show state based on the SW_* value specified in the 
                /// STARTUPINFO structure passed to the CreateProcess function by the 
                /// program that started the application.
                /// </summary>
                ShowDefault = 10,
                /// <summary>
                ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
                /// that owns the window is not responding. This flag should only be 
                /// used when minimizing windows from a different thread.
                /// </summary>
                ForceMinimize = 11
            }
            #endregion

            public static readonly uint WM_ACTIVATE = 0x0006;
            public static readonly uint WM_WINDOWPOSCHANGED = 0x0047;
            public static readonly uint ABS_ALWAYSONTOP = 0x0000002;
            public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            public static readonly IntPtr HWND_TOP = new IntPtr(0);
            public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
            [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
            public static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
            [DllImport("USER32")]
            public static extern int GetSystemMetrics(int Index);
            [DllImport("User32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern int RegisterWindowMessage(string msg);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
            #endregion

            //1.5.2014 - taken from http://msdn.microsoft.com/en-us/library/ms229683(v=vs.90).aspx
            #region Win32 for WndProcHooker
            // Contains managed wrappers and implementations of Win32 
            // structures, delegates, constants and platform invokes 
            // used by the Subclassing samples. 

            // A callback to a Win32 window procedure (wndproc): 
            // Parameters: 
            //   hwnd - The handle of the window receiving a message. 
            //   msg - The message 
            //   wParam - The message's parameters (part 1). 
            //   lParam - The message's parameters (part 2). 
            //  Returns an integer as described for the given message in MSDN. 
            public delegate int WndProc(IntPtr hwnd, uint msg, uint wParam, int lParam);
            [DllImport("user32.dll", SetLastError = true)]
            public extern static IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewLong);
            public const int GWL_WNDPROC = -4;

            [DllImport("user32.dll", SetLastError = true)]
            public extern static int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, uint wParam, int lParam);

            [DllImport("user32.dll", SetLastError = true)]
            public extern static int DefWindowProc(IntPtr hwnd, uint msg, uint wParam, int lParam);
            #endregion

            //1.5.2014 - taken from http://msdn.microsoft.com/en-us/library/ms229683(v=vs.90).aspx
            #region Win32 for Gradient
            // Contains managed wrappers and implementations of Win32 
            // structures, delegates, constants and platform invokes 
            // used by the GradientFill samples. 

            //public struct TRIVERTEX
            //{
            //    public int x;
            //    public int y;
            //    public ushort Red;
            //    public ushort Green;
            //    public ushort Blue;
            //    public ushort Alpha;
            //    public TRIVERTEX(int x, int y, Color color) : this(x, y, color.R, color.G, color.B, color.A)
            //    {
            //    }
            //    public TRIVERTEX(
            //        int x, int y,
            //        ushort red, ushort green, ushort blue,
            //        ushort alpha)
            //    {
            //        this.x = x;
            //        this.y = y;
            //        this.Red = (ushort)(red << 8);
            //        this.Green = (ushort)(green << 8);
            //        this.Blue = (ushort)(blue << 8);
            //        this.Alpha = (ushort)(alpha << 8);
            //    }
            //}
            //public struct GRADIENT_RECT
            //{
            //    public uint UpperLeft;
            //    public uint LowerRight;
            //    public GRADIENT_RECT(uint ul, uint lr)
            //    {
            //        this.UpperLeft = ul;
            //        this.LowerRight = lr;
            //    }
            //}
            //public struct GRADIENT_TRIANGLE
            //{
            //    public uint Vertex1;
            //    public uint Vertex2;
            //    public uint Vertex3;
            //    public GRADIENT_TRIANGLE(uint v1, uint v2, uint v3)
            //    {
            //        this.Vertex1 = v1;
            //        this.Vertex2 = v2;
            //        this.Vertex3 = v3;
            //    }
            //}

            //// WM_NOTIFY notification message header.
            //[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
            //public class NMHDR
            //{
            //    private IntPtr hwndFrom;
            //    public uint idFrom;
            //    public uint code;
            //}

            ////[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)] 
            //public struct TVITEM
            //{
            //    public int mask;
            //    private IntPtr hItem;
            //    public int state;
            //    public int stateMask;
            //    private IntPtr pszText;
            //    public int cchTextMax;
            //    public int iImage;
            //    public int iSelectedImage;
            //    public int cChildren;
            //    private IntPtr lParam;
            //}

            //// Native representation of a point. 
            //public struct POINT
            //{
            //    public int X;
            //    public int Y;
            //}

            //public struct TVHITTESTINFO
            //{
            //    public POINT pt;
            //    public uint flags;
            //    private IntPtr hItem;
            //}

            //[DllImport("coredll.dll", SetLastError = true, EntryPoint = "GradientFill")]
            //public extern static bool GradientFill(
            //    IntPtr hdc,
            //    TRIVERTEX[] pVertex,
            //    uint dwNumVertex,
            //    GRADIENT_RECT[] pMesh,
            //    uint dwNumMesh,
            //    uint dwMode);

            //public const int GRADIENT_FILL_RECT_H = 0x00000000;
            //public const int GRADIENT_FILL_RECT_V = 0x00000001;

            //// Not supported on Windows CE: 
            //public const int GRADIENT_FILL_TRIANGLE = 0x00000002;

            //[DllImport("coredll.dll")]
            //public extern static int SendMessage(IntPtr hwnd, uint msg, uint wParam, ref TVHITTESTINFO lParam);

            //[DllImport("coredll.dll", SetLastError = true)]
            //public extern static int SendMessage(IntPtr hwnd, uint msg, uint wParam, ref TVITEM lParam);

            //[DllImport("coredll.dll")]
            //public extern static uint GetMessagePos();

            //[DllImport("coredll.dll")]
            //public extern static IntPtr BeginPaint(IntPtr hwnd, ref PAINTSTRUCT ps);

            //[DllImport("coredll.dll")]
            //public extern static bool EndPaint(IntPtr hwnd, ref PAINTSTRUCT ps);

            //public struct PAINTSTRUCT
            //{
            //    private IntPtr hdc;
            //    public bool fErase;
            //    public Rectangle rcPaint;
            //    public bool fRestore;
            //    public bool fIncUpdate;
            //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //    public byte[] rgbReserved;
            //}

            //[DllImport("coredll.dll")]
            //public extern static IntPtr GetDC(IntPtr hwnd);

            //[DllImport("coredll.dll")]
            //public extern static bool ReleaseDC(IntPtr hwnd, IntPtr hdc);

            // Helper function to convert a Windows lParam into a Point. 
            //   lParam - The parameter to convert. 
            // Returns a Point where X is the low 16 bits and Y is the 
            // high 16 bits of the value passed in. 
            //public static Point LParamToPoint(int lParam)
            //{
            //    uint ulParam = (uint)lParam;
            //    return new Point(
            //        (int)(ulParam & 0x0000ffff),
            //        (int)((ulParam & 0xffff0000) >> 16));
            //}

            //// Windows messages 
            //public const uint WM_PAINT = 0x000F;
            //public const uint WM_ERASEBKGND = 0x0014;
            //public const uint WM_KEYDOWN = 0x0100;
            //public const uint WM_KEYUP = 0x0101;
            //public const uint WM_MOUSEMOVE = 0x0200;
            //public const uint WM_LBUTTONDOWN = 0x0201;
            //public const uint WM_LBUTTONUP = 0x0202;
            //public const uint WM_NOTIFY = 0x4E;

            //// Notifications 
            //public const uint NM_CLICK = 0xFFFFFFFE;
            //public const uint NM_DBLCLK = 0xFFFFFFFD;
            //public const uint NM_RCLICK = 0xFFFFFFFB;
            //public const uint NM_RDBLCLK = 0xFFFFFFFA;

            //// Key 
            //public const uint VK_SPACE = 0x20;
            //public const uint VK_RETURN = 0x0D;

            //// Treeview 
            //public const uint TV_FIRST = 0x1100;
            //public const uint TVM_HITTEST = TV_FIRST + 17;

            //public const uint TVHT_NOWHERE = 0x0001;
            //public const uint TVHT_ONITEMICON = 0x0002;
            //public const uint TVHT_ONITEMLABEL = 0x0004;
            //public const uint TVHT_ONITEM = (TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON);
            //public const uint TVHT_ONITEMINDENT = 0x0008;
            //public const uint TVHT_ONITEMBUTTON = 0x0010;
            //public const uint TVHT_ONITEMRIGHT = 0x0020;
            //public const uint TVHT_ONITEMSTATEICON = 0x0040;
            //public const uint TVHT_ABOVE = 0x0100;
            //public const uint TVHT_BELOW = 0x0200;
            //public const uint TVHT_TORIGHT = 0x0400;
            //public const uint TVHT_TOLEFT = 0x0800;

            //public const uint TVM_GETITEM = TV_FIRST + 62;  //TVM_GETITEMW 

            //public const uint TVIF_TEXT = 0x0001;
            //public const uint TVIF_IMAGE = 0x0002;
            //public const uint TVIF_PARAM = 0x0004;
            //public const uint TVIF_STATE = 0x0008;
            //public const uint TVIF_HANDLE = 0x0010;
            //public const uint TVIF_SELECTEDIMAGE = 0x0020;
            //public const uint TVIF_CHILDREN = 0x0040;
            //public const uint TVIF_DI_SETITEM = 0x1000;
            #endregion
        }
    }
}
