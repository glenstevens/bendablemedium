using System;

namespace KernelUtilities
{
    /// <summary>
    /// Represents whether either the left or right modifier of a specific type is being pressed
    /// </summary>
    /// <remarks>
    /// Because this does not represent a specific key, it should not be used for sending keypresses
    /// </remarks>
    [Flags]
    public enum ModifierKeys
    {
        None = 0x0,
        Alt = 0x1,
        Control = 0x2,
        Shift = 0x4,
        //Windows = 0x8,
        All = Alt | Control | Shift //| Windows
    }
}
