using System;
using GameControllerLogic;
//using SettingsLogic;
using Win32Utilities;
using System.Windows.Forms;

namespace GC360
{
    /// <summary>
    /// The logic between the controller and the game
    /// </summary>
    public class WoWLogic
    {
        #region Enums
        [Flags]
        enum Movements : int
        {
            None = 0,
            LeftStrafe = 1,
            Forward = 2,
            RightStrafe = 4,
            Left = 8,
            Backward = 16,
            Right = 32,
            Up = 64,
            Down = 128,
            All = LeftStrafe | Forward | RightStrafe | Left | Backward | Right | Up | Down
        }
        #endregion Enums

        GameController.ThumbSticks _moveStick = GameController.ThumbSticks.Left;
        //GameController.ThumbSticks _lookStick = GameController.ThumbSticks.Right;
        double _mouseLookSensitivity = 1.0;
        double _cursorMoveSensitivity = 1.0;
        //double _cursorCenterOffsetX = 0.0;
        //double _cursorCenterOffsetY = 0.0;
        //SettingsLogic.Settings _settings = null;
        bool _paused = false;
        bool _cursorMode = true;
        GameController.Triggers _triggersPressed = GameController.Triggers.None;
        Movements _moveFlag = Movements.None;

        public IntPtr WindowHandle { get; set; }

        public void AttachEvents(GameController gc)
        {
            gc.ButtonStateChanged += new GameController.ButtonStateChangedHandler(gc_ButtonStateChanged);
            gc.ThumbStickStateUpdated += new GameController.ThumbStickStateUpdatedHandler(gc_ThumbStickStateUpdated);
            gc.TriggerStateUpdated += new GameController.TriggerStateUpdatedHandler(gc_TriggerStateUpdated);
            gc.ControllerDisconnected += new GameController.ControllerDisconnectedHandler(gc_ControllerDisconnected);
            gc.ControllerConnected += new GameController.ControllerConnectedHandler(gc_ControllerConnected);
        }

        //public void InitializeSettings(SettingsLogic.Settings settings)
        //{
        //    try
        //    {
        //        _settings = settings;
        //        // It's possible the settings file doesn't include the options or there is a
        //        // conversion exception
        //        _moveStick = _settings.Options.GetOption(OptionNames.MoveStick).GetValueAsThumbSticks();
        //        _lookStick = _settings.Options.GetOption(OptionNames.LookStick).GetValueAsThumbSticks();
        //        _mouseLookSensitivity = _settings.Options.GetOption(OptionNames.MouseLookSensitivity).GetValueAsDouble();
        //        _cursorMoveSensitivity = _settings.Options.GetOption(OptionNames.CursorMoveSensitivity).GetValueAsDouble();
        //        _cursorCenterOffsetX = _settings.Options.GetOption(OptionNames.CursorCenterOffsetX).GetValueAsDouble();
        //        _cursorCenterOffsetY = _settings.Options.GetOption(OptionNames.CursorCenterOffsetY).GetValueAsDouble();
        //    }
        //    finally { }
        //}

        void MoveKeyPress(Movements btn, bool press)
        {
            // Handling each if separately allows sending All for key up when connection lost
            // For each key, check if it's requested and that the state isn't already set
            if (EnumHas(btn, Movements.Backward) && EnumHas(_moveFlag, Movements.Backward) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.S, press);
            }
            if (EnumHas(btn, Movements.Left) && EnumHas(_moveFlag, Movements.Left) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.A, press);
            }
            if (EnumHas(btn, Movements.Right) && EnumHas(_moveFlag, Movements.Right) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.D, press);
            }
            if (EnumHas(btn, Movements.LeftStrafe) && EnumHas(_moveFlag, Movements.LeftStrafe) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.Q, press);
            }
            if (EnumHas(btn, Movements.RightStrafe) && EnumHas(_moveFlag, Movements.RightStrafe) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.E, press);
            }
            if (EnumHas(btn, Movements.Forward) && EnumHas(_moveFlag, Movements.Forward) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.W, press);
            }
            if (EnumHas(btn, Movements.Up) && EnumHas(_moveFlag, Movements.Up) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.Space, press);
            }
            if (EnumHas(btn, Movements.Down) && EnumHas(_moveFlag, Movements.Down) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, (IntPtr)Keys.X, press);
            }

            // Update the flags
            if (press)
            {
                EnumAdd(_moveFlag, btn);
            }
            else
            {
                EnumRemove(_moveFlag, btn);
            }
        }

        protected void OnControllerConnected(GameController.ControllerNumber cNum)
        {
            _paused = false;
        }

        protected void OnControllerDisconnected(GameController.ControllerNumber cNum)
        {
            // release all movement buttons
            MoveKeyPress(Movements.All, false);
            _moveFlag = Movements.None;
            _triggersPressed = GameController.Triggers.None;
            _paused = true;
        }

        protected void OnTriggerStateUpdated(GameController.ControllerNumber cNum, GameController.Triggers trigger, float triggerValue)
        {
            if (!_paused)
            {
                if (triggerValue > 0.5f)
                {
                    EnumAdd(_triggersPressed, trigger);
                    //EnumAdd<GameController.Triggers>(_triggersPressed, trigger);
                }
                else
                {
                    EnumRemove(_triggersPressed, trigger);
                    //EnumRemove<GameController.Triggers>(_triggersPressed, trigger);
                }
            }
        }

        protected void OnThumbSticksUpdated(GameController.ControllerNumber cNum, GameController.ThumbSticks thumbstick, float x, float y)
        {
            if (!_paused)
            {
                if (thumbstick == _moveStick)
                {
                    // Move the character
                    MoveKeyPress(Movements.RightStrafe, (x > 0.5f));
                    MoveKeyPress(Movements.LeftStrafe, (x < -0.5f));
                    MoveKeyPress(Movements.Forward, (y > 0.5f));
                    MoveKeyPress(Movements.Backward, (y < -0.5f));
                }
                else
                {
                    // Check deadzone
                    if (Math.Abs(x) < 0.1) x = 0;
                    if (Math.Abs(y) < 0.1) y = 0;

                    // NOTE: need to update outside this callback, update every frame
                    // BUG: cannot move in diagonals
                    // BUG: in cursorMode the screen slowly creeps upward
                    // BUG: none of the enum controlled input work (triggers, left stick)
                    // Update the position of the mouse, don't send mouse moves until
                    // a click is made (in cursor mode), or switch back to look mode
                    System.Drawing.Point pos = new System.Drawing.Point();
                    pos.X = Cursor.Position.X + (int)(x * (_cursorMode ? _cursorMoveSensitivity : _mouseLookSensitivity));
                    pos.Y = Cursor.Position.Y - (int)(y * (_cursorMode ? _cursorMoveSensitivity : _mouseLookSensitivity));

                    //Cursor.Position = new System.Drawing.Point((int)x + Cursor.Position.X, -(int)y + Cursor.Position.Y);
                    Cursor.Position = pos;

                    //if (_cursorMode)
                    //{
                    //    MouseUtilities.SendMouseMove(WindowHandle, Cursor.Position.X, Cursor.Position.Y);
                    //}
                }
            }
        }

        protected void OnButtonStateChanged(GameController.ControllerNumber cNum, GameController.Buttons button, bool pressed)
        {
            if (!_paused)
            {
                // Take into account special cases:
                // 1. Thumbsticks are mouse clicks in cursor mode
                if (_cursorMode && button == GameController.Buttons.LeftStick)
                {
                    MouseUtilities.SendMouseClick(WindowHandle, Cursor.Position.X, Cursor.Position.Y, MouseUtilities.MouseButton.Left, pressed);
                    return;
                }
                if (_cursorMode && button == GameController.Buttons.RightStick)
                {
                    MouseUtilities.SendMouseClick(WindowHandle, Cursor.Position.X, Cursor.Position.Y, MouseUtilities.MouseButton.Right, pressed);
                    return;
                }

                IntPtr key = GetKeyBinding(_triggersPressed, button);

                // 2. Swap look/cursor mode
                if (pressed && key == (IntPtr)Keys.Attn)
                {
                    _cursorMode = !_cursorMode;
                    MouseUtilities.SendMouseClick(WindowHandle, Cursor.Position.X, Cursor.Position.Y, MouseUtilities.MouseButton.Right, _cursorMode);
                    return;
                }

                // Handle normal key clicks
                KeyboardUtilities.PostKeyClick(WindowHandle, key, pressed);
            }
        }

        #region GetKeyBinding
        private IntPtr GetKeyBinding(GameController.Triggers triggersPressed, GameController.Buttons button)
        {
            switch (triggersPressed)
            {
                case GameController.Triggers.None:
                    switch (button)
                    {
                        case GameController.Buttons.A:
                            return (IntPtr)Keys.D1;
                        case GameController.Buttons.B:
                            return (IntPtr)Keys.D2;
                        case GameController.Buttons.X:
                            return (IntPtr)Keys.D3;
                        case GameController.Buttons.Y:
                            return (IntPtr)Keys.D4;
                        case GameController.Buttons.Back:
                            return (IntPtr)Keys.Escape;
                        case GameController.Buttons.Start:
                            return (IntPtr)Keys.M;
                        case GameController.Buttons.LeftShoulder:
                            return (IntPtr)Keys.Tab;
                        case GameController.Buttons.RightShoulder:
                            return (IntPtr)Keys.Attn;
                        case GameController.Buttons.LeftStick:
                            return (IntPtr)Keys.Oemtilde;
                        case GameController.Buttons.RightStick:
                            return (IntPtr)Keys.Space;
                        case GameController.Buttons.DPadUp:
                            return (IntPtr)Keys.ShiftKey;
                        case GameController.Buttons.DPadLeft:
                            return (IntPtr)Keys.Menu;
                        case GameController.Buttons.DPadDown:
                            return (IntPtr)Keys.ControlKey;
                        case GameController.Buttons.DPadRight:
                            return (IntPtr)Keys.B;
                    }
                    break;
                case GameController.Triggers.Left:
                    switch (button)
                    {
                        case GameController.Buttons.A:
                            return (IntPtr)Keys.D5;
                        case GameController.Buttons.B:
                            return (IntPtr)Keys.D6;
                        case GameController.Buttons.X:
                            return (IntPtr)Keys.D7;
                        case GameController.Buttons.Y:
                            return (IntPtr)Keys.D8;
                        case GameController.Buttons.Back:
                            return (IntPtr)Keys.Escape;
                        case GameController.Buttons.Start:
                            return (IntPtr)Keys.M;
                        case GameController.Buttons.LeftShoulder:
                            return (IntPtr)Keys.Tab;
                        case GameController.Buttons.RightShoulder:
                            return (IntPtr)Keys.Attn;
                        case GameController.Buttons.LeftStick:
                            return (IntPtr)Keys.Oemtilde;
                        case GameController.Buttons.RightStick:
                            return (IntPtr)Keys.Space;
                        case GameController.Buttons.DPadUp:
                            return (IntPtr)Keys.ShiftKey;
                        case GameController.Buttons.DPadLeft:
                            return (IntPtr)Keys.Menu;
                        case GameController.Buttons.DPadDown:
                            return (IntPtr)Keys.ControlKey;
                        case GameController.Buttons.DPadRight:
                            return (IntPtr)Keys.B;
                    }
                    break;
                case GameController.Triggers.Right:
                    switch (button)
                    {
                        case GameController.Buttons.A:
                            return (IntPtr)Keys.D9;
                        case GameController.Buttons.B:
                            return (IntPtr)Keys.D0;
                        case GameController.Buttons.X:
                            return (IntPtr)Keys.Subtract;
                        case GameController.Buttons.Y:
                            return (IntPtr)Keys.Add;
                        case GameController.Buttons.Back:
                            return (IntPtr)Keys.Escape;
                        case GameController.Buttons.Start:
                            return (IntPtr)Keys.M;
                        case GameController.Buttons.LeftShoulder:
                            return (IntPtr)Keys.Tab;
                        case GameController.Buttons.RightShoulder:
                            return (IntPtr)Keys.Attn;
                        case GameController.Buttons.LeftStick:
                            return (IntPtr)Keys.Oemtilde;
                        case GameController.Buttons.RightStick:
                            return (IntPtr)Keys.Space;
                        case GameController.Buttons.DPadUp:
                            return (IntPtr)Keys.ShiftKey;
                        case GameController.Buttons.DPadLeft:
                            return (IntPtr)Keys.Menu;
                        case GameController.Buttons.DPadDown:
                            return (IntPtr)Keys.ControlKey;
                        case GameController.Buttons.DPadRight:
                            return (IntPtr)Keys.B;
                    }
                    break;
                case GameController.Triggers.Both:
                    switch (button)
                    {
                        case GameController.Buttons.A:
                            return (IntPtr)Keys.F2;
                        case GameController.Buttons.B:
                            return (IntPtr)Keys.F3;
                        case GameController.Buttons.X:
                            return (IntPtr)Keys.F4;
                        case GameController.Buttons.Y:
                            return (IntPtr)Keys.F5;
                        case GameController.Buttons.Back:
                            return (IntPtr)Keys.Escape;
                        case GameController.Buttons.Start:
                            return (IntPtr)Keys.M;
                        case GameController.Buttons.LeftShoulder:
                            return (IntPtr)Keys.Tab;
                        case GameController.Buttons.RightShoulder:
                            return (IntPtr)Keys.Attn;
                        case GameController.Buttons.LeftStick:
                            return (IntPtr)Keys.Oemtilde;
                        case GameController.Buttons.RightStick:
                            return (IntPtr)Keys.Space;
                        case GameController.Buttons.DPadUp:
                            return (IntPtr)Keys.ShiftKey;
                        case GameController.Buttons.DPadLeft:
                            return (IntPtr)Keys.Menu;
                        case GameController.Buttons.DPadDown:
                            return (IntPtr)Keys.ControlKey;
                        case GameController.Buttons.DPadRight:
                            return (IntPtr)Keys.B;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
        #endregion GetKeyBinding

        #region Events
        void gc_ControllerConnected(object sender, GameControllerEventArgs args)
        {
            OnControllerConnected(args.ControllerNum);
        }

        void gc_ControllerDisconnected(object sender, GameControllerEventArgs args)
        {
            OnControllerDisconnected(args.ControllerNum);
        }

        void gc_TriggerStateUpdated(object sender, TriggerStateEventArgs args)
        {
            OnTriggerStateUpdated(args.ControllerNum, args.Trigger, args.TriggerValue);
        }

        void gc_ThumbStickStateUpdated(object sender, ThumbStickStateEventArgs args)
        {
            OnThumbSticksUpdated(args.ControllerNum, args.ThumbStick, args.XValue, args.YValue);
        }

        void gc_ButtonStateChanged(object sender, ButtonStateEventArgs args)
        {
            OnButtonStateChanged(args.ControllerNum, args.Button, args.Pressed);
        }
        #endregion Events

        static bool EnumHas(Movements flag, Movements value)
        {
            return (flag & value) == value;
        }

        static Movements EnumAdd(Movements flag, Movements value)
        {
            return flag | value;
        }

        static Movements EnumRemove(Movements flag, Movements value)
        {
            return flag & ~value;
        }

        static bool EnumHas(GameController.Triggers flag, GameController.Triggers value)
        {
            return (flag & value) == value;
        }

        static GameController.Triggers EnumAdd(GameController.Triggers flag, GameController.Triggers value)
        {
            return flag | value;
        }

        static GameController.Triggers EnumRemove(GameController.Triggers flag, GameController.Triggers value)
        {
            return flag & ~value;
        }

        static bool EnumHas<T>(T type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        //appends a value
        static T EnumAdd<T>(T type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not append value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }

        //completely removes the value
        static T EnumRemove<T>(T type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not remove value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
    }
}
