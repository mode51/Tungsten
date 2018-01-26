#if NET45
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace W.Windows
{
    /// <summary>
    /// Aids in finding windows from their Window Text
    /// </summary>
    public class FindWindow
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        // Delegate to filter which windows to include 
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        /// <summary> Get the text for the window pointed to by hWnd </summary>
        private static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        /// <summary> Find all windows that match the given filter </summary>
        /// <param name="filter"> A delegate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned </param>
        private static List<IntPtr> Find(EnumWindowsProc filter, bool findAll)
        {
            IntPtr found = IntPtr.Zero;
            var windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                if (filter(wnd, param))
                {
                    // only add the windows that pass the filter
                    windows.Add(wnd);
                    if (!findAll)
                        return false;
                }

                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary> Find all windows matching based on the predicate </summary>
        /// <param name="filter"><para>A predicate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned</para></param>
        public static List<IntPtr> Find(Predicate<string> filter, bool findAll = false)
        {
            return Find((w, p) => { return filter.Invoke(GetWindowText(w)); }, findAll);
        }
    }
}
#endif