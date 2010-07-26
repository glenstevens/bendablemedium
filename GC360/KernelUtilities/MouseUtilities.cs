using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace KernelUtilities
{
    public static class MouseUtilities
    {
        #region Win32 External Functions
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        // PostMessage will return immediately, SendMessage will wait for the call to return
        #endregion Win32 External Functions

        #region MouseMove
        /// <summary>
        /// Post a mouse move event and return immediately
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="mousePosition">The destination point</param>
        /// <returns>True if successful</returns>
        public static bool PostMouseMove(IntPtr windowHandle, Point mousePosition)
        {
            return PostMouseMove(windowHandle, ConvertMousePosition(mousePosition));
        }

        /// <summary>
        /// Post a mouse move event and return immediately
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="x">The horizontal position</param>
        /// <param name="y">The vertical position</param>
        /// <returns>True if successful</returns>
        public static bool PostMouseMove(IntPtr windowHandle, int x, int y)
        {
            return PostMouseMove(windowHandle, ConvertMousePosition(x, y));
        }

        static bool PostMouseMove(IntPtr windowHandle, IntPtr mousePosition)
        {
            return PostMessage(windowHandle, (uint)WMessages.MouseMove, IntPtr.Zero, mousePosition);
        }

        /// <summary>
        /// Send a mouse move event and wait for the window queue to process the request
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="mousePosition">The destination point</param>
        /// <returns>Message result code</returns>
        public static IntPtr SendMouseMove(IntPtr windowHandle, Point mousePosition)
        {
            return SendMouseMove(windowHandle, ConvertMousePosition(mousePosition));
        }

        /// <summary>
        /// Send a mouse move event and wait for the window queue to process the request
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="x">The horizontal position</param>
        /// <param name="y">The vertical position</param>
        /// <returns>Message result code</returns>
        public static IntPtr SendMouseMove(IntPtr windowHandle, int x, int y)
        {
            return SendMouseMove(windowHandle, ConvertMousePosition(x, y));
        }

        static IntPtr SendMouseMove(IntPtr windowHandle, IntPtr mousePosition)
        {
            return SendMessage(windowHandle, (uint)WMessages.MouseMove, IntPtr.Zero, mousePosition);
        }
        #endregion MouseMove

        #region MouseClick
        /// <summary>
        /// Post a mouse click event and return immediately
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="mousePosition">The click point</param>
        /// <param name="button">The button to press/release</param>
        /// <param name="pressed">Whether the button is pressed or released</param>
        /// <returns>True if successful</returns>
        public static bool PostMouseClick(IntPtr windowHandle, Point mousePosition, MouseButtons button, bool pressed)
        {
            return PostMouseClick(windowHandle, ConvertMousePosition(mousePosition), button, pressed);
        }

        /// <summary>
        /// Post a mouse click event and return immediately
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="x">The horizontal position</param>
        /// <param name="y">The vertical position</param>
        /// <param name="button">The button to press/release</param>
        /// <param name="pressed">Whether the button is pressed or released</param>
        /// <returns>True if successful</returns>
        public static bool PostMouseClick(IntPtr windowHandle, int x, int y, MouseButtons button, bool pressed)
        {
            return PostMouseClick(windowHandle, ConvertMousePosition(x, y), button, pressed);
        }

        static bool PostMouseClick(IntPtr windowHandle, IntPtr mousePosition, MouseButtons button, bool pressed)
        {
            IntPtr wParam = IntPtr.Zero;
            uint msg = GetMouseMessage(button, pressed, ref wParam);
            return PostMessage(windowHandle, msg, wParam, mousePosition);
        }

        /// <summary>
        /// Send a mouse click event and wait for the window queue to process the request
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="mousePosition">The click point</param>
        /// <param name="button">The button to press/release</param>
        /// <param name="pressed">Whether the button is pressed or released</param>
        /// <returns>Message result code</returns>
        public static IntPtr SendMouseClick(IntPtr windowHandle, Point mousePosition, MouseButtons button, bool pressed)
        {
            return SendMouseClick(windowHandle, ConvertMousePosition(mousePosition), button, pressed);
        }

        /// <summary>
        /// Send a mouse click event and wait for the window queue to process the request
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="x">The horizontal position</param>
        /// <param name="y">The vertical position</param>
        /// <param name="button">The button to press/release</param>
        /// <param name="pressed">Whether the button is pressed or released</param>
        /// <returns>Message result code</returns>
        public static IntPtr SendMouseClick(IntPtr windowHandle, int x, int y, MouseButtons button, bool pressed)
        {
            return SendMouseClick(windowHandle, ConvertMousePosition(x, y), button, pressed);
        }

        static IntPtr SendMouseClick(IntPtr windowHandle, IntPtr mousePosition, MouseButtons button, bool pressed)
        {
            IntPtr wParam = IntPtr.Zero;
            uint msg = GetMouseMessage(button, pressed, ref wParam);
            return SendMessage(windowHandle, msg, wParam, mousePosition);
        }

        static uint GetMouseMessage(MouseButtons button, bool pressed, ref IntPtr wParam)
        {
            uint msg = 0;
            switch (button)
            {
                case MouseButtons.Left:
                    msg = pressed ? (uint)WMessages.LeftMouseButtonDown : (uint)WMessages.LeftMouseButtonUp;
                    break;
                case MouseButtons.Middle:
                    msg = pressed ? (uint)WMessages.MiddleMouseButtonDown : (uint)WMessages.MiddleMouseButtonUp;
                    break;
                case MouseButtons.Right:
                    msg = pressed ? (uint)WMessages.RightMouseButtonDown : (uint)WMessages.RightMouseButtonUp;
                    break;
                case MouseButtons.XButton1:
                    msg = pressed ? (uint)WMessages.ExtendedMouseButtonDown : (uint)WMessages.ExtendedMouseButtonUp;
                    wParam = (IntPtr)WMessages.ExtendedButton1;
                    break;
                case MouseButtons.XButton2:
                    msg = pressed ? (uint)WMessages.ExtendedMouseButtonDown : (uint)WMessages.ExtendedMouseButtonUp;
                    wParam = (IntPtr)WMessages.ExtendedButton2;
                    break;
            }
            return msg;
        }
        #endregion MouseClick

        #region ConvertMousePosition
        static IntPtr ConvertMousePosition(int x, int y)
        {
            return new IntPtr(x | y << 0x10);
        }

        static IntPtr ConvertMousePosition(Point location)
        {
            return ConvertMousePosition(location.X, location.Y);
        }
        #endregion ConvertMousePosition
    }
}
