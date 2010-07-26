using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace KernelUtilities
{
    public static class WindowUtilities
    {
        static string _windowTitle;
        static EnumWindowsProc callbackProc = new EnumWindowsProc(FindWindowCb);   // This keeps the proc from being GC'd while the Win32 function is running

        #region Win32 External Functions
        private delegate bool EnumWindowsProc(IntPtr hWnd, ref IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        #endregion Win32 External Functions

        #region FindWindow
        /// <summary>
        /// Finds the window with the supplied title.
        /// </summary>
        /// <param name="windowTitle">The title of the window</param>
        /// <returns>Window handle or IntPtr.Zero if not found</returns>
        public static IntPtr FindWindow(string windowTitle)
        {
            if (string.IsNullOrEmpty(windowTitle)) return IntPtr.Zero;

            _windowTitle = windowTitle;
            IntPtr windowHandle = IntPtr.Zero;

            // Go through each open window and check the name for a match
            EnumWindows(callbackProc, ref windowHandle);

            return windowHandle;
        }

        // Callback for FindWindow enumeration
        static bool FindWindowCb(IntPtr hWnd, ref IntPtr lParam)
        {
            if (GetWindowTitle(hWnd) == _windowTitle)
            {
                lParam = hWnd;
                return false;   // Found the window, stop looking
            }
            return true;
        }
        #endregion FindWindow

        #region Window Data Methods
        /// <summary>
        /// Get the title of the window
        /// </summary>
        /// <param name="windowHandle">Window handle of the target window</param>
        /// <returns>Title of the window</returns>
        public static string GetWindowTitle(IntPtr windowHandle)
        {
            StringBuilder sbName = new StringBuilder(1024);
            GetWindowText(windowHandle, sbName, sbName.Capacity);
            return sbName.ToString();
        }

        /// <summary>
        /// Get the size of the window
        /// </summary>
        /// <param name="windowHandle">Window handle of the target window</param>
        /// <returns>RECT of the top, left, bottom, and right of the window</returns>
        public static RECT GetWindowSize(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero) return new RECT();

            // Get the Top Left, and Bottom Right coords of the window client area
            RECT result = new RECT();
            GetClientRect(windowHandle, out result);
            return result;
        }

        /// <summary>
        /// Get the position of the window
        /// </summary>
        /// <param name="windowHandle">Window handle of the target window</param>
        /// <returns>Screen coordinates of the window</returns>
        public static Point GetWindowPosition(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero) return new Point();

            // Get the screen coord of the top left of the window
            Point windowTopLeftScreenPosition = new Point();
            ClientToScreen(windowHandle, ref windowTopLeftScreenPosition);
            return windowTopLeftScreenPosition;
        }

        /// <summary>
        /// Convenience method to get the center point of the window
        /// </summary>
        /// <param name="windowHandle">Window handle of the target window</param>
        /// <returns>Point at the center of the window</returns>
        public static Point GetWindowCenterPoint(IntPtr windowHandle)
        {
            if (windowHandle == null) return new Point();

            // Get window origin
            Point origin = WindowUtilities.GetWindowPosition(windowHandle);

            // Get window size
            RECT rect = WindowUtilities.GetWindowSize(windowHandle);

            // position acordingly
            Point location = new Point(origin.X + (int)(rect.Width * 0.5f),
                                        origin.Y + (int)(rect.Height * 0.5f));

            return location;
        }
        #endregion Window Data Methods

        #region Active Window
        /// <summary>
        /// Get the window handle of the currently active window
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetActiveWindow()
        {
            return GetForegroundWindow();
        }

        /// <summary>
        /// Gets the title of the currently active window
        /// </summary>
        /// <returns>The current active window title</returns>
        public static string GetActiveWindowTitle()
        {
            return GetWindowTitle(GetActiveWindow());
        }
        #endregion Active Window
    }
}
