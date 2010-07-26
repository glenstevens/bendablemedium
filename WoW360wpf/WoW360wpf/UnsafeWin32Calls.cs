using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace WoW360wpf
{
    #region RECT Structure
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Height { get { return Bottom - Top; } }
        public int Width { get { return Right - Left; } }
        public Size Size { get { return new Size(Width, Height); } }

        public Point Location { get { return new Point(Left, Top); } }

        // Handy method for converting to a System.Drawing.Rectangle
        public Rectangle ToRectangle()
        { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }

        public static RECT FromRectangle(Rectangle rectangle)
        {
            return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        public override int GetHashCode()
        {
            return Left ^ ((Top << 13) | (Top >> 0x13))
              ^ ((Width << 0x1a) | (Width >> 6))
              ^ ((Height << 7) | (Height >> 0x19));
        }

        #region Operator overloads

        public static implicit operator Rectangle(RECT rect)
        {
            return rect.ToRectangle();
        }

        public static implicit operator RECT(Rectangle rect)
        {
            return FromRectangle(rect);
        }

        #endregion
    }
    #endregion RECT Structure

    public class UnsafeWin32Calls
    {
        #region Win32 External Functions
        private delegate bool EnumWindowsProc(IntPtr hWnd, ref IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        
        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        // PostMessage will return immediately, SendMessage will wait for the call to return
        #endregion Win32 External Functions

        #region Constants
        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;
        const UInt32 WM_LBUTTONDOWN = 0x0201;
        const UInt32 WM_LBUTTONUP = 0x0202;
        const UInt32 WM_MBUTTONDOWN = 0x0207;
        const UInt32 WM_MBUTTONUP = 0x0208;
        const UInt32 WM_RBUTTONDOWN = 0x0204;
        const UInt32 WM_RBUTTONUP = 0x0205;
        const UInt32 WM_MOUSEMOVE = 0x0200;
        const UInt32 WM_XBUTTONDOWN = 0x020B;
        const UInt32 WM_XBUTTONUP = 0x020C;
        const UInt32 XBUTTON1 = 0x01;
        const UInt32 XBUTTON2 = 0x02;
        const UInt32 WM_SYSKEYDOWN = 0x0104;
        const UInt32 WM_SYSKEYUP = 0x0105;
        #endregion Constants

        static string _windowTitle;
        static Rectangle _windowSize;
        static Point _windowTopLeftScreenPosition;
        static EnumWindowsProc callbackProc = new EnumWindowsProc(FindWindowCb);   // This keeps the proc from being GC'd while the Win32 function is running

        static IntPtr WindowHandle { get; set; }

        #region Window Methods
        /// <summary>
        /// Finds the window with the supplied title.
        /// </summary>
        /// <param name="windowTitle">The title of the window</param>
        /// <returns>False if window cannot be found</returns>
        public static bool FindWindow(string windowTitle)
        {
            if (string.IsNullOrEmpty(windowTitle)) return false;

            _windowTitle = windowTitle;
            WindowHandle = IntPtr.Zero;

            // Go through each open window and check the name for a match
            EnumWindows(callbackProc, WindowHandle);

            if (WindowHandle != IntPtr.Zero)
            {
                // Get the Top Left, and Bottom Right coords of the window client area
                _windowSize = GetClientRect(WindowHandle);
                // Get the screen coord of the top left of the window
                _windowTopLeftScreenPosition = new Point();
                ClientToScreen(WindowHandle, ref _windowTopLeftScreenPosition);
                return true;
            }
            return false;
        }

        static bool FindWindowCb(IntPtr hWnd, ref IntPtr lParam)
        {
            if (GetWindowTitle(hWnd) == _windowTitle)
            {
                WindowHandle = hWnd;
                return false;   // Found the window, stop looking
            }
            return true;
        }

        /// <summary>
        /// Gets the window's title
        /// </summary>
        /// <param name="windowHandle"></param>
        /// <returns></returns>
        public static string GetWindowTitle(IntPtr windowHandle)
        {
            StringBuilder sbName = new StringBuilder(1024);
            GetWindowText(windowHandle, sbName, sbName.Capacity);
            return sbName.ToString();
        }

        public static IntPtr GetActiveWindow()
        {
            return GetForegroundWindow();
        }

        public static string GetActiveWindowTitle()
        {
            IntPtr currentWindowHandle = GetActiveWindow();
            return UnsafeWin32Calls.GetWindowTitle(currentWindowHandle);
        }

        static RECT GetClientRect(IntPtr hWnd)
        {
            RECT result = new RECT();
            GetClientRect(hWnd, out result);
            return result;
        }

        /// <summary>
        /// Ensure the window has been found, or find the window if it isn't valid
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns>False only if the window cannot be found</returns>
        public static bool EnsureWindowValid(string windowTitle)
        {
            // check and make sure the window's still up
            if (WindowHandle != IntPtr.Zero && GetWindowTitle(WindowHandle) != windowTitle)
            {
                WindowHandle = IntPtr.Zero;
            }

            // If the window handle has been reset, attempt to find it again
            if (WindowHandle == IntPtr.Zero)
            {
                FindWindow(windowTitle);
            }
            return WindowHandle == IntPtr.Zero;
        }
        #endregion Window Methods

        #region MouseMove
        public static bool PostMouseMove(Point mousePosition)
        {
            return PostMouseMove(ConvertMousePosition(mousePosition));
        }

        public static bool PostMouseMove(int x, int y)
        {
            return PostMouseMove(ConvertMousePosition(x, y));
        }

        public static bool PostMouseMove(float x, float y)
        {
            return PostMouseMove(ConvertMousePosition(x, y));
        }

        static bool PostMouseMove(IntPtr mousePosition)
        {
            return PostMessage(WindowHandle, WM_MOUSEMOVE, IntPtr.Zero, mousePosition);
        }

        public static IntPtr SendMouseMove(Point mousePosition)
        {
            return SendMouseMove(ConvertMousePosition(mousePosition));
        }

        public static IntPtr SendMouseMove(int x, int y)
        {
            return SendMouseMove(ConvertMousePosition(x, y));
        }

        public static IntPtr SendMouseMove(float x, float y)
        {
            return SendMouseMove(ConvertMousePosition(x, y));
        }

        static IntPtr SendMouseMove(IntPtr mousePosition)
        {
            return SendMessage(WindowHandle, WM_MOUSEMOVE, IntPtr.Zero, mousePosition);
        }
        #endregion MouseMove

        #region MouseClick
        public static bool PostMouseClick(Point mousePosition, MouseButton button, bool pressed)
        {
            return PostMouseClick(ConvertMousePosition(mousePosition), button, pressed);
        }

        public static bool PostMouseClick(int x, int y, MouseButton button, bool pressed)
        {
            return PostMouseClick(ConvertMousePosition(x, y), button, pressed);
        }

        public static bool PostMouseClick(float x, float y, MouseButton button, bool pressed)
        {
            return PostMouseClick(ConvertMousePosition(x, y), button, pressed);
        }

        static bool PostMouseClick(IntPtr mousePosition, MouseButton button, bool pressed)
        {
            IntPtr wParam = IntPtr.Zero;
            uint msg = GetMouseMessage(button, pressed, ref wParam);
            return PostMessage(WindowHandle, msg, wParam, mousePosition);
        }

        public static IntPtr SendMouseClick(Point mousePosition, MouseButton button, bool pressed)
        {
            return SendMouseClick(ConvertMousePosition(mousePosition), button, pressed);
        }

        public static IntPtr SendMouseClick(int x, int y, MouseButton button, bool pressed)
        {
            return SendMouseClick(ConvertMousePosition(x, y), button, pressed);
        }

        public static IntPtr SendMouseClick(float x, float y, MouseButton button, bool pressed)
        {
            return SendMouseClick(ConvertMousePosition(x, y), button, pressed);
        }

        static IntPtr SendMouseClick(IntPtr mousePosition, MouseButton button, bool pressed)
        {
            IntPtr wParam = IntPtr.Zero;
            uint msg = GetMouseMessage(button, pressed, ref wParam);
            return SendMessage(WindowHandle, msg, wParam, mousePosition);
        }

        static uint GetMouseMessage(MouseButton button, bool pressed, ref IntPtr wParam)
        {
            uint msg = 0;
            switch (button)
            {
                case MouseButton.Left:
                    msg = pressed ? WM_LBUTTONDOWN : WM_LBUTTONUP;
                    break;
                case MouseButton.Middle:
                    msg = pressed ? WM_MBUTTONDOWN : WM_MBUTTONUP;
                    break;
                case MouseButton.Right:
                    msg = pressed ? WM_RBUTTONDOWN : WM_RBUTTONUP;
                    break;
                case MouseButton.XButton1:
                    msg = pressed ? WM_XBUTTONDOWN : WM_XBUTTONUP;
                    wParam = (IntPtr)XBUTTON1;
                    break;
                case MouseButton.XButton2:
                    msg = pressed ? WM_XBUTTONDOWN : WM_XBUTTONUP;
                    wParam = (IntPtr)XBUTTON2;
                    break;
            }
            return msg;
        }
        #endregion MouseClick

        #region KeyClick
        public static bool PostKeyClick(Key key, ModifierKeys modifiers, bool pressed)
        {
            uint msg = pressed ? WM_KEYDOWN : WM_KEYUP;

            // lParam is always 0 for button press/released
            IntPtr lParam = IntPtr.Zero;

            // If the key is pressed, press the modifier first
            if (pressed)
            {
                PostModifierKeys(modifiers, msg, lParam);
            }

            bool rc = PostMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(key), lParam);

            // If the key is released, release the modifier last
            if (!pressed)
            {
                PostModifierKeys(modifiers, msg, lParam);
            }
            return rc;
        }

        private static void PostModifierKeys(ModifierKeys modifiers, uint msg, IntPtr lParam)
        {
            if ((modifiers & ModifierKeys.Control) > 0) PostMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(Key.LeftCtrl), lParam);
            if ((modifiers & ModifierKeys.Alt) > 0) PostMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(Key.LeftAlt), lParam);
            if ((modifiers & ModifierKeys.Shift) > 0) PostMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(Key.LeftShift), lParam);
        }

        public static IntPtr SendKeyClick(Key key, ModifierKeys modifiers, bool pressed)
        {
            IntPtr rc = IntPtr.Zero;
            uint msg = pressed ? WM_KEYDOWN : WM_KEYUP;

            // lParam is always 0 for button press and <shrug> for released
            IntPtr lParam = pressed ? IntPtr.Zero : (IntPtr)0xC0000000;

            // If the key is pressed, press the modifier first
            if (pressed)
            {
                SendModifierKeys(modifiers, msg, lParam);
            }

            rc = SendMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(key), lParam);

            // If the key is released, release the modifier last
            if (!pressed)
            {
                SendModifierKeys(modifiers, msg, lParam);
            }
            return rc;
        }

        private static void SendModifierKeys(ModifierKeys modifiers, uint msg, IntPtr lParam)
        {
            if ((modifiers & ModifierKeys.Control) > 0) SendMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(Key.LeftCtrl), lParam);
            if ((modifiers & ModifierKeys.Alt) > 0) SendMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(Key.LeftAlt), lParam);
            if ((modifiers & ModifierKeys.Shift) > 0) SendMessage(WindowHandle, msg, (IntPtr)KeyInterop.VirtualKeyFromKey(Key.LeftShift), lParam);
        }
        #endregion KeyClick

        #region ConvertMousePosition
        static IntPtr ConvertMousePosition(int x, int y)
        {
            return new IntPtr(x | y << 0x10);
        }

        static IntPtr ConvertMousePosition(float x, float y)
        {
            return ConvertMousePosition((int)x, (int)y);
        }

        static IntPtr ConvertMousePosition(Point location)
        {
            return ConvertMousePosition(location.X, location.Y);
        }
        #endregion ConvertMousePosition
    }
}
