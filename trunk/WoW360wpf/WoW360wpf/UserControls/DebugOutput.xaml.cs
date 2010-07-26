using System;
using System.Windows.Controls;
using GameControllerLogic;

namespace WoW360wpf.UserControls
{
    /// <summary>
    /// Interaction logic for DebugOutput.xaml
    /// </summary>
    public partial class DebugOutput : UserControl
    {
        public bool ShowOutput = true;

        public DebugOutput()
        {
            InitializeComponent();
        }

        public void AttachEvents(GameController gc)
        {
            gc.ButtonStateChanged += new GameController.ButtonStateChangedHandler(gc_ButtonStateChanged);
            gc.ThumbStickStateUpdated += new GameController.ThumbStickStateUpdatedHandler(gc_ThumbStickStateUpdated);
            gc.TriggerStateUpdated += new GameController.TriggerStateUpdatedHandler(gc_TriggerStateUpdated);
            gc.ControllerDisconnected += new GameController.ControllerDisconnectedHandler(gc_ControllerDisconnected);
        }

        #region Events
        void gc_ControllerDisconnected(object sender, GameControllerEventArgs args)
        {
            if (ShowOutput)
            {
                SetLeftTriggerValue(string.Empty);
                SetRightTriggerValue(string.Empty);
                SetLeftStickValue(string.Empty);
                SetRightStickValue(string.Empty);
                ClearButtonOutput();
            }
        }

        void gc_TriggerStateUpdated(object sender, TriggerStateEventArgs args)
        {
            if (ShowOutput)
            {
                string val = args.TriggerValue.ToString();
                switch (args.Trigger)
                {
                    case GameController.Triggers.Left:
                        SetLeftTriggerValue(val);
                        break;
                    case GameController.Triggers.Right:
                        SetRightTriggerValue(val);
                        break;
                }
            }
        }

        void gc_ThumbStickStateUpdated(object sender, ThumbStickStateEventArgs args)
        {
            if (ShowOutput)
            {
                string coords = string.Format("x:{0}, y:{1}", args.XValue, args.YValue);
                switch (args.ThumbStick)
                {
                    case GameController.ThumbSticks.Left:
                        SetLeftStickValue(coords);
                        break;
                    case GameController.ThumbSticks.Right:
                        SetRightStickValue(coords);
                        break;
                } 
            }
        }

        void gc_ButtonStateChanged(object sender, ButtonStateEventArgs args)
        {
            if (ShowOutput)
            {
                string key = " down";
                if (args.Pressed) key = " up";
                AddButtonOutput(args.Button + key); 
            }
        }
        #endregion Events

        #region Invoke Methods
        // Because the events are called from a different process we must ask the dispatcher 
        // to change the UI for us.
        public void AddButtonOutput(string text)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                ButtonOutputListBox.Items.Insert(0, text);
                while (ButtonOutputListBox.Items.Count > 100) ButtonOutputListBox.Items.RemoveAt(100);
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        public void ClearButtonOutput()
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                ButtonOutputListBox.Items.Clear();
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        public void SetLeftStickValue(string value)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                LeftStickValue.Text = value;
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        public void SetRightStickValue(string value)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                RightStickValue.Text = value;
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        public void SetRightTriggerValue(string value)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                RightTriggerValue.Text = value;
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        public void SetLeftTriggerValue(string value)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                LeftTriggerValue.Text = value;
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }
        #endregion Invoke Methods
    }
}
