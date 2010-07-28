using System;
using System.Runtime.InteropServices;

namespace KernelUtilities
{
    public static class KeyboardUtilities
    {
        #region Win32 External Functions
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        // PostMessage will return immediately, SendMessage will wait for the call to return
        #endregion Win32 External Functions

        #region PostKeyClick
        /// <summary>
        /// Post a key event to a window and return immediately
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="key">Key to send</param>
        /// <param name="pressed">Whether the key is pressed or released</param>
        /// <returns>True if successful</returns>
        public static bool PostKeyClick(IntPtr windowHandle, IntPtr key, bool pressed)
        {
            uint msg = GetKeypressWM(pressed);

            // lParam is always 0 for button press/released
            IntPtr lParam = IntPtr.Zero;

            return PostMessage(windowHandle, msg, key, lParam);
        }

        /// <summary>
        /// Post a key event to a window and return immediately
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="key">Key to send</param>
        /// <param name="modifiers">Any modifier keys sent at the same time</param>
        /// <param name="pressed">Whether the key is pressed or released</param>
        /// <returns>True if successful</returns>
        public static bool PostKeyClick(IntPtr windowHandle, KeyboardKeys key, ModifierKeys modifiers, bool pressed)
        {
            uint msg = GetKeypressWM(pressed);

            // lParam is always 0 for button press/released
            IntPtr lParam = IntPtr.Zero;

            // If the key is pressed, press the modifier first
            if (pressed)
            {
                PostModifierKeys(windowHandle, modifiers, msg, lParam);
            }

            bool rc = PostMessage(windowHandle, msg, VirtualKeyFromKey(key), lParam);

            // If the key is released, release the modifier last
            if (!pressed)
            {
                PostModifierKeys(windowHandle, modifiers, msg, lParam);
            }
            return rc;
        }

        static void PostModifierKeys(IntPtr windowHandle, ModifierKeys modifiers, uint msg, IntPtr lParam)
        {
            if ((modifiers & ModifierKeys.Control) > 0) PostMessage(windowHandle, msg, VirtualKeyFromKey(KeyboardKeys.Ctrl), lParam);
            if ((modifiers & ModifierKeys.Alt) > 0) PostMessage(windowHandle, msg, VirtualKeyFromKey(KeyboardKeys.Alt), lParam);
            if ((modifiers & ModifierKeys.Shift) > 0) PostMessage(windowHandle, msg, VirtualKeyFromKey(KeyboardKeys.Shift), lParam);
        }
        #endregion PostKeyClick

        #region SendKeyClick
        /// <summary>
        /// Send a key event and wait for the window queue to process the message
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="key">Key to send</param>
        /// <param name="pressed">Whether the key is pressed or released</param>
        /// <returns>Message result code</returns>
        public static IntPtr SendKeyClick(IntPtr windowHandle, IntPtr key, bool pressed)
        {
            uint msg = GetKeypressWM(pressed);

            // lParam is always 0 for button press and <shrug> for released
            IntPtr lParam = pressed ? IntPtr.Zero : (IntPtr)0xC0000000;

            return SendMessage(windowHandle, msg, key, lParam);
        }

        /// <summary>
        /// Send a key event and wait for the window queue to process the message
        /// </summary>
        /// <param name="windowHandle">The target window</param>
        /// <param name="key">Key to send</param>
        /// <param name="modifiers">Any modifier keys sent at the same time</param>
        /// <param name="pressed">Whether the key is pressed or released</param>
        /// <returns>Message result code</returns>
        public static IntPtr SendKeyClick(IntPtr windowHandle, KeyboardKeys key, ModifierKeys modifiers, bool pressed)
        {
            IntPtr rc = IntPtr.Zero;
            uint msg = GetKeypressWM(pressed);

            // lParam is always 0 for button press and <shrug> for released
            IntPtr lParam = pressed ? IntPtr.Zero : (IntPtr)0xC000000;

            // If the key is pressed, press the modifier first
            if (pressed)
            {
                SendModifierKeys(windowHandle, modifiers, msg, lParam);
            }

            rc = SendMessage(windowHandle, msg, VirtualKeyFromKey(key), lParam);

            // If the key is released, release the modifier last
            if (!pressed)
            {
                SendModifierKeys(windowHandle, modifiers, msg, lParam);
            }
            return rc;
        }

        static void SendModifierKeys(IntPtr windowHandle, ModifierKeys modifiers, uint msg, IntPtr lParam)
        {
            if ((modifiers & ModifierKeys.Control) > 0) SendMessage(windowHandle, msg, VirtualKeyFromKey(KeyboardKeys.Ctrl), lParam);
            if ((modifiers & ModifierKeys.Alt) > 0) SendMessage(windowHandle, msg, VirtualKeyFromKey(KeyboardKeys.Alt), lParam);
            if ((modifiers & ModifierKeys.Shift) > 0) SendMessage(windowHandle, msg, VirtualKeyFromKey(KeyboardKeys.Shift), lParam);
        }

        private static uint GetKeypressWM(bool pressed)
        {
            return (uint)(pressed ? WMessages.KeyDown : WMessages.KeyUp);
        }

        static IntPtr VirtualKeyFromKey(KeyboardKeys key)
        {
            return (IntPtr)key;
        }
        #endregion SendKeyClick
    }
}
