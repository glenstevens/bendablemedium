using System;

namespace GamePadUtilities
{
    /// <summary>
    /// The controller analog thumbsticks available
    /// </summary>
    /// <remarks>Abstracts the XNA framework away from applications using this class</remarks>
    [Flags]
    public enum GamePadThumbSticks : int
    {
        None = 0,
        Left = 1,
        Right = 2,
        Both = Left | Right
    }
}
