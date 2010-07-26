
namespace GamePadUtilities
{
    public class ThumbStickStateEventArgs : GamePadEventArgs
    {
        public readonly GamePadThumbSticks ThumbStick;
        public readonly float XValue;
        public readonly float YValue;

        public ThumbStickStateEventArgs(GamePadNumber controllerNum, GamePadThumbSticks thumbStick, float xValue, float yValue)
            : base(controllerNum)
        {
            this.ThumbStick = thumbStick;
            this.XValue = xValue;
            this.YValue = yValue;
        }
    }
}
