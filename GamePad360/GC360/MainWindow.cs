using System.ComponentModel;
using System.Windows.Forms;
using GameControllerLogic;
using System;
using System.Threading;
using Win32Utilities;

namespace GC360
{
    public partial class MainWindow : Form
    {
        const string SettingsFileName = "settings.xml";
        const int ControllerThreadTimeoutMs = 20;
        const int WindowThreadTimeoutMs = 5000;
        GameController _gc;
        BackgroundWorker _controllerWorker;
        BackgroundWorker _windowWorker;
        WoWLogic _logic;
        //SettingsCollection _settingsCollection;
        //Settings _settings;

        volatile bool _done;

        public MainWindow()
        {
            InitializeComponent();
            LoadWindowSettings();
        }

        void LoadWindowSettings()
        {
            Top = Properties.Settings.Default.MainWindowTop;
            Left = Properties.Settings.Default.MainWindowLeft;
            WindowState = Properties.Settings.Default.MainWindowState;
        }

        private void MainWindow_Load(object sender, System.EventArgs e)
        {
            try
            {
                //CreateSettingsFile();
                //LoadSettings();
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

        void InitializeGameController()
        {
            _gc = new GameController();
        }

        void InitializeLogic()
        {
            _logic = new WoWLogic();
            _logic.AttachEvents(_gc);
            //_logic.InitializeSettings(_settings);
        }

        void StartThreads()
        {
            _controllerWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _controllerWorker.DoWork += ControllerPollThread;
            _controllerWorker.RunWorkerAsync();
            _windowWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
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

                IntPtr activeWindowHandle = WindowUtilities.GetActiveWindow();

                BeginInvoke(new MethodInvoker(delegate()
                {
                    _logic.WindowHandle = activeWindowHandle;
                }));

                // sleep for a bit
                Thread.Sleep(WindowThreadTimeoutMs);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _done = true;
        }
    }
}
