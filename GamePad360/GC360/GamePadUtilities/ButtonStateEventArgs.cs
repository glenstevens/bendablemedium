
namespace GamePadUtilities
{
    /// <summary>
    /// Event Args for handling Game Controller button state for button events
    /// </summary>
    public class ButtonStateEventArgs : GamePadEventArgs
    {
        public readonly GamePadButtons Button;
        public readonly bool Pressed;

        public ButtonStateEventArgs(GamePadNumber controllerNum, GamePadButtons button, bool pressed)
            : base(controllerNum)
        {
            this.Button = button;
            this.Pressed = pressed;
        }
    }
}
