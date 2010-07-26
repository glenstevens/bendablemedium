using System;

namespace GamePadUtilities
{
    public class GamePadEventArgs : EventArgs
    {
        public readonly GamePadNumber ControllerNum;

        public GamePadEventArgs(GamePadNumber controllerNum)
        {
            this.ControllerNum = controllerNum;
        }
    }
}
