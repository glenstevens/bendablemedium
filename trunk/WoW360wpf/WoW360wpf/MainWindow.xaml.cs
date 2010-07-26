using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using GameControllerLogic;
using SettingsLogic;

namespace WoW360wpf
{
    /// <summary>
    /// Displays the main window and handles basic UI and attaching logic pieces.
    /// </summary>
    public partial class MainWindow
    {
        #region Constants
#if DEBUG
        const string WindowTitle = "Untitled - Notepad";
#else
        const string WindowTitle = "World of Warcraft";
#endif
        const string SettingsFileName = "settings.xml";
        const int ControllerThreadTimeoutMs = 20;
        const int WindowThreadTimeoutMs = 5000;
        #endregion Constants

        #region Members
        GameController _gc;
        BackgroundWorker _controllerWorker;
        BackgroundWorker _windowWorker;
        WoWLogic _logic;
        SettingsCollection _settingsCollection;
        Settings _settings;

        volatile bool _done;
        #endregion Members

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                LoadWindowSettings();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        void LoadWindowSettings()
        {
            Top = Properties.Settings.Default.MainWindowTop;
            Left = Properties.Settings.Default.MainWindowLeft;
            WindowState = Properties.Settings.Default.MainWindowState;
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateSettingsFile();
                LoadSettings();
                InitializeGameController();
                InitializeLogic();
                StartThreads();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        #region CreateSettingsFile

        static void CreateSettingsFile()
        {
            SettingsCollection sc = new SettingsCollection {CreateNewSettings("Default")};
            sc.SaveSettings(SettingsFileName);
        }

        static Settings CreateNewSettings(string displayName)
        {
            Settings s = new Settings
            {
                DisplayName = displayName,
                Options = new OptionCollection {
                        new Option { Name = OptionNames.MoveStick, Value = GameController.ThumbSticks.Left.ToString(), ValueType = ValueTypes.ThumbSticks },
                        new Option { Name = OptionNames.LookStick, Value = GameController.ThumbSticks.Right.ToString(), ValueType = ValueTypes.ThumbSticks },
                        new Option { Name = OptionNames.MouseLookSensitivity, Value = "50.0", ValueType = ValueTypes.Double },
                        new Option { Name = OptionNames.CursorMoveSensitivity, Value = "50.0", ValueType = ValueTypes.Double },
                        new Option { Name = OptionNames.CursorCenterOffsetX, Value = "0.0", ValueType = ValueTypes.Double },
                        new Option { Name = OptionNames.CursorCenterOffsetY, Value = "0.0", ValueType = ValueTypes.Double }
                    },
                KeyBindings = new KeyBindingCollection {
                        // Buttons while no trigger is pressed
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.A,
                            KeyBoardKey= System.Windows.Input.Key.D1,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.B,
                            KeyBoardKey= System.Windows.Input.Key.D2,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.X,
                            KeyBoardKey= System.Windows.Input.Key.D3,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.Y,
                            KeyBoardKey= System.Windows.Input.Key.D4,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.Back,
                            KeyBoardKey= System.Windows.Input.Key.Escape,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.Start,
                            KeyBoardKey= System.Windows.Input.Key.M,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.LeftShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Tab,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.RightShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Attn,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.LeftStick,
                            KeyBoardKey= System.Windows.Input.Key.NumLock,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.RightStick,
                            KeyBoardKey= System.Windows.Input.Key.Space,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.DPadUp,
                            KeyBoardKey= System.Windows.Input.Key.LeftShift,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.DPadLeft,
                            KeyBoardKey= System.Windows.Input.Key.LeftAlt,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.DPadDown,
                            KeyBoardKey= System.Windows.Input.Key.LeftCtrl,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.None,
                            Button= GameController.Buttons.DPadRight,
                            KeyBoardKey= System.Windows.Input.Key.B,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        // Buttons while LeftTrigger is pressed
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.A,
                            KeyBoardKey= System.Windows.Input.Key.D5,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.B,
                            KeyBoardKey= System.Windows.Input.Key.D6,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.X,
                            KeyBoardKey= System.Windows.Input.Key.D7,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.Y,
                            KeyBoardKey= System.Windows.Input.Key.D8,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.Back,
                            KeyBoardKey= System.Windows.Input.Key.Escape,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.Start,
                            KeyBoardKey= System.Windows.Input.Key.M,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.LeftShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Tab,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.RightShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Attn,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.LeftStick,
                            KeyBoardKey= System.Windows.Input.Key.NumLock,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.RightStick,
                            KeyBoardKey= System.Windows.Input.Key.Space,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.DPadUp,
                            KeyBoardKey= System.Windows.Input.Key.LeftShift,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.DPadLeft,
                            KeyBoardKey= System.Windows.Input.Key.LeftAlt,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.DPadDown,
                            KeyBoardKey= System.Windows.Input.Key.LeftCtrl,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Left,
                            Button= GameController.Buttons.DPadRight,
                            KeyBoardKey= System.Windows.Input.Key.B,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        // Buttons while RightTrigger is pressed
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.A,
                            KeyBoardKey= System.Windows.Input.Key.D9,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.B,
                            KeyBoardKey= System.Windows.Input.Key.D0,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.X,
                            KeyBoardKey= System.Windows.Input.Key.Subtract,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.Y,
                            KeyBoardKey= System.Windows.Input.Key.Add,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.Back,
                            KeyBoardKey= System.Windows.Input.Key.Escape,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.Start,
                            KeyBoardKey= System.Windows.Input.Key.M,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.LeftShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Tab,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.RightShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Attn,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.LeftStick,
                            KeyBoardKey= System.Windows.Input.Key.NumLock,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.RightStick,
                            KeyBoardKey= System.Windows.Input.Key.Space,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.DPadUp,
                            KeyBoardKey= System.Windows.Input.Key.LeftShift,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.DPadLeft,
                            KeyBoardKey= System.Windows.Input.Key.LeftAlt,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.DPadDown,
                            KeyBoardKey= System.Windows.Input.Key.LeftCtrl,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Right,
                            Button= GameController.Buttons.DPadRight,
                            KeyBoardKey= System.Windows.Input.Key.B,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        // Buttons while Both Triggers are pressed
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.A,
                            KeyBoardKey= System.Windows.Input.Key.F2,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.B,
                            KeyBoardKey= System.Windows.Input.Key.F3,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.X,
                            KeyBoardKey= System.Windows.Input.Key.F4,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.Y,
                            KeyBoardKey= System.Windows.Input.Key.F5,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.Back,
                            KeyBoardKey= System.Windows.Input.Key.Escape,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.Start,
                            KeyBoardKey= System.Windows.Input.Key.M,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.LeftShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Tab,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.RightShoulder,
                            KeyBoardKey= System.Windows.Input.Key.Attn,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.LeftStick,
                            KeyBoardKey= System.Windows.Input.Key.NumLock,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.RightStick,
                            KeyBoardKey= System.Windows.Input.Key.Space,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.DPadUp,
                            KeyBoardKey= System.Windows.Input.Key.Up,
                            Modifier= System.Windows.Input.ModifierKeys.Shift
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.DPadLeft,
                            KeyBoardKey= System.Windows.Input.Key.LeftAlt,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.DPadDown,
                            KeyBoardKey= System.Windows.Input.Key.Down,
                            Modifier= System.Windows.Input.ModifierKeys.Shift
                        },
                        new KeyBinding{ 
                            TriggerModifier= GameController.Triggers.Both,
                            Button= GameController.Buttons.DPadRight,
                            KeyBoardKey= System.Windows.Input.Key.B,
                            Modifier= System.Windows.Input.ModifierKeys.None
                        }
                    }
            };
            return s;
        }
        #endregion CreateSettingsFile

        void LoadSettings()
        {
            _settingsCollection = SettingsCollection.LoadSettings(SettingsFileName);
            _settings = _settingsCollection.GetSettings(Properties.Settings.Default.MostRecentSetting);
            SettingsProfile.ItemsSource = _settingsCollection.GetSettingsNamesList();
            SettingsProfile.SelectedValue = _settings.DisplayName;
            ucOptions.LoadSettings(_settings);
            ucKeyBinding.LoadSettings(_settings);
        }

        void InitializeGameController()
        {
            _gc = new GameController();
        }

        void InitializeLogic()
        {
            _logic = new WoWLogic();
            _logic.AttachEvents(_gc);
            _logic.InitializeSettings(_settings);
            ucDebug.AttachEvents(_gc);
        }

        void StartThreads()
        {
            _controllerWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
            _controllerWorker.DoWork += ControllerPollThread;
            _controllerWorker.RunWorkerAsync();
            _windowWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
            _windowWorker.DoWork += WindowPollThread;
            _windowWorker.RunWorkerAsync();
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

                _gc.UpdateControllerState(GameController.ControllerNumber.One);

                // sleep for a bit
                Thread.Sleep(ControllerThreadTimeoutMs);
            }
        }

        void WindowPollThread(object sender, DoWorkEventArgs args)
        {
            while (!_done)
            {
                if (_windowWorker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
  
                //UnsafeWin32Calls.EnsureWindowValid(WindowTitle);
                string activeWindowTitle = UnsafeWin32Calls.GetActiveWindowTitle();
                CurrentActiveDisplay.Text = activeWindowTitle;

                //this.Dispatcher.BeginInvoke((UpdateUIDelegate)delegate(string text)
                //        {
                //            CurrentActiveDisplay.Text = text;
                //        }
                //    , System.Windows.Threading.DispatcherPriority.Normal, activeWindowTitle);

                // sleep for a bit
                Thread.Sleep(WindowThreadTimeoutMs);
            }
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveWindowSettings();
            _done = true;
            Application.Current.Shutdown();
        }

        void SaveWindowSettings()
        {
            Properties.Settings.Default.MainWindowTop = Top;
            Properties.Settings.Default.MainWindowLeft = Left;
            Properties.Settings.Default.MainWindowState = WindowState;
            Properties.Settings.Default.Save();
        }

        private void SettingsProfile_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.MostRecentSetting = e.ToString();
        }
    }
}
