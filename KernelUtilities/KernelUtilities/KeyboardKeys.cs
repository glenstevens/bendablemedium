using System;
using System.Collections.Generic;
using System.Text;

namespace KernelUtilities
{
    /// <summary>
    /// Represents all the keys on the keyboad
    /// </summary>
    /// <remarks>
    /// Not tested on international keyboards, based on numbers returned from Windows.Forms.Keys Enum
    /// which does not seem to differentiate between Left & Right Shift/Alt, Control or the Enter keys
    /// on the regular keyboard and the Enter key on the numeric keyboard
    /// </remarks>
    public enum KeyboardKeys
    {
        /// <summary>
        /// No key pressed
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Untested: Control-break processing
        /// </summary>
        VK_CANCEL = 0x0003,
        Backspace = 0x0008,
        Tab = 0x0009,

        /// <summary>
        /// Untested: CLEAR key
        /// </summary>
        VK_CLEAR = 0x000C,
        Return = 0x000d,
        Enter = 0x000d,
        Shift = 0x0010,
        Ctrl = 0x0011,
        Alt = 0x0012,
        Pause = 0x0013,
        CapsLock = 0x0014,
        Escape = 0x001b,
        Space = 0x0020,
        PageUp = 0x0021,
        PageDown = 0x0022,
        End = 0x0023,
        Home = 0x0024,
        Left = 0x0025,
        Up = 0x0026,
        Right = 0x0027,
        Down = 0x0028,

        /// <summary>
        /// Untested: SELECT key
        /// </summary>
        VK_SELECT = 0x0029,

        /// <summary>
        /// Untested: PRINT key
        /// </summary>
        VK_PRINT = 0x002A,

        /// <summary>
        /// Untested: EXECUTE key
        /// </summary>
        VK_EXECUTE = 0x002B,

        /// <summary>
        /// Untested: PRINT SCREEN key
        /// </summary>
        VK_SNAPSHOT = 0x002C,
        Insert = 0x002d,
        Delete = 0x002e,

        /// <summary>
        /// Untested: HELP key
        /// </summary>
        VK_HELP = 0x002F,
        D0 = 0x0030,
        D1 = 0x0031,
        D2 = 0x0032,
        D3 = 0x0033,
        D4 = 0x0034,
        D5 = 0x0035,
        D6 = 0x0036,
        D7 = 0x0037,
        D8 = 0x0038,
        D9 = 0x0039,
        A = 0x0041,
        B = 0x0042,
        C = 0x0043,
        D = 0x0044,
        E = 0x0045,
        F = 0x0046,
        G = 0x0047,
        H = 0x0048,
        I = 0x0049,
        J = 0x004a,
        K = 0x004b,
        L = 0x004c,
        M = 0x004d,
        N = 0x004e,
        O = 0x004f,
        P = 0x0050,
        Q = 0x0051,
        R = 0x0052,
        S = 0x0053,
        T = 0x0054,
        U = 0x0055,
        V = 0x0056,
        W = 0x0057,
        X = 0x0058,
        Y = 0x0059,
        Z = 0x005a,
        Windows = 0x005b,
        Apps = 0x005d,
        NumPad0 = 0x0060,
        NumPad1 = 0x0061,
        NumPad2 = 0x0062,
        NumPad3 = 0x0063,
        NumPad4 = 0x0064,
        NumPad5 = 0x0065,
        NumPad6 = 0x0066,
        NumPad7 = 0x0067,
        NumPad8 = 0x0068,
        NumPad9 = 0x0069,
        NumPadMultiply = 0x006a,
        NumPadAdd = 0x006b,

        /// <summary>
        /// Untested: Separator key
        /// </summary>
        VK_SEPARATOR = 0x006C,
        NumPadSubtract = 0x006d,
        NumPadDecimal = 0x006e,
        NumPadDivide = 0x006f,
        F1 = 0x0070,
        F2 = 0x0071,
        F3 = 0x0072,
        F4 = 0x0073,
        F5 = 0x0074,
        F6 = 0x0075,
        F7 = 0x0076,
        F8 = 0x0077,
        F9 = 0x0078,
        F10 = 0x0079,
        F11 = 0x007a,
        F12 = 0x007b,
        NumLock = 0x0090,
        ScrollLock = 0x0091,

        /// <summary>
        /// Untested: Left SHIFT key
        /// </summary>
        VK_LSHIFT = 0x00A0,

        /// <summary>
        /// Untested: Right SHIFT key
        /// </summary>
        VK_RSHIFT = 0x00A1,

        /// <summary>
        /// Untested: Left CONTROL key
        /// </summary>
        VK_LCONTROL = 0x00A2,

        /// <summary>
        /// Untested: Right CONTROL key
        /// </summary>
        VK_RCONTROL = 0x00A3,

        /// <summary>
        /// Untested: Left MENU (Alt) key
        /// </summary>
        VK_LMENU = 0x00A4,

        /// <summary>
        /// Untested: Right MENU (Alt) key
        /// </summary>
        VK_RMENU = 0x00A5,
        SemiColon = 0x00ba,
        Plus = 0x00bb,
        Comma = 0x00bc,
        Minus = 0x00bd,
        Period = 0x00be,
        Slash = 0x00bf,
        Question = 0x00bf,
        Tilde = 0x00c0,
        Apostrophe = 0x00c0,
        OpenBrackets = 0x00db,
        Backslash = 0x00dc,
        Pipe = 0x00dc,
        CloseBrackets = 0x00dd,
        Quotes = 0x00de,

        /// <summary>
        /// Untested: Play key
        /// </summary>
        VK_PLAY = 0x00FA,

        /// <summary>
        /// Untested: Zoom key
        /// </summary>
        VK_ZOOM = 0x00FB,
    }
}
