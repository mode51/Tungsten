using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace W
{
    //1.5.2014 - Adapted from several sites, including http://msdn.microsoft.com/en-us/library/bb776821.aspx
    public partial class AppBar
    {
        private static class Services
        {
            public static uint New(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_NEW, ref abd);
            }
            public static uint Remove(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_REMOVE, ref abd);
            }
            public static uint Activate(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_ACTIVATE, ref abd);
            }
            public static uint GetAutoHideBar(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_GETAUTOHIDEBAR, ref abd);
            }
            public static uint SetAutoHideBar(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_SETAUTOHIDEBAR, ref abd);
            }
            public static uint GetState(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_GETSTATE, ref abd);
            }
            public static uint SetState(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_SETSTATE, ref abd);
            }
            public static uint GetTaskbarPosition(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_GETTASKBARPOS, ref abd);
            }
            public static uint QueryPosition(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_QUERYPOS, ref abd);
            }
            public static uint SetPosition(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_SETPOS, ref abd);
            }
            public static uint WindowPositionChanged(ref Win32.APPBARDATA abd)
            {
                return Win32.SHAppBarMessage((int)Win32.AppBarMessage.ABM_WINDOWPOSCHANGED, ref abd);
            }
        }
        private class RegisterInfo
        {
            public uint CallbackId { get; set; }
            public bool IsRegistered { get; set; }
            public Form Form { get; set; }
            public AppBarEdge Edge { get; set; }
            public FormBorderStyle OriginalFormBorderStyle { get; set; }
            public Point OriginalLocation { get; set; }
            public Size OriginalSize { get; set; }
            public bool OriginalTopMost { get; set; }
            public bool IsDragging { get; set; }
            public Point LastDragPoint { get; set; }

            public int WndProc(IntPtr hwnd, uint msg, uint wParam, int lParam, ref bool handled)
            {
                var abd = Win32.APPBARDATA.Create(hwnd);
                if (msg == Win32.WM_ACTIVATE)
                {
                    Services.Activate(ref abd);
                }
                else if (msg == Win32.WM_WINDOWPOSCHANGED)
                {
                    Services.WindowPositionChanged(ref abd);
                }
                else if (msg == CallbackId)
                {
                    var state = Services.GetState(ref abd);
                    switch (wParam)
                    {
                        case (uint)Win32.AppBarNotify.ABN_POSCHANGED:
                            SetPosition(Form, Edge);
                            handled = true;
                            break;
                        case (uint)Win32.AppBarNotify.ABN_FULLSCREENAPP:
                            if (lParam != 0)
                            {
                                Win32.SetWindowPos(abd.hWnd, (Win32.ABS_ALWAYSONTOP & state) == Win32.ABS_ALWAYSONTOP ? Win32.HWND_TOPMOST : Win32.HWND_BOTTOM, 0, 0, 0, 0, Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreResize);
                            }
                            else
                            {
                                if ((state & Win32.ABS_ALWAYSONTOP) == Win32.ABS_ALWAYSONTOP)
                                    Win32.SetWindowPos(abd.hWnd, Win32.HWND_TOPMOST, 0, 0, 0, 0, Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreResize);
                            }
                            break;
                        case (uint)Win32.AppBarNotify.ABN_STATECHANGE:
                            Win32.SetWindowPos(abd.hWnd, (Win32.ABS_ALWAYSONTOP & state) == Win32.ABS_ALWAYSONTOP ? Win32.HWND_TOPMOST : Win32.HWND_BOTTOM, 0, 0, 0, 0, Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreResize);
                            break;
                        case (uint)Win32.AppBarNotify.ABN_WINDOWARRANGE:
                            Win32.ShowWindow(abd.hWnd, (lParam != 0) ? Win32.ShowWindowCommands.Hide : Win32.ShowWindowCommands.ShowNoActivate);
                            break;
                    }
                }
                //}
                return 0;
            }
        }
        private static Dictionary<Form, RegisterInfo> _registeredWindowInfo = new Dictionary<Form, RegisterInfo>();
        private static RegisterInfo GetRegisterInfo(Form form)
        {
            RegisterInfo reg;
            if (_registeredWindowInfo.ContainsKey(form))
            {
                reg = _registeredWindowInfo[form];
            }
            else
            {
                reg = new RegisterInfo()
                {
                    CallbackId = 0,
                    Form = form,
                    IsRegistered = false,
                    Edge = AppBarEdge.Top,
                    OriginalFormBorderStyle = form.FormBorderStyle,
                    OriginalLocation = new Point(form.Left, form.Top),
                    OriginalSize = new Size(form.Width, form.Height),
                    OriginalTopMost = form.TopMost
                };
                _registeredWindowInfo.Add(form, reg);
            }
            return reg;
        }

        private static void form_LocationChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LocationChanged");
            var form = (Form)sender;
            var scn = Screen.FromPoint(form.Location);
            if (DoSnap(form.Left, scn.WorkingArea.Left)) form.Left = scn.WorkingArea.Left;
            if (DoSnap(form.Top, scn.WorkingArea.Top)) form.Top = scn.WorkingArea.Top;
            if (DoSnap(scn.WorkingArea.Right, form.Right)) form.Left = scn.WorkingArea.Right - form.Width;
            if (DoSnap(scn.WorkingArea.Bottom, form.Bottom)) form.Top = scn.WorkingArea.Bottom - form.Height;
        }
        private static void form_Move(object sender, EventArgs e)
        {
            var form = (Form)sender;
            var info = GetRegisterInfo(form);
            System.Diagnostics.Debug.WriteLine("Move");
        }
        private static void form_ResizeBegin(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ResizeBegin");
            var form = (Form)sender;
            var info = GetRegisterInfo(form);
            form.Width = info.OriginalSize.Width;
            form.Height = info.OriginalSize.Height;
            Application.DoEvents();
        }
        private static void form_ResizeEnd(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ResizeEnd");
            var form = (Form)sender;
            var info = GetRegisterInfo(form);

            form.Hide();
            SetPosition(form, info.Edge);
            //I don't know why I have to call SetPosition twice, but if I don't, the window "stacks" every Other time it's resized
            //let Windows complete it's what-not
            Application.DoEvents();
            SetPosition(form, info.Edge);
            form.Show();
        }
        private static void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            var form = (Form)sender;
            form.Hide();
            Unregister(form);
        }
        private static readonly uint SnapDistance = 25;
        private static bool DoSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= SnapDistance;
        }
        #region Drag by Mouse
        //static void form_MouseMove(object sender, MouseEventArgs e)
        //{
        //    var form = (Form)sender;
        //    Drag(form, e);
        //}
        //static void form_MouseUp(object sender, MouseEventArgs e)
        //{
        //    var form = (Form)sender;
        //    StopDrag(form, e);
        //}
        //static void form_MouseDown(object sender, MouseEventArgs e)
        //{
        //    var form = (Form)sender;
        //    StartDrag(form, e);
        //}
        //private static void StartDrag(Form form, MouseEventArgs e)
        //{
        //    var info = GetRegisterInfo(form);
        //    info.IsDragging = true;
        //    info.LastDragPoint = e.Location;
        //}
        //private static void StopDrag(Form form, MouseEventArgs e)
        //{
        //    var info = GetRegisterInfo(form);
        //    info.IsDragging = false;
        //}
        //private static void Drag(Form form, MouseEventArgs e)
        //{
        //    var info = GetRegisterInfo(form);
        //    if (info.IsDragging)
        //    {
        //        form.Location.Offset(new Point(e.X - info.LastDragPoint.X, e.Y - info.LastDragPoint.Y));
        //        info.LastDragPoint = e.Location;
        //    }
        //}
        #endregion

        private static void Register(Form form, AppBarEdge edge = AppBarEdge.Top)
        {
            var info = GetRegisterInfo(form);
            var abd = Win32.APPBARDATA.Create(form);
            abd.uCallbackMessage = RegisterMessage(form);

            var result = Services.New(ref abd);
            info.IsRegistered = true;
            SetPosition(form, edge);
            WndProcHooker.HookWndProc(form, info.WndProc, info.CallbackId);
        }
        private static void Unregister(Form form, bool forResizeOnly = false)
        {
            var info = GetRegisterInfo(form);
            if (info.IsRegistered)
            {
                var abd = Win32.APPBARDATA.Create(form);
                Services.Remove(ref abd);

                info.IsRegistered = false;
                if (!forResizeOnly)
                {
                    form.TopMost = info.OriginalTopMost;
                    form.FormBorderStyle = info.OriginalFormBorderStyle;
                    form.Size = info.OriginalSize;
                    form.Location = info.OriginalLocation;
                    WndProcHooker.UnhookWndProc(form, info.CallbackId);
                }
            }
        }

        private static uint RegisterMessage(Form form)
        {
            var info = GetRegisterInfo(form);
            //if (info.CallbackId != 0)
            info.CallbackId = (uint)Win32.RegisterWindowMessage("AppBarMessage");
            return info.CallbackId;
        }
        private static void SetPosition(Form form, AppBarEdge edge, bool move = true)
        {
            var abd = Win32.APPBARDATA.Create(form);
            abd.uEdge = (int)edge;
            int iHeight = 0;
            int iWidth = 0;
            if (abd.uEdge == (int)AppBarEdge.Left || abd.uEdge == (int)AppBarEdge.Right)
            {
                //iWidth = abd.rc.right - abd.rc.left;
                //if (iWidth <= 0)
                iWidth = form.Width;
                abd.rc.top = 0;
                abd.rc.bottom = SystemInformation.PrimaryMonitorSize.Height;
                if (edge == AppBarEdge.Right)
                    abd.rc.right = SystemInformation.PrimaryMonitorSize.Width;
            }
            else
            {
                //iHeight = abd.rc.bottom - abd.rc.top;
                //if (iHeight <= 0)
                iHeight = form.Height;
                abd.rc.left = 0;
                abd.rc.right = SystemInformation.PrimaryMonitorSize.Width;
                if (edge == AppBarEdge.Bottom)
                    abd.rc.bottom = SystemInformation.PrimaryMonitorSize.Height;
            }

            // Query the system for an approved size and position. 
            Services.QueryPosition(ref abd);

            // Adjust the rectangle, depending on the edge to which the 
            // appbar is anchored. 
            switch (abd.uEdge)
            {
                case (int)AppBarEdge.Left:
                    abd.rc.right = abd.rc.left + iWidth;
                    break;
                case (int)AppBarEdge.Right:
                    abd.rc.left = abd.rc.right - iWidth;
                    break;
                case (int)AppBarEdge.Top:
                    abd.rc.bottom = abd.rc.top + iHeight;
                    break;
                case (int)AppBarEdge.Bottom:
                    abd.rc.top = abd.rc.bottom - iHeight;
                    break;
            }

            // Pass the final bounding rectangle to the system. 
            Services.SetPosition(ref abd);

            // Move and size the appbar so that it conforms to the 
            // bounding rectangle passed to the system. 
            if (move)
                Win32.MoveWindow(abd.hWnd, abd.rc.left, abd.rc.top, abd.rc.Width, abd.rc.Height, true);
        }

        /// <summary>
        /// Converts a window to an AppBar
        /// </summary>
        /// <param name="form">The form to convert</param>
        /// <param name="edge">The edge of the screen to attach the AppBar</param>
        public static void MakeAppBar(System.Windows.Forms.Form form, AppBarEdge edge = AppBarEdge.Top)
        {
            var info = GetRegisterInfo(form);
            info.Edge = edge;
            if (!info.IsRegistered)
            {
                form.FormClosing += form_FormClosing;
                form.ResizeBegin += form_ResizeBegin;
                form.ResizeEnd += form_ResizeEnd;
                //form.MouseDown += form_MouseDown;
                //form.MouseUp += form_MouseUp;
                //form.MouseMove += form_MouseMove;
                form.Move += form_Move;
                form.LocationChanged += form_LocationChanged;
                if (form.IsHandleCreated)
                    Register(form, edge);
                else
                    form.HandleCreated += (o, e) => { Register(form, edge); };
            }
            else
            {
                if (edge == AppBarEdge.None)
                    Unregister(info.Form);
                else
                    SetPosition(form, info.Edge);
            }
        }
    }
}
