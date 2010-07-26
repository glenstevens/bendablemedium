using System;
using System.Collections.Generic;
using System.Text;

namespace KernelUtilities
{
    ///summary>
    /// Virtual Messages
    /// </summary>
    public enum WMessages : int
    {
        MouseMove = 0x200,
        LeftMouseButtonDown = 0x201, //Left mousebutton down
        LeftMouseButtonUp = 0x202,  //Left mousebutton up
        LeftMouseButtonDoubleClick = 0x203, //Left mousebutton doubleclick
        RightMouseButtonDown = 0x204, //Right mousebutton down
        RightMouseButtonUp = 0x205,   //Right mousebutton up
        RightMouseButtonDoubleClick = 0x206, //Right mousebutton doubleclick
        MiddleMouseButtonDown = 0x207, //Middle mousebutton down
        MiddleMouseButtonUp = 0x208,   //Middle mousebutton up
        MiddleMouseButtonDoubleClick = 0x20A, //Middle mousebutton doubleclick
        ExtendedMouseButtonDown = 0x20B, //Extended mousebutton down
        ExtendedMouseButtonUp = 0x20C,   //Extended mousebutton up
        ExtendedButton1 = 0x1,  // First extended mousebutton
        ExtendedButton2 = 0x2,  // Second extended mousebutton
        KeyDown = 0x100,  //Key down
        KeyUp = 0x101,   //Key up
        SysKeyDown = 0x104,
        SysKeyUp = 0x105,
    }
}
