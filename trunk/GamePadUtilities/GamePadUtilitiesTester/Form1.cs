using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using GamePadUtilities;

namespace GamePadUtilitiesTester
{
    public partial class Form1 : Form
    {
        private const int ControllerThreadTimeoutMs = 20;
        private GamePads _gc;
        private BackgroundWorker _controllerWorker;
        private volatile bool _done;
        public bool ShowOutput = true;
        GamePadNumber _currentController = GamePadNumber.One;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeGameController();
                AttachEvents(_gc);
                StartThreads();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        void InitializeGameController()
        {
            _gc = new GamePads();
        }

        public void AttachEvents(GamePads gc)
        {
            gc.ButtonStateChanged += new GamePads.ButtonStateChangedHandler(gc_ButtonStateChanged);
            gc.ThumbStickStateUpdated += new GamePads.ThumbStickStateUpdatedHandler(gc_ThumbStickStateUpdated);
            gc.TriggerStateUpdated += new GamePads.TriggerStateUpdatedHandler(gc_TriggerStateUpdated);
            gc.ControllerDisconnected += new GamePads.ControllerDisconnectedHandler(gc_ControllerDisconnected);
        }

        #region Events
        void gc_ControllerDisconnected(object sender, GamePadEventArgs args)
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
            if (ShowOutput && _currentController == args.ControllerNum)
            {
                string val = args.TriggerValue.ToString();
                switch (args.Trigger)
                {
                    case GamePadTriggers.Left:
                        SetLeftTriggerValue(val);
                        break;
                    case GamePadTriggers.Right:
                        SetRightTriggerValue(val);
                        break;
                }
            }
        }

        void gc_ThumbStickStateUpdated(object sender, ThumbStickStateEventArgs args)
        {
            if (ShowOutput && _currentController == args.ControllerNum)
            {
                string coords = string.Format("x:{0}, y:{1}", args.XValue, args.YValue);
                switch (args.ThumbStick)
                {
                    case GamePadThumbSticks.Left:
                        SetLeftStickValue(coords);
                        break;
                    case GamePadThumbSticks.Right:
                        SetRightStickValue(coords);
                        break;
                }
            }
        }

        void gc_ButtonStateChanged(object sender, ButtonStateEventArgs args)
        {
            if (ShowOutput && _currentController == args.ControllerNum)
            {
                string key = " down";
                if (!args.Pressed) key = " up";
                AddButtonOutput(args.Button + key);
            }
        }
        #endregion Events

        #region Invoke Methods
        // Because the events are called from a different process we must ask the dispatcher 
        // to change the UI for us.
        public void AddButtonOutput(string text)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                ButtonOutputListBox.Items.Insert(0, text);
                while (ButtonOutputListBox.Items.Count > 100) ButtonOutputListBox.Items.RemoveAt(100);
            }));
        }

        public void ClearButtonOutput()
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                ButtonOutputListBox.Items.Clear();
            }));
        }

        public void SetLeftStickValue(string value)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                txtLeftThumbstick.Text = value;
            }));
        }

        public void SetRightStickValue(string value)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                txtRightThumbstick.Text = value;
            }));
        }

        public void SetRightTriggerValue(string value)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                txtRightTrigger.Text = value;
            }));
        }

        public void SetLeftTriggerValue(string value)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                txtLeftTrigger.Text = value;
            }));
        }
        #endregion Invoke Methods

        void StartThreads()
        {
            _controllerWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _controllerWorker.DoWork += ControllerPollThread;
            _controllerWorker.RunWorkerAsync();
        }

        void ControllerPollThread(object sender, DoWorkEventArgs args)
        {
            while (!_done)
            {
                if (_controllerWorker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }

                _gc.UpdateControllerState(GamePadNumber.Four);
                _gc.UpdateControllerState(GamePadNumber.Three);
                _gc.UpdateControllerState(GamePadNumber.Two);
                _gc.UpdateControllerState(GamePadNumber.One);

                // sleep for a bit
                Thread.Sleep(ControllerThreadTimeoutMs);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _done = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _currentController = GamePadNumber.One;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _currentController = GamePadNumber.Two;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _currentController = GamePadNumber.Three;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            _currentController = GamePadNumber.Four;
        }
    }
}
