
namespace GamePadUtilities
{
    public class TriggerStateEventArgs : GamePadEventArgs
    {
        public readonly GamePadTriggers Trigger;
        public readonly float TriggerValue;

        public TriggerStateEventArgs(GamePadNumber controllerNum, GamePadTriggers trigger, float triggerValue)
            : base(controllerNum)
        {
            this.Trigger = trigger;
            this.TriggerValue = triggerValue;
        }
    }
}
