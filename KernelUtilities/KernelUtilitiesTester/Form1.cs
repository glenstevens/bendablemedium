using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using KernelUtilities;
using System.Drawing;

namespace KernelUtilitiesTester
{
    public partial class Form1 : Form
    {
        const int WindowThreadTimeoutMs = 500;
        BackgroundWorker _windowWorker;
        volatile bool _done;
        volatile bool _testInput;
        static IntPtr WindowHandle { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                StartThreads();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        void StartThreads()
        {
            _windowWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _windowWorker.DoWork += WindowPollThread;
            _windowWorker.RunWorkerAsync();
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

                DoStuff();
                Thread.Sleep(WindowThreadTimeoutMs);
            }
        }

        DateTime dtTrigger = DateTime.Now;
        private void DoStuff()
        {
            WindowHandle = WindowUtilities.GetActiveWindow();
            string activeWindowTitle = WindowUtilities.GetActiveWindowTitle();

            BeginInvoke(new MethodInvoker(delegate()
            {
                lblActiveWindow.Text = activeWindowTitle;
            }));

            if (WindowHandle != IntPtr.Zero)
            {
                if (_testInput)
                {
                    if (DateTime.Now > dtTrigger)
                    {
                        //testKeyboard();
                        testMouse();
                        dtTrigger = DateTime.Now.AddSeconds(1.0);
                    }
                }
            }
        }

        bool shifted;
        private void testKeyboard()
        {
            shifted = !shifted;
            KeyboardUtilities.PostKeyClick(WindowHandle, VirtualKeyFromKey(Keys.Oemtilde), shifted);
        }

        int testNum = 0;
        private void testMouse()
        {
            Point newPoint;
            switch (testNum)
            {
                case 0:
                    // Center cursor
                    Point centerpoint = WindowUtilities.GetWindowCenterPoint(WindowHandle);
                    Cursor.Position = centerpoint;
                    //BeginInvoke(new MethodInvoker(delegate()
                    //{
                    //    txtHistory.Text += centerpoint.X + ", " + centerpoint.Y + Environment.NewLine;
                    //}));
                    break;
                case 1:
                    // Move up
                    newPoint = Cursor.Position;
                    newPoint.Y += 100;
                    Cursor.Position = newPoint;
                    break;
                case 2:
                    // Move right
                    newPoint = Cursor.Position;
                    newPoint.X += 150;
                    Cursor.Position = newPoint;
                    break;
                case 3:
                    // Move down
                    newPoint = Cursor.Position;
                    newPoint.Y += -200;
                    Cursor.Position = newPoint;
                    break;
                case 4:
                    // Move left
                    newPoint = Cursor.Position;
                    newPoint.X += -250;
                    Cursor.Position = newPoint;
                    break;
            }

            testNum++;
            if (testNum >= 5) testNum = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _done = true;
        }

        private void btnFindWindow_Click(object sender, EventArgs e)
        {
            if (txtWindowTitle.Text.Length > 0)
            {
                WindowHandle = WindowUtilities.FindWindow(txtWindowTitle.Text);
                if (WindowHandle != IntPtr.Zero)
                {
                    //MessageBox.Show("Found window");
                    lblTargetWindow.Text = WindowUtilities.GetWindowTitle(WindowHandle);
                }
                else
                {
                    //MessageBox.Show("Window not found");
                    lblTargetWindow.Text = "None";
                }
            }
        }

        private void btnSendText_Click(object sender, EventArgs e)
        {
            _testInput = !_testInput;
            if (_testInput) btnSendText.Text = "Stop intermittent test";
            else btnSendText.Text = "Start intermittent test";
        }

        private void KeyHelper(IntPtr hWnd, Keys key, Keys modifier, bool pressed)
        {
            // If the key is pressed, press the modifier first
            if (pressed && modifier != Keys.None)
            {
                KeyboardUtilities.PostKeyClick(WindowHandle, VirtualKeyModifierFromKey(modifier), pressed);
            }

            KeyboardUtilities.PostKeyClick(WindowHandle, VirtualKeyFromKey(key), pressed);

            // If the key is released, release the modifier last
            if (!pressed && modifier != Keys.None)
            {
                KeyboardUtilities.PostKeyClick(WindowHandle, VirtualKeyModifierFromKey(modifier), pressed);
            }
        }

        static IntPtr VirtualKeyFromKey(Keys key)
        {
            //return (IntPtr)(key & Keys.KeyCode);
            return (IntPtr)(key);
        }

        static IntPtr VirtualKeyModifierFromKey(Keys key)
        {
            return (IntPtr)(key & Keys.Modifiers);
        }

        private void txtKeybind_MouseClick(object sender, MouseEventArgs e)
        {
            // Clear box
            txtKeybind.Text = string.Empty;
        }

        private void txtKeybind_KeyDown(object sender, KeyEventArgs e)
        {
            string ctrl = e.Control ? "Ctrl " : "";
            string alt = e.Alt ? "Alt " : "";
            string shift = e.Shift ? "Shift " : "";
            string key = string.Empty;
            int keycode = (int)e.KeyCode;
            int keydata = (int)e.KeyData;
            int keyvalue = e.KeyValue;

            if (e.KeyCode != Keys.ShiftKey 
                && e.KeyCode != Keys.ControlKey 
                && e.KeyCode != Keys.Menu
                && e.KeyCode != Keys.None)
            {
                key = e.KeyCode.ToString();
            }

            txtKeybind.Text = ctrl + alt + shift + key + " = 0x" + keycode.ToString("x4");
            txtHistory.Text += txtKeybind.Text + ", // " + keydata.ToString("x4") + " / " + keyvalue.ToString("x4") + Environment.NewLine;
            e.Handled = true;
        }

        private void txtKeybind_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtKeybind_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
