using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft;
using System.Runtime.InteropServices;

namespace W
{
    public partial class AppBar
    {
        //1.5.2014 - taken from http://msdn.microsoft.com/en-us/library/ms229658(v=vs.90).aspx
        internal class WndProcHooker
        {
            // The WndProcCallback method is used when a hooked 
            // window's message map contains the hooked message. 
            // Parameters: 
            // hwnd - The handle to the window for which the message 
            // was received. 
            // wParam - The message's parameters (part 1). 
            // lParam - The message's parameters (part 2). 
            // handled - The invoked function sets this to true if it 
            // handled the message. If the value is false when the callback 
            // returns, the next window procedure in the wndproc chain is 
            // called. 
            // 
            // Returns a value specified for the given message. 
            public delegate int WndProcCallback(IntPtr hwnd, uint msg, uint wParam, int lParam, ref bool handled);

            // This is the global list of all the window procedures we have 
            // hooked. The key is an hwnd. The value is a HookedProcInformation 
            // object which contains a pointer to the old wndproc and a map of 
            // message's callbacks for the window specified. Controls whose handles 
            // have been created go into this dictionary. 
            private static Dictionary<IntPtr, HookedProcInformation> hwndDict = new Dictionary<IntPtr, HookedProcInformation>();

            // The key for this dictionary is a control and the value is a 
            // HookedProcInformation. Controls whose handles have not been created 
            // go into this dictionary. When the HandleCreated event for the 
            // control is fired the control is moved into hwndDict. 
            private static Dictionary<Control, HookedProcInformation> ctlDict = new Dictionary<Control, HookedProcInformation>();

            // Makes a connection between a message on a specified window handle 
            // and the callback to be called when that message is received. If the 
            // window was not previously hooked it is added to the global list of 
            // all the window procedures hooked. 
            // Parameters: 
            // ctl - The control whose wndproc we are hooking. 
            // callback - The method to call when the specified. 
            // message is received for the specified window. 
            // msg - The message being hooked. 
            public static void HookWndProc(Control ctl, WndProcCallback callback, uint msg)
            {
                HookedProcInformation hpi = null;
                if (ctlDict.ContainsKey(ctl))
                    hpi = ctlDict[ctl];
                else if (hwndDict.ContainsKey(ctl.Handle))
                    hpi = hwndDict[ctl.Handle];
                if (hpi == null)
                {
                    // If new control, create a new 
                    // HookedProcInformation for it.
                    hpi = new HookedProcInformation(ctl, new Win32.WndProc(WndProcHooker.WindowProc));
                    ctl.HandleCreated += new EventHandler(ctl_HandleCreated);
                    ctl.HandleDestroyed += new EventHandler(ctl_HandleDestroyed);
                    ctl.Disposed += new EventHandler(ctl_Disposed);

                    // If the handle has already been created set the hook. If it 
                    // hasn't been created yet, the hook will get set in the 
                    // ctl_HandleCreated event handler. 
                    if (ctl.Handle != IntPtr.Zero)
                        hpi.SetHook();
                }

                // Stick hpi into the correct dictionary. 
                if (ctl.Handle == IntPtr.Zero)
                    ctlDict[ctl] = hpi;
                else
                    hwndDict[ctl.Handle] = hpi;

                // Add the message/callback into the message map.
                hpi.messageMap[msg] = callback;
            }

            // The event handler called when a control is disposed. 
            static void ctl_Disposed(object sender, EventArgs e)
            {
                Control ctl = sender as Control;
                if (ctlDict.ContainsKey(ctl))
                    ctlDict.Remove(ctl);
                else
                    System.Diagnostics.Debug.Assert(false);
            }

            // The event handler called when a control's handle is destroyed. 
            // We remove the HookedProcInformation from hwndDict and 
            // put it back into ctlDict in case the control get re- 
            // created and we still want to hook its messages. 
            static void ctl_HandleDestroyed(object sender, EventArgs e)
            {
                // When the handle for a control is destroyed, we want to 
                // unhook its wndproc and update our lists
                Control ctl = sender as Control;
                if (hwndDict.ContainsKey(ctl.Handle))
                {
                    HookedProcInformation hpi = hwndDict[ctl.Handle];
                    UnhookWndProc(ctl, false);
                }
                else
                    System.Diagnostics.Debug.Assert(false);
            }

            // The event handler called when a control's handle is created. We 
            // call SetHook() on the associated HookedProcInformation object and 
            // move it from ctlDict to hwndDict. 
            static void ctl_HandleCreated(object sender, EventArgs e)
            {
                Control ctl = sender as Control;
                if (ctlDict.ContainsKey(ctl))
                {
                    HookedProcInformation hpi = ctlDict[ctl];
                    hwndDict[ctl.Handle] = hpi;
                    ctlDict.Remove(ctl);
                    hpi.SetHook();
                }
                else
                    System.Diagnostics.Debug.Assert(false);
            }

            // This is a generic wndproc. It is the callback for all hooked 
            // windows. If we get into this function, we look up the hwnd in the 
            // global list of all hooked windows to get its message map. If the 
            // message received is present in the message map, its callback is 
            // invoked with the parameters listed here. 
            // Parameters: 
            // hwnd - The handle to the window that received the 
            // message 
            // msg - The message 
            // wParam - The message's parameters (part 1) 
            // lParam - The messages's parameters (part 2) 
            // Returns the callback handled the message, the callback's return 
            // value is returned form this function. If the callback didn't handle 
            // the message, the message is forwarded on to the previous wndproc. 
            private static int WindowProc(IntPtr hwnd, uint msg, uint wParam, int lParam)
            {
                if (hwndDict.ContainsKey(hwnd))
                {
                    HookedProcInformation hpi = hwndDict[hwnd];
                    if (hpi.messageMap.ContainsKey(msg))
                    {
                        WndProcCallback callback = hpi.messageMap[msg];
                        bool handled = false;
                        int retval = callback(hwnd, msg, wParam, lParam, ref handled);
                        if (handled)
                            return retval;
                    }

                    // If the callback didn't set the handled property to true, 
                    // call the original window procedure. 
                    return hpi.CallOldWindowProc(hwnd, msg, wParam, lParam);
                }
                System.Diagnostics.Debug.Assert(false, "WindowProc called for hwnd we don't know about");
                return Win32.DefWindowProc(hwnd, msg, wParam, lParam);
            }

            // This method removes the specified message from the message map for 
            // the specified hwnd. 
            public static void UnhookWndProc(Control ctl, uint msg)
            {
                // Look for the HookedProcInformation in the 
                // ctrDict and hwndDict dictionaries.
                HookedProcInformation hpi = null;
                if (ctlDict.ContainsKey(ctl))
                    hpi = ctlDict[ctl];
                else if (hwndDict.ContainsKey(ctl.Handle))
                {
                    hpi = hwndDict[ctl.Handle];
                    if (hpi != null)
                        hwndDict.Remove(ctl.Handle);
                }
                // if we couldn't find a HookedProcInformation, throw 
                if (hpi == null)
                    throw new ArgumentException("No hook exists for this control");

                hpi.Unhook();
                ctl.HandleCreated -= new EventHandler(ctl_HandleCreated);
                ctl.HandleDestroyed -= new EventHandler(ctl_HandleDestroyed);
                ctl.Disposed -= new EventHandler(ctl_Disposed);

                // look for the message we are removing in the messageMap 
                if (hpi.messageMap.ContainsKey(msg))
                    hpi.messageMap.Remove(msg);
                else
                    // if we couldn't find the message, throw 
                    throw new ArgumentException(string.Format("No hook exists for message ({0}) on this control", msg));
            }

            // Restores the previous wndproc for the specified window. 
            // Parameters: 
            // ctl - The control whose wndproc we no longer want to hook. 
            // disposing - True if HookedProcInformation is not 
            //   read back into ctlDict. 
            public static void UnhookWndProc(Control ctl, bool disposing)
            {
                HookedProcInformation hpi = null;
                if (ctlDict.ContainsKey(ctl))
                    hpi = ctlDict[ctl];
                else if (hwndDict.ContainsKey(ctl.Handle))
                    hpi = hwndDict[ctl.Handle];
                if (hpi == null)
                    throw new ArgumentException("No hook exists for this control");

                // If we found our HookedProcInformation in ctlDict and we are 
                // disposing remove it from ctlDict. 
                if (ctlDict.ContainsKey(ctl) && disposing)
                    ctlDict.Remove(ctl);

                // If we found our HookedProcInformation in hwndDict, remove it 
                // and if we are not disposing stick it in ctlDict. 
                if (hwndDict.ContainsKey(ctl.Handle))
                {
                    hpi.Unhook();
                    hwndDict.Remove(ctl.Handle);
                    if (!disposing)
                        ctlDict[ctl] = hpi;
                }
            }

            // This class remembers the old window procedure for the specified 
            // window handle and also provides the message map for the messages 
            // hooked on that window. 
            class HookedProcInformation
            {
                // The message map for the window. 
                public Dictionary<uint, WndProcCallback> messageMap;

                // The old window procedure for the window. 
                private IntPtr oldWndProc;

                // The delegate that gets called in place of this window's 
                // wndproc. 
                private Win32.WndProc newWndProc;

                // Control whose wndproc is being hooked. 
                private Control control;

                // Constructs a new HookedProcInformation object 
                // Parameters: 
                // ctl - The handle to the window being hooked 
                // wndproc - The window procedure to replace the 
                // original one for the control. 
                public HookedProcInformation(Control ctl, Win32.WndProc wndproc)
                {
                    control = ctl;
                    newWndProc = wndproc;
                    messageMap = new Dictionary<uint, WndProcCallback>();
                }

                // Replaces the windows procedure for control with the 
                // one specified in the constructor. 
                public void SetHook()
                {
                    IntPtr hwnd = control.Handle;
                    if (hwnd == IntPtr.Zero)
                        throw new InvalidOperationException("Handle for control has not been created");

                    oldWndProc = Win32.SetWindowLong(hwnd, Win32.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(newWndProc));
                }

                // Restores the original window procedure for the control. 
                public void Unhook()
                {
                    IntPtr hwnd = control.Handle;
                    if (hwnd == IntPtr.Zero)
                        throw new InvalidOperationException("Handle for control has not been created");

                    Win32.SetWindowLong(hwnd, Win32.GWL_WNDPROC, oldWndProc);
                }

                // Calls the original window procedure of the control with the 
                // arguments provided. 
                // Parameters: 
                // hwnd - The handle of the window that received the 
                // message 
                // msg - The message 
                // wParam - The message's arguments (part 1) 
                // lParam - The message's arguments (part 2) 
                // Returns the value returned by the control's original wndproc. 
                public int CallOldWindowProc(IntPtr hwnd, uint msg, uint wParam, int lParam)
                {
                    return Win32.CallWindowProc(oldWndProc, hwnd, msg, wParam, lParam);
                }
            }
        }
    }
}